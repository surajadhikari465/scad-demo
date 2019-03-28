using System;

namespace KitBuilder.DataAccess.Dto
{
    public partial class InstructionListQueueDto
    {
        public int InstructionListQueueId { get; set; }
        public int KeyId { get; set; }
        public string Status { get; set; }
        public DateTime InsertDateUtc { get; set; }
        public DateTime MessageTimestampUtc { get; set; }
    }
}