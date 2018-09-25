using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AmazonLoad.Common.Tests
{
    [TestClass]
    public class CalcBatchSizeTests
    {
        private const int defaultBatchSize = 100;

        [TestMethod]
        public void Common_Utils_CalcBatchSize_WhenMaxRowsIsZeroReturnsDefaultSize()
        {
            // Given
            int maxNumberOfRows = 0;
            int numberOfRecordsSent = 0;
            int expectedBatchSize = defaultBatchSize;

            // When
            var batchSize = Utils.CalcBatchSize(defaultBatchSize, maxNumberOfRows, numberOfRecordsSent);

            // Then
            Assert.AreEqual(expectedBatchSize, batchSize);
        }

        [TestMethod]
        public void Common_Utils_CalcBatchSize_WhenMaxRowsSmallerThanDefaultReturnsMaxRowsValue()
        {
            // Given
            int maxNumberOfRows = 50;
            int numberOfRecordsSent = 0;
            int expectedBatchSize = 50;

            // When
            var batchSize = Utils.CalcBatchSize(defaultBatchSize, maxNumberOfRows, numberOfRecordsSent);

            // Then
            Assert.AreEqual(expectedBatchSize, batchSize);
        }

        [TestMethod]
        public void Common_Utils_CalcBatchSize_WhenMaxRowsExceedsBatchSizeAndNoMessagesSent_ReturnsDefaultBatchSize()
        {
            // Given
            int maxNumberOfRows = 10000;
            int numberOfRecordsSent = 0;
            int expectedBatchSize = defaultBatchSize;

            // When
            var batchSize = Utils.CalcBatchSize(defaultBatchSize, maxNumberOfRows, numberOfRecordsSent);

            // Then
            Assert.AreEqual(expectedBatchSize, batchSize);
        }

        [TestMethod]
        public void Common_Utils_CalcBatchSize_WhenMaxRowsSmallerThanBatchSizeAndSomeMessagesAlreadySent_ReturnsNumberOfUnsentROws()
        {
            // Given
            int maxNumberOfRows = 30;
            int numberOfRecordsSent = 20;
            int expectedBatchSize = maxNumberOfRows - numberOfRecordsSent;

            // When
            var batchSize = Utils.CalcBatchSize(defaultBatchSize, maxNumberOfRows, numberOfRecordsSent);

            // Then
            Assert.AreEqual(expectedBatchSize, batchSize);
        }

        [TestMethod]
        public void Common_Utils_CalcBatchSize_WhenMaxRowsLargerThanBatchSizeAndSomeMessagesAlreadySent_ReturnsNumberOfUnsentRowsUpToBatchSize()
        {
            // Given
            int maxNumberOfRows = 500;
            int numberOfRecordsSent = 300;
            int expectedBatchSize = defaultBatchSize;

            // When
            var batchSize = Utils.CalcBatchSize(defaultBatchSize, maxNumberOfRows, numberOfRecordsSent);

            // Then
            Assert.AreEqual(expectedBatchSize, batchSize);
        }

        [TestMethod]
        public void Common_Utils_CalcBatchSize_WhenMaxRowsSmallerThanSentMessagesReturnsNegative()
        {
            // Given
            int maxNumberOfRows = 22;
            int numberOfRecordsSent = 33;
            int expectedBatchSize = -1;

            // When
            var batchSize = Utils.CalcBatchSize(defaultBatchSize, maxNumberOfRows, numberOfRecordsSent);

            // Then
            Assert.AreEqual(expectedBatchSize, batchSize);
        }
    }
}
