using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace SupportMonitor.Core.Tests
{
    [TestClass]
    public class ApplicationReaderTests
    {
        [TestMethod]
        public void GetApplicationsOnServer_ShouldReturnTasks()
        {
            ApplicationReader reader = new ApplicationReader();
            var results = reader.GetApplicationsOnServer("me", new List<string> { "hello", "world" });
            Assert.IsTrue(results.Any());
            foreach (var item in results)
            {
                Console.WriteLine(item.Name + " || " + item.Status);
            }
        }
    }
}
