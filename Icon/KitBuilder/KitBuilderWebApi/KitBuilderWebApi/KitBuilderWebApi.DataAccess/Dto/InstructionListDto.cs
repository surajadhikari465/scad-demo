using System.ComponentModel.DataAnnotations;

namespace KitBuilderWebApi.DataAccess.Dto
{
    public class InstructionListDto
    {
        public int InstructionListId { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(10, ErrorMessage = "Name can have maximum length of 10.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Instruction Type is required.")]
        public int InstructionTypeId { get; set; }
        [Required(ErrorMessage = "Status is required.")]
        public int StatusId { get; set; }
        public string Status { get; set; }
        public string InstructionTypeName { get; set; }
    }
}