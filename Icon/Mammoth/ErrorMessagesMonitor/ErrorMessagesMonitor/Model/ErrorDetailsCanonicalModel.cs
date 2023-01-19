using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErrorMessagesMonitor.Model
{
    internal class ErrorDetailsCanonicalModel
    {
        public IList<ErrorDetailsModel> ErrorDetailsList { get; set; }
        public string Application { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorSeverity { get; set; }

    }
}
