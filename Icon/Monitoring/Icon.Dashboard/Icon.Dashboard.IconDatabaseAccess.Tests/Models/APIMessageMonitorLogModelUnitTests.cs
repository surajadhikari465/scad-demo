using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;

namespace Icon.Dashboard.IconDatabaseAccess.Tests
{
    [TestClass]
    public class APIMessageMonitorLogModelUnitTests
    {
        [TestMethod]
        public void APIMessageMonitorLogMessageTypeNameProperty_WhenMessageTypeIsNotNull_ShouldGetNameValue()
        {
            //Given
            const int expectedMessageTypeId = 555;
            const string expectedMessageTypeName = "DeliveryNotice";
            MessageType messageType = new MessageType()
            {
                MessageTypeId = 555,
                MessageTypeName = expectedMessageTypeName
            };
            APIMessageMonitorLog logEntry = new APIMessageMonitorLog()
            {
                APIMessageMonitorLogID = 1,
                MessageTypeID = expectedMessageTypeId,
                StartTime = new DateTime(2016, 10, 24, 18, 30, 0),
                EndTime = new DateTime(2016, 10, 24, 18, 30, 30),
                CountProcessedMessages = 12,
                CountFailedMessages = 0,
                MessageType = messageType
            };
            //When
            var actualMessageTypeName = logEntry.MessageTypeName;
            //Then
            actualMessageTypeName.Should().BeEquivalentTo(expectedMessageTypeName);
        }

        [TestMethod]
        public void APIMessageMonitorLogMessageTypeNameProperty_WhenMessageTypeIsNull_ShouldGetNull()
        {
            //Given
            const int expectedMessageTypeId = 555;
            const string expectedMessageTypeName = null;
            MessageType messageType = null;
            APIMessageMonitorLog logEntry = new APIMessageMonitorLog()
            {
                APIMessageMonitorLogID = 1,
                MessageTypeID = expectedMessageTypeId,
                StartTime = new DateTime(2016, 10, 24, 18, 30, 0),
                EndTime = new DateTime(2016, 10, 24, 18, 30, 30),
                CountProcessedMessages = 12,
                CountFailedMessages = 0,
                MessageType = messageType
            };
            //When
            var actualMessageTypeName = logEntry.MessageTypeName;
            //Then
            actualMessageTypeName.Should().BeEquivalentTo(expectedMessageTypeName);
        }
    }
}
