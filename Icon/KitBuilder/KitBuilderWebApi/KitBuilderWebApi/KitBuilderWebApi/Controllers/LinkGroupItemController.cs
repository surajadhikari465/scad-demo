using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using KitBuilderWebApi.DataAccess.Dto;
using KitBuilderWebApi.DataAccess.Repository;
using KitBuilderWebApi.DatabaseModels;
using KitBuilderWebApi.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq.Dynamic.Core;
using KitBuilderWebApi.QueryParameters;
using System;
using System.Net;
using Microsoft.AspNetCore.Http;
using System.Data;
using System.Data.SqlClient;

namespace KitBuilderWebApi.Controllers
{
    [Route("api/LinkGroups")]
    public class LinkGroupItemController : Controller
    {
        private IRepository<LinkGroup> linkGroupRepository { get; set; }
        private IRepository<LinkGroupItem> linkGroupItemRepository { get; set; }
        private IRepository<Items> itemsRepository { get; set; }
        private IRepository<Status> statusRespository { get; set; }
        private ILogger<LinkGroupController> logger;
        private LinkGroupItemHelper linkGroupItemHelper;

        public LinkGroupItemController(IRepository<LinkGroup> linkGroupRepository,
                                   IRepository<LinkGroupItem> linkGroupItemRepository,
                                   IRepository<Items> itemsRepository,
                                   ILogger<LinkGroupController> logger,
                                   LinkGroupItemHelper linkGroupItemHelper
                                  )
        {
            this.linkGroupRepository = linkGroupRepository;
            this.linkGroupItemRepository = linkGroupItemRepository;
            this.itemsRepository = itemsRepository;
            this.statusRespository = statusRespository;
            this.logger = logger;
            this.linkGroupItemHelper = linkGroupItemHelper;
        }

        // GET api/GetLinkGroups
        [HttpDelete("{LinkGroupId}/LinkGroupItems",Name = "GetLinkGroups")]
        public IActionResult DeleteLinkGroupItems(int[] linkGroupItemIDs)
        {
            linkGroupRepository.UnitOfWork.Context.LinkGroupItem.Where(l=> linkGroupItemIDs.Contains()))
           
        }      
    }
}