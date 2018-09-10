using AutoMapper;
using KitBuilderWebApi.DataAccess.Dto;
using KitBuilderWebApi.DataAccess.Repository;
using KitBuilderWebApi.DatabaseModels;
using KitBuilderWebApi.Helper;
using KitBuilderWebApi.QueryParameters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Swashbuckle.AspNetCore.Annotations;

namespace KitBuilderWebApi.Controllers
{ 
    [Route("api/InstructionList")]
    public class InstructionListController : Controller
    {
        private IRepository<InstructionList> instructionListRepository;
        private IRepository<InstructionListMember> instructionListMemberRepository;
        private IRepository<InstructionType> instructionTypeRespository;
        private IRepository<Status> statusRespository;
        private ILogger<InstructionListController> logger;
        private IHelper<InstructionListDto, InstructionListsParameters> instructionListHelper;

        public InstructionListController(IRepository<InstructionList> instructionListRepository,
                                         IRepository<InstructionListMember> instructionListMemberRepository,
                                         IRepository<InstructionType> instructionTypeRespository,
                                         IRepository<Status> statusRespository,
                                         ILogger<InstructionListController> logger,
                                         IHelper<InstructionListDto, InstructionListsParameters> instructionListHelper
                                         )
        {
            this.instructionListRepository = instructionListRepository;
            this.instructionListMemberRepository = instructionListMemberRepository;
            this.instructionTypeRespository = instructionTypeRespository;
            this.statusRespository = statusRespository;
            this.logger = logger;
            this.instructionListHelper = instructionListHelper;
        }


