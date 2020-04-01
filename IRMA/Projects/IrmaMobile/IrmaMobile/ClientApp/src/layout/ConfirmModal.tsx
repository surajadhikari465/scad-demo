import React, { Fragment, useState } from 'react'
import { Modal, Grid } from 'semantic-ui-react'

/*

    If openExternal is used, you need to pass in its useState setter so that the window can close

    Also, doing a shorthanded version of Modal may be a better solution than this component...

*/



interface IProps {
    handleConfirmClose?: () => any;             //optional: function to be called upon the confirm button being clicked
    handleCancelClose?: () => any;              //optional: function to be called upon the cancel button being clicked
    lineOne?: string;                           //optional: first line of text
    lineTwo?: string;                           //optional: second line of text
    headerText: string;                         //optional: text to be shown at the top of the modal
    confirmButtonText: string;                  //optional: text to be shown on the confirm (left) button
    cancelButtonText: string;                   //optional: text to be shown on the cancel (right) button
    triggerButtonText?: string;                 //optional: text to be shown on the external button that opens the modal
    showTriggerButton?: boolean;                //optional, default true: whether or not to generate an external button for the user to click to open the modal
    openExternal?: boolean;                     //required: state used to tell the modal to be visible or not. Must be same state variable that is attached to setOpenExternal
    noGreyClick?: boolean;                      //optional: if true, the user cannot close the modal by clicking the grey area
    enableButton?: boolean;                     //optional, default true: determines whether or not the button is greyed out
    setOpenExternal?: (state: boolean) => any;  //required: the useState setter used to control openExternal
}

const ConfirmModal: React.FC<IProps> = ({ enableButton = true, noGreyClick = false, handleConfirmClose, lineOne, lineTwo, confirmButtonText, cancelButtonText, triggerButtonText, showTriggerButton = true, openExternal = false, setOpenExternal, headerText, handleCancelClose }) => {
    const [open, setOpen] = useState<boolean>(false)
    
    const handleCancelClick = () => {
        if(setOpenExternal) {
            setOpenExternal(false);
        }

        if(handleCancelClose) {
            handleCancelClose();
        }
    }

    const handleConfirmClick = () => {
        if(setOpenExternal) {
            setOpenExternal(false);
        }

        setOpen(false);

        if(handleConfirmClose) {
            handleConfirmClose();
        }
    }

    return (
        <Fragment>
            <Modal closeOnDimmerClick={noGreyClick} open={open || openExternal} onClose={handleCancelClick} trigger={showTriggerButton && <button className="irma-btn" disabled={!enableButton} onClick={() => {setOpen(true)}}>{triggerButtonText}</button>}>
                <Modal.Header>{headerText}</Modal.Header>
                <Modal.Content>
                    <Modal.Description>
                        <Grid columns={1}>
                            <Grid.Column style={{marginLeft: '10px'}}>
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
                    <button className="irma-btn" onClick={handleConfirmClick} style={{marginRight: '20px'}}>{confirmButtonText}</button>
                    <button className="irma-btn" onClick={handleCancelClick}>{cancelButtonText}</button>
                </Modal.Actions>
            </Modal>
        </Fragment>
    )
}

export default ConfirmModal