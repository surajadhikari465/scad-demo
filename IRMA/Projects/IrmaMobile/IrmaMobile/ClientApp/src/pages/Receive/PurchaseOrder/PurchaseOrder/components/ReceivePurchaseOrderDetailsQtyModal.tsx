import React, { Fragment } from 'react'
import { Modal } from 'semantic-ui-react'
import { WfmButton } from '@wfm/ui-react'
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
                    {`${quantity} of this item already queued in this session. Tap Add, Overwrite or Cancel`}
                </Modal.Content>
                <Modal.Actions>
                    <WfmButton style={{marginRight: '20px'}} onClick={() => handleQuantityDecision(QuantityAddMode.AddTo)} >Add</WfmButton>
                    <WfmButton style={{marginRight: '20px'}} onClick={() => handleQuantityDecision(QuantityAddMode.Overwrite)}>Overwrite</WfmButton>
                    <WfmButton onClick={() => handleQuantityDecision(QuantityAddMode.None)}>Cancel</WfmButton>
                </Modal.Actions>
            </Modal>
        </Fragment>
    )
}

export default ReceivePurchaseOrderDetailsQtyModal;