using Microsoft.Playwright;
using Newtonsoft.Json;
using NUnit.Framework;
using BrowserStack;
using Newtonsoft.Json.Linq;

namespace CSharpPlaywrightBrowserStack
{
    [TestFixture]
    public class BrowserStackNUnitTest
    {
        protected IBrowser browser;
        protected IPage page;
        protected string profile;
        protected string environment;
        protected string configFile;

        private Local browserStackLocal;

        public BrowserStackNUnitTest(string profile, string environment, string configFile)
        {
            this.profile = profile;
            this.environment = environment;
            this.configFile = configFile;
        }

        [SetUp]
        public async Task Init()
        {
            // Get Configuration for correct profile
            string currentDirectory = Directory.GetCurrentDirectory();
            string path = Path.Combine(currentDirectory, configFile);
            //string path = Path.Combine("/Users/kamalpreet/Documents/fork-samples/csharp-playwright-browserstack/CSharp-Playwright-BrowserStack/config.json");
            JObject config = JObject.Parse(File.ReadAllText(path));
            if (config is null)
                throw new Exception("Configuration not found!");

            // Get Environment specific capabilities
            JObject capabilitiesJsonArr = config.GetValue("environments") as JObject;
            JObject capabilities = capabilitiesJsonArr.GetValue(environment) as JObject;

            // Get Common Capabilities
            JObject commonCapabilities = config.GetValue("capabilities") as JObject;

            // Merge Capabilities
            capabilities.Merge(commonCapabilities);

            // Get username and accesskey
            string? username = Environment.GetEnvironmentVariable("BROWSERSTACK_USERNAME");
            if (username is null)
                username = config.GetValue("user").ToString();

            string? accessKey = Environment.GetEnvironmentVariable("BROWSERSTACK_ACCESS_KEY");
            if (accessKey is null)
                accessKey = config.GetValue("key").ToString();

            capabilities["browserstack.user"] = username;
            capabilities["browserstack.key"] = accessKey;

            // Start Local if browserstack.local is set to true
            if (profile.Equals("local") && accessKey is not null)
            {
                capabilities["browserstack.local"] = true;
                browserStackLocal = new Local();
                List<KeyValuePair<string, string>> bsLocalArgs = new List<KeyValuePair<string, string>>() {
                    new KeyValuePair<string, string>("key", accessKey)
                };
                foreach (var localOption in config.GetValue("localOptions") as JObject)
                {
                    if (localOption.Value is not null)
                    {
                        bsLocalArgs.Add(new KeyValuePair<string, string>(localOption.Key, localOption.Value.ToString()));
                    }
                }
                browserStackLocal.start(bsLocalArgs);
            }

            string capsJson = JsonConvert.SerializeObject(capabilities);
            string cdpUrl = "wss://cdp.browserstack.com/playwright?caps=" + Uri.EscapeDataString(capsJson);

            var playwright = await Playwright.CreateAsync();
            browser = await playwright.Chromium.ConnectAsync(cdpUrl);
            page = await browser.NewPageAsync();
        }

        [TearDown]
        public async Task Cleanup()
        {
            if (browser != null)
            {
                browser.CloseAsync();
            }
            if (browserStackLocal != null)
            {
                browserStackLocal.stop();
            }
        }

        public static async Task SetStatus(IPage browserPage, bool passed)
        {
            if (browserPage is not null)
            {
                if (passed)
                    await browserPage.EvaluateAsync("_ => {}", "browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": {\"status\":\"passed\", \"reason\": \"Test Passed!\"}}");
                else
                    await browserPage.EvaluateAsync("_ => {}", "browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": {\"status\":\"failed\", \"reason\": \"Test Failed!\"}}");
            }
        }
    }
}
