# csharp-playwright-browserstack
Creating a sample repo for CSharp Playwright

This sample elaborates the [NUnit](http://www.nunit.org/) Integration with BrowserStack.

![BrowserStack Logo](https://d98b8t1nnulk5.cloudfront.net/production/images/layout/logo-header.png?1469004780)

<img src ="https://nunit.org/img/nunit.svg" height = "71">

## Run Sample Build
* Clone the repo
* Open the solution `CSharp-Playwright-BrowserStack.sln` in Visual Studio
* Build the solution
* Update `browserstack.yml` file with your [BrowserStack Username and Access Key](https://www.browserstack.com/accounts/settings)

### Running your tests from CLI
- To run a single test, run test with fixture `sample-test` or test class `SingleTest`
```sh
dotnet test --filter "SingleTest"
```
- To run local tests, run test with fixture `sample-local-test` or test class `LocalTest`
```sh
dotnet test --filter "LocalTest"
```
- To run parallel tests, run test with fixture `sample-parallel-test` or test class `ParallelTest`
```sh
dotnet test --filter "ParallelTest"
```

 Understand how many parallel sessions you need by using our [Parallel Test Calculator](https://www.browserstack.com/automate/parallel-calculator?ref=github)

## Notes
* You can view your test results on the [BrowserStack automate dashboard](https://www.browserstack.com/automate)
* To test on a different set of browsers, check out our [platform configurator](https://www.browserstack.com/automate/c-sharp#setting-os-and-browser)
* You can export the environment variables for the Username and Access Key of your BrowserStack account

  * For Unix-like or Mac machines:
  ```
  export BROWSERSTACK_USERNAME=<browserstack-username> &&
  export BROWSERSTACK_ACCESS_KEY=<browserstack-access-key>
  ```

  * For Windows Cmd:
  ```
  set BROWSERSTACK_USERNAME=<browserstack-username>
  set BROWSERSTACK_ACCESS_KEY=<browserstack-access-key>
  ```

  * For Windows Powershell:
  ```
  $env:BROWSERSTACK_USERNAME=<browserstack-username>
  $env:BROWSERSTACK_ACCESS_KEY=<browserstack-access-key>
  ```

## Additional Resources
* [Documentation for writing automate playwright test scripts in C#](https://www.browserstack.com/docs/automate/playwright/getting-started/c-sharp)
* [Customizing your tests on BrowserStack](https://www.browserstack.com/automate/capabilities)
* [Browsers & mobile devices for selenium testing on BrowserStack](https://www.browserstack.com/list-of-browsers-and-platforms?product=automate)
* [Using REST API to access information about your tests via the command-line interface](https://www.browserstack.com/automate/rest-api)
