using System;
using System.Collections.Generic;

namespace MammothWebApi.DataAccess.Models
{
    public partial class TraitGroup
    {
        public int traitGroupID { get; set; }
        public string traitGroupCode { get; set; }
        public string traitGroupDesc { get; set; }
        public System.DateTime AddedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}
