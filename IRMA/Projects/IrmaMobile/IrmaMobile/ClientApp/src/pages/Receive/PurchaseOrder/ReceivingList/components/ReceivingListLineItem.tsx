import React, { Fragment, useContext, useState } from 'react'
import { Dropdown, Grid, DropdownProps } from 'semantic-ui-react';
import OrderItem from '../../types/OrderItem';
import { AppContext, IMappedReasonCode, types } from "../../../../../store";
import agent from '../../../../../api/agent';
import { toast } from 'react-toastify';

interface IProps {
    orderItem: OrderItem;
    displayReasonCodeDropdown: boolean;
}

const ReceivingListLineItem: React.FC<IProps> = ({ orderItem, displayReasonCodeDropdown }) => {
    // @ts-ignore
    const { state, dispatch } = useContext(AppContext);
    const { mappedReasonCodes, region, orderDetails } = state;
    const [code, setCode] = useState<number>(orderItem.Code);

    const handleCodeChange = async (event: React.SyntheticEvent<HTMLElement, Event>, data: DropdownProps) => {
        if (orderDetails) {
            const reasonCodeId = data.value ? parseInt(data.value.toString()) : 0;
            const orderItemIndex = orderDetails.OrderItems.findIndex(oi => oi.OrderItemId === orderItem.OrderItemId);
            const orderItemsCopy = [...orderDetails.OrderItems]
            orderItemsCopy[orderItemIndex] = { ...orderItemsCopy[orderItemIndex], Code: reasonCodeId };
            dispatch({ type: types.SETORDERDETAILS, orderDetails: { ...orderDetails, OrderItems: orderItemsCopy } });
            try {
                const result = await agent.PurchaseOrder.updateReceivingDiscrepancyCode(region, orderItem.OrderItemId, reasonCodeId);
                if (!result.status) {
                    console.error(result.errorMessage);
                    toast.error(`Error updating receiving discrepancy code. Please try again. If the problem persists please contact support. ${result.errorMessage}`);
                }
            } catch (error) {
                console.error(error);
                toast.error('Error updating receiving discrepancy code. Please try again. If the problem persists please contact support.');
            }
            setCode(reasonCodeId);
        }
    }

    const dropdown = (
        <Dropdown selection compact placeholder='Code'
            className={displayReasonCodeDropdown && orderDetails!.EInvId > 0 && code === 0 ? 'warning-component' : ''}
            value={code}
            onChange={handleCodeChange}
            options={mappedReasonCodes.map((item: IMappedReasonCode) => { return { key: item.key, value: item.value, text: item.text } })} />
    )

    return (
        <Fragment>
            <div style={{ backgroundColor: 'lightgrey' }}>
                <Grid style={{ marginTop: '0px', paddingRight: '0px', maxWidth: '100%' }} columns={4}>
                    <Grid.Column style={{ color: 'blue', fontWeight: 'bold' }} width={2}>UPC:</Grid.Column>
                    <Grid.Column width={4}>{orderItem.Upc}</Grid.Column>
                    <Grid.Column style={{ color: 'blue', fontWeight: 'bold' }} width={2}>Desc:</Grid.Column>
                    <Grid.Column width={8}>{orderItem.Description}</Grid.Column>
                </Grid>
            </div>
            <Grid style={{ marginTop: '0px', marginRight: '0px' }}>
                <Grid.Row style={{ paddingTop: '0px', paddingBottom: '0px' }} columns={displayReasonCodeDropdown ? 6 : 5} color='teal'>
                    <Grid.Column textAlign='center' width={displayReasonCodeDropdown ? 2 : 3}>Ord</Grid.Column>
                    <Grid.Column textAlign='center' width={displayReasonCodeDropdown ? 3 : 4}>Recd</Grid.Column>
                    <Grid.Column textAlign='center' width={displayReasonCodeDropdown ? 2 : 3}>eQty</Grid.Column>
                    <Grid.Column textAlign='center' width={displayReasonCodeDropdown ? 2 : 3}>Wt</Grid.Column>
                    <Grid.Column textAlign='center' width={displayReasonCodeDropdown ? 2 : 3}>eWt</Grid.Column>
                    {displayReasonCodeDropdown && <Grid.Column textAlign='center' width={5}>Code</Grid.Column>}
                </Grid.Row>
                <Grid.Row style={{ paddingTop: '0px' }}>
                    <Grid.Column style={{ paddingTop: '0px' }} textAlign='center' verticalAlign='middle' width={displayReasonCodeDropdown ? 2 : 3}>{orderItem.QtyOrdered}</Grid.Column>
                    <Grid.Column style={{ paddingTop: '0px' }} textAlign='center' verticalAlign='middle' width={displayReasonCodeDropdown ? 3 : 4}>{orderItem.QtyReceived}</Grid.Column>
                    <Grid.Column style={{ paddingTop: '0px' }} textAlign='center' verticalAlign='middle' width={displayReasonCodeDropdown ? 2 : 3}>{orderItem.eInvoiceQty}</Grid.Column>
                    <Grid.Column style={{ paddingTop: '0px' }} textAlign='center' verticalAlign='middle' width={displayReasonCodeDropdown ? 2 : 3}>{orderItem.Weight}</Grid.Column>
                    <Grid.Column style={{ paddingTop: '0px' }} textAlign='center' verticalAlign='middle' width={displayReasonCodeDropdown ? 2 : 3}>{orderItem.eInvoiceWeight}</Grid.Column>
                    {displayReasonCodeDropdown && <Grid.Column style={{ paddingTop: '0px' }} textAlign='center' width={5}>{dropdown}</Grid.Column>}
                </Grid.Row>
            </Grid>
        </Fragment>
    )
}

export default ReceivingListLineItem;