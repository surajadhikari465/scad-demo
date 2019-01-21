using System.Linq;
using AutoMapper;
using KitBuilderWebApi.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using KitBuilderWebApi.QueryParameters;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Data;
using System.Data.SqlClient;
using KitBuilder.DataAccess.DatabaseModels;
using KitBuilder.DataAccess.Dto;
using KitBuilder.DataAccess.Repository;
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
        private const string DeleteLinkGroupSpName = "DeleteLinkGroupByLinkGroupId";
        private const string LinkGroupSearchSpName = "LinkGroupsSearch";

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



        
        [HttpGet("LinkGroupsSearch")]
        public IActionResult LinkGroupSearch(LinkGroupSearchParamters parameters)
        {
            // get superset of data.
            var query = from lg in linkGroupRepository.GetAll() select lg;

            // filter based on parameters
            if (!string.IsNullOrEmpty(parameters.LinkGroupName))
                query = query.Where(q => q.GroupName.Contains(parameters.LinkGroupName));

            if (!string.IsNullOrEmpty(parameters.LinkGroupDesc))
                query = query.Where(q => q.GroupDescription.Contains(parameters.LinkGroupDesc));

            if (!string.IsNullOrEmpty(parameters.ModifierDesc))
                query = from q in query
                    join lgi in linkGroupItemRepository.GetAll() on q.LinkGroupId equals lgi.LinkGroupId
                    join i in itemsRepository.GetAll() on lgi.ItemId equals i.ItemId
                    where i.CustomerFriendlyDesc.Contains(parameters.ModifierDesc)
                    select q;

            if (!string.IsNullOrEmpty(parameters.ModifierPlu))
                query = from q in query
                    join lgi in linkGroupItemRepository.GetAll() on q.LinkGroupId equals lgi.LinkGroupId
                    join i in itemsRepository.GetAll() on lgi.ItemId equals i.ItemId
                    where i.ScanCode.Contains(parameters.ModifierPlu)
                    select q;

            // get ids for filtered results.
            var linkGroupIds = query.Select(q => q.LinkGroupId).Distinct();

            // get display data based on ids
            var data = (from lg in linkGroupRepository.GetAll()
                        where linkGroupIds.Contains(lg.LinkGroupId)
                       select new
                       {
                           LinkGroupId = lg.LinkGroupId,
                           GroupName = lg.GroupName,
                           GroupDescription = lg.GroupDescription
                       }).ToList();
   
            // return to client
            return Ok(data);
            
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
                                                InsertDateUtc = l.InsertDateUtc, 
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
            var linkGroupsAfterPaging = PagedList<LinkGroupDto>.Create(linkGroupListBeforePaging,
                                                            linkGroupParameters.PageNumber,
                                                            linkGroupParameters.PageSize
                                                            );
            
            var paginationMetadata = linkGroupHelper.GetPaginationData(linkGroupsAfterPaging, linkGroupParameters);

            Response.Headers.Add("X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            return Ok(linkGroupsAfterPaging.ShapeData(linkGroupParameters.Fields));
        }

        // GET api/GetLinkGroupById
        [HttpGet("{id}/{loadChildObjects}", Name = "GetLinkGroupById")]
        public IActionResult GetLinkGroupById(int id, bool loadChildObjects = false)
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
            linkGroupPassed.InsertDateUtc = DateTime.UtcNow;
            linkGroupPassed.LastUpdatedDateUtc = DateTime.UtcNow;

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

            var result = CreatedAtAction("CreateLinkGroup", new { id = createdlinkOfGroup.LinkGroupId }, createdlinkOfGroup);

            return result;
        }

        [HttpPut("{id}")]
        public IActionResult UpdateLinkGroup(
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

            var existingLinkGroup = linkGroupRepository.GetAll()
                                    .Where(l => l.LinkGroupId == linkGroupPassed.LinkGroupId).FirstOrDefault();

            if (existingLinkGroup == null)
            {
                logger.LogWarning("The object does not exists in the database.");
                return BadRequest();
            }

            try
            {
                existingLinkGroup.GroupName = linkGroupPassed.GroupName;
                existingLinkGroup.GroupDescription = linkGroupPassed.GroupDescription;
                existingLinkGroup.LastUpdatedDateUtc = DateTime.UtcNow;
                linkGroupRepository.UnitOfWork.Commit();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return StatusCode(StatusCodes.Status204NoContent);
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
                    linkGroupRepository.ExecWithStoreProcedure(DeleteLinkGroupSpName + " @linkGroupid", param1);

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