import { OrderDetails } from "../types/OrderDetails";
import OrderItem from "../types/OrderItem";

const orderUtil = {
    OrderHasUpc: (order: any, upc: string): boolean => {
        var orderItemsFiltered = order.orderItems.filter(
            (oi: any) => oi.identifier === upc
        );
    
        const result = orderItemsFiltered.length === 1;
    
        return result;
    },
    MapOrder: (order: any, upc: string = ""): OrderDetails => {
        var orderItemsFiltered = order.orderItems.filter(
            (oi: any) => oi.identifier === upc
        );
    
        var orderItem = null;
        if(orderItemsFiltered.length === 1) {
            orderItem = orderItemsFiltered[0];
        } else {
            orderItem = {} as any;
        }
    
        var orderDetails: OrderDetails = {
            OrderId: order.orderHeader_ID,
            Description: orderItem.item_Description,
            Quantity: 0,
            Weight: orderItem.total_Weight, 
            Uom: orderItem.packageUnitAbbr,
            Code: orderItem.receivingDiscrepancyReasonCodeID,
            QtyOrdered: orderItem.quantityOrdered,
            QtyReceived: orderItem.quantityReceived,
            EInvQty: orderItem.eInvoiceQuantity,
            IsReturnOrder: order.return_Order,
            Subteam: order.transfer_To_SubTeamName,
            SubteamNo: order.transfer_To_SubTeam,
            Vendor: order.companyName,
            OrderItemId: orderItem.orderItem_ID,
            PkgWeight: orderItem.package_Desc1,
            PkgQuantity: orderItem.package_Desc2,
            OrderUom: orderItem.orderUOMAbbr,
            InvoiceCost: order.invoiceCost,
            InvoiceFreight: order.invoiceFreight,
            InvoiceNumber: order.invoiceNumber,
            AdjustedReceivedCost: order.adjustedReceivedCost,
            OrderedCost: order.orderedCost,
            OrderTypeId: order.orderType_Id,
            EInvId: order.einvoiceID,
            EInvRequired: order.einvoiceRequired,
            ItemLoaded: orderItem.item_Key,
            CurrencyId: order.currencyID,
            InvoiceDate: order.invoiceDate,
            VendorDocDate: order.vendorDocDate,
            VendorDocId: order.vendorDocID,
            CloseDate: order.closeDate,
            PartialShipment: order.partialShipment,
            OrderItems: order.orderItems.map((oi: any) => {
                return orderUtil.MapOrderItem(oi);
            })
        }
    
        return orderDetails;
    },
    MapOrderItem: (oi: any): OrderItem => {
        return {
            OrderItemId: oi.orderItem_ID,
            QtyOrdered: oi.quantityOrdered,
            QtyReceived: oi.quantityReceived,
            eInvoiceQty: oi.eInvoiceQuantity,
            Upc: oi.identifier,
            Description: oi.item_Description,
            Weight: oi.total_Weight,
            eInvoiceWeight: oi.eInvoiceWeight,
            Code: oi.receivingDiscrepancyReasonCodeID
        } as OrderItem
    }
}

export default orderUtil;