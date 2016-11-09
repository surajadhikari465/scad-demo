namespace Icon.Infor.LoadTests.Service.Controllers
{
    using Logging;
    using Models;
    using ServiceStack.Text;
    using System;
    using System.Collections.Generic;
    using System.Web.Http;

    public class TestController : ApiController
    {
        private ILogger logger;

        public TestController()
        {
            this.logger = new NLogLogger(this.GetType());
        }

        [HttpGet]
        public List<LoadTestModel> GetTests()
        {
            try
            {
                return TestService.Instance.GetAllTests();
            }
            catch(Exception ex)
            {
                logger.Error(new { Message = "Unknown error occurred.", Exception = ex.ToString() }.Dump());
                throw;
            }
        }

        [HttpGet]
        public LoadTestModel GetTest(string testName)
        {
            try
            {
                return TestService.Instance.GetTestByName(testName);
            }
            catch(Exception ex)
            {
                logger.Error(new { Message = "Unknown error occurred.", Exception = ex.ToString() }.Dump());
                throw;
            }         
        }

        [HttpPost]
        public IHttpActionResult Start(LoadTestModel loadTest)
        {
            try
            {
                var result = TestService.Instance.StartTest(loadTest);

                if (result)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest(string.Format("Test with name {0} could not be started.", loadTest?.Name));
                }
            }
            catch(Exception ex)
            {
                logger.Error(new { Message = "Unknown error occurred.", Exception = ex.ToString() }.Dump());
                throw;
            }
        }

        [HttpPost]
        public IHttpActionResult Stop(string testName)
        {
            try
            {
                var result = TestService.Instance.StopTest(testName);

                if (result)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest(string.Format("Test with name {0} could not be stopped.", testName));
                }
            }
            catch(Exception ex)
            {
                logger.Error(new { Message = "Unknown error occurred.", Exception = ex.ToString() }.Dump());
                throw;
            }
        }
    }
}
