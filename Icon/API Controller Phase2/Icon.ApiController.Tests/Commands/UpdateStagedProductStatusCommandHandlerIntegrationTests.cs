using Icon.ApiController.DataAccess.Commands;
using Icon.Framework;
using Icon.Framework.RenewableContext;
using Icon.Logging;
using Icon.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Transactions;

namespace Icon.ApiController.Tests.Commands
{
    [TestClass]
    public class UpdateStagedProductStatusCommandHandlerIntegrationTests
    {
        private UpdateStagedProductStatusCommandHandler commandHandler;
        private IconContext context;
        private Mock<ILogger<UpdateStagedProductStatusCommandHandler>> mockLogger;
        private HierarchyClass testBrandClass;
        private HierarchyClass testBrowsingClass;
        private HierarchyClass testMerchandiseClass;
        private HierarchyClass testFinancialClass;
        private HierarchyClass testTaxClass;
        private TransactionScope transaction;

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();
            context = new IconContext();

            mockLogger = new Mock<ILogger<UpdateStagedProductStatusCommandHandler>>();
            commandHandler = new UpdateStagedProductStatusCommandHandler(mockLogger.Object, new IconDbContextFactory());
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
        }

        private void StageHierarchyClasses()
        {
            testMerchandiseClass = new HierarchyClass { hierarchyID = Hierarchies.Merchandise, hierarchyClassName = "Test Merch", hierarchyLevel = 5 };
            testBrandClass = new HierarchyClass { hierarchyID = Hierarchies.Brands, hierarchyClassName = "Test Brand", hierarchyLevel = 1 };
            testBrowsingClass = new HierarchyClass { hierarchyID = Hierarchies.Browsing, hierarchyClassName = "Test Browsing", hierarchyLevel = 1 };
            testTaxClass = new HierarchyClass { hierarchyID = Hierarchies.Tax, hierarchyClassName = "Test Tax", hierarchyLevel = 1 };
            testFinancialClass = new HierarchyClass { hierarchyID = Hierarchies.Financial, hierarchyClassName = "Financial Class (12345)", hierarchyLevel = 1 };

            context.HierarchyClass.Add(testMerchandiseClass);
            context.HierarchyClass.Add(testBrowsingClass);
            context.HierarchyClass.Add(testBrandClass);
            context.HierarchyClass.Add(testTaxClass);
            context.HierarchyClass.Add(testFinancialClass);
            context.SaveChanges();
        }

        private MessageQueueProduct CreateTestProductMessage(int messageStatusId = MessageStatusTypes.Staged)
        {
            return new TestProductMessageBuilder()
                .WithStatusId(messageStatusId)
                .WithBrandId(testBrandClass.hierarchyClassID)
                .WithMerchandiseClassId(testMerchandiseClass.hierarchyClassID)
                .WithTaxClassId(testTaxClass.hierarchyClassID)
                .WithFinancialClassId(testFinancialClass.hierarchyClassName.Split('(')[1].TrimEnd(')'))
                .WithFinancialClassName(testFinancialClass.hierarchyClassName.Split('(')[0].Trim())
                .WithBrowsingClassId(testBrowsingClass.hierarchyClassID);
        }

