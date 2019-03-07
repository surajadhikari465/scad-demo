using System;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using KitBuilder.DataAccess.DatabaseModels;
using KitBuilder.DataAccess.Dto;
using KitBuilder.DataAccess.Repository;
using KitBuilderWebApi.Helper;
using KitBuilderWebApi.QueryParameters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using KitBuilder.DataAccess;

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
        private IHelper<InstructionListDto, InstructionListsParameters> instructionListHelper;
        private IRepository<AvailablePluNumber> availablePluNumberRespository;

        public InstructionListMemberController(ILogger<InstructionListMemberController> logger,
            IHelper<InstructionListDto, InstructionListsParameters> instructionListHelper,
            IRepository<InstructionList> instructionListRepository,
            IRepository<InstructionListMember> instructionListMemberRepository,
            IRepository<InstructionType> instructionTypeRespository,
            IRepository<Status> statusRespository,
            IRepository<AvailablePluNumber> availablePluNumberRespository)
        {
            this.logger = logger;
            this.instructionListHelper = instructionListHelper;
            this.instructionListRepository = instructionListRepository;
            this.instructionListMemberRepository = instructionListMemberRepository;
            this.instructionTypeRespository = instructionTypeRespository;
            this.statusRespository = statusRespository;
            this.availablePluNumberRespository = availablePluNumberRespository;
        }

        /// <summary>
        /// InstructionListMember - GET - One
        /// </summary>
        /// <param name="instructionListId">InstructionList Id</param>
        /// <param name="instructionListMemberId">InstructionListMember Id</param>
        [HttpGet("{instructionListId}/InstructionListMember/{instructionListMemberId}")]
        public IActionResult GetInstructionListMember([FromRoute]int instructionListId, [FromRoute]int instructionListMemberId)
        {
            var instructionListMember =
                instructionListMemberRepository.Find(ilm =>
                    ilm.InstructionListId == instructionListId &&
                    ilm.InstructionListMemberId == instructionListMemberId);

            if (instructionListMember == null)
            {
                logger.LogWarning($"The InstructionListMember with Id {instructionListMemberId} was not found for Instruction List Id {instructionListId}.");
                return NotFound();
            }

            var instructionListMemberDto = Mapper.Map<InstructionListMemberDto>(instructionListMember);
            return Ok(instructionListMemberDto);
        }

        /// <summary>
        /// InstructionListMember - GET - All
        /// </summary>
        /// <param name="instructionListId">Instruction List Id</param>
        [HttpGet("{instructionListId}/InstructionListMembers")]
        public IActionResult GetInstructionListMembers([FromRoute]int instructionListId)
        {
            var instructionList = instructionListRepository.Find(il => il.InstructionListId == instructionListId);

            if (instructionList == null)
            {
                logger.LogWarning($"The InstructionList with Id {instructionListId} was not found.");
                return NotFound();
            }

            var instructionListMembers =
                instructionListMemberRepository.FindAll(il =>
                    il.InstructionListId == instructionListId);

            var instructionListMembersDto = Mapper.Map<List<InstructionListMemberDto>>(instructionListMembers);
            return Ok(instructionListMembersDto);
        }

        /// <summary>
        /// InstructionListMember - ADD - One
        /// </summary>
        /// <param name="instructionListId"></param>
        /// <param name="instructionListMemberDto"></param>

        [HttpPost("{instructionListId}/InstructionListMember")]
        public IActionResult AddInstructionListMember([FromRoute]int instructionListId, [FromBody]InstructionListMemberDto instructionListMemberDto)
        {
            if (!ModelState.IsValid || instructionListMemberDto == null)
                return BadRequest(ModelState);
            
            var instructionList =
                instructionListRepository.Find(f => f.InstructionListId==instructionListId);

            if (instructionList == null)
            {
                logger.LogWarning($"The Instruction" +
                    $"List with Id {instructionListId} was not found.");
                return NotFound();
            }
           var  firstAvailablePluNumber = availablePluNumberRespository.GetAll().Where(a => a.InUse == false).OrderByDescending(p=>p.PluNumber).Take(1).FirstOrDefault();

            if (firstAvailablePluNumber == null)
            {
                return StatusCode(500, "No Plu Numbers available.");
            }

            var instructionListMember = Mapper.Map<InstructionListMember>(instructionListMemberDto);
            instructionListMember.InstructionListId = instructionListId;
            instructionListMember.InsertDateUtc = DateTime.UtcNow;
            instructionListMember.LastUpdatedDateUtc = DateTime.UtcNow;
            instructionListMember.PluNumber = firstAvailablePluNumber.PluNumber;
            firstAvailablePluNumber.InUse = true;
            firstAvailablePluNumber.LastUpdatedDateUtc = DateTime.UtcNow; ;
            instructionList.InstructionListMember.Add(instructionListMember);

            try
            {
                instructionListRepository.UnitOfWork.Commit();
                return StatusCode((int)HttpStatusCode.Created);
            }

            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }
        /// <summary>
        /// InstructionListMember - GET - Multiple
        /// </summary>
        /// <param name="instructionListId"></param>
        /// <param name="instructionListMembersDto"></param>

        [HttpPost("{instructionListId}/InstructionListMembers")]
        public IActionResult AddInstructionListMembers([FromRoute]int instructionListId, [FromBody]List<InstructionListMemberDto> instructionListMembersDto)
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
            var availablePluNumbers = availablePluNumberRespository.GetAll().Where(a => a.InUse == false).Take(instructionListMembersDto.Count()).ToList();

            int numberOfPlu = availablePluNumbers.Count();

            if(numberOfPlu == 0)
            {
                return StatusCode(500, "No Plu Numbers available.");
            }

            foreach (var instructionListMemberDto in instructionListMembersDto)
            {
                int count = 0;

                var instructionListMember = Mapper.Map<InstructionListMember>(instructionListMemberDto);
                instructionListMember.InstructionListId = instructionListId;
                instructionListMember.LastUpdatedDateUtc = DateTime.UtcNow;
                instructionListMember.InsertDateUtc = DateTime.UtcNow;
                instructionListMember.PluNumber = availablePluNumbers[count].PluNumber;
                instructionList.InstructionListMember.Add(instructionListMember);
                availablePluNumbers[count].InUse = true;
                availablePluNumbers[count].LastUpdatedDateUtc =  DateTime.UtcNow;
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

        /// <summary>
        /// InstructionListMember - DELETE - One
        /// </summary>
        /// <param name="instructionListId"></param>
        /// <param name="instructionListMemberId"></param>

        [HttpDelete("{instructionListId}/InstructionListMember/{instructionListMemberId}")]
        public IActionResult DeleteInstructionListMember([FromRoute]int instructionListId, [FromRoute]int instructionListMemberId)
        {
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
            {
                var pluNumber = availablePluNumberRespository.GetAll().Where(p => p.PluNumber == instructionListMember.PluNumber).FirstOrDefault();
                instructionListMemberRepository.Delete(instructionListMember);
                pluNumber.InUse = false;
                pluNumber.LastUpdatedDateUtc = DateTime.UtcNow;
            }

            try
            {
                instructionListMemberRepository.UnitOfWork.Commit();
                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        /// <summary>
        /// InstructionListMember - DELETE - Multiple
        /// </summary>
        /// <param name="instructionListId"></param>
        /// <param name="instructionListMemberIds"></param>

        [HttpDelete("{instructionListId}/InstructionListMembers")]
        public IActionResult DeleteInstructionListMembers([FromRoute]int instructionListId, [FromBody]List<int> instructionListMemberIds )
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
                var pluNumber = availablePluNumberRespository.GetAll().Where(p => p.PluNumber == instructionListMember.PluNumber).FirstOrDefault();
                pluNumber.InUse = false;
                instructionListMemberRepository.Delete(instructionListMember);
            }

            try
            {
                instructionListMemberRepository.UnitOfWork.Commit();
                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "A problem happened while handling your request.");
            }

        }

        /// <summary>
        /// InstructionListMember - UPDATE - Multiple
        /// </summary>
        /// <param name="instructionListId"></param>
        /// <param name="InstructionListMembersDto"></param>
        /// <returns></returns>
        [HttpPut("{instructionListId}/InstructionListMembers")]
        public IActionResult UpdateInstructionListMembers([FromRoute]int instructionListId, [FromBody]List<InstructionListMemberDto> InstructionListMembersDto)
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

                var existingInstructionListMemeber = instructionListMemberRepository.Find(ilm =>
                    ilm.InstructionListMemberId == instructionListMemberDto.InstructionListMemberId);

                var instructionListMember = new InstructionListMember()
                {
                    InstructionListMemberId = instructionListMemberDto.InstructionListMemberId,
                    InstructionListId = instructionList.InstructionListId,
                    Group = instructionListMemberDto.Group,
                    Member = instructionListMemberDto.Member,
                    Sequence = instructionListMemberDto.Sequence,
                    LastUpdatedDateUtc = DateTime.UtcNow,
                    InsertDateUtc = existingInstructionListMemeber.InsertDateUtc,
                    PluNumber = existingInstructionListMemeber.PluNumber
                };

                instructionListMemberRepository.Update(instructionListMember, instructionListMemberDto.InstructionListMemberId);
            }

            try
            {
                instructionListMemberRepository.UnitOfWork.Commit();
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