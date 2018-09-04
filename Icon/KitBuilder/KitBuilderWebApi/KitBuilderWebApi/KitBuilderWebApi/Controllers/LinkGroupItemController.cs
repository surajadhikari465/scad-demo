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
        private ILogger<LinkGroupController> logger;
        private LinkGroupItemHelper linkGroupItemHelper;

        public LinkGroupItemController(ILogger<LinkGroupController> logger,
                                       LinkGroupItemHelper linkGroupItemHelper
                                      )
        {
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

            var linkGroup = linkGroupItemHelper.GetLinkGroupById(linkGroupId);

            if (linkGroup == null)
            {
                logger.LogWarning("The object passed is either null or does not contain any rows.");
                return NotFound();
            }

            var linkGroupItem = Mapper.Map<LinkGroupItem>(linkGroupItemDto);

            try
            {
                linkGroupItemHelper.AddLinkGroupItemToLinkGroup(linkGroup, linkGroupItem);
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

            var linkGroup = linkGroupItemHelper.GetLinkGroupById(linkGroupId);

            if (linkGroup == null)
            {
                logger.LogWarning("The object passed is either null or does not contain any rows.");
                return NotFound();
            }

            try
            {
                linkGroupItemHelper.AddLinkGroupItemsToLinkGroup(linkGroup, linkGroupItemsDto);
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
            var linkGroup = linkGroupItemHelper.GetLinkGroupById(linkGroupId);

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
           
            try
            {
                linkGroupItemHelper.DeleteLinkGroupItems(linkGroup, linkGroupItemsToDelete);

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