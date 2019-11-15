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

function SelectLocaleSearch(props: any) {

    const handlePressEnterToSearch = (e: React.KeyboardEvent) => {
        e = e || window.event;
        const ENTER = 13;
    
        if(e.keyCode == ENTER){
            props.onSearch();
        }
    }

    return (
        <React.Fragment>
            <Grid container justify="space-between" alignItems="center" className={props.classes.root + ' locale-search-container'} onKeyUp={handlePressEnterToSearch}>
                <Grid item xs={12} md={5}>
                    <TextField className='search-textfield' margin="dense" variant="outlined" label="Location Name" InputLabelProps={{
                        shrink: true,
                        FormLabelClasses: {
                            root: props.classes.labelRoot
                        }
                    }} onChange={props.LocaleName} value={props.LocaleNameValue}></TextField>
                </Grid>
                <Grid item xs={12} md={5}>
                    <TextField className='search-textfield' margin="dense" variant="outlined" label='Store Abbreviation' InputLabelProps={{
                        shrink: true,
                        FormLabelClasses: {
                            root: props.classes.labelRoot
                        }
                    }} onChange={props.StoreAbbreviation} value={props.StoreAbbreviationValue}></TextField>
                </Grid>

                <Grid item xs={12} md={5}>
                    <TextField className='search-textfield' margin="dense" variant="outlined" label='Store Business Unit ID' InputLabelProps={{
                        shrink: true,
                        FormLabelClasses: {
                            root: props.classes.labelRoot
                        }
                    }} onChange={props.BusinessUnitId} value={props.BusinessUnitIdValue}></TextField>
                </Grid>
                
                <Grid xs={12} md={5} item>
                    <Button variant="contained" color="primary" className={props.classes.button} onClick={() => props.onSearch()} >
                        Search
                    </Button>
                </Grid>
            </Grid>
        </React.Fragment>
    )
}

export default withStyles(styles, { withTheme: true })(SelectLocaleSearch);