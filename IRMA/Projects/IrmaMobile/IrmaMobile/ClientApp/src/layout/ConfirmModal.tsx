import React, { Fragment, useState } from 'react'
import { Modal, Icon, Grid } from 'semantic-ui-react'
import { WfmButton } from '@wfm/ui-react'



/*

    If openExternal is used, you need to pass in its useState setter so that the window can close

    Also, doing a shorthanded version of Modal may be a better solution than this component...

*/



interface IProps {
    handleConfirmClose?: () => any;
    lineOne?: string;
    lineTwo?: string;
    headerText: string;
    confirmButtonText: string;
    cancelButtonText: string;
    triggerButtonText?: string;
    showTriggerButton?: boolean;
    openExternal?: boolean;
    setOpenExternal?: (state: boolean) => any; 
}

const ConfirmModal: React.FC<IProps> = ({ handleConfirmClose, lineOne, lineTwo, confirmButtonText, cancelButtonText, triggerButtonText, showTriggerButton = true, openExternal = false, setOpenExternal, headerText}) => {
    const [open, setOpen] = useState<boolean>(false)

    const handleCancelClick = () => {
        if(setOpenExternal) {
            setOpenExternal(false);
        }

        setOpen(false);
    }

    const handleConfirmClick = () => {
        if(setOpenExternal) {
            setOpenExternal(false);
        }

        if(handleConfirmClose) {
            handleConfirmClose();
        }
    }

    return (
        <Fragment>
            <Modal open={open || openExternal} onClose={handleCancelClick} trigger={showTriggerButton && <WfmButton onClick={() => {setOpen(true)}}>{triggerButtonText}</WfmButton>}>
                <Modal.Header>{headerText}</Modal.Header>
                <Modal.Content>
                    <Modal.Description>
                        <Grid columns={2}>
                            <Grid.Column width={4}>
                                <Icon name='question circle' size='huge'/>
                            </Grid.Column>
                            <Grid.Column width={12}>
                                <p>
                                    {lineOne}
                                </p>
                                {lineTwo &&
                                <p>
                                    {lineTwo}
                                </p>}
                            </Grid.Column>
                        </Grid>
                    </Modal.Description>
                </Modal.Content>
                <Modal.Actions>
                    <WfmButton onClick={handleConfirmClick} style={{marginRight: '20px'}}>{confirmButtonText}</WfmButton>
                    <WfmButton onClick={handleCancelClick}>{cancelButtonText}</WfmButton>
                </Modal.Actions>
            </Modal>
        </Fragment>
    )
}

export default ConfirmModal