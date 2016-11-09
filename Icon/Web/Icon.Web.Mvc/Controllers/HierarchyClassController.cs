using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using Icon.Web.Common;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Managers;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Attributes;
using Icon.Web.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        private ICommandHandler<ClearHierarchyClassCacheCommand> clearHierarchyClassCacheCommandHandler;

		public HierarchyClassController(ILogger logger,
            IQueryHandler<GetHierarchyParameters, List<Hierarchy>> hierarchyQuery,
			IQueryHandler<GetHierarchyClassByIdParameters, HierarchyClass> hierarchyClassQuery,
            IManagerHandler<DeleteHierarchyClassManager> deleteHierarchyClass,
            IManagerHandler<AddHierarchyClassManager> addManager,
            IManagerHandler<UpdateHierarchyClassManager> updateManager,
            ICommandHandler<ClearHierarchyClassCacheCommand> clearHierarchyClassCacheCommandHandler)
		{
			this.logger = logger;
            this.hierarchyQuery = hierarchyQuery;
			this.hierarchyClassQuery = hierarchyClassQuery;
			this.deleteHierarchyClass = deleteHierarchyClass;
            this.addManager = addManager;
            this.updateManager = updateManager;
            this.clearHierarchyClassCacheCommandHandler = clearHierarchyClassCacheCommandHandler;
		}

		//
		// GET: /HierarchyClass/
		public ActionResult Index()
		{
			return RedirectToAction("Index", "Hierarchy");
		}

		// GET: /HierarchyClass/Edit?hierarchyClassId={0}
		[HttpGet]
		public ActionResult Edit(int hierarchyClassId)
		{
			if (hierarchyClassId == 0 || hierarchyClassId < 1)
			{
				return RedirectToAction("Index", "Hierarchy");
			}

			// Get HierarchyClass and Finanacial Hierarchy from Database
			var hierarchyClass = hierarchyClassQuery.Search(new GetHierarchyClassByIdParameters { HierarchyClassId = hierarchyClassId });
			var financialHierarchy = hierarchyQuery.Search(new GetHierarchyParameters { HierarchyName = "Financial" });

            if (hierarchyClass.hierarchyID == Hierarchies.Brands)
            {
                return RedirectToAction("Edit", "Brand", new { hierarchyClassId = hierarchyClassId });
            }

            if (hierarchyClass.hierarchyID == Hierarchies.Financial)
            {
                return RedirectToAction("Index", "Hierarchy");
            }

			// Populate view model
			HierarchyClassViewModel viewModel = new HierarchyClassViewModel();
			viewModel.HierarchyId = hierarchyClass.Hierarchy.hierarchyID;
			viewModel.HierarchyName = hierarchyClass.Hierarchy.hierarchyName;
			viewModel.HierarchyClassId = hierarchyClass.hierarchyClassID;
			viewModel.HierarchyParentClassId = hierarchyClass.hierarchyParentClassID;
			viewModel.HierarchyParentClassName = HierarchyClassAccessor.GetHierarchyParentName(hierarchyClass);
			viewModel.HierarchyClassName = hierarchyClass.hierarchyClassName;
			viewModel.HierarchyLevel = hierarchyClass.hierarchyLevel;
			viewModel.SubTeam = HierarchyClassAccessor.GetSubTeam(hierarchyClass);
			viewModel.TaxAbbreviation = HierarchyClassAccessor.GetTaxAbbreviation(hierarchyClass);
            viewModel.TaxRomance = HierarchyClassAccessor.GetTaxRomance(hierarchyClass);
			viewModel.HierarchyLevelName = HierarchyClassAccessor.GetHierarchyLevelName(hierarchyClass);
			viewModel.GlAccount = HierarchyClassAccessor.GetGlAccount(hierarchyClass);
            viewModel.NonMerchandiseTrait = HierarchyClassAccessor.GetNonMerchandiseTrait(hierarchyClass);
            viewModel.ProhibitDiscount = HierarchyClassAccessor.GetProhibitDiscount(hierarchyClass);
            viewModel.SubBrickCode = HierarchyClassAccessor.GetSubBrickCode(hierarchyClass);

			if (viewModel.HierarchyName == HierarchyNames.Merchandise && viewModel.HierarchyLevel == HierarchyLevels.SubBrick)
			{
                viewModel.SubTeamList = CreateSubTeamDropDown();
                if (!String.IsNullOrEmpty(viewModel.SubTeam))
                {
                    viewModel.SelectedSubTeam = financialHierarchy.Single().HierarchyClass.Single(hc => hc.hierarchyClassName == viewModel.SubTeam).hierarchyClassID;
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
            if (viewModel.TaxAbbreviation == null && viewModel.HierarchyName == HierarchyNames.Tax)
            {
                ViewData["ErrorMessage"] = "Tax Abbreviation is required.";
                return View(viewModel);
            }

            // Tax Romance is required (this has to be done manually since all hierarchies share the same model)
            if (viewModel.TaxRomance == null && viewModel.HierarchyName == HierarchyNames.Tax)
            {
                ViewData["ErrorMessage"] = "Tax Romance is required.";
                return View(viewModel);
            }

            if (!ModelState.IsValid)
			{
                var allErrors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                ViewData["ErrorMessage"] = String.Format("There was an error during update: {0}", allErrors.First());
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
                GlAccount = viewModel.GlAccount,
                SubTeamHierarchyClassId = viewModel.SelectedSubTeam,
                NonMerchandiseTrait = viewModel.NonMerchandiseTraitList.First(nm => nm.Value == viewModel.SelectedNonMerchandiseTrait.ToString()).Text,
                ProhibitDiscount = viewModel.ProhibitDiscount ? "1" : String.Empty,
                SubBrickCode = viewModel.SubBrickCode,
                UserName = User.Identity.Name,
                TaxRomance = viewModel.TaxRomance
            };            

			try
			{
                updateManager.Execute(manager);
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
                return RedirectToAction("Index", "Hierarchy", null);
            }

			return View(viewModel);
		}

		// POST: /HierarchyClass/Create/{HierarchyClass}
		[HttpPost]
		[ValidateAntiForgeryToken]
        [WriteAccessAuthorize]
        public ActionResult Create(HierarchyClassViewModel viewModel)
		{
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
                GlAccount = viewModel.GlAccount,
                TaxAbbreviation = viewModel.TaxAbbreviation,
                SubTeamHierarchyClassId = viewModel.SelectedSubTeam,
                NonMerchandiseTrait = viewModel.NonMerchandiseTraitList.First(nm => nm.Value == viewModel.SelectedNonMerchandiseTrait.ToString()).Text,
                SubBrickCode = viewModel.SubBrickCode
            };

			try
			{
                addManager.Execute(manager);
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
			return RedirectToAction("Index", "Hierarchy", new { @HierarchySearchViewModel = hierarchySearchViewModel });
		}

		// GET: /HierarchyClass/Delete/{HierarchyClassID}
		[HttpGet]
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
                GlAccount = HierarchyClassAccessor.GetGlAccount(hierarchyClass),
                SubTeam = HierarchyClassAccessor.GetSubTeam(hierarchyClass),
                NonMerchandiseTrait = HierarchyClassAccessor.GetNonMerchandiseTrait(hierarchyClass)
			};

            if (!String.IsNullOrEmpty(viewModel.SubTeam))
            {
                var financialHierarchy = hierarchyQuery.Search(new GetHierarchyParameters { HierarchyName = "Financial" });
                viewModel.SelectedSubTeam = financialHierarchy.Single().HierarchyClass.FirstOrDefault(hc => hc.hierarchyClassName == viewModel.SubTeam).hierarchyClassID;
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
			}
			catch (CommandException exception)
			{
                var exceptionLogger = new ExceptionLogger(logger);
                exceptionLogger.LogException(exception, this.GetType(), MethodBase.GetCurrentMethod());
                ViewData["ErrorMessage"] = "Error: There was a problem with applying this delete on the database.";
                return View(viewModel);
			}

			HierarchySearchViewModel hierarchySearchViewModel = new HierarchySearchViewModel() { SelectedHierarchyId = viewModel.HierarchyId };

			return RedirectToAction("Index", hierarchySearchViewModel);
		}

        public ActionResult ClearCache()
        {
            clearHierarchyClassCacheCommandHandler.Execute(new ClearHierarchyClassCacheCommand());
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
	}
}