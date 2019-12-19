import React, { useContext }from 'react'
import { Modal, Grid } from 'semantic-ui-react'
import { ReasonCode } from '../types/ReasonCode'
import { AppContext, types } from '../../../../store';

const ReceivePurchaseOrderReasonCodeModal: React.FC = () => {
    // @ts-ignore
    const { state, dispatch } = useContext(AppContext);
    const { modalOpen, reasonCodes } = state;
    
    const close = () => {
        dispatch({type: types.SETMODALOPEN, modalOpen: false})
    }

    return (
        <Modal open={modalOpen} onClose={close}>
            <Modal.Header>Reason Code Descriptions</Modal.Header>
            <Modal.Content>
                <Grid columns={2} celled>
                    <Grid.Row color="grey"> 
                        <Grid.Column width={5}>Abbreviation</Grid.Column>
                        <Grid.Column width={11}>Description</Grid.Column>
                    </Grid.Row>
                    {reasonCodes.map((code: ReasonCode) => (
                        <Grid.Row key={code.reasonCodeAbbreviation}>
                            <Grid.Column width={5}>{code.reasonCodeAbbreviation}</Grid.Column>
                            <Grid.Column width={11}>{code.reasonCodeDescription}</Grid.Column>
                        </Grid.Row>
                    ))}
                </Grid>
            </Modal.Content>
        </Modal>
    )
}

export default ReceivePurchaseOrderReasonCodeModal