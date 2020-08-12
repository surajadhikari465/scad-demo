using Icon.Common;
using Icon.Logging;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Controllers;
using Icon.Web.Mvc.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;

using Icon.Common.DataAccess;
using Icon.Web.Tests.Common.Builders;

namespace Icon.Web.Tests.Unit.Controllers
{
    [TestClass]
    public class BulkScanCodeSearchControllerTests
    {
        private BulkScanCodeSearchController controller;
        private Mock<ILogger> mockLogger;
        private Mock<IQueryHandler<GetItemsByBulkScanCodeSearchParameters, List<ItemSearchModel>>> mockGetSearchResultsQueryHandler;
        private Mock<IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel>> mockGetHierarchyLineageQueryHandler;
        private Mock<IQueryHandler<GetCertificationAgenciesParameters, List<CertificationAgencyModel>>> mockGetCertificationAgenciesQuery;

        [TestInitialize]
        public void Initialize()
        {
            mockLogger = new Mock<ILogger>();
            mockGetSearchResultsQueryHandler = new Mock<IQueryHandler<GetItemsByBulkScanCodeSearchParameters, List<ItemSearchModel>>>();
            mockGetHierarchyLineageQueryHandler = new Mock<IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel>>();
            mockGetCertificationAgenciesQuery = new Mock<IQueryHandler<GetCertificationAgenciesParameters, List<CertificationAgencyModel>>>();

            mockGetCertificationAgenciesQuery.Setup(q => q.Search(It.IsAny<GetCertificationAgenciesParameters>())).Returns(BuildAgencies());
            controller = new BulkScanCodeSearchController(
                mockLogger.Object,
                mockGetSearchResultsQueryHandler.Object,
                mockGetHierarchyLineageQueryHandler.Object,
                mockGetCertificationAgenciesQuery.Object);
        }

        [TestMethod]
        public void BulkScanCodeSearchControllerIndex_InitialPageLoad_DefaultViewShouldBeReturned()
        {
            // Given.
            int searchLimit = AppSettingsAccessor.GetIntSetting("BulkScanCodeSearchLimit", 3000);

            // When.
            var result = controller.Index() as ViewResult;

            // Then.
            var viewModel = result.Model as BulkScanCodeSearchViewModel;

            Assert.AreEqual(result.ViewName, String.Empty);
            Assert.AreEqual(searchLimit, viewModel.BulkScanCodeSearchLimit);
        }

        [TestMethod]
        public void BulkScanCodeSearchControllerSearch_InvalidModelState_IndexViewShouldBeReturnedWithViewModelIntact()
        {
            // Given.
            controller.ModelState.AddModelError("test", "test");

            var searchScanCodes = String.Join(Environment.NewLine, new string[] { "222222", "333333" });

            var viewModel = new BulkScanCodeSearchViewModel
            {
                TextAreaViewModel = new BulkScanCodeSearchTextAreaViewModel
                {
                    ScanCodes = searchScanCodes
                }
            };

            // When.
            var result = controller.Search(viewModel) as ViewResult;

            // Then.
            Assert.AreEqual(result.ViewName, "Index");
            Assert.IsNotNull(result.Model);
            Assert.AreEqual(searchScanCodes, (result.Model as BulkScanCodeSearchViewModel).TextAreaViewModel.ScanCodes);
        }

        [TestMethod]
        public void BulkScanCodeSearchControllerSearch_ValidModelState_IndexViewShouldBeReturnedWithHierarchyAndUomListsPopulated()
        {
            // Given.
            var hierarchyClassListModel = new HierarchyClassListModel
            {
                BrandHierarchyList = new List<HierarchyClassModel>(),
                TaxHierarchyList = new List<HierarchyClassModel>(),
                MerchandiseHierarchyList = new List<HierarchyClassModel>(),
                NationalHierarchyList = new List<HierarchyClassModel>()
            };

            mockGetHierarchyLineageQueryHandler.Setup(q => q.Search(It.IsAny<GetHierarchyLineageParameters>())).Returns(hierarchyClassListModel);
            mockGetSearchResultsQueryHandler.Setup(q => q.Search(It.IsAny<GetItemsByBulkScanCodeSearchParameters>())).Returns(new List<ItemSearchModel>());
            mockGetCertificationAgenciesQuery.Setup(q => q.Search(It.IsAny<GetCertificationAgenciesParameters>())).Returns(BuildAgencies());

            var searchScanCodes = new string[] { "222222", "333333" };

            var viewModel = new BulkScanCodeSearchViewModel
            {
                TextAreaViewModel = new BulkScanCodeSearchTextAreaViewModel
                {
                    ScanCodes = String.Join(Environment.NewLine, searchScanCodes)
                }
            };

            // When.
            var result = controller.Search(viewModel) as ViewResult;

            // Then.
            var returnedViewModel = result.Model as BulkScanCodeSearchViewModel;

            Assert.IsNotNull(returnedViewModel.ItemSearchResults.BrandHierarchyClasses);
            Assert.IsNotNull(returnedViewModel.ItemSearchResults.TaxHierarchyClasses);
            Assert.IsNotNull(returnedViewModel.ItemSearchResults.MerchandiseHierarchyClasses);
            Assert.IsNotNull(returnedViewModel.ItemSearchResults.RetailUoms);
        }

