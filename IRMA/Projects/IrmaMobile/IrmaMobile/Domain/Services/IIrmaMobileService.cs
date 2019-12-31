﻿using System.Collections.Generic;
using System.Threading.Tasks;
using IrmaMobile.Domain.Models;
using IrmaMobile.Legacy;

namespace IrmaMobile.Services
{
    public interface IIrmaMobileService
    {
        Task<List<StoreModel>> GetStoresAsync(string region);
        Task<List<SubteamModel>> GetSubteamsAsync(string region);
        Task<List<ShrinkSubTypeModel>> GetShrinkSubTypesAsync(string region);
        Task<StoreItemModel> GetStoreItemAsync(string region, int storeNo, int subteamNo, int? userId, string scanCode);
        Task<Order> GetPurchaseOrderAsync(string region, long poNum);
        Task<List<Order>> GetPurchaseOrdersAsync(string region, string upc, int storeNumber);
        Task<List<ReasonCode>> GetReasonCodesAsync(string region);
        Task<Result> ReceiveOrderAsync(string region, ReceiveOrderModel model);
        Task<List<InvoiceCharge>> GetOrderInvoiceChargesAsync(string region, int orderId);
        Task<Result> CloseOrderAsync(string region, int orderId, int userId);
        Task<List<InvoiceCharge>> GetAllocatedInvoiceChargesAsync(string region);
        Task<List<ListsSubteam>> GetNonallocatedInvoiceChargesAsync(string region, int orderId);
        Task<bool> AddShrinkAdjustment(string region, ShrinkAdjustmentModel shrinkAdjustmentModel);
        Task<Result> AddInvoiceChargeAsync(string region, InvoiceChargeModel model);
        Task<Result> RemoveInvoiceChargeAsync(string region, int chargeId);
        Task<List<Currency>> GetCurrenciesAsync(string region);
    }
}