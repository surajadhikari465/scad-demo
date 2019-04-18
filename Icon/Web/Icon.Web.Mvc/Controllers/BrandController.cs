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
using System.Configuration;
using System.Text;
using System.Security.Cryptography;

namespace Icon.Web.Mvc.Controllers
{
    public class BrandController : Controller
    {
        private ILogger logger;
        private IQueryHandler<GetBrandsParameters, List<BrandModel>> getBrandsQuery;
        private IQueryHandler<GetHierarchyClassByIdParameters, HierarchyClass> getHierarchyClassQuery;
        private IManagerHandler<BrandManager> updateBrandManagerHandler;
        private IExcelExporterService excelExporterService;

        const string WRITE_ACCESS = "WriteAccess";
        const string INFOR_DISABLE_ICON_INTERFACE = "InforDisableIconInterface";

        public BrandController(
            ILogger logger,
            IQueryHandler<GetBrandsParameters, List<BrandModel>> getBrandsQuery,
            IQueryHandler<GetHierarchyClassByIdParameters, HierarchyClass> getHierarchyClassQuery,
            IManagerHandler<BrandManager> updateBrandManagerHandler,
            IExcelExporterService excelExporterService)
        {
            this.logger = logger;
            this.getBrandsQuery = getBrandsQuery;
            this.getHierarchyClassQuery = getHierarchyClassQuery;
            this.updateBrandManagerHandler = updateBrandManagerHandler;
            this.excelExporterService = excelExporterService;
        }

        // GET: Brand
        public ActionResult Index()
        {
            return View();
        }

        // GET: Brand/Create
        [WriteAccessAuthorize]
        public ActionResult Create()
        {
            return View(EmptyViewModel());
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
                viewModel.BrandList =  GetBrandList();
                return View(viewModel);
            }

            string newBrandName = viewModel.BrandName.Trim();
            string newBrandAbbreviation = String.IsNullOrEmpty(viewModel.BrandAbbreviation) ? null : viewModel.BrandAbbreviation.Trim();

            var manager = new BrandManager
            {
                Brand = new HierarchyClass(){ hierarchyClassName = newBrandName, hierarchyID = Hierarchies.Brands, hierarchyLevel = HierarchyLevels.Parent, hierarchyParentClassID = null  },
                BrandAbbreviation = newBrandAbbreviation,
                Designation = string.IsNullOrWhiteSpace(viewModel.Designation) ? null : viewModel.Designation.Trim(),
                ParentCompany = string.IsNullOrWhiteSpace(viewModel.ParentCompany) ? null : viewModel.ParentCompany.Trim(),
                ZipCode = string.IsNullOrWhiteSpace(viewModel.ZipCode) ? null : viewModel.ZipCode.Replace(" ", String.Empty),
                Locality = string.IsNullOrWhiteSpace(viewModel.Locality) ? null : viewModel.Locality.Trim(),
                Update = UpdateOptions.Brand | UpdateOptions.Traits
            };

            try
            {
                updateBrandManagerHandler.Execute(manager);

                string successMessage = String.IsNullOrEmpty(newBrandAbbreviation)
                    ? String.Format("Successfully added brand {0}.", newBrandName)
                    : String.Format("Successfully added brand {0} with abbreviation {1}.", newBrandName, newBrandAbbreviation);

                ViewData["SuccessMessage"] = successMessage;

                ModelState.Clear();
                return View(EmptyViewModel());
            }
            catch (CommandException ex)
            {
                var exceptionLogger = new ExceptionLogger(logger);
                exceptionLogger.LogException(ex, this.GetType(), MethodBase.GetCurrentMethod());
                ViewData["ErrorMessage"] = ex.Message;

                return View(EmptyViewModel());
            }
        }

        // GET: Brand/Edit/5
        public ActionResult Edit(int hierarchyClassId)
        {
            var brand = getHierarchyClassQuery.Search(new GetHierarchyClassByIdParameters(){ HierarchyClassId = hierarchyClassId });
            var traits = brand.HierarchyClassTrait.ToArray();
            var brands = GetBrandList();

            var viewModel = new BrandViewModel
                {
                    BrandName = brand.hierarchyClassName,
                    BrandAbbreviation = traits.SingleOrDefault(x => x.traitID == Traits.BrandAbbreviation)?.traitValue.Trim(),
                    HierarchyId = brand.hierarchyID,
                    HierarchyClassId = brand.hierarchyClassID,
                    HierarchyLevel = HierarchyLevels.Parent,
                    Designation = traits.SingleOrDefault(x => x.traitID == Traits.Designation)?.traitValue,
                    ZipCode = traits.SingleOrDefault(x => x.traitID == Traits.ZipCode)?.traitValue,
                    Locality = traits.SingleOrDefault(x => x.traitID == Traits.Locality)?.traitValue,
                    ParentCompany = brands.FirstOrDefault(x => string.Compare(x, traits.SingleOrDefault(y => y.traitID == Traits.ParentCompany)?.traitValue.Trim(), true) == 0),
                    BrandList = brands,
                    IsBrandCoreUpdateAuthorized = IsAuthorized()
                };

            viewModel.BrandHashKey = CalculateHashKey(viewModel.BrandName, viewModel.BrandAbbreviation);
            viewModel.TraitHashKey = CalculateHashKey(viewModel.BrandAbbreviation, viewModel.Designation, viewModel.ZipCode, viewModel.Locality, viewModel.ParentCompany); 

            return View(viewModel);
        }

