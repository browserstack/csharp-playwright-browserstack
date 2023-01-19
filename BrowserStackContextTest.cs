using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace BrowserStackPlaywrightNUnit
{
    public class BrowserStackContextTest : BrowserStackBrowserTest
    {
        public IBrowserContext Context { get; private set; } = null!;

        public virtual BrowserNewContextOptions ContextOptions()
        {
            return null!;
        }

        [SetUp]
        public async Task ContextSetup()
        {
            Context = await NewContext(ContextOptions()).ConfigureAwait(false);
        }
    }
}

