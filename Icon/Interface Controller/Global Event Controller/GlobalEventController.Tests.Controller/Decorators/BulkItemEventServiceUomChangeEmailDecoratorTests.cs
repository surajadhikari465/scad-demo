using GlobalEventController.Common;
using GlobalEventController.Controller.Decorators;
using GlobalEventController.Controller.EventServices;
using GlobalEventController.DataAccess.DataServices;
using GlobalEventController.DataAccess.Infrastructure;
using GlobalEventController.DataAccess.Queries;
using GlobalEventController.Testing.Common;
using Icon.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace GlobalEventController.Tests.Controller.Decorators
{
    [TestClass]
    public class BulkItemEventServiceUomChangeEmailDecoratorTests
    {
        private Mock<IBulkEventService> mockBulkService;
        private Mock<IEmailUomChangeService> mockEmailService;
        private Mock<IQueryHandler<GetItemsByScanCodeQuery, List<IrmaItemModel>>> mockGetItemsByScanCodeQuery;
        private Mock<IGlobalControllerSettings> mockSettings;
        private BulkItemEventServiceUomChangeEmailDecorator decorator;

        [TestInitialize]
        public void Intialize()
        {
            this.mockBulkService = new Mock<IBulkEventService>();
            this.mockEmailService = new Mock<IEmailUomChangeService>();
            this.mockGetItemsByScanCodeQuery = new Mock<IQueryHandler<GetItemsByScanCodeQuery, List<IrmaItemModel>>>();
            this.mockSettings = new Mock<IGlobalControllerSettings>();

            this.decorator = new BulkItemEventServiceUomChangeEmailDecorator(
                this.mockEmailService.Object,
                this.mockGetItemsByScanCodeQuery.Object,
                this.mockSettings.Object,
                this.mockBulkService.Object);
        }

        [TestMethod]
        public void BulkItemEventEmailDecoratorRun_EmailAppSettingForRegionSetToFalse_ShouldNotCallEmailService()
        {
            // Given
            this.decorator.Region = "FL";
            this.mockSettings.SetupGet(s => s.SendRetailUomChangeEmailAlerts).Returns(false);
            this.decorator.ValidatedItemList = GetValidatedItemList();
            // When
            this.decorator.Run();

            // Then
            this.mockBulkService.Verify(s => s.Run(), Times.Once);
            this.mockEmailService.Verify(es => es.NotifyUomChanges(It.IsAny<List<IrmaItemModel>>(), It.IsAny<List<ValidatedItemModel>>(), It.IsAny<string>(),It.IsAny<string>()),
                Times.Never);
            this.mockGetItemsByScanCodeQuery.Verify(q => q.Handle(It.IsAny<GetItemsByScanCodeQuery>()), Times.Never);
        }

        [TestMethod]
        public void BulkItemEventEmailDecoratorRun_EmailAppSettingForRegionSetToTrue_ShouldCallEmailService()
        {
            // Given
            string emailSubjectEnvironment = "DEV";
            List<IrmaItemModel> irmaItems = new List<IrmaItemModel>{ new IrmaItemModel { Description = "Test", Identifier = "1221" }};
            this.decorator.Region = "FL";
            this.mockSettings.SetupGet(s => s.SendRetailUomChangeEmailAlerts).Returns(true);
            this.mockSettings.SetupGet(s => s.EmailSubjectEnvironment).Returns(emailSubjectEnvironment);
            this.mockGetItemsByScanCodeQuery.Setup(q => q.Handle(It.IsAny<GetItemsByScanCodeQuery>()))
                .Returns(irmaItems);
            this.decorator.ValidatedItemList = new List<ValidatedItemModel>();
            this.mockBulkService.SetupGet(s => s.ValidatedItemList).Returns(new List<ValidatedItemModel>());

            // When
            this.decorator.Run();

            // Then
            this.mockBulkService.Verify(s => s.Run(), Times.Once);

            this.mockEmailService.Verify(
                    es => es.NotifyUomChanges(It.Is<List<IrmaItemModel>>(iim => iim == irmaItems),
                        It.Is<List<ValidatedItemModel>>(vm => this.decorator.ValidatedItemList.SequenceEqual(vm)),
                        It.Is<string>(s => s == decorator.Region),
                        It.Is<string>(s => s == emailSubjectEnvironment)),
                Times.Once);

            this.mockGetItemsByScanCodeQuery.Verify(q => q.Handle(It.IsAny<GetItemsByScanCodeQuery>()), Times.Once);
        }

        private List<ValidatedItemModel> GetValidatedItemList()
        {
            List<ValidatedItemModel> validatedItems = new List<ValidatedItemModel>();
            validatedItems.Add(new TestValidatedItemModelBuilder().WithItemId(1).WithScanCode("12344").WithEventTypeId(EventTypes.ItemValidation).WithHasItemSignAttributes(true).Build());
            validatedItems.Add(new TestValidatedItemModelBuilder().WithItemId(2).WithScanCode("12345").WithEventTypeId(EventTypes.ItemValidation).WithHasItemSignAttributes(true).Build());
            validatedItems.Add(new TestValidatedItemModelBuilder().WithItemId(3).WithScanCode("12346").WithEventTypeId(EventTypes.ItemValidation).WithHasItemSignAttributes(true).Build());
            validatedItems.Add(new TestValidatedItemModelBuilder().WithItemId(4).WithScanCode("12347").WithEventTypeId(EventTypes.ItemValidation).WithHasItemSignAttributes(true).Build());

            return validatedItems;
        }
    }
}
