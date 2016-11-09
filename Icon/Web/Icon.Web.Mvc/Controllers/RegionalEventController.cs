using AutoMapper;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.RegionalItemCatalogs;
using Icon.Web.Mvc.Models;
using Irma.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Icon.Common.DataAccess;

namespace Icon.Web.Mvc.Controllers
{
    [Authorize(Roles = "WFM\\IRMA.Developers")]
    public class RegionalEventController : Controller
    {
        private IRegionalItemCatalogFactory regionalItemCatalogFactory;
        private IQueryHandler<GetFailedIconItemChangesParameters, List<IconItemChangeQueue>> getFailedIconItemChangesQueryHandler;
        private ICommandHandler<ReprocessIconItemChangesCommand> reprocessFailedIconItemChangesCommandHandler;

        public RegionalEventController(IRegionalItemCatalogFactory regionalItemCatalogFactory,
            IQueryHandler<GetFailedIconItemChangesParameters, List<IconItemChangeQueue>> getFailedIconItemChangesQueryHandler,
            ICommandHandler<ReprocessIconItemChangesCommand> reprocessFailedIconItemChangesCommandHandler)
        {
            this.regionalItemCatalogFactory = regionalItemCatalogFactory;
            this.getFailedIconItemChangesQueryHandler = getFailedIconItemChangesQueryHandler;
            this.reprocessFailedIconItemChangesCommandHandler = reprocessFailedIconItemChangesCommandHandler;
        }

        public ActionResult Index()
        {
            List<RegionalItemCatalog> regionalItemCatalogs = regionalItemCatalogFactory.GetRegionalItemCatalogs();

            List<FailedRegionalEventViewModel> failedRegionalItemUpdateViewModels = new List<FailedRegionalEventViewModel>();

            foreach (var regionalItemCatalog in regionalItemCatalogs)
            {
                List<IconItemChangeQueue> iconItemChangeQueue = getFailedIconItemChangesQueryHandler.Search(new GetFailedIconItemChangesParameters
                    {
                        RegionConnectionStringName = regionalItemCatalog.ConnectionStringName
                    });
                List<FailedRegionalEventViewModel> itemUpdates = iconItemChangeQueue
                        .Select(i => Mapper.Map<FailedRegionalEventViewModel>(i))
                        .ToList();
                itemUpdates.ForEach(vm => vm.RegionAbbreviation = regionalItemCatalog.Abbreviation);
                failedRegionalItemUpdateViewModels.AddRange(itemUpdates);
            }

            return View(failedRegionalItemUpdateViewModels);
        }

        [HttpPost]
        public JsonResult Reprocess(List<FailedRegionalEventViewModel> viewModels)
        {
            if (viewModels == null || viewModels.Count == 0 || viewModels.Any(vm => vm == null))
            {
                return Json(new { Success = false, Error = "Must select rows to reprocess." });
            }

            try
            {
                List<RegionalItemCatalog> regionalItemCatalogs = regionalItemCatalogFactory.GetRegionalItemCatalogs();

                foreach (var regionGroup in viewModels.GroupBy(vm => vm.RegionAbbreviation))
                {
                    reprocessFailedIconItemChangesCommandHandler.Execute(new ReprocessIconItemChangesCommand
                    {
                        IconItemChangeIds = regionGroup.Select(vm => vm.Id).ToList(),
                        RegionalConnectionStringName = regionalItemCatalogs.First(ric => ric.Abbreviation == regionGroup.Key).ConnectionStringName
                    });
                }
            }
            catch (Exception e)
            {
                return Json(new { Success = false, Error = String.Format("Error occurred while attempting to reprocess failed regional events. Error details: {0}", e) });
            }

            return Json(new { Success = true, ReprocessedData = viewModels });
        }
    }
}