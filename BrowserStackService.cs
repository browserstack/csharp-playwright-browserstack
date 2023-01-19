using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using Microsoft.Playwright.TestAdapter;
using Newtonsoft.Json;

internal class BrowserStackService : IWorkerService
{
    public IBrowser Browser { get; internal set; } = null!;

    public static Task<BrowserStackService> Register(WorkerAwareTest test, IBrowserType browserType)
    {
        Dictionary<string, string> browserstackOptions = new Dictionary<string, string>();
        browserstackOptions.Add("name", "Playwright first sample test - 2");
        browserstackOptions.Add("build", "playwright-dotnet-1");
        browserstackOptions.Add("os", "osx");
        browserstackOptions.Add("os_version", "catalina");
        browserstackOptions.Add("browser", "chrome");  
        browserstackOptions.Add("browserstack.username", "BROWSERSTACK_USERNAME");
        browserstackOptions.Add("browserstack.accessKey", "BROWSERSTACK_ACCESS_KEY");

        string capsJson = JsonConvert.SerializeObject(browserstackOptions);
        string cdpUrl = "wss://cdp.browserstack.com/playwright?caps=" + Uri.EscapeDataString(capsJson);


        return test.RegisterService("Browser", async () => new BrowserStackService
        {
            Browser = await browserType.ConnectAsync(cdpUrl).ConfigureAwait(false)
        }) ;
    }

    public Task ResetAsync() => Task.CompletedTask;
    public Task DisposeAsync() => Browser.CloseAsync();
}
