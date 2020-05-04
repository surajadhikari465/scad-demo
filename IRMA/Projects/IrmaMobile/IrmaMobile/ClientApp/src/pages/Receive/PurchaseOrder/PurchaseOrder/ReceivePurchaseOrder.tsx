import React, { Fragment, useContext, useEffect, useCallback, useState, ChangeEvent } from "react";
import { toast } from "react-toastify";
import agent from "../../../../api/agent";
import CurrentLocation from "../../../../layout/CurrentLocation";
import { AppContext, types, IMenuItem, IExternalOrder } from "../../../../store";
import ReceivePurchaseOrderDetails from "./components/ReceivePurchaseOrderDetails";
import SelectOrderByScanCode from "./components/SelectOrderByScanCode";
import ConfirmModal from "../../../../layout/ConfirmModal";
import LoadingComponent from "../../../../layout/LoadingComponent";
import { RouteComponentProps, useHistory } from "react-router-dom";
import OrderInformationModal from "../OrderInformation/OrderInformationModal";
import OrderInformation from "../types/OrderInformation";
import orderUtil from "../util/Order"
import isMinDate from "../util/MinDate";
// @ts-ignore 
import { BarcodeScanner, IBarcodeScannedEvent, transformScanCode } from '@wfm/mobile';
import { Form, Grid, Input, Button } from "semantic-ui-react";
import SelectExternalOrder from "./components/SelectExternalOrder";
import { OrderByScanCode } from "../types/OrderByScanCode";
import { IValidateOrderResult } from "../types/IValidateOrderResult";

interface RouteParams {
    openOrderInformation: string;
}

interface IProps extends RouteComponentProps<RouteParams> { }

