using Icon.Logging;
using NutritionWebApi.Common;
using NutritionWebApi.Common.Interfaces;
using NutritionWebApi.Common.Models;
using NutritionWebApi.DataAccess.Commands;
using NutritionWebApi.DataAccess.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace NutritionWebApi.Controllers
{
    public class NutritionController : ApiController
    {
        private IQueryHandler<GetNutritionItemQuery, List<NutritionItemModel>> getNutritionItemQuery;
        private ICommandHandler<AddOrUpdateNutritionItemCommand> addOrUpdateNutritionItemCommandHandler;
        private ILogger logger;

        public NutritionController(
            IQueryHandler<GetNutritionItemQuery, List<NutritionItemModel>> getNutritionItemQuery,
            ICommandHandler<AddOrUpdateNutritionItemCommand> addOrUpdateNutritionItemCommandHandler,
            ILogger logger)
        {
            this.getNutritionItemQuery = getNutritionItemQuery;
            this.addOrUpdateNutritionItemCommandHandler = addOrUpdateNutritionItemCommandHandler;
            this.logger = logger;
        }

        // GET api/Nutrition
        [HttpGet]
        public IEnumerable<NutritionItemModel> GetNutritionItems()
        {
            try
            {
                var items = getNutritionItemQuery.Search(new GetNutritionItemQuery());
                return items;
            }
            catch (Exception exception)
            {
                logger.Error("Error: " + exception.GetBaseException().Message);
                throw;
            }
        }

        // GET api/Nutrition/5
        [HttpGet]
        [Route(("api/Nutrition"))]
        public IHttpActionResult GetNutritionItem(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Bad Request: Please provide the PLU.");
            }

            try
            {
                var nutritionItme = getNutritionItemQuery.Search(new GetNutritionItemQuery() { Plu = id }).FirstOrDefault();
                return Ok(nutritionItme);
            }
            catch (Exception exception)
            {
                logger.Error("Error: " + exception.GetBaseException().Message);
                throw;
            }
        }

        [HttpPut]
        [HttpPost]
        public IHttpActionResult AddOrUpdateNutritionItem([FromBody]List<NutritionItemModel> value)
        {
            if (value == null || value.Count == 0)
            {
                logger.Error(string.Format("Request was in a format unsupported by the server. ", Request?.Content?.ReadAsStringAsync().Result));
                return BadRequest("Request was in a format unsupported by the server.");
            }
            try
            {
                string result = addOrUpdateNutritionItemCommandHandler.Execute(new AddOrUpdateNutritionItemCommand() { NutritionItems = value });
                logger.Info(string.Format(result + " PLUs: {0}", string.Join(", ", value.Select(v => v.Plu))));
                return Ok(result);
            }
            catch (Exception exception)
            {
                logger.Error("Error: " + exception.GetBaseException().Message);
                throw;
            }
        }
    }
}