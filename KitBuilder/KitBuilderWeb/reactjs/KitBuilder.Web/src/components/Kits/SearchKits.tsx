import * as React from 'react'
import { Grid, TextField, Button } from '@material-ui/core';
import { withStyles } from '@material-ui/core/styles';

const styles = (theme: any) => ({
    root: {
        marginTop: 20
    },
    label: {
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
                <Grid item md={2} className={props.classes.label}>
                    <span>Main Item Name: </span>
                </Grid>
                <Grid item md={2}>
                    <TextField variant="outlined" className={props.classes.textField} onChange={props.MainItemName} value={props.MainItemValue}></TextField>
                </Grid>
                <Grid item md={2}>
                    <Grid container justify="center" className={props.classes.label}>
                        <span>  Main Item Scancode:</span>
                    </Grid>
                </Grid>
                <Grid item md={6}>
                    <Grid container justify="flex-start">
                        <TextField variant="outlined" className={props.classes.textField} onChange={props.MainItemScanCode} value={props.ScanCodeValue}></TextField>
                    </Grid>
                </Grid>
            </Grid>

            <Grid container justify="center" alignItems="center" className={props.classes.root}>
                <Grid item md={2} className={props.classes.label}>
                    <span>Link Group Name: </span>
                </Grid>
                <Grid item md={2}>
                    <TextField variant="outlined" className={props.classes.textField} onChange={props.LinkGroupName} value={props.LinkGroupValue}></TextField>
                </Grid>

                <Grid item md={2} className={props.classes.label}>
                    <span>Kit Description: </span>
                </Grid>
                <Grid item md={2}>
                    <TextField variant="outlined" className={props.classes.textField} onChange={props.KitDescription} value={props.KitDescriptionValue}></TextField>
                </Grid>

                <Grid md={2} container justify="flex-end">
                    <Button variant="contained" color="primary" className={props.classes.button} onClick={() => props.onSearch()} >
                        Search
                    </Button>
                </Grid>
                <Grid md={2} container justify="flex-start">
                    <Button variant="contained" color="primary" className={props.classes.button} onClick={() => props.clear()} >
                        Clear
                    </Button>
                </Grid>
            </Grid>
        </React.Fragment>
    )
}

export default withStyles(styles, { withTheme: true })(SearchKits);