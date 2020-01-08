import React, { Fragment, useState } from 'react'
import { Modal, Icon, Grid } from 'semantic-ui-react'
import { WfmButton } from '@wfm/ui-react'



/*

    If openExternal is used, you need to pass in its useState setter so that the window can close

    Also, doing a shorthanded version of Modal may be a better solution than this component...

*/



interface IProps {
    handleConfirmClose?: () => any;             //optional: function to be called upon the confirm button being clicked
    lineOne?: string;                           //optional: first line of text
    lineTwo?: string;                           //optional: second line of text
    headerText: string;                         //optional: text to be shown at the top of the modal
    confirmButtonText: string;                  //optional: text to be shown on the confirm (left) button
    cancelButtonText: string;                   //optional: text to be shown on the cancel (right) button
    triggerButtonText?: string;                 //optional: text to be shown on the external button that opens the modal
    showTriggerButton?: boolean;                 //optional, default true: whether or not to generate an external WfmButton for the user to click to open the modal
    openExternal?: boolean;                      //required: state used to tell the modal to be visible or not. Must be same state variable that is attached to setOpenExternal
    setOpenExternal?: (state: boolean) => any;   //required: the useState setter used to control openExternal
}

const ConfirmModal: React.FC<IProps> = ({ handleConfirmClose, lineOne, lineTwo, confirmButtonText, cancelButtonText, triggerButtonText, showTriggerButton = true, openExternal = false, setOpenExternal, headerText}) => {
    const [open, setOpen] = useState<boolean>(false)
    
    const handleCancelClick = () => {
        if(setOpenExternal) {
            setOpenExternal(false);
        }
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