using Icon.Common.DataAccess;
using Icon.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebSupport.DataAccess.Queries;
using WebSupport.ViewModels;
using WebSupport.DataAccess.TransferObjects;
using WebSupport.Models;
using Esb.Core.EsbServices;
using Newtonsoft.Json;
using Icon.Common;
using WebSupport.DataAccess.Commands;

namespace WebSupport.Controllers
{
    public class CheckPointRequestController : Controller
    {
        private ILogger logger;
        private IEsbMultipleMessageService<CheckPointRequestViewModel> checkPointRequestMessageService;
        private IQueryHandler<GetStoresForRegionParameters, IList<StoreTransferObject>> queryForStores;
        private ICommandHandler<ArchiveCheckpointMessageCommandParameters> archiveCommand;
        private int maxItemsPerReq = 100;
        public const string tempDataErrors = "Errors";
        public const string tempDataSuccess = "Success";

        public CheckPointRequestController(
            ILogger logger,
            IQueryHandler<GetStoresForRegionParameters, IList<StoreTransferObject>> storesQuery,
            IEsbMultipleMessageService<CheckPointRequestViewModel> service,
            ICommandHandler<ArchiveCheckpointMessageCommandParameters> archiveCheckpointMessageCommandHandler)
        {
            this.logger = logger;
            this.queryForStores = storesQuery;
            this.checkPointRequestMessageService = service;
            this.archiveCommand = archiveCheckpointMessageCommandHandler;
            maxItemsPerReq = AppSettingsAccessor.GetIntSetting(EsbConstants.CheckPointRequestMaxItemsKey, maxItemsPerReq);
        }

        [HttpGet]
        public ActionResult Index()
        {
            var viewModel = new CheckPointRequestViewModel();
            viewModel.SetRegions(StaticData.WholeFoodsRegions);

            viewModel.Errors = TempData[tempDataErrors] as List<string>;
            viewModel.Success = TempData[tempDataSuccess] as bool?;

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(CheckPointRequestViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (viewModel.ScanCodesList.Count * viewModel.Stores.Length > maxItemsPerReq)
                    {
                        var limitExceededMsg = $"A maximum of {maxItemsPerReq} GPM Checkpoint item-store requests can be made at once. " +
                            $"The submitted data contained {viewModel.Stores.Length} Stores and {viewModel.ScanCodesList.Count} ScanCodes, " +
                            $"which would result in {viewModel.ScanCodesList.Count * viewModel.Stores.Length} request messages if attempted. " +
                            $"Please de-select some stores and/or items and then re-submit.";
                        this.logger.Warn(limitExceededMsg);
                        TempData[tempDataErrors] = limitExceededMsg;
                        return HandleErrorForCheckPointMessage(viewModel);
                    }

                    var responses = checkPointRequestMessageService.Send(viewModel);

                    var errorDetailsList = new List<string>();
                    foreach (var response in responses)
                    {
                        if (response.Status == EsbServiceResponseStatus.Failed)
                        {
                            errorDetailsList.Add(response.ErrorDetails);
                            this.logger.Error($"CheckPoint Request to GPM failed: {response.ErrorDetails}");
                        }
                        else
                        {
                            this.logger.Info($"CheckPoint Request sent to GPM: {response.ErrorDetails}");
                        }
                    }
                    if (responses.Any(r => r.Status == EsbServiceResponseStatus.Failed))
                    {
                        TempData[tempDataErrors] = string.Join(Environment.NewLine, errorDetailsList.ToArray());
                        return HandleErrorForCheckPointMessage(viewModel);
                    }
                }
                else
                {
                    var invalidModelMsg = $"Invalid ViewModel submitted for GPM Checkpoint Request. " +
                        $"Stores: {string.Join(",", viewModel.Stores ?? new string[] { "none" })} " +
                        $"ScanCodes: {(string.IsNullOrEmpty(viewModel.ScanCodes) ? "none" : string.Join(",", viewModel.ScanCodesList))}. " +
                        $"Model State Errors: {string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage))}";
                    this.logger.Warn(invalidModelMsg);
                    TempData[tempDataErrors] = invalidModelMsg;
                    return HandleErrorForCheckPointMessage(viewModel);
                }
            }
            catch(Exception ex)
            {
                logger.Error(JsonConvert.SerializeObject(
                    new
                    {
                        Message = "Unexpected error occurred",
                        Controller = nameof(CheckPointRequestController),
                        ViewModel = viewModel,
                        Exception = ex
                    }));
                TempData[tempDataErrors] = "Unexpected error occurred. Error:" + ex.ToString();
            }

            return RedirectToAction("Index");
        }

        private ActionResult HandleErrorForCheckPointMessage(CheckPointRequestViewModel viewModel)
        {
            //re-populate options for view model
            var existingRegionChoice = viewModel.RegionIndex;
            viewModel.SetRegions(StaticData.WholeFoodsRegions);
            if (viewModel.RegionIndex > 0)
            {
                var chosenRegion = StaticData.WholeFoodsRegions.ElementAt(viewModel.RegionIndex);
                var queryParam = new GetStoresForRegionParameters { Region = chosenRegion };

                //re-pop already selected stores (if stores are valid but items/downstream not)
                var stores = queryForStores.Search(queryParam).Select(sto => new StoreViewModel(sto));
                viewModel.SetStoreOptions(stores);
            }
            return View(viewModel);
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
    }
}