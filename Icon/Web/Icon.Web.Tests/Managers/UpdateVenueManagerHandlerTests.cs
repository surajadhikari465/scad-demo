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
    public class UpdateVenueManagerHandlerTests
    {
        private UpdateVenueManagerHandler managerHandler;
        private UpdateVenueManager manager;

        private IconContext context;
        private IMapper mapper;
        private Mock<ICommandHandler<UpdateVenueCommand>> updateVenueHandler;
        private Mock<ICommandHandler<AddLocaleMessageCommand>> addLocaleMessageHandler;
        private Mock<IQueryHandler<GetLocaleParameters, List<Locale>>> getLocaleQuery;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            mapper = AutoMapperWebConfiguration.Configure();
            updateVenueHandler = new Mock<ICommandHandler<UpdateVenueCommand>>();
            addLocaleMessageHandler = new Mock<ICommandHandler<AddLocaleMessageCommand>>();
            getLocaleQuery = new Mock<IQueryHandler<GetLocaleParameters, List<Locale>>>();

            manager = new UpdateVenueManager
            {

                LocaleId = 12,
                LocaleName = "Test LocaleName",
                ParentLocaleId = 14,
                LocaleTypeId = LocaleTypes.Venue,
                OpenDate = DateTime.Now,
                CloseDate = DateTime.Now,
                LocaleSubType = "Hospitality",
                VenueCode = "Test Venue Code",
                VenueOccupant = "Test Venye Occupant",
                LocaleSubTypeId = 1,
                UserName = "Test",
            };

            managerHandler = new UpdateVenueManagerHandler(context,
                updateVenueHandler.Object,
                addLocaleMessageHandler.Object,
                getLocaleQuery.Object,
                mapper);
        }

        [TestMethod]
        public void UpdateVenueManager_ShouldCallCommandHandlers()
        {
            // Given.
            getLocaleQuery.Setup(q => q.Search(It.IsAny<GetLocaleParameters>()))
                .Returns(new List<Locale> { new Locale { localeID = manager.LocaleId } });

            // When.
            managerHandler.Execute(manager);

            // Then.
            updateVenueHandler.Verify(cm => cm.Execute(It.IsAny<UpdateVenueCommand>()));
            updateVenueHandler.Verify(cm => cm.Execute(It.Is<UpdateVenueCommand>(c =>               
                c.CloseDate == manager.CloseDate &&
                c.VenueCode == manager.VenueCode &&
                c.VenueOccupant == manager.VenueOccupant &&
                c.LocaleSubType == manager.LocaleSubType &&
                c.LocaleId == manager.LocaleId &&
                c.LocaleName == manager.LocaleName &&
                c.OpenDate == manager.OpenDate
                )));

            getLocaleQuery.Verify(q => q.Search(It.Is<GetLocaleParameters>(p => p.LocaleId == manager.LocaleId)));

            addLocaleMessageHandler.Verify(cm => cm.Execute(It.Is<AddLocaleMessageCommand>(c =>
                c.Locale.localeID == manager.LocaleId)));
        }

        [TestMethod]
        [ExpectedException(typeof(CommandException), "Test Exception")]
        public void UpdateVenueManager_CommandHandlerThrowsArgumentException_ShouldThrowExceptionWithArgumentExceptionMessage()
        {
            // Given.
            updateVenueHandler.Setup(cm => cm.Execute(It.IsAny<UpdateVenueCommand>())).Throws(new ArgumentException("Test Exception"));

            // When.
            managerHandler.Execute(manager);

            // Then.
            // Expected exception.
        }

        [TestMethod]
        public void UpdateVenueManager_CommandHandlerThrowsException_ShouldThrowExceptionWithCustomMessage()
        {
            // Given
            updateVenueHandler.Setup(cm => cm.Execute(It.IsAny<UpdateVenueCommand>())).Throws(new Exception("Test Exception"));

            // When.
            try
            {
                managerHandler.Execute(manager);
            }
            catch (Exception e)
            {
                // Then.
                Assert.IsTrue(e.Message.StartsWith("Error updating Locale"));
            }
        }
    }
}