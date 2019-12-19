using Icon.Common.DataAccess;

using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Icon.Common.Models;

namespace Icon.Web.Mvc.Controllers
{
    public class PickListController : Controller
    {
        private IQueryHandler<GetPickListOptionsParameters, IEnumerable<PickListModel>> getPickListOptionsQueryHandler;

        public PickListController(IQueryHandler<GetPickListOptionsParameters, IEnumerable<PickListModel>> getPickListOptionsQueryHandler)
        {
            this.getPickListOptionsQueryHandler = getPickListOptionsQueryHandler;
        }

        [HttpGet]
        public ActionResult All()
        {
            var pickListOptions = getPickListOptionsQueryHandler.Search(new GetPickListOptionsParameters { ReturnAll = true  })
                .Select(p => new PickListViewModel(p));
            return Json(pickListOptions, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     returns the following format.
        ///     {
        ///         100: [Value1, Value2]
        ///                 102: [Value6, Value7, Value8]
        ///     }
        ///     
        ///     in javascript picklist values array can be returned by calling data[< attributeid >]
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AllByAttribute()
        {
            var pickListOptions = getPickListOptionsQueryHandler.Search(new GetPickListOptionsParameters { ReturnAll = true })
                .Select(p => new PickListViewModel(p));

            var grouped = from x in pickListOptions
                group x.PickListValue by x.AttributeId
                into g
                select new {AttributeId = g.Key, Values = g.ToList()};

           var data = grouped.ToDictionary(d => d.AttributeId.ToString(), d => d.Values);

            return Json(data, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public ActionResult ByAttributeId(int attributeId)
        {
            var pickListOptions = getPickListOptionsQueryHandler.Search(new GetPickListOptionsParameters { AttributeId = attributeId })
                .Select(p => new PickListViewModel(p));
            return Json(pickListOptions, JsonRequestBehavior.AllowGet);
        }
    }
}