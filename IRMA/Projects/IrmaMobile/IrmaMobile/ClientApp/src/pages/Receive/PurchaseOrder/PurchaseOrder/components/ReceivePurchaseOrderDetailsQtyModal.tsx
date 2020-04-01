import React, { Fragment } from 'react'
import { Modal } from 'semantic-ui-react'
import { QuantityAddMode } from '../../types/QuantityAddMode';

interface IProps {
    open: boolean;
    handleQuantityDecision: (decision: QuantityAddMode) => any;
    quantity: number;
}

const ReceivePurchaseOrderDetailsQtyModal: React.FC<IProps> = ({ open, handleQuantityDecision, quantity }) => {
    return (
        <Fragment>
            <Modal open={open}>
                <Modal.Header>Previously Scanned Item</Modal.Header>
                <Modal.Content>
                    {`${quantity} of this item already queued in this session. Tap Add, Overwrite, or Cancel.`}
                </Modal.Content>
                <Modal.Actions>
                    <button className="irma-btn" style={{marginRight: '20px'}} onClick={() => handleQuantityDecision(QuantityAddMode.AddTo)} >Add</button>
                    <button className="irma-btn" style={{marginRight: '20px'}} onClick={() => handleQuantityDecision(QuantityAddMode.Overwrite)}>Overwrite</button>
                    <button className="irma-btn" onClick={() => handleQuantityDecision(QuantityAddMode.None)}>Cancel</button>
                </Modal.Actions>
            </Modal>
        </Fragment>
    )
}

export default ReceivePurchaseOrderDetailsQtyModal;