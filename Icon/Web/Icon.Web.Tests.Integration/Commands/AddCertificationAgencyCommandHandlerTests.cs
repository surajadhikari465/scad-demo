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
    [TestClass] [Ignore]
    public class AddCertificationAgencyCommandHandlerTests
    {
        private AddCertificationAgencyCommandHandler commandHandler;
        private AddCertificationAgencyCommand command;
        private IconContext context;
        private DbContextTransaction transaction;
        private HierarchyClass certificationAgency;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            transaction = context.Database.BeginTransaction();
            commandHandler = new AddCertificationAgencyCommandHandler(this.context);
            command = new AddCertificationAgencyCommand();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
            transaction.Dispose();
            context.Dispose();
        }

        [TestMethod]
        public void AddCertificationAgencyCommand_AgencyDoesntExist_ShouldAddAgency()
        {
            //Given
            command.Agency = new TestHierarchyClassBuilder()
                .WithHierarchyClassName("Test Cert Agency");
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
                .First(hc => hc.hierarchyClassName == command.Agency.hierarchyClassName);
            Assert.AreEqual("Test Cert Agency", result.hierarchyClassName);
            Assert.AreEqual("1", result.HierarchyClassTrait.Single(hct => hct.traitID == Traits.GlutenFree).traitValue);
            Assert.AreEqual("1", result.HierarchyClassTrait.Single(hct => hct.traitID == Traits.GlutenFree).traitValue);
            Assert.AreEqual("1", result.HierarchyClassTrait.Single(hct => hct.traitID == Traits.GlutenFree).traitValue);
            Assert.AreEqual("1", result.HierarchyClassTrait.Single(hct => hct.traitID == Traits.GlutenFree).traitValue);
            Assert.AreEqual("1", result.HierarchyClassTrait.Single(hct => hct.traitID == Traits.GlutenFree).traitValue);
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateValueException))]
        public void AddCertificationAgencyCommand_DuplicateNameExists_ShouldThrowException()
        {
            //Given
            certificationAgency = new TestHierarchyClassBuilder()
                .WithHierarchyClassName("Test Cert Agency")
                .WithHierarchyId(Hierarchies.CertificationAgencyManagement);
            context.HierarchyClass.Add(certificationAgency);
            context.SaveChanges();

            command.Agency = new TestHierarchyClassBuilder()
                .WithHierarchyClassName("Test Cert Agency");

            //When
            commandHandler.Execute(command);
        }
    }
}
