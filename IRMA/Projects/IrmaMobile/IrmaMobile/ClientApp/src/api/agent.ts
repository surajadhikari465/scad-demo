import axios, { AxiosResponse } from "axios";
import { toast } from "react-toastify";
import Config from "../config";
import ITransferOrder from '../pages/Transfer/types/ITransferOrder'
import { IStore, IUser, IShrinkAdjustment, IStoreItem, IExternalOrder } from "../store";
import { IPurchaseOrderResult } from "../pages/Receive/PurchaseOrder/types/PurchaseOrderResult";

axios.defaults.baseURL = Config.baseUrl;
axios.defaults.timeout = 60000; //1 minute

axios.interceptors.response.use(undefined, (error: { message: string; stack: string; }) => {
    toast.error(`Error: ${error.message}`);

    return Promise.reject(error.message + "\n" + error.stack);
});

const responseBody = async (response: AxiosResponse) => (await response.data);

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
    getStores: async (region: string, useVendorIdAsStoreNo: boolean = false) => await requests.get(`/${region}/stores?useVendorIdAsStoreNo=${useVendorIdAsStoreNo}`) as IStore[],
    getShrinkSubtypes: async (region: string) => await requests.get(`/${region}/shrinksubtypes/`),
    getShrinkAdjustmentReasons: async (region: string) => await requests.get(`/${region}/shrinkAdjustments/`) as IShrinkAdjustment[]
};

const StoreItem = {
    getStoreItem: async (region: string, storeNumber: number | string, subteamNo: number | string, userId: number, upc: string | number) => await requests.get(`/${region}/storeitems?storeNo=${storeNumber}&subteamNo=${subteamNo}&userId=${userId}&scanCode=${upc}`) as IStoreItem
}

const PurchaseOrder = {
    getOrder: async (region: string, purchaseOrderNum: number) =>
        await requests.get(
            `/${region}/purchaseorder/PurchaseOrder?purchaseOrderNum=${purchaseOrderNum}`
        ),
    getExternalOrders: async (region: string, purchaseOrderNum: number, storeNumber: string) =>
        await requests.get(
            `/${region}/purchaseorder/ExternalPurchaseOrders?externalOrderNumber=${purchaseOrderNum}&storeNumber=${storeNumber}`
        ) as IExternalOrder[],
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
    reOpenOrder: async (region: string, orderId: number) => await requests.post(`/${region}/PurchaseOrder/ReopenOrder`, { OrderId: orderId }),
    updateReceivingDiscrepancyCode: async (region: string, orderItemId: number, reasonCodeId: number) : Promise<IPurchaseOrderResult> => await requests.post(`/${region}/PurchaseOrder/UpdateReceivingDiscrepancyCode`, { orderItemId, reasonCodeId })
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
    removeInvoiceCharge: async (region: string, chargeId: number) => await requests.post(`/${region}/invoicedata/removeinvoicecharge`, { ChargeId: chargeId }),
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
    updateOrderBeforeClosing: async (region: string, orderId: number, invoiceNumber: string, invoiceDate: Date | undefined, invoiceCost: number, vendorDocId: string, vendorDocDate: Date | undefined, subteamNo: number, partialShipment: boolean) =>
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
            }),
    updateOrderHeaderCosts: async (region: string, orderId: number) => await requests.get(`/${region}/invoicedata/updateOrderHeaderCosts?orderId=${orderId}`)
};

const ReceivingList = {
    getReceivingListEinvoiceExceptions: async (region: string, orderId: number) =>
        await requests.get(`/${region}/receivinglist/ReceivingListEinvoiceExceptions?orderId=${orderId}`),

}

const Document = {
    getVendors: async (region: string, storeNo: number) => await requests.get(`/${region}/document/vendors?storeNo=${storeNo}`),
    isDuplicateReceivingDocumentInvoiceNumber: async (region: string, invoiceNumber: string, vendorId: number) =>
        await requests.get(`/${region}/document/isDuplicateReceivingDocumentInvoiceNumber?invoiceNumber=${invoiceNumber}&vendorId=${vendorId}`)
}

const Transfer = {
    getSubteamByProductType: async (region: string, productTypeId: number) => await requests.get(`/${region}/transfer/subteamByProductType?productTypeId=${productTypeId}`),
    getTransferItem: async (region: string, upc: string, productType: number, storeNo: number, vendorId: number, subteam: number, supplyTeam: number) =>
        await requests.get(`/${region}/transfer/transferItem?upc=${upc}&productType=${productType}&storeNo=${storeNo}&vendorId=${vendorId}&subteam=${subteam}&supplyTeam=${supplyTeam}`),
    getReasonCodes: async (region: string) => await requests.get(`${region}/transfer/reasonCodes`),
    createTransferOrder: async (region: string, transferOrder: ITransferOrder) => await requests.post(`/${region}/transfer/createtransferorder`, transferOrder)
}

const Shrink = {
    submitShrinkItems: async (region: string, itemKey: any, storeNo: any, subteamNo: any, shrinkSubTypeId: any, adjustmentId: any, adjustmentReason: any, createdByUserId: any, userName: any, inventoryAdjustmentCodeAbbreviation: any, quantity: any, weight: any) =>
        await requests.post(`/${region}/shrinkadjustments`, {
            itemKey,
            storeNo,
            subteamNo,
            shrinkSubTypeId,
            adjustmentId,
            adjustmentReason,
            createdByUserId,
            userName,
            inventoryAdjustmentCodeAbbreviation,
            quantity,
            weight
        })
};

const User = {
    getUser: async (region: string, userName: string) => await requests.get(`/${region}/users?userName=${userName}`) as IUser | null
}

export default { RegionSelect, StoreItem, PurchaseOrder, InvoiceData, Shrink, ReceivingList, Document, Transfer, User };
