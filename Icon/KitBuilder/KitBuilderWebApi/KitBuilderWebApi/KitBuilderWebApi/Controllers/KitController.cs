using System.Linq;
using AutoMapper;
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
using KitBuilderWebApi.DataAccess.Dto;
using Microsoft.EntityFrameworkCore;

namespace KitBuilderWebApi.Controllers
{
    [Route("api/Kits")]
    public class KitController : Controller
    {
        private IRepository<LinkGroup> linkGroupRepository;
        private IRepository<Kit> kitRepository;
        private IRepository<KitLinkGroup> kitLinkGroupRepository;
        private IRepository<KitLocale> kitLocaleRepository;
        private IRepository<LinkGroupItem> linkGroupItemRepository;
        private IRepository<Items> itemsRepository;
        private IRepository<KitLinkGroup> kitlinkGroupRepository;
        private ILogger<LinkGroupController> logger;

        public KitController(IRepository<LinkGroup> linkGroupRepository,
                             IRepository<LinkGroupItem> linkGroupItemRepository,
                             IRepository<Items> itemsRepository,
                             IRepository<KitLinkGroup> kitlinkGroupRepository,
                             ILogger<LinkGroupController> logger,
                             IHelper<LinkGroupDto, LinkGroupParameters> linkGroupHelper
                            )
        {
            this.linkGroupRepository = linkGroupRepository;
            this.linkGroupItemRepository = linkGroupItemRepository;
            this.itemsRepository = itemsRepository;
            this.kitlinkGroupRepository = kitlinkGroupRepository;
            this.logger = logger;
        }


        [HttpPost()]
        public IActionResult AssignLocations(
           [FromBody] LinkGroupDto linkGroup)
        {
            if (linkGroup == null)
            {
                logger.LogWarning("The object passed is either null or does not contain any rows.");
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var linkGroupPassed = Mapper.Map<LinkGroup>(linkGroup);
            linkGroupRepository.Add(linkGroupPassed);

            try
            {
                linkGroupRepository.UnitOfWork.Commit();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "A problem happened while handling your request.");
            }

            var createdlinkOfGroup = Mapper.Map<LinkGroupDto>(linkGroupPassed);

            return CreatedAtRoute("GetLinkGroupById", new
            { id = createdlinkOfGroup.LinkGroupId }, createdlinkOfGroup);
        }
    }
}