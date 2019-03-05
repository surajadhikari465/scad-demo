using System;
using System.Collections.Generic;

namespace KitBuilder.DataAccess.Dto
{
    public partial class AvailablePluNumberDto
    {
        public AvailablePluNumberDto()
        {
            InstructionListMemberDto = new HashSet<InstructionListMemberDto>();
        }

        public int PluNumber { get; set; }
        public bool? InUse { get; set; }
        public DateTime InsertDateUtc { get; set; }
        public DateTime? LastUpdatedDateUtc { get; set; }

        public ICollection<InstructionListMemberDto> InstructionListMemberDto { get; set; }
    }
}