using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using Icon.Web.Common;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Managers;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Models;
using Infragistics.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Icon.Web.Controllers
{
    public class FinancialController : Controller
    {
        private IQueryHandler<GetHierarchyParameters, List<Hierarchy>> hierarchyQuery;

        public FinancialController(IQueryHandler<GetHierarchyParameters, List<Hierarchy>> hierarchyQuery)
        {
            this.hierarchyQuery = hierarchyQuery;
        }

        //
        // GET: /Financial/
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Index()
        {
            // Get Financial Hierarchy data from Database
            Hierarchy financialHierarchy = hierarchyQuery.Search(new GetHierarchyParameters { HierarchyName = "Financial", IncludeNavigation = true }).Single();

            // Populate ViewModel
            FinancialGridViewModel viewModel = new FinancialGridViewModel();
            viewModel.SubTeams = financialHierarchy.HierarchyClass
                .Select(hc => new FinancialViewModel
                {
                    HierarchyId = hc.hierarchyID,
                    HierarchyName = hc.Hierarchy.hierarchyName,
                    HierarchyClassId = hc.hierarchyClassID,
                    SubTeamName = hc.hierarchyClassName.Split('(')[0].Trim(),
                    PeopleSoftNumber = hc.hierarchyClassName.Split('(')[1].Trim(')'),
                    PosDeptNumber = hc.HierarchyClassTrait.Where(hct => hct.Trait.traitCode == TraitCodes.PosDepartmentNumber).Select(hct => hct.traitValue).SingleOrDefault(),
                    TeamNumber = hc.HierarchyClassTrait.Where(hct => hct.Trait.traitCode == TraitCodes.TeamNumber).Select(hct => hct.traitValue).SingleOrDefault(),
                    TeamName = hc.HierarchyClassTrait.Where(hct => hct.Trait.traitCode == TraitCodes.TeamName).Select(hct => hct.traitValue).SingleOrDefault(),
                    NonAlignedSubteam = hc.HierarchyClassTrait.Any(hct => hct.Trait.traitCode == TraitCodes.NonAlignedSubteam)
                })
                .OrderBy(hc => hc.SubTeamName);

            return View(viewModel);
        }
    }
}
