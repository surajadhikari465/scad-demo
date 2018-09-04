using KitBuilderWebApi.DataAccess.Dto;
using KitBuilderWebApi.DatabaseModels;
using KitBuilderWebApi.QueryParameters;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Linq.Dynamic.Core;

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
        public bool SetOrderBy(ref IQueryable<ItemsDto> DataBeforePaging, ItemsParameters Parameters)
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

            foreach (string orderByOption in orderBy)
            {
                if (typeof(Items).GetProperty(orderByOption.Split(" ")[0]) == null)
                {
                    return false;
                }

                DataBeforePaging = DataBeforePaging.OrderBy(orderByOption);
            }

            return true;
        }

        public object getPaginationData(PagedList<ItemsDto> DataAfterPaging, ItemsParameters Parameters)
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