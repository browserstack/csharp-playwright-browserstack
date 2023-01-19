using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace BrowserStackPlaywrightNUnit
{
    public class BrowserStackPageTest : BrowserStackContextTest
    {
        public IPage Page { get; private set; } = null!;

        [SetUp]
        public async Task PageSetup()
        {
            Page = await Context.NewPageAsync().ConfigureAwait(false);
        }

    }
}

