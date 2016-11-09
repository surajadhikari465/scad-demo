using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.Item.Models
{
    public class ValidateItemResultModel
    {
        public int ItemId { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorDetails { get; set; }
    }
}
