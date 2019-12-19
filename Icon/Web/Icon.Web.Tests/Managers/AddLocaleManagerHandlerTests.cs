using AutoMapper;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.Common;
using Icon.Web.DataAccess.Commands;
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
    public class AddLocaleManagerHandlerTests
    {
        private AddLocaleManagerHandler managerHandler;
        private AddLocaleManager manager;

        private IconContext context;
        private IMapper mapper;
        private Mock<ICommandHandler<AddLocaleCommand>> addLocaleCommandHandler;
        private Mock<ICommandHandler<AddAddressCommand>> addAddressCommandHandler;
        private Mock<ICommandHandler<AddLocaleMessageCommand>> addLocaleMessageCommandHandler;
        private Mock<ICommandHandler<AddVimEventCommand>> addVimLocaleEventCommandHandler;
        private Mock<IQueryHandler<GetLocaleParameters, List<Locale>>> getLocaleQuery;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            mapper = AutoMapperWebConfiguration.Configure();
            addLocaleCommandHandler = new Mock<ICommandHandler<AddLocaleCommand>>();
            addAddressCommandHandler = new Mock<ICommandHandler<AddAddressCommand>>();
            addLocaleMessageCommandHandler = new Mock<ICommandHandler<AddLocaleMessageCommand>>();
            addVimLocaleEventCommandHandler = new Mock<ICommandHandler<AddVimEventCommand>>();
            getLocaleQuery = new Mock<IQueryHandler<GetLocaleParameters, List<Locale>>>();

            manager = new AddLocaleManager
            {
                LocaleId = 45
            };

            managerHandler = new AddLocaleManagerHandler(context,
                addLocaleCommandHandler.Object,
                addAddressCommandHandler.Object,
                addLocaleMessageCommandHandler.Object,
                addVimLocaleEventCommandHandler.Object,
                getLocaleQuery.Object,
                mapper);
        }

        [TestCleanup]
        public void Cleanup()
        {
            context.Dispose();
        }

        [TestMethod]
        public void AddLocaleManager_ShouldCallCommandHandlers()
        {
            //Given
            getLocaleQuery.Setup(q => q.Search(It.IsAny<GetLocaleParameters>()))
                .Returns(new List<Locale> { new Locale { localeID = 45 } });

            //When
            managerHandler.Execute(manager);

            //Then
            addLocaleCommandHandler.Verify(cm => cm.Execute(It.IsAny<AddLocaleCommand>()));
            addAddressCommandHandler.Verify(cm => cm.Execute(It.IsAny<AddAddressCommand>()));
            addLocaleMessageCommandHandler.Verify(cm => cm.Execute(It.IsAny<AddLocaleMessageCommand>()));
            getLocaleQuery.Verify(q => q.Search(It.Is<GetLocaleParameters>(p => p.LocaleId == manager.LocaleId)));

            addVimLocaleEventCommandHandler.Verify(m => m.Execute(It.Is<AddVimEventCommand>(c =>
                c.EventReferenceId == manager.LocaleId
                && c.EventTypeId == VimEventTypes.LocaleAdd)));
        }

        [TestMethod]
        [ExpectedException(typeof(CommandException), "Test Exception")]
        public void AddLocaleManager_CommandHandlerThrowsArgumentException_ShouldThrowExceptionWithArgumentExceptionMethod()
        {
            //Given
            addLocaleCommandHandler.Setup(cm => cm.Execute(It.IsAny<AddLocaleCommand>()))
                .Throws(new ArgumentException("Test Exception"));

            //When
            managerHandler.Execute(manager);
        }

        [TestMethod]
        public void AddLocaleManager_CommandHandlerThrowsException_ShouldThrowExceptionWithCustomMessage()
        {
            // Given.
            addLocaleCommandHandler.Setup(cm => cm.Execute(It.IsAny<AddLocaleCommand>())).Throws(new Exception("Test Exception"));

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