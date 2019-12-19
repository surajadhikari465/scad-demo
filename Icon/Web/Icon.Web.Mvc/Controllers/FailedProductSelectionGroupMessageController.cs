using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Attributes;
using Icon.Web.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;


namespace Icon.Web.Mvc.Controllers
{
    [AdminAccessAuthorizeAttribute]
    public class FailedProductSelectionGroupMessageController : Controller
    {
        private IQueryHandler<GetFailedProductSelectionGroupMessageParameters, List<MessageQueueProductSelectionGroup>> getFailedProductSelectionGroupMessageQueryHandler;
        private ICommandHandler<ReprocessFailedProductSelectionGroupMessageCommand> reprocessFailedProductSelectionGroupMessageCommandHandler;

         public FailedProductSelectionGroupMessageController(IQueryHandler<GetFailedProductSelectionGroupMessageParameters, List<MessageQueueProductSelectionGroup>> getFailedProductSelectionGroupMessageQueryHandler,
            ICommandHandler<ReprocessFailedProductSelectionGroupMessageCommand> reprocessFailedProductSelectionGroupMessageCommandHandler)
        {
            this.getFailedProductSelectionGroupMessageQueryHandler = getFailedProductSelectionGroupMessageQueryHandler;
            this.reprocessFailedProductSelectionGroupMessageCommandHandler = reprocessFailedProductSelectionGroupMessageCommandHandler;
        }

        // GET: FailedProductSelectionGroupMessage
        public ActionResult Index()
        {
            var failedProductSelectionGroupMessageViewModels = getFailedProductSelectionGroupMessageQueryHandler.Search(new GetFailedProductSelectionGroupMessageParameters())
                .Select(ip => new FailedProductSelectionGroupMessageViewModel
                {
                    Id = ip.MessageQueueId,
                    ProductSelectionGroupName = ip.ProductSelectionGroupName,
                    ProductSelectionGroupTypeName = ip.ProductSelectionGroupTypeName,
                    InsertDate = ip.InsertDate,
                    ProcessedDate = ip.ProcessedDate
                });
            return View(failedProductSelectionGroupMessageViewModels);
        }

        public ActionResult Reprocess(List<FailedProductSelectionGroupMessageViewModel> viewModels)
        {
            if (viewModels == null || viewModels.Count == 0 || viewModels.Any(vm => vm == null))
            {
                return Json(new { Success = false, Error = "Must select rows to reprocess." });
            }

            try
            {
                reprocessFailedProductSelectionGroupMessageCommandHandler.Execute(new ReprocessFailedProductSelectionGroupMessageCommand
                {
                    MessageQueueIds = viewModels.Select(vm => vm.Id).ToList()
                });
            }
            catch (Exception e)
            {
                return Json(new { Success = false, Error = String.Format("Error occurred while attempting to reprocess failed product selection group messages. Error details: {0}", e) });
            }

            return Json(new { Success = true, ReprocessedData = viewModels });
        }
    }
}