        [TestMethod]
        public void BulkScanCodeSearchControllerSearch_ValidModelState_IndexViewShouldBeReturnedWithSearchResults()
        {
            // Given.
            var hierarchyClassListModel = new HierarchyClassListModel
            {
                BrandHierarchyList = new List<HierarchyClassModel>(),
                TaxHierarchyList = new List<HierarchyClassModel>(),
                MerchandiseHierarchyList = new List<HierarchyClassModel>(),
                NationalHierarchyList = new List<HierarchyClassModel>()
            };

            var foundScanCodes = new List<ItemSearchModel>
            {
                new TestItemSearchModelBuilder()
                    .WithScanCode("222222"),
                new TestItemSearchModelBuilder()
                    .WithScanCode("333333")
            };

            mockGetHierarchyLineageQueryHandler.Setup(q => q.Search(It.IsAny<GetHierarchyLineageParameters>())).Returns(hierarchyClassListModel);
            mockGetSearchResultsQueryHandler.Setup(q => q.Search(It.IsAny<GetItemsByBulkScanCodeSearchParameters>())).Returns(foundScanCodes);
            mockGetCertificationAgenciesQuery.Setup(q => q.Search(It.IsAny<GetCertificationAgenciesParameters>())).Returns(BuildAgencies());

            var searchScanCodes = new string[] { "222222", "333333" };

            var viewModel = new BulkScanCodeSearchViewModel
            {
                TextAreaViewModel = new BulkScanCodeSearchTextAreaViewModel
                {
                    ScanCodes = String.Join(Environment.NewLine, searchScanCodes)
                }
            };

            // When.
            var result = controller.Search(viewModel) as ViewResult;

            // Then.
            var returnedViewModel = result.Model as BulkScanCodeSearchViewModel;

            Assert.AreEqual(0, returnedViewModel.InvalidOrNotFoundScanCodes.Count);
        }

        [TestMethod]
        public void BulkScanCodeSearchControllerSearch_InputContainsInvalidOrNotFoundScanCodes_IndexViewShouldBeReturnedWithInvalidOrNotFoundScanCodes()
        {
            // Given.
            var hierarchyClassListModel = new HierarchyClassListModel
            {
                BrandHierarchyList = new List<HierarchyClassModel>(),
                TaxHierarchyList = new List<HierarchyClassModel>(),
                MerchandiseHierarchyList = new List<HierarchyClassModel>(),
                NationalHierarchyList = new List<HierarchyClassModel>()
            };

            mockGetHierarchyLineageQueryHandler.Setup(q => q.Search(It.IsAny<GetHierarchyLineageParameters>())).Returns(hierarchyClassListModel);
            mockGetSearchResultsQueryHandler.Setup(q => q.Search(It.IsAny<GetItemsByBulkScanCodeSearchParameters>())).Returns(new List<ItemSearchModel>());
            mockGetCertificationAgenciesQuery.Setup(q => q.Search(It.IsAny<GetCertificationAgenciesParameters>())).Returns(BuildAgencies());

            var notFoundScanCode = new ItemSearchModel { ScanCode = "444444" };
            var invalidScanCode = new ItemSearchModel { ScanCode = "abcdefg" };

            var searchScanCodes = new string[] { notFoundScanCode.ScanCode, invalidScanCode.ScanCode };

            var viewModel = new BulkScanCodeSearchViewModel
            {
                TextAreaViewModel = new BulkScanCodeSearchTextAreaViewModel
                {
                    ScanCodes = String.Join(Environment.NewLine, searchScanCodes)
                }
            };

            // When.
            var result = controller.Search(viewModel) as ViewResult;

            // Then.
            var returnedViewModel = result.Model as BulkScanCodeSearchViewModel;

            Assert.AreEqual(2, returnedViewModel.InvalidOrNotFoundScanCodes.Count);
            Assert.AreEqual(notFoundScanCode.ScanCode, returnedViewModel.InvalidOrNotFoundScanCodes[0]);
            Assert.AreEqual(invalidScanCode.ScanCode, returnedViewModel.InvalidOrNotFoundScanCodes[1]);
        }