        [TestMethod]
        public void UpdateStagedProductStatus_InvalidInput_WarningShouldBeLogged()
        {
            // Given.
            UpdateStagedProductStatusCommand nullList = new UpdateStagedProductStatusCommand
            {
                PublishedHierarchyClasses = null
            };

            UpdateStagedProductStatusCommand emptyList = new UpdateStagedProductStatusCommand
            {
                PublishedHierarchyClasses = new List<int>()
            };

            // When.
            commandHandler.Execute(nullList);
            commandHandler.Execute(emptyList);

            // Then.
            mockLogger.Verify(l => l.Warn(It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        public void UpdateStagedProductStatus_NoStagedProductsInTheMessageQueue_NoUpdateShouldHappen()
        {
            // Given.
            StageHierarchyClasses();

            List<MessageQueueProduct> testProducts = new List<MessageQueueProduct>
            {
                CreateTestProductMessage(MessageStatusTypes.Failed),
                CreateTestProductMessage(MessageStatusTypes.Associated)
            };

            context.MessageQueueProduct.AddRange(testProducts);
            context.SaveChanges();

            var command = new UpdateStagedProductStatusCommand
            {
                PublishedHierarchyClasses = new List<int>
                {
                    testBrandClass.hierarchyClassID,
                    testMerchandiseClass.hierarchyClassID,
                    testTaxClass.hierarchyClassID,
                    testBrowsingClass.hierarchyClassID,
                    testFinancialClass.hierarchyClassID
                }
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            var products = context.MessageQueueProduct.Where(mq => mq.ProductDescription == "Test Product Description").ToList();

            Assert.IsTrue(products.TrueForAll(p => p.MessageStatusId != MessageStatusTypes.Ready));
        }

        [TestMethod]
        public void UpdateStagedProductStatus_NoStagedProductsAreAssociatedToThePublishedHierarchyClasses_NoUpdateShouldHappen()
        {
            // Given.
            StageHierarchyClasses();
            List<MessageQueueProduct> testProducts = new List<MessageQueueProduct>
            {
                CreateTestProductMessage(MessageStatusTypes.Staged)
            };

            context.MessageQueueProduct.AddRange(testProducts);
            context.SaveChanges();

            var command = new UpdateStagedProductStatusCommand
            {
                PublishedHierarchyClasses = new List<int>
                {
                    testBrandClass.hierarchyClassID+1,
                    testMerchandiseClass.hierarchyClassID+1,
                    testTaxClass.hierarchyClassID+1,
                    testBrowsingClass.hierarchyClassID+1,
                    testFinancialClass.hierarchyClassID+1
                }
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            var products = context.MessageQueueProduct.Where(mq => mq.ProductDescription == "Test Product Description").ToList();

            Assert.IsTrue(products.TrueForAll(p => p.MessageStatusId != MessageStatusTypes.Ready));
        }

        [TestMethod]
        public void UpdateStagedProductStatus_OneStagedProductHasHierarchyAssociationsNotSentToEsb_ProductIsKeptInStagedStatus()
        {
            // Given.
            StageHierarchyClasses();

            MessageQueueProduct testProductMessage = CreateTestProductMessage();
            context.MessageQueueProduct.Add(testProductMessage);
            context.SaveChanges();

            var command = new UpdateStagedProductStatusCommand
            {
                PublishedHierarchyClasses = new List<int>
                {
                    testBrandClass.hierarchyClassID
                }
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            Assert.AreEqual(MessageStatusTypes.Staged, testProductMessage.MessageStatusId);
        }

        [TestMethod]
        public void UpdateStagedProductStatus_OneStagedProductHasAllRequiredHierarchyAssociationsSentToEsb_ProductIsSetToReadyStatus()
        {
            // Given.
            StageHierarchyClasses();

            MessageQueueProduct testProductMessage = CreateTestProductMessage();
            context.MessageQueueProduct.Add(testProductMessage);
            context.SaveChanges();

            testBrandClass.HierarchyClassTrait.Add(new HierarchyClassTrait
                {
                    traitID = Traits.SentToEsb,
                    hierarchyClassID = testBrandClass.hierarchyClassID,
                    Trait = context.Trait.Single(t => t.traitID == Traits.SentToEsb),
                    HierarchyClass = testBrandClass,
                    traitValue = DateTime.Now.ToString()
                });
            testMerchandiseClass.HierarchyClassTrait.Add(new HierarchyClassTrait
                {
                    traitID = Traits.SentToEsb,
                    hierarchyClassID = testMerchandiseClass.hierarchyClassID,
                    Trait = context.Trait.Single(t => t.traitID == Traits.SentToEsb),
                    HierarchyClass = testMerchandiseClass,
                    traitValue = DateTime.Now.ToString()
                });
            testFinancialClass.HierarchyClassTrait.Add(new HierarchyClassTrait
                {
                    traitID = Traits.SentToEsb,
                    hierarchyClassID = testFinancialClass.hierarchyClassID,
                    Trait = context.Trait.Single(t => t.traitID == Traits.SentToEsb),
                    HierarchyClass = testFinancialClass,
                    traitValue = DateTime.Now.ToString()
                });
            testTaxClass.HierarchyClassTrait.Add(new HierarchyClassTrait
                {
                    traitID = Traits.SentToEsb,
                    hierarchyClassID = testTaxClass.hierarchyClassID,
                    Trait = context.Trait.Single(t => t.traitID == Traits.SentToEsb),
                    HierarchyClass = testTaxClass,
                    traitValue = null
                });
            testBrowsingClass.HierarchyClassTrait.Add(new HierarchyClassTrait
                {
                    traitID = Traits.SentToEsb,
                    hierarchyClassID = testTaxClass.hierarchyClassID,
                    Trait = context.Trait.Single(t => t.traitID == Traits.SentToEsb),
                    HierarchyClass = testBrowsingClass,
                    traitValue = null
                });

            context.SaveChanges();

            var command = new UpdateStagedProductStatusCommand
            {
                PublishedHierarchyClasses = new List<int>
                {
                    testBrandClass.hierarchyClassID
                }
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            context.Entry(testProductMessage).Reload();
            Assert.AreEqual(MessageStatusTypes.Ready, testProductMessage.MessageStatusId);
        }

        [TestMethod]
        public void UpdateStagedProductStatus_MultipleStagedProductsHaveAllHierarchyAssociationsSentToEsb_ProductsAreSetToReadyStatus()
        {
            // Given.
            StageHierarchyClasses();
            List<MessageQueueProduct> testProducts = new List<MessageQueueProduct>
            {
                CreateTestProductMessage(),
                CreateTestProductMessage(),
                CreateTestProductMessage(),
                CreateTestProductMessage(),
                CreateTestProductMessage(),
                CreateTestProductMessage(),
                CreateTestProductMessage(),
                CreateTestProductMessage()
            };

            context.MessageQueueProduct.AddRange(testProducts);
            context.SaveChanges();

            testBrandClass.HierarchyClassTrait.Add(new HierarchyClassTrait
                {
                    traitID = Traits.SentToEsb,
                    hierarchyClassID = testBrandClass.hierarchyClassID,
                    Trait = context.Trait.Single(t => t.traitID == Traits.SentToEsb),
                    HierarchyClass = testBrandClass,
                    traitValue = DateTime.Now.ToString()
                });
            testMerchandiseClass.HierarchyClassTrait.Add(new HierarchyClassTrait
                {
                    traitID = Traits.SentToEsb,
                    hierarchyClassID = testMerchandiseClass.hierarchyClassID,
                    Trait = context.Trait.Single(t => t.traitID == Traits.SentToEsb),
                    HierarchyClass = testMerchandiseClass,
                    traitValue = DateTime.Now.ToString()
                });
            testFinancialClass.HierarchyClassTrait.Add(new HierarchyClassTrait
                {
                    traitID = Traits.SentToEsb,
                    hierarchyClassID = testFinancialClass.hierarchyClassID,
                    Trait = context.Trait.Single(t => t.traitID == Traits.SentToEsb),
                    HierarchyClass = testFinancialClass,
                    traitValue = DateTime.Now.ToString()
                });
            testTaxClass.HierarchyClassTrait.Add(new HierarchyClassTrait
                {
                    traitID = Traits.SentToEsb,
                    hierarchyClassID = testTaxClass.hierarchyClassID,
                    Trait = context.Trait.Single(t => t.traitID == Traits.SentToEsb),
                    HierarchyClass = testTaxClass,
                    traitValue = null
                });
            testBrowsingClass.HierarchyClassTrait.Add(new HierarchyClassTrait
                {
                    traitID = Traits.SentToEsb,
                    hierarchyClassID = testTaxClass.hierarchyClassID,
                    Trait = context.Trait.Single(t => t.traitID == Traits.SentToEsb),
                    HierarchyClass = testBrowsingClass,
                    traitValue = null
                });

            context.SaveChanges();

            var command = new UpdateStagedProductStatusCommand
            {
                PublishedHierarchyClasses = new List<int>
                {
                    testBrandClass.hierarchyClassID,
                    testMerchandiseClass.hierarchyClassID,
                    testTaxClass.hierarchyClassID,
                    testFinancialClass.hierarchyClassID
                }
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            foreach (var product in testProducts)
            {
                context.Entry(product).Reload();
                Assert.AreEqual(MessageStatusTypes.Ready, product.MessageStatusId);
            }
        }

        [TestMethod]
        public void UpdateStagedProductStatus_FinancialHierarchyIsPublishedAndAllAssociatedHierarchiesAreSentToEsb_ProductsAreSetToReadyStatus()
        {
            StageHierarchyClasses();

            List<MessageQueueProduct> testProducts = new List<MessageQueueProduct>
            {
                CreateTestProductMessage(),
                CreateTestProductMessage(),
                CreateTestProductMessage(),
                CreateTestProductMessage(),
                CreateTestProductMessage(),
                CreateTestProductMessage(),
                CreateTestProductMessage(),
                CreateTestProductMessage()
            };

            context.MessageQueueProduct.AddRange(testProducts);
            context.SaveChanges();

            testBrandClass.HierarchyClassTrait.Add(new HierarchyClassTrait
                {
                    traitID = Traits.SentToEsb,
                    hierarchyClassID = testBrandClass.hierarchyClassID,
                    Trait = context.Trait.Single(t => t.traitID == Traits.SentToEsb),
                    HierarchyClass = testBrandClass,
                    traitValue = DateTime.Now.ToString()
                });
            testMerchandiseClass.HierarchyClassTrait.Add(new HierarchyClassTrait
                {
                    traitID = Traits.SentToEsb,
                    hierarchyClassID = testMerchandiseClass.hierarchyClassID,
                    Trait = context.Trait.Single(t => t.traitID == Traits.SentToEsb),
                    HierarchyClass = testMerchandiseClass,
                    traitValue = DateTime.Now.ToString()
                });
            testFinancialClass.HierarchyClassTrait.Add(new HierarchyClassTrait
                {
                    traitID = Traits.SentToEsb,
                    hierarchyClassID = testFinancialClass.hierarchyClassID,
                    Trait = context.Trait.Single(t => t.traitID == Traits.SentToEsb),
                    HierarchyClass = testFinancialClass,
                    traitValue = DateTime.Now.ToString()
                });
            testTaxClass.HierarchyClassTrait.Add(new HierarchyClassTrait
                {
                    traitID = Traits.SentToEsb,
                    hierarchyClassID = testTaxClass.hierarchyClassID,
                    Trait = context.Trait.Single(t => t.traitID == Traits.SentToEsb),
                    HierarchyClass = testTaxClass,
                    traitValue = null
                });
            testBrowsingClass.HierarchyClassTrait.Add(new HierarchyClassTrait
                {
                    traitID = Traits.SentToEsb,
                    hierarchyClassID = testTaxClass.hierarchyClassID,
                    Trait = context.Trait.Single(t => t.traitID == Traits.SentToEsb),
                    HierarchyClass = testBrowsingClass,
                    traitValue = null
                });

            context.SaveChanges();

            var command = new UpdateStagedProductStatusCommand
            {
                PublishedHierarchyClasses = new List<int>
                {
                    testFinancialClass.hierarchyClassID
                }
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            foreach (var product in testProducts)
            {
                context.Entry(product).Reload();
                Assert.AreEqual(MessageStatusTypes.Ready, product.MessageStatusId);
            }
        }

        [TestMethod]
        public void UpdateStagedProductStatus_FinancialHierarchyIsPublishedAndNotAllAssociatedHierarchiesAreSentToEsb_ProductsAreSetToStagedStatus()
        {
            StageHierarchyClasses();

            List<MessageQueueProduct> testProducts = new List<MessageQueueProduct>
            {
                CreateTestProductMessage(),
                CreateTestProductMessage(),
                CreateTestProductMessage(),
                CreateTestProductMessage(),
                CreateTestProductMessage(),
                CreateTestProductMessage(),
                CreateTestProductMessage(),
                CreateTestProductMessage()
            };

            context.MessageQueueProduct.AddRange(testProducts);
            context.SaveChanges();

            testBrandClass.HierarchyClassTrait.Add(new HierarchyClassTrait
                {
                    traitID = Traits.SentToEsb,
                    hierarchyClassID = testBrandClass.hierarchyClassID,
                    Trait = context.Trait.Single(t => t.traitID == Traits.SentToEsb),
                    HierarchyClass = testBrandClass,
                    traitValue = DateTime.Now.ToString()
                });
            testMerchandiseClass.HierarchyClassTrait.Add(new HierarchyClassTrait
                {
                    traitID = Traits.SentToEsb,
                    hierarchyClassID = testMerchandiseClass.hierarchyClassID,
                    Trait = context.Trait.Single(t => t.traitID == Traits.SentToEsb),
                    HierarchyClass = testMerchandiseClass,
                    traitValue = null
                });
            testFinancialClass.HierarchyClassTrait.Add(new HierarchyClassTrait
                {
                    traitID = Traits.SentToEsb,
                    hierarchyClassID = testFinancialClass.hierarchyClassID,
                    Trait = context.Trait.Single(t => t.traitID == Traits.SentToEsb),
                    HierarchyClass = testFinancialClass,
                    traitValue = DateTime.Now.ToString()
                });
            testTaxClass.HierarchyClassTrait.Add(new HierarchyClassTrait
                {
                    traitID = Traits.SentToEsb,
                    hierarchyClassID = testTaxClass.hierarchyClassID,
                    Trait = context.Trait.Single(t => t.traitID == Traits.SentToEsb),
                    HierarchyClass = testTaxClass,
                    traitValue = null
                });
            testBrowsingClass.HierarchyClassTrait.Add(new HierarchyClassTrait
                {
                    traitID = Traits.SentToEsb,
                    hierarchyClassID = testTaxClass.hierarchyClassID,
                    Trait = context.Trait.Single(t => t.traitID == Traits.SentToEsb),
                    HierarchyClass = testBrowsingClass,
                    traitValue = null
                });

            context.SaveChanges();

            var command = new UpdateStagedProductStatusCommand
            {
                PublishedHierarchyClasses = new List<int>
                {
                    testFinancialClass.hierarchyClassID
                }
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            foreach (var product in testProducts)
            {
                Assert.AreEqual(MessageStatusTypes.Staged, product.MessageStatusId);
            }
        }

        [TestMethod]
        public void UpdateStagedProductStatus_MessageIsStagedDueToTaxClassButAllRequiredHierarchyClassesAreSentToEsb_ProductsAreSetToReadyStatus()
        {
            StageHierarchyClasses();

            List<MessageQueueProduct> testProducts = new List<MessageQueueProduct>
            {
                CreateTestProductMessage(),
                CreateTestProductMessage(),
                CreateTestProductMessage(),
                CreateTestProductMessage(),
                CreateTestProductMessage(),
                CreateTestProductMessage(),
                CreateTestProductMessage(),
                CreateTestProductMessage()
            };

            context.MessageQueueProduct.AddRange(testProducts);
            context.SaveChanges();

            testBrandClass.HierarchyClassTrait.Add(new HierarchyClassTrait
                {
                    traitID = Traits.SentToEsb,
                    hierarchyClassID = testBrandClass.hierarchyClassID,
                    Trait = context.Trait.Single(t => t.traitID == Traits.SentToEsb),
                    HierarchyClass = testBrandClass,
                    traitValue = DateTime.Now.ToString()
                });
            testMerchandiseClass.HierarchyClassTrait.Add(new HierarchyClassTrait
                {
                    traitID = Traits.SentToEsb,
                    hierarchyClassID = testMerchandiseClass.hierarchyClassID,
                    Trait = context.Trait.Single(t => t.traitID == Traits.SentToEsb),
                    HierarchyClass = testMerchandiseClass,
                    traitValue = DateTime.Now.ToString()
                });
            testFinancialClass.HierarchyClassTrait.Add(new HierarchyClassTrait
                {
                    traitID = Traits.SentToEsb,
                    hierarchyClassID = testFinancialClass.hierarchyClassID,
                    Trait = context.Trait.Single(t => t.traitID == Traits.SentToEsb),
                    HierarchyClass = testFinancialClass,
                    traitValue = DateTime.Now.ToString()
                });
            testTaxClass.HierarchyClassTrait.Add(new HierarchyClassTrait
                {
                    traitID = Traits.SentToEsb,
                    hierarchyClassID = testTaxClass.hierarchyClassID,
                    Trait = context.Trait.Single(t => t.traitID == Traits.SentToEsb),
                    HierarchyClass = testTaxClass,
                    traitValue = null
                });
            testBrowsingClass.HierarchyClassTrait.Add(new HierarchyClassTrait
                {
                    traitID = Traits.SentToEsb,
                    hierarchyClassID = testTaxClass.hierarchyClassID,
                    Trait = context.Trait.Single(t => t.traitID == Traits.SentToEsb),
                    HierarchyClass = testBrowsingClass,
                    traitValue = null
                });

            context.SaveChanges();

            var command = new UpdateStagedProductStatusCommand
            {
                PublishedHierarchyClasses = new List<int>
                {
                    testTaxClass.hierarchyClassID
                }
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            foreach (var product in testProducts)
            {
                context.Entry(product).Reload();
                Assert.AreEqual(MessageStatusTypes.Ready, product.MessageStatusId);
            }
        }
    }
}
