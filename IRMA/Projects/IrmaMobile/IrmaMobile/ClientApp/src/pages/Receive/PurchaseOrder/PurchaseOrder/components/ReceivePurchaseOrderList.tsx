import React, { Fragment } from "react";
import { Grid, Segment } from "semantic-ui-react";
import { ListedOrder } from "../../types/ListedOrder";
import "./styles.scss"

interface IProps {
    orders: ListedOrder[];
    upc: string;
    poSelected: (poNum: number, upc: string, closeOrderList: boolean) => any;
}

const ReceivePurchaseOrderList: React.FC<IProps> = ({ orders, upc, poSelected }) => {

    const purchaseOrderClicked = (e: any) => {
        poSelected(e.currentTarget.innerText, upc, true);
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
                        <Grid.Column>{(new Date(order.ExpectedDate)).toLocaleDateString()}</Grid.Column>
                        <Grid.Column>{order.Subteam}</Grid.Column>
                        <Grid.Column textAlign='center'>{order.EInv ? "Y" : "N"}</Grid.Column>
                    </Grid.Row>
                ))}
            </Grid>
        </Fragment>
    );
};

export default ReceivePurchaseOrderList;