        [TestMethod]
        public void BulkScanCodeSearchControllerSearch_InputContainsScanCodesOverTheSearchLimit_IndexViewShouldBeReturnedWithCountOfScanCodesOverTheLimit()
        {
            // Given.
            var hierarchyClassListModel = new HierarchyClassListModel
            {
                BrandHierarchyList = new List<HierarchyClassModel>(),
                TaxHierarchyList = new List<HierarchyClassModel>(),
                MerchandiseHierarchyList = new List<HierarchyClassModel>(),
                NationalHierarchyList = new List<HierarchyClassModel>()
            };

            mockGetHierarchyLineageQueryHandler.Setup(q => q.Search(It.IsAny<GetHierarchyLineageParameters>())).Returns(hierarchyClassListModel);
            mockGetSearchResultsQueryHandler.Setup(q => q.Search(It.IsAny<GetItemsByBulkScanCodeSearchParameters>())).Returns(new List<ItemSearchModel>());
            mockGetCertificationAgenciesQuery.Setup(q => q.Search(It.IsAny<GetCertificationAgenciesParameters>())).Returns(BuildAgencies());

            int searchLimit = AppSettingsAccessor.GetIntSetting("BulkScanCodeSearchLimit", 3000);
            int overTheLimit = 2;

            var searchScanCodes = new string[searchLimit + overTheLimit];

            for (int i = 0; i < searchScanCodes.Length; i++)
            {
                searchScanCodes[i] = "222222";
            }

            var viewModel = new BulkScanCodeSearchViewModel
            {
                TextAreaViewModel = new BulkScanCodeSearchTextAreaViewModel
                {
                    ScanCodes = String.Join(Environment.NewLine, searchScanCodes)
                }
            };

            // When.
            var result = controller.Search(viewModel) as ViewResult;

            // Then.
            var returnedViewModel = result.Model as BulkScanCodeSearchViewModel;

            Assert.AreEqual(overTheLimit, returnedViewModel.OverLimitScanCodeCount);
        }

        [TestMethod]
        public void BulkScanCodeSearchControllerSearch_InputContainsDuplicateScanCodes_DuplicatesShouldBeRemoved()
        {
            // Given.
            var hierarchyClassListModel = new HierarchyClassListModel
            {
                BrandHierarchyList = new List<HierarchyClassModel>(),
                TaxHierarchyList = new List<HierarchyClassModel>(),
                MerchandiseHierarchyList = new List<HierarchyClassModel>(),
                NationalHierarchyList = new List<HierarchyClassModel>()
            };

            var foundScanCodes = new List<ItemSearchModel>
            {
                new TestItemSearchModelBuilder()
                    .WithScanCode("222222"),
                new TestItemSearchModelBuilder()
                    .WithScanCode("333333")
            };

            mockGetHierarchyLineageQueryHandler.Setup(q => q.Search(It.IsAny<GetHierarchyLineageParameters>())).Returns(hierarchyClassListModel);
            mockGetSearchResultsQueryHandler.Setup(q => q.Search(It.IsAny<GetItemsByBulkScanCodeSearchParameters>())).Returns(foundScanCodes);
            mockGetCertificationAgenciesQuery.Setup(q => q.Search(It.IsAny<GetCertificationAgenciesParameters>())).Returns(BuildAgencies());

            var searchScanCodes = new string[] { "222222", "333333", "222222" };

            var viewModel = new BulkScanCodeSearchViewModel
            {
                TextAreaViewModel = new BulkScanCodeSearchTextAreaViewModel
                {
                    ScanCodes = String.Join(Environment.NewLine, searchScanCodes)
                }
            };

            // When.
            var result = controller.Search(viewModel) as ViewResult;

            // Then.
            var returnedViewModel = result.Model as BulkScanCodeSearchViewModel;

            Assert.AreEqual(1, returnedViewModel.ItemSearchResults.KosherAgencies.Count);
            Assert.AreEqual(1, returnedViewModel.ItemSearchResults.MerchandiseHierarchyClasses.Count);
            Assert.AreEqual(1, returnedViewModel.ItemSearchResults.NationalHierarchyClasses.Count);
            Assert.AreEqual(1, returnedViewModel.ItemSearchResults.NonGmoAgencies.Count);
            Assert.AreEqual(1, returnedViewModel.ItemSearchResults.OrganicAgencies.Count);
            Assert.AreEqual(1, returnedViewModel.ItemSearchResults.VeganAgencies.Count);
            Assert.AreEqual(0, returnedViewModel.InvalidOrNotFoundScanCodes.Count);
        }

