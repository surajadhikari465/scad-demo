using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Web.Mvc.Utility;

namespace Icon.Web.Tests.Unit.Utility
{
    [TestClass]
    public class ViewModelHelpersTests
    {
        [TestMethod]
        public void GetYesOrNoSelectList_ShouldReturnSelectListWithEmptyYesAndNoOptions()
        {
            //When
            var selectList = ViewModelHelpers.BuildYesOrNoSelectList();

            //Then
            Assert.AreEqual(3, selectList.Count());
            Assert.AreEqual(String.Empty, selectList.ElementAt(0).Text);
            Assert.AreEqual(null, selectList.ElementAt(0).Value);
            Assert.AreEqual("No", selectList.ElementAt(1).Text);
            Assert.AreEqual(null, selectList.ElementAt(1).Value);
            Assert.AreEqual("Yes", selectList.ElementAt(2).Text);
            Assert.AreEqual(null, selectList.ElementAt(2).Value);
        }

        [TestMethod]
        public void GetYesOrNoSelectListWihtoutBlank_ShouldReturnSelectListWithYesAndNoOptions()
        {
            //When
            var selectList = ViewModelHelpers.BuildYesOrNoSelectList(false);

            //Then
            Assert.AreEqual(2, selectList.Count());
            Assert.AreEqual("No", selectList.ElementAt(0).Text);
            Assert.AreEqual(null, selectList.ElementAt(0).Value);
            Assert.AreEqual("Yes", selectList.ElementAt(1).Text);
            Assert.AreEqual(null, selectList.ElementAt(1).Value);
        }
    }
}
