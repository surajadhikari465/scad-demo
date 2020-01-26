import React, { Fragment, useState, useEffect, useContext } from 'react'
import { AppContext, IMenuItem, types } from "../../../../store";
import ReceivingListLineItem from './components/ReceivingListLineItem'
import { RouteComponentProps } from 'react-router-dom';
import agent from '../../../../api/agent';
import orderUtil from '../util/Order';
import OrderItem from '../types/OrderItem';
import LoadingComponent from '../../../../layout/LoadingComponent';
import { Tab, Grid } from 'semantic-ui-react';

interface RouteParams {
    purchaseOrderNumber: string;
}

interface IProps extends RouteComponentProps<RouteParams> {}

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
    const [EInvoiceExceptionList, setEInvoiceExceptionList] = useState<OrderItem[]>();
    const [enablePartialShipment, setEnablePartialShipment] = useState<boolean>(false);

    useEffect(() => {
        const loadOrderItems = async () => {
            try {
                setIsLoadingExceptions(true);
                var orderItems = await agent.ReceivingList.getReceivingListEinvoiceExceptions(region, parseInt(purchaseOrderNumber));
                setEInvoiceExceptionList(orderItems && orderItems.map((oi: any) => orderUtil.MapOrderItem(oi)));
            }
            finally
            {
                setIsLoadingExceptions(false);
            }
        }

        loadOrderItems();
    }, [region, setEInvoiceExceptionList, purchaseOrderNumber])
    
    useEffect(() => {
        const newMenuItems = [
            { id: 0, order: 0, text: "Invoice Data", path: `/receive/invoicedata/${purchaseOrderNumber}`, disabled: false } as IMenuItem,
            { id: 1, order: 1, text: "Partial Shipment", path: `/receive/List/${purchaseOrderNumber}/closePartial`, disabled: !enablePartialShipment} as IMenuItem,
            { id: 2, order: 2, text: "Receive Order", path: "/receive/purchaseorder", disabled: false } as IMenuItem,
            ] as IMenuItem[];

        dispatch({ type: types.SETMENUITEMS, menuItems: newMenuItems });
    }, [dispatch, purchaseOrderNumber, enablePartialShipment])

    useEffect(() => {
        const loadOrder = async () => {
            setIsLoading(true);

            try {
                if(!orderDetails) {
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

        loadOrder();
    }, [region, purchaseOrderNumber, orderDetails])

    const panes = [
        {
            menuItem: 'Inv. Mismatch',
            render: () => <Tab.Pane attached='top' style={{padding: '0px', height: '480px', overflow: 'auto'}}>{orderDetails && orderDetails.OrderItems.filter((oi: OrderItem) => (orderDetails.EInvId > 0 && oi.QtyReceived !== oi.eInvoiceQty && (oi.QtyReceived + oi.eInvoiceQty) > 0) || (orderDetails.EInvId === 0 && oi.QtyReceived !== oi.QtyOrdered)).map((oi: OrderItem) => (<ReceivingListLineItem key={oi.OrderItemId} orderItem={oi} />))}</Tab.Pane>
        },
        {
            menuItem: 'eInv. Exception',
            render: () => <Tab.Pane loading={isLoadingExceptions} attached='top' style={{padding: '0px', height: '480px', overflow: 'auto'}}>{EInvoiceExceptionList && EInvoiceExceptionList.map((oi: OrderItem) => (<ReceivingListLineItem orderItem={oi} />))}</Tab.Pane>
        },
        {
            menuItem: 'Not Recvd.',
            render: () => <Tab.Pane attached='top' style={{padding: '0px', height: '480px', overflow: 'auto'}}>{orderDetails && orderDetails.OrderItems.filter((oi: OrderItem) => oi.QtyReceived === 0).map((oi: OrderItem) => (<ReceivingListLineItem orderItem={oi} />))}</Tab.Pane>
        }
    ]

    return (
        <Fragment>
            {isLoading ? <LoadingComponent content='Loading order...' /> : (
            <Fragment>
                <Grid style={{fontSize: '16px', fontWeight: 'bold'}}>
                    <Grid.Row style={{paddingBottom: '0px'}}>
                        <Grid.Column style={{color: 'blue'}} width={3}>PO#</Grid.Column>
                        <Grid.Column width={13}>{purchaseOrderNumber}</Grid.Column>
                    </Grid.Row>
                    <Grid.Row columns={6} style={{paddingTop: '0px'}}>
                        <Grid.Column style={{color: 'green'}} width={3}>Ordered:</Grid.Column>
                        <Grid.Column width={2}>{ordered}</Grid.Column>
                        <Grid.Column style={{color: 'green'}} width={3}>Rcvd:</Grid.Column>
                        <Grid.Column width={2}>{received}</Grid.Column>
                        <Grid.Column style={{color: 'green'}} width={3}>eInvd:</Grid.Column>
                        <Grid.Column width={3}>{eInvoiced}</Grid.Column>
                    </Grid.Row>
                </Grid>
                <Tab  menu={{attached: 'bottom'}} panes={panes} style={{minHeight: '100vh'}}/>
            </Fragment>)
            }
        </Fragment>
    )
}

export default ReceivingList;