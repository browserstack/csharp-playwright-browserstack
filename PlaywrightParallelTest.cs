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


        Dictionary<string, string> windowsChromeCap = new Dictionary<string, string>();
        windowsChromeCap.Add("browser", "chrome");    // allowed browsers are `chrome`, `edge`, `playwright-chromium`, `playwright-firefox` and `playwright-webkit`
        windowsChromeCap.Add("browser_version", "latest");
        windowsChromeCap.Add("os", "Windows");
        windowsChromeCap.Add("os_version", "11");
        windowsChromeCap.Add("build", "browserstack-build-1");
        windowsChromeCap.Add("buildTag", "Regression");
        windowsChromeCap.Add("browserstack.debug", "true");
        windowsChromeCap.Add("browserstack.networkLogs", "true");
        windowsChromeCap.Add("browserstack.console", "info");
        windowsChromeCap.Add("browserstack.username", BROWSERSTACK_USERNAME);
        windowsChromeCap.Add("browserstack.accessKey", BROWSERSTACK_ACCESS_KEY);
        capabilitiesList.Add(windowsChromeCap);


        Dictionary<string, string> venturaWebkitCap = new Dictionary<string, string>();
        venturaWebkitCap.Add("browser", "playwright-webkit");    // allowed browsers are `chrome`, `edge`, `playwright-chromium`, `playwright-firefox` and `playwright-webkit`
        venturaWebkitCap.Add("browser_version", "latest");
        venturaWebkitCap.Add("os", "osx");
        venturaWebkitCap.Add("os_version", "Ventura");
        venturaWebkitCap.Add("build", "browserstack-build-1");
        venturaWebkitCap.Add("buildTag", "Regression");
        venturaWebkitCap.Add("browserstack.debug", "true");
        venturaWebkitCap.Add("browserstack.networkLogs", "true");
        venturaWebkitCap.Add("browserstack.console", "info");
        venturaWebkitCap.Add("browserstack.username", BROWSERSTACK_USERNAME);
        venturaWebkitCap.Add("browserstack.accessKey", BROWSERSTACK_ACCESS_KEY);
        capabilitiesList.Add(venturaWebkitCap);

        Dictionary<string, string> windowsFirefoxCap = new Dictionary<string, string>();
        windowsFirefoxCap.Add("browser", "playwright-firefox"); // allowed browsers are `chrome`, `edge`, `playwright-chromium`, `playwright-firefox` and `playwright-webkit`\
        windowsFirefoxCap.Add("browser_version", "latest");
        windowsFirefoxCap.Add("os", "Windows");
        windowsFirefoxCap.Add("os_version", "11");
        windowsFirefoxCap.Add("build", "browserstack-build-1");
        windowsFirefoxCap.Add("buildTag", "Regression");
        windowsFirefoxCap.Add("browserstack.debug", "true");
        windowsFirefoxCap.Add("browserstack.networkLogs", "true");
        windowsFirefoxCap.Add("browserstack.console", "info");
        windowsFirefoxCap.Add("browserstack.username", BROWSERSTACK_USERNAME);
        windowsFirefoxCap.Add("browserstack.accessKey", BROWSERSTACK_ACCESS_KEY);
        capabilitiesList.Add(windowsFirefoxCap);

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
            await MarkTestName(page);
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
    public static async Task MarkTestName(IPage page)
    {
        string? thisFile = new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileName();
        string[] result = thisFile.Split(new char[] { '/' });
        string[] filenameResult = result.Last().Split(new char[] { '.' });
        string? testName = filenameResult.First();

        await page.EvaluateAsync("_ => {}", "browserstack_executor: {\"action\": \"setSessionName\", \"arguments\": {\"name\":\"" + testName + "\"}}");
    }
}
