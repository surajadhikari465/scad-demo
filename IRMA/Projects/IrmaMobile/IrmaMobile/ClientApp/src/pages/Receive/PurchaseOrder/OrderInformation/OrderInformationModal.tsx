import React, { Fragment, useContext } from 'react'
import { Modal, Form, Header, Segment, Grid } from 'semantic-ui-react';
import dateFormat from 'dateformat';
import { AppContext } from '../../../../store';
import { OrderDetails } from '../types/OrderDetails';

interface IProps {
    open: boolean;
    handleOnClose: () => any;
}

const OrderInformationModal: React.FC<IProps> = ({ open, handleOnClose }) => {
    // @ts-ignore
    const { state, dispatch } = useContext(AppContext)
    const { orderDetails } = state;

    return (
        <Fragment>
            <Modal onClose={handleOnClose} closeIcon open={open}>
                <Modal.Header>Order Information</Modal.Header>
                <Modal.Content scrolling style={{ padding: '0px' }}>
                    <Grid>
                        <Grid.Row centered>
                            <Form>
                                <Form.Group inline style={{ marginBottom: '10px' }}>
                                    <Form.Checkbox
                                        disabled
                                        label="Product"
                                        checked={!orderDetails?.IsReturnOrder}
                                    />
                                    <Form.Checkbox
                                        disabled
                                        label="Credit"
                                        checked={orderDetails?.IsReturnOrder}
                                    />
                                </Form.Group>
                            </Form>
                        </Grid.Row>
                    </Grid>
                    <Header as="h4">Order Notes:</Header>
                    <Segment basic>{orderDetails && orderDetails.Notes}</Segment>
                    <Grid columns={2} style={{ backgroundColor: 'darkgrey' }}>
                        <Grid.Row>
                            <Grid.Column textAlign='right' style={{ fontWeight: 'bold' }}>Buyer:</Grid.Column>
                            <Grid.Column>{orderDetails && orderDetails.CreatedByName}</Grid.Column>
                        </Grid.Row>
                        <Grid.Row>
                            <Grid.Column textAlign='right' style={{ fontWeight: 'bold' }}>Order Date:</Grid.Column>
                            <Grid.Column>{orderDetails && orderDetails.OrderDate && dateFormat(new Date(orderDetails.OrderDate), "mm/dd/yyyy")}</Grid.Column>
                        </Grid.Row>
                    </Grid>
                    <div style={{ marginTop: '20px' }} />
                </Modal.Content>
            </Modal>
        </Fragment>
    )
}

export default OrderInformationModal;