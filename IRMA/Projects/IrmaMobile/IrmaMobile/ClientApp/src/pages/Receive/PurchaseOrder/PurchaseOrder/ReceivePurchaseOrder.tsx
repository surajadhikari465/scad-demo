import React, { Fragment, useContext, useEffect, useCallback, useState } from "react";
import { toast } from "react-toastify";
import agent from "../../../../api/agent";
import CurrentLocation from "../../../../layout/CurrentLocation";
import { AppContext, types, IMenuItem } from "../../../../store";
import ReceivePurchaseOrderDetails from "./components/ReceivePurchaseOrderDetails";
import ReceivePurchaseOrderList from "./components/ReceivePurchaseOrderList";
import ReceivePoSearch from "./components/ReceivePurchaseOrderSearch";
import { OrderDetails } from "../types/OrderDetails";
import { RouteComponentProps, useHistory } from "react-router-dom";
import OrderInformationModal from "../OrderInformation/OrderInformationModal";
import OrderInformation from "../types/OrderInformation";

interface RouteParams {
    openOrderInformation: string;
}

interface IProps extends RouteComponentProps<RouteParams> {}

const ReceivePurchaseOrder: React.FC<IProps> = ({ match }) => {
    // @ts-ignore
    const { state, dispatch } = useContext(AppContext);
    const { storeNumber, listedOrders, region, purchaseOrderUpc, purchaseOrderNumber } = state;
    let selectedPo = null;
    const { openOrderInformation } = match.params;
    const [orderInformation, setOrderInformation] = useState<OrderInformation>({} as OrderInformation);
    let history = useHistory();

    const setMenuItems = useCallback(() => {
        const newMenuItems = [
            { id: 1, order: 0, text: "Refused Items", path: "#", disabled: true } as IMenuItem,
            { id: 2, order: 1, text: "Invoice Data", path: `/receive/invoicedata/${purchaseOrderNumber}`, disabled: purchaseOrderNumber === '' } as IMenuItem,
            { id: 3, order: 2, text: "Receiving List", path: "#", disabled: false } as IMenuItem,
            { id: 4, order: 3, text: "Order Info", path: `/receive/purchaseorder/open`, disabled: purchaseOrderNumber === '' } as IMenuItem,
            { id: 5, order: 4, text: "Clear Screen", path: "#", disabled: false } as IMenuItem,
            { id: 5, order: 4, text: "Review", path: "#", disabled: false } as IMenuItem,
            { id: 6, order: 5, text: "Exit Receive", path: "/functions", disabled: false } as IMenuItem,
         ] as IMenuItem[];

        dispatch({ type: types.SETMENUITEMS, menuItems: newMenuItems });
    }, [purchaseOrderNumber, dispatch])

    useEffect(() => {
        setMenuItems()

        return () => {
            dispatch({ type: types.SETMENUITEMS, menuItems: [] });
        }
    }, [setMenuItems, dispatch]);

    const handleSubmit = async (purchaseOrderNum: number, upc: string, closeOrderList: boolean = false) => {
        //int32 max...
        if(purchaseOrderNum > 2147483647) {
            toast.error(`The PO # value is too large. Please enter a smaller value`);
            return;
        }
        
        try {
            dispatch({ type: types.SETISLOADING, isLoading: true });
            dispatch({ type: types.SETPURCHASEORDERUPC, purchaseOrderUpc: upc });
            dispatch({ type: types.SETPURCHASEORDERNUMBER, purchaseOrderNumber: purchaseOrderNum });
            dispatch({ type: types.SETORDERDETAILS, orderDetails: null });
            setMenuItems();

            if (closeOrderList) {
                dispatch({ type: types.SETLISTEDORDERS, listedOrders: [] });
            }

            if (purchaseOrderNum) {
                var order = await agent.PurchaseOrder.detailsFromPurchaseOrderNumber(region, purchaseOrderNum);

                if(!order) {
                    toast.error("PO could not be loaded. Try again.");
                    return;
                }

                if(order.orderHeader_ID === 0) {
                    toast.error(`PO #${purchaseOrderNum} not found`);
                    return;
                }

                var orderItemsFiltered = order.orderItems.filter(
                    (oi: any) => oi.identifier === upc
                );

                var orderItem = null;
                if(orderItemsFiltered.length === 1) {
                    orderItem = orderItemsFiltered[0];
                } else {
                    if (upc) {
                        toast.error(`${upc} not found in PO #${purchaseOrderNum}`);
                        return;
                    }

                    orderItem = {} as any;
                }

                try {
                    var orderDetails: OrderDetails = {
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
                        ItemLoaded: orderItem.item_Key
                    }

                    var orderInformation: OrderInformation = {
                        buyer: order.createdByName,
                        isCreditOrder: order.return_Order,
                        orderDate: order.orderDate,
                        orderNotes: order.notes
                    }

                    setOrderInformation(orderInformation);

                    dispatch({ type: types.SETORDERDETAILS, orderDetails: orderDetails });
                } catch(err) {
                    toast.error("Unable to open PO")
                }
                        
            } else if (upc) {
                var ordersRaw = await agent.PurchaseOrder.detailsFromUpc(
                    region,
                    upc,
                    storeNumber
                );

                //Map ordersRaw -> orders
                const orders = ordersRaw.map((order: any) => (
                    {
                        PoNum: order.orderHeader_ID,
                        OrderCost: order.orderedCost,
                        ExpectedDate: order.expected_Date,
                        Subteam: order.subTeam_Name,
                        EInv: order.einvoiceRequired
                    }
                ))

                dispatch({ type: types.SETLISTEDORDERS, listedOrders: orders });
            } else {
                toast.error("UPC and/or PO # required");
            }
        } finally {
            dispatch({ type: types.SETISLOADING, isLoading: false });
        }
    };

    if(listedOrders.length > 0 && !selectedPo) {
        return (<ReceivePurchaseOrderList upc={purchaseOrderUpc} orders={listedOrders} poSelected={handleSubmit}/>)
    }

    const handleOnCloseOrderInformation = () => {
        history.goBack();
    }

    return (
        <Fragment>
            <OrderInformationModal handleOnClose={handleOnCloseOrderInformation} orderInformation={orderInformation} open={openOrderInformation === 'open'} />
            <CurrentLocation />
            <div style={{marginTop: '10px', padding: '0px'}}>
                <ReceivePoSearch handleSubmit={handleSubmit}/>
            </div>
            <ReceivePurchaseOrderDetails/>
        </Fragment>
    );
};

export default ReceivePurchaseOrder;
