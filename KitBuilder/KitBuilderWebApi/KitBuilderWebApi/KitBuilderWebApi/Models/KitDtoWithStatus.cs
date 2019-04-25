using KitBuilder.DataAccess.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KitBuilder.DataAccess.Enums;

namespace KitBuilderWebApi.Models
{
    public class KitDtoWithStatus: KitDto
    {
        public Status KitStatus { get; set; }
    }
}
