using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using Icon.Web.Common;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Managers;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Extensions;
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
using System.Text;
using System.Security.Cryptography;
using Icon.Web.Mvc.Utility;

namespace Icon.Web.Mvc.Controllers
{
    public class BrandController : Controller
    {
        private ILogger logger;
        private IQueryHandler<GetBrandsParameters, List<BrandModel>> getBrandsQuery;
        private IQueryHandler<GetHierarchyClassByIdParameters, HierarchyClass> getHierarchyClassQuery;
        private IManagerHandler<BrandManager> updateBrandManagerHandler;
        private IExcelExporterService excelExporterService;
        private IDonutCacheManager cacheManager;
        private IconWebAppSettings settings;
        private IManagerHandler<DeleteHierarchyClassManager> deleteHierarchyClass;
        private IQueryHandler<GetContactsParameters, List<ContactModel>> getContactsQuery;

        public BrandController(
            ILogger logger,
            IQueryHandler<GetBrandsParameters, List<BrandModel>> getBrandsQuery,
            IQueryHandler<GetHierarchyClassByIdParameters, HierarchyClass> getHierarchyClassQuery,
            IManagerHandler<BrandManager> updateBrandManagerHandler,
            IExcelExporterService excelExporterService,
            IconWebAppSettings settings,
            IDonutCacheManager cacheManager,
            IManagerHandler<DeleteHierarchyClassManager> deleteHierarchyClass,
            IQueryHandler<GetContactsParameters, List<ContactModel>> getContactsQuery)
        {
            this.logger = logger;
            this.getBrandsQuery = getBrandsQuery;
            this.getHierarchyClassQuery = getHierarchyClassQuery;
            this.updateBrandManagerHandler = updateBrandManagerHandler;
            this.excelExporterService = excelExporterService;
            this.settings = settings;
            this.cacheManager = cacheManager;
            this.deleteHierarchyClass = deleteHierarchyClass;
            this.getContactsQuery = getContactsQuery;
        }

        // GET: Brand
        public ActionResult Index()
        {
            ViewBag.IsContactViewAllowed = this.settings.IsContactViewEnabled;
            return View(new BrandViewModel() { UserWriteAccess = GetWriteAccess() });
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
                Brand = new HierarchyClass(){ hierarchyClassName = newBrandName, hierarchyID = Hierarchies.Brands, hierarchyLevel = HierarchyLevels.Brand, hierarchyParentClassID = null  },
                BrandAbbreviation = newBrandAbbreviation,
                Designation = string.IsNullOrWhiteSpace(viewModel.Designation) ? null : viewModel.Designation.Trim(),
                ParentCompany = string.IsNullOrWhiteSpace(viewModel.ParentCompany) ? null : viewModel.ParentCompany.Trim(),
                ZipCode = string.IsNullOrWhiteSpace(viewModel.ZipCode) ? null : viewModel.ZipCode.Trim(),
                Locality = string.IsNullOrWhiteSpace(viewModel.Locality) ? null : viewModel.Locality.Trim(),
                WriteAccess = GetWriteAccess()
        };

            try
            {
                updateBrandManagerHandler.Execute(manager);

                string successMessage = String.IsNullOrEmpty(newBrandAbbreviation)
                    ? String.Format("Successfully added brand {0}.", newBrandName)
                    : String.Format("Successfully added brand {0} with abbreviation {1}.", newBrandName, newBrandAbbreviation);

                ViewData["SuccessMessage"] = successMessage;

                ModelState.Clear();
                this.cacheManager.ClearCacheForController("HierarchyClass");
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

        [HttpPost]
        [WriteAccessAuthorize]
        public ActionResult Delete(int hcId)
        {
            try
            {
                if(GetWriteAccess() != Enums.WriteAccess.Full)
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;
                    return Json("You don’t have sufficient privileges to complete selected action.");
                }

                var errList = new List<string>();

                var hcDeleted = getHierarchyClassQuery.Search(new GetHierarchyClassByIdParameters { HierarchyClassId = hcId });

                // Make sure the node is not attached to any items.
                if(hcDeleted.ItemHierarchyClass.Count > 0)
                {
                    errList.Add("linked to items");
                }
    
                // Make sure the node does not have any children.
                if(hcDeleted.HierarchyClass1.Count > 0)
                {
                    errList.Add("contains subclasses");
                }

                // Make sure there's no contacts.
                if(getContactsQuery.Search(new GetContactsParameters() { HierarchyClassId = hcId }).Any())
                {
                    errList.Add("has associated conatcts");
                }

                if(errList.Count() > 0)
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                    var msg = $"Cannot be deleted for the following reason(s): { string.Join(", ", errList) }.";
                    Response.StatusDescription = msg;
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }  

                var command = new DeleteHierarchyClassManager
                {
                    DeletedHierarchyClass = hcDeleted,
                    EnableHierarchyMessages = true,
                };

                deleteHierarchyClass.Execute(command);
                this.cacheManager.ClearCacheForController("HierarchyClass");
				Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
				return Json(new { success = true, responseText = $"Successfully deleted {hcDeleted.hierarchyClassName}" }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				Response.StatusCode = (int)System.Net.HttpStatusCode.ExpectationFailed;
				return Json(ex.Message);
			}
		}

