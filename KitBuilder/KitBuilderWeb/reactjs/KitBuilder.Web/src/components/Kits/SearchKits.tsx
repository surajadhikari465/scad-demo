import * as React from 'react'
import { Grid, TextField, Button } from '@material-ui/core';
import { withStyles } from '@material-ui/core/styles';
import './style.css';

const styles = (theme: any) => ({
    root: {
        padding: theme.spacing.unit,
    },
    button: {
        margin: theme.spacing.unit,
        width: '100%',
    },
    clearButton: {
        borderColor: 'red',
    },
    textField:
    {
        minWidth: 120,
        width: '100%'
    }
});

function SearchKits(props: any) {

    return (
        <React.Fragment>
            <Grid container justify="space-between" alignItems="center" className={props.classes.root + ' kit-search-container'}>
                <Grid item md={2}>
                    <TextField className = 'search-textfield ml-2' margin="dense" variant="outlined" label="Main Item Name" InputLabelProps={{ shrink: true }} onChange={props.MainItemName} value={props.MainItemValue}></TextField>
                </Grid>
                <Grid item md={2}>
                    <TextField className = 'search-textfield' margin="dense" variant="outlined" label='Main Item Scancode' InputLabelProps={{ shrink: true }} onChange={props.MainItemScanCode} value={props.ScanCodeValue}></TextField>
                </Grid>
                
                <Grid item md={2}>
                    <TextField className = 'search-textfield' margin="dense" variant="outlined" label='Link Group Name' InputLabelProps={{ shrink: true }} onChange={props.LinkGroupName} value={props.LinkGroupValue}></TextField>
                </Grid>
                <Grid item md={2}>
                    <TextField className = 'search-textfield' margin="dense" variant="outlined" label='Kit Description' InputLabelProps={{ shrink: true }} onChange={props.KitDescription} value={props.KitDescriptionValue}></TextField>
                </Grid>

                <Grid md={2} container justify="flex-end">
                    <Button variant="contained" color="primary" className={props.classes.button} onClick={() => props.onSearch()} >
                        Search
                    </Button>
                </Grid>
            </Grid>
        </React.Fragment>
    )
}

export default withStyles(styles, { withTheme: true })(SearchKits);