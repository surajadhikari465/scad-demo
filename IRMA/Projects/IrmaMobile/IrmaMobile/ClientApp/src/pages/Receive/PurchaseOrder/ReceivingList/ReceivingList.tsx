import React, { Fragment, useState, useEffect, useContext, useCallback } from 'react'
import { AppContext, IMenuItem, types } from "../../../../store";
import ReceivingListLineItem from './components/ReceivingListLineItem'
import { RouteComponentProps, useHistory } from 'react-router-dom';
import agent from '../../../../api/agent';
import orderUtil from '../util/Order';
import OrderItem from '../types/OrderItem';
import LoadingComponent from '../../../../layout/LoadingComponent';
import { Tab, Grid } from 'semantic-ui-react';
import { toast } from 'react-toastify';
import BasicModal from '../../../../layout/BasicModal';

interface RouteParams {
    purchaseOrderNumber: string;
}

interface IProps extends RouteComponentProps<RouteParams> { }

const ReceivingList: React.FC<IProps> = ({ match }) => {
    //@ts-ignore
    const { state, dispatch } = useContext(AppContext);
    const { region, orderDetails } = state;
    const { purchaseOrderNumber } = match.params;
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [isLoadingExceptions, setIsLoadingExceptions] = useState<boolean>(false);
    const [ordered, setOrdered] = useState<number>(0);
    const [received, setReceived] = useState<number>(0);
    const [eInvoiced, setEInvoiced] = useState<number>(0);
    const [eInvoiceExceptionList, setEInvoiceExceptionList] = useState<OrderItem[]>();
    const [enablePartialShipment, setEnablePartialShipment] = useState<boolean>(false);
    const [alertReceiveOrderDiscrepancy, setAlertReceiveOrderDiscrepancy] = useState<any>({
        open: false,
        alertMessage: 'Quantity discrepancy between PO and eInvoice.  Please assign a reason code to all mismatched items.',
        type: 'default',
        header: 'Error'
    });
    let history = useHistory();

    useEffect(() => {
        const handleInvoiceClicked = () => {
            //if order is e-Invoiced then check that all mismatched items have a code
            if (orderDetails && orderDetails.EInvId > 0 && getMismatchedInvoiceItems().some(oi => oi.Code === 0)) {
                setAlertReceiveOrderDiscrepancy({ ...alertReceiveOrderDiscrepancy, open: true });
            } else {
                history.push(`/receive/invoicedata/${purchaseOrderNumber}`);
            }
        }

        const newMenuItems = [
            { id: 0, order: 0, text: "Invoice Data", path: '#', disabled: false, onClick: handleInvoiceClicked } as IMenuItem,
            { id: 1, order: 1, text: "Partial Shipment", path: `/receive/List/${purchaseOrderNumber}/closePartial`, disabled: !enablePartialShipment } as IMenuItem,
            { id: 2, order: 2, text: "Receive Order", path: "/receive/purchaseorder", disabled: false } as IMenuItem,
        ] as IMenuItem[];

        dispatch({ type: types.SETMENUITEMS, menuItems: newMenuItems });
    }, [enablePartialShipment, orderDetails])

    useEffect(() => {
        dispatch({ type: types.SETTITLE, Title: 'Receiving List' });

        return () => {
            dispatch({ type: types.SETTITLE, Title: 'IRMA Mobile' });
        };
    }, []);

    useEffect(() => {
        setIsLoading(true);

        const loadEInvoiceExceptions = async () => {
            try {
                setIsLoadingExceptions(true);
                var orderItems = await agent.ReceivingList.getReceivingListEinvoiceExceptions(region, parseInt(purchaseOrderNumber));
                setEInvoiceExceptionList(orderItems && orderItems.map((oi: any) => orderUtil.MapOrderItem(oi)));
            } catch (error) {
                console.error(error);
                toast.error('Error loading e-Invoice exceptions.');
            } finally {
                setIsLoadingExceptions(false);
            }
        }

        loadEInvoiceExceptions();

        const loadOrderValues = async () => {
            try {
                if (!orderDetails) {
                    return;
                }

                let ordered = 0;
                let received = 0;
                let eInvoiced = 0;
                for (let index = 0; index < orderDetails.OrderItems.length; index++) {
                    const oi = orderDetails.OrderItems[index];
                    ordered += oi.QtyOrdered;
                    received += oi.QtyReceived;
                    eInvoiced += oi.eInvoiceQty;
                }

                setOrdered(ordered);
                setReceived(received);
                setEInvoiced(eInvoiced);
                setEnablePartialShipment(received > 0);
            }
            finally {
                setIsLoading(false);
            }
        };

        loadOrderValues();
    }, []);

    const getMismatchedInvoiceItems = useCallback((): OrderItem[] => {
        if (orderDetails) {
            if (orderDetails.EInvId > 0) {
                return orderDetails.OrderItems.filter((oi: OrderItem) => oi.QtyReceived !== oi.eInvoiceQty && (oi.QtyReceived + oi.eInvoiceQty) > 0);
            } else {
                return orderDetails.OrderItems.filter((oi: OrderItem) => oi.QtyReceived !== oi.QtyOrdered);
            }
        } else {
            return [];
        }
    }, [orderDetails]);

    const panes = [
        {
            menuItem: 'Inv. Mismatch',
            render: () => <Tab.Pane attached='top' style={{ padding: '0px', height: '480px', overflow: 'auto' }}>{getMismatchedInvoiceItems().map((oi: OrderItem) => (<ReceivingListLineItem key={oi.OrderItemId} orderItem={oi} displayReasonCodeDropdown={true} />))}</Tab.Pane>
        },
        {
            menuItem: 'eInv. Exception',
            render: () => <Tab.Pane loading={isLoadingExceptions} attached='top' style={{ padding: '0px', height: '480px', overflow: 'auto' }}>{eInvoiceExceptionList && eInvoiceExceptionList.map((oi: OrderItem) => (<ReceivingListLineItem key={oi.OrderItemId} orderItem={oi} displayReasonCodeDropdown={false} />))}</Tab.Pane>
        },
        {
            menuItem: 'Not Recvd.',
            render: () => <Tab.Pane attached='top' style={{ padding: '0px', height: '480px', overflow: 'auto' }}>{orderDetails && orderDetails.OrderItems.filter((oi: OrderItem) => oi.QtyReceived === 0).map((oi: OrderItem) => (<ReceivingListLineItem key={oi.OrderItemId} orderItem={oi} displayReasonCodeDropdown={false} />))}</Tab.Pane>
        }
    ]

    return (
        <Fragment>
            {isLoading ? <LoadingComponent content='Loading order...' /> : (
                <Fragment>
                    <Grid style={{ fontSize: '16px', fontWeight: 'bold' }}>
                        <Grid.Row style={{ paddingBottom: '0px' }}>
                            <Grid.Column style={{ color: 'blue' }} width={3}>PO#</Grid.Column>
                            <Grid.Column width={13}>{purchaseOrderNumber}</Grid.Column>
                        </Grid.Row>
                        <Grid.Row columns={6} style={{ paddingTop: '0px' }}>
                            <Grid.Column style={{ color: 'green' }} width={3}>Ordered:</Grid.Column>
                            <Grid.Column width={2}>{ordered}</Grid.Column>
                            <Grid.Column style={{ color: 'green' }} width={3}>Rcvd:</Grid.Column>
                            <Grid.Column width={2}>{received}</Grid.Column>
                            <Grid.Column style={{ color: 'green' }} width={3}>eInvd:</Grid.Column>
                            <Grid.Column width={3}>{eInvoiced}</Grid.Column>
                        </Grid.Row>
                    </Grid>
                    <Tab menu={{ attached: 'bottom' }} panes={panes} style={{ minHeight: '100vh' }} />
                    <BasicModal alert={alertReceiveOrderDiscrepancy} setAlert={setAlertReceiveOrderDiscrepancy}></BasicModal>
                </Fragment>)
            }
        </Fragment>
    )
}

export default ReceivingList;