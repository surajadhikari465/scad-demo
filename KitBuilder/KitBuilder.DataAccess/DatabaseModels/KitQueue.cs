using System;

namespace KitBuilder.DataAccess.DatabaseModels
{
    public partial class KitQueue
    {
        public int KitQueueId { get; set; }
        public int KitId { get; set; }
        public int StoreId { get; set; }
        public int VenueId { get; set; }
        public int KitLocaleId { get; set; }
        public DateTime InsertDateUtc { get; set; }
        public string Status { get; set; }
        public DateTime? MessageTimestampUtc { get; set; }
    }
}