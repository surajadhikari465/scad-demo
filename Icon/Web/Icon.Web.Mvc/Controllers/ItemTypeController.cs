using Icon.Framework;
using System.Linq;
using System.Web.Mvc;

namespace Icon.Web.Mvc.Controllers
{
    public class ItemTypeController : Controller
    {
        public ActionResult All()
        {
            var itemTypes = ItemTypes.Descriptions.AsDictionary
                   .Select(kvp => new { ItemTypeId = kvp.Key, ItemTypeDesc = kvp.Value });
            return Json(itemTypes, JsonRequestBehavior.AllowGet);
        }
    }
}