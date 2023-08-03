using NUnit.Framework;

namespace CSharpPlaywrightBrowserStack
{
    [TestFixture("parallel", "chrome")]
    [TestFixture("parallel", "firefox")]
    [TestFixture("parallel", "safari")]
    [TestFixture("parallel", "edge")]
    [Parallelizable(ParallelScope.Fixtures)]
    [Category("sample-parallel-test")]
    public class ParallelTest : SingleTest
    {
        public ParallelTest(string profile, string environment) : base(profile, environment) { }
    }
}
