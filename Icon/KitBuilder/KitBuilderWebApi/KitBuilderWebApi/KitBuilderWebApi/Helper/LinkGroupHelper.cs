using KitBuilderWebApi.DataAccess.Dto;
using KitBuilderWebApi.DataAccess.Repository;
using KitBuilderWebApi.DatabaseModels;
using KitBuilderWebApi.QueryParameters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace KitBuilderWebApi.Helper
{
    public class LinkGroupHelper
    {
        private IUrlHelper urlHelper;
        private IRepository<LinkGroupItem> linkGroupItemRepository;
        private IRepository<Items> itemsRepository;
        private IRepository<LinkGroup> linkGroupRepository;
        private IRepository<KitLinkGroupItem> kitlinkGroupItemRepository;
        private IRepository<KitLinkGroup> kitlinkGroupRepository;

        public LinkGroupHelper( IUrlHelper urlHelper,
                                IRepository<LinkGroup> linkGroupRepository,
                                IRepository<LinkGroupItem> linkGroupItemRepository,
                                IRepository<Items> itemsRepository,
                                IRepository<KitLinkGroupItem> kitlinkGroupItemRepository,
                                IRepository<KitLinkGroup> kitlinkGroupRepository
                                    )
        {
            this.urlHelper = urlHelper;
            this.linkGroupItemRepository = linkGroupItemRepository;
            this.itemsRepository = itemsRepository;
            this.linkGroupRepository = linkGroupRepository;
            this.kitlinkGroupItemRepository = kitlinkGroupItemRepository;
            this.kitlinkGroupRepository = kitlinkGroupRepository;
        }

        internal void BuildQueryToFilterData(LinkGroupParameters linkGroupParameters, ref IQueryable<LinkGroupDto> linkGroupBeforePaging)
        {
            if (!string.IsNullOrEmpty(linkGroupParameters.GroupName))
            {
                var nameForWhereClause = linkGroupParameters.GroupName.Trim().ToLowerInvariant();
                linkGroupBeforePaging = linkGroupBeforePaging
                                               .Where(i => i.GroupName.ToLowerInvariant() == nameForWhereClause);
            }

            if (!string.IsNullOrEmpty(linkGroupParameters.ScanCode))
            {
                var scanCodeForWhereClause = linkGroupParameters.ScanCode.Trim().ToLowerInvariant();
                linkGroupBeforePaging = from l in linkGroupBeforePaging
                                        join lgi in linkGroupItemRepository.GetAll() on l.LinkGroupId equals lgi.LinkGroupId
                                        join i in itemsRepository.GetAll() on lgi.ItemId equals i.ItemId
                                        where i.ScanCode.ToLowerInvariant().Contains(scanCodeForWhereClause)
                                        select l;
            }

            if (!string.IsNullOrEmpty(linkGroupParameters.SearchGroupNameQuery))
            {
                var searchQueryForWhereClause = linkGroupParameters.SearchGroupNameQuery.Trim().ToLowerInvariant();
                linkGroupBeforePaging = linkGroupBeforePaging
                                               .Where(i => i.GroupName.ToLowerInvariant().Contains(searchQueryForWhereClause));
            }
        }

        internal bool SetOrderBy(ref IQueryable<LinkGroupDto> LinkGroupBeforePaging, LinkGroupParameters LinkGroupParameters)
        {
            string[] orderBy;

            if (!string.IsNullOrEmpty(LinkGroupParameters.OrderBy))
            {
                orderBy = LinkGroupParameters.OrderBy.Split(',');
            }
            else

            {
                orderBy = new string[] { "GroupName" };
            }

            foreach (string orderByOption in orderBy)
            {
                if (typeof(InstructionList).GetProperty(orderByOption.Split(" ")[0]) == null)
                {
                    return false;
                }

                LinkGroupBeforePaging = LinkGroupBeforePaging.OrderBy(orderByOption);
            }

            return true;
        }

        internal object getPaginationData(PagedList<LinkGroupDto> LinkGroupAfterPaging, LinkGroupParameters LinkGroupParameters)
        {
            var previousPageLink = LinkGroupAfterPaging.HasPrevious ?
                CreateInstructionListResourceUri(LinkGroupParameters,
                ResourceUriType.PreviousPage) : null;

            var nextPageLink = LinkGroupAfterPaging.HasNext ?
                CreateInstructionListResourceUri(LinkGroupParameters,
                ResourceUriType.NextPage) : null;

            var paginationMetadata = new
            {
                previousPageLink = previousPageLink,
                nextPageLink = nextPageLink,
                totalCount = LinkGroupAfterPaging.TotalCount,
                pageSize = LinkGroupAfterPaging.PageSize,
                currentPage = LinkGroupAfterPaging.CurrentPage,
                totalPages = LinkGroupAfterPaging.TotalPages
            };

            return paginationMetadata;
        }

        internal IQueryable<LinkGroup> BuildLinkGroupByItemIdQuery(int id, bool loadChildObjects)
        {
            if (loadChildObjects)
            {
                return linkGroupRepository.UnitOfWork.Context.LinkGroup
                            .IncludeMultiple(l => l.LinkGroupItem, l => l.LinkGroupItem.Select(o => o.Item), l => l.LinkGroupItem.Select(o => o.InstructionList));

            }

            else
            {
                return linkGroupRepository.GetAll().Where(l => l.LinkGroupId == id);
            }
        }

        internal string CreateInstructionListResourceUri(
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

        internal bool IsLinkGroupUsedbyKit(int linkGroupId)
        {
            var query = from l in linkGroupRepository.GetAll().Where(l => l.LinkGroupId == linkGroupId)
                        join lgi in kitlinkGroupRepository.GetAll() on l.LinkGroupId equals lgi.LinkGroupId
                        select l;

            return query.Any();

        }

        internal IQueryable<LinkGroupDto> GetlinkGroupBeforePagingQuery()
        {
            var linkGroupListBeforePaging = from l in linkGroupRepository.GetAll()
                                            select new LinkGroupDto()
                                            {
                                                LinkGroupId = l.LinkGroupId,
                                                GroupName = l.GroupName,
                                                GroupDescription = l.GroupDescription,
                                                InsertDate = l.InsertDate
                                            };

            return linkGroupListBeforePaging;
        }

        internal void CreateLinkGroup(LinkGroup linkGroup)
        {
            linkGroupRepository.Add(linkGroup);
            linkGroupRepository.UnitOfWork.Commit();
        }

        internal void DeleteLinkGroup(int linkGroupId, string spName)
        {
            var param1 = new SqlParameter("linkGroupid", SqlDbType.BigInt) { Value = linkGroupId };

            linkGroupRepository.ExecWithStoreProcedure(spName + " @linkGroupid", param1);
        }
    }
}