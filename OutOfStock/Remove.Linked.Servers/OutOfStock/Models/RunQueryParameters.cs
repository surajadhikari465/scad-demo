using System;
using System.Collections.Generic;

namespace OutOfStock.Models
{
    public class RunQueryParameters
    {
        public RunQueryParameters(DateTime? start, DateTime? end, Dictionary<int, string> storeInfo, Dictionary<string, string> teamInfo, Dictionary<string, string> subTeamInfo, DateTime todaysDate, int groupByUpc)
        {
            Start = start;
            End = end;
            StoreInfo = storeInfo;
            TeamInfo = teamInfo;
            SubTeamInfo = subTeamInfo;
            TodaysDate = todaysDate;
            GroupByUpc = groupByUpc;
        }

        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public Dictionary<int, string> StoreInfo { get; set; }
        public Dictionary<string, string> TeamInfo { get; set; }
        public Dictionary<string, string> SubTeamInfo { get; set; }
        public DateTime TodaysDate { get; set; }
        public int GroupByUpc { get; set; }
    }
}