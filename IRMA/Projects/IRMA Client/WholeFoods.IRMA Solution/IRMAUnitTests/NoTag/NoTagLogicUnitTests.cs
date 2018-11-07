using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using WholeFoods.IRMA.Pricing.BusinessLogic;

namespace IRMAUnitTests.NoTag
{
    [TestClass]
    public class NoTagLogicUnitTests
    {
        private PriceBatchNoTagLogic tagLogic;
        private List<INoTagRule> rules;
        private List<int> testItems;
        private Mock<NoTagDataAccess> mockDb;
        private Mock<INoTagRule> mockMovementRule;
        private Mock<INoTagRule> mockOrderingRule;
        private Mock<INoTagRule> mockReceivingRule;
        private PriceBatchHeaderBO header;

        [TestInitialize]
        public void Initialize()
        {
            mockMovementRule = new Mock<INoTagRule>();
            mockOrderingRule = new Mock<INoTagRule>();
            mockReceivingRule = new Mock<INoTagRule>();
            
            mockDb = new Mock<NoTagDataAccess>();

            rules = new List<INoTagRule>();

            rules.Add(mockMovementRule.Object);
            rules.Add(mockOrderingRule.Object);
            rules.Add(mockReceivingRule.Object);

            header = new PriceBatchHeaderBO();

            testItems = new List<int>
            {
                222222222,
                222222223,
                222222224
            };

            tagLogic = new PriceBatchNoTagLogic(
                rules,
                mockDb.Object,
                testItems.ToArray(),
                header);

            mockDb.Setup(d => d.GetSubteamOverride(It.IsAny<int>())).Returns(0);
            mockDb.Setup(d => d.GetRuleDefaultThreshold(It.IsAny<string>())).Returns(0);
            mockDb.Setup(d => d.GetLabelTypeExclusions(It.IsAny<List<int>>())).Returns(new List<int>());
            mockDb.Setup(d => d.GetOffSaleExclusions(It.IsAny<List<int>>(), header.PriceBatchHeaderId)).Returns(new List<int>());
        }

