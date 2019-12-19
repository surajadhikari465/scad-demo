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
    public class FailedItemLocaleMessageController : Controller
    {
        private IQueryHandler<GetFailedItemLocaleMessageParameters, List<MessageQueueItemLocale>> getFailedItemLocaleMessageQueryHandler;
        private ICommandHandler<ReprocessFailedItemLocaleMessageCommand> reprocessFailedItemLocaleMessageCommandHandler;

        public FailedItemLocaleMessageController(IQueryHandler<GetFailedItemLocaleMessageParameters, List<MessageQueueItemLocale>> getFailedItemLocaleMessageQueryHandler,
            ICommandHandler<ReprocessFailedItemLocaleMessageCommand> reprocessFailedItemLocaleMessageCommandHandler)
        {
            this.getFailedItemLocaleMessageQueryHandler = getFailedItemLocaleMessageQueryHandler;
            this.reprocessFailedItemLocaleMessageCommandHandler = reprocessFailedItemLocaleMessageCommandHandler;
        }

        // GET: FailedItemLocaleMessage
        public ActionResult Index()
        {
            var failedItemLocaleMessageViewModels = getFailedItemLocaleMessageQueryHandler.Search(new GetFailedItemLocaleMessageParameters())
                .Select(il => new FailedItemLocaleMessageViewModel
                {
                    Id = il.MessageQueueId,
                    ScanCode = il.ScanCode,
                    BusinessUnit_ID = il.BusinessUnit_ID,
                    RegionCode = il.RegionCode,
                    ProcessedDate = il.ProcessedDate,
                    InsertDate = il.InsertDate
                });
            return View(failedItemLocaleMessageViewModels);
        }

        public ActionResult Reprocess(List<FailedItemLocaleMessageViewModel> viewModels)
        {
            if (viewModels == null || viewModels.Count == 0 || viewModels.Any(vm => vm == null))
            {
                return Json(new { Success = false, Error = "Must select rows to reprocess." });
            }

            try
            {
                reprocessFailedItemLocaleMessageCommandHandler.Execute(new ReprocessFailedItemLocaleMessageCommand
                {
                    MessageQueueIds = viewModels.Select(vm => vm.Id).ToList()
                });
            }
            catch (Exception e)
            {
                return Json(new { Success = false, Error = String.Format("Error occurred while attempting to reprocess failed item locale messages. Error details: {0}", e) });
            }

            return Json(new { Success = true, ReprocessedData = viewModels });

            
        }
    }
}