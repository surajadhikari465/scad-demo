using Icon.Common;
using Icon.Infor.LoadTests.Service.Models;
using Icon.Infor.LoadTests.Web.Models;
using Icon.Logging;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Icon.Infor.LoadTests.Web.Controllers
{
    public class HomeController : Controller
    {
        private const string timeFormat = "d':'hh':'mm':'ss";
        private string serviceUrl;
        private ILogger logger;

        public HomeController()
        {
            serviceUrl = AppSettingsAccessor.GetStringSetting("ServiceUrl");
            logger = new NLogLogger(this.GetType());
        }

        // GET: Home
        public ActionResult Index()
        {
            try
            {
                ViewBag.Message = TempData["Message"];

                var client = new RestClient(serviceUrl);

                var result = client.Execute<List<LoadTestModel>>(new RestRequest("GetTests", Method.GET));

                return View(result.Data.Select(t => new LoadTestViewModel(t)).ToList());
            }
            catch (Exception ex)
            {
                logger.Info("Unexpected error occurred. " + ex.ToString());
                ViewBag.Message = "Unable to load tests. An unexpected error occured when getting tests. Check logs for details.";
                return View();
            }
        }

        public ActionResult Details(string testName)
        {
            try
            {
                var client = new RestClient(serviceUrl);
                var request = new RestRequest("GetTest", Method.GET);

                request.AddParameter("testName", testName, ParameterType.QueryString);

                var result = client.Execute<LoadTestModel>(request);

                return View(new LoadTestViewModel(result.Data));
            }
            catch (Exception ex)
            {
                logger.Info("Unexpected error occurred when getting test " + testName + ". " + ex.ToString());

                TempData["Message"] = "Unable to get test details. An unexpected error occured when getting test " + testName + ". Check logs for details.";

                return RedirectToAction("Index");
            }
        }

        public ActionResult Start(LoadTestViewModel loadTest)
        {
            var client = new RestClient(serviceUrl);
            var request = new RestRequest("Start", Method.POST);

            request.AddJsonBody(new LoadTestModel
            {
                Name = loadTest.Name,
                EntityCount = loadTest.EntityCount,
                EmailRecipients = loadTest.EmailRecipients,
                PopulateTestDataInterval = loadTest.PopulateTestDataInterval,
                TestRunTime = loadTest.TestRunTime
            });

            var result = client.Execute(request);

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("Index");
            }
            else if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                logger.Info(result.ErrorMessage);
                TempData["Message"] = result.ErrorMessage;
            }

            return RedirectToAction("Details", new { testName = loadTest.Name });
        }

        public ActionResult Stop(LoadTestViewModel loadTest)
        {
            var client = new RestClient(serviceUrl);
            var request = new RestRequest("Stop", Method.POST);

            request.AddParameter("testName", loadTest.Name, ParameterType.QueryString);

            var result = client.Execute(request);

            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                ViewBag.Message = result.ErrorMessage;
            }

            return RedirectToAction("Index");
        }
    }
}