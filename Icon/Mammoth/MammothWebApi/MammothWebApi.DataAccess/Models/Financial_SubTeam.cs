using System;
using System.Collections.Generic;

namespace MammothWebApi.DataAccess.Models
{
    public partial class Financial_SubTeam
    {
        public int FinancialSubTeamID { get; set; }
        public int FinancialHCID { get; set; }
        public string Name { get; set; }
        public Nullable<int> PSNumber { get; set; }
        public Nullable<int> POSDeptNumber { get; set; }
        public System.DateTime AddedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}
