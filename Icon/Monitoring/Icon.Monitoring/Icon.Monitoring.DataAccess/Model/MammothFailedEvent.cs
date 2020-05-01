using System;

namespace Icon.Monitoring.DataAccess.Model
{
    public class MammothFailedEvent
    {
        public int Identifier { get; set; }
        public int ItemKey { get; set; }
        public int StoreNo { get; set; }
        public DateTime InsertDate { get; set; }
    }
}
