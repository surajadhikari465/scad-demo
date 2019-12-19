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

export default { PurchaseOrder };
