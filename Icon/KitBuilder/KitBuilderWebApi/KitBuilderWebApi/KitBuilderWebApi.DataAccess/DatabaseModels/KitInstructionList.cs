using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KitBuilderWebApi.DatabaseModels
{
    public partial class KitInstructionList
    {
        public int KitInstructionListId { get; set; }
        [Required]
        public int KitId { get; set; }
        [Required]
        public int InstructionListId { get; set; }

        public InstructionList InstructionList { get; set; }
        public Kit KitInstructionListNavigation { get; set; }
    }
}
