using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for VendorTrend
/// </summary>
namespace POReports
{
    public class VendorTrendRow
    {
        public string Label { get; set; }
        public int TotalPOs { get; set; }
        public int SuspendedPOs { get; set; }
        public string Region {get; set;}
        public decimal PercentOfTotal
        {
            get
            {
                if (TotalPOs > 0 && SuspendedPOs > 0)
                {
                    return Convert.ToDecimal((SuspendedPOs / TotalPOs) * 100);
                }
                else if (TotalPOs > 0 && (TotalPOs == SuspendedPOs))
                {
                    return 100;
                }
                else
                {
                    return 0;
                }
            }
        }
        public int Period = 0;

        private void ResetData()
        {
            Label = "";
            TotalPOs = 0;
            SuspendedPOs = 0;
            Region = "";
            Period = 0;
        }

        public VendorTrendRow() {
            ResetData();
        }

        public VendorTrendRow(int _period, int _totalPOs, int _suspendedPOs, string _region)
        {
            ResetData();
            Period = _period;
            TotalPOs = _totalPOs;
            SuspendedPOs = _suspendedPOs;
            Region = _region;
            calculate();
        }

        public void calculate()
        {
            Label = string.Format("FP{0}", Period);
        }
    }
}