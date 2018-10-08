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
    [Route("api/LinkGroups")]
    public class LinkGroupController : Controller
    {
        private IRepository<LinkGroup> linkGroupRepository;
        private IRepository<LinkGroupItem> linkGroupItemRepository;
        private IRepository<Items> itemsRepository;
        private IRepository<KitLinkGroup> kitlinkGroupRepository;
        private ILogger<LinkGroupController> logger;
        private IHelper<LinkGroupDto, LinkGroupParameters> linkGroupHelper;
        private const string deleteLinkGroupSpName = "DeleteLinkGroupByLinkGroupId";

        public LinkGroupController(IRepository<LinkGroup> linkGroupRepository,
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
                                                InsertDateUtc = l.InsertDateUtc
                                            };

            // will set order by if passed, otherwise use default orderby    
            try
            {
                linkGroupListBeforePaging = linkGroupHelper.SetOrderBy( linkGroupListBeforePaging, linkGroupParameters);
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest();
            }
            
            //build the query if any filter or search query critiera is passed
            BuildQueryToFilterData(linkGroupParameters, ref linkGroupListBeforePaging);

            // call the static method on the paged list to filter items
            var instructionListsAfterPaging = PagedList<LinkGroupDto>.Create(linkGroupListBeforePaging,
                                                            linkGroupParameters.PageNumber,
                                                            linkGroupParameters.PageSize
                                                            );

            var paginationMetadata = linkGroupHelper.GetPaginationData(instructionListsAfterPaging, linkGroupParameters);

            Response.Headers.Add("X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            return Ok(instructionListsAfterPaging.ShapeData(linkGroupParameters.Fields));
        }

        // GET api/GetLinkGroupById
        [HttpGet("{id}", Name = "GetLinkGroupById")]
        public IActionResult GetLinkGroupById(int id, bool loadChildObjects)
        {
            var linkGroupQuery = BuildLinkGroupByItemIdQuery(id, loadChildObjects);
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

        [HttpDelete("{id}", Name = "DeleteLinkGroup")]
        public IActionResult DeleteLinkGroup(int id)
        {
            var linkGroupToDelete = BuildLinkGroupByItemIdQuery(id, false).FirstOrDefault();

            if (linkGroupToDelete == null)
            {
                logger.LogWarning("The object passed is either null or does not contain any rows.");
                return NotFound();
            }

            if (!IsLinkGroupUsedbyKit(id))
            {
                try
                {
                    var param1 = new SqlParameter("linkGroupid", SqlDbType.BigInt) { Value = id };
                    linkGroupRepository.ExecWithStoreProcedure(deleteLinkGroupSpName + " @linkGroupid", param1);

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

        internal bool IsLinkGroupUsedbyKit(int linkGroupId)
        {
            var query = from l in linkGroupRepository.GetAll().Where(l => l.LinkGroupId == linkGroupId)
                        join lgi in kitlinkGroupRepository.GetAll() on l.LinkGroupId equals lgi.LinkGroupId
                        select l;

            return query.Any();

        }

        internal IQueryable<LinkGroup> BuildLinkGroupByItemIdQuery(int id, bool loadChildObjects)
        {
            if (loadChildObjects)
            {
                return linkGroupRepository.UnitOfWork.Context.LinkGroup
                            .Where(l => l.LinkGroupId == id)
                            .Include(l => l.LinkGroupItem).ThenInclude(p => p.Item)
                            .Include(l => l.LinkGroupItem).ThenInclude(p => p.InstructionList);
            }

            else
            {
                return linkGroupRepository.GetAll().Where(l => l.LinkGroupId == id);
            }
        }

        internal void BuildQueryToFilterData(LinkGroupParameters linkGroupParameters, ref IQueryable<LinkGroupDto> linkGroupBeforePaging)
        {
            if (!string.IsNullOrEmpty(linkGroupParameters.GroupName))
            {
                var nameForWhereClause = linkGroupParameters.GroupName.Trim().ToLower();
                linkGroupBeforePaging = linkGroupBeforePaging
                                               .Where(i => i.GroupName.ToLower() == nameForWhereClause);
            }

            if (!string.IsNullOrEmpty(linkGroupParameters.ScanCode))
            {
                var scanCodeForWhereClause = linkGroupParameters.ScanCode.Trim().ToLower();
                linkGroupBeforePaging = from l in linkGroupBeforePaging
                                        join lgi in linkGroupItemRepository.GetAll() on l.LinkGroupId equals lgi.LinkGroupId
                                        join i in itemsRepository.GetAll() on lgi.ItemId equals i.ItemId
                                        where i.ScanCode.ToLower().Contains(scanCodeForWhereClause)
                                        select l;
            }

            if (!string.IsNullOrEmpty(linkGroupParameters.SearchGroupNameQuery))
            {
                var searchQueryForWhereClause = linkGroupParameters.SearchGroupNameQuery.Trim().ToLower();
                linkGroupBeforePaging = linkGroupBeforePaging
                                               .Where(i => i.GroupName.ToLower().Contains(searchQueryForWhereClause));
            }
        }
    }
}