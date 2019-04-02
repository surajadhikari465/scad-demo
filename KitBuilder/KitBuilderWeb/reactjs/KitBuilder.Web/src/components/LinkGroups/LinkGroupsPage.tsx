import * as React from "react";
import Grid from "@material-ui/core/Grid";
import { withStyles } from "@material-ui/core/styles";
import SearchLinkGroups from "./SearchLinkGroups";
import DisplayLinkGroups from "./DisplayLinkGroups";
import axios from "axios";
import { KbApiMethod } from "../helpers/kbapi";
import { AddNewLinkGroupModal } from "./AddNewLinkGroupModal";
import { PerformLinkGroupSearch } from "./LinkGroupFunctions";
import StyledPanel from "../PageStyle/StyledPanel";
import PageTitle from "../PageTitle";
import { Button } from "@material-ui/core";
import EditLinkGroupDialog from "./EditLinkGroupDialog";

class SearchOptions {
  LinkGroupName: string;
  LinkGroupDesc: string;
  ModifierName: string;
  ModifierPLU: string;
  Region: string;

  constructor() {}
}

interface ILinkGroupsPageState {
  error: any;
  message: any;
  searchOptions: SearchOptions;
  linkGroupResults: any;
  showSearchProgress: boolean;
  showEditScreen: boolean;
  showSearchScreen: boolean;
  showAddNew: boolean;
  selectedLinkGroup: any;
  regions: string[];
}
interface ILinkGroupsPageProps {
  styles: any;
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

export class LinkGroupsPage extends React.Component<
  ILinkGroupsPageProps,
  ILinkGroupsPageState
> {
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
      selectedLinkGroup: [],
      regions: []
    };
  }

  handleSearchOptionsChange = (event: any) => {
    var temp = { ...this.state.searchOptions };
    temp[event.target.name] = event.target.value;
    this.setState({ ...this.state, searchOptions: temp });
  }

  componentWillMount() {
    this.loadRegions()
      .then((d: any) => {
        this.setState({ ...this.state, regions: d });
      })
      .catch(e => {
        console.log("could not load region information");
        console.log(e);
      });
  }

  loadRegions = () => {
    return new Promise((resolve, reject) => {
      //pass linkgroupid and true to return child objects.
      axios
        .get(KbApiMethod("LocalesByType"), {
          params: {
            localeTypeDesc: "Region"
          }
        })
        .then(res => {
          resolve(res.data);
        })
        .catch(error => {
          reject(error);
        });
    });
  }

  onSearch = (linkGroupName: string, linkGroupDesc: string, modifierName: string, modifierPlu: string, region: string) => {
    this.setState({ ...this.state, showSearchProgress: true });

    PerformLinkGroupSearch(
      linkGroupName,
      linkGroupDesc,
      modifierName,
      modifierPlu,
      region
    )
      .then(result => {
        this.setState({ ...this.state, linkGroupResults: result });
      })
      .catch(error => {
        this.setState({ ...this.state, error: error });
      });
  }

  onEdit = (linkGroupId: number) => {
    this.loadLinkGroup(linkGroupId)
      .then(result => {
        this.setState({
          selectedLinkGroup: result,
          showEditScreen: true,
          showSearchScreen: false
        });
      })
      .catch(error => {
        alert(error);
      });
  }

  loadLinkGroup = (linkGroupId: number) => {
    return new Promise((resolve, reject) => {
      //pass linkgroupid and true to return child objects.
      axios
        .get(KbApiMethod("LinkGroups") + "/" + linkGroupId + "/true", {})
        .then(res => {
          resolve(res.data);
        })
        .catch(error => {
          reject(error);
        });
    });
  }

  render() {
    return (
      <>
        <StyledPanel header={<PageTitle icon="link">Link Groups</PageTitle>}>
          <br />
            <Grid container>
              <Grid item md={12}>
                <SearchLinkGroups
                onAddNew = {() => {}}
                  regions={this.state.regions}
                  searchOptions={this.state.searchOptions}
                  onChange={this.handleSearchOptionsChange}
                  showSearchProgress={this.state.showSearchProgress}
                  onSearch={this.onSearch}
                  onRegionsChanged={(regions: string[]) => {
                    let updatedSearchOptions = this.state.searchOptions;
                    updatedSearchOptions.Region = regions.join(",");
                    this.setState({
                      ...this.state,
                      searchOptions: updatedSearchOptions
                    });
                  }}
                />
              </Grid>

              <Grid item md={12}>
                <DisplayLinkGroups
                  DisplayData={this.state.linkGroupResults}
                  onSearch={this.onSearch}
                  onEdit={this.onEdit}
                />
              </Grid>
            </Grid>

          <AddNewLinkGroupModal
            onCreated={this.onEdit}
            open={this.state.showAddNew}
            closeModal={() => {
              this.setState({ showAddNew: false });
            }}
          />
        </StyledPanel>
        <Grid container justify="center" className="mt-3">
          <Grid item md={10}>
            <Grid container justify="flex-end">
              <Grid item md={3}>
                <Button
                  style={{ width: "100%" }}
                  variant="outlined"
                  color="primary"
                  onClick={() => {
                    this.setState({
                      ...this.state,
                      showAddNew: !this.state.showAddNew
                    });
                  }}
                >
                  Create Link Group
                </Button>
              </Grid>
            </Grid>
          </Grid>
        </Grid>

        <EditLinkGroupDialog
          isOpen={this.state.showEditScreen}
          selectedLinkGroup={this.state.selectedLinkGroup}
          onClose={() => this.setState({ showEditScreen: false })}
        />
      </>
    );
  }
}

export default withStyles(styles)(LinkGroupsPage);
