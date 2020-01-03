import React, { useContext, Fragment } from "react";
import { Modal, Grid } from "semantic-ui-react";
import { ReasonCode } from "../../types/ReasonCode";
import { AppContext, types } from "../../../../../store";

const ReceivePurchaseOrderReasonCodeModal: React.FC = () => {
    // @ts-ignore
    const { state, dispatch } = useContext(AppContext);
    const { modalOpen, reasonCodes } = state;

    const close = () => {
        dispatch({ type: types.SETMODALOPEN, modalOpen: false });
    };

    return (
        <Fragment>
            <Modal open={modalOpen} onClose={close} closeIcon>
                <Modal.Header>Reason Code Descriptions</Modal.Header>
                <Modal.Content>
                    <Grid columns={2} celled>
                        <Grid.Row color="grey">
                            <Grid.Column width={4}>Code</Grid.Column>
                            <Grid.Column width={12}>Description</Grid.Column>
                        </Grid.Row>
                        {reasonCodes.map((code: ReasonCode) => (
                            <Grid.Row key={code.reasonCodeAbbreviation}>
                                <Grid.Column width={4}>
                                    {code.reasonCodeAbbreviation}
                                </Grid.Column>
                                <Grid.Column width={12}>
                                    {code.reasonCodeDescription}
                                </Grid.Column>
                            </Grid.Row>
                        ))}
                    </Grid>
                </Modal.Content>
            </Modal>
        </Fragment>
    );
};

export default ReceivePurchaseOrderReasonCodeModal;
