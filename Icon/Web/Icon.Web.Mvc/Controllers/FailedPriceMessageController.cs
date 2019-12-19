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
    public class FailedPriceMessageController : Controller
    {
        private IQueryHandler<GetFailedPriceMessageParameters, List<MessageQueuePrice>> getFailedPriceMessageQueryHandler;
        private ICommandHandler<ReprocessFailedPriceMessageCommand> reprocessFailedPriceMessageCommandHandler;
        
        public FailedPriceMessageController(IQueryHandler<GetFailedPriceMessageParameters, List<MessageQueuePrice>> getFailedPriceMessageQueryHandler,
            ICommandHandler<ReprocessFailedPriceMessageCommand> reprocessFailedPriceMessageCommandHandler)
        {
            this.getFailedPriceMessageQueryHandler = getFailedPriceMessageQueryHandler;
            this.reprocessFailedPriceMessageCommandHandler = reprocessFailedPriceMessageCommandHandler;
        }
        
        
        // GET: FailedPriceMessage
        public ActionResult Index()
        {
            var failedPriceMessageViewModels = getFailedPriceMessageQueryHandler.Search(new GetFailedPriceMessageParameters())
                 .Select(ip => new FailedPriceMessageViewModel
                 {
                     Id = ip.MessageQueueId,
                     RegionCode = ip.RegionCode,
                     BusinesUnit_ID = ip.BusinessUnit_ID,
                     LocaleName = ip.LocaleName,
                     Price = ip.Price,
                     SalePrice = ip.SalePrice,
                     ChangeType = ip.ChangeType,
                     ScanCode = ip.ScanCode,
                     InsertDate = ip.InsertDate,
                     ProcessedDate = ip.ProcessedDate
                 });
            return View(failedPriceMessageViewModels);
        }

        public ActionResult Reprocess(List<FailedPriceMessageViewModel> viewModels)
        {
            if (viewModels == null || viewModels.Count == 0 || viewModels.Any(vm => vm == null))
            {
                return Json(new { Success = false, Error = "Must select rows to reprocess." });
            }

            try
            {
                reprocessFailedPriceMessageCommandHandler.Execute(new ReprocessFailedPriceMessageCommand
                {
                    MessageQueueIds = viewModels.Select(vm => vm.Id).ToList()
                });
            }
            catch (Exception e)
            {
                return Json(new { Success = false, Error = String.Format("Error occurred while attempting to reprocess failed price messages. Error details: {0}", e) });
            }

            return Json(new { Success = true, ReprocessedData = viewModels });
        }
    }
}