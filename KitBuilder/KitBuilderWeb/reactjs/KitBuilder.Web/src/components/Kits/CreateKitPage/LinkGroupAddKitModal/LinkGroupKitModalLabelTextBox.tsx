import * as React from 'react';
import { Grid, TextField } from '@material-ui/core';
import { withStyles } from '@material-ui/core/styles';
const styles1 = (theme: any) => ({
    marginBottom: {
        marginBottom: 10+ 'px',
        zIndex:1000
    }  ,
    marginTop: {
        marginTop:10+ 'px',
    }  
});
function LinkGroupKitModalLabelTextBox(props: any) {
    return (
        <React.Fragment>
            <Grid container justify="flex-start" className={props.classes.marginBottom} >
                <Grid item md={4} className={props.classes.marginTop}>
                    <span >{props.labelValue} </span>
                </Grid>
                <Grid item md={5}>
                    <TextField id={props.name}
                        label={props.labelName}
                        variant="outlined"
                        fullWidth
                        name={props.name}
                        onChange={props.onChange}
                        value={props.value}
                    />
                </Grid>
            </Grid>

        </React.Fragment>
    )
}
export default withStyles(styles1, { withTheme: true })(LinkGroupKitModalLabelTextBox);