const ReceivePurchaseOrder: React.FC<IProps> = ({ match }) => {
    // @ts-ignore
    const { state, dispatch } = useContext(AppContext);
    const { storeNumber, user, region, purchaseOrderUpc, purchaseOrderNumber, orderDetails } = state;
    const { openOrderInformation } = match.params;
    const [orderInformation, setOrderInformation] = useState<OrderInformation>({} as OrderInformation);
    let history = useHistory();
    const [openPartial, setOpenPartial] = useState<boolean>(false);
    const [costedByWeight, setCostedByWeight] = useState<boolean>(false);
    const [isReopening, setIsReopening] = useState<boolean>(false);
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [displaySelectExternalOrder, setDisplaySelectExternalOrder] = useState<boolean>(false);
    const [displaySelectOrderByScanCode, setDisplaySelectOrderByScanCode] = useState<boolean>(false);
    const [externalOrders, setExternalOrders] = useState<IExternalOrder[]>();
    const [ordersByScanCode, setOrdersByScanCode] = useState<OrderByScanCode[]>();

    const setMenuItems = useCallback(() => {
        const handleExitReceiveClick = () => {
            dispatch({ type: types.SETPURCHASEORDERUPC, purchaseOrderUpc: '' });
            dispatch({ type: types.SETPURCHASEORDERNUMBER, purchaseOrderNumber: '' });
            dispatch({ type: types.SETORDERDETAILS, orderDetails: null });
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
        BarcodeScanner.registerHandler((data: IBarcodeScannedEvent) => {
            let scanCode = '';
            try {
                scanCode = transformScanCode({ scanCode: data.Data, symbology: data.Symbology });
            } catch (error) {
                toast.error(error);
                return;
            }

            dispatch({ type: types.SETPURCHASEORDERUPC, purchaseOrderUpc: scanCode });
            search();
        });

        dispatch({ type: types.SETTITLE, Title: 'Receive' });

        return () => {
            dispatch({ type: types.SETTITLE, Title: 'IRMA Mobile' });
        };
    }, [dispatch]);

    useEffect(() => {
        setMenuItems()

        return () => {
            dispatch({ type: types.SETMENUITEMS, menuItems: [] });
        }
    }, [setMenuItems, dispatch]);
    
    const searchForExternalOrders = async (purchaseOrderNumber: string) => {
        //int32 max...
        if (purchaseOrderNumber && parseInt(purchaseOrderNumber) > 2147483647) {
            toast.error(`The PO # value is too large. Please enter a smaller value`, { autoClose: false });
        } else {
            setIsLoading(true);
            dispatch({ type: types.SETORDERDETAILS, orderDetails: null });
            setMenuItems();

            try {
                let externalOrders = await agent.PurchaseOrder.getExternalOrders(region, parseInt(purchaseOrderNumber), storeNumber);

                if (externalOrders.length === 1) {
                    searchForOrder(externalOrders[0].orderHeaderId);
                } else if (externalOrders.length > 1) {
                    setExternalOrders(externalOrders);
                    setDisplaySelectExternalOrder(true);
                } else {
                    searchForOrder(parseInt(purchaseOrderNumber));
                }
            } catch (error) {
                console.error(error);
                toast.error(`Error occurred when searching for order an order. Please retry your request.  If the problem persists, please contact support.`, { autoClose: false });
                dispatch({ type: types.SETORDERDETAILS, orderDetails: null });
                setIsLoading(false);
            } 
        }
    }

    const searchForOrder = async (purchaseOrderNumber: number) => {
        setIsLoading(true);
        try {
            const order = await agent.PurchaseOrder.getOrder(region, purchaseOrderNumber);
            const validationResult = validateOrder(order);
            if (validationResult.isValid) {
                if (purchaseOrderUpc && purchaseOrderUpc.length > 0) {
                    if (order.orderItems.some((oi: { identifier: string }) => oi.identifier === purchaseOrderUpc)) {
                        searchForOrderItem(purchaseOrderNumber, purchaseOrderUpc);
                    } else {
                        toast.error(`${purchaseOrderUpc} not found in PO #${purchaseOrderNumber}`, { autoClose: false });
                    }
                }

                var orderDetails = orderUtil.MapOrder(order, purchaseOrderUpc);
                var orderInformation: OrderInformation = {
                    buyer: order.createdByName,
                    isCreditOrder: order.return_Order,
                    orderDate: order.orderDate,
                    orderNotes: order.notes
                }
                setOrderInformation(orderInformation);
                dispatch({ type: types.SETORDERDETAILS, orderDetails: orderDetails });

                if(!isMinDate(order.closeDate) && order.partialShipment) {
                    setOpenPartial(true);
                }
            } else {
                toast.error(validationResult.errorMessage);
                dispatch({ type: types.SETPURCHASEORDERUPC, purchaseOrderUpc: '' });
                dispatch({ type: types.SETPURCHASEORDERNUMBER, purchaseOrderNumber: '' });
                dispatch({ type: types.SETORDERDETAILS, orderDetails: null });
            }
        } catch (error) {
            console.error(`Error retrieving order ${purchaseOrderNumber}: ${error}`);
            toast.error(`Error occurred when searching for order an order. Please retry your request.  If the problem persists, please contact support.`, { autoClose: false });
            dispatch({ type: types.SETPURCHASEORDERUPC, purchaseOrderUpc: '' });
            dispatch({ type: types.SETPURCHASEORDERNUMBER, purchaseOrderNumber: '' });
            dispatch({ type: types.SETORDERDETAILS, orderDetails: null });
        } finally {
            setIsLoading(false);
        }
    };

    const validateOrder = (order: { orderHeader_ID: number, store_No: number, storeCompanyName: string, sentDate: Date, closeDate: Date, partialShipment: boolean }): IValidateOrderResult => {
        if (!order) {
            return { isValid: false, errorMessage: "PO could not be loaded. Please try again." };
        } else if (order.orderHeader_ID === 0) {
            return { isValid: false, errorMessage: `PO #${purchaseOrderNumber} not found.` };
        } else if (order.store_No !== parseInt(storeNumber)) {
            return { isValid: false, errorMessage: `PO ${order.orderHeader_ID} is for ${order.storeCompanyName}. Please try again.` };
        } else if (!order.sentDate || isMinDate(order.sentDate)) {
            return { isValid: false, errorMessage: `PO #${order.orderHeader_ID} has not yet been sent. Please try again.` };
        } else if (purchaseOrderUpc && purchaseOrderUpc.length > 0 && !orderUtil.OrderHasUpc(order, purchaseOrderUpc)) {
            return { isValid: false, errorMessage: `${purchaseOrderUpc} not found in PO #${order.orderHeader_ID}.` };
        } else if (!isMinDate(order.closeDate) && !order.partialShipment) {
            return { isValid: false, errorMessage: `PO ${order.orderHeader_ID} is already closed. To review or reopen a closed order, please use the IRMA client.` }
        } else {
            return { isValid: true };
        }
    };

    const searchForOrderItem = async (purchaseOrderNumber: number, scanCode: string) => {
        let storeItem = await agent.StoreItem.getStoreItem(
            region,
            storeNumber,
            0,
            user!.userId,
            scanCode
        );

        setCostedByWeight(storeItem.costedByWeight)
    }

    const searchForOrderByScanCode = async (scanCode: string) => {
        setIsLoading(true);
        try {
            let orders = await agent.PurchaseOrder.detailsFromUpc(region, scanCode, storeNumber);
            if (!orders || orders.length <= 0) {
                toast.error('No open orders found.');
            } else {
                setOrdersByScanCode(orders.map((order: any) => (
                    {
                        PoNum: order.orderHeader_ID.toString(),
                        OrderCost: order.orderedCost,
                        ExpectedDate: order.expected_Date,
                        Subteam: order.subTeam_Name,
                        EInv: order.einvoiceRequired
                    }
                )));
                setDisplaySelectOrderByScanCode(true);
            }
        } catch (error) {
            console.error(error);
            toast.error('Error occurred while searching for orders by UPC. Please try again.');
        } finally {
            setIsLoading(false);
        }
    }

    const handleOnCloseOrderInformation = () => {
        history.goBack();
    }

    const handleReopenPartial = async () => {
        try {
            setIsReopening(true);
            const result = await agent.PurchaseOrder.reOpenOrder(region, parseInt(purchaseOrderNumber!));

            if (result && result.status) {
                toast.info('The order has been reopened');
            } else {
                toast.error(`Error reopening the order: ${(result && result.errorMessage) || 'No message given'}`);
            }
        } catch (error) {
            toast.error(`Error reopening the order: ${error}`);
        } finally {
            setIsReopening(false);
        }
    }

    const handleReopenCancel = () => {
        dispatch({ type: types.SETPURCHASEORDERUPC, purchaseOrderUpc: '' });
        dispatch({ type: types.SETPURCHASEORDERNUMBER, purchaseOrderNumber: '' });
        dispatch({ type: types.SETORDERDETAILS, orderDetails: null });
    }

    const handleSearchClicked = () => {
        search();
    }

    const handlePurchaseOrderUpcChanged = (e: ChangeEvent<HTMLInputElement>) => {        
        dispatch({ type: types.SETPURCHASEORDERUPC, purchaseOrderUpc: e.target.value });
    }

    const handlePurchaseOrderNumberChanged = (e: ChangeEvent<HTMLInputElement>) => {
        dispatch({ type: types.SETPURCHASEORDERNUMBER, purchaseOrderNumber: e.target.value });
    }

    const handleSelectExternalOrder = (upc: string, orderHeaderId: number) => {
        setDisplaySelectExternalOrder(false);
        dispatch({ type: types.SETPURCHASEORDERNUMBER, purchaseOrderNumber: orderHeaderId.toString() });
        searchForOrder(orderHeaderId);
    };

    const handleSelectOrderByScanCode = (upc: string, orderHeaderId: number) => {
        setDisplaySelectOrderByScanCode(false);
        dispatch({ type: types.SETPURCHASEORDERNUMBER, purchaseOrderNumber: orderHeaderId.toString() });
        searchForOrder(orderHeaderId);
    };

    const search = () => {
        setMenuItems();

        if (purchaseOrderNumber && purchaseOrderNumber.length > 0 && purchaseOrderUpc && purchaseOrderUpc.length > 0) {
            searchForExternalOrders(purchaseOrderNumber);
        } else if (purchaseOrderNumber && purchaseOrderNumber.length > 0) {
            searchForExternalOrders(purchaseOrderNumber);
        } else if (purchaseOrderUpc && purchaseOrderUpc.length > 0) {
            searchForOrderByScanCode(purchaseOrderUpc);
        } else {
            toast.error('Please enter a value for the PO # or the UPC to perform a search.');
        }
    }

    if (displaySelectExternalOrder) {
        return (<SelectExternalOrder upc={purchaseOrderUpc!} orders={externalOrders!} orderSelected={handleSelectExternalOrder} />)
    }
    else if (displaySelectOrderByScanCode) {
        return (<SelectOrderByScanCode upc={purchaseOrderUpc!} orders={ordersByScanCode!} orderSelected={handleSelectOrderByScanCode} />)
    }
    else {
        return (
            <Fragment>
                {isReopening ? <LoadingComponent content="Reopening order..." /> :
                    <Fragment>
                        <ConfirmModal
                            handleCancelClose={handleReopenCancel}
                            handleConfirmClose={handleReopenPartial}
                            setOpenExternal={setOpenPartial}
                            showTriggerButton={false}
                            openExternal={openPartial}
                            headerText='Receive'
                            cancelButtonText='No'
                            confirmButtonText='Yes'
                            lineOne={'This order was closed as a partial shipment. Re-open the order now to scan more items?'} />
                        <OrderInformationModal handleOnClose={handleOnCloseOrderInformation} orderInformation={orderInformation} open={openOrderInformation === 'open'} />
                        <CurrentLocation />
                        <div style={{ marginTop: '10px', padding: '0px' }}>
                            <Form onSubmit={handleSearchClicked}>
                                <Grid centered>
                                    <Grid.Row style={{ paddingBottom: "0px" }}>
                                        <Form.Group inline>
                                            <Grid.Column>
                                                <Form.Field>
                                                    <Input
                                                        name="purchaseOrderNumber"
                                                        type="number"
                                                        placeholder="PO #"
                                                        min={0}
                                                        value={purchaseOrderNumber}
                                                        onChange={handlePurchaseOrderNumberChanged}
                                                    />
                                                </Form.Field>
                                                <Form.Field>
                                                    <Input
                                                        name="purchaseOrderUpc"
                                                        type="number"
                                                        placeholder="UPC"
                                                        min={0}
                                                        value={purchaseOrderUpc}
                                                        onChange={handlePurchaseOrderUpcChanged}
                                                    />
                                                </Form.Field>
                                            </Grid.Column>
                                            <Grid.Column>
                                                <Button
                                                    loading={isLoading}
                                                    type="submit"
                                                    content="Search"
                                                    disabled={(!purchaseOrderUpc || purchaseOrderUpc?.length === 0) && (!purchaseOrderNumber || purchaseOrderNumber?.length === 0)}
                                                />
                                            </Grid.Column>
                                        </Form.Group>
                                    </Grid.Row>
                                </Grid>
                            </Form>
                        </div>
                        <ReceivePurchaseOrderDetails costedByWeight={costedByWeight} />
                    </Fragment>
                }
            </Fragment>
        );
    }
};

export default ReceivePurchaseOrder;
