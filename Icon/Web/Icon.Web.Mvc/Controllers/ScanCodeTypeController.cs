using Icon.Framework;
using System.Linq;
using System.Web.Mvc;

namespace Icon.Web.Mvc.Controllers
{
    public class ScanCodeTypeController : Controller
    {
        public ActionResult All()
        {
            var scanCodeTypes = ScanCodeTypes.Descriptions.AsDictionary
                .Select(kvp => new { ScanCodeTypeId = kvp.Key, ScanCodeTypeDesc = kvp.Value });
            return Json(scanCodeTypes, JsonRequestBehavior.AllowGet);
        }
    }
}