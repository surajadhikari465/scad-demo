using DevTrends.MvcDonutCaching;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using Icon.Web.Common;
using Icon.Web.Common.Utility;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Managers;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Attributes;
using Icon.Web.Mvc.Extensions;
using Icon.Web.Mvc.Models;
using Icon.Web.Mvc.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Icon.Web.Controllers
{
    public class HierarchyClassController : Controller
    {
        private ILogger logger;
        private IQueryHandler<GetHierarchyParameters, List<Hierarchy>> hierarchyQuery;
        private IQueryHandler<GetHierarchyClassByIdParameters, HierarchyClass> hierarchyClassQuery;
        private IManagerHandler<DeleteHierarchyClassManager> deleteHierarchyClass;
        private IManagerHandler<AddHierarchyClassManager> addManager;
        private IManagerHandler<UpdateHierarchyClassManager> updateManager;
        private IQueryHandler<GetHierarchyClassesParameters, IEnumerable<HierarchyClassModel>> getHierarchyClassesQueryHandler;
        private IQueryHandler<GetMerchandiseHierarchyClassTraitsParameters, IEnumerable<MerchandiseHierarchyClassTrait>> getMerchandiseHierarchyTraitsQueryHandler;

        private IDonutCacheManager cacheManager;
        public HierarchyClassController(ILogger logger,
            IQueryHandler<GetHierarchyParameters, List<Hierarchy>> hierarchyQuery,
            IQueryHandler<GetHierarchyClassByIdParameters, HierarchyClass> hierarchyClassQuery,
            IManagerHandler<DeleteHierarchyClassManager> deleteHierarchyClass,
            IManagerHandler<AddHierarchyClassManager> addManager,
            IManagerHandler<UpdateHierarchyClassManager> updateManager,
            IQueryHandler<GetHierarchyClassesParameters, IEnumerable<HierarchyClassModel>> getHierarchyClassesQueryHandler,
            IQueryHandler<GetMerchandiseHierarchyClassTraitsParameters, IEnumerable<MerchandiseHierarchyClassTrait>> getMerchandiseHierarchyTraitsQueryHandler,
            IDonutCacheManager cacheManager)
        {
            this.logger = logger;
            this.hierarchyQuery = hierarchyQuery;
            this.hierarchyClassQuery = hierarchyClassQuery;
            this.deleteHierarchyClass = deleteHierarchyClass;
            this.addManager = addManager;
            this.updateManager = updateManager;
            this.getHierarchyClassesQueryHandler = getHierarchyClassesQueryHandler;
            this.getMerchandiseHierarchyTraitsQueryHandler = getMerchandiseHierarchyTraitsQueryHandler;
            this.cacheManager = cacheManager;

        }

        //
        // GET: /HierarchyClass/
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Hierarchy");
        }

        // GET: /HierarchyClass/Edit?hierarchyClassId={0}
        [HttpGet]
        [WriteAccessAuthorize]
        public ActionResult Edit(int hierarchyClassId)
        {
            // Get HierarchyClass and Finanacial Hierarchy from Database
            var hierarchyClass = hierarchyClassQuery.Search(new GetHierarchyClassByIdParameters { HierarchyClassId = hierarchyClassId });

            if (hierarchyClassId == 0 || hierarchyClassId < 1)
            {
                return RedirectToAction("Index", "Hierarchy", new { SelectedHeirachyId = hierarchyClass.hierarchyID });
            }

            if (hierarchyClass.hierarchyID == Hierarchies.Brands)
            {
                return RedirectToAction("Edit", "Brand", new { hierarchyClassId = hierarchyClassId });
            }

            if (hierarchyClass.hierarchyID == Hierarchies.Financial)
            {
                return RedirectToAction("Index", "Hierarchy", new { SelectedHeirachyId = hierarchyClass.hierarchyID });
            }

            // Populate view model
            HierarchyClassViewModel viewModel = new HierarchyClassViewModel
            {
                HierarchyId = hierarchyClass.Hierarchy.hierarchyID,
                HierarchyName = hierarchyClass.Hierarchy.hierarchyName,
                HierarchyClassId = hierarchyClass.hierarchyClassID,
                HierarchyParentClassId = hierarchyClass.hierarchyParentClassID,
                HierarchyParentClassName = HierarchyClassAccessor.GetHierarchyParentName(hierarchyClass),
                HierarchyClassName = hierarchyClass.hierarchyClassName,
                HierarchyLevel = hierarchyClass.hierarchyLevel,
                SubTeam = HierarchyClassAccessor.GetSubTeam(hierarchyClass),
                TaxAbbreviation = HierarchyClassAccessor.GetTaxAbbreviation(hierarchyClass),
                TaxRomance = HierarchyClassAccessor.GetTaxRomance(hierarchyClass),
                HierarchyLevelName = HierarchyClassAccessor.GetHierarchyLevelName(hierarchyClass),
                NonMerchandiseTrait = HierarchyClassAccessor.GetNonMerchandiseTrait(hierarchyClass),
                ProhibitDiscount = HierarchyClassAccessor.GetProhibitDiscount(hierarchyClass),
                SubBrickCode = HierarchyClassAccessor.GetSubBrickCode(hierarchyClass)
            };

            if (viewModel.HierarchyName == HierarchyNames.Merchandise && viewModel.HierarchyLevel == HierarchyLevels.SubBrick)
            {
                viewModel.SubTeamList = CreateSubTeamDropDown();
                if (!String.IsNullOrEmpty(viewModel.SubTeam))
                {
                    var financialHierarchy = hierarchyQuery.Search(new GetHierarchyParameters { HierarchyName = "Financial" });
                    viewModel.SelectedSubTeam = financialHierarchy.Single().HierarchyClass.Single(hc => hc.hierarchyClassID.ToString() == viewModel.SubTeam).hierarchyClassID;
                }

                if (!String.IsNullOrEmpty(viewModel.NonMerchandiseTrait))
                {
                    viewModel.SelectedNonMerchandiseTrait = Int32.Parse(viewModel.NonMerchandiseTraitList.Single(nm => nm.Text == viewModel.NonMerchandiseTrait).Value);
                }
            }

            return View(viewModel);
        }

        // POST: /HierarchyClass/Edit/
        [HttpPost]
        [ValidateAntiForgeryToken]
        [WriteAccessAuthorize]
        public ActionResult Edit(HierarchyClassViewModel viewModel)
        {
            // Tax Abbreviation is required (this has to be done manually since all hierarchies share the same model)
            if (string.IsNullOrWhiteSpace(viewModel.TaxAbbreviation) && viewModel.HierarchyName == HierarchyNames.Tax)
            {
                ViewData["ErrorMessage"] = "Tax Abbreviation is required.";
                return View(viewModel);
            }

            // Tax Romance is required (this has to be done manually since all hierarchies share the same model)
            if (string.IsNullOrWhiteSpace(viewModel.TaxRomance) && viewModel.HierarchyName == HierarchyNames.Tax)
            {
                ViewData["ErrorMessage"] = "Tax Romance is required.";
                return View(viewModel);
            }

            if (!ModelState.IsValid)
            {
                var allErrors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                ViewData["ErrorMessage"] = $"There was an error during update: {allErrors.First()}";
                if (viewModel.HierarchyName == HierarchyNames.Merchandise)
                {
                    viewModel.SubTeamList = CreateSubTeamDropDown();
                }
                return View(viewModel);
            }

            var hierarchyClass = new HierarchyClass
            {
                hierarchyID = viewModel.HierarchyId,
                hierarchyClassID = viewModel.HierarchyClassId,
                hierarchyParentClassID = viewModel.HierarchyParentClassId,
                hierarchyClassName = viewModel.HierarchyClassName.Trim(),
                hierarchyLevel = viewModel.HierarchyLevel,
            };

            var manager = new UpdateHierarchyClassManager
            {
                UpdatedHierarchyClass = hierarchyClass,
                TaxAbbreviation = viewModel.TaxAbbreviation,
                SubTeamHierarchyClassId = viewModel.SelectedSubTeam,
                NonMerchandiseTrait = viewModel.NonMerchandiseTraitList.First(nm => nm.Value == viewModel.SelectedNonMerchandiseTrait.ToString()).Text,
                ProhibitDiscount = viewModel.ProhibitDiscount ? viewModel.ProhibitDiscount.ToString().ToLower() : string.Empty,
                SubBrickCode = viewModel.SubBrickCode,
                UserName = User.Identity.Name,
                TaxRomance = viewModel.TaxRomance,
                ModifiedDateTimeUtc = DateTime.UtcNow.ToFormattedDateTimeString()
            };

            try
            {
                updateManager.Execute(manager);
                this.cacheManager.ClearCacheForController("HierarchyClass");
            }
            catch (Exception exception)
            {
                var exceptionLogger = new ExceptionLogger(logger);
                exceptionLogger.LogException(exception, this.GetType(), MethodBase.GetCurrentMethod());
                ViewData["ErrorMessage"] = exception.Message;
                if (viewModel.HierarchyName == HierarchyNames.Merchandise)
                {
                    viewModel.SubTeamList = CreateSubTeamDropDown();
                }
                return View(viewModel);
            }

            TempData["SuccessMessage"] = "Update was successful.";
            return RedirectToAction("Edit", new { @hierarchyClassId = viewModel.HierarchyClassId });
        }

        // GET :  /HierarchyClass/Create
        [HttpGet]
        [WriteAccessAuthorize]
        public ActionResult Create(int? parentId, int hierarchyId)
        {
            if ((hierarchyId == 0 || hierarchyId < 1) || (parentId == null || parentId < 0))
            {
                return RedirectToAction("Index");
            }

            HierarchyClassViewModel viewModel = new HierarchyClassViewModel();
            GetHierarchyClassByIdParameters hierarchyClassParameters = new GetHierarchyClassByIdParameters { HierarchyClassId = parentId.HasValue ? parentId.Value : 0 };
            GetHierarchyParameters hierarchyParameters = new GetHierarchyParameters { HierarchyId = hierarchyId };

            // A root node is being added, so find the details of the hierarchy it's being added to.
            if (parentId == 0)
            {
                Hierarchy hierarchy = hierarchyQuery.Search(hierarchyParameters).Single();
                viewModel.HierarchyId = hierarchy.hierarchyID;
                viewModel.HierarchyName = hierarchy.hierarchyName;
                viewModel.HierarchyLevel = HierarchyLevels.Parent;
                viewModel.HierarchyLevelName = hierarchy.HierarchyPrototype.Single(hp => hp.hierarchyLevel == viewModel.HierarchyLevel).hierarchyLevelName;
            }

            // A child node is being added, so find details of the parent.
            if (parentId > 0)
            {
                HierarchyClass parentHierarchyClass = hierarchyClassQuery.Search(hierarchyClassParameters);
                viewModel.HierarchyId = parentHierarchyClass.Hierarchy.hierarchyID;
                viewModel.HierarchyName = parentHierarchyClass.Hierarchy.hierarchyName;
                viewModel.HierarchyLevel = parentHierarchyClass.hierarchyLevel + 1;
                viewModel.HierarchyParentClassId = parentHierarchyClass.hierarchyClassID;
                viewModel.HierarchyParentClassName = parentHierarchyClass.hierarchyClassName;
                viewModel.HierarchyLevelName = parentHierarchyClass.Hierarchy.HierarchyPrototype.Single(hp => hp.hierarchyLevel == viewModel.HierarchyLevel).hierarchyLevelName;

                // Populate SubTeam Information for a Level 5 hierarchy
                if (parentHierarchyClass.Hierarchy.hierarchyName == HierarchyNames.Merchandise && viewModel.HierarchyLevel == HierarchyLevels.SubBrick)
                {
                    viewModel.SubTeamList = CreateSubTeamDropDown();
                }
            }

            // The only kind of hierarchy class that can be added through this controller is a Merchandise Level 5 SubBrick.  We don't want to allow the ability to create 
            // anything else should the user access this controller directly through a query string in the url.
            if (viewModel.HierarchyName == HierarchyNames.Brands)
            {
                return RedirectToAction("Create", "Brand", null);
            }

            if (!(viewModel.HierarchyName == HierarchyNames.Merchandise && viewModel.HierarchyLevel == HierarchyLevels.SubBrick))
            {
                return RedirectToAction("Index", "Hierarchy", new { SelectedHeirachyId = viewModel.HierarchyId });
            }

            return View(viewModel);
        }

        // POST: /HierarchyClass/Create/{HierarchyClass}
        [HttpPost]
        [ValidateAntiForgeryToken]
        [WriteAccessAuthorize]
        public ActionResult Create(HierarchyClassViewModel viewModel)
        {
            viewModel.SubTeamList = CreateSubTeamDropDown();

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            HierarchyClass hierarchyClass = new HierarchyClass()
            {
                hierarchyID = viewModel.HierarchyId,
                hierarchyClassID = viewModel.HierarchyClassId,
                hierarchyParentClassID = viewModel.HierarchyParentClassId,
                hierarchyClassName = viewModel.HierarchyClassName.Trim(),
                hierarchyLevel = viewModel.HierarchyLevel
            };

            AddHierarchyClassManager manager = new AddHierarchyClassManager
            {
                NewHierarchyClass = hierarchyClass,
                TaxAbbreviation = viewModel.TaxAbbreviation,
                SubTeamHierarchyClassId = viewModel.SelectedSubTeam,
                NonMerchandiseTrait = viewModel.NonMerchandiseTraitList.First(nm => nm.Value == viewModel.SelectedNonMerchandiseTrait.ToString()).Text,
                SubBrickCode = viewModel.SubBrickCode
            };

            try
            {
                addManager.Execute(manager);
                this.cacheManager.ClearCacheForController("HierarchyClass");
            }
            catch (Exception exception)
            {
                var exceptionLogger = new ExceptionLogger(logger);
                exceptionLogger.LogException(exception, this.GetType(), MethodBase.GetCurrentMethod());
                ViewData["ErrorMessage"] = exception.Message;
                viewModel.SubTeamList = CreateSubTeamDropDown();
                return View(viewModel);
            }

            HierarchySearchViewModel hierarchySearchViewModel = new HierarchySearchViewModel { SelectedHierarchyId = viewModel.HierarchyId };
            return RedirectToAction("Index", "Hierarchy", hierarchySearchViewModel);
        }

        // GET: /HierarchyClass/Delete/{HierarchyClassID}
        [HttpGet]
        [WriteAccessAuthorize]
        public ActionResult Delete(int hierarchyClassId)
        {
            if (hierarchyClassId < 1)
            {
                return RedirectToAction("Index");
            }

            GetHierarchyClassByIdParameters hierarchyClassParameters = new GetHierarchyClassByIdParameters { HierarchyClassId = hierarchyClassId };
            HierarchyClass hierarchyClass = hierarchyClassQuery.Search(hierarchyClassParameters);

            HierarchyClassViewModel viewModel = new HierarchyClassViewModel()
            {
                HierarchyId = hierarchyClass.Hierarchy.hierarchyID,
                HierarchyName = hierarchyClass.Hierarchy.hierarchyName,
                HierarchyClassId = hierarchyClass.hierarchyClassID,
                HierarchyParentClassId = hierarchyClass.hierarchyParentClassID,
                HierarchyParentClassName = HierarchyClassAccessor.GetHierarchyParentName(hierarchyClass),
                HierarchyClassName = hierarchyClass.hierarchyClassName,
                HierarchyLevel = hierarchyClass.hierarchyLevel,
                HierarchyLevelName = HierarchyClassAccessor.GetHierarchyLevelName(hierarchyClass),
                TaxAbbreviation = HierarchyClassAccessor.GetTaxAbbreviation(hierarchyClass),
                TaxRomance = HierarchyClassAccessor.GetTaxRomance(hierarchyClass),
                SubTeam = HierarchyClassAccessor.GetSubTeam(hierarchyClass),
                NonMerchandiseTrait = HierarchyClassAccessor.GetNonMerchandiseTrait(hierarchyClass)
            };

            if (!String.IsNullOrEmpty(viewModel.SubTeam))
            {
                viewModel.SelectedSubTeam = Int32.TryParse(viewModel.SubTeam, out int subTeamId) ? subTeamId : 0; // SubTeam now contains HierarchyClassID of the sub-team so should match SelectList Value
                viewModel.SubTeamList = CreateSubTeamDropDown();
            }

            return View(viewModel);
        }

        // POST: /HierarchyClass/Delete/
        [HttpPost]
        [ValidateAntiForgeryToken]
        [WriteAccessAuthorize]
        public ActionResult Delete(HierarchyClassViewModel viewModel)
        {
            viewModel.SubTeamList = CreateSubTeamDropDown();

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

            var command = new DeleteHierarchyClassManager
            {
                DeletedHierarchyClass = deletedHierarchyClass
            };

            try
            {
                deleteHierarchyClass.Execute(command);
                this.cacheManager.ClearCacheForController("HierarchyClass");
            }
            catch (CommandException exception)
            {
                var exceptionLogger = new ExceptionLogger(logger);
                exceptionLogger.LogException(exception, this.GetType(), MethodBase.GetCurrentMethod());
                ViewData["ErrorMessage"] = "Error: There was a problem with applying this delete on the database.";
                return View(viewModel);
            }

            switch (viewModel.HierarchyId)
            {
                case Hierarchies.Brands:
                    return RedirectToAction("Index", "Brand");
                case Hierarchies.Manufacturer:
                    return RedirectToAction("Index", "Manufacturer");
                case Hierarchies.Tax:
                    return RedirectToAction("Index", "Hierarchy", new { SelectedHierarchyId = Hierarchies.Tax });
            }

            HierarchySearchViewModel hierarchySearchViewModel = new HierarchySearchViewModel() { SelectedHierarchyId = viewModel.HierarchyId };

            return RedirectToAction("Index", "Hierarchy", hierarchySearchViewModel);
        }

        public ActionResult ClearCache()
        {
            this.cacheManager.ClearCacheForController("HierarchyClass");
            this.cacheManager.ClearCacheForController("Hierarchy");
            this.cacheManager.ClearCacheForController("Attribute");
            return View();
        }

        private IEnumerable<SelectListItem> CreateSubTeamDropDown()
        {
            var financialHierarchy = hierarchyQuery.Search(new GetHierarchyParameters { HierarchyName = HierarchyNames.Financial });
            var subTeams = financialHierarchy.Single().HierarchyClass.Select(hc => new SelectListItem
            {
                Value = hc.hierarchyClassID.ToString(),
                Text = hc.hierarchyClassName
            }).ToList();

            return subTeams.OrderBy(st => st.Text);
        }

        [HttpGet]
        [DonutOutputCache(Duration = 3600, Location = System.Web.UI.OutputCacheLocation.Server, VaryByParam = "hierarchyId;columnFilters")]
        public ActionResult ByHierarchyId(int hierarchyId, string columnFilters)
        {
            ContentResult result;

            var hierarchyClasses = getHierarchyClassesQueryHandler
                .Search(new GetHierarchyClassesParameters
                {
                    HierarchyId = hierarchyId
                })
                .Select(s => new HierarchyClassViewModel(s));

            if (!string.IsNullOrWhiteSpace(columnFilters))
                result = Content(JsonConvert.SerializeObject(hierarchyClasses.ShapeData(columnFilters)), "application/json");
            else
                result = Content(JsonConvert.SerializeObject(hierarchyClasses), "application/json");

            return result;
        }

        [HttpGet]
        public ActionResult MerchandiseHierarchyTraits()
        {
            var merchandiseTraits = getMerchandiseHierarchyTraitsQueryHandler
                .Search(new GetMerchandiseHierarchyClassTraitsParameters())
                .Select(s => new MerchandiseHierarchyClassTraitViewModel(s));

            return Content(JsonConvert.SerializeObject(merchandiseTraits), "application/json");
        }

        public ActionResult ByHierarchyName(string hierarchyName)
        {
            var hierarchies = hierarchyQuery.Search(new GetHierarchyParameters { HierarchyName = hierarchyName });

            return this.ComboDataSource(hierarchies.First().hierarchyID);
        }

        /// <summary>
        /// This method returns a list of hierarchies based on the query parameters hierarchyId, filter and inititalSelection.
        /// Filter must be passed and this method is designed to work with the infragistics comboBox.
        /// This method can be called with a filter or it can be called with inititalSelection which is 
        /// a single hierarchyClassId that you want the hierarchy returned for. This exists because
        /// when you load a combobox in edit mode you want to load the existing selected record but you 
        /// don't want to load all of the records. Passing inititalSelection will return that single record.
        /// Otherwise when searching you must pass the filter which is an OData statment that we parse the contains clause
        /// from.
        /// </summary>
        /// <param name="hierarchyId"></param>
        /// <returns></returns>
        public ActionResult ComboDataSource(int hierarchyId)
        {
            string hierarchyLineageFilter = null;
            int? hierarchyClassIdFilter = null;
            string filter = this.Request.QueryString["$filter"];

            // we are filtering from a combobox search request with the format indexof(tolower(HierarchyLineage),'test') ge 0
            if (filter != null)
            {
                Regex hierarchyClassNameFilterRegex = new Regex(@"^(indexof\(tolower\(HierarchyLineage\),')(.*)('\) ge 0)$");

                if (hierarchyClassNameFilterRegex.IsMatch(filter))
                {
                    string query = filter
                        .Replace("indexof(tolower(HierarchyLineage),'", "")
                        .Replace("') ge 0", "")
                        .Trim();
                    hierarchyLineageFilter = query;
                }
            }
            else
            {
                // we are only going to return the single hierarchy node that was passed in inititalSelection
                hierarchyClassIdFilter = Convert.ToInt32(this.Request.QueryString["initialSelection"]);
            }


            if (string.IsNullOrWhiteSpace(hierarchyLineageFilter) && hierarchyClassIdFilter == null)
            {
                // if the request is missing a filter and inititalSelection was also not passed return nothing instead of everything
                return Content(JsonConvert.SerializeObject(new List<HierarchyClassModel>() { }), "application/json");
            }
            else if (!string.IsNullOrWhiteSpace(hierarchyLineageFilter) && hierarchyLineageFilter.Length < 2)
            {
                // if the request is less than 2 characters don't return anything yet. This is to help with searching performance.
                return Content(JsonConvert.SerializeObject(new List<HierarchyClassModel>() { }), "application/json");
            }
            else
            {
                var hierarchyClasses = getHierarchyClassesQueryHandler.Search(new GetHierarchyClassesParameters
                {
                    HierarchyId = hierarchyId,
                    HierarchyLineageFilter = hierarchyLineageFilter,
                    HierarchyClassId = hierarchyClassIdFilter
                });
                return Content(JsonConvert.SerializeObject(hierarchyClasses), "application/json");
            }
        }
    }
}