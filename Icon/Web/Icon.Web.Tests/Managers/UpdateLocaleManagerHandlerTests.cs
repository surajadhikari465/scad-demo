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
    [TestClass] [Ignore]
    public class UpdateLocaleManagerHandlerTests
    {
        private UpdateLocaleManagerHandler managerHandler;
        private UpdateLocaleManager manager;

        private IconContext context;
        private Mock<ICommandHandler<UpdateLocaleCommand>> updateLocaleHandler;
        private Mock<ICommandHandler<AddLocaleMessageCommand>> addLocaleMessageHandler;
        private Mock<ICommandHandler<AddVimEventCommand>> addVimLocaleEventCommandHandler;
        private Mock<IQueryHandler<GetLocaleParameters, List<Locale>>> getLocaleQuery;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            updateLocaleHandler = new Mock<ICommandHandler<UpdateLocaleCommand>>();
            addLocaleMessageHandler = new Mock<ICommandHandler<AddLocaleMessageCommand>>();
            addVimLocaleEventCommandHandler = new Mock<ICommandHandler<AddVimEventCommand>>();
            getLocaleQuery = new Mock<IQueryHandler<GetLocaleParameters, List<Locale>>>();

            manager = new UpdateLocaleManager
            {
                AddressId = 10,
                AddressLine1 = "Test Address Line 1",
                AddressLine2 = "Test Address Line 2",
                AddressLine3 = "Test Address Line 3",
                BusinessUnitId = "Test BusinessUnitId",
                CityName = "Test City Name",
                CloseDate = DateTime.Now,
                ContactPerson = "Test Contact Person",
                CountryId = 11,
                CountyName = "Test Country Name",
                Latitude = 1.1m,
                LocaleId = 12,
                LocaleName = "Test LocaleName",
                LocaleTypeId = LocaleTypes.Metro,
                Longitude = 2.2m,
                OpenDate = DateTime.Now,
                OwnerOrgPartyId = 13,
                ParentLocaleId = 14,
                PhoneNumber = "Test Phone Number",
                PostalCode = "Test Postal Code",
                StoreAbbreviation = "Test Store Abbreviation",
                TerritoryId = 15,
                TimezoneId = 16,
                EwicAgencyId = "ZZ"
            };

            managerHandler = new UpdateLocaleManagerHandler(context,
                updateLocaleHandler.Object,
                addLocaleMessageHandler.Object,
                addVimLocaleEventCommandHandler.Object,
                getLocaleQuery.Object);

            AutoMapperWebConfiguration.Configure();
        }

        [TestCleanup]
        public void Cleanup()
        {
            Mapper.Reset();
        }

        [TestMethod]
        public void UpdateLocaleManager_ShouldCallCommandHandlers()
        {
            // Given.
            getLocaleQuery.Setup(q => q.Search(It.IsAny<GetLocaleParameters>())).Returns(new List<Locale> { new Locale { localeID = manager.LocaleId } });

            // When.
            managerHandler.Execute(manager);

            // Then.
            updateLocaleHandler.Verify(cm => cm.Execute(It.Is<UpdateLocaleCommand>(c =>
                c.AddressId == manager.AddressId &&
                c.AddressLine1 == manager.AddressLine1 &&
                c.AddressLine2 == manager.AddressLine2 &&
                c.AddressLine3 == manager.AddressLine3 &&
                c.BusinessUnitId == manager.BusinessUnitId &&
                c.CityName == manager.CityName &&
                c.CloseDate == manager.CloseDate &&
                c.ContactPerson == manager.ContactPerson &&
                c.CountryId == manager.CountryId &&
                c.CountyName == manager.CountyName &&
                c.Latitude == manager.Latitude &&
                c.LocaleId == manager.LocaleId &&
                c.LocaleName == manager.LocaleName &&
                c.Longitude == manager.Longitude &&
                c.OpenDate == manager.OpenDate &&
                c.PhoneNumber == manager.PhoneNumber &&
                c.PostalCode == manager.PostalCode &&
                c.StoreAbbreviation == manager.StoreAbbreviation &&
                c.TerritoryId == manager.TerritoryId &&
                c.TimezoneId == manager.TimezoneId &&
                c.EwicAgencyId == manager.EwicAgencyId)));

            getLocaleQuery.Verify(q => q.Search(It.Is<GetLocaleParameters>(p => p.LocaleId == manager.LocaleId)));

            addLocaleMessageHandler.Verify(cm => cm.Execute(It.Is<AddLocaleMessageCommand>(c =>
                c.Locale.localeID == manager.LocaleId)));

            addVimLocaleEventCommandHandler.Verify(m => m.Execute(It.Is<AddVimEventCommand>(c =>
                c.EventReferenceId == manager.LocaleId
                && c.EventTypeId == VimEventTypes.LocaleUpdate)));
        }

        [TestMethod]
        [ExpectedException(typeof(CommandException), "Test Exception")]
        public void UpdateLocaleManager_CommandHandlerThrowsArgumentException_ShouldThrowExceptionWithArgumentExceptionMessage()
        {
            // Given.
            updateLocaleHandler.Setup(cm => cm.Execute(It.IsAny<UpdateLocaleCommand>())).Throws(new ArgumentException("Test Exception"));

            // When.
            managerHandler.Execute(manager);

            // Then.
            // Expected exception.
        }

        [TestMethod]
        public void UpdateLocaleManager_CommandHandlerThrowsException_ShouldThrowExceptionWithCustomMessage()
        {
            //Given
            updateLocaleHandler.Setup(cm => cm.Execute(It.IsAny<UpdateLocaleCommand>())).Throws(new Exception("Test Exception"));

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