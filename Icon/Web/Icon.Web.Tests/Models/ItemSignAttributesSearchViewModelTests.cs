using Icon.Web.Mvc.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Icon.Web.Tests.Unit.Models
{
    [TestClass]
    public class ItemSignAttributesSearchViewModelTests
    {
        private ItemSignAttributesSearchViewModel viewModel;

        [TestMethod]
        public void Constructor_ShouldFillSelectLists()
        {
            //When
            viewModel = new ItemSignAttributesSearchViewModel();

            ////Then
            //AssertSelectListIsEqualToDictionary(MilkTypes.AsDictionary, viewModel.CheeseMilkTypes);
            //AssertSelectListIsEqualToDictionary(EcoScaleRatings.AsDictionary, viewModel.EcoScaleRatings);
            //AssertSelectListIsEqualToDictionary(AnimalWelfareRatings.AsDictionary, viewModel.AnimalWelfareRatings);
            //AssertSelectListIsEqualToDictionary(SeafoodCatchTypes.AsDictionary, viewModel.SeafoodCatchTypes);
            //AssertSelectListIsEqualToDictionary(SeafoodFreshOrFrozenTypes.AsDictionary, viewModel.SeafoodFreshOrFrozen);
            AssertSelectListIsYesOrNoSelectList(viewModel.BiodynamicOptions);
            AssertSelectListIsYesOrNoSelectList(viewModel.CheeseRawOptions);
            AssertSelectListIsYesOrNoSelectList(viewModel.PremiumBodyCareOptions);
            AssertSelectListIsYesOrNoSelectList(viewModel.VegetarianOptions);
            AssertSelectListIsYesOrNoSelectList(viewModel.WholeTradeOptions);
        }

        private void AssertSelectListIsEqualToDictionary(Dictionary<int, string> dictionary, SelectList selectList)
        {
            // The SelectList will contain an entry for empty string which needs to be accounted for.
            Assert.IsTrue(selectList.Any(item => item.Text == String.Empty));

            selectList = new SelectList(selectList.Where(item => item.Text != String.Empty), "Value", "Text");
            Assert.AreEqual(dictionary.Count, selectList.Count());

            foreach (var kvp in dictionary)
            {
                var selectListItem = selectList.Single(sli => Int32.Parse(sli.Value) == kvp.Key);
                Assert.AreEqual(kvp.Value, selectListItem.Text);
            }
        }

        private void AssertSelectListIsYesOrNoSelectList(SelectList selectList)
        {
            Assert.AreEqual(String.Empty, selectList.ElementAt(0).Text);
            Assert.AreEqual("No", selectList.ElementAt(1).Text);
            Assert.AreEqual("Yes", selectList.ElementAt(2).Text);
        }
    }
}
