import * as React from 'react'
import Grid from '@material-ui/core/Grid';
import Button from '@material-ui/core/Button';
import './style.css';

export default function KitFooter(props: any) {
    return <React.Fragment>
         <Grid className="kit-footer-root" container justify="flex-end">
                <Grid item xs={12} md={4}>
                        <Button className='kit-footer-button' variant="outlined" color="primary" onClick={() => props.createKit()} >
                            Create New Kit
                </Button>
                    </Grid>
                </Grid>
    </React.Fragment>
}