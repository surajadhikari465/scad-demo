using KitBuilder.DataAccess.DatabaseModels;
using System;
using System.Collections.Generic;

namespace KitBuilder.DataAccess
{
    public partial class AvailablePluNumber
    {
        public AvailablePluNumber()
        {
            InstructionListMember = new HashSet<InstructionListMember>();
        }

        public int PluNumber { get; set; }
        public bool? InUse { get; set; }
        public DateTime InsertDateUtc { get; set; }
        public DateTime? LastUpdatedDateUtc { get; set; }

        public ICollection<InstructionListMember> InstructionListMember { get; set; }
    }
}