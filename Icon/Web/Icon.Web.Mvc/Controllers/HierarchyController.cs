using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Extensions;
using Icon.Web.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Icon.Web.Controllers
{
	public class HierarchyController : Controller
	{
		private ILogger logger;
		private IQueryHandler<GetHierarchyParameters, List<Hierarchy>> getHierarchyQuery;
		
		public HierarchyController(
            ILogger logger, 
            IQueryHandler<GetHierarchyParameters, 
            List<Hierarchy>> getHierarchyQuery)
		{
			this.logger = logger;
			this.getHierarchyQuery = getHierarchyQuery;
		}

		// GET: /Hierarchy/
		public ActionResult Index(HierarchySearchViewModel viewModel)
		{
			int selectedHierarchyId = viewModel.SelectedHierarchyId;
			HierarchySearchViewModel hierarchySearchViewModel = new HierarchySearchViewModel();

			hierarchySearchViewModel.Hierarchies = PopulateHierarchySelectList();
			hierarchySearchViewModel.SelectedHierarchyId = selectedHierarchyId;

			return View(hierarchySearchViewModel);
		}

		// GET: /Hierarchy/Search
		[OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
		public PartialViewResult Search(HierarchySearchViewModel viewModel)
		{
			// Set up a search object and execute the query.
			GetHierarchyParameters searchHierarchyParameters = new GetHierarchyParameters
			{
				HierarchyId = viewModel.SelectedHierarchyId,
				IncludeNavigation = true
			};

			viewModel.Hierarchy = getHierarchyQuery.Search(searchHierarchyParameters).Single();

			var hierarchyClasses = viewModel.Hierarchy.HierarchyClass
				.Select(hc => new HierarchyClassGridViewModel
				    {
					    HierarchyClassId = hc.hierarchyClassID,
					    HierarchyClassName = hc.hierarchyClassName,
					    HierarchyParentClassId = hc.hierarchyParentClassID,
					    TaxAbbreviation = HierarchyClassAccessor.GetTaxAbbreviation(hc),
                        TaxRomance = HierarchyClassAccessor.GetTaxRomance(hc),
					    GlAccount = HierarchyClassAccessor.GetGlAccount(hc),
					    SubTeam = HierarchyClassAccessor.GetSubTeam(hc),
                        NonMerchandiseTrait = HierarchyClassAccessor.GetNonMerchandiseTrait(hc),
                        SubBrickCode = HierarchyClassAccessor.GetSubBrickCode(hc),
					    AddNodeLink = String.Format("<a href=\"../HierarchyClass/Create?parentId={0}&hierarchyId={1}\">Add Child</a>", hc.hierarchyClassID, hc.hierarchyID),
					    EditNodeLink = String.Format("<a href=\"../HierarchyClass/Edit?hierarchyClassId={0}\">Edit</a>",
						    hc.hierarchyClassID),
					    DeleteNodeLink = String.Format("<a href=\"../HierarchyClass/Delete?hierarchyClassId={0}\">Delete</a>",
						    hc.hierarchyClassID)
				    })
                .OrderBy(hc => hc.HierarchyClassName)
                .ToList();

			viewModel.HierarchyClasses = BuildHierarchyClassModelRecursive(hierarchyClasses, null).AsQueryable();

			return PartialView("_HierarchyViewPartial", viewModel);
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
					GlAccount = hc.GlAccount,
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

            var dropDownList = hierarchies.Where(h => h.hierarchyID != Hierarchies.Financial && h.hierarchyID != Hierarchies.Brands && h.hierarchyID != Hierarchies.National && h.hierarchyID != Hierarchies.CertificationAgencyManagement)
				.Select(h => new DropDownViewModel
				{
					Id = h.hierarchyID,
					Name = h.hierarchyName
				});

			return dropDownList.ToSelectListItem();
		}
	}
}