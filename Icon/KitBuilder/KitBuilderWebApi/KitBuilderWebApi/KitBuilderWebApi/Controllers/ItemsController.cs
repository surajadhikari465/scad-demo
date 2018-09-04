using System.Linq;
using AutoMapper;
using KitBuilderWebApi.DataAccess.Repository;
using KitBuilderWebApi.DatabaseModels;
using KitBuilderWebApi.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq.Dynamic.Core;
using KitBuilderWebApi.QueryParameters;
using KitBuilderWebApi.DataAccess.Dto;

namespace KitBuilderWebApi.Controllers
{
    [Route("api/Items")]
    public class ItemsController : Controller
    {
        private ILogger<LinkGroupController> logger;
        private ItemHelper itemHelper;
        public ItemsController( ILogger<LinkGroupController> logger,
                                ItemHelper itemHelper
                                   )
        {
            this.logger = logger;
            this.itemHelper = itemHelper;
        }

        // GET api/GetLinkGroups
        [HttpGet(Name = "GetItems")]
        public IActionResult GetItems(ItemsParameters itemsParameters)
        {

            var itemsListBeforePaging = itemHelper.GetitemsListBeforePaging();

            // will set order by if passed, otherwise use default orderby                           
            if (!itemHelper.SetOrderBy(ref itemsListBeforePaging, itemsParameters))
                return BadRequest();

            //build the query if any filter or search query critiera is passed
            itemHelper.BuildQueryToFilterData(itemsParameters, ref itemsListBeforePaging);

            // call the static method on the paged list to filter items
            var itemsListsAfterPaging = PagedList<ItemsDto>.Create(itemsListBeforePaging,
                                                            itemsParameters.PageNumber,
                                                            itemsParameters.PageSize
                                                            );

            var paginationMetadata = itemHelper.getPaginationData(itemsListsAfterPaging, itemsParameters);

            Response.Headers.Add("X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            return Ok(itemsListsAfterPaging.ShapeData(itemsParameters.Fields));
        }
    }
}