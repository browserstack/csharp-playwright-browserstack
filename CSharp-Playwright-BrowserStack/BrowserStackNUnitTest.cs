using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;

using Microsoft.Playwright;
using Newtonsoft.Json;
using NUnit.Framework;
using BrowserStack;

namespace CSharpPlaywrightBrowserStack
{
    [TestFixture]
    public class BrowserStackNUnitTest
    {
        protected IBrowser browser;
        protected IPage page;
        protected string profile;
        protected string environment;
        private Local browserStackLocal;

        public BrowserStackNUnitTest(string profile, string environment)
        {
            this.profile = profile;
            this.environment = environment;
        }

        [SetUp]
        public async Task Init()
        {
            NameValueCollection? caps =
                ConfigurationManager.GetSection("capabilities/" + profile) as NameValueCollection;
            NameValueCollection? settings =
                ConfigurationManager.GetSection("environments/" + environment)
                    as NameValueCollection;

            NameValueCollection cc = (NameValueCollection) ConfigurationManager.AppSettings;

            Console.WriteLine("KAMAL " + settings + "-" + caps + "-" + profile + "-" + environment
                + cc["user"]);

            Dictionary<string, object> browserstackOptions = new Dictionary<string, object>
            {
                { "browserName", settings["browser"] }
            };

            foreach (string key in caps.AllKeys)
            {
                browserstackOptions.Add(key, caps[key]);
            }

            String username = Environment.GetEnvironmentVariable("BROWSERSTACK_USERNAME");
            if (username == null)
            {
                username = ConfigurationManager.AppSettings.Get("user");
            }

            String accesskey = Environment.GetEnvironmentVariable("BROWSERSTACK_ACCESS_KEY");
            if (accesskey == null)
            {
                accesskey = ConfigurationManager.AppSettings.Get("key");
            }

            browserstackOptions.Add("userName", username);
            browserstackOptions.Add("accessKey", accesskey);

            if (caps.Get("local").ToString() == "true")
            {
                browserStackLocal = new Local();
                List<KeyValuePair<string, string>> bsLocalArgs = new List<
                  KeyValuePair<string, string>
                >()
                {
                  new KeyValuePair<string, string>("key", accesskey)
                };
                browserStackLocal.start(bsLocalArgs);
            }
            string capsJson = JsonConvert.SerializeObject(browserstackOptions);
            string cdpUrl = "wss://cdp.browserstack.com/playwright?caps=" + Uri.EscapeDataString(capsJson);

            var playwright = await Playwright.CreateAsync();
            browser = await playwright.Chromium.ConnectAsync(cdpUrl);
            page = await browser.NewPageAsync();
        }

        [TearDown]
        public async Task Cleanup()
        {
            if (page != null)
            {
                page.CloseAsync();
            }
            if (browser != null)
            {
                browser.CloseAsync();
            }
            if (browserStackLocal != null)
            {
                browserStackLocal.stop();
            }
        }
    }
}
