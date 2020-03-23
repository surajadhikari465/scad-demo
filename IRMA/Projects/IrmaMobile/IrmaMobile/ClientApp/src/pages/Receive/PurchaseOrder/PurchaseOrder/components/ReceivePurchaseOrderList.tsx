import React, { Fragment, useContext, useEffect } from "react";
import { Grid, Segment } from "semantic-ui-react";
import { ListedOrder } from "../../types/ListedOrder";
import "./styles.scss"
import dateFormat from 'dateformat'
import { AppContext, types } from "../../../../../store";

interface IProps {
    orders: ListedOrder[];
    upc: string;
    poSelected: (upc: string, purchaseOrderNumber: string, closeOrderList: boolean) => any;
}

const ReceivePurchaseOrderList: React.FC<IProps> = ({ orders, upc, poSelected }) => {
    //@ts-ignore
    const { state, dispatch } = useContext(AppContext);

    useEffect(() => {
          dispatch({ type: types.SETTITLE, Title: 'Find PO By Item' });
          return () => {
            dispatch({ type: types.SETTITLE, Title: 'Receive' });
        };
      },[dispatch]);

      
    const purchaseOrderClicked = (e: React.MouseEvent<HTMLButtonElement>) => {
        const purchaseOrderNumber = parseInt(e.currentTarget.textContent!).toString();
        dispatch({ type: types.SETPURCHASEORDERNUMBER, purchaseOrderNumber: purchaseOrderNumber });
        poSelected(isNaN(parseInt(upc)) ? '' : upc, 
            isNaN(parseInt(purchaseOrderNumber)) ? '' : purchaseOrderNumber, 
            true);
    }

    return (
        <Fragment>
            <Segment textAlign='center' size='large' basic style={{ fontWeight: 'bold' }}>UPC: {upc}</Segment>
            <Grid columns={5} celled>
                <Grid.Row color="grey" style={{ fontSize: '10px' }} textAlign='center'>
                    <Grid.Column>PO #</Grid.Column>
                    <Grid.Column>Order Cost</Grid.Column>
                    <Grid.Column>Exp. Date</Grid.Column>
                    <Grid.Column>Subteam</Grid.Column>
                    <Grid.Column>Einv</Grid.Column>
                </Grid.Row>

                {orders.map(order => (
                    <Grid.Row key={order.PoNum}>
                        <Grid.Column style={{ fontWeight: 'bold' }}><button className="link-button" onClick={purchaseOrderClicked}>{order.PoNum}</button></Grid.Column>
                        <Grid.Column>{order.OrderCost}</Grid.Column>
                        <Grid.Column style={{ fontSize: '12px' }}>{dateFormat(order.ExpectedDate, "mm/dd/yyyy")}</Grid.Column>
                        <Grid.Column>{order.Subteam}</Grid.Column>
                        <Grid.Column textAlign='center'>{order.EInv ? "Y" : "N"}</Grid.Column>
                    </Grid.Row>
                ))}
            </Grid>
        </Fragment>
    );
};

export default ReceivePurchaseOrderList;
