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
    public class GlobalPosPushController : Controller
    {
        private IQueryHandler<GetFailedIrmaPushParameters, List<IRMAPush>> getFailedIrmaPushQueryHandler;
        private ICommandHandler<ReprocessFailedIrmaPushCommand> reprocessIrmaPushCommandHandler;

        public GlobalPosPushController(IQueryHandler<GetFailedIrmaPushParameters, List<IRMAPush>> getFailedIrmaPushQueryHandler,
            ICommandHandler<ReprocessFailedIrmaPushCommand> reprocessIrmaPushCommandHandler)
        {
            this.getFailedIrmaPushQueryHandler = getFailedIrmaPushQueryHandler;
            this.reprocessIrmaPushCommandHandler = reprocessIrmaPushCommandHandler;
        }

        // GET: GlobalPosPush
        public ActionResult Index()
        {
            var failedGlobalPosPushViewModels = getFailedIrmaPushQueryHandler.Search(new GetFailedIrmaPushParameters())
                .Select(ip => new FailedGlobalPosPushViewModel
                {
                    Id = ip.IRMAPushID,
                    ScanCode = ip.Identifier,
                    BusinessUnitId = ip.BusinessUnit_ID,
                    ChangeType = ip.ChangeType,
                    RegionAbbreviation = ip.RegionCode,
                    EsbProcessFailedDate = ip.EsbReadyFailedDate,
                    UdmProcessFailedDate = ip.UdmFailedDate
                });

            return View(failedGlobalPosPushViewModels);
        }

        public ActionResult Reprocess(List<FailedGlobalPosPushViewModel> viewModels)
        {
            if (viewModels == null || viewModels.Count == 0 || viewModels.Any(vm => vm == null))
            {
                return Json(new { Success = false, Error = "Must select rows to reprocess." });
            }

            try
            {
                reprocessIrmaPushCommandHandler.Execute(new ReprocessFailedIrmaPushCommand
                {
                    IrmaPushIds = viewModels.Select(vm => vm.Id).ToList()
                });
            }
            catch (Exception e)
            {
                return Json(new { Success = false, Error = String.Format("Error occurred while attempting to reprocess failed global POS pushes. Error details: {0}", e) });
            }

            return Json(new { Success = true, ReprocessedData = viewModels });
        }
    }
}