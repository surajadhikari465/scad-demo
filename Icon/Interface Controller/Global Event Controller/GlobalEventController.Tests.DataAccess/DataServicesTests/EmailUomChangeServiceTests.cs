using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Irma.Framework;
using Icon.Common.Email;
using GlobalEventController.DataAccess.Infrastructure;
using Moq;
using GlobalEventController.DataAccess.DataServices;
using System.Collections;
using GlobalEventController.Common;
using System.Collections.Generic;
using Irma.Testing.Builders;
using System.Data.Entity;
using System.Linq;
using GlobalEventController.Testing.Common;

namespace GlobalEventController.Tests.DataAccess.DataServicesTests
{
    [TestClass]
    public class EmailUomChangeServiceTests
    {
        private Mock<IEmailClient> mockEmailClient;
        private EmailUomChangeService emailUomChangeService;

        [TestInitialize]
        public void Initialize()
        {
            this.mockEmailClient = new Mock<IEmailClient>();
            this.emailUomChangeService = new EmailUomChangeService(this.mockEmailClient.Object);
        }

        [TestMethod]
        public void EmailUomChangeServiceNotifyUomChanges_ItemsHaveRetailUomAndRetailUnitChangesFromPoundToSomethingOtherThanPound_ShouldSendEmailWithChangedItems()
        {
            // Given
            string region = "FL";
            string emailSubjectEnvironment = "DEV";

            List<IrmaItemModel> irmaItems = new List<IrmaItemModel>();
            irmaItems.Add(new IrmaItemModel { Identifier = "123", RetailUomAbbreviation = "LB", RetailUnitAbbreviation = "LB", SubTeamName = "Grocery", IsDefaultIdentifier = true });
            irmaItems.Add(new IrmaItemModel { Identifier = "122", RetailUomAbbreviation = "LB", RetailUnitAbbreviation = "LB", SubTeamName = "Grocery", IsDefaultIdentifier = true });

            List<ValidatedItemModel> validatedItems = new List<ValidatedItemModel>();
            validatedItems.Add(new TestValidatedItemModelBuilder().WithScanCode("123").WithRetailUom("ML").Build());
            validatedItems.Add(new TestValidatedItemModelBuilder().WithScanCode("122").WithRetailUom("KG").Build());

            // When
            emailUomChangeService.NotifyUomChanges(irmaItems, validatedItems, region, emailSubjectEnvironment);

            // Then
            mockEmailClient.Verify(ec => ec.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void EmailUomChangeServiceNotifyUomChanges_ItemsHaveRetailUomAndRetailUnitChangesFromSomethingOtherThanPoundToPound_ShouldSendEmailWithChangedItems()
        {
            // Given
            string region = "FL";
            string emailSubjectEnvironment = "DEV";

            List<IrmaItemModel> irmaItems = new List<IrmaItemModel>();
            irmaItems.Add(new IrmaItemModel { Identifier = "123", RetailUomAbbreviation = "CT", RetailUnitAbbreviation = "EA", SubTeamName = "Grocery", IsDefaultIdentifier = true });
            irmaItems.Add(new IrmaItemModel { Identifier = "122", RetailUomAbbreviation = "OZ", RetailUnitAbbreviation = "EA", SubTeamName = "Grocery", IsDefaultIdentifier = true });

            List<ValidatedItemModel> validatedItems = new List<ValidatedItemModel>();
            validatedItems.Add(new TestValidatedItemModelBuilder().WithScanCode("123").WithRetailUom("LB").Build());
            validatedItems.Add(new TestValidatedItemModelBuilder().WithScanCode("122").WithRetailUom("LB").Build());

            // When
            emailUomChangeService.NotifyUomChanges(irmaItems, validatedItems, region, emailSubjectEnvironment);

            // Then
            mockEmailClient.Verify(ec => ec.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void EmailUomChangeServiceNotifyUomChanges_ItemsHaveRetailUomThatChangeToPoundAndRetailUnitDidNotChangeToOrFromPound_ShouldNotSendEmailWithChangedRetailUnit()
        {
            // Given
            string region = "FL";
            string emailSubjectEnvironment = "DEV";

            List<IrmaItemModel> irmaItems = new List<IrmaItemModel>();
            irmaItems.Add(new IrmaItemModel { Identifier = "123", RetailUomAbbreviation = "CT", RetailUnitAbbreviation = "LB", SubTeamName = "Grocery", IsDefaultIdentifier = true });
            irmaItems.Add(new IrmaItemModel { Identifier = "122", RetailUomAbbreviation = "OZ", RetailUnitAbbreviation = "LB", SubTeamName = "Grocery", IsDefaultIdentifier = true });

            List<ValidatedItemModel> validatedItems = new List<ValidatedItemModel>();
            validatedItems.Add(new TestValidatedItemModelBuilder().WithScanCode("123").WithRetailUom("LB").Build());
            validatedItems.Add(new TestValidatedItemModelBuilder().WithScanCode("122").WithRetailUom("LB").Build());

            // When
            emailUomChangeService.NotifyUomChanges(irmaItems, validatedItems, region, emailSubjectEnvironment);

            // Then
            mockEmailClient.Verify(ec => ec.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void EmailUomChangeServiceNotifyUomChanges_ItemsHaveRetailUomThatChangeFromPoundAndRetailUnitDidNotChangeToOrFromPound_ShouldNotSendEmailWithChangedItems()
        {
            // Given
            string region = "FL";
            string emailSubjectEnvironment = "DEV";

            List<IrmaItemModel> irmaItems = new List<IrmaItemModel>();
            irmaItems.Add(new IrmaItemModel { Identifier = "123", RetailUomAbbreviation = "LB", RetailUnitAbbreviation = "EA", SubTeamName = "Grocery", IsDefaultIdentifier = true });
            irmaItems.Add(new IrmaItemModel { Identifier = "122", RetailUomAbbreviation = "LB", RetailUnitAbbreviation = "EA", SubTeamName = "Grocery", IsDefaultIdentifier = true });

            List<ValidatedItemModel> validatedItems = new List<ValidatedItemModel>();
            validatedItems.Add(new TestValidatedItemModelBuilder().WithScanCode("123").WithRetailUom("FZ").Build());
            validatedItems.Add(new TestValidatedItemModelBuilder().WithScanCode("122").WithRetailUom("FZ").Build());

            // When
            emailUomChangeService.NotifyUomChanges(irmaItems, validatedItems, region, emailSubjectEnvironment);

            // Then
            mockEmailClient.Verify(ec => ec.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void EmailUomChangeServiceNotifyUomChanges_ItemsHaveRetailUomThatDidNotChangeToOrFromPoundAndRetailUnitIsNotPoundAndIconRetailUomIsNotPound_ShouldNotSendEmailWithChangedItems()
        {
            // Given
            string region = "FL";
            string emailSubjectEnvironment = "DEV";

            List<IrmaItemModel> irmaItems = new List<IrmaItemModel>();
            irmaItems.Add(new IrmaItemModel { Identifier = "123", RetailUomAbbreviation = "LB", RetailUnitAbbreviation = "EA", SubTeamName = "Grocery", IsDefaultIdentifier = true });
            irmaItems.Add(new IrmaItemModel { Identifier = "122", RetailUomAbbreviation = "LB", RetailUnitAbbreviation = "EA", SubTeamName = "Grocery", IsDefaultIdentifier = true });

            List<ValidatedItemModel> validatedItems = new List<ValidatedItemModel>();
            validatedItems.Add(new TestValidatedItemModelBuilder().WithScanCode("123").WithRetailUom("LB").Build());
            validatedItems.Add(new TestValidatedItemModelBuilder().WithScanCode("122").WithRetailUom("LB").Build());

            // When
            emailUomChangeService.NotifyUomChanges(irmaItems, validatedItems, region, emailSubjectEnvironment);

            // Then
            mockEmailClient.Verify(ec => ec.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void EmailUomChangeServiceNotifyUomChanges_ItemsHaveRetailUomThatDidNotChangeToOrFromPoundAndRetailUnitIsPoundAndIconItemRetailUomIsNotPound_ShouldNotSendEmailWithChangedItems()
        {
            // Given
            string region = "FL";
            string emailSubjectEnvironment = "DEV";

            List<IrmaItemModel> irmaItems = new List<IrmaItemModel>();
            irmaItems.Add(new IrmaItemModel { Identifier = "123", RetailUomAbbreviation = "TB", RetailUnitAbbreviation = "LB", SubTeamName = "Grocery", IsDefaultIdentifier = true });
            irmaItems.Add(new IrmaItemModel { Identifier = "122", RetailUomAbbreviation = "LT", RetailUnitAbbreviation = "LB", SubTeamName = "Grocery", IsDefaultIdentifier = true });

            List<ValidatedItemModel> validatedItems = new List<ValidatedItemModel>();
            validatedItems.Add(new TestValidatedItemModelBuilder().WithScanCode("123").WithRetailUom("OZ").Build());
            validatedItems.Add(new TestValidatedItemModelBuilder().WithScanCode("122").WithRetailUom("FZ").Build());

            // When
            emailUomChangeService.NotifyUomChanges(irmaItems, validatedItems, region, emailSubjectEnvironment);

            // Then
            mockEmailClient.Verify(ec => ec.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void EmailUomChangeServiceNotifyUomChanges_NotDefaultIdentifier_ShouldNotSendEmailWithChangedItems()
        {
            // Given
            string region = "FL";
            string emailSubjectEnvironment = "DEV";

            List<IrmaItemModel> irmaItems = new List<IrmaItemModel>();
            irmaItems.Add(new IrmaItemModel { Identifier = "123", RetailUomAbbreviation = "CT", RetailUnitAbbreviation = "EA", SubTeamName = "Grocery", IsDefaultIdentifier = false });
            irmaItems.Add(new IrmaItemModel { Identifier = "122", RetailUomAbbreviation = "OZ", RetailUnitAbbreviation = "EA", SubTeamName = "Grocery", IsDefaultIdentifier = false });

            List<ValidatedItemModel> validatedItems = new List<ValidatedItemModel>();
            validatedItems.Add(new TestValidatedItemModelBuilder().WithScanCode("123").WithRetailUom("LB").Build());
            validatedItems.Add(new TestValidatedItemModelBuilder().WithScanCode("122").WithRetailUom("LB").Build());

            // When
            emailUomChangeService.NotifyUomChanges(irmaItems, validatedItems, region, emailSubjectEnvironment);

            // Then
            mockEmailClient.Verify(ec => ec.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }
    }
}
