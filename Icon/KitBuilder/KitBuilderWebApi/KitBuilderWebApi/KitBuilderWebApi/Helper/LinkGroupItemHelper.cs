using KitBuilderWebApi.DataAccess.Dto;
using KitBuilderWebApi.DataAccess.Repository;
using KitBuilderWebApi.DatabaseModels;
using KitBuilderWebApi.QueryParameters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace KitBuilderWebApi.Helper
{
    public class LinkGroupItemHelper
    {
        private IUrlHelper urlHelper;
        private IRepository<LinkGroupItem> linkGroupItemRepository;
        private IRepository<Items> itemsRepository;
        private IRepository<LinkGroup> linkGroupRepository;
        private IRepository<KitLinkGroupItem> kitlinkGroupItemRepository;
        private IRepository<KitLinkGroup> kitlinkGroupRepository; 

       internal IQueryable<LinkGroupItem> BuildLinkGroupItemsDeleteQuery(List<int> linkGroupItemIDs)
        {
            return linkGroupRepository.UnitOfWork.Context.LinkGroupItem.Where(l => linkGroupItemIDs.Contains(l.LinkGroupItemId));

        }
    }
}
