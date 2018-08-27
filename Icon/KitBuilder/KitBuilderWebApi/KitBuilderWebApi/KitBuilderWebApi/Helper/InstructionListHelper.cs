using KitBuilderWebApi.DataAccess.Dto;
using KitBuilderWebApi.DatabaseModels;
using KitBuilderWebApi.QueryParameters;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace KitBuilderWebApi.Helper
{
    public class InstructionListHelper
    {
        private IUrlHelper urlHelper;

        public InstructionListHelper( IUrlHelper urlHelper
                                    )
        {
            this.urlHelper = urlHelper;
        }

        public void BuildQueryToFilterData(InstructionListsParameters instructionListsParameters, ref IQueryable<InstructionListDto> instructionListsBeforePaging)
        {
            if (!string.IsNullOrEmpty(instructionListsParameters.Name))
            {
                var nameForWhereClause = instructionListsParameters.Name.Trim().ToLowerInvariant();
                instructionListsBeforePaging = instructionListsBeforePaging
                                               .Where(i => i.Name.ToLowerInvariant() == nameForWhereClause);
            }

            if (!string.IsNullOrEmpty(instructionListsParameters.SearchQuery))
            {
                var searchQueryForWhereClause = instructionListsParameters.SearchQuery.Trim().ToLowerInvariant();
                instructionListsBeforePaging = instructionListsBeforePaging
                                               .Where(i => i.Name.ToLowerInvariant().Contains(searchQueryForWhereClause));
            }
        }

        public bool SetOrderBy(ref IQueryable<InstructionListDto> instructionListsBeforePaging, InstructionListsParameters instructionListsParameters)
        {
            string[] orderBy;

            if (!string.IsNullOrEmpty(instructionListsParameters.OrderBy))
            {
                orderBy = instructionListsParameters.OrderBy.Split(',');
            }
            else

            {
                orderBy = new string[] { "Name" };
            }

            foreach (string orderByOption in orderBy)
            {
                if (typeof(InstructionList).GetProperty(orderByOption.Split(" ")[0]) == null)
                {
                    return false;
                }

                instructionListsBeforePaging = instructionListsBeforePaging.OrderBy(orderByOption);
            }

            return true;
        }

        public object getPaginationData(PagedList<InstructionListDto> instructionListsAfterPaging, InstructionListsParameters instructionListsParameters)
        {
            var previousPageLink = instructionListsAfterPaging.HasPrevious ?
                CreateInstructionListResourceUri(instructionListsParameters,
                ResourceUriType.PreviousPage) : null;

            var nextPageLink = instructionListsAfterPaging.HasNext ?
                CreateInstructionListResourceUri(instructionListsParameters,
                ResourceUriType.NextPage) : null;

            var paginationMetadata = new
            {
                previousPageLink = previousPageLink,
                nextPageLink = nextPageLink,
                totalCount = instructionListsAfterPaging.TotalCount,
                pageSize = instructionListsAfterPaging.PageSize,
                currentPage = instructionListsAfterPaging.CurrentPage,
                totalPages = instructionListsAfterPaging.TotalPages
            };

            return paginationMetadata;
        }

        public string CreateInstructionListResourceUri(
           InstructionListsParameters instructionListsParameters,
           ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return urlHelper.Link("GetInstructionsList",
                      new
                      {
                          fields = instructionListsParameters.Fields,
                          orderBy = instructionListsParameters.OrderBy,
                          searchQuery = instructionListsParameters.SearchQuery,
                          name = instructionListsParameters.Name,
                          pageNumber = instructionListsParameters.PageNumber - 1,
                          pageSize = instructionListsParameters.PageSize
                      });
                case ResourceUriType.NextPage:
                    return urlHelper.Link("GetInstructionsList",
                      new
                      {
                          fields = instructionListsParameters.Fields,
                          orderBy = instructionListsParameters.OrderBy,
                          searchQuery = instructionListsParameters.SearchQuery,
                          name = instructionListsParameters.Name,
                          pageNumber = instructionListsParameters.PageNumber + 1,
                          pageSize = instructionListsParameters.PageSize
                      });
                case ResourceUriType.Current:
                default:
                    return urlHelper.Link("GetInstructionLists",
                    new
                    {
                        fields = instructionListsParameters.Fields,
                        orderBy = instructionListsParameters.OrderBy,
                        searchQuery = instructionListsParameters.SearchQuery,
                        name = instructionListsParameters.Name,
                        pageNumber = instructionListsParameters.PageNumber,
                        pageSize = instructionListsParameters.PageSize
                    });
            }
        }
    }
}
