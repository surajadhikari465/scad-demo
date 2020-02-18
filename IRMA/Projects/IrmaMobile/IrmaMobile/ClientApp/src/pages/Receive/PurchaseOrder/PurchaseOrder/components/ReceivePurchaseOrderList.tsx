import React, { Fragment } from "react";
import { Grid, Segment } from "semantic-ui-react";
import { ListedOrder } from "../../types/ListedOrder";
import "./styles.scss"
import dateFormat from 'dateformat'

interface IProps {
    orders: ListedOrder[];
    upc: string;
    poSelected: ( upc: string, poNum: number, closeOrderList: boolean) => any;
}

const ReceivePurchaseOrderList: React.FC<IProps> = ({ orders, upc, poSelected }) => {
    console.log('orders', orders);
    const purchaseOrderClicked = (e: any) => {
        console.log('purchase order clicked');
        poSelected(upc, e.currentTarget.innerText,true);
    }

    return (
        <Fragment>
            <Segment textAlign='center' size='large' basic style={{fontWeight: 'bold'}}>UPC: {upc}</Segment>
            <Grid columns={5} celled>
                <Grid.Row color="grey" style={{fontSize: '10px'}} textAlign='center'>
                    <Grid.Column>PO #</Grid.Column>
                    <Grid.Column>Order Cost</Grid.Column>
                    <Grid.Column>Exp. Date</Grid.Column>
                    <Grid.Column>Subteam</Grid.Column>
                    <Grid.Column>Einv</Grid.Column>
                </Grid.Row>

                {orders.map(order => (
                    <Grid.Row key={order.PoNum}>
                        <Grid.Column style={{fontWeight: 'bold'}}><button className="link-button" onClick={purchaseOrderClicked}>{order.PoNum}</button></Grid.Column>
                        <Grid.Column>{order.OrderCost}</Grid.Column>
                        <Grid.Column style={{fontSize: '12px'}}>{dateFormat(order.ExpectedDate, "mm/dd/yyyy")}</Grid.Column>
                        <Grid.Column>{order.Subteam}</Grid.Column>
                        <Grid.Column textAlign='center'>{order.EInv ? "Y" : "N"}</Grid.Column>
                    </Grid.Row>
                ))}
            </Grid>
        </Fragment>
    );
};

export default ReceivePurchaseOrderList;
