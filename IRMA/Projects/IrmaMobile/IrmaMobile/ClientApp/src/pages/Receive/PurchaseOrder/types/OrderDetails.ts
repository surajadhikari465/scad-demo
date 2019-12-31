export interface OrderDetails {
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
    Vendor: string;
    OrderItemId: number;
    PkgWeight: number;
    PkgQuantity: number;
    OrderUom: string;
    InvoiceCost: number;
    InvoiceFreight: number;
    InvoiceNumber: string;
    AdjustedReceivedCost: number;
    OrderedCost: number;
    OrderTypeId: number;
    EInvId: number,
    EInvRequired: boolean;
    ItemLoaded: boolean;
}