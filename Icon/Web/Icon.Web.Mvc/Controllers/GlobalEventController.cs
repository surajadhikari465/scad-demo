using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Models;
using Icon.Web.Mvc.RegionalItemCatalogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Icon.Web.Mvc.Controllers
{
    [Authorize(Roles = "WFM\\IRMA.Developers")]
    public class GlobalEventController : Controller
    {
        private IQueryHandler<GetFailedEventsParameters, List<EventQueue>> getFailedEventsQueryHandler;
        private ICommandHandler<ReprocessFailedEventsCommand> reprocessFailedEventsCommandHandler;

        public GlobalEventController(IQueryHandler<GetFailedEventsParameters, List<EventQueue>> getFailedEventsQueryHandler,
            ICommandHandler<ReprocessFailedEventsCommand> reprocessFailedEventsCommandHandler)
        {
            this.getFailedEventsQueryHandler = getFailedEventsQueryHandler;
            this.reprocessFailedEventsCommandHandler = reprocessFailedEventsCommandHandler;
        }

        public ActionResult Index()
        {
            var failedEvents = getFailedEventsQueryHandler.Search(new GetFailedEventsParameters())
                .Select(e => new FailedGlobalEventViewModel
                {
                    Id = e.QueueId,
                    EventMessage = e.EventMessage,
                    EventType = e.EventType.EventName,
                    RegionAbbreviation = e.RegionCode,
                    ProcessFailedDate = e.ProcessFailedDate.Value
                });

            return View(failedEvents);
        }

        public ActionResult Reprocess(List<FailedGlobalEventViewModel> viewModels)
        {
            if(viewModels == null || viewModels.Count == 0 || viewModels.Any(vm => vm == null))
            {
                return Json(new { Success = false, Error = "Must select rows to reprocess." });
            }

            try
            {
                reprocessFailedEventsCommandHandler.Execute(new ReprocessFailedEventsCommand
                {
                    EventQueueIds = viewModels.Select(vm => vm.Id).ToList()
                });
            }
            catch (Exception e)
            {
                return Json(new { Success = false, Error = String.Format("Error occurred while attempting to reprocess global events. Error details: {0}", e) });
            }

            return Json(new { Success = true, ReprocessedData = viewModels });
        }
    }
}