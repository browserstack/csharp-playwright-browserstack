using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace CSharpPlaywrightBrowserStack
{
    [TestFixture]
    [Category("sample-local-test")]
    public class SampleLocalTest : PageTest
    {
        public SampleLocalTest() : base() { }

        [Test]
        public async Task BStackHealthCheck()
        {
            // Navigate to the base url
            await Page.GotoAsync("http://bs-local.com:45454/");

            // Verify if BrowserStackLocal running
            var title = await Page.TitleAsync();
            StringAssert.Contains("BrowserStack Local", title);
        }
    }
}

