using AutoMapper;
using Icon.Framework;
using Icon.Web.Mvc.App_Start;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Icon.Web.Tests.Unit.AutoMapperConverters
{
    [TestClass]
    public class LocaleToMessageQueueLocaleConverterTests
    {
        IMapper mapper;

        [TestInitialize]
        public void Initialize()
        {
            mapper = AutoMapperWebConfiguration.Configure();
        }

        [TestMethod]
        public void LocaleToMessageQueueConverter_ShouldConvert_LocaleName()
        {
            // Given.
            Locale source = TestHelpers.GetFakeLocaleWithAddress();
            Assert.IsNotNull(source.localeName,
                "sourece.localeName must have value for a valid test");

            // When.
            var result = mapper.Map<MessageQueueLocale>(source);

            // Then.
            Assert.IsNotNull(result);
            Assert.AreEqual(result.LocaleName, source.localeName);
        }

        [TestMethod]
        public void LocaleToMessageQueueConverter_ShouldConvert_Address_Timezone_UsingPosTimeZoneName()
        {
            // Given.
            Locale source = TestHelpers.GetFakeLocaleWithAddress();
            Assert.IsNotNull(source.LocaleAddress.FirstOrDefault().Address.PhysicalAddress.Timezone,
                "LocaleAddress.Address.PhysicalAddress.Timezone must have value for a valid test");

            // When.
            var result = mapper.Map<MessageQueueLocale>(source);

            // Then.
            Assert.IsNotNull(result);
            //should convert posTimeZoneName not timezoneName
            Assert.AreEqual(result.TimezoneName, source.LocaleAddress.FirstOrDefault().Address.PhysicalAddress.Timezone.posTimeZoneName);
        }

        [TestMethod]
        public void LocaleToMessageQueueConverter_ShouldNotConvertNullCloseDateToMaxDate()
        {
            // Given.
            Locale source = TestHelpers.GetFakeLocaleWithAddress();
            source.localeCloseDate = null;
            Assert.IsNotNull(source.LocaleAddress.FirstOrDefault().Address.PhysicalAddress.Timezone,
                "LocaleAddress.Address.PhysicalAddress.Timezone must have value for a valid test");

            // When.
            var result = mapper.Map<MessageQueueLocale>(source);

            // Then.
            Assert.IsNotNull(result);
            //should leave close date as null
            Assert.IsNull(result.LocaleCloseDate);
        }
    }
}
