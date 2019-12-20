using IrmaMobile.Domain.Exceptions;
using IrmaMobile.Domain.Models;
using IrmaMobile.Legacy;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;

namespace IrmaMobile.Services
{
    public class IrmaMobileService : IIrmaMobileService
    {
        private ILogger<IrmaMobileService> logger;
        private readonly Dictionary<string, string> serviceUris;

        public IrmaMobileService(IOptions<AppConfiguration> options, ILogger<IrmaMobileService> logger)
        {
            this.serviceUris = options.Value.ServiceUris;
            this.logger = logger;
        }

        public async Task<List<SubteamModel>> GetSubteamsAsync(string region)
        {
            var serviceSubteams = await MakeServiceRequest(region, client => client.GetSubteamsAsync());
            var subteams = serviceSubteams.Select(s => new SubteamModel
            {
                IsFixedSpoilage = s.SubteamIsFixedSpoilage,
                IsUnrestricted = s.SubteamIsUnrestricted,
                SubteamName = s.SubteamName,
                SubteamNo = s.SubteamNo,
                SubteamTypeId = s.SubteamType
            }).ToList();

            return subteams;
        }

        public async Task<List<StoreModel>> GetStoresAsync(string region)
        {
            var serviceStores = await MakeServiceRequest(region, client => client.GetStoresAsync(false));
            var subteams = serviceStores.Select(s => new StoreModel
            {
                Name = s.StoreName,
                StoreNo = s.StoreNo
            }).ToList();

            return subteams;
        }

        public async Task<List<ShrinkSubTypeModel>> GetShrinkSubTypesAsync(string region)
        {
            var serviceShrinkSubTypes = await MakeServiceRequest(region, client => client.GetShrinkSubTypesAsync());
            var shrinkSubTypes = serviceShrinkSubTypes.Select(s => new ShrinkSubTypeModel
            {
                Abbreviation = s.Abbreviation,
                InventoryAdjustmentCodeId = s.InventoryAdjustmentCodeID,
                LastUpdateDateTime = s.LastUpdateDateTime,
                LastUpdateUserId = s.LastUpdateUserId,
                ReasonCode = s.ReasonCode,
                ShrinkSubTypeId = s.ShrinkSubTypeID,
                ShrinkSubTypeMember = s.ShrinkSubTypeMember,
                ShrinkType = s.ShrinkType
            }).ToList();

            return shrinkSubTypes;
        }

        public async Task<StoreItemModel> GetStoreItemAsync(string region, int storeNo, int subteamNo, int? userId, string scanCode)
        {
            //TODO: update userId to not be hardcoded to 1 when we implement authentication
            // Send 0 for the ItemKey because it is ignored by the Legacy service
            var serviceStoreItem = await MakeServiceRequest(region, client => client.GetStoreItemAsync(storeNo, subteamNo, 1, 0, scanCode));
            return new StoreItemModel
            {
                AverageCost = serviceStoreItem.AvgCost,
                CanInventory = serviceStoreItem.CanInventory,
                CostedByWeight = serviceStoreItem.CostedByWeight,
                DiscontinueItem = serviceStoreItem.DiscontinueItem,
                GlAccount = serviceStoreItem.GLAcct,
                HfmItem = serviceStoreItem.HFMItem,
                Identifier = serviceStoreItem.Identifier,
                IsItemAuthorized = serviceStoreItem.IsItemAuthorized,
                IsSellable = serviceStoreItem.IsSellable,
                ItemDescription = serviceStoreItem.ItemDescription,
                ItemKey = serviceStoreItem.ItemKey,
                Multiple = serviceStoreItem.Multiple,
                NotAvailable = serviceStoreItem.NotAvailable,
                OnSale = serviceStoreItem.OnSale,
                PackageDesc1 = serviceStoreItem.PackageDesc1,
                PackageDesc2 = serviceStoreItem.PackageDesc2,
                PackageUnitAbbreviation = serviceStoreItem.PackageUnitAbbr,
                PosDescription = serviceStoreItem.POSDescription,
                Price = serviceStoreItem.Price,
                PriceChangeTypeDescription = serviceStoreItem.PriceChgTypeDesc,
                RetailSale = serviceStoreItem.RetailSale,
                RetailSubteamName = serviceStoreItem.RetailSubteamName,
                RetailSubteamNo = serviceStoreItem.RetailSubteamNo,
                RetailUnitId = serviceStoreItem.retailUnitId,
                RetailUnitName = serviceStoreItem.retailUnitName,
                SaleEarnedDisc1 = serviceStoreItem.SaleEarnedDisc1,
                SaleEarnedDisc2 = serviceStoreItem.SaleEarnedDisc2,
                SaleEarnedDisc3 = serviceStoreItem.SaleEarnedDisc3,
                SaleEndDate = serviceStoreItem.SaleEndDate,
                SaleMultiple = serviceStoreItem.SaleMultiple,
                SalePrice = serviceStoreItem.SalePrice,
                SaleStartDate = serviceStoreItem.SaleStartDate,
                SignDescription = serviceStoreItem.SignDescription,
                SoldByWeight = serviceStoreItem.SoldByWeight,
                StoreNo = serviceStoreItem.StoreNo,
                StoreVendorID = serviceStoreItem.StoreVendorID,
                TransferToSubteamName = serviceStoreItem.TransferToSubteamName,
                TransferToSubteamNo = serviceStoreItem.TransferToSubteamNo,
                UserId = serviceStoreItem.UserID,
                VendorCost = serviceStoreItem.vendorCost,
                VendorId = serviceStoreItem.VendorID,
                VendorName = serviceStoreItem.VendorName,
                VendorPack = serviceStoreItem.vendorPack,
                VendorUnitId = serviceStoreItem.VendorUnitId,
                VendorUnitName = serviceStoreItem.VendorUnitName,
                WfmItem = serviceStoreItem.WFMItem
            };
        }