        // POST: Brand/Edit/
        [HttpPost]
        public ActionResult Edit(BrandViewModel viewModel)
        {
            // Ignore model state errors for this property until all hierarchy logic has been separated and this property can be safely removed.
            if (ModelState.ContainsKey("HierarchyClassName"))
            {
                ModelState["HierarchyClassName"].Errors.Clear();
            }

            if (!ModelState.IsValid)
            {
                viewModel.BrandList = GetBrandList();
                return View(viewModel);
            }

            try
            {
                var brandHashKey = CalculateHashKey(viewModel.BrandName, viewModel.BrandAbbreviation);
                var traitHashKey = CalculateHashKey(viewModel.BrandAbbreviation, viewModel.Designation, viewModel.ZipCode, viewModel.Locality, viewModel.ParentCompany);

                if(brandHashKey == viewModel.BrandHashKey && traitHashKey == viewModel.TraitHashKey)
                {
                     ViewData["SuccessMessage"] = $"No data change has been detected for {viewModel.BrandName} brand.";
                }
                else
                {
                    var updatedBrand = new HierarchyClass
                    {
                        hierarchyID = viewModel.HierarchyId,
                        hierarchyClassID = viewModel.HierarchyClassId,
                        hierarchyClassName = viewModel.BrandName.Trim(),
                        hierarchyLevel = viewModel.HierarchyLevel
                    };

                    var manager = new BrandManager
                    {
                        Brand = updatedBrand,
                        BrandAbbreviation = String.IsNullOrWhiteSpace(viewModel.BrandAbbreviation) ? null : viewModel.BrandAbbreviation.Trim(),
                        Designation = String.IsNullOrWhiteSpace(viewModel.Designation) ? null : viewModel.Designation.Trim(),
                        ParentCompany = String.IsNullOrWhiteSpace(viewModel.ParentCompany) ? null : viewModel.ParentCompany.Trim(),
                        ZipCode = String.IsNullOrWhiteSpace(viewModel.ZipCode) ? null : viewModel.ZipCode.Trim(),
                        Locality = String.IsNullOrWhiteSpace(viewModel.Locality) ? null : viewModel.Locality.Trim(),
                        Update = ((!IsAuthorized() || brandHashKey == viewModel.BrandHashKey) ? UpdateOptions.None : UpdateOptions.Brand) | (traitHashKey == viewModel.TraitHashKey ? UpdateOptions.None : UpdateOptions.Traits)
                    };

                    ModelState.Clear();
                    updateBrandManagerHandler.Execute(manager);
                    viewModel.BrandHashKey = brandHashKey;
                    viewModel.TraitHashKey = traitHashKey;
                    ViewData["SuccessMessage"] = "Brand update was successful.";
                }
            }
            catch (CommandException ex)
            {
                var exceptionLogger = new ExceptionLogger(logger);
                exceptionLogger.LogException(ex, this.GetType(), MethodBase.GetCurrentMethod());
                ViewData["ErrorMessage"] = ex.Message;
            }
            catch(Exception ex)
            {
                 ViewData["ErrorMessage"] = ex.Message;
            }
            
            viewModel.BrandList = GetBrandList();
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
        
        private BrandViewModel EmptyViewModel()
        {
            return new BrandViewModel
                {
                    BrandName = String.Empty,
                    BrandAbbreviation = String.Empty,
                    HierarchyId = -1,
                    HierarchyClassId = -1,
                    HierarchyLevel = HierarchyLevels.Parent,
                    BrandList = GetBrandList()
                };
        }

        bool IsAuthorized()
        {
            try
            {
                return ConfigurationManager.AppSettings[INFOR_DISABLE_ICON_INTERFACE] == "1"
                    ? true
                    : ConfigurationManager.AppSettings[WRITE_ACCESS].Split(',').Any(x => this.HttpContext.User.IsInRole(x.Trim()));
            }
            catch { return false; }
        }

        string[] GetBrandList()
        {
            return getBrandsQuery.Search(new GetBrandsParameters()).Select(x => x.HierarchyClassName).OrderBy(x => x).ToArray();
        }

        string CalculateHashKey(params string[] values)
        {
            if(values == null || values.Length == 0) return String.Empty;

            var hashBuilder = new StringBuilder();
            using(var sha = MD5.Create())
            {
                foreach(var i in sha.ComputeHash(Encoding.UTF8.GetBytes(String.Join("|", values.Select(x => string.IsNullOrWhiteSpace(x) ? String.Empty : x.Trim())))))
                {
                    hashBuilder.Append(i.ToString("x2"));
                }
            }

            return hashBuilder.ToString();
        }
    }
}