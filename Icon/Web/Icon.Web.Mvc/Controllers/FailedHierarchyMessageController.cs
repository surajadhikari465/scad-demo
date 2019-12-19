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
    public class FailedHierarchyMessageController : Controller
    {
        private IQueryHandler<GetFailedHierarchyMessageParameters, List<MessageQueueHierarchy>> getFailedHierarchyMessageQueryHandler;
        private ICommandHandler<ReprocessFailedHierarchyMessageCommand> reprocessFailedHierarchyMessageCommandHandler;

        public FailedHierarchyMessageController(IQueryHandler<GetFailedHierarchyMessageParameters, List<MessageQueueHierarchy>> getFailedHierarchyMessageQueryHandler,
            ICommandHandler<ReprocessFailedHierarchyMessageCommand> reprocessFailedHierarchyCommandHandler)
        {
            this.getFailedHierarchyMessageQueryHandler = getFailedHierarchyMessageQueryHandler;
            this.reprocessFailedHierarchyMessageCommandHandler = reprocessFailedHierarchyCommandHandler;
        }

        // GET: FailedHierarchyMessage
        public ActionResult Index()
        {
            var failedHierarchyMessageViewModels = getFailedHierarchyMessageQueryHandler.Search(new GetFailedHierarchyMessageParameters())
                .Select(ip => new FailedHierarchyMessageViewModel
                {
                    Id = ip.MessageQueueId,
                    HierarchyName = ip.HierarchyName,
                    HierarchyClassName = ip.HierarchyClassName,
                    InsertDate = ip.InsertDate,
                    ProcessedDate = ip.ProcessedDate
                });
            return View(failedHierarchyMessageViewModels);
        }

        public ActionResult Reprocess(List<FailedHierarchyMessageViewModel> viewModels)
        {
            if (viewModels == null || viewModels.Count == 0 || viewModels.Any(vm => vm == null))
            {
                return Json(new { Success = false, Error = "Must select rows to reprocess." });
            }

            try
            {
                reprocessFailedHierarchyMessageCommandHandler.Execute(new ReprocessFailedHierarchyMessageCommand
                {
                    MessageQueueIds = viewModels.Select(vm => vm.Id).ToList()
                });
            }
            catch (Exception e)
            {
                return Json(new { Success = false, Error = String.Format("Error occurred while attempting to reprocess failed hierarchy messages. Error details: {0}", e) });
            }

            return Json(new { Success = true, ReprocessedData = viewModels });
        }
    }
}