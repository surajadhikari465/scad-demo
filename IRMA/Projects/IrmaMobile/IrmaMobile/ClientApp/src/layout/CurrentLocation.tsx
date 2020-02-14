import React, { useContext } from 'react';
import { AppContext } from '../store';
import { Grid } from 'semantic-ui-react';
import './styles.scss';

const CurrentLocation: React.FC = () => {
    const {state} = useContext(AppContext);
    const {store, region, subteam, shrinkType, showShrinkHeader} = state;
    return (
        <Grid style={{marginTop: '-0.2rem'}}>
            <Grid.Row color="grey">
                <Grid.Column className={region.length > 3 ? 'smalltext':''} style={{marginLeft: '5px'}} floated='left' width={4}>
                    Region: {region}
                </Grid.Column>
                <Grid.Column className={subteam.subteamName.length + store.length + 3 > 20 ? 'smalltext':''} style={{marginRight: '5px', textAlign: 'right'}} floated='right' width={11}>
                    Store/Subteam: {store} {store && subteam ? "/" : ""} {subteam ? subteam.subteamName : ""}
                </Grid.Column>
            </Grid.Row>
            { shrinkType.shrinkType !== '' && showShrinkHeader  &&
                <Grid.Row style={{backgroundColor: 'lightgrey'}}>
                    <Grid.Column className={shrinkType.shrinkSubTypeMember.length > 36 ? 'smalltext':''} style={{marginLeft: '5px'}} floated='left' width={15}>
                        IRMA Shrink: {shrinkType.shrinkSubTypeMember}
                    </Grid.Column>
                </Grid.Row>
            }
        </Grid>
    )
}

export default CurrentLocation;