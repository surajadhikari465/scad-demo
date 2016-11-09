﻿using System;

namespace Mammoth.ItemLocale.Controller.Tests.Specs.TestInfrastructure
{
    public class TestQueueModel
    {
        public int QueueID { get; set; }
        public int Item_Key { get; set; }
        public int? Store_No { get; set; }
        public string Identifier { get; set; }
        public int EventTypeID { get; set; }
        public int? EventReferenceID { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime? ProcessFailedDate { get; set; }
        public int? InProcessBy { get; set; }
    }
}
