using Microsoft.Playwright;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

class PlaywrightPixelTest
{
    public static async Task main(string[] args)
    {
        using var playwright = await Playwright.CreateAsync();

        string? BROWSERSTACK_USERNAME = Environment.GetEnvironmentVariable("BROWSERSTACK_USERNAME");
        string? BROWSERSTACK_ACCESS_KEY = Environment.GetEnvironmentVariable("BROWSERSTACK_ACCESS_KEY");

        Dictionary<string, string> browserstackOptions = new Dictionary<string, string>();
        browserstackOptions.Add("name", "Test on Playwright emulated Pixel 5");
        browserstackOptions.Add("build", "playwright-dotnet-4");
        browserstackOptions.Add("browser", "playwright-webkit");  // allowed browsers are `chrome`, `edge`, `playwright-chromium`, `playwright-firefox` and `playwright-webkit`
        browserstackOptions.Add("browserstack.username", BROWSERSTACK_USERNAME);
        browserstackOptions.Add("browserstack.accessKey", BROWSERSTACK_ACCESS_KEY);
        string capsJson = JsonConvert.SerializeObject(browserstackOptions);
        string cdpUrl = "wss://cdp.browserstack.com/playwright?caps=" + Uri.EscapeDataString(capsJson);

        await using var browser = await playwright.Chromium.ConnectAsync(cdpUrl);

        var context = await browser.NewContextAsync(playwright.Devices["Pixel 5"]);  // Complete list of devices - https://github.com/microsoft/playwright/blob/main/packages/playwright-core/src/server/deviceDescriptorsSource.json

        var page = await context.NewPageAsync();
        try
        {
            page.SetDefaultTimeout(60000);
            await page.GotoAsync("https://bstackdemo.com/");
            await page.Locator("#\\31 > .shelf-item__buy-btn").ClickAsync();

            var text = await page.Locator("#__next > div > div > div.float-cart.float-cart--open > div.float-cart__content > div.float-cart__shelf-container > div > div.shelf-item__details > p.title").TextContentAsync();

            if (text == "iPhone 12")
            {
                await MarkTestStatus("passed", "Item Added", page);
            }
            else
            {
                await MarkTestStatus("failed", "Test Failed", page);
            }
        }
        catch (Exception err)
        {
            Console.WriteLine(err.Message);
            await MarkTestStatus("failed", "Something Failed", page);
        }
        await browser.CloseAsync();
    }

    public static async Task MarkTestStatus(string status, string reason, IPage page)
    {
        await page.EvaluateAsync("_ => {}", "browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": {\"status\":\"" + status + "\", \"reason\": \"" + reason + "\"}}");
    }
}
