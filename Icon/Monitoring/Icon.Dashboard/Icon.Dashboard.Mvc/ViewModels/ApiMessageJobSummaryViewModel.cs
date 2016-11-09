using Icon.Dashboard.CommonDatabaseAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace Icon.Dashboard.Mvc.ViewModels
{
    public class ApiMessageJobSummaryViewModel
    {
        public ApiMessageJobSummaryViewModel()
        {

        }

        public ApiMessageJobSummaryViewModel(IAPIMessageMonitorLog entity) : this()
        {
            this.APIMessageMonitorLogID = entity.APIMessageMonitorLogID;
            this.MessageType = entity.MessageTypeName;
            if (entity.StartTime.HasValue)
            {
                this.StartTime = entity.StartTime.Value.ToLocalTime();
            }
            if (entity.EndTime.HasValue)
            {
                this.EndTime = entity.EndTime.Value.ToLocalTime();
            }
            this.CountProcessedMessages = entity.CountProcessedMessages;
            this.CountFailedMessages = entity.CountFailedMessages;
        }

        [DisplayName("ID")]
        public int APIMessageMonitorLogID { get; set; }

        [DisplayName("Type")]
        public string MessageType { get; set; }

        [DisplayName("Start")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss.ff}")]
        public DateTime? StartTime { get; set; }

        [DisplayName("End")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss.ff}")]
        public DateTime? EndTime { get; set; }

        [DisplayName("Elapsed")]
        [DisplayFormat(DataFormatString = "{0:mm\\:ss\\.fff}")]
        public TimeSpan? ElapsedTime
        {
            get
            {
                if (StartTime.HasValue && EndTime.HasValue)
                {
                    return EndTime.GetValueOrDefault() - StartTime.GetValueOrDefault();
                }
                return null;
            }
        }

        [DisplayName("Processed")]
        public int? CountProcessedMessages { get; set; }

        [DisplayName("Failed")]
        public int? CountFailedMessages { get; set; }

        [DisplayName("Total")]
        public int? CountTotalMessages
        {
            get
            {
                return CountProcessedMessages.GetValueOrDefault() + CountFailedMessages.GetValueOrDefault();
            }
        }

        [HiddenInput(DisplayValue = false)]
        public bool SomeFailed
        {
            get
            {
                return CountProcessedMessages.HasValue && CountFailedMessages.GetValueOrDefault() > 0;
            }
        }
    }
}