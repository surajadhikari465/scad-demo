using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.DataAccess.Models
{
    public class BulkImportBrandModel
    {
        public string BrandLineage { get; set; }
        public string BrandId { get; set; }
        public string BrandName { get; set; }
        public string BrandAbbreviation { get; set; }
        public string Error { get; set; }
    }
}
