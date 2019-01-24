import * as React from 'react';
import Grid from '@material-ui/core/Grid';
import { withStyles } from '@material-ui/core/styles'
import SearchLinkGroups from './Components/SearchLinkGroups';
import DisplayLinkGroups from './Components/DisplayLinkGroups';
import axios from 'axios'
import { KbApiMethod } from '../helpers/kbapi';
import EditLinkGroup from './EditLinkGroup';
import { AddNewLinkGroupModal } from './AddNewLinkGroupModal';
import  {PerformLinkGroupSearch} from './LinkGroupFunctions'

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
    linkGroupResults: any,
    showSearchProgress: boolean,
    showEditScreen: boolean,
    showSearchScreen: boolean,
    showAddNew: boolean,
    selectedLinkGroup: any

}
interface ILinkGroupsPageProps {
    styles: any
}

const styles = () => ({
    root: {
        flexGrow: 1
    },
    hidden: {
        display: "none"
    },
    EditButtons: {
        width: "100%"
    },
    AddNewModal: {
        width: "50%"
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
            showSearchProgress: false,
            showEditScreen: false,
            showSearchScreen: true,
            showAddNew: false,
            selectedLinkGroup: []
        }
        this.handleSearchOptionsChange = this.handleSearchOptionsChange.bind(this);
        this.onSearch = this.onSearch.bind(this);
        this.loadLinkGroup = this.loadLinkGroup.bind(this);
        this.switchToEditMode = this.switchToEditMode.bind(this);
        this.switchToSearchMode = this.switchToSearchMode.bind(this);
    }

    handleSearchOptionsChange(event: any) {
        var temp = { ...this.state.searchOptions };
        temp[event.target.name] = event.target.value;
        this.setState({ ...this.state, searchOptions: temp });
    }


    onSearch() {

        this.setState({ ...this.state, showSearchProgress: true });
        var searchOptions = this.state.searchOptions;


        PerformLinkGroupSearch(searchOptions.LinkGroupName, searchOptions.LinkGroupDesc, searchOptions.ModifierName, searchOptions.ModifierPLU)
            .then(result => {
                this.setState({ ...this.state, linkGroupResults: result })
            }).catch(error => {
                this.setState({ ...this.state, error: error });
            });
    }

    switchToEditMode(linkGroupId: number) {
        this.loadLinkGroup(linkGroupId)
            .then(result => {
                this.setState({ selectedLinkGroup: result, showEditScreen: true, showSearchScreen: false })
            })
            .catch(error => {
                alert(error);
            });
    }

    switchToSearchMode() {
        this.setState({ selectedLinkGroup: [], showEditScreen: false, showSearchScreen: true })
    }

    loadLinkGroup(linkGroupId: number) {
        return new Promise((resolve, reject) => {
            //pass linkgroupid and true to return child objects.
            axios.get(KbApiMethod("LinkGroups") + "/" + linkGroupId + "/true", { 
            }).then(res => {
                resolve(res.data);
            }).catch(error => {
                reject(error);
            });
        })
    }

 

 


    render() {
        return (
            <React.Fragment>
                <br />
                <form onSubmit={() => this.onSearch()} onKeyDown={(d) => { if (d.keyCode === 13) { this.onSearch() } }}>

                    {!this.state.showSearchScreen ||
                        <Grid container spacing={24} justify="center">
                            <Grid item md={12} >
                                <div className="error-message" >
                                    <span style={errorStyle}> {this.state.error}</span>
                                </div>
                            </Grid>
                            <Grid item md={12}>
                                <div className="Success-message" >
                                    <span style={sucesssStyle}> {this.state.message}</span>
                                </div>
                            </Grid>

                            <Grid item md={12}>
                                <SearchLinkGroups
                                    searchOptions={this.state.searchOptions}
                                    onChange={this.handleSearchOptionsChange}
                                    showSearchProgress={this.state.showSearchProgress}
                                    onSearch={this.onSearch}
                                    onAddNew={() => {
                                        this.setState({ ...this.state, showAddNew: !this.state.showAddNew })
                                    }}
                                />

                            </Grid>

                            <Grid item md={10}>
                                <DisplayLinkGroups
                                    DisplayData={this.state.linkGroupResults}
                                    handleSearchRowClick={this.switchToEditMode} />
                            </Grid>

                        </Grid>
                    }
                    <br />
                    {!this.state.showEditScreen ||
                        <Grid container justify="center">
                            <Grid item md={10} >
                                <EditLinkGroup data={this.state.selectedLinkGroup}
                                    handleCancelClick={this.switchToSearchMode}
                                />
                            </Grid>

                        </Grid>
                    }

                </form>
                <br />

                <AddNewLinkGroupModal
                    open={this.state.showAddNew}
                    closeModal={() => { this.setState({ ...this.state, showAddNew: false }) }}
                    onCreated={this.switchToEditMode}
                />
            </React.Fragment>
        )
    }
}


export default withStyles(styles)(LinkGroupsPage);