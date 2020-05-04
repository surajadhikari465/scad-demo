import React, { Fragment, useContext, useEffect } from "react";
import { Grid, Segment } from "semantic-ui-react";
import { OrderByScanCode } from "../../types/OrderByScanCode";
import "./styles.scss"
import dateFormat from 'dateformat'
import { AppContext, types } from "../../../../../store";

interface IProps {
    orders: OrderByScanCode[];
    upc: string;
    orderSelected: (upc: string, orderHeaderId: number) => any;
}

const SelectOrderByScanCode: React.FC<IProps> = (props) => {
    //@ts-ignore
    const { state, dispatch } = useContext(AppContext);

    useEffect(() => {
        dispatch({ type: types.SETTITLE, Title: 'Find PO By Item' });
        return () => {
            dispatch({ type: types.SETTITLE, Title: 'Receive' });
        };
    }, [dispatch]);

    const purchaseOrderClicked = (e: React.MouseEvent<HTMLButtonElement>) => {
        const orderHeaderId = parseInt(e.currentTarget.textContent!);
        props.orderSelected(props.upc, orderHeaderId);
    }

    return (
        <Fragment>
            <Segment textAlign='center' size='large' basic style={{ fontWeight: 'bold' }}>UPC: {props.upc}</Segment>
            <Grid columns={5} celled>
                <Grid.Row color="grey" style={{ fontSize: '10px' }} textAlign='center'>
                    <Grid.Column>PO #</Grid.Column>
                    <Grid.Column>Order Cost</Grid.Column>
                    <Grid.Column>Exp. Date</Grid.Column>
                    <Grid.Column>Subteam</Grid.Column>
                    <Grid.Column>Einv</Grid.Column>
                </Grid.Row>

                {props.orders.map(order => (
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

export default SelectOrderByScanCode;
