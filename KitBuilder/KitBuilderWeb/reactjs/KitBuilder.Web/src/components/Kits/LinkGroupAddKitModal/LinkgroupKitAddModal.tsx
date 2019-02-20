import * as React from 'react';
import LinkGroupKitAddModalSearch from './LinkGroupKitAddModalSearch';
import KitItemAddModalDisplay from './KitItemAddModalDisplay';
import { Grid, Button } from '@material-ui/core';
import Dialog from '@material-ui/core/Dialog';
import DialogTitle from '@material-ui/core/DialogTitle';
import DialogContent from '@material-ui/core/DialogContent';
import DialogActions from '@material-ui/core/DialogActions';
const hStyle = { color: 'red' };
const successStyle = { color: 'blue' };
const ModalStyle = { margin: "auto", marginTop: "10px", overlay: { zIndex: 10 } }

function LinkgroupKitAddModal(props: any) {
     return (
     
        <React.Fragment>
            <Dialog fullWidth = {true}
                maxWidth = 'lg'
                open={props.open}
            >
            < Grid container justify="center">
                <DialogTitle id="form-dialog-title" >Link Group Add to Kit Screen</DialogTitle>
                </Grid>
                <DialogContent >
                < Grid container justify="center">
                    <Grid container justify="center">
                        <div className="error-message" >
                            <span style={hStyle}> {props.errorlinkGroupAdd}</span>
                        </div>
                    </Grid>
                    <Grid container justify="center">
                        <div className="Success-message" >
                            <span style={successStyle}> {props.messagelinkGroupAdd}</span>
                        </div>
                    </Grid>
                </Grid>
                    <Grid container justify="center" style={ModalStyle}>  
                        <Grid item md={10}>
                            <LinkGroupKitAddModalSearch
                                linkGroupName={props.linkGroupName}
                                linkGrouplabelName={props.linkGrouplabelName}
                                onLinkGroupChange={props.onLinkGroupChange}
                                modifierPluName={props.modifierPluName}
                                modifierPlulabelName={props.modifierPlulabelName}
                                onModifierPluChange={props.onModifierPluChange}
                                onLinkGroupSearch={props.onLinkGroupSearch}
                                searchLinkGroupText={props.searchLinkGroupText}
                                searchModifierPluText={props.searchModifierPluText}               
                            />
                        </Grid>
                        {
                                <Grid item md={12}>
                                    < KitItemAddModalDisplay
                                        linkGroupData={props.linkGroupData}
                                        selectedData={props.selectedData}
                                        onRemove={props.onRemove}
                                        onSelect={props.onSelect}
                                        queueLinkGroups = {props.queueLinkGroups}
                                        addToKit = {props.addToKit}
                                    />
                                </Grid>
                        }
                    </Grid>
                </DialogContent>
                <DialogActions>
                    <Button onClick={props.onLinkGroupClose} color="primary">
                        Cancel
          </Button>

                </DialogActions>
            </Dialog>
        </React.Fragment>
    )
}

export default LinkgroupKitAddModal;