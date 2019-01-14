import * as React from 'react'
import Grid from '@material-ui/core/Grid'
import TextField from '@material-ui/core/TextField'
import Button from '@material-ui/core/Button'
import { withStyles } from '@material-ui/core/styles'


const styles = (theme: any) => ({
    root: {
        flexGrow: 1
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
    searchButtons: {
        width: '150px',
        marginRight: '20px',
        marginTop: '10px'

    },
    formControl:
    {
        margin: theme.spacing.unit,
        minWidth: 120,
        width: '100%'
    },

});

interface IProps {
    classes: any,
    searchOptions: any,
    onChange(event: any): void,
    onSearch(): void,
    showSearchProgress: boolean
}

interface IState {
    inputLinkGroupName: string,
    inputLinkGroupDesc: string,
    inputRegion: string,
    inputModifierName: string,
    inputModifierPLU: string, 
}

class SearchLinkGroups extends React.Component<IProps, IState> {
    constructor(props: any) {
        super(props);
        this.state = {
            inputLinkGroupName: "",
            inputLinkGroupDesc: "",
            inputRegion: "",
            inputModifierName: "",
            inputModifierPLU: "", 
        }
    }


    render() {

        return (
            <React.Fragment>
                <Grid container spacing={24} justify='center'>
                    <Grid item md={3}>
                        <TextField id="LinkGroupName"
                            label="Link Group Name"
                            fullWidth
                            name="LinkGroupName"
                            onChange={this.props.onChange}
                        />
                        <TextField id="LinkGroupDesc"
                            label="Link Group Desc"
                            fullWidth
                            name="LinkGroupDesc"
                            onChange={this.props.onChange}
                        />
                        <TextField id="Region"
                            label="Region"
                            fullWidth
                            name="Region"
                            onChange={this.props.onChange}
                        />
                    </Grid>
                    <Grid item md={3}>
                        <TextField id="ModifierName"
                            label="Modifier Name"
                            fullWidth
                            name="ModifierName"
                            onChange={this.props.onChange}
                        />
                        <TextField id="ModifierPLU"
                            label="Modifier PLU"
                            fullWidth
                            name="ModifierPLU"
                            onChange={this.props.onChange}
                        />
                        <Button
                            variant="contained"
                            color="primary"
                            className={this.props.classes.searchButtons}
                            onClick={() => this.props.onSearch()}
                        >
                            Search
                            </Button>
                        <Button
                            variant="contained"
                            color="secondary"
                            className={this.props.classes.searchButtons}
                        >
                            Add New
                            </Button>
                    </Grid>
                </Grid>
                <br/>
            </React.Fragment>
        )
    }
}

export default withStyles(styles)(SearchLinkGroups);