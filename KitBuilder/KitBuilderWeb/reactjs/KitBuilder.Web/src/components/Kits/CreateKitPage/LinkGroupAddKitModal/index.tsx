import * as React from "react";
import LinkGroupKitAddModalSearch from "./LinkGroupKitAddModalSearch";
import KitItemAddModalDisplay from "./KitItemAddModalDisplay";
import { Grid, Button } from "@material-ui/core";
import Dialog from "@material-ui/core/Dialog";
import DialogTitle from "@material-ui/core/DialogTitle";
import DialogContent from "@material-ui/core/DialogContent";
import DialogActions from "@material-ui/core/DialogActions";
import { KbApiMethod } from 'src/components/helpers/kbapi';
import { LinkGroup } from 'src/types/LinkGroup';
const hStyle = { color: "red" };
const successStyle = { color: "blue" };
const ModalStyle = {
  margin: "auto",
  marginTop: "10px",
  overlay: { zIndex: 10 }
};

interface ILinkGroupKitAddModalProps {
  onAddToKit: any;
  onCancel: any;
  isOpen: boolean;
  kitLinkGroup: Array<LinkGroup>
}

interface ILinkGroupKitAddModalState {
  searchResults: Array<LinkGroup>;
  queuedLinkGroups: Array<LinkGroup>;
  error: string;
  message: string;
}

class LinkgroupKitAddModal extends React.PureComponent<
  ILinkGroupKitAddModalProps,
  ILinkGroupKitAddModalState
> {
  constructor(props: ILinkGroupKitAddModalProps) {
    super(props);
    this.state = {
      searchResults: [],
      queuedLinkGroups: [],
      error: "",
      message: ""
    };
  }

  handleSearch = (name: string, plu: string) => {
    let searchBy, searchValue;
    if(name) {
      searchBy = "LinkGroupName";
      searchValue = name;
    } else if (plu) {
      searchBy = "ModifierPlu";
      searchValue = plu;
    } else {
      return;
    }

    const url = KbApiMethod('LinkGroupsSearch');
    const queryString = `?${searchBy}=${searchValue}`

    fetch(url + queryString)
    .then(result => result.json())
    .then(result => result.filter((lg: LinkGroup) => lg.linkGroupItemDto.length > 0))
    .then(result => {
      this.setState({searchResults: result});
    });
  };

  handleQueue = (linkGroupsToAdd: Array<any>) => {
    const { queuedLinkGroups } = this.state;
    this.setState({
      queuedLinkGroups: [...queuedLinkGroups, ...linkGroupsToAdd]
    });
  };

  handleDeQueue = (linkGroupToRemove: LinkGroup) => {
    const { queuedLinkGroups } = this.state;
    const queuedToKeep = queuedLinkGroups.filter(
      group => group !== linkGroupToRemove
    );
    this.setState({ queuedLinkGroups: queuedToKeep });
  };

  handleCancel = () => {
    this.setState({ searchResults: [], queuedLinkGroups: [] });
    this.props.onCancel();
  };

  handleAdd = () => {
      this.props.onAddToKit(this.state.queuedLinkGroups);
      this.setState({ searchResults: [], queuedLinkGroups: [] });
  }

  render() {
    return (
      <React.Fragment>
        <Dialog fullWidth={true} maxWidth="lg" open={this.props.isOpen}>
          <Grid container justify="center">
            <DialogTitle id="form-dialog-title">
              Add Link Group
            </DialogTitle>
          </Grid>
          <DialogContent>
            <Grid container justify="center">
              <Grid container justify="center">
                <div className="error-message">
                  <span style={hStyle}> {this.state.error}</span>
                </div>
              </Grid>
              <Grid container justify="center">
                <div className="Success-message">
                  <span style={successStyle}> {this.state.message}</span>
                </div>
              </Grid>
            </Grid>
            <Grid container justify="center" style={ModalStyle} spacing={16}>
              <Grid item md={12}>
                <LinkGroupKitAddModalSearch onSubmit={this.handleSearch} />
              </Grid>
                <Grid item md={12}>
                  <KitItemAddModalDisplay
                    kitLinkGroup={this.props.kitLinkGroup}
                    searchResults={this.state.searchResults}
                    onQueue={this.handleQueue}
                    onDeQueue={this.handleDeQueue}
                    queuedLinkGroups={this.state.queuedLinkGroups}
                  />
                </Grid>
                <Grid container justify="flex-end">
                <DialogActions>
                  
            <Button onClick={this.handleCancel} color="primary" variant="outlined">
              Cancel
            </Button>
            <Button onClick={this.handleAdd} color="primary" variant="contained">
              Add To Kit
            </Button>
          </DialogActions>
          </Grid>
            </Grid>
          </DialogContent>
          
        </Dialog>
      </React.Fragment>
    );
  }
}

export default LinkgroupKitAddModal;
