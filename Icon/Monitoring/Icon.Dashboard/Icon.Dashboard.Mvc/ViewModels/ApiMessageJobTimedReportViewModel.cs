using Icon.Dashboard.CommonDatabaseAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Icon.Dashboard.Mvc.ViewModels
{
    public class ApiMessageJobTimedReportViewModel
    {
        public ApiMessageJobTimedReportViewModel()
        {
            StartTime = DateTime.Now.AddDays(-7);
            EndTime = DateTime.Now;
        }

        public ApiMessageJobTimedReportViewModel(string messageType) : this()
        {
            MessageType = messageType;
            StartTime = DateTime.Now.AddDays(-7);
            EndTime = DateTime.Now;
        }
        public ApiMessageJobTimedReportViewModel(IApiJobSummary dataQueryResult) : this()
        {
            MessageType = dataQueryResult.MessageType;
            CountProcessedMessages = dataQueryResult.CountProcessedMessages;
            CountFailedMessages = dataQueryResult.CountFailedMessages;
            SetElapsedTime(dataQueryResult.StartTime, dataQueryResult.EndTime);
        }
        
        [DisplayName("Type")]
        public string MessageType { get; set; }
        
        [DisplayName("Start")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public DateTime? StartTime { get; set; }
        
        [DisplayName("End")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public DateTime? EndTime { get; set; }
        
        [DisplayName("Elapsed")]
        [DisplayFormat(DataFormatString = "{0:dd\\.hh\\:mm\\:ss}")]
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
        public long? CountProcessedMessages { get; set; }

        [DisplayName("Failed")]
        public long? CountFailedMessages { get; set; }

        [DisplayName("Total")]
        public long? CountTotalMessages
        {
            get
            {
                return CountProcessedMessages.GetValueOrDefault() + CountFailedMessages.GetValueOrDefault();
            }
        }

        //[HiddenInput(DisplayValue = false)]
        //public bool SomeFailed
        //{
        //    get
        //    {
        //        return CountProcessedMessages.HasValue && CountFailedMessages.GetValueOrDefault() > 0;
        //    }
        //}

        public TimeSpan? SetElapsedTime(DateTime? start, DateTime? end)
        {
            this.StartTime = start;
            this.EndTime = end;
            //_elapsedTime = (start.HasValue && end.HasValue)
            //    ? _elapsedTime = end.Value - start.Value
            //    : null;
            return this.ElapsedTime;
        }

        public string Errors { get; set; }
    }
}