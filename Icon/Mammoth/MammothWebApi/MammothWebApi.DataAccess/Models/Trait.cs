using System;
using System.Collections.Generic;

namespace MammothWebApi.DataAccess.Models
{
    public partial class Trait
    {
        public int TraitID { get; set; }
        public Nullable<int> TraitGroupID { get; set; }
        public string TraitCode { get; set; }
        public string TraitDesc { get; set; }
        public System.DateTime AddedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}
