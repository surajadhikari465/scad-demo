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
    public class LinkGroupItemHelper
    {
        private IUrlHelper urlHelper;
        private IRepository<LinkGroupItem> linkGroupItemRepository { get; set; }
        private IRepository<Items> itemsRepository { get; set; }
        private IRepository<LinkGroup> linkGroupRepository { get; set; }
        private IRepository<KitLinkGroupItem> kitlinkGroupItemRepository { get; set; }
        private IRepository<KitLinkGroup> kitlinkGroupRepository { get; set; }

       internal bool DeleteLinkGroupItems(int[] linkGroupItemIDs)
        {
           

        }
    }
}
