import * as React from 'react';
import Grid from '@material-ui/core/Grid';
import SearchLinkGroups from './SearchLinkGroups';
import DisplayLinkGroups from './DisplayLinkGroups';
import axios from 'axios'
import { KbApiMethod } from '../helpers/kbapi';
import { withStyles } from '@material-ui/core/styles'
import Card from '@material-ui/core/Card';
import CardHeader from '@material-ui/core/CardHeader'
import CardContent from '@material-ui/core/CardContent'

const errorStyle = { color: 'red' };
const sucesssStyle = { color: 'blue' };



class SearchOptions {
    LinkGroupName: string;
    LinkGroupDesc: string;
    ModifierName: string;
    ModifierPLU: string;
    Region: string;

    constructor() {
    }
}

interface ILinkGroupsPageState {
    error: any,
    message: any,
    searchOptions: SearchOptions,
    linkGroupResults: [], 
    showSearchProgress: boolean
}
interface ILinkGroupsPageProps {
}

const styles = (theme: any) => ({
    root: {
        flexGrow: 1
    },
    hideProgress: {
        display: "none"
    }

});


export class LinkGroupsPage extends React.Component<ILinkGroupsPageProps, ILinkGroupsPageState>
{
    constructor(props: any) {
        super(props);

        this.state = {
            error: "",
            message: "",
            searchOptions: {
                LinkGroupName: "",
                LinkGroupDesc: "",
                ModifierName: "",
                ModifierPLU: "",
                Region: ""
            },
            linkGroupResults: [],
            showSearchProgress: false
        }
        this.handleSearchOptionsChange = this.handleSearchOptionsChange.bind(this);
        this.onSearch = this.onSearch.bind(this);
    }

    componentDidMount() {

    }

    handleSearchOptionsChange(event: any) {
        var temp = { ...this.state.searchOptions };
        temp[event.target.name] = event.target.value;
        this.setState({ ...this.state, searchOptions: temp });
    }

    onSearch() {
        
        this.setState({...this.state, showSearchProgress: true});
        var searchOptions = this.state.searchOptions;

        axios.get(KbApiMethod("LinkGroupsSearch"), {
            params: {
                LinkGroupName: searchOptions.LinkGroupName,
                LinkGroupDesc: searchOptions.LinkGroupDesc,
                ModifierName: searchOptions.ModifierName,
                ModifierPLU: searchOptions.ModifierPLU
            }
        })
            .then(res => {
                this.setState({ ...this.state, linkGroupResults: res.data })
                this.setState({...this.state, showSearchProgress: false});
            }).catch(error => {
                this.setState({...this.state, showSearchProgress: false});
                this.setState({...this.state, error: error});
            });
            

    }

    render() {
        return (
            <React.Fragment>
                <form onSubmit={() => this.onSearch()} onKeyDown={(d) => {if (d.keyCode === 13) {this.onSearch()} }}>
               
                <Grid container md={12} justify='center'>
                    <Card>
                        <CardHeader title="Search" />
                        <CardContent>
                        <Grid container justify="center">
                    <Grid container md={12} justify="center">
                        <div className="error-message" >
                            <span style={errorStyle}> {this.state.error}</span>
                        </div>
                    </Grid>
                    <Grid container md={12} justify="center">
                        <div className="Success-message" >
                            <span style={sucesssStyle}> {this.state.message}</span>
                        </div>
                    </Grid>
                    <Grid container md={12} justify="center">
                        <Grid item md={10}>
                            <SearchLinkGroups
                                searchOptions={this.state.searchOptions}
                                onChange={this.handleSearchOptionsChange}
                                showSearchProgress={this.state.showSearchProgress}
                                onSearch={this.onSearch} 
                            />
                        </Grid>
                    </Grid>
                    <Grid container md={10} justify="center">
                        <Grid item md={10}>
                            <DisplayLinkGroups DisplayData={this.state.linkGroupResults} />
                        </Grid>
                    </Grid>
                </Grid>
                        </CardContent>
                    </Card>
                </Grid>
                </form>
                <br />
            </React.Fragment>
        )
    }
}


export default withStyles(styles)(LinkGroupsPage);