using System;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using KitBuilderWebApi.DataAccess.Dto;
using KitBuilderWebApi.DataAccess.Repository;
using KitBuilderWebApi.DatabaseModels;
using KitBuilderWebApi.Helper;
using KitBuilderWebApi.QueryParameters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace KitBuilderWebApi.Controllers
{



    [Produces("application/json")]
    [Route("api/InstructionList")]
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

        [HttpGet("{instructionListId}/InstructionListMember/{instructionListMemberId}")]
        public IActionResult GetInstructionListMember(int instructionListId, int instructionListMemberId)
        {
            var instructionListMember =
                instructionListMemberRepository.Find(ilm =>
                    ilm.InstructionListId == instructionListId &&
                    ilm.InstructionListMemberId == instructionListMemberId);

            if (instructionListMember == null)
            {
                logger.LogWarning($"The InstructionListMember with Id {instructionListMemberId} was not found for Instruction List Id {instructionListId}.");
                return NotFound(ModelState);
            }

            var instructionListMemberDto = Mapper.Map<InstructionListMemberDto>(instructionListMember);
            return Ok(instructionListMemberDto);
        }

        [HttpGet("{instructionListId}/InstructionListMembers")]
        public IActionResult GetInstructionListMembers(int instructionListId)
        {
            var instructionList = instructionListRepository.Find(il => il.InstructionListId == instructionListId);

            if (instructionList == null)
            {
                logger.LogWarning($"The InstructionList with Id {instructionListId} was not found.");
                return NotFound(ModelState);
            }

            var instructionListMembers =
                instructionListMemberRepository.FindAll(il =>
                    il.InstructionListId == instructionListId);

            var instructionListMembersDto = Mapper.Map<List<InstructionListMemberDto>>(instructionListMembers);
            return Ok(instructionListMembersDto);
        }


        [HttpPost("{instructionListId}/InstructionListMember")]
        public IActionResult AddInstructionListMember(int instructionListId, [FromBody]InstructionListMemberDto instructionListMemberDto)
        {
            //Add instruction members to instruction list -this method will let consumers add instruction members to the instruction list.
            //It will accept a list of model with InstructionListId, Group, Member and Sequence as fields.It will insert records into Instruction
            //List Member table.

            if (!ModelState.IsValid || instructionListMemberDto == null)
                return BadRequest(ModelState);
            
            var instructionList =
                instructionListRepository.Find(f => f.InstructionListId==instructionListId);

            if (instructionList == null)
            {
                logger.LogWarning($"The InstructionList with Id {instructionListId} was not found.");
                return NotFound();
            }

            var instructionListMember = Mapper.Map<InstructionListMember>(instructionListMemberDto);
            instructionList.InstructionListMember.Add(instructionListMember);

            try
            {
                instructionListRepository.Save();
                return StatusCode((int)HttpStatusCode.Created);
            }

            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        [HttpPost("{instructionListId}/InstructionListMembers")]
        public IActionResult AddInstructionListMembers(int instructionListId, [FromBody]List<InstructionListMemberDto> instructionListMembersDto)
        {

            if (!ModelState.IsValid || instructionListMembersDto == null)
                return BadRequest(ModelState);

            var instructionList =
                instructionListRepository.Find(f => f.InstructionListId == instructionListId);

            if (instructionList == null)
            {
                logger.LogWarning($"The InstructionList with Id {instructionListId} was not found.");
                return NotFound();
            }

            foreach (var instructionListMemberDto in instructionListMembersDto)
            {
                var instructionListMember = Mapper.Map<InstructionListMember>(instructionListMemberDto);
                instructionList.InstructionListMember.Add(instructionListMember);
            }


            try
            {
                instructionListRepository.Save();
                return StatusCode((int)HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        [HttpDelete("{instructionListId}/InstructionListMember/{instructionListMemberId}")]
        public IActionResult DeleteInstructionListMember(int instructionListId, int instructionListMemberId)
        {
            //Delete instruction member from instruction list -this method will let consumers delete instruction members from the instruction list.
            //It will accept InstructionListId and list of InstructionListMemberID. It will delete records from Instruction List Member table.

            var instructionList =
                instructionListRepository.Find(il => il.InstructionListId == instructionListId);

            if (instructionList == null)
            {
                logger.LogWarning($"The InstructionList with Id {instructionListId} was not found.");
                ModelState.AddModelError("UnknownInstructionList", $"Instruction List for Id {instructionListId} not found");
                return NotFound(ModelState);
            }

            var instructionListMember =
                instructionListMemberRepository.Find(ilm =>
                    ilm.InstructionListId == instructionListId &&
                    ilm.InstructionListMemberId == instructionListMemberId);
               
            
            if (instructionListMember != null)
                   instructionListMemberRepository.Delete(instructionListMember);


            try
            {
                instructionListMemberRepository.Save();
                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "A problem happened while handling your request.");
            }

        }

        [HttpDelete("{instructionListId}/InstructionListMembers")]
        public IActionResult DeleteInstructionListMembers(int instructionListId, [FromBody]List<int> instructionListMemberIds )
        {
            
            var instructionList =
                instructionListRepository.Find(il => il.InstructionListId == instructionListId);

            if (instructionList == null)
            {
                logger.LogWarning($"The InstructionList with Id {instructionListId} was not found.");
                ModelState.AddModelError("UnknownInstructionList", $"Instruction List for Id {instructionListId} not found");
                return NotFound(ModelState);
            }

            var instructionListMembersToDelete =
                instructionListMemberRepository.FindAll(il => il.InstructionListId == instructionListId)
                    .Where(ilm => instructionListMemberIds.Contains(ilm.InstructionListMemberId));

            foreach (var instructionListMember in instructionListMembersToDelete)
            {
                instructionListMemberRepository.Delete(instructionListMember);
            }

            try
            {
                instructionListMemberRepository.Save();
                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "A problem happened while handling your request.");
            }

        }

        [HttpPut("{instructionListId}/InstructionListMembers")]
        public IActionResult UpdateInstructionListMembers(int instructionListId, [FromBody]List<InstructionListMemberDto> InstructionListMembersDto)
        {
            if (!ModelState.IsValid || InstructionListMembersDto == null)
                return BadRequest(ModelState);

            var instructionList = instructionListRepository.Find(il => il.InstructionListId == instructionListId);

            if (instructionList == null)
            {
                logger.LogWarning($"The InstructionList with Id {instructionListId} was not found.");
                ModelState.AddModelError("UnknownInstructionList", $"Instruction List for Id {instructionListId} not found");
                return NotFound(ModelState);

            }

            foreach (var instructionListMemberDto in InstructionListMembersDto)
            {
                var instructionListMember = new InstructionListMember()
                {
                    InstructionListMemberId = instructionListMemberDto.InstructionListMemberId,
                    InstructionListId = instructionListMemberDto.InstructionListId,
                    Group = instructionListMemberDto.Group,
                    Member = instructionListMemberDto.Member,
                    Sequence = instructionListMemberDto.Sequence
                };

                instructionListMemberRepository.Update(instructionListMember, instructionListMemberDto.InstructionListMemberId);
            }

            try
            {
                instructionListMemberRepository.Save();
                return Accepted();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }




    }
}