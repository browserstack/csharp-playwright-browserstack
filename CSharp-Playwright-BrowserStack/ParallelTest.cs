using NUnit.Framework;

namespace CSharpPlaywrightBrowserStack
{
    [TestFixture("parallel", "chrome", "parallel.conf.json")]
    [TestFixture("parallel", "playwright-firefox", "parallel.conf.json")]
    [TestFixture("parallel", "playwright-webkit", "parallel.conf.json")]
    [Parallelizable(ParallelScope.Fixtures)]
    [Category("sample-parallel-test")]
    public class ParallelTest : SingleTest
    {
        public ParallelTest(string profile, string environment, string configFile) : base(profile, environment, configFile) { }
    }
}
