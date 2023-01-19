using Microsoft.Playwright.NUnit;
using Microsoft.Playwright;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BrowserStackPlaywrightNUnit;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class SampleTest2 : BrowserStackPageTest
{
    [Test]
    public async Task SampleTest2Test1()
    {
        Page.SetDefaultTimeout(60000);
        await Page.GotoAsync("https://playwright.dev");
        await Assertions.Expect(Page.Locator("text=enables reliable end-to-end testing for modern web apps")).ToBeVisibleAsync();
    }

    [Test]
    public async Task SampleTest2Test2()
    {
        Page.SetDefaultTimeout(60000);
        await Page.GotoAsync("https://playwright.dev");
        var title = Page.Locator(".navbar__inner .navbar__title");
        await Assertions.Expect(title).ToHaveTextAsync("Playwright");
    }
}