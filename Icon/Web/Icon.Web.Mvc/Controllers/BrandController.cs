using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using Icon.Web.Common;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Managers;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Extensions;
using Icon.Web.Mvc.Attributes;
using Icon.Web.Mvc.Excel;
using Icon.Web.Mvc.Exporters;
using Icon.Web.Mvc.Models;
using Infragistics.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Icon.Web.Mvc.Controllers
{
    public class BrandController : Controller
    {
        private ILogger logger;
        private IQueryHandler<GetBrandsParameters, List<BrandModel>> getBrandsQuery;
        private IQueryHandler<GetHierarchyClassByIdParameters, HierarchyClass> getHierarchyClassQuery;
        private IManagerHandler<AddBrandManager> addBrandManagerHandler;
        private IManagerHandler<UpdateBrandManager> updateBrandManagerHandler;
        private IExcelExporterService excelExporterService;

        public BrandController(
            ILogger logger,
            IQueryHandler<GetBrandsParameters, List<BrandModel>> getBrandsQuery,
            IQueryHandler<GetHierarchyClassByIdParameters, HierarchyClass> getHierarchyClassQuery,
            IManagerHandler<AddBrandManager> addBrandManagerHandler,
            IManagerHandler<UpdateBrandManager> updateBrandManagerHandler,
            IExcelExporterService excelExporterService)
        {
            this.logger = logger;
            this.getBrandsQuery = getBrandsQuery;
            this.getHierarchyClassQuery = getHierarchyClassQuery;
            this.addBrandManagerHandler = addBrandManagerHandler;
            this.updateBrandManagerHandler = updateBrandManagerHandler;
            this.excelExporterService = excelExporterService;
        }

        // GET: Brand
        public ActionResult Index()
        {
            return View();
        }

        // GET: Brand/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Brand/Create
        [HttpPost]
        [WriteAccessAuthorize]
        public ActionResult Create(BrandViewModel viewModel)
        {
            // Ignore model state errors for this property until all hierarchy logic has been separated and this property can be safely removed.
            if (ModelState.ContainsKey("HierarchyClassName"))
            {
                ModelState["HierarchyClassName"].Errors.Clear();
            }

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            string newBrandName = viewModel.BrandName.Trim();
            string newBrandAbbreviation = String.IsNullOrEmpty(viewModel.BrandAbbreviation) ? null : viewModel.BrandAbbreviation.Trim();

            var newBrand = new HierarchyClass
            {
                hierarchyClassName = newBrandName
            };

            var manager = new AddBrandManager
            {
                Brand = newBrand,
                BrandAbbreviation = newBrandAbbreviation
            };

            try
            {
                addBrandManagerHandler.Execute(manager);

                string successMessage = String.IsNullOrEmpty(newBrandAbbreviation)
                    ? String.Format("Successfully added brand {0}.", newBrandName)
                    : String.Format("Successfully added brand {0} with abbreviation {1}.", newBrandName, newBrandAbbreviation);

                ViewData["SuccessMessage"] = successMessage;

                ModelState.Clear();
                return View();
            }
            catch (CommandException ex)
            {
                var exceptionLogger = new ExceptionLogger(logger);
                exceptionLogger.LogException(ex, this.GetType(), MethodBase.GetCurrentMethod());
                ViewData["ErrorMessage"] = ex.Message;

                return View();
            }
        }

        // GET: Brand/Edit/5
        public ActionResult Edit(int hierarchyClassId)
        {
            var viewModel = BuildBrandViewModel(hierarchyClassId);

            return View(viewModel);
        }

        // POST: Brand/Edit/
        [HttpPost]
        [WriteAccessAuthorize]
        public ActionResult Edit(BrandViewModel viewModel)
        {
            // Ignore model state errors for this property until all hierarchy logic has been separated and this property can be safely removed.
            if (ModelState.ContainsKey("HierarchyClassName"))
            {
                ModelState["HierarchyClassName"].Errors.Clear();
            }

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            string updatedBrandName = viewModel.BrandName.Trim();
            string updatedBrandAbbreviation = String.IsNullOrWhiteSpace(viewModel.BrandAbbreviation) ? null : viewModel.BrandAbbreviation.Trim();

            var updatedBrand = new HierarchyClass
            {
                hierarchyID = viewModel.HierarchyId,
                hierarchyClassID = viewModel.HierarchyClassId,
                hierarchyClassName = updatedBrandName,
                hierarchyLevel = viewModel.HierarchyLevel
            };

            var manager = new UpdateBrandManager
            {
                Brand = updatedBrand,
                BrandAbbreviation = updatedBrandAbbreviation
            };

            try
            {
                updateBrandManagerHandler.Execute(manager);
                ViewData["SuccessMessage"] = "Brand update was successful.";
            }
            catch (CommandException ex)
            {
                var exceptionLogger = new ExceptionLogger(logger);
                exceptionLogger.LogException(ex, this.GetType(), MethodBase.GetCurrentMethod());
                ViewData["ErrorMessage"] = ex.Message;
            }

            return View(viewModel);
        }

        [GridDataSourceAction]
        public ActionResult All()
        {
            List<BrandModel> brands = getBrandsQuery.Search(new GetBrandsParameters());
            var viewModels = brands.ToViewModels().AsQueryable();
            return View(viewModels);
        }

        public void Export()
        {
            List<BrandExportViewModel> brands = getBrandsQuery.Search(new GetBrandsParameters())
                .ToViewModels()
                .Select(b => new BrandExportViewModel 
                {
                    BrandId = b.HierarchyClassId.ToString(),
                    BrandName = b.BrandName,
                    BrandAbbreviation = b.BrandAbbreviation
                })
                .ToList();

            var brandExporter = excelExporterService.GetBrandExporter();
            brandExporter.ExportData = brands;
            brandExporter.Export();

            ExcelHelper.SendForDownload(Response, brandExporter.ExportModel.ExcelWorkbook, brandExporter.ExportModel.ExcelWorkbook.CurrentFormat, "Brand");
        }
        
        private BrandViewModel BuildBrandViewModel(int hierarchyClassId)
        {
            var parameters = new GetHierarchyClassByIdParameters
            {
                HierarchyClassId = hierarchyClassId
            };

            var brand = getHierarchyClassQuery.Search(parameters);

            string brandName = brand.hierarchyClassName;
            string brandAbbreviation = null;

            var brandAbbreviationTrait = brand.HierarchyClassTrait.SingleOrDefault(hct => hct.Trait.traitCode == TraitCodes.BrandAbbreviation);

            if (brandAbbreviationTrait != null)
            {
                brandAbbreviation = brandAbbreviationTrait.traitValue;
            }

            var viewModel = new BrandViewModel
            {
                BrandName = brand.hierarchyClassName,
                BrandAbbreviation = brandAbbreviation,
                HierarchyId = brand.hierarchyID,
                HierarchyClassId = brand.hierarchyClassID,
                HierarchyLevel = HierarchyLevels.Parent
            };

            return viewModel;
        }
    }
}
