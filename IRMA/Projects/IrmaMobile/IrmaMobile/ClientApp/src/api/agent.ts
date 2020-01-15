import axios, { AxiosResponse } from "axios";
import { toast } from "react-toastify";
import Config from "../config";

axios.defaults.baseURL = Config.baseUrl;

axios.interceptors.response.use(undefined, error => {
    toast.error(`Error: ${error.message}`);

    return Promise.reject(error.message + "\n" + error.stack);
});

const responseBody = async (response: AxiosResponse) =>( await response.data );

const requests = {
    get: async (url: string) =>
        await axios
            .get(url)
            .then(responseBody)
            .catch(e => {
                console.log(e);
            }),
    post: async (url: string, body: {}) =>
        await axios
            .post(url, body)
            .then(responseBody)
            .catch(e => {
                console.log(e);
            }),
    put: async (url: string, body: {}) =>
        await axios
            .put(url, body)
            .then(responseBody)
            .catch(e => {
                console.log(e);
            }),
    del: async (url: string) => 
        await axios
            .delete(url)
            .then(responseBody)
            .catch(e => {
                console.log(e);
            })
};

const RegionSelect = {
    getStores: async(region:string) => await requests.get(`/${region}/stores/`),
    getShrinkSubtypes: async(region:string) => await requests.get(`/${region}/shrinksubtypes/`)
};   

const PurchaseOrder = {
    detailsFromPurchaseOrderNumber: async (
        region: string,
        purchaseOrderNum: number
    ) =>
        await requests.get(
            `/${region}/purchaseorder/PurchaseOrder?purchaseOrderNum=${purchaseOrderNum}`
        ),
    detailsFromUpc: async (region: string, upc: string, store: string) =>
        await requests.get(
            `/${region}/purchaseorder/PurchaseOrders?upc=${upc}&storeNumber=${store}`
        ),
    reasonCodes: async (region: string) =>
        await requests.get(`/${region}/purchaseorder/ReasonCodes`),
    receiveOrder: async (
        region: string,
        quantity: number,
        weight: number,
        date: Date,
        correction: boolean,
        orderItemId: number,
        reasonCodeId: number,
        packSize: number,
        userId: number
    ) =>
        await requests.post(`/${region}/purchaseorder/receiveorder`, {
            quantity,
            weight,
            date,
            correction,
            orderItemId,
            reasonCodeId,
            packSize,
            userId
        }),
    reopenOrder: async (region: string, orderId: number) => await requests.post(`/${region}/PurchaseOrder/ReopenOrder`, { OrderId: orderId })
};

const InvoiceData = {
    getCurrencies: async (region: string) =>
        await requests.get(`/${region}/invoicedata/Currencies`),
    getOrderInvoiceCharges: async (region: string, orderId: number) =>
        await requests.get(
            `/${region}/invoicedata/orderInvoiceCharges?orderId=${orderId}`
        ),
    getAllocatedInvoiceCharges: async (region: string) => 
        await requests.get(`/${region}/invoicedata/allocatedInvoiceCharges`),
    getNonallocatedInvoiceCharges: async (region: string, orderId: number) => 
        await requests.get(`/${region}/invoicedata/nonallocatedInvoiceCharges?orderId=${orderId}`),
    addInvoiceCharge: async (region: string, orderId: number, sacType: number, description: string, subteamGlAccount: number, allowance: boolean, chargeValue: number) =>
        await requests.post(`/${region}/invoicedata/addinvoicecharge`, {
            OrderId: orderId,
            SacType: sacType,
            Description: description,
            SubteamGlAccount: subteamGlAccount,
            Allowance: allowance,
            ChargeValue: chargeValue
        }),
    removeInvoiceCharge: async (region: string, chargeId: number) => await requests.post(`/${region}/invoicedata/removeinvoicecharge`, {ChargeId: chargeId}),
    getRefuseCodes: async (region: string) => await requests.get(`/${region}/invoicedata/refusecodes`),
    refuseOrder: async (region: string, orderId: number, userId: number, reasonCodeId: number) => 
        await requests.post(`/${region}/invoicedata/refuseorder`,
        {
            OrderId: orderId,
            UserId: userId,
            ReasonCodeId: reasonCodeId    
        }),
    reparseEInvoice: async (region: string, eInvId: number) => await requests.post(`/${region}/invoicedata/reparseEInvoice`, { EInvId: eInvId }),
    closeOrder: async (region: string, orderId: number, userId: number) => await requests.post(`/${region}/invoicedata/closeorder`, { OrderId: orderId, UserId: userId }),
    updateOrderBeforeClosing: async (region: string, orderId: number, invoiceNumber: string, invoiceDate: Date|undefined, invoiceCost: number, vendorDocId: string, vendorDocDate: Date|undefined, subteamNo: number, partialShipment: boolean) => 
        await requests.post(`/${region}/invoicedata/updateorderbeforeclosing`, 
        {
            OrderId: orderId,
            InvoiceNumber: invoiceNumber,
            InvoiceDate: invoiceDate,
            InvoiceCost: invoiceCost,
            VendorDocId: vendorDocId,
            VendorDocDate: vendorDocDate,
            SubteamNo: subteamNo,
            PartialShipment: partialShipment
        })
};

const ReceivingList = {
    getReceivingListEinvoiceExceptions: async (region: string, orderId: number) => 
        await requests.get(`/${region}/receivinglist/ReceivingListEinvoiceExceptions?orderId=${orderId}`),

}

const Document = {
    getVendors: async (region: string, storeNo: number) => await requests.get(`/${region}/document/vendors?storeNo=${storeNo}`)

}

const Shrink = {
    shrinkItemSubmit: async(region: string, ItemKey: any, StoreNo: any, SubteamNo: any, ShrinkSubTypeId: any, AdjustmentId: any, AdjustmentReason: any, CreatedByUserId: any, UserName: any, InventoryAdjustmentCodeAbbreviation:any, Quantity:any, Weight: any) =>
        await requests.post(`/${region}/shrinkadjustments`, {
                ItemKey,
                StoreNo,
                SubteamNo,
                ShrinkSubTypeId,
                AdjustmentId,
                AdjustmentReason,
                CreatedByUserId,
                UserName,
                InventoryAdjustmentCodeAbbreviation,
                Quantity,
                Weight
            })
        
};

export default { RegionSelect, PurchaseOrder, InvoiceData, Shrink, ReceivingList, Document };
