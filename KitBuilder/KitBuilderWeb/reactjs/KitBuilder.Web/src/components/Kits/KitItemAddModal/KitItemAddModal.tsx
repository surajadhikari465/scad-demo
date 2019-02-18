import * as React from 'react';
import KitItemAddModalLabelSearchBox from './KitItemAddModalSearch';
import KitItemAddModalDisplay from './KitItemAddModalDisplay';
import { Grid, Button } from '@material-ui/core';
import Dialog from '@material-ui/core/Dialog';
import DialogTitle from '@material-ui/core/DialogTitle';
import DialogContent from '@material-ui/core/DialogContent';
import DialogActions from '@material-ui/core/DialogActions';
const hStyle = { color: 'red' };
const successStyle = { color: 'blue' };
const ModalStyle = { margin: "auto", width:"500px", marginTop: "10px", overlay: { zIndex: 10 } }

function KitItemAddModal(props: any) {

    return (
        <React.Fragment>
            <Dialog
                
                open={props.open}
            >
                <DialogTitle id="form-dialog-title">Kit Item Add Screen</DialogTitle>
                <DialogContent >
                <Grid container justify="center">
                    <Grid container justify="center">
                        <div className="error-message" >
                            <span style={hStyle}> {props.errorKitItem}</span>
                        </div>
                    </Grid>
                    <Grid container justify="center">
                        <div className="Success-message" >
                            <span style={successStyle}> {props.messageKitItem}</span>
                        </div>
                    </Grid>
                </Grid>

                    <Grid container justify="center" style={ModalStyle}>
                        <Grid item md={10}>
                            <KitItemAddModalLabelSearchBox
                                mainItemName={props.mainItemName}
                                mainItemlabelName={props.mainItemlabelName}
                                onMainItemChange={props.onMainItemChange}
                                scanCodeName={props.scanCodeName}
                                scanCodelabelName={props.scanCodelabelName}
                                onScanCodeChange={props.onScanCodeChange}
                                onSearch={props.onSearch}
                                mainItem={props.mainItem}
                                scanCode={props.scanCode}
                                data={props.data}
                            />
                        </Grid>
                        {
                            props.showDisplay ?
                                <Grid item md={10}>

                                    < KitItemAddModalDisplay
                                        data={props.data}
                                        onChecked={props.onChecked}
                                    />
                                </Grid>
                                : <></>
                        }
                    </Grid>
                </DialogContent>
                <DialogActions>
                    <Button onClick={props.onClose} color="primary">
                        Cancel
          </Button>

                </DialogActions>
            </Dialog>
        </React.Fragment>
    )
}

export default KitItemAddModal;