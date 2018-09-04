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
        private IRepository<InstructionList> instructionListRepository;
        private IRepository<InstructionListMember> instructionListMemberRepository;
        private IRepository<InstructionType> instructionTypeRespository;
        private IRepository<Status> statusRespository;
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
            if (!ModelState.IsValid || instructionListsParameters == null)
                return BadRequest(ModelState);


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
            {
                ModelState.AddModelError("BadOrderBy","Invalid OrderBy Parameter");
                return BadRequest(ModelState);
            }
               

            //build the query if any filter or search query critiera is passed
            instructionListHelper.BuildQueryToFilterData(instructionListsParameters, ref instructionListsBeforePaging);

            // call the static method on the paged list to filter items
            var instructionListsAfterPaging = PagedList<InstructionListDto>.Create(instructionListsBeforePaging,
                                                            instructionListsParameters.PageNumber,
                                                            instructionListsParameters.PageSize
                                                            );

            var paginationMetadata = instructionListHelper.getPaginationData(instructionListsAfterPaging, instructionListsParameters);

            if (Response !=null)
            Response.Headers.Add("X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            return Ok(instructionListsAfterPaging.ShapeData(instructionListsParameters.Fields));
        }

        [HttpPut]
        public IActionResult UpdateInstructionList(InstructionList parameters)
        {
            if (!ModelState.IsValid || parameters == null)
                return BadRequest(ModelState);


            var list = instructionListRepository.Find(i => i.InstructionListId == parameters.InstructionListId);

            if (list == null) return NotFound();

            instructionListRepository.Update(parameters, list.InstructionListId);
            instructionListRepository.Save();
        

            return Ok();
        }

        [HttpPost]
        public IActionResult AddInstructionList(AddInstructionListPrameters parameters)
        {
            if (!ModelState.IsValid || parameters == null)
                return BadRequest(ModelState);

            var defaultStatus = statusRespository.Find(s => s.StatusCode == "ENA");
            if (defaultStatus == null)
            {

                ModelState.AddModelError("DefaultStatus", "Unable to find 'Enabled' Status");
                return BadRequest(ModelState);
            }

            var instructionList = new InstructionList()
            {
                InstructionTypeId = parameters.TypeId,
                Name = parameters.Name,
                StatusId = defaultStatus.StatusId
            };

            instructionListRepository.Add(instructionList);
            instructionListRepository.Save();


            return Ok();
        }
        
    }
    
}