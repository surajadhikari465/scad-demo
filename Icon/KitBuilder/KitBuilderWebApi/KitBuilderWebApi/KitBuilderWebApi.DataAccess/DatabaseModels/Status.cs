using System;
using System.Collections.Generic;

namespace KitBuilderWebApi.DatabaseModels
{
    public partial class Status
    {
        public Status()
        {
            InstructionList = new HashSet<InstructionList>();
            KitLocale = new HashSet<KitLocale>();
        }

        public int StatusId { get; set; }
        public string StatusCode { get; set; }
        public string StatusDescription { get; set; }

        public ICollection<InstructionList> InstructionList { get; set; }
        public ICollection<KitLocale> KitLocale { get; set; }
    }
}
