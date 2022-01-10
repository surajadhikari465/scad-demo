using Icon.Esb.Schemas.Wfm.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Icon.Services.ItemPublisher.Infrastructure.MessageQueue.Tests
{
    [TestClass]
    public class TraitMessageBuilderTests
    {
        /// <summary>
        /// Tests the we can build a boolean trait
        /// </summary>
        [TestMethod()]
        public void BuildTrait_Bool()
        {
            // Given.
            var builder = new TraitMessageBuilder();

            // When.
            TraitType trait = builder.BuildTrait("TST", "TEST", true);

            // Then.
            Assert.AreEqual("TST", trait.code);
            Assert.AreEqual("TEST", trait.type.description);
            Assert.AreEqual("1", trait.type.value.First().value);
        }

        /// <summary>
        /// Tests the we can build a nullable boolean trait
        /// </summary>
        [TestMethod()]
        public void BuildTraitTest_NullableBool()
        {
            // Given.
            var builder = new TraitMessageBuilder();

            bool? value = true;
            // When.
            TraitType trait = builder.BuildTrait("TST", "TEST", value);

            // Then.
            Assert.AreEqual("TST", trait.code);
            Assert.AreEqual("TEST", trait.type.description);
            Assert.AreEqual("1", trait.type.value.First().value);
        }

        /// <summary>
        /// Tests the we can build a nullable boolean trait and that we force an empty string if it's null
        /// </summary>
        [TestMethod()]
        public void BuildTraitLeaveBlankIfNullTest()
        {
            // Given.
            var builder = new TraitMessageBuilder();
            bool? value = null;

            // When.
            TraitType trait = builder.BuildTraitLeaveBlankIfNull("TST", "TEST", value);

            // Then.
            Assert.AreEqual("TST", trait.code);
            Assert.AreEqual("TEST", trait.type.description);
            Assert.AreEqual(string.Empty, trait.type.value.First().value);
        }

        /// <summary>
        /// Tests the we can build a nullable int trait
        /// </summary>
        [TestMethod()]
        public void BuildTraitTest_NullableInt()
        {
            // Given.
            var builder = new TraitMessageBuilder();
            int? value = 1;

            // When.
            TraitType trait = builder.BuildTrait("TST", "TEST", value);

            // Then.
            Assert.AreEqual("TST", trait.code);
            Assert.AreEqual("TEST", trait.type.description);
            Assert.AreEqual("1", trait.type.value.First().value);
        }

        /// <summary>
        /// Tests the we can build a nullable double trait
        /// </summary>
        [TestMethod()]
        public void BuildTraitTest_NullableDouble()
        {
            // Given.
            var builder = new TraitMessageBuilder();
            double? value = 1;

            // When.
            TraitType trait = builder.BuildTrait("TST", "TEST", value);

            // Then.
            Assert.AreEqual("TST", trait.code);
            Assert.AreEqual("TEST", trait.type.description);
            Assert.AreEqual("1", trait.type.value.First().value);
        }

        /// <summary>
        /// Tests the we can build a nullable decimal trait
        /// </summary>
        [TestMethod()]
        public void BuildTrait_NullableDecimal()
        {
            // Given.
            var builder = new TraitMessageBuilder();

            decimal? value = 1;
            // When.
            TraitType trait = builder.BuildTrait("TST", "TEST", value);

            // Then.
            Assert.AreEqual("TST", trait.code);
            Assert.AreEqual("TEST", trait.type.description);
            Assert.AreEqual("1", trait.type.value.First().value);
        }

        /// <summary>
        /// Tests the we can build a UOM trait
        /// </summary>
        [TestMethod()]
        public void BuildTraitTest_UOM()
        {
            // Given.
            var builder = new TraitMessageBuilder();

            UomType value = new UomType();
            value.code = WfmUomCodeEnumType.BC;
            value.codeSpecified = true;
            value.name = WfmUomDescEnumType.BAG;
            value.nameSpecified = true;

            // When.
            TraitType trait = builder.BuildTrait("TST", "TEST", "VALUE", value);

            // Then.
            Assert.AreEqual("TST", trait.code);
            Assert.AreEqual("TEST", trait.type.description);
            Assert.AreEqual("VALUE", trait.type.value.First().value);
            Assert.AreEqual(WfmUomCodeEnumType.BC, trait.type.value.First().uom.code);
            Assert.AreEqual(WfmUomDescEnumType.BAG, trait.type.value.First().uom.name);
        }

        /// <summary>
        /// Tests the we can build a nullable string trait
        /// </summary>
        [TestMethod()]
        public void BuildTraitTest_String()
        {
            // Given.
            var builder = new TraitMessageBuilder();

            string value = "VALUE";

            // When.
            TraitType trait = builder.BuildTrait("TST", "TEST", value);

            // Then.
            Assert.AreEqual("TST", trait.code);
            Assert.AreEqual("TEST", trait.type.description);
            Assert.AreEqual("VALUE", trait.type.value.First().value);
        }
    }
}