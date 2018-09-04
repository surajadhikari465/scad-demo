using KitBuilderWebApi.DataAccess.Dto;
using KitBuilderWebApi.DataAccess.Repository;
using KitBuilderWebApi.DatabaseModels;
using KitBuilderWebApi.QueryParameters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace KitBuilderWebApi.Helper
{
    public class ItemHelper
    {

        private IUrlHelper urlHelper;
        private IRepository<Items> itemsRepository { get; set; }

        public ItemHelper(IUrlHelper urlHelper,
                                IRepository<Items> itemsRepository
                                    )
        {
            this.urlHelper = urlHelper;
            this.itemsRepository = itemsRepository;
        }
        internal bool SetOrderBy(ref IQueryable<ItemsDto> ItemsBeforePaging, ItemsParameters ItemsParameters)
        {
            string[] orderBy;

            if (!string.IsNullOrEmpty(ItemsParameters.OrderBy))
            {
                orderBy = ItemsParameters.OrderBy.Split(',');
            }
            else

            {
                orderBy = new string[] { "ScanCode" };
            }

            foreach (string orderByOption in orderBy)
            {
                if (typeof(InstructionList).GetProperty(orderByOption.Split(" ")[0]) == null)
                {
                    return false;
                }

                ItemsBeforePaging = ItemsBeforePaging.OrderBy(orderByOption);
            }

            return true;
        }
        internal object getPaginationData(PagedList<ItemsDto> ItemsAfterPaging, ItemsParameters ItemsParameters)
        {
            var previousPageLink = ItemsAfterPaging.HasPrevious ?
                CreateItemsResourceUri(ItemsParameters,
                ResourceUriType.PreviousPage) : null;

            var nextPageLink = ItemsAfterPaging.HasNext ?
                CreateItemsResourceUri(ItemsParameters,
                ResourceUriType.NextPage) : null;

            var paginationMetadata = new
            {
                previousPageLink = previousPageLink,
                nextPageLink = nextPageLink,
                totalCount = ItemsAfterPaging.TotalCount,
                pageSize = ItemsAfterPaging.PageSize,
                currentPage = ItemsAfterPaging.CurrentPage,
                totalPages = ItemsAfterPaging.TotalPages
            };

            return paginationMetadata;
        }
        internal void BuildQueryToFilterData(ItemsParameters itemsParameters, ref IQueryable<ItemsDto> itemsBeforePaging)
        {
            if (!string.IsNullOrEmpty(itemsParameters.ProductDesc))
            {
                var nameForWhereClause = itemsParameters.ProductDesc.Trim().ToLowerInvariant();
                itemsBeforePaging = itemsBeforePaging
                                               .Where(i => i.ProductDesc.ToLowerInvariant() == nameForWhereClause);
            }

            if (!string.IsNullOrEmpty(itemsParameters.ScanCode))
            {
                var scanCodeForWhereClause = itemsParameters.ScanCode.Trim().ToLowerInvariant();
                itemsBeforePaging = itemsBeforePaging
                                               .Where(i => i.ScanCode.ToLowerInvariant() == scanCodeForWhereClause);
            }

            if (!string.IsNullOrEmpty(itemsParameters.SearchScanCodeQuery))
            {
                var searchQueryForWhereClause = itemsParameters.SearchScanCodeQuery.Trim().ToLowerInvariant();
                itemsBeforePaging = itemsBeforePaging
                                               .Where(i => i.ScanCode.ToLowerInvariant().Contains(searchQueryForWhereClause));
            }

            if (!string.IsNullOrEmpty(itemsParameters.SearchProductDescQuery))
            {
                var searchQueryForWhereClause = itemsParameters.SearchProductDescQuery.Trim().ToLowerInvariant();
                itemsBeforePaging = itemsBeforePaging
                                               .Where(i => i.ProductDesc.ToLowerInvariant().Contains(searchQueryForWhereClause));
            }
        }

        internal IQueryable<ItemsDto> GetitemsListBeforePaging()
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
                                            InsertDate = l.InsertDate
                                        };

            return itemsListBeforePaging;
        }
        internal string CreateItemsResourceUri(
           ItemsParameters ItemsParameters,
           ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return urlHelper.Link("GetItems",
                      new
                      {
                          fields = ItemsParameters.Fields,
                          orderBy = ItemsParameters.OrderBy,
                          searchScanCodeQuery = ItemsParameters.SearchScanCodeQuery,
                          SearchProductDescQuery = ItemsParameters.SearchProductDescQuery,
                          ScanCode = ItemsParameters.ScanCode,
                          ProductDesc = ItemsParameters.ProductDesc,
                          pageNumber = ItemsParameters.PageNumber - 1,
                          pageSize = ItemsParameters.PageSize
                      });
                case ResourceUriType.NextPage:
                    return urlHelper.Link("GetItemss",
                      new
                      {
                          fields = ItemsParameters.Fields,
                          orderBy = ItemsParameters.OrderBy,
                          searchScanCodeQuery = ItemsParameters.SearchScanCodeQuery,
                          SearchProductDescQuery = ItemsParameters.SearchProductDescQuery,
                          ScanCode = ItemsParameters.ScanCode,
                          ProductDesc = ItemsParameters.ProductDesc,
                          pageNumber = ItemsParameters.PageNumber + 1,
                          pageSize = ItemsParameters.PageSize
                      });
                case ResourceUriType.Current:
                default:
                    return urlHelper.Link("GetItemss",
                    new
                    {
                        fields = ItemsParameters.Fields,
                        orderBy = ItemsParameters.OrderBy,
                        searchScanCodeQuery = ItemsParameters.SearchScanCodeQuery,
                        SearchProductDescQuery = ItemsParameters.SearchProductDescQuery,
                        ScanCode = ItemsParameters.ScanCode,
                        ProductDesc = ItemsParameters.ProductDesc,
                        pageNumber = ItemsParameters.PageNumber,
                        pageSize = ItemsParameters.PageSize
                    });
            }
        }
    }
}
