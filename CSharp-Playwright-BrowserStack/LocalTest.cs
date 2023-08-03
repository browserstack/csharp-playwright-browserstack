using NUnit.Framework;
using Microsoft.Playwright;
using System.Text.RegularExpressions;

namespace CSharpPlaywrightBrowserStack
{
    [TestFixture("local", "chrome")]
    [Category("sample-local-test")]
    public class LocalTest : BrowserStackNUnitTest
    {
        public LocalTest(string profile, string environment) : base(profile, environment) { }

        [Test]
        public async Task HealthCheck()
        {
            // Navigate to the base url
            await page.GotoAsync("http://bs-local.com:45454/");

            // Verify if BrowserStackLocal running
            var title = await page.TitleAsync();
            StringAssert.Contains("BrowserStack Local", title);
        }
    }
}

