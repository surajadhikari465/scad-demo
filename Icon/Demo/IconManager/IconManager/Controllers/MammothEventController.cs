using IconManager.EventGenerators;
using IconManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IconManager.Controllers
{
    public class MammothEventController : Controller
    {
        // GET: MammothEvent
        public ActionResult Index()
        {
            return View(new GenerateMammothEventsViewModel());
        }

        [HttpPost]
        public ActionResult GenerateEvents(GenerateMammothEventsViewModel viewModel)
        {
            MammothEventGenerator eventGenerator = new MammothEventGenerator();
            try
            {
                eventGenerator.GenerateMammothEvents(viewModel.BusinessUnit);
            }
            catch (Exception e)
            {
                if(e.Data["ContainsUserFriendlyErrorMessage"] != null)
                {
                    ViewBag.ErrorMessage = e.Message;
                }
                else
                {
                    ViewBag.ErrorMessage = "An unexpected exception occurred when generating mammoth events.";
                }
            }
            return View();
        }
    }
}