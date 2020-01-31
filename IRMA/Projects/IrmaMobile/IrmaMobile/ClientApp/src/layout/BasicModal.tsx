import React, { Fragment } from 'react'
import { Modal, Header, Button } from 'semantic-ui-react'

interface IProps {
    alert:{open:any, header:any, alertMessage:any, confirmAction: any, cancelAction: any, type: any},
    toggleAlert?: any, 
    add?: any,
    overwrite?:any,
    setAlert?: any
}

const BasicModal: React.FC<IProps> = ({alert, add, overwrite, setAlert}) => {
    const cancel = (e:any) => {
        setAlert({...alert, open: false});
    }
    const toggle = (e:any) =>{
        setAlert({...alert, 
          open:!alert.open});
    }
    return (
        <Fragment>
            <Modal open={alert.open}>   
                <Header content={alert.header} />
                <Modal.Content>
                    {alert.alertMessage}
                </Modal.Content>
                <Modal.Actions>
                    {alert.type==='default' ? (
                        <Button onClick={toggle}> OK </Button>
                    ): alert.type==='confirm' ? (
                    <Fragment>
                        <Button onClick={alert.confirmAction}> Yes </Button>
                        <Button onClick={alert.cancelAction ? alert.cancelAction: cancel}> No </Button>
                    </Fragment>
                    ):(
                    <Fragment>
                        <Button onClick={add}> Add </Button>
                        <Button onClick={overwrite}> Overwrite </Button>
                        <Button onClick={alert.cancelAction ? alert.cancelAction: cancel}> Cancel </Button>
                    </Fragment>
                    )}
                </Modal.Actions>
            </Modal>
        </Fragment>
    )
}

export default BasicModal