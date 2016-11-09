using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Models;
using Icon.Web.Mvc.RegionalItemCatalogs;
using Irma.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Icon.Web.Mvc.Controllers
{
    [Authorize(Roles = "WFM\\IRMA.Developers")]
    public class RegionalPosPushController : Controller
    {
        private IRegionalItemCatalogFactory regionalItemCatalogFactory;
        private IQueryHandler<GetFailedIconPosPushPublishesParameters, List<IConPOSPushPublish>> getFailedIconPosPushPublishesQueryHandler;
        private ICommandHandler<ReprocessFailedIconPosPushPublishCommand> reprocessFailedIconPosPushPublishCommandHandler;
        public RegionalPosPushController(IRegionalItemCatalogFactory regionalItemCatalogFactory,
            IQueryHandler<GetFailedIconPosPushPublishesParameters, List<IConPOSPushPublish>> getFailedIconPosPushPublishesQueryHandler,
            ICommandHandler<ReprocessFailedIconPosPushPublishCommand> reprocessFailedIconPosPushPublishCommandHandler)
        {
            this.regionalItemCatalogFactory = regionalItemCatalogFactory;
            this.getFailedIconPosPushPublishesQueryHandler = getFailedIconPosPushPublishesQueryHandler;
            this.reprocessFailedIconPosPushPublishCommandHandler = reprocessFailedIconPosPushPublishCommandHandler;
        }

        public ActionResult Index()
        {
            var regionalItemCatalogs = regionalItemCatalogFactory.GetRegionalItemCatalogs();
            List<FailedRegionalPosPushViewModel> regionalPosPushViewModels = new List<FailedRegionalPosPushViewModel>();

            foreach (var regionalItemCatalog in regionalItemCatalogs)
            {
                regionalPosPushViewModels.AddRange(getFailedIconPosPushPublishesQueryHandler.Search(new GetFailedIconPosPushPublishesParameters
                    {
                        RegionConnectionStringName = regionalItemCatalog.ConnectionStringName
                    })
                    .Select(p => new FailedRegionalPosPushViewModel
                    {
                        RegionAbbreviation = regionalItemCatalog.Abbreviation,
                        Id = p.IConPOSPushPublishID,
                        ScanCode = p.Identifier,
                        BusinessUnitId = p.BusinessUnit_ID,
                        ChangeType = p.ChangeType,
                        ProcessFailedDate = p.ProcessingFailedDate.Value
                    })
                );
            }

            return View(regionalPosPushViewModels);
        }

        public ActionResult Reprocess(List<FailedRegionalPosPushViewModel> viewModels)
        {
            if (viewModels == null || viewModels.Count == 0 || viewModels.Any(vm => vm == null))
            {
                return Json(new { Success = false, Error = "Must select rows to reprocess." });
            }

            try
            {
                List<RegionalItemCatalog> regionalItemCatalogs = regionalItemCatalogFactory.GetRegionalItemCatalogs();

                foreach (var regionGroup in viewModels.GroupBy(p => p.RegionAbbreviation))
                {
                    reprocessFailedIconPosPushPublishCommandHandler.Execute(new ReprocessFailedIconPosPushPublishCommand
                    {
                        IconPosPushPublishIds = regionGroup.Select(p => p.Id).ToList(),
                        RegionalConnectionStringName = regionalItemCatalogs.First(r => r.Abbreviation == regionGroup.Key).ConnectionStringName
                    });
                }
            }
            catch (Exception e)
            {
                return Json(new { Success = false, Error = String.Format("Error occurred while attempting to reprocess regional POS pushes. Error details: {0}" + e) });
            }

            return Json(new { Success = true, ReprocessedData = viewModels });
        }
    }
}