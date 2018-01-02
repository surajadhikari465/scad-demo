using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSupport.DataAccess.Models;

namespace WebSupport.ViewModels
{
    /// <summary>
    /// Extension of the DataAccess.Models.JobSchedule with additional validation attributes
    /// </summary>
    [MetadataType(metadataClassType: typeof(JobScheduleMetaData))]
    public partial class JobScheduleViewModel : JobSchedule
    {
        public static JobScheduleViewModel FromDataAccessModel(JobSchedule dataModel)
        {
            if (dataModel == null)
            {
                throw new ArgumentNullException("JobSchedule");
            }

            return new JobScheduleViewModel
            {
                JobScheduleId = dataModel.JobScheduleId,
                JobName = dataModel.JobName,
                Region = dataModel.Region,
                DestinationQueueName = dataModel.DestinationQueueName,
                StartDateTimeUtc = dataModel.StartDateTimeUtc,
                LastScheduledDateTimeUtc = dataModel.LastScheduledDateTimeUtc,
                LastRunEndDateTimeUtc = dataModel.LastRunEndDateTimeUtc,
                NextScheduledDateTimeUtc = dataModel.NextScheduledDateTimeUtc,
                IntervalInSeconds = dataModel.IntervalInSeconds,
                Enabled = dataModel.Enabled,
                Status = dataModel.Status,
                XmlObject = dataModel.XmlObject
            };
        }
    }

    /// <summary>
    /// Allows validation attributes to be separated from the model in the DataAccess namespace and used in a view model
    /// </summary>
    internal class JobScheduleMetaData
    {
        [Display(Name = "Job Schedule Id")]
        public object JobScheduleId;

        [Required]
        [Display(Name = "Job Name")]
        public object JobName;

        [Display(Name = "Region")]
        [MaxLength(2)]
        public object Region;

        [Required]
        [Display(Name = "Destination Queue Name")]
        [DataType(DataType.MultilineText)]
        public object DestinationQueueName;

        [Required]
        [Display(Name = "Start DateTime UTC")]
        [DisplayFormat(DataFormatString = "{0:yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'}", ApplyFormatInEditMode = true)]
        public object StartDateTimeUtc;

        [Display(Name = "Last Scheduled DateTime UTC")]
        [DisplayFormat(DataFormatString = "{0:yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'}", ApplyFormatInEditMode = true)]
        public object LastScheduledDateTimeUtc;

        [Display(Name = "Last Run End DateTime UTC")]
        [DisplayFormat(DataFormatString = "{0:yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'}", ApplyFormatInEditMode = true)]
        public object LastRunEndDateTimeUtc;

        [Required]
        [Display(Name = "Next Scheduled DateTime UTC")]
        [DisplayFormat(DataFormatString = "{0:yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'}", ApplyFormatInEditMode = true)]
        public object NextScheduledDateTimeUtc;

        [Display(Name = "Interval (In Seconds)")]
        [Range(0, Int32.MaxValue)]
        public object IntervalInSeconds;

        [Required]
        [Display(Name = "Enabled?")]
        public object Enabled;

        [Required]
        [Display(Name = "Status")]
        public object Status;

        [Display(Name = "Xml Object (Parameter)")]
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public object XmlObject { get; set; }
    }
}