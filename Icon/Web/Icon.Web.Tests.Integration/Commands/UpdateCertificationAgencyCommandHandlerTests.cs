using Icon.Framework;
using Icon.Testing.Builders;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.Tests.Integration.Commands
{
    [TestClass]
    public class UpdateCertificationAgencyCommandHandlerTests
    {
        private UpdateCertificationAgencyCommandHandler commandHandler;
        private UpdateCertificationAgencyCommand command;
        private IconContext context;
        private DbContextTransaction transaction;
        private HierarchyClass certificationAgency;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            transaction = context.Database.BeginTransaction();
            commandHandler = new UpdateCertificationAgencyCommandHandler(this.context);
            command = new UpdateCertificationAgencyCommand();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
            transaction.Dispose();
            context.Dispose();
        }

        [TestMethod]
        public void UpdateCertificationAgencyCommand_AgencyExists_ShouldUpdateAgency()
        {
            //Given
            certificationAgency = new TestHierarchyClassBuilder()
                .WithHierarchyClassName("Test Cert Agency")
                .WithHierarchyId(Hierarchies.CertificationAgencyManagement);
            context.HierarchyClass.Add(certificationAgency);
            context.SaveChanges();

            command.Agency = new TestHierarchyClassBuilder()
                .WithHierarchyClassId(certificationAgency.hierarchyClassID)
                .WithHierarchyClassName("Test Cert Agency 2");
            command.GlutenFree = "1";
            command.Kosher = "1";
            command.NonGMO = "1";
            command.Organic = "1";
            command.Vegan = "1";

            //When
            commandHandler.Execute(command);

            //Then
            var result = context.HierarchyClass
                .AsNoTracking()
                .Include(hc => hc.HierarchyClassTrait)
                .First(hc => hc.hierarchyClassID == certificationAgency.hierarchyClassID);
            Assert.AreEqual("Test Cert Agency 2", result.hierarchyClassName);
            Assert.AreEqual("1", result.HierarchyClassTrait.Single(hct => hct.traitID == Traits.GlutenFree).traitValue);
            Assert.AreEqual("1", result.HierarchyClassTrait.Single(hct => hct.traitID == Traits.GlutenFree).traitValue);
            Assert.AreEqual("1", result.HierarchyClassTrait.Single(hct => hct.traitID == Traits.GlutenFree).traitValue);
            Assert.AreEqual("1", result.HierarchyClassTrait.Single(hct => hct.traitID == Traits.GlutenFree).traitValue);
            Assert.AreEqual("1", result.HierarchyClassTrait.Single(hct => hct.traitID == Traits.GlutenFree).traitValue);
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateValueException))]
        public void UpdateCertificationAgencyCommand_DuplicateNameExists_ShouldThrowException()
        {
            //Given
            certificationAgency = new TestHierarchyClassBuilder()
                .WithHierarchyClassName("Test Cert Agency")
                .WithHierarchyId(Hierarchies.CertificationAgencyManagement);
            context.HierarchyClass.Add(certificationAgency);
            context.SaveChanges();
            var duplicateCertificationAgency = new TestHierarchyClassBuilder()
                .WithHierarchyClassName("Test Cert Agency 2")
                .WithHierarchyId(Hierarchies.CertificationAgencyManagement)
                .Build();
            context.HierarchyClass.Add(duplicateCertificationAgency);
            context.SaveChanges();

            command.Agency = new TestHierarchyClassBuilder()
                .WithHierarchyClassId(certificationAgency.hierarchyClassID)
                .WithHierarchyClassName("Test Cert Agency 2");
            command.GlutenFree = "1";
            command.Kosher = "1";
            command.NonGMO = "1";
            command.Organic = "1";
            command.Vegan = "1";

            //When
            commandHandler.Execute(command);
        }

        [TestMethod]
        [ExpectedException(typeof(HierarchyClassTraitUpdateException))]
        public void UpdateCertificationAgencyCommand_RemovingGlutenFreeFromAgencyButAgencyIsAssociatedToItemAsGlutenFree_ShouldThrowExceptionBecauseAgencyIsAssociatedToItem()
        {
            //Given
            certificationAgency = new TestHierarchyClassBuilder()
                .WithHierarchyClassName("Test Cert Agency")
                .WithHierarchyId(Hierarchies.CertificationAgencyManagement)
                .WithGlutenFreeTrait("1");
            context.HierarchyClass.Add(certificationAgency);
            context.SaveChanges();

            Item item = new Item { itemTypeID = ItemTypes.RetailSale, ItemSignAttribute = new List<ItemSignAttribute> { new ItemSignAttribute { GlutenFreeAgencyId = certificationAgency.hierarchyClassID } } };
            context.Item.Add(item);
            context.SaveChanges();

            command.Agency = certificationAgency;
            command.GlutenFree = "0";
            command.Kosher = "0";
            command.NonGMO = "0";
            command.Organic = "0";
            command.Vegan = "0";

            //When
            commandHandler.Execute(command);
        }
    }
}
