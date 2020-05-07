﻿using AutoMapper;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Managers;
using Icon.Web.Mvc.App_Start;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace Icon.Web.Tests.Unit.ManagerHandlers
{
    [TestClass]
    public class DeleteHierarchyClassManagerHandlerTests
    {
        private DeleteHierarchyClassManagerHandler managerHandler;
        private DeleteHierarchyClassManager manager;

        private IconContext context;
        private Mock<ICommandHandler<DeleteHierarchyClassCommand>> deleteHierarchyClassHandler;
        private Mock<ICommandHandler<AddHierarchyClassMessageCommand>> addHierarchyClassMessageHandler;
        private IMapper mapper;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            mapper = AutoMapperWebConfiguration.Configure();
            deleteHierarchyClassHandler = new Mock<ICommandHandler<DeleteHierarchyClassCommand>>();
            addHierarchyClassMessageHandler = new Mock<ICommandHandler<AddHierarchyClassMessageCommand>>();
            
            manager = new DeleteHierarchyClassManager
                {
                    DeletedHierarchyClass = new HierarchyClass
                    {
                        hierarchyClassName = "Test Delete HierarchyClass",
                        hierarchyClassID = 45,
                        hierarchyID = Hierarchies.Financial
                    }
                };

            managerHandler = new DeleteHierarchyClassManagerHandler(context,
                deleteHierarchyClassHandler.Object,
                addHierarchyClassMessageHandler.Object,
                mapper);
        }

        [TestCleanup]
        public void Cleanup()
        {
            context.Dispose();
        }

        [TestMethod]
        public void DeleteHierarchyClassManager_ShouldCallCommandHandlers()
        {
            //When
            manager.EnableHierarchyMessages = true;
            managerHandler.Execute(manager);

            //Then
            deleteHierarchyClassHandler.Verify(cm => cm.Execute(It.Is<DeleteHierarchyClassCommand>(c => 
                c.DeletedHierarchyClass.hierarchyClassName == manager.DeletedHierarchyClass.hierarchyClassName &&
                c.DeletedHierarchyClass.hierarchyClassID == manager.DeletedHierarchyClass.hierarchyClassID &&
                c.DeletedHierarchyClass.hierarchyID == manager.DeletedHierarchyClass.hierarchyID)));
            addHierarchyClassMessageHandler.Verify(cm => cm.Execute(It.Is<AddHierarchyClassMessageCommand>(c =>
                c.HierarchyClass.hierarchyClassName == manager.DeletedHierarchyClass.hierarchyClassName &&
                c.HierarchyClass.hierarchyClassID == manager.DeletedHierarchyClass.hierarchyClassID &&
                c.HierarchyClass.hierarchyID == manager.DeletedHierarchyClass.hierarchyID &&
                c.ClassNameChange &&
                c.DeleteMessage)));
        }

        [TestMethod]
        public void DeleteHierarchyClassManager_CommandHandlerThrowsException_ShouldThrowExceptionWithCustomMessage()
        {
            //Given
            deleteHierarchyClassHandler.Setup(cm => cm.Execute(It.IsAny<DeleteHierarchyClassCommand>()))
                .Throws(new Exception());

            //When
            try
            {
                managerHandler.Execute(manager);
            }
            catch (Exception e)
            {
                //Then
                Assert.IsTrue(e.Message.StartsWith("There was an error deleting hierarchyClassId"));
            }
        }
    }
}
