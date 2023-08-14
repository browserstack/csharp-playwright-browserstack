using NUnit.Framework;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace CSharpPlaywrightBrowserStack
{
    [TestFixture]
    [Category("sample-test")]
    public class SampleTest : PageTest
    {
        public SampleTest() : base() { }

        [Test]
        public async Task SearchBstackDemo()
        {
            //Navigate to the bstackdemo url
            _ = await Page.GotoAsync("https://bstackdemo.com/");

            // Add the first item to cart
            await Page.Locator("[id=\"\\31 \"]").GetByText("Add to Cart").ClickAsync();
            IReadOnlyList<string> phone = await Page.Locator("[id=\"\\31 \"]").Locator(".shelf-item__title").AllInnerTextsAsync();
            Console.WriteLine("Phone =>" + phone[0]);


            // Get the items from Cart
            IReadOnlyList<string> quantity = await Page.Locator(".bag__quantity").AllInnerTextsAsync();
            Console.WriteLine("Bag quantity =>" + quantity[0]);

            // Verify if there is a shopping cart
            StringAssert.Contains("1", await Page.Locator(".bag__quantity").InnerTextAsync());


            //Get the handle for cart item
            ILocator cartItem = Page.Locator(".shelf-item__details").Locator(".title");

            // Verify if the cart has the right item
            StringAssert.Contains(await cartItem.InnerTextAsync(), string.Join(" ", phone));
            IReadOnlyList<string> cartItemText = await cartItem.AllInnerTextsAsync();
            Console.WriteLine("Cart item => " + cartItemText[0]);

            Assert.That(phone[0], Is.EqualTo(cartItemText[0]));
        }
    }
}
