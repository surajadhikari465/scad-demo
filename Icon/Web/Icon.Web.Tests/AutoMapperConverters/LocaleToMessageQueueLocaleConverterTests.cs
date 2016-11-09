using AutoMapper;
using Icon.Framework;
using Icon.Web.Mvc.AutoMapperConverters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.Tests.Unit.AutoMapperConverters
{
    [TestClass]
    public class LocaleToMessageQueueLocaleConverterTests
    {
        [TestMethod]
        public void LocaleToMessageQueueConverter_ShouldConvert_LocaleName()
        {
            // Given.
            Mapper.CreateMap<Locale, MessageQueueLocale>().ConvertUsing<LocaleToMessageQueueLocaleConverter>();
            Locale source = TestHelpers.GetFakeLocaleWithAddress();
            Assert.IsNotNull(source.localeName,
                "sourece.localeName must have value for a valid test");

            // When.
            var result = Mapper.Map<MessageQueueLocale>(source);

            // Then.
            Assert.IsNotNull(result);
            Assert.AreEqual(result.LocaleName, source.localeName);
        }

        [TestMethod]
        public void LocaleToMessageQueueConverter_ShouldConvert_Address_Timezone_UsingPosTimeZoneName()
        {
            // Given.
            Mapper.CreateMap<Locale, MessageQueueLocale>().ConvertUsing<LocaleToMessageQueueLocaleConverter>();
            Locale source = TestHelpers.GetFakeLocaleWithAddress();
            Assert.IsNotNull(source.LocaleAddress.FirstOrDefault().Address.PhysicalAddress.Timezone,
                "LocaleAddress.Address.PhysicalAddress.Timezone must have value for a valid test");

            // When.
            var result = Mapper.Map<MessageQueueLocale>(source);

            // Then.
            Assert.IsNotNull(result);
            //should convert posTimeZoneName not timezoneName
            Assert.AreEqual(result.TimezoneName, source.LocaleAddress.FirstOrDefault().Address.PhysicalAddress.Timezone.posTimeZoneName);
        }
    }
}
