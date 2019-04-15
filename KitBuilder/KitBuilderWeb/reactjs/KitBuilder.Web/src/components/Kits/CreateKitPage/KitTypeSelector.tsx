import * as React from 'react';
import { Grid, Radio } from '@material-ui/core';

export default function KitTypeSelector(props: any) {


    const handleKitTypeChange = (e: any) => {
        const type = parseInt(e.target.value);
        props.onKitTypeChange(type);
    }

    return <Grid container spacing ={16}>
        <Grid item><Radio checked={props.kitType === 1} value="1" color="primary" onChange={handleKitTypeChange}/>Simple Item</Grid>
        <Grid item><Radio checked={props.kitType === 3} value="3" color="primary" onChange={handleKitTypeChange}/>Customizable Item</Grid>
    </Grid>
}