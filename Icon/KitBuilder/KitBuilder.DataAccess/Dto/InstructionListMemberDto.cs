using System.ComponentModel.DataAnnotations;

namespace KitBuilder.DataAccess.Dto
{
    public class InstructionListMemberDto
    {
        [Required]
        public int InstructionListId { get; set; }
        public int InstructionListMemberId { get; set; }
        [Required]
        public int Sequence { get; set; }
        [Required]
        public string Group { get; set; }
        [Required]
        public string Member { get; set; }
    }
}