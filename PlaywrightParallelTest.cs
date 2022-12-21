using Microsoft.Playwright;
using System;
using System.Threading;
using Newtonsoft.Json;
using System.Collections;

class PlaywrightParallelTest
{
    public static async Task main(string[] args)
    {
        //  The following capability variables contains the set of os/browser environments where you want to run your tests. You can choose to alter this list according to your needs. Read more on https://browserstack.com/docs/automate/playwright/browsers-and-os
        try
        {
            ArrayList capabilitiesList = getCapabilitiesList();
            Task[] taskList = new Task[capabilitiesList.Count];

            for (int i = 0; i < capabilitiesList.Count; i++)
            {
                string capsJson;
                capsJson = JsonConvert.SerializeObject(capabilitiesList[i]);
                var task = Executetestwithcaps(capsJson);
                taskList[i] = task;
            }

            await Task.WhenAll(taskList);

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    static ArrayList getCapabilitiesList()
    {
        ArrayList capabilitiesList = new ArrayList();

        string? BROWSERSTACK_USERNAME = Environment.GetEnvironmentVariable("BROWSERSTACK_USERNAME");
        string? BROWSERSTACK_ACCESS_KEY = Environment.GetEnvironmentVariable("BROWSERSTACK_ACCESS_KEY");

        Dictionary<string, string> catalinaChromeCap = new Dictionary<string, string>();
        catalinaChromeCap.Add("browser", "chrome");    // allowed browsers are `chrome`, `edge`, `playwright-chromium`, `playwright-firefox` and `playwright-webkit`
        catalinaChromeCap.Add("browser_version", "latest");
        catalinaChromeCap.Add("os", "osx");
        catalinaChromeCap.Add("os_version", "Monterey");
        catalinaChromeCap.Add("name", "Branded Google Chrome on Monterey");
        catalinaChromeCap.Add("build", "playwright-dotnet-2");
        catalinaChromeCap.Add("browserstack.username", BROWSERSTACK_USERNAME);
        catalinaChromeCap.Add("browserstack.accessKey", BROWSERSTACK_ACCESS_KEY);
        capabilitiesList.Add(catalinaChromeCap);

        Dictionary<string, string> catalinaEdgeCap = new Dictionary<string, string>();
        catalinaEdgeCap.Add("browser", "edge");  // allowed browsers are `chrome`, `edge`, `playwright-chromium`, `playwright-firefox` and `playwright-webkit`
        catalinaEdgeCap.Add("browser_version", "latest");
        catalinaEdgeCap.Add("os", "osx");
        catalinaEdgeCap.Add("os_version", "Monterey");
        catalinaEdgeCap.Add("name", "Branded Microsoft Edge on Monterey");
        catalinaEdgeCap.Add("build", "playwright-dotnet-2");
        catalinaEdgeCap.Add("browserstack.username", BROWSERSTACK_USERNAME);
        catalinaEdgeCap.Add("browserstack.accessKey", BROWSERSTACK_ACCESS_KEY);
        capabilitiesList.Add(catalinaEdgeCap);

        Dictionary<string, string> catalinaFirefoxCap = new Dictionary<string, string>();
        catalinaFirefoxCap.Add("browser", "playwright-firefox");    // allowed browsers are `chrome`, `edge`, `playwright-chromium`, `playwright-firefox` and `playwright-webkit`
        catalinaFirefoxCap.Add("browser_version", "latest");
        catalinaFirefoxCap.Add("os", "osx");
        catalinaFirefoxCap.Add("os_version", "Monterey");
        catalinaFirefoxCap.Add("name", "Playwright firefox on Monterey");
        catalinaFirefoxCap.Add("build", "playwright-dotnet-2");
        catalinaFirefoxCap.Add("browserstack.username", BROWSERSTACK_USERNAME);
        catalinaFirefoxCap.Add("browserstack.accessKey", BROWSERSTACK_ACCESS_KEY);
        capabilitiesList.Add(catalinaFirefoxCap);

        Dictionary<string, string> catalinaWebkitCap = new Dictionary<string, string>();
        catalinaWebkitCap.Add("browser", "playwright-webkit"); // allowed browsers are `chrome`, `edge`, `playwright-chromium`, `playwright-firefox` and `playwright-webkit`\
        catalinaWebkitCap.Add("browser_version", "latest");
        catalinaWebkitCap.Add("os", "osx");
        catalinaWebkitCap.Add("os_version", "Monterey");
        catalinaWebkitCap.Add("name", "Playwright webkit on Monterey");
        catalinaWebkitCap.Add("build", "playwright-dotnet-2");
        catalinaWebkitCap.Add("browserstack.username", BROWSERSTACK_USERNAME);
        catalinaWebkitCap.Add("browserstack.accessKey", BROWSERSTACK_ACCESS_KEY);
        capabilitiesList.Add(catalinaWebkitCap);

        Dictionary<string, string> catalinaChromiumCap = new Dictionary<string, string>();
        catalinaChromiumCap.Add("browser", "playwright-chromium"); // allowed browsers are `chrome`, `edge`, `playwright-chromium`, `playwright-firefox` and `playwright-webkit`
        catalinaChromiumCap.Add("browser_version", "latest");
        catalinaChromiumCap.Add("os", "osx");
        catalinaChromiumCap.Add("os_version", "Monterey");
        catalinaChromiumCap.Add("name", "Playwright webkit on Monterey");
        catalinaChromiumCap.Add("build", "playwright-dotnet-2");
        catalinaChromiumCap.Add("browserstack.username", BROWSERSTACK_USERNAME);
        catalinaChromiumCap.Add("browserstack.accessKey", BROWSERSTACK_ACCESS_KEY);
        capabilitiesList.Add(catalinaChromiumCap);

        return capabilitiesList;
    }

    //Executetestwithcaps function takes capabilities from 'SampleTestCase' function and executes the test
    public static async Task Executetestwithcaps(string capabilities)
    {
        using var playwright = await Playwright.CreateAsync();
        string cdpUrl = "wss://cdp.browserstack.com/playwright?caps=" + Uri.EscapeDataString(capabilities);

        await using var browser = await playwright.Chromium.ConnectAsync(cdpUrl);
        var page = await browser.NewPageAsync();
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