        public async Task<Order> GetPurchaseOrderAsync(string region, long poNum)
        {
            if(poNum > int.MaxValue)
            {
                return null;
            }

            var order = await MakeServiceRequest(region, client => client.GetOrderAsync(poNum));
            return order;
        }

        public async Task<List<Order>> GetPurchaseOrdersAsync(string region, string upc, int storeNumber)
        {
            var orders = await MakeServiceRequest(region, client => client.GetOrderHeaderByIdentifierAsync(upc, storeNumber));
            return orders;
        }

        public async Task<List<ReasonCode>> GetReasonCodesAsync(string region)
        {
            var reasonCodes = await MakeServiceRequest(region, client => client.GetReasonCodesByTypeAsync("RD"));
            return reasonCodes;
        }

        public async Task<Result> ReceiveOrderAsync(string region, ReceiveOrderModel model)
        {
            var result = await MakeServiceRequest(region, client => client.ReceiveOrderItemAsync(model.Quantity, model.Weight, model.Date, model.Correction, model.OrderItemId, model.ReasonCodeId, model.PackSize, model.UserId));
            return result;
        }

        public async Task<List<InvoiceCharge>> GetOrderInvoiceChargesAsync(string region, int orderId)
        {
            var result = await MakeServiceRequest(region, client => client.GetOrderInvoiceChargesAsync(orderId));
            return result;
        }

        public async Task<Result> CloseOrder(string region, int orderId, int userId)
        {
            var result = await MakeServiceRequest(region, client => client.CloseOrderAsync(orderId, userId));
            return result;
        }


        // Following best practices for handling WCF ServiceClient lifecycle as documented here:
        // https://docs.microsoft.com/en-us/dotnet/framework/wcf/samples/use-close-abort-release-wcf-client-resources
        private async Task<T> MakeServiceRequest<T>(string region, Func<IGateway, Task<T>> request)
        {
            var serviceClient = new GatewayClient(
                new BasicHttpBinding(BasicHttpSecurityMode.None),
                new EndpointAddress(serviceUris[region]));

            try
            {
                var result = await request(serviceClient);

                await serviceClient.CloseAsync();
                return result;
            }
            catch (CommunicationException cex)
            {
                logger.LogError(cex, "Error occurred when communicating with legacy service.");
                serviceClient.Abort();
                throw new ServiceCommunicationException(
                    "Encountered an error communicating with downstream IRMA WCF service.", cex);
            }
            catch (TimeoutException tex)
            {
                logger.LogError(tex, "Error occurred when communicating with legacy service.");
                serviceClient.Abort();
                throw new ServiceCommunicationException("Connection to IRMA WCF service timed out.", tex);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred when communicating with legacy service.");
                serviceClient.Abort();
                throw;
            }
        }

        private async Task MakeServiceRequest(string region, Func<IGateway, Task> request)
        {
            var serviceClient = new GatewayClient(
                new BasicHttpBinding(BasicHttpSecurityMode.None),
                new EndpointAddress(serviceUris[region]));

            try
            {
                await request(serviceClient);

                await serviceClient.CloseAsync();
            }
            catch (CommunicationException cex)
            {
                logger.LogError(cex, "Error occurred when communicating with legacy service.");
                serviceClient.Abort();
                throw new ServiceCommunicationException(
                    "Encountered an error communicating with downstream WCF service.", cex);
            }
            catch (TimeoutException tex)
            {
                logger.LogError(tex, "Error occurred when communicating with legacy service.");
                serviceClient.Abort();
                throw new ServiceCommunicationException("Connection to WCF service timed out.", tex);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred when communicating with legacy service.");
                serviceClient.Abort();
                throw;
            }
        }
    }
}
