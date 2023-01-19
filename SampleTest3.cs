using Microsoft.Playwright.NUnit;
using Microsoft.Playwright;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BrowserStackPlaywrightNUnit;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class SampleTest3 : BrowserStackPageTest
{
    //[Test]
    //public async Task BrowserStackDemo()
    //{
    //    Page.SetDefaultTimeout(60000);
    //    await Page.GotoAsync("https://bstackdemo.com/");
    //    await Page.Locator("#\\31 > .shelf-item__buy-btn").ClickAsync();

    //    var text = await Page.Locator("#__next > div > div > div.float-cart.float-cart--open > div.float-cart__content > div.float-cart__shelf-container > div > div.shelf-item__details > p.title").TextContentAsync();

    //    //if (text == "iPhone 12")
    //    Assertions.Equals(text, "iPhone 12");
    //    //await Assertions.Expect(Page.Locator("text=enables reliable end-to-end testing for modern web apps")).ToBeVisibleAsync();
    //}

    [Test]
    public async Task SampleTest3Test()
    {
        Page.SetDefaultTimeout(60000);
        await Page.GotoAsync("https://playwright.dev");
        var title = Page.Locator(".navbar__inner .navbar__title");
        await Assertions.Expect(title).ToHaveTextAsync("Playwright");
    }
}