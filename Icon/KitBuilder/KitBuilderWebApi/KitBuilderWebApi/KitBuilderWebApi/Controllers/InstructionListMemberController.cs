using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KitBuilderWebApi.DataAccess.Repository;
using KitBuilderWebApi.DatabaseModels;
using KitBuilderWebApi.Helper;
using KitBuilderWebApi.QueryParameters;
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

        public InstructionListMemberController(ILogger<InstructionListController> logger,
            InstructionListHelper instructionListHelper, IRepository<InstructionList> instructionListRepository,
            IRepository<InstructionListMember> instructionListMemberRepository,
            IRepository<InstructionType> instructionTypeRespository, IRepository<Status> statusRespository)
        {
            this.logger = logger;
            this.instructionListHelper = instructionListHelper;
            this.instructionListRepository = instructionListRepository;
            this.instructionListMemberRepository = instructionListMemberRepository;
            this.instructionTypeRespository = instructionTypeRespository;
            this.statusRespository = statusRespository;
        }

        [HttpPost]
        public IActionResult AddInstructionListMember([FromBody]List<AddInstructionListMemberParameters> parameters)
        {
            //Add instruction members to instruction list -this method will let consumers add instruction members to the instruction list.
            //It will accept a list of model with InstructionListId, Group, Member and Sequence as fields.It will insert records into Instruction
            //List Member table.

            var ids = parameters.Select(p => p.InstructionListId);
            var instructionLists =
                instructionListRepository.FindAll(f => ids.Contains(f.InstructionListId));

            foreach (var il in parameters)
            {
                var instructionListMember = new InstructionListMember
                { 
                    Group = il.Group,
                    Member = il.Member,
                    Sequence = il.Sequence,
                    InstructionListId = il.InstructionListId
                };

                var existingInstructionList =
                    instructionLists.FirstOrDefault(existing => existing.InstructionListId == il.InstructionListId);

                if(existingInstructionList != null) instructionListMemberRepository.Add(instructionListMember);

            }
            instructionListMemberRepository.Save();

            return Ok();
        }

        [HttpPut]
        public IActionResult UpdateInstructionListMember(UpdateInstructionListMemberParameters[] parameters)
        {
            //Update Instruction members - this method will let consumers update instruction members.It will accept a
            //list of model with InstructionListId, Group,Member and Sequence as fields.It will update existing records in Instruction List Member table.

            foreach (var update in parameters)
            {
                var existingInstructionList = instructionListRepository.Find(il => il.InstructionListId == update.InstructionListId);
                if (existingInstructionList == null) continue;

                var instructionListMember = new InstructionListMember()
                {
                    Group = update.Group,
                    Member = update.Member,
                    Sequence = update.Sequence,
                    InstructionListId = update.InstructionListId
                };

                instructionListMemberRepository.Update(instructionListMember, instructionListMember.InstructionListId);

            }
            instructionListMemberRepository.Save();

            return Ok();
        }

        [HttpDelete]
        public IActionResult DeleteInstructionListMember([FromBody]DeleteInstructionListMembersParameters parameters)
        {
            //Delete instruction member from instruction list -this method will let consumers delete instruction members from the instruction list.
            //It will accept InstructionListId and list of InstructionListMemberID. It will delete records from Instruction List Member table.

            var instructionList =
                instructionListRepository.Find(il => il.InstructionListId == parameters.InstructionListId);

            if (instructionList == null)
            {
                ModelState.AddModelError("UnknownInstructionLIst", $"Instruction List for Id {parameters.InstructionListId} not found");
                return NotFound(ModelState);
            }


            var instructionListMembers =
                instructionListMemberRepository.FindAll(ilm => ilm.InstructionListId == parameters.InstructionListId)
                    .Where(ilm=>parameters.InstructionListMemberIds.Contains(ilm.InstructionListMemberId));

            foreach (var ilm in instructionListMembers)
            {
                instructionListMemberRepository.Delete(ilm);
            }
            instructionListMemberRepository.Save();


            return Ok();

        }

    }
}