        // GET: Brand/Edit/5
        [WriteAccessAuthorize]
        public ActionResult Edit(int hierarchyClassId)
        {
            var brand = getHierarchyClassQuery.Search(new GetHierarchyClassByIdParameters(){ HierarchyClassId = hierarchyClassId });

            if (brand.hierarchyID != Hierarchies.Brands)
            {
                string msg = $"Hierarchy class is not a brand. HierarchyClassId = {hierarchyClassId}";
                logger.Error(msg);
                ViewData["ErrorMessage"] = msg;
                return View("Error");
            }

            var traits = brand.HierarchyClassTrait.ToArray();
            var brands = GetBrandList();

            var viewModel = new BrandViewModel()
                {
                    BrandName = brand.hierarchyClassName,
                    BrandAbbreviation = traits.SingleOrDefault(x => x.traitID == Traits.BrandAbbreviation)?.traitValue.Trim(),
                    HierarchyId = brand.hierarchyID,
                    HierarchyClassId = brand.hierarchyClassID,
                    HierarchyLevel = HierarchyLevels.Brand,
                    Designation = traits.SingleOrDefault(x => x.traitID == Traits.Designation)?.traitValue,
                    ZipCode = traits.SingleOrDefault(x => x.traitID == Traits.ZipCode)?.traitValue,
                    Locality = traits.SingleOrDefault(x => x.traitID == Traits.Locality)?.traitValue,
                    ParentCompany = brands.FirstOrDefault(x => string.Compare(x, traits.SingleOrDefault(y => y.traitID == Traits.ParentCompany)?.traitValue.Trim(), true) == 0),
                    BrandList = brands,
                    UserWriteAccess = GetWriteAccess()
                };

            viewModel.BrandHashKey = CalculateHashKey(viewModel.BrandName, viewModel.BrandAbbreviation);
            viewModel.TraitHashKey = CalculateHashKey(viewModel.BrandAbbreviation, viewModel.Designation, viewModel.ZipCode, viewModel.Locality, viewModel.ParentCompany); 

            ViewBag.IsContactViewAllowed = this.settings.IsContactViewEnabled;
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
                    };

                    updateBrandManagerHandler.Execute(manager);
                    ModelState.Clear();
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
            
            viewModel.UserWriteAccess = GetWriteAccess();
            viewModel.BrandList = GetBrandList();

            this.cacheManager.ClearCacheForController("HierarchyClass");

            ViewBag.IsContactViewAllowed = this.settings.IsContactViewEnabled;
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
                    BrandAbbreviation = b.BrandAbbreviation,
                    Designation = b.Designation,
                    ParentCompany = b.ParentCompany,
                    ZipCode = b.ZipCode,
                    Locality = b.Locality
                })
                .ToList();

            var brandExporter = excelExporterService.GetBrandExporter();
            brandExporter.ExportData = brands;
            brandExporter.Export();

            ExcelHelper.SendForDownload(Response, brandExporter.ExportModel.ExcelWorkbook, brandExporter.ExportModel.ExcelWorkbook.CurrentFormat, "Brand");
        }

        [HttpGet]
        public void BrandTemplateExport()
        {
            var brandTemplateExporter = excelExporterService.GetBrandTemplateExporter();
            brandTemplateExporter.Export();

            ExcelHelper.SendForDownload(Response, brandTemplateExporter.ExportModel.ExcelWorkbook, brandTemplateExporter.ExportModel.ExcelWorkbook.CurrentFormat, "Brand");
        }

        private BrandViewModel EmptyViewModel()
        {
            return new BrandViewModel
            {
                BrandName = String.Empty,
                BrandAbbreviation = String.Empty,
                HierarchyId = -1,
                HierarchyClassId = -1,
                HierarchyLevel = HierarchyLevels.Brand,
                BrandList = GetBrandList()
            };
        }

        /// <summary>
        /// If Icon is disabled, users cannot edit brand name or brand abbreviation, even if they are part of WriteAccess setting.
        /// If Icon is enabled, users can only edit brand name and brand abbreviation if they are part of WriteAccess setting.
        /// </summary>
        /// <returns>Enums.WriteAccess value
        /// None if user has no access to edit anything
        /// Full if user can edit both Brand name & abbreviation and Brand traits
        /// Traits if user can edit only Brand traits.
        /// </returns>
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

        string[] GetBrandList()
        {
            return getBrandsQuery.Search(new GetBrandsParameters()).Select(x => x.HierarchyClassName).OrderBy(x => x).ToArray();
        }

        string CalculateHashKey(params string[] values)
        {
            if (values == null || values.Length == 0)
            {
                return String.Empty;
            }

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