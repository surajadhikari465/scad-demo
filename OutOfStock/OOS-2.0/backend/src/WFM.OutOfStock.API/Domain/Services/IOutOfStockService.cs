using System.Collections.Generic;
using System.Threading.Tasks;
using WFM.OutOfStock.API.Domain.Request;
using WFM.OutOfStock.API.Domain.Response;

namespace WFM.OutOfStock.API.Services
{
    public interface IOutOfStockService
    {
        Task<List<StoreResponse>> RetrieveStoresForRegion(string regionCode);
        Task SubmitList(UploadItemsRequest list);
    }
}