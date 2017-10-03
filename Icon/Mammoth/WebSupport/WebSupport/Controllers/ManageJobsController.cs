using Esb.Core.EsbServices;
using Icon.Common.DataAccess;
using Icon.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using WebSupport.DataAccess.Commands;
using WebSupport.DataAccess.Models;
using WebSupport.DataAccess.Queries;
using WebSupport.Filters;
using WebSupport.Models;

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

            return View(jobSchedules);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var jobSchedule = getJobScheduleQueryHandler.Search(new GetJobScheduleParameters
            {
                JobScheduleId = id
            });

            return View(jobSchedule);
        }

        [HttpPost]
        public ActionResult Edit(JobSchedule jobSchedule)
        {
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
            return View();
        }

        [HttpPost]
        public ActionResult Create(JobSchedule jobSchedule)
        {
            createJobScheduleCommandHandler.Execute(new CreateJobScheduleCommand
            {
                JobSchedule = jobSchedule
            });

            LogJobScheduleChange("Create", jobSchedule);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Start(JobSchedule jobSchedule)
        {
            if (ModelState.IsValid)
            {
                JobScheduleModel model = new JobScheduleModel
                {
                    JobScheduleId = jobSchedule.JobScheduleId,
                    DestinationQueueName = jobSchedule.DestinationQueueName,
                    Enabled = jobSchedule.Enabled,
                    IntervalInSeconds = jobSchedule.IntervalInSeconds,
                    JobName = jobSchedule.JobName,
                    LastRunDateTimeUtc = jobSchedule.LastRunDateTimeUtc,
                    Region = jobSchedule.Region,
                    StartDateTimeUtc = jobSchedule.StartDateTimeUtc,
                    XmlObject = jobSchedule.XmlObject
                };

                var response = startJobService.Send(model);
                if (response.Status == EsbServiceResponseStatus.Failed)
                {
                    ViewBag.Error = response.ErrorDetails;
                    LogJobScheduleChange("Start", jobSchedule, response.ErrorDetails);
                }
                else
                {
                    LogJobScheduleChange("Start", jobSchedule);
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