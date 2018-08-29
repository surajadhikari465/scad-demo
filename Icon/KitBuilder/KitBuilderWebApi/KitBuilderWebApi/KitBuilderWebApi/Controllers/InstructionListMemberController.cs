using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KitBuilderWebApi.DataAccess.Repository;
using KitBuilderWebApi.DatabaseModels;
using KitBuilderWebApi.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace KitBuilderWebApi.Controllers
{

 

    [Produces("application/json")]
    [Route("api/InstructionListMember")]
    public class InstructionListMemberController : Controller
    {

        private IRepository<InstructionList> instructionListRepository { get; set; }
        private IRepository<InstructionListMember> instructionListMemberRepository { get; set; }
        private IRepository<InstructionType> instructionTypeRespository { get; set; }
        private IRepository<Status> statusRespository { get; set; }
        private ILogger<InstructionListController> logger;
        private InstructionListHelper instructionListHelper;

        public InstructionListMemberController(ILogger<InstructionListController> logger, InstructionListHelper instructionListHelper, IRepository<InstructionList> instructionListRepository, IRepository<InstructionListMember> instructionListMemberRepository, IRepository<InstructionType> instructionTypeRespository, IRepository<Status> statusRespository)
        {
            this.logger = logger;
            this.instructionListHelper = instructionListHelper;
            this.instructionListRepository = instructionListRepository;
            this.instructionListMemberRepository = instructionListMemberRepository;
            this.instructionTypeRespository = instructionTypeRespository;
            this.statusRespository = statusRespository;
        }

        [HttpGet]
        public IActionResult GetInstructionListMember()
        {
            return Ok();
        }
        
    }
}