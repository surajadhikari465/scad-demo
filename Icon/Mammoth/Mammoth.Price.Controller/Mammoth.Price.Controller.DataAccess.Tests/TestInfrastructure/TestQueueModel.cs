using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mammoth.Price.Controller.DataAccess.Tests.TestInfrastructure
{
    public class TestQueueModel
    {
        public int ItemKey { get; set; }
        public int StoreNo { get; set; }
        public string Identifier { get; set; }
        public int EventTypeId { get; set; }
        public int? EventReferenceId { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime? ProcessFailedDate { get; set; }
        public int? InProcessBy { get; set; }
    }
}
