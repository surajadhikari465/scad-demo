using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MammothWebApi.DataAccess.Models
 {
    public class PrimePsgItemStoreDataModel
     {
        public int ItemId { get; set; }
        public string ItemTypeCode { get; set; }
        public string ScanCode { get; set; }
        public int PsSubTeamNumber { get; set; }
        public int BusinessUnitId { get; set; }
        public string StoreName { get; set; }
        public string Region { get; set; }
    }
}
