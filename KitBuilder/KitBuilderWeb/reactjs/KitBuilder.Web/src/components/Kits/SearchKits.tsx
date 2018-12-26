import * as React from 'react'
import { Grid, TextField, Button } from '@material-ui/core';
import { withStyles } from '@material-ui/core/styles';

const styles = (theme: any) => ({
    root: {
        marginTop: 20
    },
    label: {
        fontSize: 20,
        textAlign: "right" as 'right',
        marginBottom: 0 + ' !important',
        paddingRight: 10 + 'px'
    },
    button: {
        margin: theme.spacing.unit,
    },
    formControl:
    {
        margin: theme.spacing.unit,
        minWidth: 120,
        width: '100%'
    }
});

function SearchKits(props: any) {

    return (
        <React.Fragment>
            <Grid container justify="center" alignItems="center" className={props.classes.root}>
                <Grid item md={3} className={props.classes.label}>
                    <span>Main Item Name: </span>
                </Grid>
                <Grid item md={3}>
                    <TextField variant="outlined" className={props.classes.textField} onChange={props.MainItemName} value={props.MainItemValue}></TextField>
                </Grid>
                <Grid item md={2}>
                    <Grid container justify="flex-start" className={props.classes.label}>
                        <span>  Main Item Scancode:</span>
                    </Grid>
                </Grid>
                <Grid item md={4}>
                    <Grid container justify="flex-start">
                        <TextField variant="outlined" className={props.classes.textField} onChange={props.MainItemScanCode} value={props.ScanCodeValue}></TextField>
                    </Grid>
                </Grid>
            </Grid>

            <Grid container justify="center" alignItems="center" className={props.classes.root}>
                <Grid item md={3} className={props.classes.label}>
                    <span>Link Group Name: </span>
                </Grid>
                <Grid item md={3}>
                    <TextField variant="outlined" className={props.classes.textField} onChange={props.LinkGroupName} value={props.LinkGroupValue}></TextField>
                </Grid>

                <Grid md={1} container justify="flex-start">
                    <Button variant="contained" color="default" className={props.classes.button} onClick={() => props.onSearch()} >
                        Search
                    </Button>
                </Grid>
                <Grid md={5} container justify="flex-start">
                    <Button variant="contained" color="default" className={props.classes.button} onClick={() => props.clear()} >
                        Clear
                    </Button>
                </Grid>
            </Grid>
        </React.Fragment>
    )
}

export default withStyles(styles, { withTheme: true })(SearchKits);