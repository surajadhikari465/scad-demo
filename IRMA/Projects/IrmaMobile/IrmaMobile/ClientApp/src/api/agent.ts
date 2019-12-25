import axios, { AxiosResponse } from "axios";
import { toast } from "react-toastify";
import { Config } from "../config";

axios.defaults.baseURL = Config.baseUrl;

axios.interceptors.response.use(undefined, error => {
    toast.error(`Error: ${error.message}`);

    return Promise.reject(error.message + "\n" + error.stack);
});

const responseBody = async (response: AxiosResponse) => await response.data;

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
        await axios.put(url, body).then(responseBody),
    del: async (url: string) => await axios.delete(url).then(responseBody)
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
        })
};

const PurchaseOrderClose = {
    getCurrencies: async (region: string) =>
        await requests.get(`/${region}/purchaseorder/getCurrencies`),
    getOrderInvoiceCharges: async (region: string, orderId: number) =>
        await requests.get(
            `/${region}/purchaseorder/orderInvoiceCharges?orderId=${orderId}`
        ),
    closeOrder: async (region: string, orderId: number, userId: number) =>
        await requests.post(
            `/${region}/purchaseorder/closeOrder`, {orderId, userId}
        ),
    getAllocatedInvoiceCharges: async (region: string) => 
        await requests.get(`/${region}/purchaseorder/allocatedInvoiceCharges`),
    getNonallocatedInvoiceCharges: async (region: string, orderId: number) => 
        await requests.get(`/${region}/purchaseorder/nonallocatedInvoiceCharges?orderId=${orderId}`),
    addInvoiceCharge: async (region: string, OrderId: number, SacType: number, Description: string, SubteamGlAccount: number, Allowance: boolean, ChargeValue: number) =>
        await requests.post(`/${region}/purchaseorder/addinvoicecharge`, {
            OrderId,
            SacType,
            Description,
            SubteamGlAccount,
            Allowance,
            ChargeValue
        }),
    removeInvoiceCharge: async (region: string, chargeId: number) => await requests.post(`/${region}/purchaseorder/removeinvoicecharge`, {ChargeId: chargeId})
};

export default { PurchaseOrder, PurchaseOrderClose };