        [TestMethod]
        public void BulkScanCodeSearchControllerSearch_InputContainsWhitespaceLines_WhitespaceLinesShouldBeRemoved()
        {
            // Given.
            var hierarchyClassListModel = new HierarchyClassListModel
            {
                BrandHierarchyList = new List<HierarchyClassModel>(),
                TaxHierarchyList = new List<HierarchyClassModel>(),
                MerchandiseHierarchyList = new List<HierarchyClassModel>(),
                NationalHierarchyList = new List<HierarchyClassModel>()
            };

            var foundScanCodes = new List<ItemSearchModel>
            {
                new TestItemSearchModelBuilder()
                    .WithScanCode("222222"),
                new TestItemSearchModelBuilder()
                    .WithScanCode("333333")
            };

            mockGetHierarchyLineageQueryHandler.Setup(q => q.Search(It.IsAny<GetHierarchyLineageParameters>())).Returns(hierarchyClassListModel);
            mockGetSearchResultsQueryHandler.Setup(q => q.Search(It.IsAny<GetItemsByBulkScanCodeSearchParameters>())).Returns(foundScanCodes);
            mockGetCertificationAgenciesQuery.Setup(q => q.Search(It.IsAny<GetCertificationAgenciesParameters>())).Returns(BuildAgencies());

            var searchScanCodes = new string[] { "", "   ", "222222", "333333" };

            var viewModel = new BulkScanCodeSearchViewModel
            {
                TextAreaViewModel = new BulkScanCodeSearchTextAreaViewModel
                {
                    ScanCodes = String.Join(Environment.NewLine, searchScanCodes)
                }
            };

            // When.
            var result = controller.Search(viewModel) as ViewResult;

            // Then.
            var returnedViewModel = result.Model as BulkScanCodeSearchViewModel;

            Assert.AreEqual(0, returnedViewModel.InvalidOrNotFoundScanCodes.Count);
        }

        [TestMethod]
        public void BulkScanCodeSearchControllerSearch_InputContainsScanCodesGreaterThan13Characters_TooLongScanCodesShouldBeRemovedAndAddedToInvalidCollection()
        {
            // Given.
            var hierarchyClassListModel = new HierarchyClassListModel
            {
                BrandHierarchyList = new List<HierarchyClassModel>(),
                TaxHierarchyList = new List<HierarchyClassModel>(),
                MerchandiseHierarchyList = new List<HierarchyClassModel>(),
                NationalHierarchyList = new List<HierarchyClassModel>()
            };

            var foundScanCodes = new List<ItemSearchModel>
            {
                new TestItemSearchModelBuilder()
                    .WithScanCode("222222"),
                new TestItemSearchModelBuilder()
                    .WithScanCode("333333")
            };

            mockGetHierarchyLineageQueryHandler.Setup(q => q.Search(It.IsAny<GetHierarchyLineageParameters>())).Returns(hierarchyClassListModel);
            mockGetSearchResultsQueryHandler.Setup(q => q.Search(It.IsAny<GetItemsByBulkScanCodeSearchParameters>())).Returns(foundScanCodes);
            mockGetCertificationAgenciesQuery.Setup(q => q.Search(It.IsAny<GetCertificationAgenciesParameters>())).Returns(BuildAgencies());

            var searchScanCodes = new string[] { "234234234234234234", "222222", "333333" };

            var viewModel = new BulkScanCodeSearchViewModel
            {
                TextAreaViewModel = new BulkScanCodeSearchTextAreaViewModel
                {
                    ScanCodes = String.Join(Environment.NewLine, searchScanCodes)
                }
            };

            // When.
            var result = controller.Search(viewModel) as ViewResult;

            // Then.
            var returnedViewModel = result.Model as BulkScanCodeSearchViewModel;

            Assert.AreEqual(1, returnedViewModel.InvalidOrNotFoundScanCodes.Count);
        }

        [TestMethod]
        public void BulkScanCodeSearchControllerSearch_InputContainsScanCodesWithLeadingOrTrailingSpaces_ScanCodesShouldBeTrimmed()
        {
            // Given.
            var hierarchyClassListModel = new HierarchyClassListModel
            {
                BrandHierarchyList = new List<HierarchyClassModel>(),
                TaxHierarchyList = new List<HierarchyClassModel>(),
                MerchandiseHierarchyList = new List<HierarchyClassModel>(),
                NationalHierarchyList = new List<HierarchyClassModel>()
            };

            var foundScanCodes = new List<ItemSearchModel>
            {
                new TestItemSearchModelBuilder()
                    .WithScanCode("222222    "),
                new TestItemSearchModelBuilder()
                    .WithScanCode("    333333")
            };

            mockGetHierarchyLineageQueryHandler.Setup(q => q.Search(It.IsAny<GetHierarchyLineageParameters>())).Returns(hierarchyClassListModel);
            mockGetSearchResultsQueryHandler.Setup(q => q.Search(It.IsAny<GetItemsByBulkScanCodeSearchParameters>())).Returns(foundScanCodes);
            mockGetCertificationAgenciesQuery.Setup(q => q.Search(It.IsAny<GetCertificationAgenciesParameters>())).Returns(BuildAgencies());

            var searchScanCodes = new string[] { "222222   ", "   333333" };

            var viewModel = new BulkScanCodeSearchViewModel
            {
                TextAreaViewModel = new BulkScanCodeSearchTextAreaViewModel
                {
                    ScanCodes = String.Join(Environment.NewLine, searchScanCodes)
                }
            };

            // When.
            var result = controller.Search(viewModel) as ViewResult;

            // Then.
            var returnedViewModel = result.Model as BulkScanCodeSearchViewModel;

            Assert.AreEqual(2, returnedViewModel.InvalidOrNotFoundScanCodes.Count);
        }

