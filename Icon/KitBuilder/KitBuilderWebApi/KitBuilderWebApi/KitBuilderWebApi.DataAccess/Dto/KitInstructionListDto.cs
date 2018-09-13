using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KitBuilderWebApi.DataAccess.Dto
{
  public class KitInstructionListDto
    {
        public int KitInstructionListId { get; set; }
        [Required]
        public int KitId { get; set; }
        [Required]
        public int InstructionListId { get; set; }

        public InstructionListDto InstructionList { get; set; }
        public KitDto KitInstructionListNavigation { get; set; }
    }
}
