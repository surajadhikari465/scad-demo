import * as React from 'react'
import Grid from '@material-ui/core/Grid';
import Button from '@material-ui/core/Button';
import FormControl from '@material-ui/core/FormControl';
import MenuItem from '@material-ui/core/MenuItem';
import TextField from '@material-ui/core/TextField';
import { withStyles } from '@material-ui/core/styles';
import './style.css';

const styles = (theme: any) => ({
    root: {
        marginTop: 24
    },
    label: {
        textAlign: "right" as 'right',
        marginBottom: 0 + ' !important',
        paddingRight: 10 + 'px'
    },
    button: {
        width: '100%',
    },
    editButton: {
        width: '100%',
        marginLeft: '16px'
    },
    formControl:
    {
        width: '100%'
    }
});

const getAllMenuItems = (options: Array<any>) => 
    options.map((item: any) => 
        <MenuItem key={item.InstructionListId} value={item.InstructionListId}> {item.Name} </MenuItem>
    );

function SearchInstructionLists(props: any) {
    const { options, value } = props;

    return (<div className="search-container">
            <Grid container justify="space-between" alignItems="center" spacing={16}>
                <Grid item xs={12} sm={9} md={5}>
                <Grid container spacing={16}>
                        <Grid item xs={12} sm={8}>
                            <FormControl className={props.classes.formControl}>
                                <TextField
                                    select
                                    onChange={props.onChange}
                                    variant='outlined'
                                    label='Select Instruction'
                                    className = 'search-textfield'
                                    InputLabelProps={{ shrink: true }}
                                    value={value}>
                                    {getAllMenuItems(options)}
                                </TextField>
                            </FormControl>
                        </Grid>
                        <Grid item xs={12} sm={4}>
                            <Button variant="outlined" disabled={!value} className = "full-width" color="primary" onClick={props.onEdit}>
                                Edit
                            </Button>
                        </Grid>
                    </Grid>
                </Grid>
                <Grid item xs={12} sm={9} md={3} lg={3}>
                    <Button variant="outlined" color="primary" className={props.classes.button} onClick={props.onAddNewList} >
                        Create Instruction
                    </Button>
                </Grid>
            </Grid>
        </div>)
}

export default withStyles(styles, { withTheme: true })(SearchInstructionLists);