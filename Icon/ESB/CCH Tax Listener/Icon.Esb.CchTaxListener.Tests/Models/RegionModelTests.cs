using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Esb.CchTax.Models;
using System.Linq;
using System.Configuration;

namespace Icon.Esb.CchTax.Tests.Models
{
    [TestClass]
    public class RegionModelTests
    {
        [TestMethod]
        public void CreateRegionModelsFromConfig_12RegionsSetInAppConfig_ShouldReturnAnListWith12Regions()
        {
            //Given
            ConfigurationManager.AppSettings
                .Set("IconToIrmaTaxHierarchyUpdateEnabledRegionsList", "FL,MA,MW,NA,NC,NE,PN,RM,SO,SP,SW,UK");

            //When
            var regions = RegionModel.CreateRegionModelsFromConfig()
                .OrderBy(r => r.RegionAbbr)
                .ToList();

            //Then
            Assert.AreEqual("FL", regions[0].RegionAbbr);
            Assert.AreEqual("MA", regions[1].RegionAbbr);
            Assert.AreEqual("MW", regions[2].RegionAbbr);
            Assert.AreEqual("NA", regions[3].RegionAbbr);
            Assert.AreEqual("NC", regions[4].RegionAbbr);
            Assert.AreEqual("NE", regions[5].RegionAbbr);
            Assert.AreEqual("PN", regions[6].RegionAbbr);
            Assert.AreEqual("RM", regions[7].RegionAbbr);
            Assert.AreEqual("SO", regions[8].RegionAbbr);
            Assert.AreEqual("SP", regions[9].RegionAbbr);
            Assert.AreEqual("SW", regions[10].RegionAbbr);
            Assert.AreEqual("UK", regions[11].RegionAbbr);
        }

        [TestMethod]
        public void CreateRegionModelsFromConfig_NoRegionsSetInAppConfig_ShouldReturnAnEmptyList()
        {
            //Given
            ConfigurationManager.AppSettings
                .Set("IconToIrmaTaxHierarchyUpdateEnabledRegionsList", "");

            //When
            var regions = RegionModel.CreateRegionModelsFromConfig()
                .OrderBy(r => r.RegionAbbr)
                .ToList();

            //Then
            Assert.AreEqual(0, regions.Count);
        }
    }
}
