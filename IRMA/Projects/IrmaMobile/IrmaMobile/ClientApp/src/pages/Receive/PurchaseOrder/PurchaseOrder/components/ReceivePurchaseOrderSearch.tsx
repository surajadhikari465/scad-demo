import React, { Fragment, useContext, ChangeEvent } from "react";
import { Grid, Form, Button, Input } from "semantic-ui-react";
import { AppContext, types } from "../../../../../store";

interface IProps {
    handleSubmit: (upc: string, purchaseOrderNumber: string, closeOrderList: boolean) => any;
}

const ReceivePurchaseOrderSearch: React.FC<IProps> = ({ handleSubmit }) => {
    //@ts-ignore
    const { state, dispatch } = useContext(AppContext);
    const { isLoading, purchaseOrderNumber, purchaseOrderUpc } = state;

    const handleSubmitFromPoBox = (event: any) => {
        handleSubmit(isNaN(parseInt(purchaseOrderUpc)) ? '' : purchaseOrderUpc, 
            isNaN(parseInt(purchaseOrderNumber)) ? '' : purchaseOrderNumber, 
            true);
    };

    return (
        <Fragment>
            <Form onSubmit={handleSubmitFromPoBox}>
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
                                        onChange={(e: ChangeEvent<HTMLInputElement>) => dispatch({ type: types.SETPURCHASEORDERNUMBER, purchaseOrderNumber: parseInt(e.target.value).toString() })}
                                    />
                                </Form.Field>
                                <Form.Field>
                                    <Input
                                        name="purchaseOrderUpc"
                                        type="number"
                                        placeholder="UPC"
                                        min={0}
                                        value={purchaseOrderUpc}
                                        onChange={(e: ChangeEvent<HTMLInputElement>) => dispatch({ type: types.SETPURCHASEORDERUPC, purchaseOrderUpc: parseInt(e.target.value).toString() })}
                                    />
                                </Form.Field>
                            </Grid.Column>
                            <Grid.Column>
                                <Button
                                    loading={isLoading}
                                    type="submit"
                                    content="Search"
                                />
                            </Grid.Column>
                        </Form.Group>
                    </Grid.Row>
                </Grid>
            </Form>
        </Fragment>
    );
};

export default ReceivePurchaseOrderSearch;
