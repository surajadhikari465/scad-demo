using Icon.Esb.ItemMovementListener.Commands;
using Icon.Esb.ItemMovementListener.Models;
using Icon.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace Icon.Esb.ItemMovement.Tests.Commands
{
    [TestClass]
    public class SaveItemMovementTransactionCommandHandlerTests
    {
        private SaveItemMovementTransactionCommandHandler commandHandler;
        private IconContext context;
        private TransactionScope transactionScope;

        [TestInitialize]
        public void Initialize()
        {
            transactionScope = new TransactionScope();

            commandHandler = new SaveItemMovementTransactionCommandHandler();
            context = new IconContext();
            context.Database.ExecuteSqlCommand("truncate table app.ItemMovement");
        }

        [TestCleanup]
        public void Cleanup()
        {
            var itemMovementEntities = context.ItemMovement.Where(im => im.ESBMessageID == "Test");
            foreach (var entity in itemMovementEntities.ToList())
            {
                context.ItemMovement.Remove(entity);
            }
            context.SaveChanges();
            context.Dispose();
            transactionScope.Dispose();
        }

        [TestMethod]
        public void SaveItemMovementTransactionCommandHandler_ItemMovementDataIsGiven_ShouldSaveItemMovementData()
        {
            //Given
            IRMATransactionModel model = new IRMATransactionModel
            {
                ESBMessageID = "Test",
                BusinessUnitId = 10052,
                RegisterNumber = 3,
                TransactionSequenceNumber = 1,
                LineItemNumber = 1,
                Identifier = "7612",
                TransDate = DateTime.Now,
                Quantity = 1,
                ItemVoid = true,
                ItemType = 0,
                BasePrice = 16.99m,
                Weight = 0m,
                MarkDownAmount = 2m
            };

            //When
            commandHandler.Execute(new SaveItemMovementTransactionCommand { ItemMovementTransactions = new List<IRMATransactionModel> { model } });

            //Then
            var itemMovementEntities = context.ItemMovement.Where(im => im.ESBMessageID == "Test");
            Assert.AreEqual(1, itemMovementEntities.Count());
        }
    }
}
