using GlobalEventController.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalEventController.Controller.EventServices
{
    public interface IBulkEventService
    {
        int? ReferenceId { get; set; }
        string Message { get; set; }
        string Region { get; set; }
        List<ValidatedItemModel> ValidatedItemList { get; set; }
        List<string> ScanCodesWithNoTaxList { get; set; }
        List<NutriFactsModel> ItemNutriFacts { get; set; }
        List<RegionalItemMessageModel> RegionalItemMessage { get; set; }
        void Run();
    }
}
