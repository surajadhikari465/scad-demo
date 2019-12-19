import { OrderDetails } from "./OrderDetails";

export interface Order {
    SubTeam: string;
    VendorName: string;
    StoreNumber: number;
    OrderItems: OrderDetails[];
}