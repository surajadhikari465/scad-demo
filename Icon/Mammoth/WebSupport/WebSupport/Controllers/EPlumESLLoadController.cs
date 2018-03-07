using Icon.Common.DataAccess;
using Icon.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebSupport.DataAccess;
using WebSupport.DataAccess.Commands;
using WebSupport.DataAccess.Queries;
using WebSupport.DataAccess.TransferObjects;
using WebSupport.Models;
using WebSupport.Services;
using WebSupport.ViewModels;

namespace WebSupport.Controllers
{
    public class EPlumESLLoadController : Controller
    {
        #region Fields

        private ILogger logger;
        private IQueryHandler<GetStoresForRegionParameters, IList<StoreTransferObject>> queryForStores;
        private ICommandHandler<MassInsertToEPlumQueueCommand> eplumCommandHandler;
        private ICommandHandler<MassInsertToESLQueueCommand> eslCommandHandler;

        #endregion
        public EPlumESLLoadController(
            ILogger logger,
            IQueryHandler<GetStoresForRegionParameters, IList<StoreTransferObject>> queryForStores,
            ICommandHandler<MassInsertToEPlumQueueCommand> eplumCommandHandler,
            ICommandHandler<MassInsertToESLQueueCommand> eslCommandHandler)
        {
            this.logger = logger;
            this.queryForStores = queryForStores;
            this.eplumCommandHandler = eplumCommandHandler;
            this.eslCommandHandler = eslCommandHandler;
        }

        [HttpGet]
        public ActionResult Index()
        {
            EPlumESLLoadViewModel viewModel = new EPlumESLLoadViewModel();
            viewModel.SetRegionAndSystemOptions(StaticData.WholeFoodsRegions, StaticData.EPlumESLSystems);
            if (TempData["Errors"] != null)
            {
                viewModel.Errors = TempData["Errors"] as string;
            }
            if (TempData["Success"] != null)
            {
                viewModel.Success = TempData["Success"] as bool?;
            }

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(EPlumESLLoadViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            this.ExecuteQueueEplumEslItems(viewModel);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult Stores(int regionCode)
        {
            if (StaticData.WholeFoodsRegions.Length > 0 && regionCode < StaticData.WholeFoodsRegions.Length)
            {
                var chosenRegion = StaticData.WholeFoodsRegions.ElementAt(regionCode);
                if (String.IsNullOrWhiteSpace(chosenRegion))
                {
                    throw new ArgumentException($"invalid region '{chosenRegion ?? "null"}'", nameof(chosenRegion));
                }
                else
                {
                    var stores = queryForStores.Search(new GetStoresForRegionParameters { Region = chosenRegion });
                    return Json(stores, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(regionCode), $"invalid region code {regionCode}");
            }
        }

        private void ExecuteQueueEplumEslItems(EPlumESLLoadViewModel viewModel)
        {
            string region = StaticData.WholeFoodsRegions.ElementAt(viewModel.RegionIndex);

            List<string> systems = StaticData
                .EPlumESLSystems
                .Where((s, i) => viewModel.DownstreamSystems.Contains(i))
                .ToList();

            List<string> stores = viewModel.Stores.ToList();

            bool eplumQueued = false;
            bool eslQueued = false;

            if (systems.Contains(EPlumESLConstants.EPlum))
            {
                try
                {
                    this.eplumCommandHandler.Execute(new MassInsertToEPlumQueueCommand
                    {
                        Region = region,
                        BusinessUnitIds = stores
                    });

                    eplumQueued = true;
                }
                catch (Exception e)
                {
                    TempData["Errors"] = $"Unexpected error occurred in queuing items for EPlum, check logs for more details. Error:{e.Message}";
                    logger.Error(e.ToString());
                }
            }

            if (systems.Contains(EPlumESLConstants.ESL))
            {
                try
                {
                    this.eslCommandHandler.Execute(new MassInsertToESLQueueCommand
                    {
                        Region = region,
                        BusinessUnitIds = stores
                    });

                    eslQueued = true;
                }
                catch (Exception e)
                {
                    if (TempData["Errors"] != null)
                        TempData["Errors"] = TempData["Errors"] + $"  Unexpected error occurred in queuing items for ESL, check logs for more details. Error:{e.Message}";
                    else
                        TempData["Errors"] = $"Unexpected error occurred in queuing items for ESL, check logs for more details. Error:{e.Message}";

                    logger.Error(e.ToString());
                }
            }

            if (TempData["Errors"] != null && eslQueued)
                TempData["Errors"] = $"Items have been sucessfully queued for ESL. " + TempData["Errors"];
            else if (TempData["Errors"] != null && eplumQueued)
                TempData["Errors"] = $"Items have been sucessfully queued for Eplum. " + TempData["Errors"];

            if (TempData["Errors"] == null)
                TempData["Success"] = true;
            else
                TempData["Success"] = false;
        }
    }
}