using AutoMapper;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.Common;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Managers;
using Icon.Web.Mvc.App_Start;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace Icon.Web.Tests.Unit.ManagerHandlers
{
    [TestClass]
    public class UpdateHierarchyClassManagerHandlerTests
    {
        private UpdateHierarchyClassManagerHandler managerHandler;
        private UpdateHierarchyClassManager manager;

        private IconContext context;
        private Mock<ICommandHandler<UpdateHierarchyClassCommand>> updateHierarchyClassCommandHandler;
        private Mock<ICommandHandler<UpdateHierarchyClassTraitCommand>> updateHierarchyClassTraitCommandHandler;
        private Mock<ICommandHandler<AddTaxEventCommand>> addTaxEventCommandHandler;
        private Mock<ICommandHandler<AddHierarchyClassMessageCommand>> addHierarchyClassMessageCommandHandler;
        private IMapper mapper;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            mapper = AutoMapperWebConfiguration.Configure();

            updateHierarchyClassCommandHandler = new Mock<ICommandHandler<UpdateHierarchyClassCommand>>();
            updateHierarchyClassTraitCommandHandler = new Mock<ICommandHandler<UpdateHierarchyClassTraitCommand>>();
            addTaxEventCommandHandler = new Mock<ICommandHandler<AddTaxEventCommand>>();
            addHierarchyClassMessageCommandHandler = new Mock<ICommandHandler<AddHierarchyClassMessageCommand>>();
            managerHandler = new UpdateHierarchyClassManagerHandler(context,
                updateHierarchyClassCommandHandler.Object,
                updateHierarchyClassTraitCommandHandler.Object,
                addTaxEventCommandHandler.Object,
                addHierarchyClassMessageCommandHandler.Object,
                mapper);

            manager = new UpdateHierarchyClassManager
            {
                UpdatedHierarchyClass = new HierarchyClass 
                { 
                    hierarchyClassID = 55,
                    hierarchyClassName = "Test Update HierarchyClass",
                    hierarchyID = Hierarchies.Financial
                }, 
                NonMerchandiseTrait = "Test Non Merch Trait",
                SubTeamHierarchyClassId = 32,
                TaxAbbreviation = "Test Tax Abb"
            };

            AutoMapperWebConfiguration.Configure();
        }

        [TestCleanup]
        public void Cleanup()
        {
            context.Dispose();
        }

        [TestMethod]
        public void UpdateHierarchyClassManager_ShouldCallCommandHandlers()
        {
            //Given
            //Setting up the command handler expectation here because Moq seems to have a bug when verifying a parameter that you've updated from a callback.
            updateHierarchyClassCommandHandler.Setup(ch => ch.Execute(It.Is<UpdateHierarchyClassCommand>(c =>    
                c.SubTeamHierarchyClassId == manager.SubTeamHierarchyClassId &&
                c.TaxAbbreviation == manager.TaxAbbreviation &&
                c.UpdatedHierarchyClass.hierarchyClassName == manager.UpdatedHierarchyClass.hierarchyClassName &&
                !c.ClassNameChanged)))
                .Callback<UpdateHierarchyClassCommand>(c => c.ClassNameChanged = true)
                .Verifiable();

            //When
            managerHandler.Execute(manager);

            //Then
            updateHierarchyClassCommandHandler.Verify();
            updateHierarchyClassTraitCommandHandler.Verify(cm => cm.Execute(It.Is<UpdateHierarchyClassTraitCommand>(c =>
                c.NonMerchandiseTrait == manager.NonMerchandiseTrait &&
                c.SubTeamHierarchyClassId == manager.SubTeamHierarchyClassId &&
                c.TaxAbbreviation == manager.TaxAbbreviation &&
                c.UpdatedHierarchyClass.hierarchyClassName == manager.UpdatedHierarchyClass.hierarchyClassName)));
            addTaxEventCommandHandler.Verify(cm => cm.Execute(It.Is<AddTaxEventCommand>(c =>
                c.HierarchyClassId == manager.UpdatedHierarchyClass.hierarchyClassID &&
                c.TaxAbbreviation == manager.TaxAbbreviation)));
            addHierarchyClassMessageCommandHandler.Verify(cm => cm.Execute(It.Is<AddHierarchyClassMessageCommand>(c => 
                c.ClassNameChange &&
                !c.DeleteMessage &&
                c.HierarchyClass.hierarchyClassName == manager.UpdatedHierarchyClass.hierarchyClassName)));
        }

        [TestMethod]
        [ExpectedException(typeof(CommandException), "Test Exception")]
        public void UpdateHierarchyClassManager_CommandHandlerThrowsHierarchyClassTraitUpdateException_ShouldThrowExceptionWithHierarchyClassTraitUpdateExceptionMessage()
        {
            //Given
            updateHierarchyClassCommandHandler.Setup(cm => cm.Execute(It.IsAny<UpdateHierarchyClassCommand>()))
                .Throws(new HierarchyClassTraitUpdateException("Test Exception"));

            //When
            managerHandler.Execute(manager);
        }

        [TestMethod]
        [ExpectedException(typeof(CommandException), "Test Exception")]
        public void UpdateHierarchyClassManager_CommandHandlerThrowsArgumentException_ShouldThrowExceptionWithArgumentExceptionMessage()
        {
            //Given
            updateHierarchyClassCommandHandler.Setup(cm => cm.Execute(It.IsAny<UpdateHierarchyClassCommand>()))
                .Throws(new ArgumentException("Test Exception"));

            //When
            managerHandler.Execute(manager);
        }

        [TestMethod]
        public void UpdateHierarchyClassManager_CommandHandlerThrowsException_ShouldThrowExceptionWithCustomMessage()
        {
            //Given
            updateHierarchyClassCommandHandler.Setup(cm => cm.Execute(It.IsAny<UpdateHierarchyClassCommand>()))
                .Throws(new Exception("Test Exception"));

            //When
            try
            {
                managerHandler.Execute(manager);
            }
            catch (Exception e)
            {
                //Then
                Assert.IsTrue(e.Message.StartsWith("There was an error updating HierarchyClassID"));
            }
        }
    }
}
