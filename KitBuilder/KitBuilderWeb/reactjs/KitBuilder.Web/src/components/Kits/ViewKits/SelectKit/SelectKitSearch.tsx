import * as React from 'react'
import { Grid, TextField, Button } from '@material-ui/core';
import { withStyles } from '@material-ui/core/styles';

const styles = (theme: any) => ({
    root: {
        padding: theme.spacing.unit,
    },
    button: {
        width: '100%',
    },
    clearButton: {
        borderColor: 'red',
    },
    labelRoot: {
        fontSize: 18,
        fontWeight: 600
    },
    textField:
    {
        minWidth: 120,
        width: '100%'
    }
});

function SelectKitSearch(props: any) {

    return (
        <React.Fragment>
            <Grid container justify="space-between" alignItems="center" className={props.classes.root + ' kit-search-container'}>
                <Grid item xs={12} md={5}>
                    <TextField className='search-textfield' margin="dense" variant="outlined" label="Main Item Name" InputLabelProps={{
                        shrink: true,
                        FormLabelClasses: {
                            root: props.classes.labelRoot
                        }
                    }} onChange={props.mainItemName} value={props.mainItemValue}></TextField>
                </Grid>
                <Grid item xs={12} md={5}>
                    <TextField className='search-textfield' margin="dense" variant="outlined" label='Main Item Scancode' InputLabelProps={{
                        shrink: true,
                        FormLabelClasses: {
                            root: props.classes.labelRoot
                        }
                    }} onChange={props.mainItemScanCode} value={props.scanCodeValue}></TextField>
                </Grid>

                <Grid item xs={12} md={5}>
                    <TextField className='search-textfield' margin="dense" variant="outlined" label='Link Group Name' InputLabelProps={{
                        shrink: true,
                        FormLabelClasses: {
                            root: props.classes.labelRoot
                        }
                    }} onChange={props.linkGroupName} value={props.linkGroupValue}></TextField>
                </Grid>
                <Grid item xs={12} md={5}>
                    <TextField className='search-textfield' margin="dense" variant="outlined" label='Kit Description' InputLabelProps={{
                        shrink: true,
                        FormLabelClasses: {
                            root: props.classes.labelRoot
                        }
                    }} onChange={props.kitDescription} value={props.kitDescriptionValue}></TextField>
                </Grid>
                <Grid xs={12} md={5} item></Grid>
                <Grid xs={12} md={5} item>
                    <Button variant="contained" color="primary" className={props.classes.button} onClick={() => props.onSearch()} >
                        Search
                    </Button>
                </Grid>
            </Grid>
        </React.Fragment>
    )
}

export default withStyles(styles, { withTheme: true })(SelectKitSearch);