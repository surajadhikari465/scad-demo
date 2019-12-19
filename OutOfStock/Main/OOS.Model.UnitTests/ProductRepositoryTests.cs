using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using NUnit.Framework;
using OOS.Model.Repository;
using SharedKernel;
using StructureMap;

namespace OOS.Model.UnitTests
{
    [TestFixture]
    public class ProductRepositoryTests
    {
        [SetUp]
        public void Setup()
        {
            Bootstrapper.Bootstrap();
        }

        [Test]
        public void TestProductInvalidReturnsProductNone()
        {
            var upcs = new List<string> { "INVALID PRODUCT" };
            var repository = CreateObjectUnderTest();
            var products = repository.For(upcs);
            Assert.AreEqual(1, products.Count());

        }

        [Test]
        [Category("Integration Test")]
        public void TestQueryMaster()
        {
            var upcs = new List<string> { "0471151800871" };
            var repository = CreateObjectUnderTest();
            var products = repository.For(upcs);
            Assert.AreEqual(1, products.Count());
        }

        private IProductRepository CreateObjectUnderTest()
        {
            return ObjectFactory.GetInstance<IProductRepository>();
        }

        [Test]
        [Category("integration Test")]
        public void TestQueryMasterQueryRegion()
        {
            var upcs = new List<string> { "0009948244088", "0471151800871" };
            var repository = CreateObjectUnderTest();
            var products = repository.For(upcs);

            Assert.AreEqual(2, products.Count());
            var dic = products.ToDictionary(p => p.UPC.Code, q => q);
            Assert.IsTrue(dic.ContainsKey("0009948244088"));
            Assert.AreEqual("OTHER", dic["0009948244088"].CategoryName);
            Assert.IsTrue(dic.ContainsKey("0471151800871"));
            Assert.AreEqual("Meat Alternatives", dic["0471151800871"].ClassName);
        }

        [Test]
        [Category("Integration Test")]
        public void TestProductInvalidValidReturnsOneValidOneNew()
        {
            var upcs = new List<string> { "0007085900074", "NOT IN STORE" };
            var repository = CreateObjectUnderTest();
            var products = repository.For(upcs);
            Assert.AreEqual(2, products.Count());
            var dic = products.ToDictionary(p => p.UPC.Code, q => q);
            Assert.IsTrue(dic.ContainsKey("0007085900074") && dic["0007085900074"].ClassName == "Juices & Functional Drinks");
        }


        [Test]
        [Category("Integration Test")]
        public void TestSingleUPC()
        {
            var repository = CreateObjectUnderTest();
            var product = repository.For("0089445500071");
            Assert.IsTrue(product.UPC.Code == "0089445500071");
            Assert.AreEqual(null, product.CategoryName);
        }

        [Test]
        [Category("Integration Test")]
        public void TestSingleInvalidUPC()
        {
            var repository = CreateObjectUnderTest();
            var product = repository.For("NOT IN STORE");
            Assert.IsTrue(product.UPC.Code == "NOT IN STORE");
            Assert.AreEqual(string.Empty, product.CategoryName);
        }

        [Test]
        [Category("Integration Test")]
        public void TestItemRegionLeftJoinOnRegHierarchy()
        {
            var upcs = new List<string> { "0089519200109", "0089618100137" };
            var repository = CreateObjectUnderTest();
            var products = repository.For(upcs);
            Assert.AreEqual(2, products.Count());
            var dic = products.ToDictionary(p => p.UPC.Code, q => q);
            Assert.IsTrue(dic.ContainsKey("0089618100137"));
            Assert.AreEqual("MISCELLANEOUS", dic["0089618100137"].ClassName);

            Assert.IsTrue(dic.ContainsKey("0089519200109"));
            Assert.AreEqual(null, dic["0089519200109"].ClassName);
        }

        [Test]
        [Category("Integration Test")]
        public void DumpUpcsPresentStatusFromFile()
        {
            Console.WriteLine("DumpUpcsPresentStatusFromFile()\n");
            var upcs = UploadMessage.From(Properties.Resources.upload).Scans.ToArray();
            var result = GetUpcsPresentStatus(upcs);
            Console.WriteLine(result);
        }

        private string GetUpcsPresentStatus(string[] upcs)
        {
            var repository = CreateObjectUnderTest();
            var products = repository.For(upcs);
            var distinctProducts = new List<IProduct>();
            foreach (var product in products)
            {
                var product1 = product;
                if (!distinctProducts.Any(p => p.UPC.Code == product1.UPC.Code))
                    distinctProducts.Add(product);
            }

            var upcsPresent = (from upc in upcs
                               let productMap = distinctProducts.ToDictionary(p => p.UPC.Code, q => q)
                               where productMap[upc].Brand != string.Empty && productMap[upc].Size != string.Empty
                               select upc).ToList();
            var builder = new StringBuilder();

            for (var i = 0; i < upcs.Length; i++)
            {
                if (i > 0) builder = builder.Append("\n");
                builder.Append("\"").Append(upcs[i]).Append("\"").Append(' ').Append(upcsPresent.Contains(upcs[i]));
            }

            return builder.ToString();
        }

        [Test]
        public void DumpUpcPresentStatusFromList()
        {
            var upcs = new List<string>
                           {
                               "0009034132200"
                           };
            var result = GetUpcsPresentStatus(upcs.ToArray());
            Console.WriteLine(result);
        }


    }
}
