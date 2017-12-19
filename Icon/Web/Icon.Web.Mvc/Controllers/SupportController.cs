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
    [Authorize(Roles = "WFM\\IRMA.Applications, WFM\\IRMA.Developers")]
    public class SupportController : Controller
    {
        private ILogger logger;
        private IQueryHandler<GetHierarchyParameters, List<Hierarchy>> hierarchyQuery;
        private IQueryHandler<GetHierarchyClassByIdParameters, HierarchyClass> hierarchyClassQuery;
        private IManagerHandler<DeleteHierarchyClassManager> deleteHierarchyClass;

        public SupportController(ILogger logger,
            IQueryHandler<GetHierarchyParameters, List<Hierarchy>> hierarchyQuery,
            IQueryHandler<GetHierarchyClassByIdParameters, HierarchyClass> hierarchyClassQuery,
            IManagerHandler<DeleteHierarchyClassManager> deleteHierarchyClass)
        {
            this.logger = logger;
            this.hierarchyQuery = hierarchyQuery;
            this.hierarchyClassQuery = hierarchyClassQuery;
            this.deleteHierarchyClass = deleteHierarchyClass;
        }

        //
        // GET: /Support/
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ChooseHierarchyClass()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DeleteHierarchyClass(int? hierarchyClassId)
        {
            if (hierarchyClassId.GetValueOrDefault(0) < 1)
            {
                return RedirectToAction("ChooseHierarchyClass");
            }

            GetHierarchyClassByIdParameters hierarchyClassParameters = 
                new GetHierarchyClassByIdParameters { HierarchyClassId = hierarchyClassId.Value };
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
        public ActionResult DeleteHierarchyClass(HierarchyClassViewModel viewModel)
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