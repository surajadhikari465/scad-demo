using System;
using System.Collections.Generic;
using System.Linq;
using KitBuilderWebApi.DataAccess.Repository;
using KitBuilderWebApi.DatabaseModels;
using KitBuilderWebApi.Helper;
using KitBuilderWebApi.QueryParameters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace KitBuilderWebApi.Controllers
{



    [Produces("application/json")]
    [Route("api/InstructionListMember")]
    public class InstructionListMemberController : Controller
    {

        private IRepository<InstructionList> instructionListRepository;
        private IRepository<InstructionListMember> instructionListMemberRepository;
        private IRepository<InstructionType> instructionTypeRespository;
        private IRepository<Status> statusRespository;
        private ILogger<InstructionListMemberController> logger;
        private InstructionListHelper instructionListHelper;

        public InstructionListMemberController(ILogger<InstructionListMemberController> logger,
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

            if (!ModelState.IsValid || parameters == null)
                return BadRequest(ModelState);

            // get all the instruction lists referenced in the parameters.
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

                // make sure the referenced instruction list exists.
                var existingInstructionList =
                    instructionLists.FirstOrDefault(existing => existing.InstructionListId == il.InstructionListId);

                // if it does, add the instruction list member.
                if (existingInstructionList != null) instructionListMemberRepository.Add(instructionListMember);

            }
            // save changes
            instructionListMemberRepository.Save();

            return Ok();
        }

        [HttpPut]
        public IActionResult UpdateInstructionListMember([FromBody]List<UpdateInstructionListMemberParameters> parameters)
        {
            //Update Instruction members - this method will let consumers update instruction members.It will accept a
            //list of model with InstructionListId, Group,Member and Sequence as fields.It will update existing records in Instruction List Member table.

            if (!ModelState.IsValid || parameters == null)
                return BadRequest(ModelState);

            foreach (var update in parameters)
            {
                var existingInstructionList = instructionListRepository.Find(il => il.InstructionListId == update.InstructionListId);
                if (existingInstructionList == null) continue;

                var instructionListMember = new InstructionListMember()
                {
                    InstructionListMemberId =  update.InstructionListMemberId, 
                    InstructionListId =  update.InstructionListId,
                    Group = update.Group,
                    Member = update.Member,
                    Sequence = update.Sequence
                };

                instructionListMemberRepository.Update(instructionListMember, update.InstructionListMemberId);

            }
            instructionListMemberRepository.Save();

            return Ok();
        }

        [HttpDelete]
        public IActionResult DeleteInstructionListMember([FromBody]DeleteInstructionListMembersParameters parameters)
        {
            //Delete instruction member from instruction list -this method will let consumers delete instruction members from the instruction list.
            //It will accept InstructionListId and list of InstructionListMemberID. It will delete records from Instruction List Member table.

            if (!ModelState.IsValid || parameters == null)
                return BadRequest(ModelState);

            var instructionList =
                instructionListRepository.Find(il => il.InstructionListId == parameters.InstructionListId);

            if (instructionList == null)
            {
                ModelState.AddModelError("UnknownInstructionList", $"Instruction List for Id {parameters.InstructionListId} not found");
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