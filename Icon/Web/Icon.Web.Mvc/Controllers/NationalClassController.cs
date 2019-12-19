using DevTrends.MvcDonutCaching;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using Icon.Web.Common;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Managers;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Attributes;
using Icon.Web.Mvc.Exporters;
using Icon.Web.Mvc.Models;
using Icon.Web.Mvc.Utility;
using Infragistics.Documents.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Icon.Web.Controllers
{
    public class NationalClassController : Controller
    {
        private ILogger logger;
        private IconWebAppSettings settings;
        private IQueryHandler<GetHierarchyParameters, List<Hierarchy>> getHierarchyQuery;
        private IQueryHandler<GetHierarchyClassByIdParameters, HierarchyClass> hierarchyClassQuery;
        private IManagerHandler<UpdateNationalHierarchyManager> updateNationalHierarchyManager;
        private IManagerHandler<AddNationalHierarchyManager> addNationalHierarchyManager;
        private IManagerHandler<DeleteNationalHierarchyManager> deleteNationalHierarchyManager;
        private IExcelExporterService exporterService;
        private IDonutCacheManager cacheManager;
        private IQueryHandler<GetHierarchyClassesParameters, IEnumerable<HierarchyClassModel>> getHierarchyClassesQueryHandler;
        public NationalClassController(
            ILogger logger,
            IconWebAppSettings settings,
            IQueryHandler<GetHierarchyParameters, List<Hierarchy>> getHierarchyQuery,
            IQueryHandler<GetHierarchyClassByIdParameters, HierarchyClass> hierarchyClassQuery,
            IManagerHandler<UpdateNationalHierarchyManager> updateNationalHierarchyManager,
            IManagerHandler<AddNationalHierarchyManager> addNationalHierarchyManager,
            IManagerHandler<DeleteNationalHierarchyManager> deleteNationalHierarchyManager,
            IExcelExporterService exporterService,
            IDonutCacheManager cacheManager,
            IQueryHandler<GetHierarchyClassesParameters, IEnumerable<HierarchyClassModel>> getHierarchyClassesQueryHandler)
        {
            this.logger = logger;
            this.settings = settings;
            this.getHierarchyQuery = getHierarchyQuery;
            this.hierarchyClassQuery = hierarchyClassQuery;
            this.updateNationalHierarchyManager = updateNationalHierarchyManager;
            this.addNationalHierarchyManager = addNationalHierarchyManager;
            this.deleteNationalHierarchyManager = deleteNationalHierarchyManager;
            this.exporterService = exporterService;
            this.cacheManager = cacheManager;
            this.getHierarchyClassesQueryHandler = getHierarchyClassesQueryHandler;
        }

        // GET: /NationalClass/
        public ActionResult Index()
		{
            NationalClassSearchViewModel viewModel = new NationalClassSearchViewModel()
            {
                UserWriteAccess = this.GetWriteAccess()
            };            
            GetHierarchyParameters searchHierarchyParameters = new GetHierarchyParameters
            {
                HierarchyId = Hierarchies.National,
                IncludeNavigation = true
            };

            Hierarchy Hierarchy = getHierarchyQuery.Search(searchHierarchyParameters).Single();

            var hierarchyClasses = Hierarchy.HierarchyClass
                .Select(hc => new NationalClassGridViewModel
                {
                    HierarchyClassId = hc.hierarchyClassID,
                    HierarchyClassName = hc.hierarchyClassName,
                    HierarchyParentClassId = hc.hierarchyParentClassID,
                    HierarchyLevel = hc.hierarchyLevel,
                    HierarchyId = hc.hierarchyID,
                    NationalClassCode = HierarchyClassAccessor.GetNationalClassCode(hc)
                })
                .OrderBy(hc => hc.HierarchyClassName)
                .ToList();

            viewModel.NationalClasses = BuildNationalClassModelRecursive(hierarchyClasses, null).AsQueryable();

            return View(viewModel);
		}

        // GET: /NationalClass/Create/
        [HttpGet]
        [WriteAccessAuthorize]
        public ActionResult Create(int? hierarchyParentClassID)
        {
            HierarchyClass  parentHierarchyClass = null;
            if (hierarchyParentClassID.HasValue)
            {
                parentHierarchyClass = hierarchyClassQuery.Search(new GetHierarchyClassByIdParameters { HierarchyClassId = hierarchyParentClassID.Value });
            }
            NationalClassViewModel viewModel = new NationalClassViewModel
            {
                HierarchyId = Hierarchies.National,
                HierarchyLevel = parentHierarchyClass == null ? HierarchyLevels.NationalFamily : parentHierarchyClass.hierarchyLevel.Value + 1,
                HierarchyParentClassId = hierarchyParentClassID,
                HierarchyParentClassName = parentHierarchyClass == null ? string.Empty : parentHierarchyClass.hierarchyClassName
            };

            return View(viewModel);
        }

        // POST: /NationalClass/Create/{NationalClassViewModel}
        [HttpPost]
        [ValidateAntiForgeryToken]
        [WriteAccessAuthorize]
        public ActionResult Create(NationalClassViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            if (viewModel.HierarchyLevel == HierarchyLevels.NationalClass && viewModel.NationalClassCode == null)
            {
                ViewData["ErrorMessage"] = "National class code is required.";
                return View(viewModel);
            }

            AddNationalHierarchyManager manager = new AddNationalHierarchyManager
            {
                NationalHierarchy = new HierarchyClass 
                { 
                    hierarchyLevel = viewModel.HierarchyLevel,
                    hierarchyParentClassID = viewModel.HierarchyParentClassId,
                    hierarchyClassName = viewModel.HierarchyClassName,
                    hierarchyID = viewModel.HierarchyId
                },
                NationalClassCode = viewModel.NationalClassCode,
                UserName = User.Identity.Name
            };

            try
            {
                addNationalHierarchyManager.Execute(manager);
                this.cacheManager.ClearCacheForController("HierarchyClass");
            }
            catch (Exception ex)
            {
                var exceptionLogger = new ExceptionLogger(logger);
                exceptionLogger.LogException(ex, this.GetType(), MethodBase.GetCurrentMethod());

                ViewData["ErrorMessage"] = ex.Message;

                return View(viewModel);
            }

            TempData["SuccessMessage"] = $"Created {viewModel.HierarchyClassName} successfully.";

            return RedirectToAction("Index");
        }

        // GET: /NationalClass/Edit
        [HttpGet]
        [WriteAccessAuthorize]
        public ActionResult Edit(int hierarchyClassId)
        {
            if (hierarchyClassId < 1)
            {
                return RedirectToAction("Index");
            }
            var hierarchyClass = hierarchyClassQuery.Search(new GetHierarchyClassByIdParameters { HierarchyClassId = hierarchyClassId  });

            if(hierarchyClass.hierarchyID != Hierarchies.National)
            {
                string msg = $"Hierarchy class is not a National Class. HierarchyClassId = {hierarchyClassId}";
                logger.Error(msg);
                ViewData["ErrorMessage"] = msg;
                return View("Error");
            }

            NationalClassViewModel viewModel = new NationalClassViewModel
            {                
                    HierarchyClassId = hierarchyClass.hierarchyClassID,
                    HierarchyClassName = hierarchyClass.hierarchyClassName,
                    HierarchyParentClassId = hierarchyClass.hierarchyParentClassID,
                    HierarchyLevel = hierarchyClass.hierarchyLevel,
                    HierarchyId = hierarchyClass.hierarchyID,
                    NationalClassCode = HierarchyClassAccessor.GetNationalClassCode(hierarchyClass),
                    HierarchyName = HierarchyNames.National
             };

            return View(viewModel);
        }

        // POST: /NationalClass/Edit/{NationalClassViewModel}
        [HttpPost]
        [ValidateAntiForgeryToken]
        [WriteAccessAuthorize]
        public ActionResult Edit(NationalClassViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            if(viewModel.HierarchyLevel == HierarchyLevels.NationalClass && viewModel.NationalClassCode == null)
            {
                ViewData["ErrorMessage"] = "National class code is required.";
                return View(viewModel);
            }

            UpdateNationalHierarchyManager command = new UpdateNationalHierarchyManager
            {
                NationalHierarchy = new HierarchyClass
                {
                    hierarchyClassID = viewModel.HierarchyClassId,
                    hierarchyLevel = viewModel.HierarchyLevel,
                    hierarchyParentClassID = viewModel.HierarchyParentClassId,
                    hierarchyClassName = viewModel.HierarchyClassName,
                    hierarchyID = viewModel.HierarchyId
                },
                NationalClassCode = viewModel.NationalClassCode,
                UserName = User.Identity.Name
            };

            try
            {
                updateNationalHierarchyManager.Execute(command);
                this.cacheManager.ClearCacheForController("HierarchyClass");
            }
            catch (Exception ex)
            {
                var exceptionLogger = new ExceptionLogger(logger);
                exceptionLogger.LogException(ex, this.GetType(), MethodBase.GetCurrentMethod());
                ViewData["ErrorMessage"] = ex.Message;
                                
                return View(viewModel);
            }

            TempData["SuccessMessage"] = $"Updated {viewModel.HierarchyClassName} successfully.";

            return RedirectToAction("Index");
        }


        // GET: /NationalClass/Delete
        [HttpGet]
        [WriteAccessAuthorize]
        public ActionResult Delete(int hierarchyClassId)
        {
            if (hierarchyClassId < 1)
            {
                return RedirectToAction("Index");
            }

            var hierarchyClass = hierarchyClassQuery.Search(new GetHierarchyClassByIdParameters { HierarchyClassId = hierarchyClassId });

            NationalClassViewModel viewModel = new NationalClassViewModel
            {
                HierarchyClassId = hierarchyClass.hierarchyClassID,
                HierarchyClassName = hierarchyClass.hierarchyClassName,
                HierarchyParentClassId = hierarchyClass.hierarchyParentClassID,
                HierarchyLevel = hierarchyClass.hierarchyLevel,
                HierarchyId = hierarchyClass.hierarchyID,
                NationalClassCode = HierarchyClassAccessor.GetNationalClassCode(hierarchyClass),
                HierarchyName = HierarchyNames.National
            };

            return View(viewModel);
        }

        // POST: /NationalClass/Delete/{NationalClassViewModel}
        [WriteAccessAuthorize]
        public ActionResult Delete(NationalClassViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            GetHierarchyClassByIdParameters hierarchyClassParameters = new GetHierarchyClassByIdParameters { HierarchyClassId = viewModel.HierarchyClassId };
            HierarchyClass deletedHierarchyClass = hierarchyClassQuery.Search(hierarchyClassParameters);

            // Make sure the node is not attached to any items.
            if (deletedHierarchyClass.ItemHierarchyClass.Count > 0)
            {
                ViewData["ErrorMessage"] = "Error: This hierarchy class is linked to items, so it cannot be deleted.";
                return View(viewModel);
            }

            // Make sure the node does not have any children.
            if (deletedHierarchyClass.HierarchyClass1.Count > 0)
            {
                ViewData["ErrorMessage"] = "Error: This hierarchy class contains subclasses, so it cannot be deleted.";
                return View(viewModel);
            }

            var command = new DeleteNationalHierarchyManager
            {
                DeletedHierarchyClass = deletedHierarchyClass,
                NationalClassCode = viewModel.NationalClassCode
            };

            try
            {
                deleteNationalHierarchyManager.Execute(command);
                this.cacheManager.ClearCacheForController("HierarchyClass");
            }
            catch (CommandException exception)
            {
                var exceptionLogger = new ExceptionLogger(logger);
                exceptionLogger.LogException(exception, this.GetType(), MethodBase.GetCurrentMethod());
                ViewData["ErrorMessage"] = "Error: There was a problem with applying this delete on the database.";
                return View(viewModel);
            }

            TempData["SuccessMessage"] = $"Deleted {viewModel.HierarchyClassName} successfully.";

            return RedirectToAction("Index");
        }

        private List<NationalClassGridViewModel> BuildNationalClassModelRecursive(List<NationalClassGridViewModel> allHierarchyClasses, NationalClassGridViewModel parentHierarchyClass)
		{
			int? hierarchyParentClassId = null;

			if (parentHierarchyClass != null)
			{
				hierarchyParentClassId = parentHierarchyClass.HierarchyClassId;
			}

			var childHierarchyClasses = allHierarchyClasses.Where(e => e.HierarchyParentClassId == hierarchyParentClassId).ToList();

			// If all classes are top-level classes, then there are no children to traverse.  Return the list of hierarchy classes.
			if (childHierarchyClasses.Count == allHierarchyClasses.Count)
			{
                return allHierarchyClasses;
			}

            List<NationalClassGridViewModel> nationalClassGridViewModels = new List<NationalClassGridViewModel>();

			foreach (var hc in childHierarchyClasses)
			{
                nationalClassGridViewModels.Add(new NationalClassGridViewModel()
				{
					HierarchyClassId = hc.HierarchyClassId,
                    HierarchyLevel = hc.HierarchyLevel,
                    HierarchyId = hc.HierarchyId,
					HierarchyParentClassId = hc.HierarchyParentClassId,
                    HierarchyClassName = hc.HierarchyClassName,
                    NationalClassCode = hc.NationalClassCode,
                    AddNodeLink = String.Format("<a href=/NationalClass/Create?hierarchyParentClassID={0}>Add</a>", hc.HierarchyClassId.ToString()),
                    EditNodeLink = String.Format("<a href=/NationalClass/Edit?hierarchyClassId={0}>Edit</a>", hc.HierarchyClassId.ToString()),
                    DeleteNodeLink = String.Format("<a href=/NationalClass/Delete?hierarchyClassId={0}>Delete</a>", hc.HierarchyClassId.ToString()),
                    HierarchySubClasses = BuildNationalClassModelRecursive(allHierarchyClasses, hc)
				});
			}

            return nationalClassGridViewModels;
		}

        [HttpPost]
        public void Export()
        {
            IEnumerable<HierarchyClassModel> nationalHierarchyModel = getHierarchyClassesQueryHandler.Search(
                new GetHierarchyClassesParameters
                {
                    HierarchyId = Hierarchies.National
                });

            var nationalHierarchyClass = nationalHierarchyModel
                .ToDictionary(nc => nc.HierarchyClassId.ToString(), nc => nc.HierarchyLineage + " | " + nc.HierarchyClassId);

            var nationalClassExporter = exporterService.GetNationalClassExporter();
            nationalClassExporter.ExportData = nationalHierarchyClass;
            nationalClassExporter.Export();

            SendForDownload(nationalClassExporter.ExportModel.ExcelWorkbook, nationalClassExporter.ExportModel.ExcelWorkbook.CurrentFormat, "NationalClassHierarchy");
        }

        private void SendForDownload(Workbook document, WorkbookFormat excelFormat, string source)
        {
            string documentFileNameRoot = String.Format("IconExport_{0}_{1}.xlsx", source, DateTime.Now.ToString("yyyyMMddHHmmss"));

            Response.Clear();
            Response.AppendHeader("content-disposition", "attachment; filename=" + documentFileNameRoot);
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.SetCookie(new HttpCookie("fileDownload", "true") { Path = "/" });
            document.SetCurrentFormat(excelFormat);
            document.Save(Response.OutputStream);
            Response.End();
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