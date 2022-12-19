using Microsoft.Playwright;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

class PlaywrightTest
{
    public static async Task main(string[] args)
    {
        using var playwright = await Playwright.CreateAsync();

        string? BROWSERSTACK_USERNAME = Environment.GetEnvironmentVariable("BROWSERSTACK_USERNAME");
        string? BROWSERSTACK_ACCESS_KEY = Environment.GetEnvironmentVariable("BROWSERSTACK_ACCESS_KEY");

        Dictionary<string, string> browserstackOptions = new Dictionary<string, string>();
        browserstackOptions.Add("name", "Playwright first sample test");
        browserstackOptions.Add("build", "playwright-dotnet-1");
        browserstackOptions.Add("os", "osx");
        browserstackOptions.Add("os_version", "Monterey");
        browserstackOptions.Add("browser", "chrome");  // allowed browsers are `chrome`, `edge`, `playwright-chromium`, `playwright-firefox` and `playwright-webkit`
        browserstackOptions.Add("browser_version", "latest");
        browserstackOptions.Add("browserstack.username", BROWSERSTACK_USERNAME);
        browserstackOptions.Add("browserstack.accessKey", BROWSERSTACK_ACCESS_KEY);
        string capsJson = JsonConvert.SerializeObject(browserstackOptions);
        string cdpUrl = "wss://cdp.browserstack.com/playwright?caps=" + Uri.EscapeDataString(capsJson);

        await using var browser = await playwright.Chromium.ConnectAsync(cdpUrl);
        var page = await browser.NewPageAsync();
        try
        {
            page.SetDefaultTimeout(60000);
            await page.GotoAsync("https://bstackdemo.com/");
            await page.Locator("#signin").ClickAsync();
            await page.FillAsync("#react-select-2-input", "fav_user");
            await page.PressAsync("#react-select-2-input", "Enter");
            await page.FillAsync("#react-select-3-input", "testingisfun99");
            await page.PressAsync("#react-select-3-input", "Enter");
            await page.Locator(".Button_root__24MxS").ClickAsync();
            var username = await page.Locator(".username").TextContentAsync();

            if (username == "fav_user")
            {
                // following line of code is responsible for marking the status of the test on BrowserStack as 'passed'. You can use this code in your after hook after each test
                await MarkTestStatus("passed", "Login Done", page);
            }
            else
            {
                await MarkTestStatus("failed", "Login Failed", page);
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
