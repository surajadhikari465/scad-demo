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

namespace KitBuilderWebApi.Controllers
{
    //[Authorize]
    [Route("api/InstructionList")]
    public class InstructionListController : Controller
    {
        private IRepository<InstructionList> instructionListRepository { get; set; }
        private IRepository<InstructionListMember> instructionListMemberRepository { get; set; }
        private IRepository<InstructionType> instructionTypeRespository { get; set; }
        private IRepository<Status> statusRespository { get; set; }
        private ILogger<InstructionListController> logger;
        private InstructionListHelper instructionListHelper;

        public InstructionListController(IRepository<InstructionList> instructionListRepository,
                                         IRepository<InstructionListMember> instructionListMemberRepository,
                                         IRepository<InstructionType> instructionTypeRespository,
                                          IRepository<Status> statusRespository,
                                         ILogger<InstructionListController> logger,
                                         InstructionListHelper instructionListHelper
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
        [HttpGet(Name = "GetInstructionsList")]
        public IActionResult GetInstructionsList(InstructionListsParameters instructionListsParameters)
        {

            var instructionListsBeforePaging = from i in instructionListRepository.GetAll()
                                               join  itr in instructionTypeRespository.GetAll() on i.InstructionTypeId equals itr.InstructionTypeId
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
            if (!instructionListHelper.SetOrderBy(ref instructionListsBeforePaging, instructionListsParameters))
                return BadRequest();

            //build the query if any filter or search query critiera is passed
            instructionListHelper.BuildQueryToFilterData(instructionListsParameters, ref instructionListsBeforePaging);

            // call the static method on the paged list to filter items
            var instructionListsAfterPaging = PagedList<InstructionListDto>.Create(instructionListsBeforePaging,
                                                            instructionListsParameters.PageNumber,
                                                            instructionListsParameters.PageSize
                                                            );

            var paginationMetadata = instructionListHelper.getPaginationData(instructionListsAfterPaging, instructionListsParameters);

            Response.Headers.Add("X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            return Ok(instructionListsAfterPaging.ShapeData(instructionListsParameters.Fields));
        }
    }
}