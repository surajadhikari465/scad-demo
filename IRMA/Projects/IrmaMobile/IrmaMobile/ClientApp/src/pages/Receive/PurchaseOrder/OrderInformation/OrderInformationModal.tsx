import React, { Fragment } from 'react'
import { Modal, Form, Header, Segment, Grid } from 'semantic-ui-react';
import OrderInformation from '../types/OrderInformation';
import dateFormat from 'dateformat';

interface IProps {
    open: boolean;
    orderInformation: OrderInformation;
    handleOnClose: () => any;
}

const OrderInformationModal: React.FC<IProps> = ({ open, orderInformation, handleOnClose }) => {
    return ( 
        <Fragment>
            <Modal onClose={handleOnClose} closeIcon open={open}>
                <Modal.Header>Order Information</Modal.Header>
                <Modal.Content scrolling style={{padding: '0px'}}>
                    <Grid>
                        <Grid.Row centered>
                            <Form>
                                <Form.Group inline style={{marginBottom: '10px'}}>
                                    <Form.Checkbox
                                        disabled
                                        label="Product" 
                                        checked={orderInformation && !orderInformation.isCreditOrder}
                                    />
                                    <Form.Checkbox
                                        disabled
                                        label="Credit"
                                        checked={orderInformation && orderInformation.isCreditOrder}
                                    />
                                </Form.Group>
                            </Form>
                        </Grid.Row>
                    </Grid>
                    <Header as="h4">Order Notes:</Header>
                    <Segment basic>{orderInformation && orderInformation.orderNotes}</Segment>
                    <Grid columns={2} style={{backgroundColor: 'darkgrey'}}>
                        <Grid.Row>
                            <Grid.Column textAlign='right' style={{fontWeight: 'bold'}}>Buyer:</Grid.Column>
                            <Grid.Column>{orderInformation && orderInformation.buyer}</Grid.Column>
                        </Grid.Row>
                        <Grid.Row>
                            <Grid.Column textAlign='right' style={{fontWeight: 'bold'}}>Order Date:</Grid.Column>
                            <Grid.Column>{orderInformation && orderInformation.orderDate && dateFormat(new Date(orderInformation.orderDate), "mm/dd/yyyy")}</Grid.Column>
                        </Grid.Row>
                    </Grid>
                    <div style={{marginTop: '20px'}}/>
                </Modal.Content>
            </Modal>
        </Fragment>
    )
}

export default OrderInformationModal;