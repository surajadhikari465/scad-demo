import React, { useContext } from 'react';
import { AppContext } from '../store';
import { Grid } from 'semantic-ui-react';

const CurrentLocation: React.FC = () => {
    const {state} = useContext(AppContext);
    const {store, region, subteam} = state;

    return (
        <Grid>
            <Grid.Row color="grey">
                <Grid.Column style={{marginLeft: '5px'}} floated='left' width={4}>
                    Region: {region}
                </Grid.Column>
                <Grid.Column style={{marginRight: '5px', textAlign: 'right'}} floated='right' width={11}>
                    Store/Subteam: {store} {store && subteam ? "/" : ""} {subteam ? subteam.subteamName : ""}
                </Grid.Column>
            </Grid.Row>
        </Grid>
    )
}

export default CurrentLocation;