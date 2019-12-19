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
    public class FailedLocaleMessageController : Controller
    {

       private IQueryHandler<GetFailedLocaleMessageParameters, List<MessageQueueLocale>> getFailedLocaleMessageQueryHandler;
        private ICommandHandler<ReprocessFailedLocaleMessageCommand> reprocessFailedLocaleMessageCommandHandler;

        public FailedLocaleMessageController(IQueryHandler<GetFailedLocaleMessageParameters, List<MessageQueueLocale>> getFailedLocaleMessageQueryHandler,
            ICommandHandler<ReprocessFailedLocaleMessageCommand> reprocessFailedLocaleMessageCommandHandler)
        {
            this.getFailedLocaleMessageQueryHandler = getFailedLocaleMessageQueryHandler;
            this.reprocessFailedLocaleMessageCommandHandler = reprocessFailedLocaleMessageCommandHandler;
        }

        // GET: FailedLocaleMessage
        public ActionResult Index()
        {
            var failedLocaleMessageViewModels = getFailedLocaleMessageQueryHandler.Search(new GetFailedLocaleMessageParameters())
                .Select(il => new FailedLocaleMessageViewModel
                {
                    Id = il.MessageQueueId,
                    StoreAbbreviation = il.StoreAbbreviation,
                    LocaleName = il.LocaleName,
                    TerritoryCode = il.TerritoryCode,
                    ProcessedDate = il.ProcessedDate,
                    InsertDate = il.InsertDate
                });
            return View(failedLocaleMessageViewModels);
        }

        public ActionResult Reprocess(List<FailedItemLocaleMessageViewModel> viewModels)
        {
            if (viewModels == null || viewModels.Count == 0 || viewModels.Any(vm => vm == null))
            {
                return Json(new { Success = false, Error = "Must select rows to reprocess." });
            }

            try
            {
                reprocessFailedLocaleMessageCommandHandler.Execute(new ReprocessFailedLocaleMessageCommand
                {
                    MessageQueueIds = viewModels.Select(vm => vm.Id).ToList()
                });
            }
            catch (Exception e)
            {
                return Json(new { Success = false, Error = String.Format("Error occurred while attempting to reprocess failed locale messages. Error details: {0}", e) });
            }

            return Json(new { Success = true, ReprocessedData = viewModels });

            
        }
    }
}