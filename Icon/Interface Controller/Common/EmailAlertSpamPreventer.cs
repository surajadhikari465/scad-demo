using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceController.Common
{
    public static class EmailAlertApprover
    {
        private static DateTime lastRestrictedErrorDatetime;

        public static bool ShouldSendEmailAlert(Exception ex, int minimumAlertFrequencyMinutes)
        {
            //determine if its a retricted error type
            if (SqlAzureRetriableExceptionDetector.ShouldRetryOn(ex))
            {
                //if yes then check last time emailed
                if ((DateTime.Now - lastRestrictedErrorDatetime) > TimeSpan.FromMinutes(minimumAlertFrequencyMinutes))
                {
                    lastRestrictedErrorDatetime = DateTime.Now;
                    return true;
                }
            }
            return false;
        }

        public static void Reset()
        {
            lastRestrictedErrorDatetime = new DateTime();
        }
    }
}
