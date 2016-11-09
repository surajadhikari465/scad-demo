namespace Icon.Dashboard.DataFileAccess.Tests.Models
{
    using DataFileAccess.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    [TestClass]
    public class ScheduledTaskModelTests
    {
        [TestMethod]
        public void WhenCreatingScheduledTaskModel_ThenConstructor_ShouldSetApplicationType()
        {
            // Given

            // When
            var scheduledTask = new ScheduledTask();

            // Then
            Assert.IsNotNull(scheduledTask);
            Assert.AreEqual(ApplicationTypeEnum.ScheduledTask, scheduledTask.TypeOfApplication);
        }
    }
}
