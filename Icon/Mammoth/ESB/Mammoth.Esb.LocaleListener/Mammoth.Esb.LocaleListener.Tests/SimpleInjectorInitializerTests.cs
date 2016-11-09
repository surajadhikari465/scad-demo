﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mammoth.Esb.LocaleListener.Tests
{
    [TestClass]
    public class SimpleInjectorInitializerTests
    {
        [TestMethod]
        public void InitializeSimpleInjectorContainer_ProgramIsNotInitialized_ShouldVerifyContentsAndNotThrowException()
        {
            //When
            var container = SimpleInjectorInitializer.InitializeContainer();
            container.Verify();

            //Then 
            Assert.IsNotNull(container);
        }
    }
}
