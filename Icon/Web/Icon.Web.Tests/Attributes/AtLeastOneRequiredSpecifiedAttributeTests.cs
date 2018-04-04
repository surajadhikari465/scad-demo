﻿using Icon.Web.Attributes;
using Icon.Web.Mvc.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Icon.Web.Tests.Unit.Attributes
{
    [TestClass] [Ignore]
    public class AtLeastOneRequiredSpecifiedAttributeTests
    {
        [TestMethod]
        public void AtLeastOneRequiredSpecifiedAttribute_NoValueIsPresent_ShouldReturnFailure()
        {
            //Given
            AtLeastOneRequiredSpecifiedAttribute attribute = new AtLeastOneRequiredSpecifiedAttribute();

            //When
            var result = attribute.IsValid(new ItemSearchViewModel());

            //Then
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void AtLeastOneRequiredSpecifiedAttribute_ValueIsPresent_ShouldReturnSuccess()
        {
            //Given
            AtLeastOneRequiredSpecifiedAttribute attribute = new AtLeastOneRequiredSpecifiedAttribute();

            //When
            var result = attribute.IsValid(new ItemSearchViewModel { ScanCode = "12345" });

            //Then
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void AtLeastOneRequiredSpecifiedAttribute_ValueIsPresentInPropertyOfProperty_ShouldReturnSuccess()
        {

            //Given
            AtLeastOneRequiredSpecifiedAttribute attribute = new AtLeastOneRequiredSpecifiedAttribute();

            //When
            var result = attribute.IsValid(new ItemSearchViewModel { ItemSignAttributes = new ItemSignAttributesSearchViewModel { KosherAgency = "Test Kosher" } });

            //Then
            Assert.IsTrue(result);
        }
    }
}
