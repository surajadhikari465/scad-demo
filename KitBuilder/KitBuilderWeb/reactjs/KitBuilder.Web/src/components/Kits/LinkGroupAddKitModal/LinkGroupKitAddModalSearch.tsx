import * as React from 'react';
import { Grid, Button } from '@material-ui/core';
import KitItemAddModalLabelTextBox from './LinkGroupKitModalLabelTextBox';
import { withStyles } from '@material-ui/core/styles';
const styles = (theme: any) => ({
    searchButtons: {
        width: '150px',
        marginRight: '20px',
        marginTop: '10px',
        marginBottom: '10px'
    }
})

function LinkGroupKitAddModalSearch(props: any) {

    return (
        <React.Fragment>
            <Grid container justify="flex-start">
                < Grid item xs = {12} sm={12}  md={5}>
                    <KitItemAddModalLabelTextBox
                        Name={props.linkGroupName}
                        labelName={props.linkGrouplabelName}
                        onChange={props.onLinkGroupChange}
                        value={props.searchLinkGroupText}
                        labelValue = {props.linkGrouplabelName}
                    />
                </Grid>
            < Grid item xs = {12} sm={12} md={5}>
                <KitItemAddModalLabelTextBox
                    Name={props.modifierPluName}
                    labelName={props.modifierPlulabelName}
                    onChange={props.onModifierPluChange}
                    value={props.searchModifierPluText}
                    labelValue = {props.modifierPlulabelName}
                />
                            </Grid>
            
              < Grid item xs = {12} sm={12}  md={2}>
                < Grid container justify="flex-start">                   
                    <Button
                        variant="contained"
                        color="primary"
                        className={props.classes.searchButtons}
                        onClick={() => props.onLinkGroupSearch()}
                    >
                        Search
                    </Button>
                </Grid>
            </Grid>
            </Grid>
        </React.Fragment>
    )
}
export default withStyles(styles, { withTheme: true })(LinkGroupKitAddModalSearch);
