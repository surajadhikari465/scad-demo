import * as React from 'react'
import { withStyles } from '@material-ui/core/styles';

const styles = (theme: any) => ({
    root: {
        marginTop: 20
    },
    label: {
        fontSize: 20,
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

function AssignKitProperties(props: any) {

    return (
        <React.Fragment>
            <div>
                <span> Assign Properties</span>
            </div>
        </React.Fragment>
    )
}

export default withStyles(styles, { withTheme: true })(AssignKitProperties);