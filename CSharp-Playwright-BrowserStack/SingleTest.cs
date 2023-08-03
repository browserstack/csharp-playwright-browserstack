using NUnit.Framework;
using Microsoft.Playwright;

namespace CSharpPlaywrightBrowserStack
{
    [TestFixture("single", "chrome")]
    [Category("sample-test")]
    public class SingleTest : BrowserStackNUnitTest
    {
        public SingleTest(string profile, string environment) : base(profile, environment) { }

        [Test]
        public async Task SearchBstackDemo()
        {
            //Navigate to the bstackdemo url
            _ = await page.GotoAsync("https://bstackdemo.com/");

            // Add the first item to cart
            await page.Locator("[id=\"\\31 \"]").GetByText("Add to Cart").ClickAsync();
            IReadOnlyList<string> phone = await page.Locator("[id=\"\\31 \"]").Locator(".shelf-item__title").AllInnerTextsAsync();
            Console.WriteLine("Phone =>" + phone[0]);


            // Get the items from Cart
            IReadOnlyList<string> quantity = await page.Locator(".bag__quantity").AllInnerTextsAsync();
            Console.WriteLine("Bag quantity =>" + quantity[0]);

            // Verify if there is a shopping cart
            StringAssert.Contains("1", await page.Locator(".bag__quantity").InnerTextAsync());


            //Get the handle for cart item
            ILocator cartItem = page.Locator(".shelf-item__details").Locator(".title");

            // Verify if the cart has the right item
            StringAssert.Contains(await cartItem.InnerTextAsync(), string.Join(" ", phone));
            IReadOnlyList<string> cartItemText = await cartItem.AllInnerTextsAsync();
            Console.WriteLine("Cart item => " + cartItemText[0]);

            Assert.Equals(cartItemText[0], phone[0]);
        }
    }
}
