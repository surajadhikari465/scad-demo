using System.Web.Mvc;

namespace Icon.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Item");
        }
    }
}