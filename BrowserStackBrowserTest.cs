using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace BrowserStackPlaywrightNUnit
{
    public class BrowserStackBrowserTest : PlaywrightTest
    {
        public IBrowser Browser { get; internal set; } = null!;
        private readonly List<IBrowserContext> _contexts = new();

        public async Task<IBrowserContext> NewContext(BrowserNewContextOptions? options = null)
        {
            var context = await Browser.NewContextAsync(options).ConfigureAwait(false);
            _contexts.Add(context);
            return context;
        }

        [SetUp]
        public async Task BrowserSetup()
        {
            var service = await BrowserStackService.Register(this, BrowserType).ConfigureAwait(false);
            Console.WriteLine(BrowserName);
            Browser = service.Browser;
        }

        //onetimeTearDown
        [TearDown]
        public async Task BrowserTearDown()
        {
            if (TestOk())
            {
                foreach (var context in _contexts)
                {
                    await context.CloseAsync().ConfigureAwait(false);
                    //await Browser.CloseAsync();
                }
            }
            _contexts.Clear();

            //await Browser.CloseAsync();
            Browser = null!;
            //await Browser.CloseAsync();

        }

        //[OneTimeTearDown]
        //public async Task BrowserSessionTearDown()
        //{
        //    Console.WriteLine("OneTimeTearDown process");
        //    await Browser.CloseAsync();
        //    Browser = null!;


        //}
    }
}

