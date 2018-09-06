﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KitBuilderWebApi.DataAccess.Dto
{
    public class LinkGroupItemDto
    {
        public LinkGroupItemDto()
        {
            
        }

        public int LinkGroupItemId { get; set; }
        [Required]
        public int LinkGroupId { get; set; }
        [Required]
        public int ItemId { get; set; }
        public int? InstructionListId { get; set; }
        public DateTime InsertDate { get; set; }

        public InstructionListDto InstructionList { get; set; }
        public ItemsDto Item { get; set; }
    }
}