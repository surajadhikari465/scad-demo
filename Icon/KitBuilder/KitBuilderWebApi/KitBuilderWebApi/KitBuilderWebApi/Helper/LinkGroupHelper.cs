using KitBuilderWebApi.DataAccess.Dto;
using KitBuilderWebApi.DatabaseModels;
using KitBuilderWebApi.QueryParameters;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace KitBuilderWebApi.Helper
{
    public class LinkGroupHelper : IHelper<LinkGroupDto, LinkGroupParameters>
    {
        private IUrlHelper urlHelper;
        
        public LinkGroupHelper(IUrlHelper urlHelper
                              )
        {
            this.urlHelper = urlHelper;
        }   

        public bool SetOrderBy(ref IQueryable<LinkGroupDto> DataBeforePaging, LinkGroupParameters Parameters)
        {
            string[] orderBy;

            if (!string.IsNullOrEmpty(Parameters.OrderBy))
            {
                orderBy = Parameters.OrderBy.Split(',');
            }
            else

            {
                orderBy = new string[] { "GroupName" };
            }

            foreach (string orderByOption in orderBy)
            {
                if (typeof(LinkGroup).GetProperty(orderByOption.Split(" ")[0]) == null)
                {
                    return false;
                }

                DataBeforePaging = DataBeforePaging.OrderBy(orderByOption);
            }

            return true;
        }

        public object getPaginationData(PagedList<LinkGroupDto> DataAfterPaging, LinkGroupParameters Parameters)
        {
            var previousPageLink = DataAfterPaging.HasPrevious ?
                CreateInstructionListResourceUri(Parameters,
                ResourceUriType.PreviousPage) : null;

            var nextPageLink = DataAfterPaging.HasNext ?
                CreateInstructionListResourceUri(Parameters,
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

        public string CreateInstructionListResourceUri(
           LinkGroupParameters LinkGroupParameters,
           ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return urlHelper.Link("GetLinkGroups",
                      new
                      {
                          fields = LinkGroupParameters.Fields,
                          orderBy = LinkGroupParameters.OrderBy,
                          searchQuery = LinkGroupParameters.SearchGroupNameQuery,
                          GroupName = LinkGroupParameters.GroupName,
                          ScanCode = LinkGroupParameters.ScanCode,
                          pageNumber = LinkGroupParameters.PageNumber - 1,
                          pageSize = LinkGroupParameters.PageSize
                      });
                case ResourceUriType.NextPage:
                    return urlHelper.Link("GetLinkGroups",
                      new
                      {
                          fields = LinkGroupParameters.Fields,
                          orderBy = LinkGroupParameters.OrderBy,
                          searchQuery = LinkGroupParameters.SearchGroupNameQuery,
                          GroupName = LinkGroupParameters.GroupName,
                          ScanCode = LinkGroupParameters.ScanCode,
                          pageNumber = LinkGroupParameters.PageNumber + 1,
                          pageSize = LinkGroupParameters.PageSize
                      });
                case ResourceUriType.Current:
                default:
                    return urlHelper.Link("GetLinkGroups",
                    new
                    {
                        fields = LinkGroupParameters.Fields,
                        orderBy = LinkGroupParameters.OrderBy,
                        searchQuery = LinkGroupParameters.SearchGroupNameQuery,
                        GroupName = LinkGroupParameters.GroupName,
                        ScanCode = LinkGroupParameters.ScanCode,
                        pageNumber = LinkGroupParameters.PageNumber,
                        pageSize = LinkGroupParameters.PageSize
                    });
            }
        }
    }
}