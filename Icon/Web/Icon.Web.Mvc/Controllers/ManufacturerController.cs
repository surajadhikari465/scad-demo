using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using Icon.Web.Common;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Managers;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Attributes;
using Icon.Web.Mvc.Excel;
using Icon.Web.Mvc.Exporters;
using Icon.Web.Mvc.Extensions;
using Icon.Web.Mvc.Models;
using Icon.Web.Mvc.Utility;
using Infragistics.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Icon.Web.Mvc.Controllers
{
    public class ManufacturerController : Controller
    {
        private readonly ILogger logger;
        private readonly IQueryHandler<GetManufacturersParameters, List<ManufacturerModel>> getManufacturersQuery;
        private IQueryHandler<GetHierarchyClassByIdParameters, HierarchyClass> getHierarchyClassQuery;
        private readonly IManagerHandler<ManufacturerManager> manufacturerManagerHandler;
        private readonly IExcelExporterService excelExporterService;
        private IDonutCacheManager cacheManager;
        private IconWebAppSettings settings;

        public ManufacturerController(ILogger logger,
            IQueryHandler<GetManufacturersParameters, List<ManufacturerModel>> getManufacturersQuery,
            IQueryHandler<GetHierarchyClassByIdParameters, HierarchyClass> getHierarchyClassQuery,
            IManagerHandler<ManufacturerManager> manufacturerManagerHandler,
            IExcelExporterService excelExporterService,
            IDonutCacheManager cacheManager,
            IconWebAppSettings settings
            )
        {
            this.logger = logger;
            this.settings = settings;
            this.getHierarchyClassQuery = getHierarchyClassQuery;
            this.getManufacturersQuery = getManufacturersQuery;
            this.manufacturerManagerHandler = manufacturerManagerHandler;
            this.excelExporterService = excelExporterService;
            this.cacheManager = cacheManager;
        }

        // GET: Manufacturer
        public ActionResult Index()
        {
            return View(new ManufacturerViewModel() { UserWriteAccess = GetWriteAccess() });
        }

        [WriteAccessAuthorize]
        public ActionResult Create()
        {
            return View();
        }

        [WriteAccessAuthorize]
        [HttpPost]
        public ActionResult Create(ManufacturerViewModel viewModel)
        {
            // Ignore model state errors for this property until all hierarchy logic has been separated and this property can be safely removed.
            if (ModelState.ContainsKey("HierarchyClassName"))
            {
                ModelState["HierarchyClassName"].Errors.Clear();
            }

            if (!ModelState.IsValid)
            {
                string msg = string.Empty;
                foreach (var mState in ModelState)
                {
                    if (mState.Value.Errors.Count > 0)
                    {
                        msg += $"{mState.Key.Replace("Name", " Name")} is invalid\n";
                    }
                }
                ViewData["ErrorMessage"] = msg;

                return View(viewModel);
            }

            string newManufacturerName = viewModel.ManufacturerName.Trim();
            string newZipCode = string.IsNullOrEmpty(viewModel.ZipCode) ? null : viewModel.ZipCode.Trim();
            string newArCustomerId = string.IsNullOrEmpty(viewModel.ArCustomerId) ? null : viewModel.ArCustomerId.Trim();

            var manager = new ManufacturerManager
            {
                Manufacturer = new HierarchyClass { hierarchyClassName = newManufacturerName, hierarchyID = Hierarchies.Manufacturer, hierarchyLevel = HierarchyLevels.Manufacturer, hierarchyParentClassID = null },
                ZipCode = newZipCode,
                ArCustomerId = newArCustomerId,
                IsManufacturerHierarchyMessage = settings.IsManufacturerHierarchyMessage
            };

            try
            {
                manufacturerManagerHandler.Execute(manager);

                string successMessage = $"Successfully added manufacturer {newManufacturerName}.";

                ViewData["SuccessMessage"] = successMessage;
                
                this.cacheManager.ClearCacheForController("HierarchyClass");
                return View(viewModel);
            }
            catch (CommandException ex)
            {
                var exceptionLogger = new ExceptionLogger(logger);
                exceptionLogger.LogException(ex, this.GetType(), MethodBase.GetCurrentMethod());
                ViewData["ErrorMessage"] = ex.Message;

                return View(EmptyViewModel());
            }
        }

        private ManufacturerViewModel EmptyViewModel()
        {
            return new ManufacturerViewModel
            {
                HierarchyId = -1,
                HierarchyClassId = -1,
                HierarchyLevel = HierarchyLevels.Manufacturer
            };
        }

        public void Export()
        {
            List<ManufacturerExportViewModel> manufacturers = getManufacturersQuery.Search(new GetManufacturersParameters())
                .ToViewModels()
                .Select(m => new ManufacturerExportViewModel
                {
                    ManufacturerId = m.HierarchyClassId.ToString(),
                    ManufacturerName = m.ManufacturerName,
                    ZipCode = m.ZipCode,
                    ArCustomerId = m.ArCustomerId
                })
                .ToList();

            var manufacturerExporter = excelExporterService.GetManufacturerExporter();
            manufacturerExporter.ExportData = manufacturers;
            manufacturerExporter.Export();

            ExcelHelper.SendForDownload(Response, manufacturerExporter.ExportModel.ExcelWorkbook, manufacturerExporter.ExportModel.ExcelWorkbook.CurrentFormat, "Manufacturer");
        }

        [WriteAccessAuthorize]
        public ActionResult Edit(int hierarchyClassId)
        {
            var manufacturer = getHierarchyClassQuery.Search(new GetHierarchyClassByIdParameters() { HierarchyClassId = hierarchyClassId });

            if (manufacturer.hierarchyID != Hierarchies.Manufacturer)
            {
                string msg = $"Hierarchy class is not a manufacturer. HierarchyClassId = {hierarchyClassId}";
                logger.Error(msg);
                ViewData["ErrorMessage"] = msg;
                return View("Error");
            }

            var traits = manufacturer.HierarchyClassTrait.ToArray();

            ManufacturerViewModel model = new ManufacturerViewModel()
            {
                ArCustomerId = traits.SingleOrDefault(x => x.traitID == Traits.ArCustomerId)?.traitValue,
                HierarchyClassId = manufacturer.hierarchyClassID,
                ManufacturerName = manufacturer.hierarchyClassName,
                UserWriteAccess = Enums.WriteAccess.Full,
                ZipCode = traits.SingleOrDefault(x => x.traitID == Traits.ZipCode)?.traitValue,
            };
            return View(model);
        }

        [HttpPost]
        [WriteAccessAuthorize]
        public ActionResult Edit(ManufacturerViewModel viewModel)
        {
            // Ignore model state errors for this property until all hierarchy logic has been separated and this property can be safely removed.
            if (ModelState.ContainsKey("HierarchyClassName"))
            {
                ModelState["HierarchyClassName"].Errors.Clear();
            }

            if (!ModelState.IsValid)
            {
                string msg = string.Empty;
                foreach (var mState in ModelState)
                {
                    if (mState.Value.Errors.Count > 0)
                    {
                        msg += $"{mState.Key.Replace("Name", " Name")} is invalid\n";
                    }
                }
                ViewData["ErrorMessage"] = msg;

                return View(viewModel);
            }

            string newManufacturerName = viewModel.ManufacturerName.Trim();
            string newZipCode = string.IsNullOrEmpty(viewModel.ZipCode) ? null : viewModel.ZipCode.Trim();
            string newArCustomerId = string.IsNullOrEmpty(viewModel.ArCustomerId) ? null : viewModel.ArCustomerId.Trim();

            var manager = new ManufacturerManager
            {
                Manufacturer = new HierarchyClass { hierarchyClassID = viewModel.HierarchyClassId, hierarchyClassName = newManufacturerName, hierarchyID = Hierarchies.Manufacturer, hierarchyLevel = HierarchyLevels.Manufacturer, hierarchyParentClassID = null },
                ZipCode = newZipCode,
                ArCustomerId = newArCustomerId,
                IsManufacturerHierarchyMessage = settings.IsManufacturerHierarchyMessage
            };

            try
            {
                manufacturerManagerHandler.Execute(manager);

                string successMessage = $"Successfully updated manufacturer {newManufacturerName}.";

                ViewData["SuccessMessage"] = successMessage;

                this.cacheManager.ClearCacheForController("HierarchyClass");
                return View(viewModel);
            }
            catch (CommandException ex)
            {
                var exceptionLogger = new ExceptionLogger(logger);
                exceptionLogger.LogException(ex, this.GetType(), MethodBase.GetCurrentMethod());
                ViewData["ErrorMessage"] = ex.Message;

                return View(EmptyViewModel());
            }
        }

        [GridDataSourceAction]
        public ActionResult All()
        {
            List<ManufacturerModel> manufacturers = getManufacturersQuery.Search(new GetManufacturersParameters());

            var viewModels = manufacturers.ToViewModels().AsQueryable();
            return View(viewModels);
        }

        private Enums.WriteAccess GetWriteAccess()
        {
            bool hasWriteAccess = this.settings.WriteAccessGroups.Split(',').Any(x => this.HttpContext.User.IsInRole(x.Trim()));

            var userAccess = Enums.WriteAccess.None;

            if (hasWriteAccess)
            {
                userAccess = Enums.WriteAccess.Full;
            }

            return userAccess;
        }
    }
}