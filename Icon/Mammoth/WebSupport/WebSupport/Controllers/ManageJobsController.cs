using Esb.Core.EsbServices;
using Icon.Common.DataAccess;
using Icon.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebSupport.DataAccess.Commands;
using WebSupport.DataAccess.Models;
using WebSupport.DataAccess.Queries;
using WebSupport.Filters;
using WebSupport.Models;
using WebSupport.ViewModels;

namespace WebSupport.Controllers
{
    [ErrorLogActionFilter]
    public class ManageJobsController : Controller
    {
        private IQueryHandler<GetJobSchedulesParameters, List<JobSchedule>> getJobSchedulesQueryHandler;
        private IQueryHandler<GetJobScheduleParameters, JobSchedule> getJobScheduleQueryHandler;
        private ICommandHandler<UpdateJobScheduleCommand> updateJobScheduleCommandHandler;
        private ICommandHandler<DeleteJobScheduleCommand> deleteJobScheduleCommandHandler;
        private ICommandHandler<CreateJobScheduleCommand> createJobScheduleCommandHandler;
        private IEsbService<JobScheduleModel> startJobService;
        private ILogger logger;

        public ManageJobsController(
            IQueryHandler<GetJobSchedulesParameters, List<JobSchedule>> getJobSchedulesQueryHandler,
            IQueryHandler<GetJobScheduleParameters, JobSchedule> getJobScheduleQueryHandler,
            ICommandHandler<UpdateJobScheduleCommand> updateJobScheduleCommandHandler,
            ICommandHandler<DeleteJobScheduleCommand> deleteJobScheduleCommandHandler,
            ICommandHandler<CreateJobScheduleCommand> createJobScheduleCommandHandler,
            IEsbService<JobScheduleModel> startJobService,
            ILogger logger)
        {
            this.getJobSchedulesQueryHandler = getJobSchedulesQueryHandler;
            this.getJobScheduleQueryHandler = getJobScheduleQueryHandler;
            this.updateJobScheduleCommandHandler = updateJobScheduleCommandHandler;
            this.deleteJobScheduleCommandHandler = deleteJobScheduleCommandHandler;
            this.createJobScheduleCommandHandler = createJobScheduleCommandHandler;
            this.startJobService = startJobService;
            this.logger = logger;
        }

        public ActionResult Index()
        {
            List<JobSchedule> jobSchedules = getJobSchedulesQueryHandler.Search(new GetJobSchedulesParameters());
            var jobScheduleViewModels = jobSchedules.Select(m => JobScheduleViewModel.FromDataAccessModel(m));

            return View(jobScheduleViewModels);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var jobSchedule = getJobScheduleQueryHandler.Search(new GetJobScheduleParameters
            {
                JobScheduleId = id
            });
            var jobScheduleViewModel = JobScheduleViewModel.FromDataAccessModel(jobSchedule);

            return View(jobScheduleViewModel);
        }

        [HttpPost]
        public ActionResult Edit(JobScheduleViewModel jobSchedule)
        {
            PopulateUtcDates(jobSchedule);
            updateJobScheduleCommandHandler.Execute(new UpdateJobScheduleCommand
            {
                JobSchedule = jobSchedule
            });

            LogJobScheduleChange("Update", jobSchedule);

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var jobSchedule = getJobScheduleQueryHandler.Search(new GetJobScheduleParameters
            {
                JobScheduleId = id
            });
            deleteJobScheduleCommandHandler.Execute(new DeleteJobScheduleCommand
            {
                JobScheduleId = id
            });

            LogJobScheduleChange("Delete", jobSchedule);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Create()
        {
            var jobSchedule = new JobScheduleViewModel()
            {
                StartDateTime = DateTime.Now,
                NextScheduledDateTime = DateTime.Now
            };
            return View(jobSchedule);
        }

        [HttpPost]
        public ActionResult Create(JobScheduleViewModel jobSchedule)
        {
            PopulateUtcDates(jobSchedule);
            createJobScheduleCommandHandler.Execute(new CreateJobScheduleCommand
            {
                JobSchedule = jobSchedule
            });

            LogJobScheduleChange("Create", jobSchedule);

            return RedirectToAction("Index");
        }

        private void PopulateUtcDates(JobScheduleViewModel jobSchedule)
        {
            jobSchedule.NextScheduledDateTimeUtc = jobSchedule.NextScheduledDateTime.ToUniversalTime();
            jobSchedule.StartDateTimeUtc = jobSchedule.StartDateTime.ToUniversalTime();
        }

        [HttpGet]
        public ActionResult Start(int id)
        {
            var jobSchedule = getJobScheduleQueryHandler.Search(new GetJobScheduleParameters
            {
                JobScheduleId = id
            });
            var jobScheduleViewModel = JobScheduleViewModel.FromDataAccessModel(jobSchedule);

            JobScheduleModel model = new JobScheduleModel
            {
                JobScheduleId = jobScheduleViewModel.JobScheduleId,
                DestinationQueueName = jobScheduleViewModel.DestinationQueueName,
                Enabled = jobScheduleViewModel.Enabled,
                IntervalInSeconds = jobScheduleViewModel.IntervalInSeconds,
                JobName = jobScheduleViewModel.JobName,
                LastScheduledDateTimeUtc = jobScheduleViewModel.LastScheduledDateTimeUtc,
                LastRunEndDateTimeUtc = jobScheduleViewModel.LastRunEndDateTimeUtc,
                NextScheduledDateTimeUtc = jobScheduleViewModel.NextScheduledDateTimeUtc,
                Status = jobScheduleViewModel.Status,
                Region = jobScheduleViewModel.Region,
                StartDateTimeUtc = jobScheduleViewModel.StartDateTimeUtc,
                XmlObject = jobScheduleViewModel.XmlObject
            };

            if (ModelState.IsValid)
            {
                var response = startJobService.Send(model);
                if (response.Status == EsbServiceResponseStatus.Failed)
                {
                    ViewBag.Error = response.ErrorDetails;
                    LogJobScheduleChange("Start", jobSchedule, response.ErrorDetails);
                }
                else
                {
                    LogJobScheduleChange("Start", jobSchedule);

                    //update next scheduled run datetime if the job scheduler gets stuck, and an adhoc run is kicked off
                    if (DateTime.UtcNow > jobSchedule.NextScheduledDateTimeUtc)
                    {
                        var secondsInDifference = Math.Ceiling((DateTime.UtcNow - jobSchedule.NextScheduledDateTimeUtc).TotalSeconds / jobSchedule.IntervalInSeconds) * jobSchedule.IntervalInSeconds;
                        jobSchedule.NextScheduledDateTimeUtc = jobSchedule.NextScheduledDateTimeUtc.AddSeconds(secondsInDifference);

                        updateJobScheduleCommandHandler.Execute(new UpdateJobScheduleCommand
                        {
                            JobSchedule = jobSchedule
                        });

                        LogJobScheduleChange("Update", jobSchedule);
                    }
                }
            }

            return RedirectToAction("Index");
        }

        private void LogJobScheduleChange(string action, JobSchedule jobSchedule, string error = null)
        {
            logger.Info(JsonConvert.SerializeObject(new
            {
                Action = action,
                JobSchedule = jobSchedule,
                Error = error
            }));
        }
    }
}