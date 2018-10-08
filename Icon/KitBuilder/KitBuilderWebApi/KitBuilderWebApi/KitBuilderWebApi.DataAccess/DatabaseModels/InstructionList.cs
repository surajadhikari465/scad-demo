using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KitBuilderWebApi.DatabaseModels
{
    public partial class InstructionList
    {
        public InstructionList()
        {
            InstructionListMember = new HashSet<InstructionListMember>();
            KitInstructionList = new HashSet<KitInstructionList>();
            LinkGroupItem = new HashSet<LinkGroupItem>();
        }

        [Required]
        public int InstructionListId { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(10, ErrorMessage = "Name can have maximum length of 10.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Instruction Type is required.")]
        public int InstructionTypeId { get; set; }
        [Required(ErrorMessage = "Status is required.")]
        public int StatusId { get; set; }
        public InstructionType InstructionType { get; set; }
        public Status Status { get; set; }
        public DateTime InsertDateUtc { get; set; }
        public DateTime? LastUpdatedDateUtc { get; set; }
        public ICollection<InstructionListMember> InstructionListMember { get; set; }
        public ICollection<KitInstructionList> KitInstructionList { get; set; }
        public ICollection<LinkGroupItem> LinkGroupItem { get; set; }
    }
}
