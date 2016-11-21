using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Esb;

namespace Icon.Infor.Listeners.HierarchyClass.Tests
{
    [TestClass]
    public class ProgramTests
    {
        [TestMethod]
        public void CreateHierarchyClassListener()
        {
            Program.CreateHierarchyClassListener().Verify();
        }
    }
}
