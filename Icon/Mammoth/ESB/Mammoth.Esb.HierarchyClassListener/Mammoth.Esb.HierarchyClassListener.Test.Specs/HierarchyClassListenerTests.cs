using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Esb.Subscriber;
using Moq;
using System.IO;
using Dapper;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using Mammoth.Esb.HierarchyClassListener.Models;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Mammoth.Esb.HierarchyClassListener.Test.Specs
{
    [TestClass]
    public class HierarchyClassListenerTests
    {
        private MammothHierarchyClassListener listener;

        [TestMethod]
        [Ignore] // Need to not truncate tables to affect other tests.
        public void HandleMessage_BrandMessages_ShouldAddBrandsToMammoth()
        {
            //Given
            var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString);
            connection.Execute(@"truncate table dbo.HierarchyClass;");

            var container = SimpleInjectorInitializer.Initialize();
            container.Options.AllowOverridingRegistrations = true;
            container.RegisterSingleton<IEsbSubscriber>(() => new Mock<IEsbSubscriber>().Object);

            listener = container.GetInstance<MammothHierarchyClassListener>();

            //When
            CallListenerWithFileText(@"TestMessages/Brands1.xml");
            CallListenerWithFileText(@"TestMessages/Brands2.xml");

            //Then
            var count = connection.Query<int>("SELECT COUNT(*) FROM dbo.HierarchyClass").Single();
            Assert.AreEqual(200, count);

            connection.Execute(@"truncate table dbo.HierarchyClass;");
        }

        [TestMethod]
        [Ignore] // Need to not truncate tables to affect other tests.
        public void HandleMessage_FullMerchandiseHierarchyLoad_ShouldPopulateAllMerchandiseHierarchies()
        {
            //Given
            var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString);
            connection.Execute(
                @"truncate table dbo.Hierarchy_Merchandise;
                  truncate table dbo.HierarchyClass;");

            var container = SimpleInjectorInitializer.Initialize();
            container.Options.AllowOverridingRegistrations = true;
            container.RegisterSingleton<IEsbSubscriber>(() => new Mock<IEsbSubscriber>().Object);

            listener = container.GetInstance<MammothHierarchyClassListener>();

            //When
            CallListenerWithFileText(@"TestMessages/Segments.xml");
            var count = connection.Query<int>("SELECT COUNT(*) FROM dbo.Hierarchy_Merchandise").Single();
            Assert.AreEqual(23, count);

            CallListenerWithFileText(@"TestMessages/Families.xml");
            count = connection.Query<int>("SELECT COUNT(*) FROM dbo.Hierarchy_Merchandise").Single();
            Assert.AreEqual(58, count);

            CallListenerWithFileText(@"TestMessages/Classes1.xml");
            CallListenerWithFileText(@"TestMessages/Classes2.xml");
            count = connection.Query<int>("SELECT COUNT(*) FROM dbo.Hierarchy_Merchandise").Single();
            Assert.AreEqual(193, count);

            CallListenerWithFileText(@"TestMessages/Bricks1.xml");
            CallListenerWithFileText(@"TestMessages/Bricks2.xml");
            CallListenerWithFileText(@"TestMessages/Bricks3.xml");
            CallListenerWithFileText(@"TestMessages/Bricks4.xml");
            CallListenerWithFileText(@"TestMessages/Bricks5.xml");
            CallListenerWithFileText(@"TestMessages/Bricks6.xml");
            CallListenerWithFileText(@"TestMessages/Bricks7.xml");
            count = connection.Query<int>("SELECT COUNT(*) FROM dbo.Hierarchy_Merchandise").Single();
            Assert.AreEqual(629, count);
            
            CallListenerWithFileText(@"TestMessages/SubBricks1.xml");
            CallListenerWithFileText(@"TestMessages/SubBricks2.xml");
            CallListenerWithFileText(@"TestMessages/SubBricks3.xml");
            CallListenerWithFileText(@"TestMessages/SubBricks4.xml");
            CallListenerWithFileText(@"TestMessages/SubBricks5.xml");
            CallListenerWithFileText(@"TestMessages/SubBricks6.xml");
            CallListenerWithFileText(@"TestMessages/SubBricks7.xml");
            CallListenerWithFileText(@"TestMessages/SubBricks8.xml");
            CallListenerWithFileText(@"TestMessages/SubBricks9.xml");
            CallListenerWithFileText(@"TestMessages/SubBricks10.xml");
            count = connection.Query<int>("SELECT COUNT(*) FROM dbo.Hierarchy_Merchandise").Single();
            Assert.AreEqual(965, count);

            //Then
            var emptyHierarchies = connection.Query<MerchandiseHierarchyModel>(
                @"select * from dbo.Hierarchy_Merchandise
                    where SegmentHCID is null
                        or FamilyHCID is null
                        or ClassHCID is null
                        or BrickHCID is null")
                .ToList();
            Assert.AreEqual(0, emptyHierarchies.Count);
            var emptySubBrickHierarchies = connection.Query<MerchandiseHierarchyModel>(
                @"select * from dbo.Hierarchy_Merchandise
                    where SubBrickHCID is null")
                .ToList();
            Assert.AreEqual(4, emptySubBrickHierarchies.Count);

            var totalMerchandiseHierarchyClassCount = connection.Query<int>("SELECT COUNT (*) FROM dbo.HierarchyClass").Single();
            Assert.AreEqual(1864, totalMerchandiseHierarchyClassCount);

            connection.Execute(
                @"truncate table dbo.Hierarchy_Merchandise;
                  truncate table dbo.HierarchyClass;");
        }
        
        private void CallListenerWithFileText(string filePath)
        {
            string text = File.ReadAllText(filePath);

            Mock<IEsbMessage> message = new Mock<IEsbMessage>();
            message.SetupGet(m => m.MessageText).Returns(text);

            listener.HandleMessage(null, new EsbMessageEventArgs { Message = message.Object });
        }
    }
}
