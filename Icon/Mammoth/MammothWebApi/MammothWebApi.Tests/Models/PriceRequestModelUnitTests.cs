using MammothWebApi.Models;
using MammothWebApi.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace MammothWebApi.Tests.Models
{
    [TestClass]
    public class PriceRequestModelUnitTests
    {
        //[TestMethod]
        //public void PriceRequestModel_ValidModel_PassesValidation()
        //{
        //    // arrange
        //    var expected = true;
        //    var model = new PriceRequestModel()
        //    {
        //        StoreItem = new StoreItem
        //        {
        //            BusinessUnitId = PriceTestData.Valid.FiveDigitBusinessUnit,
        //            ScanCode = PriceTestData.Valid.ThirteenDigitScanCode
        //        },
        //        EffectiveDate = PriceTestData.Valid.DateIn2018April
        //    };
        //    // act
        //    var actual = ViewModelValidationHelper.Validate(model);
        //    // assert
        //    Assert.AreEqual(expected, actual);
        //}

        //[TestMethod]
        //public void PriceRequestModel_ModelWithNegativeBusinessUnit_FailsValidation()
        //{
        //    // arrange
        //    var expected = false;
        //    var model = new PriceRequestModel()
        //    {
        //        BusinessUnitId = PriceTestData.Bad.NegativeBusinessUnit,
        //        ScanCode = PriceTestData.Valid.ThirteenDigitScanCode,
        //        EffectiveDate = PriceTestData.Valid.DateIn2018April
        //    };
        //    // act
        //    var actual = ViewModelValidationHelper.Validate(model);
        //    // assert
        //    Assert.AreEqual(expected, actual);
        //}

        //[TestMethod]
        //public void PriceRequestModel_ModelWithNullScanCode_FailsValidation()
        //{
        //    // arrange
        //    var expected = false;
        //    var model = new PriceRequestModel()
        //    {
        //        BusinessUnitId = PriceTestData.Valid.FiveDigitBusinessUnit,
        //        ScanCode =null,
        //        EffectiveDate = PriceTestData.Valid.DateIn2018April
        //    };
        //    // act
        //    var actual = ViewModelValidationHelper.Validate(model);
        //    // assert
        //    Assert.AreEqual(expected, actual);
        //}

        //[TestMethod]
        //public void PriceRequestModel_ModelWithOverlongScanCode_FailsValidation()
        //{
        //    // arrange
        //    var expected = false;
        //    var model = new PriceRequestModel()
        //    {
        //        BusinessUnitId = PriceTestData.Valid.FiveDigitBusinessUnit,
        //        ScanCode = PriceTestData.Bad.FourteenDigitScanCode,
        //        EffectiveDate = PriceTestData.Valid.DateIn2018April
        //    };
        //    // act
        //    var actual = ViewModelValidationHelper.Validate(model);
        //    // assert
        //    Assert.AreEqual(expected, actual);
        //}
        //[TestMethod]
        //public void PriceRequestModel_ModelWithDefaultEffectiveDate_FailsValidation()
        //{
        //    // arrange
        //    var expected = false;
        //    var model = new PriceRequestModel()
        //    {
        //        BusinessUnitId = PriceTestData.Valid.FiveDigitBusinessUnit,
        //        ScanCode = PriceTestData.Valid.ThirteenDigitScanCode,
        //        EffectiveDate = new DateTime()
        //    };
        //    // act
        //    var actual = ViewModelValidationHelper.Validate(model);
        //    // assert
        //    Assert.AreEqual(expected, actual);
        //}

        //[TestMethod]
        //public void PriceRequestModel_ModelWithEffectiveDateFrom2009_FailsValidation()
        //{
        //    // arrange
        //    var expected = false;
        //    var model = new PriceRequestModel()
        //    {
        //        BusinessUnitId = PriceTestData.Valid.FiveDigitBusinessUnit,
        //        ScanCode = PriceTestData.Valid.ThirteenDigitScanCode,
        //        EffectiveDate = PriceTestData.Bad.DateIn2009June
        //    };
        //    // act
        //    var actual = ViewModelValidationHelper.Validate(model);
        //    // assert
        //    Assert.AreEqual(expected, actual);
        //}

        //[TestMethod]
        //public void PriceRequestModel_ModelWith3InvalidProperties_FailsValidation()
        //{
        //    // arrange
        //    var expected = false;
        //    var model = new PriceRequestModel()
        //    {
        //        BusinessUnitId = PriceTestData.Bad.NegativeBusinessUnit,
        //        ScanCode = PriceTestData.Bad.FourteenDigitScanCode,
        //        EffectiveDate = PriceTestData.Bad.DateIn2009June
        //    };
        //    // act
        //    var actual = ViewModelValidationHelper.Validate(model);
        //    // assert
        //    Assert.AreEqual(expected, actual);
        //}

        //[TestMethod]
        //public void PriceRequestModel_ModelWith3InvalidProperties_FailsValidationWithCorrectErrorCount()
        //{
        //    // arrange
        //    var expected = 3;
        //    var modelStateDictionary = new ModelStateDictionary();
        //    var model = new PriceRequestModel()
        //    {
        //        BusinessUnitId = PriceTestData.Bad.NegativeBusinessUnit,
        //        ScanCode = PriceTestData.Bad.FourteenDigitScanCode,
        //        EffectiveDate = PriceTestData.Bad.DateIn2009June
        //    };
        //    // act
        //    var actual = ViewModelValidationHelper.Validate(model, ref modelStateDictionary);
        //    // assert
        //    Assert.AreEqual(expected, modelStateDictionary.Count);
        //}
    }
}