        [TestMethod]
        public void BulkScanCodeSearchControllerUpload_InvalidModelState_IndexViewShouldBeReturned()
        {
            // Given.
            controller.ModelState.AddModelError("test", "test");

            var viewModel = new BulkScanCodeSearchViewModel
            {
                FileUploadViewModel = new BulkScanCodeSearchFileUploadViewModel
                {
                    TextFileAttachment = new Mock<HttpPostedFileBase>().Object
                }
            };

            // When.
            var result = controller.Upload(viewModel) as ViewResult;

            // Then.
            Assert.AreEqual(result.ViewName, "Index");
            Assert.IsNotNull(result.Model);
        }

        [TestMethod]
        public void BulkScanCodeSearchControllerUpload_ValidModelState_IndexViewShouldBeReturnedWithHierarchyAndUomListsPopulated()
        {
            // Given.
            var hierarchyClassListModel = new HierarchyClassListModel
            {
                BrandHierarchyList = new List<HierarchyClassModel>(),
                TaxHierarchyList = new List<HierarchyClassModel>(),
                MerchandiseHierarchyList = new List<HierarchyClassModel>(),
                NationalHierarchyList = new List<HierarchyClassModel>()
            };

            mockGetHierarchyLineageQueryHandler.Setup(q => q.Search(It.IsAny<GetHierarchyLineageParameters>())).Returns(hierarchyClassListModel);
            mockGetSearchResultsQueryHandler.Setup(q => q.Search(It.IsAny<GetItemsByBulkScanCodeSearchParameters>())).Returns(new List<ItemSearchModel>());

            var mockAttachment = new Mock<HttpPostedFileBase>();
            mockAttachment.SetupGet(attachment => attachment.InputStream).Returns(new MemoryStream(Encoding.UTF8.GetBytes(Icon.Web.Tests.Unit.Resources.TextFile)));

            var viewModel = new BulkScanCodeSearchViewModel
            {
                FileUploadViewModel = new BulkScanCodeSearchFileUploadViewModel
                {
                    TextFileAttachment = mockAttachment.Object
                }
            };

            // When.
            var result = controller.Upload(viewModel) as ViewResult;

            // Then.
            var returnedViewModel = result.Model as BulkScanCodeSearchViewModel;

            Assert.IsNotNull(returnedViewModel.ItemSearchResults.BrandHierarchyClasses);
            Assert.IsNotNull(returnedViewModel.ItemSearchResults.TaxHierarchyClasses);
            Assert.IsNotNull(returnedViewModel.ItemSearchResults.MerchandiseHierarchyClasses);
            Assert.IsNotNull(returnedViewModel.ItemSearchResults.RetailUoms);
        }

        [TestMethod]
        public void BulkScanCodeSearchControllerUpload_ValidModelState_IndexViewShouldBeReturnedWithSearchResults()
        {
            // Given.
            var hierarchyClassListModel = new HierarchyClassListModel
            {
                BrandHierarchyList = new List<HierarchyClassModel>(),
                TaxHierarchyList = new List<HierarchyClassModel>(),
                MerchandiseHierarchyList = new List<HierarchyClassModel>(),
                NationalHierarchyList = new List<HierarchyClassModel>()
            };

            var foundScanCodes = new List<ItemSearchModel>
            {
                new TestItemSearchModelBuilder()
                    .WithScanCode("222222"),
                new TestItemSearchModelBuilder()
                    .WithScanCode("333333")
            };

            mockGetHierarchyLineageQueryHandler.Setup(q => q.Search(It.IsAny<GetHierarchyLineageParameters>())).Returns(hierarchyClassListModel);
            mockGetSearchResultsQueryHandler.Setup(q => q.Search(It.IsAny<GetItemsByBulkScanCodeSearchParameters>())).Returns(foundScanCodes);

            var mockAttachment = new Mock<HttpPostedFileBase>();
            mockAttachment.SetupGet(attachment => attachment.InputStream).Returns(new MemoryStream(Encoding.UTF8.GetBytes(Icon.Web.Tests.Unit.Resources.TextFile)));

            var viewModel = new BulkScanCodeSearchViewModel
            {
                FileUploadViewModel = new BulkScanCodeSearchFileUploadViewModel
                {
                    TextFileAttachment = mockAttachment.Object
                }
            };

            // When.
            var result = controller.Upload(viewModel) as ViewResult;

            // Then.
            var returnedViewModel = result.Model as BulkScanCodeSearchViewModel;

            Assert.AreEqual(2, returnedViewModel.InvalidOrNotFoundScanCodes.Count);
        }

