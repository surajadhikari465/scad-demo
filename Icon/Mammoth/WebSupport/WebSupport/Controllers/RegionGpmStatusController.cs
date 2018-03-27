using Icon.Common.DataAccess;
using Icon.Logging;
using Mammoth.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebSupport.DataAccess.Commands;
using WebSupport.DataAccess.Models;
using WebSupport.DataAccess.Queries;
using WebSupport.Filters;
using WebSupport.ViewModels;

namespace WebSupport.Controllers
{
    [ErrorLogActionFilter]
    public class RegionGpmStatusController : Controller
    {
        private IQueryHandler<GetGpmStatusParameters, IList<RegionGpmStatus>> getGpmStatusQuery;
        private ICommandHandler<UpdateGpmStatusTableCommandParameters> updateGpmStatusCommand;

        private ILogger logger;

        public RegionGpmStatusController(
            IQueryHandler<GetGpmStatusParameters, IList<RegionGpmStatus>> getGpmStatusQuery,
            ICommandHandler<UpdateGpmStatusTableCommandParameters> updateGpmStatusCommand,
            ILogger logger)
        {
            this.getGpmStatusQuery = getGpmStatusQuery;
            this.updateGpmStatusCommand = updateGpmStatusCommand;
            this.logger = logger;
        }

        [HttpGet]
        public ActionResult Index(string region = null)
        {
            ViewBag.CanEdit = true;

            var existingGpmStatuses = getGpmStatusQuery
                .Search(new GetGpmStatusParameters { Region = region })
                .Select(m=>new RegionGpmStatusViewModel(m));
            return View(existingGpmStatuses);
        }

        [HttpPost]
        public ActionResult Index(IEnumerable<RegionGpmStatusViewModel> posted)
        {
            var postedEntityData = posted
                .Select(r => RegionGpmStatusViewModel.ToEntityModel(r));
            var cmdParameters = new UpdateGpmStatusTableCommandParameters(postedEntityData);
            updateGpmStatusCommand.Execute(cmdParameters);

            return RedirectToAction("Index");
        }
    }
}