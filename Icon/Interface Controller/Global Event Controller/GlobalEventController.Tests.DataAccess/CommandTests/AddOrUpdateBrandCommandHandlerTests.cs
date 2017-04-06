using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Irma.Framework;
using GlobalEventController.DataAccess.Commands;
using Moq;
using Icon.Logging;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;

namespace GlobalEventController.Tests.DataAccess.CommandTests
{
    [TestClass]
    public class AddOrUpdateBrandCommandHandlerTests
    {
        private IrmaContext context;
        private AddOrUpdateBrandCommand command;
        private AddOrUpdateBrandCommandHandler handler;
        private Mock<ILogger<AddOrUpdateBrandCommandHandler>> mockLogger;
        private DbContextTransaction transaction;

        [TestInitialize]
        public void InitializeData()
        {
            this.context = new IrmaContext();
            this.command = new AddOrUpdateBrandCommand();
            this.mockLogger = new Mock<ILogger<AddOrUpdateBrandCommandHandler>>();
            this.handler = new AddOrUpdateBrandCommandHandler(this.context, mockLogger.Object);

            this.transaction = this.context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void CleanupData()
        {
            if (this.transaction != null)
            {
                this.transaction.Rollback();
                this.transaction.Dispose();
            }
            context.Dispose();
        }

        [TestMethod]
        public void UpdateBrand_BrandNotFoundInIrma_ShouldAddBrandToIrma()
        {
            // Given
            this.command.IconBrandId = -1;
            this.command.BrandName = "test";
            this.command.Region = "SW";

            // When
            this.handler.Handle(this.command);

            // Then
            var validatedBrand = context.ValidatedBrand.AsNoTracking().Single(vb => vb.IconBrandId == command.IconBrandId);
            Assert.AreEqual(validatedBrand.ItemBrand.Brand_Name, command.BrandName);
        }

        [TestMethod]
        public void UpdateBrand_BrandNotFoundInIrma_CommandObjectBrandIdIsSetToNewBrand()
        {
            // Given
            this.command.IconBrandId = -1;
            this.command.BrandName = "test";
            this.command.Region = "SW";

            // When
            this.handler.Handle(this.command);

            // Then
            var validatedBrand = context.ValidatedBrand.AsNoTracking().Single(vb => vb.IconBrandId == command.IconBrandId);
            Assert.AreEqual(validatedBrand.IrmaBrandId, this.command.BrandId);
            Assert.AreEqual(validatedBrand.ItemBrand.Brand_ID, this.command.BrandId);

        }

        [TestMethod]
        public void UpdateBrand_BrandFoundInIrma_BrandNameUpdated()
        {
            // Given
            ValidatedBrand existingBrand = GetValidatedBrand();

            this.command.IconBrandId = existingBrand.IconBrandId;
            this.command.BrandName = "automated test";
            this.command.Region = "FL";

            // When
            this.handler.Handle(this.command);

            // Then
            ItemBrand actual = this.context.ItemBrand.First(ib => ib.Brand_ID == this.command.BrandId);

            Assert.AreEqual(this.command.BrandName, actual.Brand_Name, "The BrandName was not updated as expected.");
        }

        [TestMethod]
        public void UpdateBrand_BrandFoundInIrma_BrandIdIsSetInCommandClass()
        {
            // Given
            ValidatedBrand existingBrand = GetValidatedBrand();

            this.command.IconBrandId = existingBrand.IconBrandId;
            this.command.BrandName = "automated test";
            this.command.Region = "FL";

            // When
            this.handler.Handle(this.command);

            // Then
            ItemBrand actual = this.context.ItemBrand.First(ib => ib.Brand_ID == this.command.BrandId);
            Assert.IsTrue(this.command.BrandId == actual.Brand_ID);
        }

        [TestMethod]
        public void AddOrUpdateBrand_IrmaBrandExistsButIsNotValidated_ValidatesTheIrmaBrand()
        {
            //Given
            ItemBrand brand = new ItemBrand { Brand_Name = "Test Brand GloCon" };
            context.ItemBrand.Add(brand);
            context.SaveChanges();

            this.command.IconBrandId = 4000000;
            this.command.BrandName = brand.Brand_Name;
            this.command.Region = "FL";

            //When
            this.handler.Handle(this.command);

            //Then
            Assert.IsNotNull(context.ItemBrand.SingleOrDefault(ib => ib.Brand_Name == brand.Brand_Name));
            Assert.IsNotNull(context.ValidatedBrand.SingleOrDefault(vb => vb.IconBrandId == command.IconBrandId.Value));
        }

        [TestMethod]
        public void AddOrUpdateBrand_IrmaBrandExistsButIsAssociatedToDifferentValidatedBrand_DeletesOldAssociationAndAddsNewAssociation()
        {
            //Given
            ItemBrand brand = new ItemBrand
            {
                Brand_Name = "Test Brand GloCon"
            };
            context.ItemBrand.Add(brand);
            context.SaveChanges();
            context.ValidatedBrand.Add(new ValidatedBrand { IconBrandId = 4000000, ItemBrand = brand });
            context.SaveChanges();

            this.command.IconBrandId = 4000001;
            this.command.BrandName = brand.Brand_Name;
            this.command.Region = "FL";

            //When
            this.handler.Handle(this.command);

            //Then
            Assert.IsNotNull(context.ItemBrand.SingleOrDefault(ib => ib.Brand_Name == brand.Brand_Name));
            Assert.IsNotNull(context.ValidatedBrand.SingleOrDefault(vb => vb.IconBrandId == command.IconBrandId.Value));
            Assert.IsNull(context.ValidatedBrand.SingleOrDefault(vb => vb.IconBrandId == 4000000));
        }

        private ValidatedBrand GetValidatedBrand()
        {
            // Given
            ValidatedBrand existingBrand = this.context.ValidatedBrand.FirstOrDefault();
            if (existingBrand == null)
            {
                IEnumerable<int> existingValidatedBrandIDs = this.context.ValidatedBrand.Select(vb => vb.IrmaBrandId);
                ValidatedBrand validatedBrandtoAdd = new ValidatedBrand();
                validatedBrandtoAdd.IconBrandId = 1;
                validatedBrandtoAdd.IrmaBrandId = this.context.ItemBrand.First(ib => !existingValidatedBrandIDs.Contains(ib.Brand_ID)).Brand_ID;
                this.context.ValidatedBrand.Add(validatedBrandtoAdd);
                this.context.SaveChanges();

                existingBrand = validatedBrandtoAdd;
            }

            return existingBrand;
        }
    }
}
