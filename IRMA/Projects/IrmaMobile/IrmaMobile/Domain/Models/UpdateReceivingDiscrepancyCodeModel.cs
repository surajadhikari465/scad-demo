using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IrmaMobile.Domain.Models
{
    public class UpdateReceivingDiscrepancyCodeModel
    {
        public int OrderItemId { get; set; }
        public int ReasonCodeId { get; set; }
    }
}
