using Icon.Framework;
using Icon.Logging;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Icon.Web.Tests.Integration.Commands
{
    [TestClass] [Ignore]
    public class BulkImportCertificationAgencyCommandHandlerTests
    {
        private BulkImportCertificationAgencyCommandHandler commandHandler;
        private BulkImportCommand<BulkImportCertificationAgencyModel> command;
        private IconContext context;
        private DbContextTransaction transaction;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            transaction = context.Database.BeginTransaction();
            commandHandler = new BulkImportCertificationAgencyCommandHandler(context, new Mock<ILogger>().Object);
            command = new BulkImportCommand<BulkImportCertificationAgencyModel>
            {
                BulkImportData = new List<BulkImportCertificationAgencyModel>()
            };
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
            transaction.Dispose();
            context.Dispose();
        }

        [TestMethod]
        public void BulkImportCertificationAgency_NewAgencyData_ShouldAddAgency()
        {
            //Given
            command.BulkImportData.Add(new BulkImportCertificationAgencyModel
                {
                    CertificationAgencyId = "0",
                    CertificationAgencyName = "Test",
                    GlutenFree = "1",
                    Kosher = "1",
                    NonGmo = "1",
                    Organic = "1",
                    Vegan = "1"
                });

            //When
            commandHandler.Execute(command);

            //Then
            var agency = context.HierarchyClass
                .AsNoTracking()
                .Include(hc => hc.HierarchyClassTrait)
                .Single(hc => hc.hierarchyClassName == "Test");
            Assert.AreEqual("1", agency.HierarchyClassTrait.Single(hct => hct.traitID == Traits.GlutenFree).traitValue);
            Assert.AreEqual("1", agency.HierarchyClassTrait.Single(hct => hct.traitID == Traits.Kosher).traitValue);
            Assert.AreEqual("1", agency.HierarchyClassTrait.Single(hct => hct.traitID == Traits.NonGmo).traitValue);
            Assert.AreEqual("1", agency.HierarchyClassTrait.Single(hct => hct.traitID == Traits.Organic).traitValue);
            Assert.AreEqual("1", agency.HierarchyClassTrait.Single(hct => hct.traitID == Traits.Vegan).traitValue);
        }

        [TestMethod]
        public void BulkImoprtCertificationAgency_ExistingAgencyData_ShouldUpdateAgency()
        {
            //Given
            var hierarchyClass = new HierarchyClass
                {
                    hierarchyClassName = "Test",
                    hierarchyID = Hierarchies.CertificationAgencyManagement
                };
            context.HierarchyClass.Add(hierarchyClass);
            context.SaveChanges();

            command.BulkImportData.Add(new BulkImportCertificationAgencyModel
            {
                CertificationAgencyId = hierarchyClass.hierarchyClassID.ToString(),
                CertificationAgencyName = "Test",
                GlutenFree = "1",
                Kosher = "1",
                NonGmo = "1",
                Organic = "1",
                Vegan = "1"
            });

            //When
            commandHandler.Execute(command);

            //Then
            var agency = context.HierarchyClass
                .AsNoTracking()
                .Include(hc => hc.HierarchyClassTrait)
                .Single(hc => hc.hierarchyClassName == "Test");
            Assert.AreEqual("1", agency.HierarchyClassTrait.Single(hct => hct.traitID == Traits.GlutenFree).traitValue);
            Assert.AreEqual("1", agency.HierarchyClassTrait.Single(hct => hct.traitID == Traits.Kosher).traitValue);
            Assert.AreEqual("1", agency.HierarchyClassTrait.Single(hct => hct.traitID == Traits.NonGmo).traitValue);
            Assert.AreEqual("1", agency.HierarchyClassTrait.Single(hct => hct.traitID == Traits.Organic).traitValue);
            Assert.AreEqual("1", agency.HierarchyClassTrait.Single(hct => hct.traitID == Traits.Vegan).traitValue);
        }

        [TestMethod]
        public void BulkImportCertificationAgency_MultipleAgencies_ShouldUpdateAgencies()
        {
            //Given
            var hierarchyClass1 = new HierarchyClass
            {
                hierarchyClassName = "Test1",
                hierarchyID = Hierarchies.CertificationAgencyManagement
            }; 
            var hierarchyClass2 = new HierarchyClass
            {
                hierarchyClassName = "Test2",
                hierarchyID = Hierarchies.CertificationAgencyManagement,
                HierarchyClassTrait = new List<HierarchyClassTrait>
                        {
                            new HierarchyClassTrait { traitID = Traits.GlutenFree, traitValue = "1" },
                            new HierarchyClassTrait { traitID = Traits.Kosher, traitValue = "1" },
                            new HierarchyClassTrait { traitID = Traits.NonGmo, traitValue = "1" },
                            new HierarchyClassTrait { traitID = Traits.Organic, traitValue = "1" },
                            new HierarchyClassTrait { traitID = Traits.Vegan, traitValue = "1" }
                        }
            }; 
            var hierarchyClass3 = new HierarchyClass
            {
                hierarchyClassName = "Test3",
                hierarchyID = Hierarchies.CertificationAgencyManagement,
                HierarchyClassTrait = new List<HierarchyClassTrait>
                        {
                            new HierarchyClassTrait { traitID = Traits.GlutenFree, traitValue = "1" },
                            new HierarchyClassTrait { traitID = Traits.Kosher, traitValue = "1" },
                            new HierarchyClassTrait { traitID = Traits.NonGmo, traitValue = "1" },
                            new HierarchyClassTrait { traitID = Traits.Organic, traitValue = "1" },
                        }
            };
            context.HierarchyClass.Add(hierarchyClass1);
            context.HierarchyClass.Add(hierarchyClass2);
            context.HierarchyClass.Add(hierarchyClass3);
            context.SaveChanges();

            command.BulkImportData.Add(new BulkImportCertificationAgencyModel
            {
                CertificationAgencyId = hierarchyClass1.hierarchyClassID.ToString(),
                CertificationAgencyName = "Test1",
                GlutenFree = "1",
                Kosher = "1",
                NonGmo = "1",
                Organic = "1",
                Vegan = "1"
            });
            command.BulkImportData.Add(new BulkImportCertificationAgencyModel
            {
                CertificationAgencyId = hierarchyClass2.hierarchyClassID.ToString(),
                CertificationAgencyName = "Test2",
                GlutenFree = "0",
                Kosher = "0",
                NonGmo = "0",
                Organic = "0",
                Vegan = "0"
            });
            command.BulkImportData.Add(new BulkImportCertificationAgencyModel
            {
                CertificationAgencyId = hierarchyClass3.hierarchyClassID.ToString(),
                CertificationAgencyName = "Test3",
                GlutenFree = "1",
                Kosher = "",
                NonGmo = "",
                Organic = "1",
                Vegan = "1"
            });

            //When
            commandHandler.Execute(command);

            //Then
            var agency1 = context.HierarchyClass
                .AsNoTracking()
                .Include(hc => hc.HierarchyClassTrait)
                .Single(hc => hc.hierarchyClassName == "Test1");
            Assert.AreEqual("1", agency1.HierarchyClassTrait.Single(hct => hct.traitID == Traits.GlutenFree).traitValue);
            Assert.AreEqual("1", agency1.HierarchyClassTrait.Single(hct => hct.traitID == Traits.Kosher).traitValue);
            Assert.AreEqual("1", agency1.HierarchyClassTrait.Single(hct => hct.traitID == Traits.NonGmo).traitValue);
            Assert.AreEqual("1", agency1.HierarchyClassTrait.Single(hct => hct.traitID == Traits.Organic).traitValue);
            Assert.AreEqual("1", agency1.HierarchyClassTrait.Single(hct => hct.traitID == Traits.Vegan).traitValue);

            var agency2 = context.HierarchyClass
                .AsNoTracking()
                .Include(hc => hc.HierarchyClassTrait)
                .Single(hc => hc.hierarchyClassName == "Test2");
            Assert.IsFalse(agency2.HierarchyClassTrait.Any(hct => hct.traitID == Traits.GlutenFree));
            Assert.IsFalse(agency2.HierarchyClassTrait.Any(hct => hct.traitID == Traits.Kosher));
            Assert.IsFalse(agency2.HierarchyClassTrait.Any(hct => hct.traitID == Traits.NonGmo));
            Assert.IsFalse(agency2.HierarchyClassTrait.Any(hct => hct.traitID == Traits.Organic));
            Assert.IsFalse(agency2.HierarchyClassTrait.Any(hct => hct.traitID == Traits.Vegan));

            var agency3 = context.HierarchyClass
                .AsNoTracking()
                .Include(hc => hc.HierarchyClassTrait)
                .Single(hc => hc.hierarchyClassName == "Test3");
            Assert.AreEqual("1", agency3.HierarchyClassTrait.Single(hct => hct.traitID == Traits.GlutenFree).traitValue);
            Assert.AreEqual("1", agency3.HierarchyClassTrait.Single(hct => hct.traitID == Traits.Kosher).traitValue);
            Assert.AreEqual("1", agency3.HierarchyClassTrait.Single(hct => hct.traitID == Traits.NonGmo).traitValue);
            Assert.AreEqual("1", agency3.HierarchyClassTrait.Single(hct => hct.traitID == Traits.Organic).traitValue);
            Assert.AreEqual("1", agency3.HierarchyClassTrait.Single(hct => hct.traitID == Traits.Vegan).traitValue);
        }

        [TestMethod]
        public void BulkImoprtCertificationAgency_DifferentNameThanId_ShouldNotChangeName()
        {
            //Given
            var hierarchyClass = new HierarchyClass
            {
                hierarchyClassName = "Test",
                hierarchyID = Hierarchies.CertificationAgencyManagement
            };
            context.HierarchyClass.Add(hierarchyClass);
            context.SaveChanges();

            command.BulkImportData.Add(new BulkImportCertificationAgencyModel
            {
                CertificationAgencyId = hierarchyClass.hierarchyClassID.ToString(),
                CertificationAgencyName = "New Name",
                GlutenFree = "1",
                Kosher = "1",
                NonGmo = "1",
                Organic = "1",
                Vegan = "1"
            });

            //When
            commandHandler.Execute(command);

            //Then
            var agency = context.HierarchyClass
                .AsNoTracking()
                .Include(hc => hc.HierarchyClassTrait)
                .Single(hc => hc.hierarchyClassID == hierarchyClass.hierarchyClassID);
            Assert.AreEqual(hierarchyClass.hierarchyClassName, agency.hierarchyClassName);
            Assert.AreEqual("1", agency.HierarchyClassTrait.Single(hct => hct.traitID == Traits.GlutenFree).traitValue);
            Assert.AreEqual("1", agency.HierarchyClassTrait.Single(hct => hct.traitID == Traits.Kosher).traitValue);
            Assert.AreEqual("1", agency.HierarchyClassTrait.Single(hct => hct.traitID == Traits.NonGmo).traitValue);
            Assert.AreEqual("1", agency.HierarchyClassTrait.Single(hct => hct.traitID == Traits.Organic).traitValue);
            Assert.AreEqual("1", agency.HierarchyClassTrait.Single(hct => hct.traitID == Traits.Vegan).traitValue);
        }
    }
}
