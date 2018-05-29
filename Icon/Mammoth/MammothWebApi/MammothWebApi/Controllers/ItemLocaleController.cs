using Mammoth.Common;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Logging;
using MammothWebApi.DataAccess.Commands;
using MammothWebApi.DataAccess.Queries;
using MammothWebApi.Extensions;
using MammothWebApi.Models;
using MammothWebApi.Service.Models;
using MammothWebApi.Service.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Http;

namespace MammothWebApi.Controllers
{
    [Authorize]
    public class ItemLocaleController : ApiController
    {
        private IUpdateService<AddUpdateItemLocale> itemLocaleService;
        private IQueryHandler<GetAllBusinessUnitsQuery, List<int>> getAllBusinessUnitsQueryHandler;
        private IUpdateService<DeauthorizeItemLocale> deauthorizeItemLocaleService;
        private ILogger logger;

        public ItemLocaleController(IUpdateService<AddUpdateItemLocale> itemLocaleService,
            IQueryHandler<GetAllBusinessUnitsQuery, List<int>> getAllBusinessUnitsQueryHandler,
            IUpdateService<DeauthorizeItemLocale> deauthorizeItemLocaleService,
            ILogger logger)
        {
            this.itemLocaleService = itemLocaleService;
            this.getAllBusinessUnitsQueryHandler = getAllBusinessUnitsQueryHandler;
            this.deauthorizeItemLocaleService = deauthorizeItemLocaleService;
            this.logger = logger;
        }

        [HttpPut]
        [Route("api/DeauthorizeItemLocale")]
        public IHttpActionResult DeauthorizeItemLocale(List<ItemLocaleModel> itemLocales)
        {
            if (itemLocales == null || itemLocales.Count == 0)
            {
                logger.Warn("The object passed is either null or does not contain any rows.");
                return BadRequest("The object sent is either null or does not contain any rows.");
            }

            try
            {        // Map to Service Model
                List<ItemLocaleServiceModel> itemLocaleServiceModels = itemLocales.ToItemLocaleServiceModel();
                DeauthorizeItemLocale deauthorizeItemLocaleData = new DeauthorizeItemLocale { ItemLocaleServiceModelList = itemLocaleServiceModels };
                deauthorizeItemLocaleService.Handle(deauthorizeItemLocaleData);

                return Ok();
            }

            catch (SqlException sqlException)
            {
                logger.Error("Sql Exception occurred.", sqlException);
                return InternalServerError(sqlException);
            }
            catch (Exception exception)
            {
                logger.Error("Error occurred in DeleteItemLocalePrice.", exception);
                return InternalServerError(exception);
            }
        }

        [HttpPut]
        [Route("api/itemLocale")]
        public IHttpActionResult AddOrUpdateItemLocale(List<ItemLocaleModel> itemLocales)
        {
            if (itemLocales == null || itemLocales.Count == 0)
            {
                logger.Warn("The object passed is either null or does not contain any rows.");
                return BadRequest("The object sent is either null or does not contain any rows.");
            }

            try
            {
                logger.Debug(String.Format("Running AddOrUpdate for {0} ItemLocale rows for the following items: {1}.",
                    itemLocales.Count, String.Join("|", itemLocales.Select(il => il.ToLogString()).ToArray())));

                // Remove locales that don't have business units that exist
                var existingBusinessUnits = getAllBusinessUnitsQueryHandler.Search(new GetAllBusinessUnitsQuery());
                var nonExistingBusinessUnitModels = itemLocales
                    .Where(il => !existingBusinessUnits.Contains(il.BusinessUnitId))
                    .ToList();
                foreach (var nonExistingBusinessUnitModel in nonExistingBusinessUnitModels)
                {
                    itemLocales.Remove(nonExistingBusinessUnitModel);
                }

                // Map to Service Model
                List<ItemLocaleServiceModel> itemLocaleServiceModels = itemLocales.ToItemLocaleServiceModel();

                // Run Add/Update Service
                AddUpdateItemLocale itemLocaleServiceData = new AddUpdateItemLocale { ItemLocales = itemLocaleServiceModels };
                this.itemLocaleService.Handle(itemLocaleServiceData);

                // Setup Response
                if (nonExistingBusinessUnitModels.Any())
                {
                    IEnumerable<ErrorResponseModel<ItemLocaleModel>> errorModels = nonExistingBusinessUnitModels
                        .Select(il => new ErrorResponseModel<ItemLocaleModel>
                        {
                            Error = string.Format("BusinessUnit {0} does not exist.", il.BusinessUnitId),
                            ErrorResponseCode = ErrorResponseCodes.BusinessUnitDoesNotExist,
                            Model = il
                        });

                    logger.Debug(String.Format("{0} items had a BusinessUnit that did not exist. Item : {1}",
                        nonExistingBusinessUnitModels.Count, String.Join("|", nonExistingBusinessUnitModels.Select(il => il.ToLogString()).ToArray())));

                    return Ok(errorModels);
                }
                else
                {
                    logger.Debug("Returning OK for ItemLocale AddOrUpdate.");
                    return Ok();
                }
            }
            catch (SqlException sqlException)
            {
                logger.Error("Sql Exception occurred.", sqlException);
                return InternalServerError(sqlException);
            }
            catch (Exception exception)
            {
                logger.Error("Error occurred in AddOrUpdateItemLocales.", exception);
                return InternalServerError(exception);
            }
        }
    }
}