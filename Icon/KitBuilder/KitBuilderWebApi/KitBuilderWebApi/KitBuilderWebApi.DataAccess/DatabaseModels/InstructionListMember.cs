using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KitBuilderWebApi.DatabaseModels
{
    public partial class InstructionListMember
    {
        public int InstructionListMemberId { get; set; }
        [Required(ErrorMessage = "Instruction List is required.")]
        public int InstructionListId { get; set; }
        [Required(ErrorMessage = "Group is required.")]
        [StringLength(60, ErrorMessage = "Group can have maximum length of 60.")]
        public string Group { get; set; }
        [Required(ErrorMessage = "Sequence is required.")]
        public int Sequence { get; set; }
        public string Member { get; set; }
        public DateTime InsertDateUtc { get; set; }
        public DateTime? LastUpdatedDateUtc { get; set; }
        public InstructionList InstructionList { get; set; }
    }
}