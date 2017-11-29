using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Icon.Infor.Listeners.HierarchyClass.ErrorUtility;
using Icon.Infor.Listeners.HierarchyClass.Commands;
using Icon.Infor.Listeners.HierarchyClass.Constants;

namespace Icon.Infor.Listeners.HierarchyClass.Tests.ErrorUtility
{
    [TestClass]
    public class ErrorMapperTests
    {
        private ErrorMapper errorMapper;

        [TestInitialize]
        public void Initialize()
        {
            errorMapper = new ErrorMapper();
        }

        [TestMethod]
        public void GetAssociatedErrorCode_AddOrUpdateHierarchyClassesCommandHandler_AddOrUpdateHierarchyClassError()
        {
            //When
            var errorCode = errorMapper.GetAssociatedErrorCode(typeof(AddOrUpdateHierarchyClassesCommandHandler));

            //Then
            Assert.AreEqual(ApplicationErrors.Codes.AddOrUpdateHierarchyClassError, errorCode);
        }

        [TestMethod]
        public void GetAssociatedErrorCode_DeleteHierarchyClassesCommandHandler_DeleteHierarchyClassError()
        {
            //When
            var errorCode = errorMapper.GetAssociatedErrorCode(typeof(DeleteHierarchyClassesCommandHandler));

            //Then
            Assert.AreEqual(ApplicationErrors.Codes.DeleteHierarchyClassError, errorCode);
        }

        [TestMethod]
        public void GetAssociatedErrorCode_NonDefinedType_UnexpectedError()
        {
            //When
            var errorCode = errorMapper.GetAssociatedErrorCode(typeof(string));

            //Then
            Assert.AreEqual(ApplicationErrors.Codes.UnexpectedError, errorCode);
        }

        [TestMethod]
        public void GetFormattedErrorDetails_AddOrUpdateHierarchyClassesCommandHandler_AddOrUpdateHierarchyClassError()
        {
            //Given
            Exception exception = new Exception("Test");

            //When
            var errorDetails = errorMapper.GetFormattedErrorDetails(typeof(AddOrUpdateHierarchyClassesCommandHandler), exception);

            //Then
            Assert.AreEqual($"{ApplicationErrors.Descriptions.AddOrUpdateHierarchyClassError} Exception Details: {exception.ToString()}", errorDetails);
        }

        [TestMethod]
        public void GetFormattedErrorDetails_DeleteHierarchyClassesCommandHandler_DeleteHierarchyClassError()
        {
            //Given
            Exception exception = new Exception("Test");

            //When
            var errorDetails = errorMapper.GetFormattedErrorDetails(typeof(DeleteHierarchyClassesCommandHandler), exception);

            //Then
            Assert.AreEqual($"{ApplicationErrors.Descriptions.DeleteHierarchyClassError} Exception Details: {exception.ToString()}", errorDetails);
        }

        [TestMethod]
        public void GetFormattedErrorDetails_NonDefinedType_UnexpectedError()
        {
            //Given
            Exception exception = new Exception("Test");

            //When
            var errorDetails = errorMapper.GetFormattedErrorDetails(typeof(string), exception);

            //Then
            Assert.AreEqual($"{ApplicationErrors.Descriptions.UnexpectedError} Exception Details: {exception.ToString()}", errorDetails);
        }
    }
}