        [TestMethod]
        public void BulkScanCodeSearchControllerUpload_InputContainsInvalidOrNotFoundScanCodes_IndexViewShouldBeReturnedWithInvalidOrNotFoundScanCodes()
        {
            // Given.
            var hierarchyClassListModel = new HierarchyClassListModel
            {
                BrandHierarchyList = new List<HierarchyClassModel>(),
                TaxHierarchyList = new List<HierarchyClassModel>(),
                MerchandiseHierarchyList = new List<HierarchyClassModel>(),
                NationalHierarchyList = new List<HierarchyClassModel>()
            };

            mockGetHierarchyLineageQueryHandler.Setup(q => q.Search(It.IsAny<GetHierarchyLineageParameters>())).Returns(hierarchyClassListModel);
            mockGetSearchResultsQueryHandler.Setup(q => q.Search(It.IsAny<GetItemsByBulkScanCodeSearchParameters>())).Returns(new List<ItemSearchModel>());

            var notFoundScanCode = new ItemSearchModel { ScanCode = "444444" };
            var invalidScanCode = new ItemSearchModel { ScanCode = "abcdefg" };

            var searchScanCodes = new string[] { notFoundScanCode.ScanCode, invalidScanCode.ScanCode };
            string searchStream = String.Join(Environment.NewLine, searchScanCodes);

            var mockAttachment = new Mock<HttpPostedFileBase>();
            mockAttachment.SetupGet(attachment => attachment.InputStream).Returns(new MemoryStream(Encoding.UTF8.GetBytes(searchStream)));

            var viewModel = new BulkScanCodeSearchViewModel
            {
                FileUploadViewModel = new BulkScanCodeSearchFileUploadViewModel
                {
                    TextFileAttachment = mockAttachment.Object
                }
            };

            // When.
            var result = controller.Upload(viewModel) as ViewResult;

            // Then.
            var returnedViewModel = result.Model as BulkScanCodeSearchViewModel;

            Assert.AreEqual(2, returnedViewModel.InvalidOrNotFoundScanCodes.Count);
            Assert.AreEqual(notFoundScanCode.ScanCode, returnedViewModel.InvalidOrNotFoundScanCodes[0]);
            Assert.AreEqual(invalidScanCode.ScanCode, returnedViewModel.InvalidOrNotFoundScanCodes[1]);
        }

        [TestMethod]
        public void BulkScanCodeSearchControllerUpload_InputContainsScanCodesOverTheSearchLimit_IndexViewShouldBeReturnedWithCountOfScanCodesOverTheLimit()
        {
            // Given.
            var hierarchyClassListModel = new HierarchyClassListModel
            {
                BrandHierarchyList = new List<HierarchyClassModel>(),
                TaxHierarchyList = new List<HierarchyClassModel>(),
                MerchandiseHierarchyList = new List<HierarchyClassModel>(),
                NationalHierarchyList = new List<HierarchyClassModel>()
            };

            mockGetHierarchyLineageQueryHandler.Setup(q => q.Search(It.IsAny<GetHierarchyLineageParameters>())).Returns(hierarchyClassListModel);
            mockGetSearchResultsQueryHandler.Setup(q => q.Search(It.IsAny<GetItemsByBulkScanCodeSearchParameters>())).Returns(new List<ItemSearchModel>());

            int searchLimit = AppSettingsAccessor.GetIntSetting("BulkScanCodeSearchLimit", 3000);
            int overTheLimit = 2;

            var searchScanCodes = new string[searchLimit + overTheLimit];

            for (int i = 0; i < searchScanCodes.Length; i++)
            {
                searchScanCodes[i] = "222222";
            }

            string searchStream = String.Join(Environment.NewLine, searchScanCodes);

            var mockAttachment = new Mock<HttpPostedFileBase>();
            mockAttachment.SetupGet(attachment => attachment.InputStream).Returns(new MemoryStream(Encoding.UTF8.GetBytes(searchStream)));

            var viewModel = new BulkScanCodeSearchViewModel
            {
                FileUploadViewModel = new BulkScanCodeSearchFileUploadViewModel
                {
                    TextFileAttachment = mockAttachment.Object
                }
            };

            // When.
            var result = controller.Upload(viewModel) as ViewResult;

            // Then.
            var returnedViewModel = result.Model as BulkScanCodeSearchViewModel;

            Assert.AreEqual(overTheLimit, returnedViewModel.OverLimitScanCodeCount);
        }

