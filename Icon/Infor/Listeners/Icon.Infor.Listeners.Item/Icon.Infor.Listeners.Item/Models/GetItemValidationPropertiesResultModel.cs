using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.Item.Models
{
    public class GetItemValidationPropertiesResultModel
    {
        public int? ItemId { get; set; }
        public int? BrandId { get; set; }
        public int? SubTeamId { get; set; }
        public int? SubBrickId { get; set; }
        public int? NationalClassId { get; set; }
        public int? TaxClassId { get; set; }
        public string ModifiedDate { get; set; }
        public decimal? SequenceId { get; set; }
    }
}
