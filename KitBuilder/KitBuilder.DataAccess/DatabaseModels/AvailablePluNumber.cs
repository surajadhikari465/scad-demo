using KitBuilder.DataAccess.DatabaseModels;
using System;
using System.Collections.Generic;

namespace KitBuilder.DataAccess
{
    public partial class AvailablePluNumber
    {
        public AvailablePluNumber()
        {
            InstructionListMember = new InstructionListMember();
        }

        public int PluNumber { get; set; }
        public bool? InUse { get; set; }
        public DateTime InsertDateUtc { get; set; }
        public DateTime? LastUpdatedDateUtc { get; set; }

        public InstructionListMember InstructionListMember { get; set; }
    }
}