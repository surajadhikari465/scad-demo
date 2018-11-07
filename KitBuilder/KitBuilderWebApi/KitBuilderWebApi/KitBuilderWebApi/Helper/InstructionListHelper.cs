using System;
using KitBuilderWebApi.QueryParameters;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Linq.Dynamic.Core;
using KitBuilder.DataAccess.DatabaseModels;
using KitBuilder.DataAccess.Dto;

namespace KitBuilderWebApi.Helper
{
    public class InstructionListHelper : IHelper<InstructionListDto, InstructionListsParameters>
    {
        private IUrlHelper urlHelper;

        public InstructionListHelper( IUrlHelper urlHelper
                                    )
        {
            this.urlHelper = urlHelper;
        }



        public IQueryable<InstructionListDto> SetOrderBy(IQueryable<InstructionListDto> DataBeforePaging, InstructionListsParameters Parameters)
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
                    orderBy = new string[] { "Name" };
                }

                foreach (string orderByOption in orderBy.Reverse())
                {
                    if (typeof(InstructionList).GetProperty(orderByOption.Split(" ")[0]) == null)
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

        public object GetPaginationData(PagedList<InstructionListDto> instructionListsAfterPaging, InstructionListsParameters instructionListsParameters)
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
                          searchQuery = instructionListsParameters.SearchNameQuery,
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
                          searchQuery = instructionListsParameters.SearchNameQuery,
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
                        searchQuery = instructionListsParameters.SearchNameQuery,
                        name = instructionListsParameters.Name,
                        pageNumber = instructionListsParameters.PageNumber,
                        pageSize = instructionListsParameters.PageSize
                    });
            }
        }
    }
}
