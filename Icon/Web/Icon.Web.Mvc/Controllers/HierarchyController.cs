using DevTrends.MvcDonutCaching;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using Icon.Web.Common;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Extensions;
using Icon.Web.Mvc.Models;
using Icon.Web.Mvc.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Icon.Web.Controllers
{
    public class HierarchyController : Controller
    {
        private ILogger logger;
        private IconWebAppSettings settings;
        private IQueryHandler<GetHierarchyParameters, List<Hierarchy>> getHierarchyQuery;
        private IQueryHandler<GetHierarchiesParameters, IEnumerable<HierarchyModel>> getHierarchiesQueryHandler;

        public HierarchyController(
            ILogger logger,
            IconWebAppSettings settings,
            IQueryHandler<GetHierarchyParameters, List<Hierarchy>> getHierarchyQuery,
            IQueryHandler<GetHierarchiesParameters, IEnumerable<HierarchyModel>> getHierarchiesQueryHandler)
        {
            this.settings = settings;
            this.logger = logger;
            this.getHierarchyQuery = getHierarchyQuery;
            this.getHierarchiesQueryHandler = getHierarchiesQueryHandler;
        }

        // GET: /Hierarchy/
        public ActionResult Index(HierarchySearchViewModel viewModel)
        {
            int selectedHierarchyId = viewModel.SelectedHierarchyId;

            //These two Hierarchies should no longer be accessible by ANYONE
            switch(selectedHierarchyId)
            {
                case Hierarchies.Browsing:
                case Hierarchies.CertificationAgencyManagement:
                    return View("AccessDenied");
            }

            HierarchySearchViewModel hierarchySearchViewModel = new HierarchySearchViewModel();

            hierarchySearchViewModel.Hierarchies = PopulateHierarchySelectList();
            hierarchySearchViewModel.SelectedHierarchyId = selectedHierarchyId;
            hierarchySearchViewModel.UserWriteAccess = this.GetWriteAccess();
            return View(hierarchySearchViewModel);
        }

        // GET: /Hierarchy/Search
        [DonutOutputCache(Duration=1, VaryByParam = "*")]
        public PartialViewResult Search(HierarchySearchViewModel viewModel)
        {
            try
            {
                // Set up a search object and execute the query.
                GetHierarchyParameters searchHierarchyParameters = new GetHierarchyParameters
                {
                    HierarchyId = viewModel.SelectedHierarchyId,
                    IncludeNavigation = true
                };

                viewModel.Hierarchy = getHierarchyQuery.Search(searchHierarchyParameters).Single();

                var financialHierarchyClasses = getHierarchyQuery.Search(new GetHierarchyParameters
                {
                    HierarchyId = Hierarchies.Financial
                }).Single();

                var hierarchyClasses = viewModel.Hierarchy.HierarchyClass
                    .Select(hc => new HierarchyClassGridViewModel
                    {
                        HierarchyClassId = hc.hierarchyClassID,
                        HierarchyClassName = hc.hierarchyClassName,
                        HierarchyParentClassId = hc.hierarchyParentClassID,
                        TaxAbbreviation = HierarchyClassAccessor.GetTaxAbbreviation(hc),
                        TaxRomance = HierarchyClassAccessor.GetTaxRomance(hc),
                        SubTeam = HierarchyClassAccessor.MapSubTeamTraitIdToValue(hc, financialHierarchyClasses.HierarchyClass.ToList()),
                        NonMerchandiseTrait = HierarchyClassAccessor.GetNonMerchandiseTrait(hc),
                        SubBrickCode = HierarchyClassAccessor.GetSubBrickCode(hc),
                        AddNodeLink = $"<a href=\"../HierarchyClass/Create?parentId={hc.hierarchyClassID}&hierarchyId={hc.hierarchyID}\">Add Child</a>",
                        EditNodeLink = $"<a href=\"../HierarchyClass/Edit?hierarchyClassId={hc.hierarchyClassID}\">Edit</a>",
                        DeleteNodeLink = $"<a href=\"../HierarchyClass/Delete?hierarchyClassId={hc.hierarchyClassID}\">Delete</a>"
                    })
                    .OrderBy(hc => hc.HierarchyClassName)
                    .ToList();

                viewModel.HierarchyClasses = BuildHierarchyClassModelRecursive(hierarchyClasses, null).AsQueryable();
                viewModel.UserWriteAccess = this.GetWriteAccess();
                return PartialView("_HierarchyViewPartial", viewModel);
            }

            catch (Exception ex)
            {
                var exceptionLogger = new ExceptionLogger(logger);
                exceptionLogger.LogException(ex, this.GetType(), MethodBase.GetCurrentMethod());
                List<string> allErrors = new List<string>() { "Error in getting data." };
                ViewData["ErrorMessages"] = allErrors;
                return PartialView("_HierarchyViewPartial", viewModel);
            }
        }

        private List<HierarchyClassGridViewModel> BuildHierarchyClassModelRecursive(List<HierarchyClassGridViewModel> allHierarchyClasses, HierarchyClassGridViewModel parentHierarchyClass)
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

            List<HierarchyClassGridViewModel> hierarchyClasses = new List<HierarchyClassGridViewModel>();

            foreach (var hc in childHierarchyClasses)
            {
                hierarchyClasses.Add(new HierarchyClassGridViewModel()
                {
                    HierarchyClassId = hc.HierarchyClassId,
                    HierarchyParentClassId = hc.HierarchyParentClassId,
                    HierarchyClassName = hc.HierarchyClassName,
                    TaxAbbreviation = hc.TaxAbbreviation,
                    SubTeam = hc.SubTeam,
                    NonMerchandiseTrait = hc.NonMerchandiseTrait,
                    SubBrickCode = hc.SubBrickCode,
                    AddNodeLink = hc.AddNodeLink,
                    EditNodeLink = hc.EditNodeLink,
                    DeleteNodeLink = hc.DeleteNodeLink,
                    HierarchySubClasses = BuildHierarchyClassModelRecursive(allHierarchyClasses, hc)
                });
            }

            return hierarchyClasses;
        }

        private IEnumerable<SelectListItem> PopulateHierarchySelectList()
        {
            var hierarchies = getHierarchyQuery.Search(new GetHierarchyParameters { IncludeNavigation = false });

            var dropDownList = hierarchies
                .Where(h => h.hierarchyID != Hierarchies.Financial
                    && h.hierarchyID != Hierarchies.Brands
                    && h.hierarchyID != Hierarchies.National
                    && h.hierarchyID != Hierarchies.CertificationAgencyManagement
                    && h.hierarchyID != Hierarchies.Browsing)
                .Select(h => new DropDownViewModel
                {
                    Id = h.hierarchyID,
                    Name = h.hierarchyName
                });

            return dropDownList.ToSelectListItem();
        }

        [HttpGet]
        [DonutOutputCache(Duration = 3600, Location = System.Web.UI.OutputCacheLocation.Server, VaryByParam = "")]
        public ActionResult All()
        {
            var hierarchies = getHierarchiesQueryHandler.Search(new GetHierarchiesParameters())
                .Select(h => new HierarchyViewModel(h));
            return Json(hierarchies, JsonRequestBehavior.AllowGet);
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