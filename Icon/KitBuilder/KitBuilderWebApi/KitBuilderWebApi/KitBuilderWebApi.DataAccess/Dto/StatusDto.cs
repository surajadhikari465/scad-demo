using System;
using System.Collections.Generic;
using System.Text;

namespace KitBuilderWebApi.DataAccess.Dto
{
    public class StatusDto
    {
        public StatusDto()
        {
            InstructionList = new HashSet<InstructionListDto>();
            KitLocale = new HashSet<KitLocaleDto>();
        }

        public int StatusId { get; set; }
        public string StatusCode { get; set; }
        public string StatusDescription { get; set; }

        public ICollection<InstructionListDto> InstructionList { get; set; }
        public ICollection<KitLocaleDto> KitLocale { get; set; }
    }
}
