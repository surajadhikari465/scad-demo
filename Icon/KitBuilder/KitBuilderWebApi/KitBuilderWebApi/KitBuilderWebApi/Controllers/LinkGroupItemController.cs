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
using System;
using System.Net;

namespace KitBuilderWebApi.Controllers
{
    [Route("api/LinkGroups")]
    public class LinkGroupItemController : Controller
    {
        private IRepository<LinkGroup> linkGroupRepository;
        private IRepository<LinkGroupItem> linkGroupItemRepository;
        private IRepository<Items> itemsRepository;
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
            this.logger = logger;
            this.linkGroupItemHelper = linkGroupItemHelper;
        }

       // GET api/GetLinkGroups
        [HttpPost("{linkGroupId}/LinkGroupItem", Name = "CreateLinkGroupItem")]
        public IActionResult CreateLinkGroupItem(int linkGroupId, LinkGroupItemDto linkGroupItemDto)
        {
            if (linkGroupItemDto == null)
            {
                return BadRequest();
            }

            var linkGroup = linkGroupRepository.Get(linkGroupId);

            if (linkGroup == null)
            {
                logger.LogWarning("The object passed is either null or does not contain any rows.");
                return NotFound();
            }

            var linkGroupItem = Mapper.Map<LinkGroupItem>(linkGroupItemDto);

            linkGroup.LinkGroupItem.Add(linkGroupItem);

            try
            {

                linkGroupRepository.UnitOfWork.Commit();
                return StatusCode((int)HttpStatusCode.Created);
            }

            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        // GET api/GetLinkGroups
        [HttpPost("linkGroupId}/LinkGroupItems", Name = "CreateLinkGroupItems")]
        public IActionResult CreateLinkGroupItems(int linkGroupId, List<LinkGroupItemDto> linkGroupItemsDto)
        {
            if (linkGroupItemsDto == null)
            {
                return BadRequest();
            }

            var linkGroup = linkGroupRepository.Get(linkGroupId);

            if (linkGroup == null)
            {
                logger.LogWarning("The object passed is either null or does not contain any rows.");
                return NotFound();
            }

            foreach (var linkGroupItemDto in linkGroupItemsDto)
            {
                var linkGroupItem = Mapper.Map<LinkGroupItem>(linkGroupItemDto);
                linkGroup.LinkGroupItem.Add(linkGroupItem);
            }

            try
            {

                linkGroupRepository.UnitOfWork.Commit();
                return StatusCode((int)HttpStatusCode.Created);
            }

            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        // GET api/GetLinkGroups
        [HttpDelete("{linkGroupId}/LinkGroupItems", Name = "GetLinkGroups")]
        public IActionResult DeleteLinkGroupItems(int linkGroupId, List<int> linkGroupItemIDs)
        {
            var linkGroup = linkGroupRepository.Get(linkGroupId);

            if (linkGroupItemIDs == null)
            {
                return BadRequest();
            }

            if (linkGroup == null)
            {
                logger.LogWarning("The object passed is either null or does not contain any rows.");
                return NotFound();
            }

            var linkGroupItemsToDelete = linkGroupItemHelper.BuildLinkGroupItemsDeleteQuery(linkGroupItemIDs);

            foreach (var linkGroupItem in linkGroupItemsToDelete)
            {
                linkGroup.LinkGroupItem.Remove(linkGroupItem);
            }

            try
            {

                linkGroupItemRepository.UnitOfWork.Commit();
                return NoContent();
            }

            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }
    }
}