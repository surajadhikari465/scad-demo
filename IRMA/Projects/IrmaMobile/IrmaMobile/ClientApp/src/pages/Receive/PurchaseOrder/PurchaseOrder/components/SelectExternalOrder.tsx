import React, { Fragment, useState, useEffect, useContext } from "react"
import { Grid } from "semantic-ui-react"
import { AppContext, types, IExternalOrder } from "../../../../../store";

interface IProps {
    upc: string;
    orders: IExternalOrder[];
    orderSelected: (upc: string, orderHeaderId: number) => void;
}

const SelectExternalOrder: React.FC<IProps> = (props) => {
    // @ts-ignore
    const { state, dispatch } = useContext(AppContext);
    const [orders] = useState<IExternalOrder[]>(props.orders);
    
    useEffect(() => {
        dispatch({ type: types.SETTITLE, Title: 'Select PO' });
        return () => {
          dispatch({ type: types.SETTITLE, Title: 'Receive' });
      };
    },[dispatch]);
    
    const handleOrderClicked = (e: React.MouseEvent<HTMLButtonElement>) => {
        const orderHeaderId = parseInt(e.currentTarget.textContent!);
        props.orderSelected(props.upc, orderHeaderId);
    }

    return (
        <Fragment>
            <Grid columns={3} celled>
                <Grid.Row color="grey" style={{ fontSize: '10px' }} textAlign='center'>
                    <Grid.Column>IRMAPO</Grid.Column>
                    <Grid.Column>Source</Grid.Column>
                    <Grid.Column>Vendor</Grid.Column>
                </Grid.Row>
                {orders.map(o => (
                    <Grid.Row key={o.orderHeaderId}>
                        <Grid.Column style={{ fontWeight: 'bold' }}><button className="link-button" onClick={handleOrderClicked}>{o.orderHeaderId}</button></Grid.Column>
                        <Grid.Column>{o.source}</Grid.Column>
                        <Grid.Column>{o.companyName}</Grid.Column>
                    </Grid.Row>
                ))}
            </Grid>
        </Fragment>
    );
};

export default SelectExternalOrder;