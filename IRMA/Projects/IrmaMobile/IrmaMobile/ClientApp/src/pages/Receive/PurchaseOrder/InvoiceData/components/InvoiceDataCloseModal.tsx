import React, { Fragment, useState } from 'react'
import { Modal, Icon } from 'semantic-ui-react'
import { WfmButton } from '@wfm/ui-react'

interface IProps {
    invoiceDate: string;
    handleClose: () => any;
}

const InvoiceDataCloseModal: React.FC<IProps> = ({invoiceDate, handleClose}) => {
    const [open, setOpen] = useState<boolean>(false)

    return (
        <Fragment>
            <Modal open={open} trigger={<WfmButton className="wfmButton" onClick={() => {setOpen(true)}}>Close Invoice</WfmButton>}>
                <Modal.Header>Confirm Close Order</Modal.Header>
                <Modal.Content image>
                    <Icon wrapped name='question circle' size='huge'/>
                    <Modal.Description>
                        <div style={{textAlign: 'center'}}>
                            <p>Invoice Date: {invoiceDate}</p>
                            <p>Close this order?</p>
                            <Modal.Actions>
                                <WfmButton onClick={handleClose}>OK</WfmButton>
                                <WfmButton onClick={() => {setOpen(false)}}>Cancel</WfmButton>
                            </Modal.Actions>
                        </div>
                    </Modal.Description>
                </Modal.Content>
            </Modal>
        </Fragment>
    )
}

export default InvoiceDataCloseModal