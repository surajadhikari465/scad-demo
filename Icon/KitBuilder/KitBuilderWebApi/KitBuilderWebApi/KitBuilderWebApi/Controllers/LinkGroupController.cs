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

namespace KitBuilderWebApi.Controllers
{
    [Route("api/LinkGroups")]
    public class LinkGroupController : Controller
    {
        private IRepository<LinkGroup> linkGroupRepository;
        private IRepository<LinkGroupItem> linkGroupItemRepository;
        private IRepository<Items> itemsRepository;
        private ILogger<LinkGroupController> logger;
        private LinkGroupHelper linkGroupHelper;
        private const string deleteLinkGroupSpName = "DeleteLinkGroupByLinkGroupId";

        public LinkGroupController(IRepository<LinkGroup> linkGroupRepository,
                                   IRepository<LinkGroupItem> linkGroupItemRepository,
                                   IRepository<Items> itemsRepository,
                                   ILogger<LinkGroupController> logger,
                                   LinkGroupHelper linkGroupHelper
                                  )
        {
            this.linkGroupRepository = linkGroupRepository;
            this.linkGroupItemRepository = linkGroupItemRepository;
            this.itemsRepository = itemsRepository;
            this.logger = logger;
            this.linkGroupHelper = linkGroupHelper;
        }

        // GET api/GetLinkGroups
        [HttpGet(Name = "GetLinkGroups")]
        public IActionResult GetLinkGroups(LinkGroupParameters linkGroupParameters)
        {

            var linkGroupListBeforePaging = linkGroupHelper.GetlinkGroupBeforePagingQuery();

            // will set order by if passed, otherwise use default orderby                           
            if (!linkGroupHelper.SetOrderBy(ref linkGroupListBeforePaging, linkGroupParameters))
            {
                logger.LogWarning("The object passed is either null or does not contain any rows.");
                return BadRequest();
            }
              

            //build the query if any filter or search query critiera is passed
            linkGroupHelper.BuildQueryToFilterData(linkGroupParameters, ref linkGroupListBeforePaging);

            // call the static method on the paged list to filter items
            var instructionListsAfterPaging = PagedList<LinkGroupDto>.Create(linkGroupListBeforePaging,
                                                            linkGroupParameters.PageNumber,
                                                            linkGroupParameters.PageSize
                                                            );

            var paginationMetadata = linkGroupHelper.getPaginationData(instructionListsAfterPaging, linkGroupParameters);

            Response.Headers.Add("X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            return Ok(instructionListsAfterPaging.ShapeData(linkGroupParameters.Fields));
        }


        // GET api/GetLinkGroupById
        [HttpGet("{id}", Name = "GetLinkGroupById")]
        public IActionResult GetLinkGroupById(int id, bool loadChildObjects)
        {
            var linkGroupQuery = linkGroupHelper.BuildLinkGroupByItemIdQuery(id, loadChildObjects);
            var linkGroup = linkGroupQuery.FirstOrDefault();

            if (linkGroup == null)
            {
                logger.LogWarning("The object passed is either null or does not contain any rows.");
                return NotFound();
            }

            LinkGroupDto linkGroupDto = Mapper.Map<LinkGroupDto>(linkGroup);

            return Ok(linkGroupDto);
        }

        [HttpPost()]
        public IActionResult CreateLinkGroup(
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

            try
            {
                linkGroupHelper.CreateLinkGroup(linkGroupPassed);
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

        [HttpDelete("{id}", Name = "DeleteLinkGroup")]
        public IActionResult DeleteLinkGroup(int id)
        {
            var linkGroupToDelete = linkGroupHelper.BuildLinkGroupByItemIdQuery(id, false).FirstOrDefault();

            if (linkGroupToDelete == null)
            {
                logger.LogWarning("The object passed is either null or does not contain any rows.");
                return NotFound();
            }

            if (!linkGroupHelper.IsLinkGroupUsedbyKit(id))
            {
                try
                {
                    linkGroupHelper.DeleteLinkGroup(id, deleteLinkGroupSpName);
                    return NoContent();
                }

                catch (Exception ex)
                {
                    logger.LogError(ex.Message);
                    return StatusCode(500, "A problem happened while handling your request.");
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status409Conflict);
            }
           
        }
    }
}