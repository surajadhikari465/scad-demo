import React, { Fragment, useState } from 'react'
import { Modal, Icon, Form, Button } from 'semantic-ui-react'

interface IProps {
    invoiceDate: string;
    handleClose: () => any;
}

const ReceivePurchaseOrderCloseModal: React.FC<IProps> = ({invoiceDate, handleClose}) => {
    const [open, setOpen] = useState<boolean>(false)

    return (
        <Fragment>
            <Modal open={open} trigger={<button className="wfmButton" onClick={() => {setOpen(true)}}>Close Invoice</button>}>
                <Modal.Header>Confirm Close Order</Modal.Header>
                <Modal.Content image>
                    <Icon wrapped name='question circle' size='huge'/>
                    <Modal.Description>
                        <div style={{textAlign: 'center'}}>
                            <p>Invoice Date: {invoiceDate}</p>
                            <p>Close this order?</p>
                            <Form.Group inline><Button onClick={handleClose}>OK</Button><Button onClick={() => {setOpen(false)}}>Cancel</Button></Form.Group>
                        </div>
                    </Modal.Description>
                </Modal.Content>
            </Modal>
        </Fragment>
    )
}

export default ReceivePurchaseOrderCloseModal