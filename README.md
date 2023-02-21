# Testing with playwright-browserstack in C#

[Playwright](https://playwright.dev/dotnet/) Integration with BrowserStack.

![BrowserStack Logo](https://d98b8t1nnulk5.cloudfront.net/production/images/layout/logo-header.png?1469004780)

## Setup

* Clone the repo and run `cd csharp-playwright-browserstack`
* Set `BROWSERSTACK_USERNAME` and `BROWSERSTACK_ACCESS_KEY` as environment variables with your [BrowserStack Username and Access Key](https://www.browserstack.com/accounts/settings)
* Run `dotnet build`

## Running your tests

- To run a parallel test, run command `dotnet run parallel`

  ### [Web application hosted on internal environment] Running your tests on BrowserStack using BrowserStackLocal

    #### Prerequisites

    - Clone the [BrowserStack demo application](https://github.com/browserstack/browserstack-demo-app) repository.
      ```sh
      git clone https://github.com/browserstack/browserstack-demo-app
      ```
    - Please follow the README.md on the BrowserStack demo application repository to install and start the dev server on localhost.
    - Note: You may need to provide additional BrowserStackLocal arguments to successfully connect your localhost environment with BrowserStack infrastructure. (e.g if you are behind firewalls, proxy or VPN).
    - Further details for successfully creating a BrowserStackLocal connection can be found here:

      - [Local Testing with Automate](https://www.browserstack.com/local-testing/automate)

      #### Running the test using Local Testing:
      - You can then run the sample Local test using `dotnet run local`


## Notes
* You can view your test results on the [BrowserStack Automate dashboard](https://www.browserstack.com/automate)

## Additional Resources
* [Documentation for writing Automate test scripts with BrowserStack](https://www.browserstack.com/docs/automate/playwright)
