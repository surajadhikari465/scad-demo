import OrderItem from "./OrderItem";

export interface OrderDetails {
    OrderId: number
    Description: string;
    Quantity: number;
    Weight: number;
    Uom: string;
    Code: number;
    QtyOrdered: number;
    QtyReceived: number;
    EInvQty: number;
    IsReturnOrder: boolean;
    Subteam: string;
    SubteamNo: number;
    Vendor: string;
    OrderItemId: number;
    PkgWeight: number;
    PkgQuantity: number;
    OrderUom: string;
    InvoiceCost: number;
    InvoiceFreight: number;
    InvoiceNumber: string;
    InvoiceDate: Date;
    VendorDocDate: Date;
    VendorDocId: number;
    AdjustedReceivedCost: number;
    OrderedCost: number;
    OrderTypeId: number;
    EInvId: number;
    EInvRequired: boolean;
    ItemLoaded: boolean;
    CurrencyId: number;
    CloseDate: Date;
    PartialShipment: boolean;
    OrderItems: OrderItem[];
}