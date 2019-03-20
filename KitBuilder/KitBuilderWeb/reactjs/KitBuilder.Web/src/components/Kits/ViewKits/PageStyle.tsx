import * as React from 'react'
import Grid from '@material-ui/core/Grid';
import './style.css';

export default function PageStyle(props: any) {
    return <Grid container justify="center">
                <Grid item xs={12} md={10}>
                    {props.children}
                </Grid>
            </Grid>
}