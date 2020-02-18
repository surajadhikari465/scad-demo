import React, { Fragment, useContext, useEffect, useCallback, useState } from "react";
import { toast } from "react-toastify";
import agent from "../../../../api/agent";
import CurrentLocation from "../../../../layout/CurrentLocation";
import { AppContext, types, IMenuItem } from "../../../../store";
import ReceivePurchaseOrderDetails from "./components/ReceivePurchaseOrderDetails";
import ReceivePurchaseOrderList from "./components/ReceivePurchaseOrderList";
import ReceivePoSearch from "./components/ReceivePurchaseOrderSearch";
import ConfirmModal from "../../../../layout/ConfirmModal";
import LoadingComponent from "../../../../layout/LoadingComponent";
import { RouteComponentProps, useHistory } from "react-router-dom";
import OrderInformationModal from "../OrderInformation/OrderInformationModal";
import OrderInformation from "../types/OrderInformation";
import orderUtil from "../util/Order"
import isMinDate from "../util/MinDate";
// @ts-ignore 
import { BarcodeScanner, IBarcodeScannedEvent} from '@wfm/mobile';


interface RouteParams {
    openOrderInformation: string;
}

interface IProps extends RouteComponentProps<RouteParams> {}

const ReceivePurchaseOrder: React.FC<IProps> = ({ match }) => {
    // @ts-ignore
    const { state, dispatch } = useContext(AppContext);
    const { store, storeNumber, listedOrders, region, purchaseOrderUpc, purchaseOrderNumber, orderDetails } = state;
    let selectedPo = null;
    const { openOrderInformation } = match.params;
    const [orderInformation, setOrderInformation] = useState<OrderInformation>({} as OrderInformation);
    let history = useHistory();
    const [openPartial, setOpenPartial] = useState<boolean>(false);
    const [isReopening, setIsReopening] = useState<boolean>(false);

    const setMenuItems = useCallback(() => {
        const newMenuItems = [
            { id: 1, order: 0, text: "Refused Items", path: "#", disabled: true } as IMenuItem,
            { id: 2, order: 1, text: "Invoice Data", path: `/receive/invoicedata/${purchaseOrderNumber}`, disabled: orderDetails === null } as IMenuItem,
            { id: 3, order: 2, text: "Receiving List", path: `/receive/List/${purchaseOrderNumber}`, disabled: orderDetails === null } as IMenuItem,
            { id: 4, order: 3, text: "Order Info", path: `/receive/purchaseorder/open`, disabled: orderDetails === null } as IMenuItem,
            { id: 5, order: 4, text: "Clear Screen", path: "/receive/PurchaseOrder/clearscreen", disabled: false } as IMenuItem,
            { id: 7, order: 6, text: "Exit Receive", path: "/functions", disabled: false } as IMenuItem,
         ] as IMenuItem[];

        dispatch({ type: types.SETMENUITEMS, menuItems: newMenuItems });
    }, [purchaseOrderNumber, orderDetails, dispatch]);

    useEffect(() => {
        setMenuItems()

        return () => {
            dispatch({ type: types.SETMENUITEMS, menuItems: [] });
        }
    }, [setMenuItems, dispatch]);

    useEffect(() => {
        dispatch({ type: types.SETTITLE, Title: 'Receive' });
        return () => {
          dispatch({ type: types.SETTITLE, Title: 'IRMA Mobile' });
        };
      }, [dispatch]);

    const loadPurchaseOrder = async ( upc: string, purchaseOrderNum?: number | undefined, closeOrderList: boolean = false) => {
        if(purchaseOrderNum === undefined){
            purchaseOrderNum = parseInt(purchaseOrderNumber);
        }
        //int32 max...
        if(purchaseOrderNum > 2147483647) {
            toast.error(`The PO # value is too large. Please enter a smaller value`, { autoClose: false });
            return;
        }
        
        try {
            dispatch({ type: types.SETISLOADING, isLoading: true });
            dispatch({ type: types.SETORDERDETAILS, orderDetails: null });
            setMenuItems();

            if (closeOrderList) {
                dispatch({ type: types.SETLISTEDORDERS, listedOrders: [] });
            }

            if (purchaseOrderNum) {
                var order = await agent.PurchaseOrder.detailsFromPurchaseOrderNumber(region, purchaseOrderNum);

                if(!order) {
                    toast.error("PO could not be loaded. Please try again.", { autoClose: false });
                    return;
                }

                if(order.storeCompanyName !== store){
                    toast.error( `PO ${purchaseOrderNum} is for ${order.storeCompanyName}. Please try again.`, { autoClose: false });
                    return;
                }

                if(order.orderHeader_ID === 0) {
                    toast.error(`PO #${purchaseOrderNum} not found`, { autoClose: false });
                    return;
                }

                if(!order.sentDate || isMinDate(order.sentDate)) {
                    toast.error(`PO #${purchaseOrderNum} has not yet been sent. Please try again.`, { autoClose: false });
                    return;
                }

                if (upc && !orderUtil.OrderHasUpc(order, upc)) {
                    toast.error(`${upc} not found in PO #${purchaseOrderNum}`, { autoClose: false });
                    return;
                }

                try {
                    var orderDetails = orderUtil.MapOrder(order, upc);

                    if(!isMinDate(orderDetails.CloseDate)) {
                        if(!orderDetails.PartialShipment){
                            toast.error(`PO ${purchaseOrderNum} is already closed. To review or reopen a closed order, please use the IRMA client.`, { autoClose: false });
                            dispatch({ type: types.SETPURCHASEORDERUPC, purchaseOrderUpc: '' });
                            dispatch({ type: types.SETPURCHASEORDERNUMBER, purchaseOrderNumber: '' });
                            return;
                        } else {
                            setOpenPartial(true);
                            return;
                        }
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
                    toast.error("Unable to open PO", { autoClose: false })
                }
                        
            } else if (upc) {
                var ordersRaw = await agent.PurchaseOrder.detailsFromUpc(
                    region,
                    upc,
                    storeNumber
                );

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
                toast.error("UPC and/or PO # required", { autoClose: false });
            }
        } finally {
            dispatch({ type: types.SETISLOADING, isLoading: false });
        }
    };

    useEffect(() => {
        BarcodeScanner.registerHandler((data: IBarcodeScannedEvent) => {
            loadPurchaseOrder(data.Data);
          });

        return () => {
            BarcodeScanner.scanHandler = () => {};
        }
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    if(listedOrders.length > 0 && !selectedPo) {
        return (<ReceivePurchaseOrderList upc={purchaseOrderUpc} orders={listedOrders} poSelected={loadPurchaseOrder}/>)
    }

    const handleOnCloseOrderInformation = () => {
        history.goBack();
    }

    const handleReopenPartial = async () => {
        try{
            setIsReopening(true);
            const result = await agent.PurchaseOrder.reopenOrder(region, parseInt(purchaseOrderNumber));

            if(result && result.status) {
                toast.info('The order has been reopened');
            } else {
                toast.error(`Error reopening the order: ${(result && result.errorMessage) || 'No message given'}`);
            }
        } 
        finally {
            setIsReopening(false);
        }
    }

    const handleReopenCancel = () => {
        dispatch({ type: types.SETPURCHASEORDERUPC, purchaseOrderUpc: '' });
        dispatch({ type: types.SETPURCHASEORDERNUMBER, purchaseOrderNumber: '' });
        dispatch({ type: types.SETORDERDETAILS, orderDetails: null });
    }

    return (
        <Fragment>
            {isReopening ? <LoadingComponent content="Reopening order..." /> :
            <Fragment>
                <ConfirmModal handleCancelClose={handleReopenCancel} handleConfirmClose={handleReopenPartial} setOpenExternal={setOpenPartial} showTriggerButton={false} 
                                    openExternal={openPartial} headerText='Receive' cancelButtonText='No' confirmButtonText='Yes' 
                                    lineOne={'This order was closed as a partial shipment. Re-open the order now to scan more items?'} /> 
                <OrderInformationModal handleOnClose={handleOnCloseOrderInformation} orderInformation={orderInformation} open={openOrderInformation === 'open'} />
                <CurrentLocation />
                <div style={{marginTop: '10px', padding: '0px'}}>
                    <ReceivePoSearch handleSubmit={loadPurchaseOrder}/>
                </div>
                <ReceivePurchaseOrderDetails/>
            </Fragment>
            }
        </Fragment>
    );
};

export default ReceivePurchaseOrder;
