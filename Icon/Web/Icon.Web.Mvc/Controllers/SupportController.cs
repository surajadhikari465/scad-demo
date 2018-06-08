using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using Icon.Web.Common;
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
    public class SupportController : Controller
    {
        private ILogger logger;
        private IQueryHandler<GetHierarchyParameters, List<Hierarchy>> hierarchyQuery;
        private IQueryHandler<GetHierarchyClassByIdParameters, HierarchyClass> hierarchyClassQuery;
        private IManagerHandler<DeleteHierarchyClassManager> deleteClassManagerHandler;

        public SupportController(ILogger logger,
            IQueryHandler<GetHierarchyParameters, List<Hierarchy>> hierarchyQuery,
            IQueryHandler<GetHierarchyClassByIdParameters, HierarchyClass> hierarchyClassQuery,
            IManagerHandler<DeleteHierarchyClassManager> deleteHierarchyClass)
        {
            this.logger = logger;
            this.hierarchyQuery = hierarchyQuery;
            this.hierarchyClassQuery = hierarchyClassQuery;
            this.deleteClassManagerHandler = deleteHierarchyClass;
        }

        //
        // GET: /Support/
        [SupportAccessAuthorize]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [SupportAccessAuthorize]
        public ActionResult ChooseHierarchyClass()
        {
            return View();
        }

        [HttpGet]
        [SupportAccessAuthorize]
        public ActionResult DeleteHierarchyClass(int? hierarchyClassId)
        {
            //validate parameter for positive value
            if (hierarchyClassId.GetValueOrDefault(0) < 1)
            {
                var invalidHierarchyMsg = "Error: invalid Hierarchy Class ID parameter submitted (" +
                    (hierarchyClassId.HasValue ? hierarchyClassId.Value.ToString() : "null") + ")";
                TempData["ErrorMessage"] = invalidHierarchyMsg;
                logger.Warn(invalidHierarchyMsg);
                return RedirectToAction("ChooseHierarchyClass");
            }

            //query for hierarchyClass object using parameter
            var hierarchyClassParameters = new GetHierarchyClassByIdParameters { HierarchyClassId = hierarchyClassId.Value };
            var hierarchyClass = hierarchyClassQuery.Search(hierarchyClassParameters);

            // verify that the query found good data
            if (hierarchyClass?.hierarchyClassID == null )
            {
                var unknownHierarchyMsg = "Error: The requested Hierarchy Class ID could not be found (ID: " +
                    hierarchyClassId.Value.ToString() + "). " +
                    "Be sure to use the internal Icon database ID, not the infor class code";
                TempData["ErrorMessage"] = unknownHierarchyMsg;
                logger.Warn(unknownHierarchyMsg);
                return RedirectToAction("ChooseHierarchyClass");
            }

            // transmute data object into view model
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

            // populate subteams for selected hierarchy class 
            if (!String.IsNullOrEmpty(viewModel.SubTeam))
            {
                var financialHierarchy = hierarchyQuery
                    .Search(new GetHierarchyParameters { HierarchyName = HierarchyNames.Financial })
                    .Single();
                var subTeamHierarchyClassCollection = financialHierarchy.HierarchyClass;
                var selectedSubteamHierarchyClass = subTeamHierarchyClassCollection
                    .FirstOrDefault(hc => hc.hierarchyClassName == viewModel.SubTeam);

                viewModel.SelectedSubTeam = selectedSubteamHierarchyClass.hierarchyClassID;
                viewModel.SubTeamList = CreateSubteamSelectList(subTeamHierarchyClassCollection);
            }

            return View(viewModel);
        }

        // POST: /HierarchyClass/Delete/
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SupportAccessAuthorize]
        public ActionResult DeleteHierarchyClass(HierarchyClassViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var hierarchyClassParameters = new GetHierarchyClassByIdParameters { HierarchyClassId = viewModel.HierarchyClassId };
            HierarchyClass deletedHierarchyClass = hierarchyClassQuery.Search(hierarchyClassParameters);

            // Make sure the node is not attached to any items.
            if (deletedHierarchyClass.ItemHierarchyClass.Count > 0)
            {
                var linkedHierarchyMsg = "Error: This hierarchy class is linked to items, so it cannot be deleted. (" +
                    deletedHierarchyClass.hierarchyClassName + ")";
                TempData["ErrorMessage"] = linkedHierarchyMsg;
                logger.Warn(linkedHierarchyMsg);
                return View(viewModel);
            }

            // Make sure the node does not have any children.
            if (deletedHierarchyClass.HierarchyClass1.Count > 0)
            {
                var hierarchyWithSubclassesMsg = "Error: This hierarchy class contains subclasses, so it cannot be deleted. (" +
                    deletedHierarchyClass.hierarchyClassName + ")";
                TempData["ErrorMessage"] = hierarchyWithSubclassesMsg;
                logger.Warn(hierarchyWithSubclassesMsg);
                return View(viewModel);
            }

            var cmdManagerData = new DeleteHierarchyClassManager
            {
                DeletedHierarchyClass = deletedHierarchyClass
            };

            try
            {
                this.deleteClassManagerHandler.Execute(cmdManagerData);
            }
            catch (CommandException exception)
            {
                var exceptionLogger = new ExceptionLogger(logger);
                exceptionLogger.LogException(exception, this.GetType(), MethodBase.GetCurrentMethod());
                TempData["ErrorMessage"] = "Error: There was a problem with applying this delete on the database.";
                return View(viewModel);
            }

            return RedirectToAction(actionName: "ChooseHierarchyClass");
        }

        private IEnumerable<SelectListItem> CreateSubteamSelectList(IEnumerable<HierarchyClass> hierarchyClasses)
        {
            var subTeams = hierarchyClasses
                .Select(hc => new SelectListItem
                {
                    Value = hc.hierarchyClassID.ToString(),
                    Text = hc.hierarchyClassName
                })
                .ToList()
                .OrderBy(st => st.Text);
            return subTeams;
        }
    }
}