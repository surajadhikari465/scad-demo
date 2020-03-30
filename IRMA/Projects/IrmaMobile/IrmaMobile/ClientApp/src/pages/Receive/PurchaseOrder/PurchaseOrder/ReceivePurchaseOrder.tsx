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
import { BarcodeScanner, IBarcodeScannedEvent } from '@wfm/mobile';
import { stat } from "fs";
import BasicModal from "../../../../layout/BasicModal";
import ScanCodeProcessor from "../../../../scanning/ScanCodeProcessor";


interface RouteParams {
    openOrderInformation: string;
}

interface IProps extends RouteComponentProps<RouteParams> { }

const ReceivePurchaseOrder: React.FC<IProps> = ({ match }) => {
    // @ts-ignore
    const { state, dispatch } = useContext(AppContext);
    const { storeNumber, subteamNo, user, listedOrders, region, purchaseOrderUpc, purchaseOrderNumber, orderDetails } = state;
    let selectedPo = null;
    const { openOrderInformation } = match.params;
    const [orderInformation, setOrderInformation] = useState<OrderInformation>({} as OrderInformation);
    let history = useHistory();
    const [openPartial, setOpenPartial] = useState<boolean>(false);
    const [costedByWeight, setCostedByWeight] = useState<boolean>(false);
    const [isReopening, setIsReopening] = useState<boolean>(false);

    const setMenuItems = useCallback(() => {
        const handleExitReceiveClick = () => {
            dispatch({ type: types.SETPURCHASEORDERUPC, purchaseOrderUpc: '' });
            dispatch({ type: types.SETORDERDETAILS, orderDetails: null });
            dispatch({ type: types.SETPURCHASEORDERNUMBER, purchaseOrderNumber: '' });
            dispatch({ type: types.SETLISTEDORDERS, listedOrders: [] });
        };

        const handleNavigateAway = () => {
            dispatch({ type: types.SETPURCHASEORDERUPC, purchaseOrderUpc: '' });
        };

        const newMenuItems = [
            { id: 1, order: 0, text: "Refused Items", path: "#", disabled: true } as IMenuItem,
            { id: 2, order: 1, text: "Invoice Data", path: `/receive/invoicedata/${purchaseOrderNumber}`, disabled: orderDetails === null, onClick: handleNavigateAway } as IMenuItem,
            { id: 3, order: 2, text: "Receiving List", path: `/receive/List/${purchaseOrderNumber}`, disabled: orderDetails === null } as IMenuItem,
            { id: 4, order: 3, text: "Order Info", path: `/receive/purchaseorder/open`, disabled: orderDetails === null } as IMenuItem,
            { id: 5, order: 4, text: "Clear Screen", path: "/receive/PurchaseOrder/clearscreen", disabled: false } as IMenuItem,
            { id: 7, order: 6, text: "Exit Receive", path: "/functions", disabled: false, onClick: handleExitReceiveClick } as IMenuItem,
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

    useEffect(() => {
        BarcodeScanner.registerHandler((data: IBarcodeScannedEvent) => {
            let scanCode = '';
            try {
                scanCode = ScanCodeProcessor.parseScanCode(data.Data, data.Symbology);
            } catch(error) {
                toast.error(error);
            }
            dispatch({ type: types.SETPURCHASEORDERUPC, purchaseOrderUpc: scanCode });
            loadPurchaseOrder(scanCode,
                isNaN(parseInt(state.purchaseOrderNumber, 10)) ? '' : parseInt(state.purchaseOrderNumber, 10).toString(),
                true);
        });
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [state]);

    const loadPurchaseOrder = async (upc: string, purchaseOrderNumber: string, closeOrderList: boolean = false) => {
        //int32 max...
        if (purchaseOrderNumber && parseInt(purchaseOrderNumber) > 2147483647) {
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

            if (purchaseOrderNumber && purchaseOrderNumber !== '') {
                var order = await agent.PurchaseOrder.detailsFromPurchaseOrderNumber(region, parseInt(purchaseOrderNumber));

                if (!order) {
                    toast.error("PO could not be loaded. Please try again.", { autoClose: false });
                    return;
                }

                if (parseInt(order.store_No) !== parseInt(storeNumber)) {
                    toast.error(`PO ${purchaseOrderNumber} is for ${order.storeCompanyName}. Please try again.`, { autoClose: false });
                    return;
                }

                if (order.orderHeader_ID === 0) {
                    toast.error(`PO #${purchaseOrderNumber} not found`, { autoClose: false });
                    return;
                }

                if (!order.sentDate || isMinDate(order.sentDate)) {
                    toast.error(`PO #${purchaseOrderNumber} has not yet been sent. Please try again.`, { autoClose: false });
                    return;
                }

                if (upc && upc !== '' && !orderUtil.OrderHasUpc(order, upc)) {
                    toast.error(`${upc} not found in PO #${purchaseOrderNumber}`, { autoClose: false });
                    return;
                }

                try {
                    var orderDetails = orderUtil.MapOrder(order, upc);

                    if (!isMinDate(orderDetails.CloseDate)) {
                        if (!orderDetails.PartialShipment) {
                            toast.error(`PO ${purchaseOrderNumber} is already closed. To review or reopen a closed order, please use the IRMA client.`, { autoClose: false });
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

                    if (orderDetails.ItemLoaded) {
                        let storeItem = await agent.StoreItem.getStoreItem(
                            region,
                            storeNumber,
                            subteamNo,
                            user!.userId,
                            upc
                        );

                        setCostedByWeight(storeItem.costedByWeight)
                    }

                    dispatch({ type: types.SETORDERDETAILS, orderDetails: orderDetails });
                } catch (err) {
                    toast.error("Unable to open PO", { autoClose: false })
                }

            } else if (upc && upc !== '') {
                var ordersRaw = await agent.PurchaseOrder.detailsFromUpc(
                    region,
                    upc,
                    storeNumber
                );

                if (!ordersRaw || ordersRaw.length === 0) {
                    toast.error('No open orders found.');
                } else {
                    const orders = ordersRaw.map((order: any) => (
                        {
                            PoNum: order.orderHeader_ID.toString(),
                            OrderCost: order.orderedCost,
                            ExpectedDate: order.expected_Date,
                            Subteam: order.subTeam_Name,
                            EInv: order.einvoiceRequired
                        }
                    ));

                    dispatch({ type: types.SETLISTEDORDERS, listedOrders: orders });
                }
            } else {
                toast.error("UPC and/or PO # required", { autoClose: false });
            }
        } finally {
            dispatch({ type: types.SETISLOADING, isLoading: false });
        }
    };

    const handleOnCloseOrderInformation = () => {
        history.goBack();
    }

    const handleReopenPartial = async () => {
        try {
            setIsReopening(true);
            const result = await agent.PurchaseOrder.reopenOrder(region, parseInt(purchaseOrderNumber));

            if (result && result.status) {
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

    if (listedOrders.length > 0 && !selectedPo) {
        return (<ReceivePurchaseOrderList upc={purchaseOrderUpc} orders={listedOrders} poSelected={loadPurchaseOrder} />)
    }
    else {
        return (
            <Fragment>
                {isReopening ? <LoadingComponent content="Reopening order..." /> :
                    <Fragment>
                        <ConfirmModal handleCancelClose={handleReopenCancel} handleConfirmClose={handleReopenPartial} setOpenExternal={setOpenPartial} showTriggerButton={false}
                            openExternal={openPartial} headerText='Receive' cancelButtonText='No' confirmButtonText='Yes'
                            lineOne={'This order was closed as a partial shipment. Re-open the order now to scan more items?'} />
                        <OrderInformationModal handleOnClose={handleOnCloseOrderInformation} orderInformation={orderInformation} open={openOrderInformation === 'open'} />
                        <CurrentLocation />
                        <div style={{ marginTop: '10px', padding: '0px' }}>
                            <ReceivePoSearch handleSubmit={loadPurchaseOrder} />
                        </div>
                        <ReceivePurchaseOrderDetails costedByWeight={costedByWeight} />
                    </Fragment>
                }
            </Fragment>
        );
    }
};

export default ReceivePurchaseOrder;