        [TestMethod]
        public void BulkScanCodeSearchControllerUpload_InputContainsDuplicateScanCodes_DuplicatesShouldBeRemoved()
        {
            // Given.
            var hierarchyClassListModel = new HierarchyClassListModel
            {
                BrandHierarchyList = new List<HierarchyClassModel>(),
                TaxHierarchyList = new List<HierarchyClassModel>(),
                MerchandiseHierarchyList = new List<HierarchyClassModel>(),
                NationalHierarchyList = new List<HierarchyClassModel>()
            };

            var foundScanCodes = new List<ItemSearchModel>
            {
                new TestItemSearchModelBuilder()
                    .WithScanCode("222222"),
                new TestItemSearchModelBuilder()
                    .WithScanCode("333333")
            };

            mockGetHierarchyLineageQueryHandler.Setup(q => q.Search(It.IsAny<GetHierarchyLineageParameters>())).Returns(hierarchyClassListModel);
            mockGetSearchResultsQueryHandler.Setup(q => q.Search(It.IsAny<GetItemsByBulkScanCodeSearchParameters>())).Returns(foundScanCodes);

            var searchScanCodes = new string[] { "222222", "333333", "222222" };
            string searchStream = String.Join(Environment.NewLine, searchScanCodes);

            var mockAttachment = new Mock<HttpPostedFileBase>();
            mockAttachment.SetupGet(attachment => attachment.InputStream).Returns(new MemoryStream(Encoding.UTF8.GetBytes(searchStream)));

            var viewModel = new BulkScanCodeSearchViewModel
            {
                FileUploadViewModel = new BulkScanCodeSearchFileUploadViewModel
                {
                    TextFileAttachment = mockAttachment.Object
                }
            };

            // When.
            var result = controller.Upload(viewModel) as ViewResult;

            // Then.
            var returnedViewModel = result.Model as BulkScanCodeSearchViewModel;

            Assert.AreEqual(0, returnedViewModel.InvalidOrNotFoundScanCodes.Count);
        }

        [TestMethod]
        public void BulkScanCodeSearchControllerUpload_InputContainsWhitespaceLines_WhitespaceLinesShouldBeRemoved()
        {
            // Given.
            var hierarchyClassListModel = new HierarchyClassListModel
            {
                BrandHierarchyList = new List<HierarchyClassModel>(),
                TaxHierarchyList = new List<HierarchyClassModel>(),
                MerchandiseHierarchyList = new List<HierarchyClassModel>(),
                NationalHierarchyList = new List<HierarchyClassModel>()
            };

            var foundScanCodes = new List<ItemSearchModel>
            {
                new TestItemSearchModelBuilder()
                    .WithScanCode("222222"),
                new TestItemSearchModelBuilder()
                    .WithScanCode("333333")
            };

            mockGetHierarchyLineageQueryHandler.Setup(q => q.Search(It.IsAny<GetHierarchyLineageParameters>())).Returns(hierarchyClassListModel);
            mockGetSearchResultsQueryHandler.Setup(q => q.Search(It.IsAny<GetItemsByBulkScanCodeSearchParameters>())).Returns(foundScanCodes);

            var searchScanCodes = new string[] { "", "    ", "222222", "333333" };
            string searchStream = String.Join(Environment.NewLine, searchScanCodes);

            var mockAttachment = new Mock<HttpPostedFileBase>();
            mockAttachment.SetupGet(attachment => attachment.InputStream).Returns(new MemoryStream(Encoding.UTF8.GetBytes(searchStream)));

            var viewModel = new BulkScanCodeSearchViewModel
            {
                FileUploadViewModel = new BulkScanCodeSearchFileUploadViewModel
                {
                    TextFileAttachment = mockAttachment.Object
                }
            };

            // When.
            var result = controller.Upload(viewModel) as ViewResult;

            // Then.
            var returnedViewModel = result.Model as BulkScanCodeSearchViewModel;

            Assert.AreEqual(0, returnedViewModel.InvalidOrNotFoundScanCodes.Count);
        }