        [TestMethod]
        public void NoTagLogic_BatchIsTypeNEW_NoTagLogicShouldNotBeApplied()
        {
            // Given.
            header.ItemChgTypeID = 1;

            // When.
            tagLogic.ApplyNoTagLogic();

            // Then.
            mockMovementRule.Verify(r => r.ApplyRule(It.IsAny<List<int>>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            mockOrderingRule.Verify(r => r.ApplyRule(It.IsAny<List<int>>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            mockReceivingRule.Verify(r => r.ApplyRule(It.IsAny<List<int>>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            mockDb.Verify(d => d.WriteToNoTagExclusion(It.IsAny<List<int>>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public void NoTagLogic_BatchIsTypeDEL_NoTagLogicShouldNotBeApplied()
        {
            // Given.
            header.ItemChgTypeID = 3;

            // When.
            tagLogic.ApplyNoTagLogic();

            // Then.
            mockMovementRule.Verify(r => r.ApplyRule(It.IsAny<List<int>>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            mockOrderingRule.Verify(r => r.ApplyRule(It.IsAny<List<int>>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            mockReceivingRule.Verify(r => r.ApplyRule(It.IsAny<List<int>>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            mockDb.Verify(d => d.WriteToNoTagExclusion(It.IsAny<List<int>>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public void NoTagLogic_AllItemsPassTheFirstRule_NoOtherRulesShouldBeExecuted()
        {
            // Given.
            mockMovementRule.SetupGet(r => r.ExcludedItems).Returns(new List<int>());
            mockOrderingRule.SetupGet(r => r.ExcludedItems).Returns(new List<int>());
            mockReceivingRule.SetupGet(r => r.ExcludedItems).Returns(new List<int>());

            // When.
            tagLogic.ApplyNoTagLogic();

            // Then.
            mockMovementRule.Verify(r => r.ApplyRule(It.IsAny<List<int>>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            mockOrderingRule.Verify(r => r.ApplyRule(It.IsAny<List<int>>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            mockReceivingRule.Verify(r => r.ApplyRule(It.IsAny<List<int>>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            mockDb.Verify(d => d.WriteToNoTagExclusion(It.IsAny<List<int>>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public void NoTagLogic_AllItemsPassTheSecondRule_NoOtherRulesShouldBeExecuted()
        {
            // Given.
            mockMovementRule.SetupGet(r => r.ExcludedItems).Returns(new List<int> { testItems[0] });
            mockOrderingRule.SetupGet(r => r.ExcludedItems).Returns(new List<int>());
            mockReceivingRule.SetupGet(r => r.ExcludedItems).Returns(new List<int>());

            // When.
            tagLogic.ApplyNoTagLogic();

            // Then.
            mockMovementRule.Verify(r => r.ApplyRule(It.IsAny<List<int>>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            mockOrderingRule.Verify(r => r.ApplyRule(It.IsAny<List<int>>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            mockReceivingRule.Verify(r => r.ApplyRule(It.IsAny<List<int>>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            mockDb.Verify(d => d.WriteToNoTagExclusion(It.IsAny<List<int>>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public void NoTagLogic_AllItemsPassTheFinalRule_NoExceptionsShouldBeWritten()
        {
            // Given.
            mockMovementRule.SetupGet(r => r.ExcludedItems).Returns(new List<int> { testItems[0] });
            mockOrderingRule.SetupGet(r => r.ExcludedItems).Returns(new List<int> { testItems[0] });
            mockReceivingRule.SetupGet(r => r.ExcludedItems).Returns(new List<int>());

            // When.
            tagLogic.ApplyNoTagLogic();

            // Then.
            mockMovementRule.Verify(r => r.ApplyRule(It.IsAny<List<int>>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            mockOrderingRule.Verify(r => r.ApplyRule(It.IsAny<List<int>>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            mockReceivingRule.Verify(r => r.ApplyRule(It.IsAny<List<int>>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            mockDb.Verify(d => d.WriteToNoTagExclusion(It.IsAny<List<int>>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public void NoTagLogic_ItemFailsMovementCheckOnly_ItemShouldNotBeExcluded()
        {
            // Given.
            mockMovementRule.SetupGet(r => r.ExcludedItems).Returns(new List<int> { testItems[0] });
            mockOrderingRule.SetupGet(r => r.ExcludedItems).Returns(new List<int>());
            mockReceivingRule.SetupGet(r => r.ExcludedItems).Returns(new List<int>());

            // When.
            tagLogic.ApplyNoTagLogic();

            // Then.
            Assert.IsFalse(tagLogic.ItemsExcluded);
            mockDb.Verify(d => d.WriteToNoTagExclusion(It.IsAny<List<int>>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public void NoTagLogic_ItemFailsMovementAndOrderingChecksOnly_ItemShouldNotBeExcluded()
        {
            // Given.
            mockMovementRule.SetupGet(r => r.ExcludedItems).Returns(new List<int> { testItems[0] });
            mockOrderingRule.SetupGet(r => r.ExcludedItems).Returns(new List<int> { testItems[0] });
            mockReceivingRule.SetupGet(r => r.ExcludedItems).Returns(new List<int>());

            // When.
            tagLogic.ApplyNoTagLogic();

            // Then.
            Assert.IsFalse(tagLogic.ItemsExcluded);
            mockDb.Verify(d => d.WriteToNoTagExclusion(It.IsAny<List<int>>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public void NoTagLogic_ItemFailsMovementAndReceivingChecksOnly_ItemShouldNotBeExcluded()
        {
            // Given.
            mockMovementRule.SetupGet(r => r.ExcludedItems).Returns(new List<int> { testItems[0] });
            mockOrderingRule.SetupGet(r => r.ExcludedItems).Returns(new List<int>());
            mockReceivingRule.SetupGet(r => r.ExcludedItems).Returns(new List<int> { testItems[0] });

            // When.
            tagLogic.ApplyNoTagLogic();

            // Then.
            Assert.IsFalse(tagLogic.ItemsExcluded);
            mockDb.Verify(d => d.WriteToNoTagExclusion(It.IsAny<List<int>>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public void NoTagLogic_ItemFailsOrderingAndReceivingChecksOnly_ItemShouldNotBeExcluded()
        {
            // Given.
            mockMovementRule.SetupGet(r => r.ExcludedItems).Returns(new List<int>());
            mockOrderingRule.SetupGet(r => r.ExcludedItems).Returns(new List<int> { testItems[0] });
            mockReceivingRule.SetupGet(r => r.ExcludedItems).Returns(new List<int> { testItems[0] });

            // When.
            tagLogic.ApplyNoTagLogic();

            // Then.
            Assert.IsFalse(tagLogic.ItemsExcluded);
            mockDb.Verify(d => d.WriteToNoTagExclusion(It.IsAny<List<int>>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public void NoTagLogic_ItemFailsAllRules_ItemShouldBeExcluded()
        {
            // Given.
            mockMovementRule.SetupGet(r => r.ExcludedItems).Returns(new List<int> { testItems[0] });
            mockOrderingRule.SetupGet(r => r.ExcludedItems).Returns(new List<int> { testItems[0] });
            mockReceivingRule.SetupGet(r => r.ExcludedItems).Returns(new List<int> { testItems[0] });

            // When.
            tagLogic.ApplyNoTagLogic();

            // Then.
            Assert.IsTrue(tagLogic.ItemsExcluded);
            mockDb.Verify(d => d.WriteToNoTagExclusion(It.IsAny<List<int>>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public void NoTagLogic_SubteamHasThresholdOverride_DefaultRuleThresholdShouldNotBeUsed()
        {
            // Given.
            mockMovementRule.SetupGet(r => r.ExcludedItems).Returns(new List<int>());
            mockOrderingRule.SetupGet(r => r.ExcludedItems).Returns(new List<int>());
            mockReceivingRule.SetupGet(r => r.ExcludedItems).Returns(new List<int>());

            mockDb.Setup(d => d.GetSubteamOverride(It.IsAny<int>())).Returns(1);

            // When.
            tagLogic.ApplyNoTagLogic();

            // Then.
            mockDb.Verify(d => d.GetSubteamOverride(It.IsAny<int>()), Times.Once);
            mockDb.Verify(d => d.GetRuleDefaultThreshold(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void NoTagLogic_SubteamDoesNotHaveThresholdOverride_DefaultRuleThresholdShouldBeUsed()
        {
            // Given.
            mockMovementRule.SetupGet(r => r.ExcludedItems).Returns(new List<int>());
            mockOrderingRule.SetupGet(r => r.ExcludedItems).Returns(new List<int>());
            mockReceivingRule.SetupGet(r => r.ExcludedItems).Returns(new List<int>());

            mockDb.Setup(d => d.GetSubteamOverride(It.IsAny<int>())).Returns(0);

            // When.
            tagLogic.ApplyNoTagLogic();

            // Then.
            mockDb.Verify(d => d.GetSubteamOverride(It.IsAny<int>()), Times.Once);
            mockDb.Verify(d => d.GetRuleDefaultThreshold(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void NoTagLogic_LabelTypeIsNONEForAllItemsInTheBatch_ItemsShouldBeExcludedWithNoOtherRulesExecuted()
        {
            // Given.
            mockDb.Setup(db => db.GetLabelTypeExclusions(It.IsAny<List<int>>())).Returns(testItems);
            
            // When.
            tagLogic.ApplyNoTagLogic();

            // Then.
            Assert.IsTrue(tagLogic.ItemsExcluded);
            Assert.AreEqual(testItems.Count, tagLogic.ExcludedItems.Count);

            mockDb.Verify(db => db.GetLabelTypeExclusions(It.Is<List<int>>(items => testItems.SequenceEqual(items))), Times.Once);

            mockMovementRule.Verify(r => r.ApplyRule(It.IsAny<List<int>>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            mockOrderingRule.Verify(r => r.ApplyRule(It.IsAny<List<int>>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            mockReceivingRule.Verify(r => r.ApplyRule(It.IsAny<List<int>>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        //[TestMethod]
        //public void NoTagLogic_LabelTypeIsNONEForSomeItemsInTheBatch_ItemsShouldBeExcludedAndOtherRulesExecuted()
        //{
        //    // Given.
        //    var labelTypeExclusions = new List<int> { testItems[0] };

        //    mockDb.Setup(db => db.GetLabelTypeExclusions(It.IsAny<List<int>>(), It.IsAny<int>())).Returns(labelTypeExclusions);
        //    //mockMovementRule.Setup(r => r.ApplyRule(It.IsAny<List<int>>(), It.IsAny<int>()));

        //    mockMovementRule.SetupGet(r => r.ExcludedItems).Returns(new List<int>());
        //    mockOrderingRule.SetupGet(r => r.ExcludedItems).Returns(new List<int>());
        //    mockReceivingRule.SetupGet(r => r.ExcludedItems).Returns(new List<int>());

        //    // When.
        //    tagLogic.ApplyNoTagLogic();

        //    // Then.
        //    var nonLabelTypeExclusions = testItems.Except(labelTypeExclusions).ToList();
            
        //    Assert.IsTrue(tagLogic.ItemsExcluded);
        //    Assert.AreEqual(labelTypeExclusions.Count, tagLogic.ExcludedItems.Count);

        //    mockDb.Verify(db => db.GetLabelTypeExclusions(It.Is<List<int>>(items => testItems.SequenceEqual(items)),
        //        It.Is<int>(batchHeader => batchHeader == header.PriceBatchHeaderID)), Times.Once);

        //    mockMovementRule.Verify(r => r.ApplyRule(It.Is<List<int>>(items => nonLabelTypeExclusions.SequenceEqual(items)), 
        //        It.Is<int>(historyThreshold => historyThreshold == 0)), Times.Once);

        //    //mockOrderingRule.Verify(r => r.ApplyRule(It.Is<List<int>>(items => nonLabelTypeExclusions.SequenceEqual(items)),
        //    //    It.IsAny<int>()), Times.Once);

        //    //mockReceivingRule.Verify(r => r.ApplyRule(It.Is<List<int>>(items => nonLabelTypeExclusions.SequenceEqual(items)),
        //    //    It.IsAny<int>()), Times.Once);
        //}

        [TestMethod]
        public void NoTagLogic_LabelTypeIsNONEForSomeItemsInTheBatch_FailedItemsShouldBeExcludedAndOtherRulesExecuted()
        {
            // Given.
            var labelTypeExclusions = new List<int> { testItems[0] };

            mockDb.Setup(db => db.GetLabelTypeExclusions(It.IsAny<List<int>>())).Returns(labelTypeExclusions);
            
            mockMovementRule.SetupGet(r => r.ExcludedItems).Returns(new List<int>());
            mockOrderingRule.SetupGet(r => r.ExcludedItems).Returns(new List<int>());
            mockReceivingRule.SetupGet(r => r.ExcludedItems).Returns(new List<int>());

            // When.
            tagLogic.ApplyNoTagLogic();

            // Then.
            var nonLabelTypeExclusions = testItems.Except(labelTypeExclusions).ToList();

            Assert.IsTrue(tagLogic.ItemsExcluded);
            Assert.AreEqual(labelTypeExclusions.Count, tagLogic.ExcludedItems.Count);

            mockDb.Verify(db => db.GetLabelTypeExclusions(It.IsAny<List<int>>()), Times.Once);

            mockMovementRule.Verify(r => r.ApplyRule(It.IsAny<List<int>>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            mockOrderingRule.Verify(r => r.ApplyRule(It.IsAny<List<int>>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            mockReceivingRule.Verify(r => r.ApplyRule(It.IsAny<List<int>>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public void NoTagLogic_LabelTypeIsNONEForAllItemsInTheBatch_ItemsShouldBeWrittenToExclusionsTable()
        {
            // Given.
            mockDb.Setup(db => db.GetLabelTypeExclusions(It.IsAny<List<int>>())).Returns(testItems);

            // When.
            tagLogic.ApplyNoTagLogic();

            // Then.
            mockDb.Verify(db => db.WriteToNoTagExclusion(It.Is<List<int>>(items => testItems.SequenceEqual(items)),
                It.Is<int>(batchHeader => batchHeader == header.PriceBatchHeaderId), It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public void NoTagLogic_LabelTypeIsNONEAndBatchTypeIsNEW_LabelTypeLogicShouldStillApply()
        {
            // Given.
            header.ItemChgTypeID = 1;
            mockDb.Setup(db => db.GetLabelTypeExclusions(It.IsAny<List<int>>())).Returns(testItems);

            // When.
            tagLogic.ApplyNoTagLogic();

            // Then.
            Assert.IsTrue(tagLogic.ItemsExcluded);
            Assert.AreEqual(testItems.Count, tagLogic.ExcludedItems.Count);

            mockDb.Verify(db => db.GetLabelTypeExclusions(It.Is<List<int>>(items => testItems.SequenceEqual(items))), Times.Once);

            mockMovementRule.Verify(r => r.ApplyRule(It.IsAny<List<int>>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            mockOrderingRule.Verify(r => r.ApplyRule(It.IsAny<List<int>>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            mockReceivingRule.Verify(r => r.ApplyRule(It.IsAny<List<int>>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }
    }
}
