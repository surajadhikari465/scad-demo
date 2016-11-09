using System;
using System.Collections.Generic;

namespace MammothWebApi.DataAccess.Models
{
    public partial class AttributeMetaData
    {
        public int AttributeMetaDataID { get; set; }
        public string TraitCode { get; set; }
        public string TraitGroupCode { get; set; }
        public string TraitDesc { get; set; }
        public Nullable<int> TraitGroup { get; set; }
    }
}