        [TestMethod]
        public void BulkScanCodeSearchControllerUpload_InputContainsScanCodesGreaterThan13Characters_TooLongScanCodesShouldBeRemovedAndAddedToInvalidCollection()
        {
            // Given.
            var hierarchyClassListModel = new HierarchyClassListModel
            {
                BrandHierarchyList = new List<HierarchyClassModel>(),
                TaxHierarchyList = new List<HierarchyClassModel>(),
                MerchandiseHierarchyList = new List<HierarchyClassModel>(),
                NationalHierarchyList = new List<HierarchyClassModel>()
            };

            var foundScanCodes = new List<ItemSearchModel>
            {
                new TestItemSearchModelBuilder()
                    .WithScanCode("222222"),
                new TestItemSearchModelBuilder()
                    .WithScanCode("333333")
            };

            mockGetHierarchyLineageQueryHandler.Setup(q => q.Search(It.IsAny<GetHierarchyLineageParameters>())).Returns(hierarchyClassListModel);
            mockGetSearchResultsQueryHandler.Setup(q => q.Search(It.IsAny<GetItemsByBulkScanCodeSearchParameters>())).Returns(foundScanCodes);

            var searchScanCodes = new string[] { "222222234234234234", "333333", "222222" };
            string searchStream = String.Join(Environment.NewLine, searchScanCodes);

            var mockAttachment = new Mock<HttpPostedFileBase>();
            mockAttachment.SetupGet(attachment => attachment.InputStream).Returns(new MemoryStream(Encoding.UTF8.GetBytes(searchStream)));

            var viewModel = new BulkScanCodeSearchViewModel
            {
                FileUploadViewModel = new BulkScanCodeSearchFileUploadViewModel
                {
                    TextFileAttachment = mockAttachment.Object
                }
            };

            // When.
            var result = controller.Upload(viewModel) as ViewResult;

            // Then.
            var returnedViewModel = result.Model as BulkScanCodeSearchViewModel;

            Assert.AreEqual(1, returnedViewModel.InvalidOrNotFoundScanCodes.Count);
        }

        [TestMethod]
        public void BulkScanCodeSearchControllerUpload_InputContainsScanCodesWithLeadingOrTrailingSpaces_ScanCodesShouldBeTrimmed()
        {
            // Given.
            var hierarchyClassListModel = new HierarchyClassListModel
            {
                BrandHierarchyList = new List<HierarchyClassModel>(),
                TaxHierarchyList = new List<HierarchyClassModel>(),
                MerchandiseHierarchyList = new List<HierarchyClassModel>(),
                NationalHierarchyList = new List<HierarchyClassModel>()
            };

            var foundScanCodes = new List<ItemSearchModel>
            {
                new TestItemSearchModelBuilder()
                    .WithScanCode("222222   "),
                new TestItemSearchModelBuilder()
                    .WithScanCode("   333333")
            };

            mockGetHierarchyLineageQueryHandler.Setup(q => q.Search(It.IsAny<GetHierarchyLineageParameters>())).Returns(hierarchyClassListModel);
            mockGetSearchResultsQueryHandler.Setup(q => q.Search(It.IsAny<GetItemsByBulkScanCodeSearchParameters>())).Returns(foundScanCodes);
            mockGetCertificationAgenciesQuery.Setup(q => q.Search(It.IsAny<GetCertificationAgenciesParameters>())).Returns(BuildAgencies());

            var searchScanCodes = new string[] { "222222   ", "   333333" };
            string searchStream = String.Join(Environment.NewLine, searchScanCodes);

            var mockAttachment = new Mock<HttpPostedFileBase>();
            mockAttachment.SetupGet(attachment => attachment.InputStream).Returns(new MemoryStream(Encoding.UTF8.GetBytes(searchStream)));

            var viewModel = new BulkScanCodeSearchViewModel
            {
                FileUploadViewModel = new BulkScanCodeSearchFileUploadViewModel
                {
                    TextFileAttachment = mockAttachment.Object
                }
            };

            // When.
            var result = controller.Upload(viewModel) as ViewResult;

            // Then.
            var returnedViewModel = result.Model as BulkScanCodeSearchViewModel;

            Assert.AreEqual(2, returnedViewModel.InvalidOrNotFoundScanCodes.Count);
        }

        private List<CertificationAgencyModel> BuildAgencies()
        {
            var agencyClasses = new List<CertificationAgencyModel>
            {
                new TestCertificationAgencyModelBuilder().WithHierarchyClassName("GlutenFree").WithHierarchyClassId(9).WithGlutenFree("1"),
                new TestCertificationAgencyModelBuilder().WithHierarchyClassName("WithKosher").WithHierarchyClassId(2).WithKosher("1"),
                new TestCertificationAgencyModelBuilder().WithHierarchyClassName("WithNonGMO").WithHierarchyClassId(3).WithNonGMO("1"),
                new TestCertificationAgencyModelBuilder().WithHierarchyClassName("WithOrganic").WithHierarchyClassId(4).WithOrganic("1"),
                new TestCertificationAgencyModelBuilder().WithHierarchyClassName("WithVegan").WithHierarchyClassId(5).WithVegan("1"),
            };

            return agencyClasses;
        }
    }
}