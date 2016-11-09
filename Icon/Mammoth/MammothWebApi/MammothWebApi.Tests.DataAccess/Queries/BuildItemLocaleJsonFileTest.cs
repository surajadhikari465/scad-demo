using Dapper;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using MammothWebApi.Models;
using System.IO;
using Newtonsoft.Json;
using Mammoth.Common.DataAccess.DbProviders;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using MammothWebApi.DataAccess.Models;

namespace MammothWebApi.Tests.DataAccess.Queries
{
    [TestClass]
    public class BuildItemLocaleJsonFileTest
    {
        private SqlDbProvider db;
        private string connectionString;

        [TestInitialize]
        public void Initialize()
        {
            db = new SqlDbProvider();
            connectionString = ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString;
        }

        public void TestItemLocaleJsonExistingItemsFile()
        {
            List<ItemLocaleModel> itemLocaleList = new List<ItemLocaleModel>();
            List<Item> items = new List<Item>();
            using (db.Connection = new SqlConnection(connectionString))
            {
                items = this.db.Connection.Query<Item>("SELECT TOP 500 * FROM Items WHERE Desc_Product like '%multi%'").ToList();
            }
            
            int counter = 0;
            foreach (var item in items)
            {
                ItemLocaleModel itemLocale = new ItemLocaleModel
                {
                    AgeRestriction = null,
                    Authorized = true,
                    BusinessUnitId = 10006,
                    CaseDiscount = true,
                    ChicagoBaby = null,
                    ColorAdded = null,
                    CountryOfProcessing = null,
                    Discontinued = false,
                    ElectronicShelfTag = null,
                    Exclusive = null,
                    LabelTypeDescription = "LRG",
                    LinkedItem = null,
                    LocalItem = false,
                    Locality = null,
                    MSRP = 3.99M,
                    NumberOfDigitsSentToScale = null,
                    Origin = null,
                    ProductCode = null,
                    Region = "NC",
                    RestrictedHours = false,
                    RetailUnit = "EACH",
                    ScaleExtraText = null,
                    ScanCode = item.ScanCode,
                    SignDescription = item.Desc_Product,
                    SignRomanceLong = String.Format("Test Long Sign Romance Text {0}", counter),
                    SignRomanceShort = String.Format("Test Short Sign Romance Text {0}", counter),
                    TagUom = null,
                    TmDiscount = true
                };

                itemLocaleList.Add(itemLocale);
            }

            using (var file = new StreamWriter(@"C:\Temp\testItemLocalejsonExisting.txt"))
            {
                string json = JsonConvert.SerializeObject(itemLocaleList);
                file.WriteLine(json);
            }
        }
    }
}
