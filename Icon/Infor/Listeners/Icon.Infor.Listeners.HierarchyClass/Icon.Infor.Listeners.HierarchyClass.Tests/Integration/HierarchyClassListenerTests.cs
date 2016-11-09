﻿using Icon.Common.Context;
using Icon.Esb.ListenerApplication;
using Icon.Esb.Subscriber;
using Icon.Framework;
using Icon.Infor.Listeners.HierarchyClass.Notifier;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.HierarchyClass.Tests.Integration
{
    [TestClass]
    public class HierarchyClassListenerTests
    {
        [TestMethod]
        public void HandleMessage_MerchandiseHierarchy_ShouldAddOrUpdateMerchandiseHierarchyClass()
        {
            //Given
            var messageText = File.ReadAllText(@"TestMessages/IntegrationMerchandiseMessage.xml");
            var expectedHierarchyClassId = 83453;
            var expectedHierarchyClassName = "Apparel Accessory Test1 For Infor";
            var expectedTraits = new Dictionary<int, string>
            {
                { Traits.SubBrickCode, "99999999" }
            };

            //When
            RunHandleMessageTestForAddOrUpdates(messageText, expectedHierarchyClassId, expectedHierarchyClassName, expectedTraits);
        }

        [TestMethod]
        public void HandleMessage_TaxHierarchy_ShouldAddOrUpdateMerchandiseHierarchyClass()
        {
            //Given
            var messageText = File.ReadAllText(@"TestMessages/IntegrationTaxMessage.xml");
            var expectedHierarchyClassId = 40212;
            var expectedHierarchyClassName = "0000000 GENERAL MERCHANDISE TEST";
            var expectedTraits = new Dictionary<int, string>
            {
                { Traits.TaxAbbreviation, "0000000 GENERAL MERCHANDISE ABR" },
                { Traits.TaxRomance, "0000000 GENERAL MERCHANDISE ROMANCE" }
            };

            //When
            RunHandleMessageTestForAddOrUpdates(messageText, expectedHierarchyClassId, expectedHierarchyClassName, expectedTraits);
        }

        [TestMethod]
        public void HandleMessage_BrandHierarchy_ShouldAddOrUpdateBrandHierarchyClass()
        {
            //Given
            var messageText = File.ReadAllText(@"TestMessages/IntegrationBrandMessage.xml");
            var expectedHierarchyClassId = 129887;
            var expectedHierarchyClassName = "Test Brand For Infor";
            var expectedTraits = new Dictionary<int, string>
            {
                { Traits.BrandAbbreviation, "TST B ABR" }
            };

            //When
            RunHandleMessageTestForAddOrUpdates(messageText, expectedHierarchyClassId, expectedHierarchyClassName, expectedTraits);
        }

        [TestMethod]
        public void HandleMessage_BrandHierarchyGeneratedFromInfor_ShouldAddOrUpdateBrandHierarchyClass()
        {
            //Given
            var messageText = File.ReadAllText(@"TestMessages/IntegrationBrandMessage2.xml");
            var expectedHierarchyClassId = 40487;
            var expectedHierarchyClassName = "Blakes 2 FAMOUS MINERAL WATER";
            var expectedTraits = new Dictionary<int, string>
            {
                { Traits.BrandAbbreviation, "FMW" }
            };

            //When
            RunHandleMessageTestForAddOrUpdates(messageText, expectedHierarchyClassId, expectedHierarchyClassName, expectedTraits);
        }

        [TestMethod]
        public void HandleMessage_BrandHierarchyGeneratedFromInfor2_ShouldAddOrUpdateBrandHierarchyClass()
        {
            //Given
            var messageText = File.ReadAllText(@"TestMessages/InforMessage.xml");
            var expectedHierarchyClassId = 40487;
            var expectedHierarchyClassName = "blake is awesome";
            var expectedTraits = new Dictionary<int, string>
            {
                { Traits.BrandAbbreviation, "FMW" }
            };

            //When
            RunHandleMessageTestForAddOrUpdates(messageText, expectedHierarchyClassId, expectedHierarchyClassName, expectedTraits);
        }

        [TestMethod]
        public void HandleMessage_BrandHierarchyGeneratedFromInfor3_ShouldAddOrUpdateBrandHierarchyClass()
        {
            //Given
            var messageText = File.ReadAllText(@"TestMessages/IntegrationBrandMessage3.xml");
            var expectedHierarchyClassId = 99997;
            var expectedHierarchyClassName = "xerox";
            var expectedTraits = new Dictionary<int, string>
            {
                { Traits.BrandAbbreviation, "xex" }
            };

            //When
            RunHandleMessageTestForAddOrUpdates(messageText, expectedHierarchyClassId, expectedHierarchyClassName, expectedTraits);
        }

        [TestMethod]
        public void HandleMessage_NationalHierarchy_ShouldAddOrUpdateMerchandiseHierarchyClass()
        {
            //Given
            var messageText = File.ReadAllText(@"TestMessages/IntegrationNationalMessage.xml");
            var expectedHierarchyClassId = 124295;
            var expectedHierarchyClassName = "1.5 Box DELETE TEST";
            var expectedTraits = new Dictionary<int, string>
            {
                { Traits.NationalClassCode, "42770" }
            };

            //When
            RunHandleMessageTestForAddOrUpdates(messageText, expectedHierarchyClassId, expectedHierarchyClassName, expectedTraits);
        }

        [TestMethod]
        public void HandleMessage_FailedInforNationalHierarchyMessage_ShouldAddOrUpdateNationalHierarchyClass()
        {
            //Given
            var messageText = File.ReadAllText(@"TestMessages/FailedInforHierarchyMessage.xml");
            var expectedHierarchyClassId = 4000000;
            var expectedHierarchyClassName = "Scarfs For Infor";
            var expectedTraits = new Dictionary<int, string>
            {
                { Traits.NationalClassCode, "4000000" }
            };

            //When
            RunHandleMessageTestForAddOrUpdates(messageText, expectedHierarchyClassId, expectedHierarchyClassName, expectedTraits);
        }

        [TestMethod]
        public void HandleMessage_DeleteBrandWithNoTrait_ShouldDeleteBrandAndTrait()
        {
            //Given
            var messageText = File.ReadAllText(@"TestMessages/DeleteBrandTestMessage.xml");
            var hierarchyClassId = 10000000;
            var hierarchyId = Hierarchies.Brands;
            var hierarchyClassName = "Test Brand";
            var hierarchyClassLevel = 1;
            int? hierarchyParentClassId = null;
            var hierarchyClassTraits = new Dictionary<int, string>
            {
                { Traits.BrandAbbreviation, "TST B ABR" }
            };

            //When
            RunHandleMessageTestForDeletes(messageText, hierarchyClassId, hierarchyId, hierarchyClassName, hierarchyClassLevel, hierarchyParentClassId, hierarchyClassTraits);
        }

        private void RunHandleMessageTestForAddOrUpdates(string messageText, int expectedHierarchyClassId, string expectedHierarchyClassName, Dictionary<int, string> expectedTraits)
        {
            //Given
            using (IconContext context = new IconContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    var container = Program.CreateHierarchyClassListener();
                    container.Options.AllowOverridingRegistrations = true;
                    container.Register<IHierarchyClassListenerNotifier>(() => new Mock<IHierarchyClassListenerNotifier>().Object);

                    Mock<IEsbMessage> mockMessage = new Mock<IEsbMessage>();
                    mockMessage.SetupGet(m => m.MessageText)
                        .Returns(messageText);
                    mockMessage.Setup(m => m.GetProperty("IconMessageID")).Returns(Guid.NewGuid().ToString());

                    Mock<IRenewableContext<IconContext>> mockContext = new Mock<IRenewableContext<IconContext>>();
                    mockContext.SetupGet(m => m.Context).Returns(context);
                    container.Register<IRenewableContext<IconContext>>(() => mockContext.Object);

                    //When
                    var listener = container.GetInstance<IListenerApplication>() as HierarchyClassListener;
                    listener.HandleMessage(null, new EsbMessageEventArgs { Message = mockMessage.Object });

                    //Then
                    var hierarchyClass = context.HierarchyClass.SingleOrDefault(hc => hc.hierarchyClassID == expectedHierarchyClassId);
                    Assert.IsNotNull(hierarchyClass);
                    Assert.AreEqual(expectedHierarchyClassName, hierarchyClass.hierarchyClassName);
                    foreach (var trait in expectedTraits)
                    {
                        Assert.AreEqual(trait.Value, hierarchyClass.HierarchyClassTrait.First(hct => hct.traitID == trait.Key).traitValue);
                    }

                    var messageHierarchyArchive = context.MessageArchiveHierarchy.FirstOrDefault(hc => hc.HierarchyClassId == expectedHierarchyClassId);
                    Assert.IsNotNull(messageHierarchyArchive);

                    transaction.Rollback();
                }
            }
        }

        private void RunHandleMessageTestForDeletes(string messageText, int hierarchyClassId, int hierarchyId, string hierarchyClassName, int hierarchyClassLevel, int? hierarchyParentClassId, Dictionary<int, string> hierarchyClassTraits)
        {
            //Given
            using (IconContext context = new IconContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    if (!context.HierarchyClass.Any(hc => hc.hierarchyClassID == hierarchyClassId))
                    {
                        context.Database.ExecuteSqlCommand(
                            @"SET IDENTITY_INSERT dbo.HierarchyClass ON

                            insert into dbo.HierarchyClass(hierarchyClassID, hierarchyLevel, hierarchyID, hierarchyParentClassID, hierarchyClassName)
                            values (@hierarchyClassID, @hierarchyLevel, @hierarchyID, @hierarchyParentClassID, @hierarchyClassName)
                            
                            SET IDENTITY_INSERT dbo.HierarchyClass OFF",
                            new SqlParameter("hierarchyClassID", SqlDbType.Int) { Value = hierarchyClassId },
                            new SqlParameter("hierarchyID", SqlDbType.Int) { Value = hierarchyId },
                            new SqlParameter("hierarchyClassName", SqlDbType.NVarChar) { Value = hierarchyClassName },
                            new SqlParameter("hierarchyLevel", SqlDbType.Int) { Value = hierarchyClassLevel },
                            new SqlParameter("hierarchyParentClassID", SqlDbType.Int) { Value = hierarchyParentClassId.HasValue ? (object)hierarchyParentClassId.Value : DBNull.Value });
                    }

                    var hierarchyClass = context.HierarchyClass.First(hc => hc.hierarchyClassID == hierarchyClassId);
                    foreach (var trait in hierarchyClassTraits)
                    {
                        if (!hierarchyClass.HierarchyClassTrait.Any(hct => hct.hierarchyClassID == hierarchyClassId && hct.traitID == trait.Key))
                        {
                            hierarchyClass.HierarchyClassTrait.Add(new HierarchyClassTrait
                            {
                                traitID = trait.Key,
                                traitValue = trait.Value
                            });
                            context.SaveChanges();
                        }
                    }

                    var container = Program.CreateHierarchyClassListener();
                    container.Options.AllowOverridingRegistrations = true;
                    container.Register<IHierarchyClassListenerNotifier>(() => new Mock<IHierarchyClassListenerNotifier>().Object);

                    Mock<IRenewableContext<IconContext>> mockContext = new Mock<IRenewableContext<IconContext>>();
                    mockContext.SetupGet(m => m.Context).Returns(context);
                    container.Register<IRenewableContext<IconContext>>(() => mockContext.Object);

                    Mock<IEsbMessage> mockMessage = new Mock<IEsbMessage>();
                    mockMessage.SetupGet(m => m.MessageText)
                        .Returns(messageText);
                    mockMessage.Setup(m => m.GetProperty("IconMessageID")).Returns(Guid.NewGuid().ToString());


                    //When
                    var listener = container.GetInstance<IListenerApplication>() as HierarchyClassListener;
                    listener.HandleMessage(null, new EsbMessageEventArgs { Message = mockMessage.Object });

                    //Then
                    Assert.IsFalse(context.HierarchyClass.Any(hc => hc.hierarchyClassID == hierarchyClassId));
                    Assert.IsFalse(context.HierarchyClassTrait.Any(hct => hct.hierarchyClassID == hierarchyClassId));

                    var messageHierarchyArchive = context.MessageArchiveHierarchy.FirstOrDefault(hc => hc.HierarchyClassId == hierarchyClassId);
                    Assert.IsNotNull(messageHierarchyArchive);

                    transaction.Rollback();
                }
            }
        }
    }
}
