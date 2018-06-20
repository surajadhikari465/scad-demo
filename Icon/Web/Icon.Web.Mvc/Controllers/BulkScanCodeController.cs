using Icon.Common;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Extensions;
using Icon.Web.Mvc.Attributes;
using Icon.Web.Mvc.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Icon.Web.Mvc.Controllers
{
    public class BulkScanCodeSearchController : Controller
    {
        private ILogger logger;
        private IQueryHandler<GetItemsByBulkScanCodeSearchParameters, List<ItemSearchModel>> getSearchResultsQueryHandler;
        private IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel> getHierarchyLineageQueryHandler;
        private IQueryHandler<GetCertificationAgenciesParameters, List<CertificationAgencyModel>> getCertificationAgenciesQuery;
        private int searchLimit;

        public BulkScanCodeSearchController(
            ILogger logger,
            IQueryHandler<GetItemsByBulkScanCodeSearchParameters, List<ItemSearchModel>> getSearchResultsQueryHandler,
            IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel> getHierarchyLineageQueryHandler,
            IQueryHandler<GetCertificationAgenciesParameters, List<CertificationAgencyModel>> getCertificationAgenciesQuery)
        {
            this.logger = logger;
            this.getSearchResultsQueryHandler = getSearchResultsQueryHandler;
            this.getHierarchyLineageQueryHandler = getHierarchyLineageQueryHandler;
            this.getCertificationAgenciesQuery = getCertificationAgenciesQuery;

            searchLimit = AppSettingsAccessor.GetIntSetting("BulkScanCodeSearchLimit", 3000);
        }

        // GET: BulkScanCodeSearch
        public ActionResult Index()
        {
            var viewModel = new BulkScanCodeSearchViewModel
            {
                BulkScanCodeSearchLimit = searchLimit,
                ItemSearchResults = null
            };

            return View(viewModel);
        }

        // POST: BulkScanCodeSearch/Search
        [HttpPost]
        public ActionResult Search(BulkScanCodeSearchViewModel viewModel)
        {
            viewModel.BulkScanCodeSearchLimit = searchLimit;

            if (!ModelState.IsValid)
            {
                return View("Index", viewModel);
            }

            string[] parsedScanCodes = viewModel.TextAreaViewModel.ScanCodes.ParseByLine();
            string[] tooLongScanCodes = parsedScanCodes.Where(sc => sc.Length > 13).ToArray();
            string[] validScanCodes = parsedScanCodes.Except(tooLongScanCodes).ToArray();

            string[] uniqueScanCodes = validScanCodes.Distinct().ToArray();
            string[] uniqueScanCodesWithNoWhitespaceLines = uniqueScanCodes.Where(sc => !String.IsNullOrWhiteSpace(sc)).ToArray();
            string[] scanCodesToSearch = uniqueScanCodesWithNoWhitespaceLines.Take(searchLimit).ToArray();

            var parameters = new GetItemsByBulkScanCodeSearchParameters
            {
                ScanCodes = scanCodesToSearch.ToList()
            };

            List<ItemSearchModel> items = getSearchResultsQueryHandler.Search(parameters);
            viewModel.ItemSearchResults.Items = items.ToViewModels();

            List<string> foundScanCodes = items.Select(i => i.ScanCode).ToList();
            List<string> invalidOrNotFoundScanCodes = scanCodesToSearch.Except(foundScanCodes).ToList();
            invalidOrNotFoundScanCodes.AddRange(tooLongScanCodes.ToList());

            viewModel.InvalidOrNotFoundScanCodes = invalidOrNotFoundScanCodes;

            int scanCodesOverSearchLimit = parsedScanCodes.Length - searchLimit;

            if (scanCodesOverSearchLimit > 0)
            {
                viewModel.OverLimitScanCodeCount = scanCodesOverSearchLimit;
            }

            viewModel.ItemSearchResults.RetailUoms = GetRetailUomSelectList(String.Empty, includeInitialBlank: false);
            viewModel.ItemSearchResults.DeliverySystems = GetDeliverySystemSelectList(String.Empty, includeInitialBlank: false);
            viewModel.ItemSearchResults.AnimalWelfareRatings = AnimalWelfareRatings.AsDictionary.Select(kvp => new HierarchyClassViewModel { HierarchyClassId = kvp.Key, HierarchyClassLineage = kvp.Value }).ToList();
            viewModel.ItemSearchResults.MilkTypes = MilkTypes.AsDictionary.Select(kvp => new HierarchyClassViewModel { HierarchyClassId = kvp.Key, HierarchyClassLineage = kvp.Value }).ToList();
            viewModel.ItemSearchResults.EcoScaleRatings = EcoScaleRatings.AsDictionary.Select(kvp => new HierarchyClassViewModel { HierarchyClassId = kvp.Key, HierarchyClassLineage = kvp.Value }).ToList();
            viewModel.ItemSearchResults.SeafoodFreshOrFrozenTypes = SeafoodFreshOrFrozenTypes.AsDictionary.OrderBy(kvp => kvp.Value).Select(kvp => new HierarchyClassViewModel { HierarchyClassId = kvp.Key, HierarchyClassLineage = kvp.Value }).ToList();
            viewModel.ItemSearchResults.SeafoodCatchTypes = SeafoodCatchTypes.AsDictionary.Select(kvp => new HierarchyClassViewModel { HierarchyClassId = kvp.Key, HierarchyClassLineage = kvp.Value }).ToList();

            var certificationAgencies = getCertificationAgenciesQuery.Search(new GetCertificationAgenciesParameters());
            viewModel.ItemSearchResults.GlutenFreeAgencies = certificationAgencies.Where(ca => ca.GlutenFree == "1").ToList();
            viewModel.ItemSearchResults.KosherAgencies = certificationAgencies.Where(ca => ca.Kosher == "1").ToList();
            viewModel.ItemSearchResults.NonGmoAgencies = certificationAgencies.Where(ca => ca.NonGMO == "1").ToList();
            viewModel.ItemSearchResults.OrganicAgencies = certificationAgencies.Where(ca => ca.Organic == "1").ToList();
            viewModel.ItemSearchResults.VeganAgencies = certificationAgencies.Where(ca => ca.Vegan == "1").ToList();

            HierarchyClassListModel hierarchyListModel = getHierarchyLineageQueryHandler.Search(new GetHierarchyLineageParameters());
            viewModel.ItemSearchResults.BrandHierarchyClasses = GetHierarchyLineage(hierarchyListModel.BrandHierarchyList);
            viewModel.ItemSearchResults.TaxHierarchyClasses = GetHierarchyLineage(hierarchyListModel.TaxHierarchyList);
            viewModel.ItemSearchResults.MerchandiseHierarchyClasses = GetHierarchyLineage(hierarchyListModel.MerchandiseHierarchyList);
            viewModel.ItemSearchResults.NationalHierarchyClasses = GetHierarchyLineage(hierarchyListModel.NationalHierarchyList);

            viewModel.ItemSearchResults.NullableBooleanComboBoxValues = new NullableBooleanComboBoxValuesViewModel();

            return View("Index", viewModel);
        }

        // GET: BulkScanCodeSearch/Upload
        [HttpPost]
        [WriteAccessAuthorize]
        public ActionResult Upload(BulkScanCodeSearchViewModel viewModel)
        {
            viewModel.BulkScanCodeSearchLimit = searchLimit;

            if (!ModelState.IsValid)
            {
                return View("Index", viewModel);
            }

            string uploadedText = ReadTextAttachment(viewModel.FileUploadViewModel.TextFileAttachment);
            string[] parsedScanCodes = uploadedText.ParseByLine();
            string[] tooLongScanCodes = parsedScanCodes.Where(sc => sc.Length > 13).ToArray();
            string[] validScanCodes = parsedScanCodes.Except(tooLongScanCodes).ToArray();

            string[] uniqueScanCodes = validScanCodes.Distinct().ToArray();
            string[] uniqueScanCodesWithNoWhitespaceLines = uniqueScanCodes.Where(sc => !String.IsNullOrWhiteSpace(sc)).ToArray();
            string[] scanCodesToSearch = uniqueScanCodesWithNoWhitespaceLines.Take(searchLimit).ToArray();

            // Paste the uploaded values into the TextArea so that it will be easier to re-run the search if needed (as a result of grid operations).
            viewModel.TextAreaViewModel = new BulkScanCodeSearchTextAreaViewModel
            {
                ScanCodes = String.Join(Environment.NewLine, scanCodesToSearch)
            };

            var parameters = new GetItemsByBulkScanCodeSearchParameters
            {
                ScanCodes = scanCodesToSearch.ToList()
            };

            List<ItemSearchModel> items = getSearchResultsQueryHandler.Search(parameters);
            viewModel.ItemSearchResults.Items = items.ToViewModels();

            List<string> foundScanCodes = items.Select(i => i.ScanCode).ToList();
            List<string> invalidOrNotFoundScanCodes = scanCodesToSearch.Except(foundScanCodes).ToList();
            invalidOrNotFoundScanCodes.AddRange(tooLongScanCodes.ToList());

            viewModel.InvalidOrNotFoundScanCodes = invalidOrNotFoundScanCodes;

            int scanCodesOverSearchLimit = parsedScanCodes.Length - searchLimit;

            if (scanCodesOverSearchLimit > 0)
            {
                viewModel.OverLimitScanCodeCount = scanCodesOverSearchLimit;
            }

            viewModel.ItemSearchResults.RetailUoms = GetRetailUomSelectList(String.Empty, includeInitialBlank: false);
            viewModel.ItemSearchResults.DeliverySystems = GetDeliverySystemSelectList(String.Empty, includeInitialBlank: false);
            viewModel.ItemSearchResults.AnimalWelfareRatings = AnimalWelfareRatings.AsDictionary.Select(kvp => new HierarchyClassViewModel { HierarchyClassId = kvp.Key, HierarchyClassLineage = kvp.Value }).ToList();
            viewModel.ItemSearchResults.MilkTypes = MilkTypes.AsDictionary.Select(kvp => new HierarchyClassViewModel { HierarchyClassId = kvp.Key, HierarchyClassLineage = kvp.Value }).ToList();
            viewModel.ItemSearchResults.EcoScaleRatings = EcoScaleRatings.AsDictionary.Select(kvp => new HierarchyClassViewModel { HierarchyClassId = kvp.Key, HierarchyClassLineage = kvp.Value }).ToList();
            viewModel.ItemSearchResults.SeafoodFreshOrFrozenTypes = SeafoodFreshOrFrozenTypes.AsDictionary.OrderBy(kvp => kvp.Value).Select(kvp => new HierarchyClassViewModel { HierarchyClassId = kvp.Key, HierarchyClassLineage = kvp.Value }).ToList();
            viewModel.ItemSearchResults.SeafoodCatchTypes = SeafoodCatchTypes.AsDictionary.Select(kvp => new HierarchyClassViewModel { HierarchyClassId = kvp.Key, HierarchyClassLineage = kvp.Value }).ToList();

            var certificationAgencies = getCertificationAgenciesQuery.Search(new GetCertificationAgenciesParameters());
            viewModel.ItemSearchResults.GlutenFreeAgencies = certificationAgencies.Where(ca => ca.GlutenFree == "1").ToList();
            viewModel.ItemSearchResults.KosherAgencies = certificationAgencies.Where(ca => ca.Kosher == "1").ToList();
            viewModel.ItemSearchResults.NonGmoAgencies = certificationAgencies.Where(ca => ca.NonGMO == "1").ToList();
            viewModel.ItemSearchResults.OrganicAgencies = certificationAgencies.Where(ca => ca.Organic == "1").ToList();
            viewModel.ItemSearchResults.VeganAgencies = certificationAgencies.Where(ca => ca.Vegan == "1").ToList();

            HierarchyClassListModel hierarchyListModel = getHierarchyLineageQueryHandler.Search(new GetHierarchyLineageParameters());
            viewModel.ItemSearchResults.BrandHierarchyClasses = GetHierarchyLineage(hierarchyListModel.BrandHierarchyList);
            viewModel.ItemSearchResults.TaxHierarchyClasses = GetHierarchyLineage(hierarchyListModel.TaxHierarchyList);
            viewModel.ItemSearchResults.MerchandiseHierarchyClasses = GetHierarchyLineage(hierarchyListModel.MerchandiseHierarchyList);
            viewModel.ItemSearchResults.NationalHierarchyClasses = GetHierarchyLineage(hierarchyListModel.NationalHierarchyList);

            viewModel.ItemSearchResults.NullableBooleanComboBoxValues = new NullableBooleanComboBoxValuesViewModel();

            return View("Index", viewModel);
        }

        private string ReadTextAttachment(HttpPostedFileBase attachment)
        {
            string uploadedText;
            using (var reader = new StreamReader(attachment.InputStream, Encoding.UTF8))
            {
                uploadedText = reader.ReadToEnd();
            }

            return uploadedText;
        }

        private SelectList GetRetailUomSelectList(string selectedUom, bool includeInitialBlank)
        {
            var uoms = UomCodes.ByName.Values.ToList();

            if (includeInitialBlank)
            {
                // Insert empty value at the beginning of the list to allow for an un-selected state.
                uoms.Insert(0, String.Empty);
            }

            return new SelectList(uoms, selectedUom);
        }

        private SelectList GetDeliverySystemSelectList(string selectedDeliverySystem, bool includeInitialBlank)
        {
            var deliverySystems = DeliverySystems.AsDictionary.Values.ToList();

            if (includeInitialBlank)
            {
                // Insert empty value at the beginning of the list to allow for an un-selected state.
                deliverySystems.Insert(0, String.Empty);
            }

            return new SelectList(deliverySystems, selectedDeliverySystem);
        }

        private List<HierarchyClassViewModel> GetHierarchyLineage(List<HierarchyClassModel> hierarchyList)
        {
            List<HierarchyClassViewModel> hierarchyClasses = hierarchyList.HierarchyClassForCombo();
            hierarchyClasses.Insert(0, new HierarchyClassViewModel { HierarchyClassId = 0, HierarchyClassName = String.Empty });
            return hierarchyClasses;
        }
    }
}