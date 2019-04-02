import * as React from 'react'
import Grid from '@material-ui/core/Grid';
import './style.css';

const StyledPanel = (props: any) => {
    return <Grid container justify="center">
    <Grid item xs={12} md={10}>
    {props.header}
    </Grid>
                <Grid item xs={12} md={10}  className = "styled-panel"  style={{padding: props.padding}}>
                    {props.children }
                </Grid>
            </Grid>
}
export default StyledPanel;