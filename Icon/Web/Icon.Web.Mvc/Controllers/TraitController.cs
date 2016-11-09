using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Models;
using MoreLinq;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Icon.Web.Controllers
{
    public class TraitController : Controller
    {
        private ILogger logger;
        private IQueryHandler<GetTraitGroupParameters, List<TraitGroup>> traitQuery;

        public TraitController(
            ILogger logger,
            IQueryHandler<GetTraitGroupParameters,
            List<TraitGroup>> traitQuery)
        {
            this.logger = logger;
            this.traitQuery = traitQuery;
        }


        // GET: /Trait/
        public ActionResult Index(TraitGridViewModel viewModel)
        {
            // Default to first Trait Group when page is first loaded
            if (viewModel.TraitGroupSelectedId == 0)
            {
                viewModel.TraitGroupSelectedId = 1;
            }

            // Get Trait Groups from Database for Drop Down List
            var traitGroups = traitQuery.Search(new GetTraitGroupParameters());

            // Set SelectList in View Model from list of TraitGroups
            viewModel.TraitGroup = traitGroups
                .DistinctBy(tg => tg.traitGroupDesc)
                .Select(d => new SelectListItem
                    {
                        Value = d.traitGroupID.ToString(),
                        Text = d.traitGroupDesc
                    });

            // Set Traits in ViewModel list based on traitGroupID
            viewModel.Traits = traitGroups
                .Where(tg => tg.traitGroupID == viewModel.TraitGroupSelectedId)
                .SelectMany(tg => tg.Trait)
                .Select(t => new TraitViewModel
                {
                    TraitId = t.traitID,
                    TraitDesc = t.traitDesc,
                    TraitPattern = t.traitPattern,
                    TraitGroupId = t.TraitGroup.traitGroupID,
                    TraitGroupDescription = t.TraitGroup.traitGroupDesc
                })
                .ToList();

            return View(viewModel);
        }
    }
}