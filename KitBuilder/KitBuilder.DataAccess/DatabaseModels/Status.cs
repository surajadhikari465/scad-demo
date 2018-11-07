using System.Collections.Generic;

namespace KitBuilder.DataAccess.DatabaseModels
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
