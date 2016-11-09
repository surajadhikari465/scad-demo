using Mammoth.Common;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Logging;
using MammothWebApi.DataAccess.Queries;
using MammothWebApi.Extensions;
using MammothWebApi.Models;
using MammothWebApi.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace MammothWebApi.Controllers
{
    public class PriceController : ApiController
    {
        private IService<AddUpdatePrice> addUpdatePriceService;
        private IService<DeletePrice> deletePriceService;
        private IQueryHandler<GetAllBusinessUnitsQuery, List<int>> getAllBusinessUnitsQueryHandler;
        private ILogger logger;

        public PriceController(IService<AddUpdatePrice> addUpdatePriceService, 
            IService<DeletePrice> deletePriceService, 
            IQueryHandler<GetAllBusinessUnitsQuery, List<int>> getAllBusinessUnitsQueryHandler,
            ILogger logger)
        {
            this.addUpdatePriceService = addUpdatePriceService;
            this.deletePriceService = deletePriceService;
            this.getAllBusinessUnitsQueryHandler = getAllBusinessUnitsQueryHandler;
            this.logger = logger;
        }

        [HttpPut]
        [Route("api/prices")]
        public IHttpActionResult AddOrUpdatePrices([FromBody] List<PriceModel> prices)
        {
            if (prices == null || prices.Count == 0)
            {
                this.logger.Warn("Did not receive any prices to add or update.");
                return BadRequest("There were no prices submitted or the format of the data could not be read.");
            }

            if (prices.Count == 0)
            {
                this.logger.Warn("Did list of prices pass in was null.");
                return BadRequest("The list of prices passed in was null or the format of the data could not be recognized.");
            }

            try
            {
                logger.Debug(String.Format("Running AddOrUpdate for {0} Price rows for the following items: {1}.",
                       prices.Count, String.Join("|", prices.Select(p => p.ToLogString()).ToArray())));

                // Remove locales that don't have business units that exist
                var existingBusinessUnits = getAllBusinessUnitsQueryHandler.Search(new GetAllBusinessUnitsQuery());
                var nonExistingBusinessUnitModels = prices
                    .Where(p => !existingBusinessUnits.Contains(p.BusinessUnitId))
                    .ToList();
                foreach (var nonExistingBusinessUnitModel in nonExistingBusinessUnitModels)
                {
                    prices.Remove(nonExistingBusinessUnitModel);
                }

                // Run add/update price service
                AddUpdatePrice priceData = new AddUpdatePrice();
                priceData.Prices = prices.ToPriceServiceModel();
                addUpdatePriceService.Handle(priceData);

                // Setup Response
                if (nonExistingBusinessUnitModels.Any())
                {
                    IEnumerable<ErrorResponseModel<PriceModel>> errorModels = nonExistingBusinessUnitModels
                        .Select(p => new ErrorResponseModel<PriceModel>
                        {
                            Error = string.Format("BusinessUnit {0} does not exist.", p.BusinessUnitId),
                            ErrorResponseCode = ErrorResponseCodes.BusinessUnitDoesNotExist,
                            Model = p
                        });
                    logger.Debug(String.Format("{0} items had a BusinessUnit that did not exist. Item : {1}",
                        nonExistingBusinessUnitModels.Count, String.Join("|", nonExistingBusinessUnitModels.Select(p => p.ToLogString()).ToArray())));
                    return Ok(errorModels);
                }
                else
                {
                    logger.Debug("Returning OK for Price AddOrUpdate.");
                    return Ok();
                }
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        [HttpPost]
        [Route("api/prices/rollback")]
        public IHttpActionResult DeletePrices([FromBody] List<PriceModel> prices)
        {
            if (prices == null || prices.Count == 0)
            {
                this.logger.Warn("Did not receive any prices to rollback (delete).");
                return BadRequest("There were no prices submitted or the format of the data could not be read.");
            }

            if (prices.Count == 0)
            {
                this.logger.Warn("The list of prices passed in was null.");
                return BadRequest("The list of items passed in was null or the format of the data could not be recognized.");
            }

            try
            {
                // Run delete price service
                DeletePrice deletePriceData = new DeletePrice();
                deletePriceData.Prices = prices.ToPriceServiceModel();
                this.deletePriceService.Handle(deletePriceData);

                return Ok();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }
    }
}
