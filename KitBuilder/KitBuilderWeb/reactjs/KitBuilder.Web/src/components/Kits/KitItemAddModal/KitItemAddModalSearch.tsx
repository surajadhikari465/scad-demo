import * as React from 'react';
import { Grid, Button } from '@material-ui/core';
import KitItemAddModalLabelTextBox from './KitItemModalLabelTextBox';
import { withStyles } from '@material-ui/core/styles';
const styles = (theme: any) => ({
    searchButtons: {
        width: '150px',
        marginRight: '20px',
        marginTop: '10px',
        marginBottom: '10px'
    }
})

function KitItemAddModalLabelSearchBox(props: any) {

    return (
        <React.Fragment>
            <Grid container justify="flex-start">
                < Grid item md={12}>
                    <KitItemAddModalLabelTextBox
                        Name={props.mainItemName}
                        labelName={props.mainItemlabelName}
                        onChange={props.onMainItemChange}
                        value={props.mainItem}
                        labelValue = {props.mainItemlabelName}
                    />
                </Grid>
            < Grid item md={12}>
                <KitItemAddModalLabelTextBox
                    Name={props.scanCodeName}
                    labelName={props.scanCodelabelName}
                    onChange={props.onScanCodeChange}
                    value={props.scanCode}
                    labelValue = {props.scanCodelabelName}
                />
                            </Grid>
            
            < Grid item md={12}>
            < Grid container justify="flex-end">                   
                <Button
                    variant="contained"
                    color="primary"
                    className={props.classes.searchButtons}
                    onClick={() => props.onSearch()}
                >
                    Search
                            </Button>
            </Grid>
            </Grid>
            </Grid>
        </React.Fragment>
    )
}
export default withStyles(styles, { withTheme: true })(KitItemAddModalLabelSearchBox);
