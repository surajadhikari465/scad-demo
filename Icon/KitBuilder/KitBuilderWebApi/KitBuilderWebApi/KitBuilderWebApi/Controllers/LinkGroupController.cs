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

namespace KitBuilderWebApi.Controllers
{
    [Route("api/LinkGroups")]
    public class LinkGroupController : Controller
    {
        private IRepository<LinkGroup> linkGroupRepository { get; set; }
        private IRepository<LinkGroupItem> linkGroupItemRepository { get; set; }
        private IRepository<Items> itemsRepository { get; set; }
        private IRepository<Status> statusRespository { get; set; }
        private ILogger<LinkGroupController> logger;
        private LinkGroupHelper linkGroupHelper;
        private const string deleteLinkGroupSpName = "DeleteLinkGroup";

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
            this.statusRespository = statusRespository;
            this.logger = logger;
            this.linkGroupHelper = linkGroupHelper;
        }

        // GET api/GetLinkGroups
        [HttpGet(Name = "GetLinkGroups")]
        public IActionResult GetLinkGroups(LinkGroupParameters linkGroupParameters)
        {

            var linkGroupListBeforePaging = from l in linkGroupRepository.GetAll()
                                            select new LinkGroupDto()
                                            {
                                                LinkGroupId = l.LinkGroupId,
                                                GroupName = l.GroupName,
                                                GroupDescription = l.GroupDescription,
                                                InsertDate = l.InsertDate
                                            };

            // will set order by if passed, otherwise use default orderby                           
            if (!linkGroupHelper.SetOrderBy(ref linkGroupListBeforePaging, linkGroupParameters))
                return BadRequest();

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
                return NotFound();
            }

            if (!linkGroupHelper.IsLinkGroupUsedbyKit(id))
            {
                var param1 = new SqlParameter("linkGroupid", SqlDbType.BigInt) { Value = id };
                linkGroupRepository.ExecWithStoreProcedure(deleteLinkGroupSpName + " @linkGroupid", param1);
            }
            else
            {
                return StatusCode(StatusCodes.Status409Conflict);
            }

            try
            {
                linkGroupRepository.UnitOfWork.Commit();
                return NoContent();
            }
            catch (Exception ex)
            {

                return StatusCode(500, "A problem happened while handling your request.");
            }
        }
    }
}