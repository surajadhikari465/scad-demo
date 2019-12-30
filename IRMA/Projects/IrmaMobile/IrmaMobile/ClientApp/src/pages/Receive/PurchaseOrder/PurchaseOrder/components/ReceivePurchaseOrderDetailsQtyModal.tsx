import React, { Fragment } from 'react'
import { Modal } from 'semantic-ui-react'
import { WfmButton } from '@wfm/ui-react'
import { QuantityAddMode } from '../../types/QuantityAddMode';

interface IProps {
    open: boolean;
    handleQuantityDecision: (decision: QuantityAddMode) => any;
}

const ReceivePurchaseOrderDetailsQtyModal: React.FC<IProps> = ({ open, handleQuantityDecision }) => {
    return (
        <Fragment>
            <Modal open={open}>
                <Modal.Header>Previously Scanned Item</Modal.Header>
                <Modal.Content>
                    There is already quantity assigned to this PO.
                    <p/>
                    Do you want to add the entered quantity to the existing amount or overwrite the existing amount with the entered quantity?
                </Modal.Content>
                <Modal.Actions>
                    <WfmButton style={{marginRight: '20px'}} onClick={() => handleQuantityDecision(QuantityAddMode.AddTo)} >Add to</WfmButton>
                    <WfmButton style={{marginRight: '20px'}} onClick={() => handleQuantityDecision(QuantityAddMode.Overwrite)}>Overwrite</WfmButton>
                    <WfmButton onClick={() => handleQuantityDecision(QuantityAddMode.None)}>Cancel</WfmButton>
                </Modal.Actions>
            </Modal>
        </Fragment>
    )
}

export default ReceivePurchaseOrderDetailsQtyModal;