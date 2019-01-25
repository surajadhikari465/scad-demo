using AutoMapper;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.Common;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Managers;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.App_Start;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace Icon.Web.Tests.Unit.ManagerHandlers
{
    [TestClass]
    public class AddVenueManagerHandlerTests
    {
        private AddVenueManagerHandler managerHandler;
        private AddVenueManager manager;

        private IconContext context;
        private Mock<ICommandHandler<AddVenueCommand>> addVenueCommandHandler;
        private Mock<ICommandHandler<AddAddressCommand>> addAddressCommandHandler;
        private Mock<ICommandHandler<AddLocaleMessageCommand>> addLocaleMessageCommandHandler;
        private Mock<IQueryHandler<GetLocaleParameters, List<Locale>>> getLocaleQuery;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            addVenueCommandHandler = new Mock<ICommandHandler<AddVenueCommand>>();
            addLocaleMessageCommandHandler = new Mock<ICommandHandler<AddLocaleMessageCommand>>();
            getLocaleQuery = new Mock<IQueryHandler<GetLocaleParameters, List<Locale>>>();

            manager = new AddVenueManager
            {
                LocaleId = 45
            };

            managerHandler = new AddVenueManagerHandler(context,
                addVenueCommandHandler.Object,
                addLocaleMessageCommandHandler.Object,
                getLocaleQuery.Object);

            AutoMapperWebConfiguration.Configure();
        }

        [TestCleanup]
        public void Cleanup()
        {
            context.Dispose();
            Mapper.Reset();
        }

        [TestMethod]
        public void AddLocaleVenueManager_ShouldCallCommandHandlers()
        {
            //Given
            getLocaleQuery.Setup(q => q.Search(It.IsAny<GetLocaleParameters>()))
                .Returns(new List<Locale> { new Locale { localeID = 45 } });

            //When
            managerHandler.Execute(manager);

            //Then
            addVenueCommandHandler.Verify(cm => cm.Execute(It.IsAny<AddVenueCommand>()));
         
            addLocaleMessageCommandHandler.Verify(cm => cm.Execute(It.IsAny<AddLocaleMessageCommand>()));
            getLocaleQuery.Verify(q => q.Search(It.Is<GetLocaleParameters>(p => p.LocaleId == manager.LocaleId)));
         
        }

        [TestMethod]
        [ExpectedException(typeof(CommandException), "Test Exception")]
        public void AddLocaleManager_CommandHandlerThrowsArgumentException_ShouldThrowExceptionWithArgumentExceptionMethod()
        {
            //Given
            addVenueCommandHandler.Setup(cm => cm.Execute(It.IsAny<AddVenueCommand>()))
                .Throws(new ArgumentException("Test Exception"));

            //When
            managerHandler.Execute(manager);
        }

        [TestMethod]
        public void AddVenueManager_CommandHandlerThrowsException_ShouldThrowExceptionWithCustomMessage()
        {
            // Given.
            addVenueCommandHandler.Setup(cm => cm.Execute(It.IsAny<AddVenueCommand>()))
                .Throws(new Exception("Test Exception"));

            // When.
            try
            {
                managerHandler.Execute(manager);
            }
            catch (CommandException e)
            {
                // Then.
                Assert.IsTrue(e.Message.StartsWith("Error adding Locale"));
            }
        }
    }
}