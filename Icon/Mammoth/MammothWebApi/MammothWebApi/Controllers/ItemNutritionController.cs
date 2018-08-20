using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Logging;
using MammothWebApi.DataAccess.Models;
using MammothWebApi.DataAccess.Queries;
using MammothWebApi.Models;

namespace MammothWebApi.Controllers
{
    public class ItemNutritionController : ApiController
    {

        private ILogger logger;
        private IQueryHandler<GetItemNutritionAttributesByItemIdQuery,IEnumerable<ItemNutritionAttributes>> getItemNutritionAttributesByItemId;


        public ItemNutritionController(ILogger logger,
            IQueryHandler<GetItemNutritionAttributesByItemIdQuery, IEnumerable<ItemNutritionAttributes>>  getItemNutritionAttributesByItemId)
        {
            this.logger = logger;
            this.getItemNutritionAttributesByItemId = getItemNutritionAttributesByItemId;
        }


        // using POST instead of GET to overcome the URI Parameter length limit that GET has.
        [HttpPost]
        [Route("api/itemNutrition")]
        public IHttpActionResult GetItemNutrition([FromBody] ItemNutritionRequestModel  request)
        {
            if (!ModelState.IsValid || request == null)
            {
                return this.BadRequest("Parameters are invalid.");
            }

            if (request.ItemIds == null) return this.BadRequest("At least one ItemId must be included");

            try
            {
                GetItemNutritionAttributesByItemIdQuery nutritionRequsts =
                    new GetItemNutritionAttributesByItemIdQuery {ItemIds = request.ItemIds.ToList()};

                var itemNutritionAttributes = this.getItemNutritionAttributesByItemId.Search(nutritionRequsts);

                // convert to Dictionary so json results can be quickly referenced by ItemId
                var itemNutritionDictionary = itemNutritionAttributes
                    .ToDictionary(i => i.ItemId,  i=> i)
                    .OrderBy(o => o.Key);

                return Json(itemNutritionDictionary);

                /*

                 EXAMPLE DATA:

                results = {
                    "45840": {
                        "Calories": 150,
                        "ServingPerContainer": "6",
                        "ServingSizeDesc": "1 ea",
                        "ServingsPerPortion": 6
                    },
                    "76603": {
                        "Calories": 280,
                        "ServingPerContainer": "4",
                        "ServingSizeDesc": "1 ea",
                        "ServingsPerPortion": 4
                        }
                    }

                    results[76603] = {
                        "Calories": 280,
                        "ServingPerContainer": "4",
                        "ServingSizeDesc": "1 ea",
                        "ServingsPerPortion": 4
                    }

                    results[76603].Calories = 280

                   */

            }
            catch (Exception e)
            {
                this.logger.Error("Error performing GetItemNutrition Http Get request.", e);
                return InternalServerError(new Exception(
                    "There was an error retrieving the item Nutrition Attributes from the Mammoth database.  Please reach out to the support team for assistance."));
            }
        }
    }
}