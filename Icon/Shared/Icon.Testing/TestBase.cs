using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Icon.Testing
{
    [TestClass]
    public class TestBase
    {
        [TestInitialize]
        public void BaseInitialize()
        {
            Given();
            When();
        }

        [TestCleanup]
        public void BaseCleanup()
        {
            Cleanup();
        }

        public virtual void Given() { }
        public virtual void When() { }
        public virtual void Cleanup() { }
    }
}
