import * as React from 'react'
import Grid from '@material-ui/core/Grid';
import Button from '@material-ui/core/Button';
import FormControl from '@material-ui/core/FormControl';
import Select from '@material-ui/core/Select';
import MenuItem from '@material-ui/core/MenuItem';
import { withStyles } from '@material-ui/core/styles';
import Hidden from '@material-ui/core/Hidden';

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

function SearchInstructionLists(props: any) {
    const { options, value } = props;

    return (
        <React.Fragment>
            <Grid container justify="center" alignItems="center" className={props.classes.root}>
                <Hidden only={['md', 'lg', 'xl']}>
                    <Grid item xs >

                    </Grid>
                </Hidden>
                <Grid item xs={11} sm={2} md={3} className={props.classes.label}>
                    <span style={{ marginLeft: '20px', textAlign: 'center' }}> Instruction List Name: </span>
                </Grid>
                <Hidden only={['xs', 'md', 'lg', 'xl']}>
                    <Grid item xs >

                    </Grid>
                </Hidden>
                <Grid item xs={11} sm={9} md={3}>
                    <FormControl className={props.classes.formControl}>
                        <Select
                            onChange={props.onChange}
                            value={value}
                        >
                            {options.map((item: any) =>
                                <MenuItem key={item.InstructionListId} value={item.InstructionListId}> {item.Name} </MenuItem>

                            )}
                        </Select>
                    </FormControl>
                </Grid>
                <Hidden only={['md', 'lg', 'xl']}>
                    <Grid item xs >

                    </Grid>
                </Hidden>
                <Grid item xs={11} sm={11} md={5} style={{ marginLeft: '25px' }}>
                    <Grid container justify="flex-start">
                        <Button variant="contained" color="primary" className={props.classes.button} onClick={() => props.onSearch()} >
                            Search
                    </Button>
                    </Grid>
                </Grid>
                <Hidden only={['md', 'lg', 'xl']}>
                    <Grid item xs >

                    </Grid>
                </Hidden>
                <Grid item xs={11} sm={11} md={10}>
                    <Grid container justify="flex-start">
                        <Button variant="contained" color="primary" className={props.classes.button} onClick={() => props.onAddNewList()} >
                            Add New List
                    </Button>
                    </Grid>
                </Grid>
            </Grid>
        </React.Fragment>
    )
}

export default withStyles(styles, { withTheme: true })(SearchInstructionLists);