using System.Linq;
using KitBuilderWebApi.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using KitBuilderWebApi.QueryParameters;
using System;
using KitBuilder.DataAccess.DatabaseModels;
using KitBuilder.DataAccess.Dto;
using KitBuilder.DataAccess.Repository;

namespace KitBuilderWebApi.Controllers
{
    [Route("api/Items")]
    public class ItemsController : Controller
    {
        private IRepository<Items> itemsRepository;
        private ILogger<LinkGroupController> logger;
        private IHelper<ItemsDto, ItemsParameters> itemHelper;
        public ItemsController(IRepository<Items> itemsRepository,
                                ILogger<LinkGroupController> logger,
                                IHelper<ItemsDto, ItemsParameters> itemHelper
                                   )
        {
            this.itemsRepository = itemsRepository;
            this.logger = logger;
            this.itemHelper = itemHelper;
        }

        // GET api/GetLinkGroups
        [HttpGet(Name = "GetItems")]
        public IActionResult GetItems(ItemsParameters itemsParameters)
        {

            var itemsListBeforePaging = from l in itemsRepository.GetAll()
                                        select new ItemsDto()
                                        {
                                            ItemId = l.ItemId,
                                            ScanCode = l.ScanCode,
                                            ProductDesc = l.ProductDesc,
                                            CustomerFriendlyDesc = l.CustomerFriendlyDesc,
                                            KitchenDesc = l.KitchenDesc,
                                            BrandName = l.BrandName,
                                            LargeImageUrl = l.LargeImageUrl,
                                            SmallImageUrl = l.SmallImageUrl,
                                            InsertDateUtc = l.InsertDateUtc
                                        };

            // will set order by if passed, otherwise use default orderby 
            try
            {
                itemsListBeforePaging = itemHelper.SetOrderBy(itemsListBeforePaging, itemsParameters);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest();
            }

            //build the query if any filter or search query critiera is passed
            BuildQueryToFilterData(itemsParameters, ref itemsListBeforePaging);

            // call the static method on the paged list to filter items
            var itemsListsAfterPaging = PagedList<ItemsDto>.Create(itemsListBeforePaging,
                                                            itemsParameters.PageNumber,
                                                            itemsParameters.PageSize
                                                            );

            var paginationMetadata = itemHelper.GetPaginationData(itemsListsAfterPaging, itemsParameters);

            Response.Headers.Add("X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            return Ok(itemsListsAfterPaging.ShapeData(itemsParameters.Fields));
        }

        internal void BuildQueryToFilterData(ItemsParameters itemsParameters, ref IQueryable<ItemsDto> itemsBeforePaging)
        {
            if (!string.IsNullOrEmpty(itemsParameters.ProductDesc))
            {
                var nameForWhereClause = itemsParameters.ProductDesc.Trim().ToLower();
                itemsBeforePaging = itemsBeforePaging
                                               .Where(i => i.ProductDesc.ToLower() == nameForWhereClause);
            }

            if (!string.IsNullOrEmpty(itemsParameters.ScanCode))
            {
                var scanCodeForWhereClause = itemsParameters.ScanCode.Trim().ToLower();
                itemsBeforePaging = itemsBeforePaging
                                               .Where(i => i.ScanCode.ToLower() == scanCodeForWhereClause);
            }

            if (!string.IsNullOrEmpty(itemsParameters.SearchScanCodeQuery))
            {
                var searchQueryForWhereClause = itemsParameters.SearchScanCodeQuery.Trim().ToLower();
                itemsBeforePaging = itemsBeforePaging
                                               .Where(i => i.ScanCode.ToLower().Contains(searchQueryForWhereClause));
            }

            if (!string.IsNullOrEmpty(itemsParameters.SearchProductDescQuery))
            {
                var searchQueryForWhereClause = itemsParameters.SearchProductDescQuery.Trim().ToLower();
                itemsBeforePaging = itemsBeforePaging
                                               .Where(i => i.ProductDesc.ToLower().Contains(searchQueryForWhereClause));
            }
        }
    }
}