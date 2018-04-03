using Mammoth.Price.Controller.DataAccess.Models;
using System.Collections.Generic;

namespace Mammoth.Price.Controller.ApplicationModules
{
    public interface IErrorAlerter
    {
        void AlertErrors(List<PriceEventModel> priceEventModels);
        void AlertErrors(List<CancelAllSalesEventModel> cancelAllSalesEventModels);
    }
}