        // GET api/InstructionLists
        /// <summary>
        /// InstructionList - GET
        /// </summary>
        /// <param name="instructionListsParameters"></param>
        [SwaggerResponse(200, "Ok")]
        [SwaggerResponse(400, "Bad Request")]
        [SwaggerResponse(500, "Server Error")]
        [HttpGet(Name = "GetInstructionsList")]
        public IActionResult GetInstructionsList(InstructionListsParameters instructionListsParameters)
        {
            if (!ModelState.IsValid || instructionListsParameters == null)
                return BadRequest(ModelState);


            var instructionListsBeforePaging = from i in instructionListRepository.GetAll()
                                               join itr in instructionTypeRespository.GetAll() on i.InstructionTypeId equals itr.InstructionTypeId
                                               join s in statusRespository.GetAll() on i.StatusId equals s.StatusId
                                               select new InstructionListDto()
                                               {
                                                   InstructionListId = i.InstructionListId,
                                                   Name = i.Name,
                                                   InstructionTypeId = i.InstructionTypeId,
                                                   StatusId = i.StatusId,
                                                   Status = s.StatusCode,
                                                   InstructionTypeName = itr.Name

                                               };
            // will set order by if passed, otherwise use default orderby    
            try
            {
                instructionListsBeforePaging =
                    instructionListHelper.SetOrderBy(instructionListsBeforePaging, instructionListsParameters);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                ModelState.AddModelError("BadOrderBy", "Invalid OrderBy Parameter");
                return BadRequest(ModelState);
            }


            //build the query if any filter or search query critiera is passed
            BuildQueryToFilterData(instructionListsParameters, ref instructionListsBeforePaging);

            // call the static method on the paged list to filter items
            var instructionListsAfterPaging = PagedList<InstructionListDto>.Create(instructionListsBeforePaging,
                                                            instructionListsParameters.PageNumber,
                                                            instructionListsParameters.PageSize
                                                            );

            var paginationMetadata = instructionListHelper.GetPaginationData(instructionListsAfterPaging, instructionListsParameters);

            if (Response != null)
                Response.Headers.Add("X-Pagination",
                    Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            return Ok(instructionListsAfterPaging.ShapeData(instructionListsParameters.Fields));
        }

        /// <summary>
        /// InstructionList - UPDATE
        /// </summary>
        /// <param name="instructionListId"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        [SwaggerResponse(200, "Ok")]
        [SwaggerResponse(400, "Bad Request")]
        [SwaggerResponse(404, "Not Found")]
        [SwaggerResponse(500, "Server Error")]
        [HttpPut("{instructionListId}")]
        public IActionResult UpdateInstructionList([FromRoute]int instructionListId, [FromBody]InstructionListUpdateDto list)
        {
            if (!ModelState.IsValid || list == null)
                return BadRequest(ModelState);


            var existingList = instructionListRepository.Find(i => i.InstructionListId == instructionListId);

            if (existingList == null) return NotFound();

            var instructionList = Mapper.Map<InstructionList>(list);
            instructionList.InstructionListId = existingList.InstructionListId;

            try
            {
                instructionListRepository.Update(instructionList, existingList.InstructionListId);
                instructionListRepository.UnitOfWork.Commit();
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "A problem happened while handling your request.");
            }

 
        }
        /// <summary>
        /// InstructionList - DELETE
        /// </summary>
        /// <param name="instructionListId"></param>
        [SwaggerResponse(200, "Ok")]
        [SwaggerResponse(400, "Bad Request")]
        [SwaggerResponse(404, "Not Found")]
        [SwaggerResponse(500, "Server Error")]
        [HttpDelete("{instructionListId}")]
        public IActionResult DeleteInstructionList([FromRoute]int instructionListId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var list = instructionListRepository.Find(i => i.InstructionListId == instructionListId);

            if (list == null) return NotFound();

            if (InstructionListHasMembers(list)) return NoContent();


            try
            {
                instructionListRepository.Delete(list);
                instructionListRepository.UnitOfWork.Commit();
                return Ok();

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        /// <summary>
        /// InstructionList - ADD
        /// </summary>
        /// <param name="instructionListAddDto"></param>
        /// <response code="201">Created</response>

     
        [SwaggerResponse(201, "Created")]
        [SwaggerResponse(400, "Bad Request")]
        [SwaggerResponse(500, "Server Error")]
        [HttpPost(Name = "AddInstructionList")]
        public IActionResult AddInstructionList([FromBody]InstructionListAddDto instructionListAddDto)
        {
            if (!ModelState.IsValid || instructionListAddDto == null)
                return BadRequest(ModelState);

            var defaultStatus = statusRespository.Find(s => s.StatusCode == "ENA");
            if (defaultStatus == null)
            {
                ModelState.AddModelError("DefaultStatus", "Unable to find 'Enabled' Status");
                logger.LogError("AddInstructionList: Unable to find default status.");
                return BadRequest(ModelState);
            }

            var instructionList = Mapper.Map<InstructionList>(instructionListAddDto);
            instructionList.StatusId = defaultStatus.StatusId;

            instructionListRepository.Add(instructionList);

            try
            {
                instructionListRepository.UnitOfWork.Commit();
                return CreatedAtRoute("AddInstructionList", new { id = instructionList.InstructionListId }, instructionList);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "A problem happened while handling your request.");
            }

          
        }

        internal bool InstructionListHasMembers(InstructionList list)
        {
            return instructionListMemberRepository.FindAll(ilm => ilm.InstructionListId == list.InstructionListId).Any();
            
        }

        internal void BuildQueryToFilterData(InstructionListsParameters instructionListsParameters, ref IQueryable<InstructionListDto> instructionListsBeforePaging)
        {
            if (!string.IsNullOrEmpty(instructionListsParameters.Name))
            {
                var nameForWhereClause = instructionListsParameters.Name.Trim().ToLowerInvariant();
                instructionListsBeforePaging = instructionListsBeforePaging
                    .Where(i => i.Name.ToLowerInvariant() == nameForWhereClause);
            }

            if (!string.IsNullOrEmpty(instructionListsParameters.SearchNameQuery))
            {
                var searchQueryForWhereClause = instructionListsParameters.SearchNameQuery.Trim().ToLowerInvariant();
                instructionListsBeforePaging = instructionListsBeforePaging
                    .Where(i => i.Name.ToLowerInvariant().Contains(searchQueryForWhereClause));
            }
        }

    }

}