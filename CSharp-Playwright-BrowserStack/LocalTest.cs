using NUnit.Framework;

namespace CSharpPlaywrightBrowserStack
{
    [TestFixture("local", "chrome", "local.conf.json")]
    [Category("sample-local-test")]
    public class LocalTest : BrowserStackNUnitTest
    {
        public LocalTest(string profile, string environment, string configFile) : base(profile, environment, configFile) { }

        [Test]
        public async Task HealthCheck()
        {
            try
            {
                // Navigate to the base url
                await page.GotoAsync("http://bs-local.com:45454/");

                // Verify if BrowserStackLocal running
                var title = await page.TitleAsync();
                StringAssert.Contains("BrowserStack Local", title);
                SetStatus(page, title.Contains("BrowserStack Local"));
            } catch (Exception)
            {
                SetStatus(page, false);
                throw;
            }
        }
    }
}

