using Icon.Dashboard.Mvc.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Icon.Dashboard.Mvc.ViewModels
{
    public class ApiMessageJobReportViewModel
    {
        public ApiMessageJobReportViewModel()
        {
            this.JobSummaries = new List<ApiMessageJobSummaryViewModel>();
            this.TimedReport = new ApiMessageJobTimedReportViewModel(this.JobType);
        }

        public ApiMessageJobReportViewModel(string jobType) : this()
        {
            this.JobType = jobType;
        }

        public string ViewTitle { get; set; }

        public string JobType { get; set; }

        public ApiJobMessageTypeEnum JobTypeEnum { get; set; }

        public List<ApiMessageJobSummaryViewModel> JobSummaries { get; set;}

        public ApiMessageJobTimedReportViewModel TimedReport { get; set; }

        public PaginationPageSetViewModel PaginationModel { get; set; }
    }
}