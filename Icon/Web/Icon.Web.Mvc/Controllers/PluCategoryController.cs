using DevTrends.MvcDonutCaching;
using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Models;
using Infragistics.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Icon.Web.Controllers
{
    public class PluCategoryController : Controller
    {
        private readonly IQueryHandler<GetBarcodeTypeParameters, List<BarcodeTypeModel>> getBarcodeTypesQueryHandler;

        public PluCategoryController(
            IQueryHandler<GetBarcodeTypeParameters, List<BarcodeTypeModel>> getBarcodeTypesQueryHandler)
        {
            this.getBarcodeTypesQueryHandler = getBarcodeTypesQueryHandler;
        }

        //
        // GET: /PluCategory/
        [DonutOutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Index()
        {
            // Get PLU categoty range data from Database
            var barcodeTypesList = getBarcodeTypesQueryHandler.Search(new GetBarcodeTypeParameters { });

            // Populate ViewModel
            BarcodeTypeGridViewModel viewModel = new BarcodeTypeGridViewModel();

            viewModel.BarcodeTypes = barcodeTypesList
                .Select(bt => new BarcodeTypeViewModel
                    {
                        BarcodeTypeId = bt.BarcodeTypeId,
                        BarcodeType = bt.BarcodeType,
                        BeginRange = bt.BeginRange?.ToString(),
                        EndRange = bt.EndRange?.ToString(),
                        ScalePlu = bt.ScalePlu
                    });

            return View(viewModel);
        }
    }
}
