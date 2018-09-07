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

        public LinkGroupItemController(IRepository<LinkGroup> linkGroupRepository,
                                       IRepository<LinkGroupItem> linkGroupItemRepository,
                                       IRepository<Items> itemsRepository,
                                       ILogger<LinkGroupController> logger
                                       )
        {
            this.linkGroupRepository = linkGroupRepository;
            this.linkGroupItemRepository = linkGroupItemRepository;
            this.itemsRepository = itemsRepository;
            this.logger = logger;
        }

        [HttpGet("{linkGroupId}/LinkGroupItem/{linkGroupItemId}", Name = "GetLinkGroupItem")]
        public IActionResult GetLinkGroupItem(int linkGroupId, int linkGroupItemId)
        {
            var linkGroupItem = linkGroupItemRepository.Get(linkGroupItemId);

            if (linkGroupItem == null)
            {
                logger.LogWarning("The object passed is either null or does not contain any rows.");
                return NotFound();
            }

            var linkGroupItemDto = Mapper.Map<LinkGroupItemDto>(linkGroupItem);
            return Ok(linkGroupItemDto);
        }

        [HttpPost("{linkGroupId}/LinkGroupItem", Name = "CreateLinkGroupItem")]
        public IActionResult CreateLinkGroupItem(int linkGroupId, [FromBody]LinkGroupItemDto linkGroupItemDto)
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

        [HttpPost("{linkGroupId}/LinkGroupItems", Name = "CreateLinkGroupItems")]
        public IActionResult CreateLinkGroupItems(int linkGroupId, [FromBody]List<LinkGroupItemDto> linkGroupItemsDto)
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

        [HttpDelete("{linkGroupId}/LinkGroupItem/{linkGroupItemId}", Name = "DeleteLinkGroupItem")]
        public IActionResult DeleteLinkGroupItem(int linkGroupId, int linkGroupItemId)
        {
            var linkGroup = linkGroupRepository.Get(linkGroupId);

            if (linkGroup == null)
            {
                logger.LogWarning("The object passed is either null or does not contain any rows.");
                return NotFound();
            }

            var linkGroupItem = linkGroupItemRepository.Get(linkGroupItemId);

            if (linkGroup.LinkGroupId == linkGroupItem.LinkGroupId)
            {
                linkGroupItemRepository.Delete(linkGroupItem);
            }
            else
            {
                logger.LogWarning("The id's passed does not match.");
                return NotFound();
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

        [HttpDelete("{linkGroupId}/LinkGroupItems", Name = "DeleteLinkGroupItems")]
        public IActionResult DeleteLinkGroupItems(int linkGroupId, [FromBody] List<int> linkGroupItemIds)
        {
            var linkGroup = linkGroupRepository.Get(linkGroupId);

            if (linkGroupItemIds == null)
            {
                return BadRequest();
            }

            if (linkGroup == null)
            {
                logger.LogWarning("The object passed is either null or does not contain any rows.");
                return NotFound();
            }

            var linkGroupItemsToDelete = BuildLinkGroupItemsDeleteQuery(linkGroupItemIds, linkGroupId);

            foreach (var linkGroupItem in linkGroupItemsToDelete.ToList())
            {
                linkGroupItemRepository.Delete(linkGroupItem);
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

        internal IQueryable<LinkGroupItem> BuildLinkGroupItemsDeleteQuery(List<int> linkGroupItemIDs, int linkGroupId)
        {
            return linkGroupItemRepository.FindBy(l => linkGroupItemIDs.Contains(l.LinkGroupId) && l.LinkGroupId == linkGroupId);

        }
    }
}