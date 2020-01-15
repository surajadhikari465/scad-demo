interface OrderItem 
{
    OrderItemId: number;
    QtyOrdered: number;
    QtyReceived: number;
    eInvoiceQty: number;
    Upc: number;
    Description: string;
    Weight: number;
    eInvoiceWeight: number;
    Code: string;
}

export default OrderItem;