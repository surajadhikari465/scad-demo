using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Icon.Web.Mvc.Controllers
{
    public class BarcodeTypeController : Controller
    {
        private IQueryHandler<GetBarcodeTypeParameters, List<BarcodeTypeModel>> getBarcodeTypesQueryHandler;

        public BarcodeTypeController(IQueryHandler<GetBarcodeTypeParameters, List<BarcodeTypeModel>> getBarcodeTypesQueryHandler)
        {
            this.getBarcodeTypesQueryHandler = getBarcodeTypesQueryHandler;
        }

        [HttpGet]
        public ActionResult All()
        {
            var pickListOptions = getBarcodeTypesQueryHandler.Search(new GetBarcodeTypeParameters { });
            return Json(pickListOptions, JsonRequestBehavior.AllowGet);
        }
    }
}