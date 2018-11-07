using KitBuilderWebApi.QueryParameters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using KitBuilder.DataAccess.DatabaseModels;
using KitBuilder.DataAccess.Dto;

namespace KitBuilderWebApi.Helper
{
    public class ItemHelper: IHelper<ItemsDto, ItemsParameters>
    {
        private IUrlHelper urlHelper;

        public ItemHelper(IUrlHelper urlHelper
                          )
        {
            this.urlHelper = urlHelper;
        }
        public IQueryable<ItemsDto> SetOrderBy(IQueryable<ItemsDto> DataBeforePaging, ItemsParameters Parameters)
        {
            try
            {
                string[] orderBy;

                if (!string.IsNullOrEmpty(Parameters.OrderBy))
                {
                    orderBy = Parameters.OrderBy.Split(',');
                }
                else

                {
                    orderBy = new string[] { "ScanCode" };
                }

                foreach (string orderByOption in orderBy.Reverse())
                {
                    if (typeof(Items).GetProperty(orderByOption.Split(" ")[0]) == null)
                    {
                        throw new Exception("Invalid Order By");
                    }

                    DataBeforePaging = DataBeforePaging.OrderBy(orderByOption);
                }
                return DataBeforePaging;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object GetPaginationData(PagedList<ItemsDto> DataAfterPaging, ItemsParameters Parameters)
        {
            var previousPageLink = DataAfterPaging.HasPrevious ?
                CreateItemsResourceUri(Parameters,
                ResourceUriType.PreviousPage) : null;

            var nextPageLink = DataAfterPaging.HasNext ?
                CreateItemsResourceUri(Parameters,
                ResourceUriType.NextPage) : null;

            var paginationMetadata = new
            {
                previousPageLink = previousPageLink,
                nextPageLink = nextPageLink,
                totalCount = DataAfterPaging.TotalCount,
                pageSize = DataAfterPaging.PageSize,
                currentPage = DataAfterPaging.CurrentPage,
                totalPages = DataAfterPaging.TotalPages
            };

            return paginationMetadata;
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
                    return urlHelper.Link("GetItems",
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
                    return urlHelper.Link("GetItems",
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