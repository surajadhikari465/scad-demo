using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using AmazonLoad.Common;

namespace AmazonLoad.IconItemLocale.Tests
{
    /// <summary>
    /// Extebns IconItemLocalePsgMapper to allow return fake data without a DB connection
    /// </summary>
    public class FakeIconItemLocalePsgMapper : IconItemLocalePsgMapper
    {
        public List<ProductSelectionGroupModel> FakePsgData = new List<ProductSelectionGroupModel>();

        public FakeIconItemLocalePsgMapper() { }
        public FakeIconItemLocalePsgMapper(List<ProductSelectionGroupModel> testData)
        {
            FakePsgData = testData;
            base.LoadProductSelectionGroups();
        }

        protected internal override List<ProductSelectionGroupModel> QueryForProductSelectionGroups()
        {
            return FakePsgData;
        }
    }
}