import React, { Fragment, useContext, useState } from 'react'
import { Dropdown, Grid, DropdownProps } from 'semantic-ui-react';
import OrderItem from '../../types/OrderItem';
import { AppContext, IMappedReasonCode } from "../../../../../store";
import agent from '../../../../../api/agent';
import Config from '../../../../../config';

interface IProps {
    orderItem: OrderItem;
}

const ReceivingListLineItem: React.FC<IProps> = ({ orderItem }) => {
    // @ts-ignore
    const { state } = useContext(AppContext);
    const { mappedReasonCodes, region } = state;

    const [code, setCode] = useState<number>(parseInt(orderItem.Code));

    const handleCodeChange = async (event: React.SyntheticEvent<HTMLElement, Event>, data: DropdownProps) => {
        if(data.value) {
            orderItem.Code = data.value.toString();
            setCode(parseInt(orderItem.Code)); 

            await agent.PurchaseOrder.receiveOrder(
                region,
                orderItem.QtyReceived,
                orderItem.Weight,
                new Date(),
                false,
                orderItem.OrderItemId,
                parseInt(orderItem.Code),
                0,
                Config.userId
            );
        }
    }

    const dropdown = (
        <Dropdown selection compact placeholder='Code'
            value={code}
            onChange={handleCodeChange}
            options = {mappedReasonCodes.map((item: IMappedReasonCode) => { return {key: item.key, value: item.value, text: item.text} } )} />
    )

    return (
        <Fragment>  
            <div style={{backgroundColor: 'lightgrey'}}>
                <Grid style={{marginTop: '0px', paddingRight: '0px', maxWidth: '100%'}} columns={4}>
                    <Grid.Column style={{color: 'blue', fontWeight: 'bold'}} width={2}>UPC:</Grid.Column>
                    <Grid.Column width={4}>{orderItem.Upc}</Grid.Column>
                    <Grid.Column style={{color: 'blue', fontWeight: 'bold'}} width={2}>Desc:</Grid.Column>
                    <Grid.Column width={8}>{orderItem.Description}</Grid.Column>
                </Grid>
            </div> 
            <Grid style={{marginTop: '0px', marginRight: '0px'}}>
                <Grid.Row style={{paddingTop: '0px', paddingBottom: '0px'}} columns={6} color='teal'>
                    <Grid.Column textAlign='center' width={2}>Ord</Grid.Column>
                    <Grid.Column textAlign='center' width={3}>Recd</Grid.Column>
                    <Grid.Column textAlign='center' width={2}>eQty</Grid.Column>
                    <Grid.Column textAlign='center' width={2}>Wt</Grid.Column>
                    <Grid.Column textAlign='center' width={2}>eWt</Grid.Column>
                    <Grid.Column textAlign='center' width={5}>Code</Grid.Column>
                </Grid.Row>
                <Grid.Row style={{paddingTop: '0px'}}>
                    <Grid.Column style={{paddingTop: '0px'}} verticalAlign='middle'width={2}>{orderItem.QtyOrdered}</Grid.Column>
                    <Grid.Column style={{paddingTop: '0px'}} verticalAlign='middle' width={3}>{orderItem.QtyReceived}</Grid.Column>
                    <Grid.Column style={{paddingTop: '0px'}} verticalAlign='middle' width={2}>{orderItem.eInvoiceQty}</Grid.Column>
                    <Grid.Column style={{paddingTop: '0px'}} verticalAlign='middle' width={2}>{orderItem.Weight}</Grid.Column>
                    <Grid.Column style={{paddingTop: '0px'}} verticalAlign='middle' width={2}>{orderItem.eInvoiceWeight}</Grid.Column>
                    <Grid.Column style={{paddingTop: '0px'}} textAlign='center' width={5}>{dropdown}</Grid.Column>
                </Grid.Row>
            </Grid>
        </Fragment>
    )
}

export default ReceivingListLineItem;