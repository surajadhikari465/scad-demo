using Infor.Services.NewItem.Notifiers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infor.Services.NewItem.Tests.Notifiers
{
    [TestClass]
    public class NewItemNotifierSettingsTests
    {
        private NewItemNotifierSettings settings;

        [TestMethod]
        public void CreateFromConfig_FLNotificationSettingsExist_BuildsEnabledRegionDictionaryWithFL()
        {
            //When
            settings = NewItemNotifierSettings.CreateFromConfig();

            //Then
            Assert.AreEqual(true, settings.RegionalNotificationEnabled["FL"]);
        }

        [TestMethod]
        public void CreateFromConfig_MWNotificationSettingsDontExist_BuildsEnabledDictionaryWithoutMW()
        {
            //When
            settings = NewItemNotifierSettings.CreateFromConfig();

            //Then
            Assert.AreEqual(true, settings.RegionalNotificationEnabled["FL"]);
            Assert.IsFalse(settings.RegionalNotificationEnabled.ContainsKey("MW"));
        }
    }
}
