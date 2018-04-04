using Icon.Web.Mvc.Excel;
using Icon.Web.Mvc.Excel.Models;
using Icon.Web.Mvc.Excel.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.Tests.Unit.Excel.Validators
{
    [TestClass] [Ignore]
    public class ItemWithoutUpdatesExcelValidatorTests
    {
        private ItemWithoutUpdatesExcelValidator<ItemExcelModel> validator;

        [TestInitialize]
        public void Initialize()
        {
            validator = new ItemWithoutUpdatesExcelValidator<ItemExcelModel>();
        }

        [TestMethod]
        public void ItemWithoutUpdates_3ItemsHaveNoUpdates_ShouldSetErrorOnItemsWithNoUpdates()
        {
            //Given
            var properties = typeof(ItemExcelModel).GetProperties()
                .Where(p => p.IsDefined(typeof(ExcelColumnAttribute), false) && p.Name != "ScanCode")
                .ToList();

            List<ItemExcelModel> models = properties
                .Select(p => new ItemExcelModel())
                .ToList();
            //Set initial values of properties to empty string
            properties.ForEach(p => models.ForEach(m => p.SetValue(m, string.Empty)));

            //Populate one property on each model
            for (int i = 0; i < properties.Count; i++)
            {
                properties[i].SetValue(models[i], "Test");
            }

            //Add new models with no properties set, they should fail
            var noUpdateItems = new List<ItemExcelModel>
            {
                new ItemExcelModel(),
                new ItemExcelModel(),
                new ItemExcelModel()
            };
            properties.ForEach(p => noUpdateItems.ForEach(m => p.SetValue(m, string.Empty)));
            models.AddRange(noUpdateItems);

            //When
            validator.Validate(models);

            //Then
            Assert.AreEqual(3, models.Count(m => m.Error != null));
            Assert.AreEqual(models.Count - 3, models.Count(m => m.Error == null));
        }
    }
}
