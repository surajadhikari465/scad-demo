using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Icon.Web.Mvc.Controllers
{
    [Authorize(Roles = "WFM\\IRMA.Developers")]
    public class FailedProductMessageController : Controller
    {
        private IQueryHandler<GetFailedProductMessageParameters, List<MessageQueueProduct>> getFailedProductMessageQueryHandler;
        private ICommandHandler<ReprocessFailedProductMessageCommand> reprocessFailedProductMessageCommandHandler;

        public FailedProductMessageController(IQueryHandler<GetFailedProductMessageParameters, List<MessageQueueProduct>> getFailedProductMessageQueryHandler,
            ICommandHandler<ReprocessFailedProductMessageCommand> reprocessFailedProductMessageCommandHandler)
        {
            this.getFailedProductMessageQueryHandler = getFailedProductMessageQueryHandler;
            this.reprocessFailedProductMessageCommandHandler = reprocessFailedProductMessageCommandHandler;
        }

        // GET: FailedProductMessage
        public ActionResult Index()
        {
            var failedProductMessageViewModels = getFailedProductMessageQueryHandler.Search(new GetFailedProductMessageParameters())
                .Select(ip => new FailedProductMessageViewModel
                {
                    Id = ip.MessageQueueId,
                    ScanCode = ip.ScanCode,
                    InsertDate = ip.InsertDate,
                    ProcessedDate = ip.ProcessedDate
                });
            return View(failedProductMessageViewModels);
        }


        public ActionResult Reprocess(List<FailedProductMessageViewModel> viewModels)
        {
            if (viewModels == null || viewModels.Count == 0 || viewModels.Any(vm => vm == null))
            {
                return Json(new { Success = false, Error = "Must select rows to reprocess." });
            }

            try
            {
                reprocessFailedProductMessageCommandHandler.Execute(new ReprocessFailedProductMessageCommand
                {
                    MessageQueueIds = viewModels.Select(vm => vm.Id).ToList()
                });
            }
            catch (Exception e)
            {
                return Json(new { Success = false, Error = String.Format("Error occurred while attempting to reprocess failed product messages. Error details: {0}", e) });
            }

            return Json(new { Success = true, ReprocessedData = viewModels });
        }
    }
}