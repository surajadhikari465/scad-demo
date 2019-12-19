using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOS.Model
{
    public interface ICustomReportRepository
    {
        OOSCustomReport For(DateTime startDate, DateTime endDate, List<int> storeIds, List<string> teams, List<string> subTeams);
    }
}
