using KitBuilderWebApi.DataAccess.Dto;
using KitBuilderWebApi.DatabaseModels;
using KitBuilderWebApi.QueryParameters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace KitBuilderWebApi.Helper
{
    public class KitHelper : IHelper<KitDto, KitSearchParameters>
    {
        private IUrlHelper urlHelper;

        public KitHelper(IUrlHelper urlHelper)
        {
            this.urlHelper = urlHelper;
        }

        public IQueryable<KitDto> SetOrderBy(IQueryable<KitDto> DataBeforePaging, KitSearchParameters Parameters)
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
                    orderBy = new string[] { "Description" };
                }

                foreach (string orderByOption in orderBy.Reverse())
                {
                    if (typeof(Kit).GetProperty(orderByOption.Split(" ")[0]) == null)
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

        public object GetPaginationData(PagedList<KitDto> dataAfterPaging, KitSearchParameters parameters)
        {
            var previousPageLink = dataAfterPaging.HasPrevious ?
                CreateInstructionListResourceUri(parameters,
                    ResourceUriType.PreviousPage) : null;

            var nextPageLink = dataAfterPaging.HasNext ?
                CreateInstructionListResourceUri(parameters,
                    ResourceUriType.NextPage) : null;

            var paginationMetadata = new
            {
                previousPageLink = previousPageLink,
                nextPageLink = nextPageLink,
                totalCount = dataAfterPaging.TotalCount,
                pageSize = dataAfterPaging.PageSize,
                currentPage = dataAfterPaging.CurrentPage,
                totalPages = dataAfterPaging.TotalPages
            };

            return paginationMetadata;
        }


        public string CreateInstructionListResourceUri(
         KitSearchParameters instructionListsParameters,
         ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return urlHelper.Link("GetKits",
                      new
                      {
                          fields = instructionListsParameters.Fields,
                          orderBy = instructionListsParameters.OrderBy,
                          ItemDescription = instructionListsParameters.ItemDescription,
                          ItemScanCode = instructionListsParameters.ItemScanCode,
                          LinkGroupName = instructionListsParameters.LinkGroupName,
                          pageNumber = instructionListsParameters.PageNumber - 1,
                          pageSize = instructionListsParameters.PageSize
                      });
                case ResourceUriType.NextPage:
                    return urlHelper.Link("GetKits",
                      new
                      {
                          fields = instructionListsParameters.Fields,
                          orderBy = instructionListsParameters.OrderBy,
                          ItemDescription = instructionListsParameters.ItemDescription,
                          ItemScanCode = instructionListsParameters.ItemScanCode,
                          LinkGroupName = instructionListsParameters.LinkGroupName,
                          pageNumber = instructionListsParameters.PageNumber + 1,
                          pageSize = instructionListsParameters.PageSize,
                          
                      });
                case ResourceUriType.Current:
                default:
                    return urlHelper.Link("GetKits",
                    new
                    {
                        fields = instructionListsParameters.Fields,
                        orderBy = instructionListsParameters.OrderBy,
                        ItemDescription = instructionListsParameters.ItemDescription,
                        ItemScanCode = instructionListsParameters.ItemScanCode,
                        LinkGroupName = instructionListsParameters.LinkGroupName,
                        pageNumber = instructionListsParameters.PageNumber,
                        pageSize = instructionListsParameters.PageSize
                    });
            }
        }

    }
}
