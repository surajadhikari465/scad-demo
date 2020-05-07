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
    Code: number;
}

export default OrderItem;