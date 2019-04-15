import * as React from "react";
import Grid from "@material-ui/core/Grid";
import ReactTable from "react-table";
import InstructionListPicker from "./InstructionListPicker";
import TextField from "@material-ui/core/TextField";
import Button from "@material-ui/core/Button";
import { Delete } from "@material-ui/icons";
import CopyLinkGroupButton from "./CopyLinkGroupButton";
import * as LinkGroupFunctions from "./LinkGroupFunctions";
import { DialogContent } from '@material-ui/core';
import AddItemToLinkGroupDialog from './AddItemToLinkGroupDialog';
import { Item, LinkGroup, LinkGroupItem } from 'src/types/LinkGroup';
import { KbApiMethod } from '../helpers/kbapi';
import Axios, { AxiosError } from 'axios';
import withSnackbar from '../PageStyle/withSnackbar';

interface IState {
  InstructionsList: any;
  data: any;
  LinkGroupItems: Array<any>;
  LinkGroupName: string;
  LinkGroupDesc: string;
  showAddModifiers: boolean;
  editedModifiers: LinkGroupItem[],
  errors: {
    name?: string,
    desciption?: string,
  }
}

interface IProps {
  data: any;
  handleCancelClick(): void;
  showAlert(message: string, type?: string): void;
  clearSearchResults(): void;
}

class EditLinkGroup extends React.Component<IProps, IState> {
  constructor(props: any) {
    super(props);
    this.loadCookingInstructionsList = this.loadCookingInstructionsList.bind(
      this
    );

    this.state = {
      InstructionsList: [],
      data: {},
      LinkGroupItems: [],
      LinkGroupName: "",
      LinkGroupDesc: "",
      showAddModifiers: false,
      editedModifiers: [],
      errors: {},
    };
  }

  componentDidMount() {
    this.getLinkGroupInfo();
    this.loadCookingInstructionsList();
  }

  hasSameName = () =>{
    const { LinkGroupName, data } = this.state;
    return LinkGroupFunctions.PerformLinkGroupSearch(LinkGroupName, "", "", "", "")
      .then((result: LinkGroup[]) => {
        const hasSameName = result.some(
          linkGroup =>
            linkGroup.groupName.toLowerCase().trim() ===
            LinkGroupName.toLowerCase().trim() && linkGroup.linkGroupId !== data.linkGroupId
        );
        return hasSameName;
      });
  }

  handleSaveLinkGroup = async () => {
    const linkGroup = {... this.state.data };
    const { LinkGroupDesc, LinkGroupName } = this.state;
      if(LinkGroupName.trim().length === 0) {
        this.setState({errors: { name: "Name cannot be blank." }});
        return;
      }
      if(LinkGroupDesc.trim().length === 0) {
        this.setState({errors: { desciption: "Description cannot be blank." }});
        return;
      }
      const hasSameName = await this.hasSameName();
      if(hasSameName) {
        this.setState({errors: { name: "Another link group already has this name." }});
        return;
      }
      this.setState({errors: {}});
      linkGroup.groupName = this.state.LinkGroupName;
      linkGroup.groupDescription = this.state.LinkGroupDesc;

    const url = `${KbApiMethod("LinkGroups")}/${linkGroup.linkGroupId}`;
    
    Axios.put(url, linkGroup).then(() => {
      //skip modifiers if none have been edited.
      if(this.state.editedModifiers.length === 0) return;
      
      //now try and save any edited modifiers
      const url = `${KbApiMethod("LinkGroups")}/${this.props.data.linkGroupId}/LinkGroupItems`;
      Axios.put(url, this.state.editedModifiers);
    })
    .then(() => {
      this.props.showAlert("Update Successful", "success");
      this.props.clearSearchResults();

    })
    .catch((error: AxiosError) => {
      const message = error.response ? error.response.data : error.message;
      this.props.showAlert(message, "error");
    })
  }

  getLinkGroupInfo = () => {
      const url = `${KbApiMethod("LinkGroups")}/${this.props.data.linkGroupId}/true`;
      Axios.get(url)
      .then((response: any) => response.data).then((linkGroup: LinkGroup) => {
        this.setState(    {
            data: linkGroup,
            LinkGroupItems: linkGroup.linkGroupItemDto,
            LinkGroupName: linkGroup.groupName,
            LinkGroupDesc: linkGroup.groupDescription
          });
      })
  }

  handleModifierEdit = (item: LinkGroupItem) => {
    const { editedModifiers } = this.state;
    let itemToAdd;
    const existingItem = editedModifiers.find(i => i.linkGroupItemId === item.linkGroupItemId);
    if(existingItem) {
      existingItem.instructionListId = item.instructionListId;
      itemToAdd = { ...existingItem };
    } else {
      itemToAdd = item;
    }
    const restOfItems = editedModifiers.filter(i => i.linkGroupItemId !== item.linkGroupItemId);
    this.setState({ editedModifiers: [...restOfItems, itemToAdd]});
  }

  removeLinkGroupItem = (itemIdToDelete: number) => {
    const { LinkGroupItems } = this.state;
    const allItemsButDeletedItem = LinkGroupItems.filter((item) => item.linkGroupItemId != itemIdToDelete);

    const url = `${KbApiMethod("LinkGroups")}/${this.props.data.linkGroupId}/LinkGroupItem/${itemIdToDelete}`;
    Axios.delete(url).then(() => {
      this.setState({ LinkGroupItems: allItemsButDeletedItem });
      this.props.showAlert("Modifier deleted successfully.", "success");
    })
    .catch((error) => {
      const message = error.response ? error.response.data : error.message;
      this.props.showAlert(message, "error");
    })
  }

  loadCookingInstructionsList() {
    LinkGroupFunctions.LoadCookingInstructions().then(result => {
      this.setState({ InstructionsList: result });
    });
  }

  handleChange = (name: string, event: any) => {
    this.setState({ ...this.state, [name]: event.target.value });
  }

  handleAddModifiers = (items: Item[]) => {
    const remodeled = items.map((item: Item) => ({
        itemId: item.itemId,
        linkGroupId: this.props.data.linkGroupId,
    }));

    const url = `${KbApiMethod("LinkGroups")}/${this.props.data.linkGroupId}/LinkGroupItems`;
    Axios.post(url, remodeled).then(() => {
        this.getLinkGroupInfo();
        this.setState({showAddModifiers: false});
        this.props.showAlert("Modifier added succesfully.", "success");
    }).catch((error) => {
      this.props.showAlert(error.message, "error");
    })
  }

  render() {
    return (<DialogContent><Grid container justify="space-between" spacing = {16}>
          <Grid item md={3}>
            <TextField
              id="LinkGroupName"
              label="Link Group Name"
              name="LinkGroupName"
              error={!!this.state.errors.name}
              helperText = {this.state.errors.name}
              value={this.state.LinkGroupName}
              onChange={e => this.handleChange("LinkGroupName", e)}
              fullWidth
              variant="outlined"
              InputLabelProps={{ shrink: true }}
            />
          </Grid>
          <Grid item md={3}>
            <TextField
              id="LinkGroupDesc"
              label="Link Group Description"
              name="LinkGroupDesc"
              error={!!this.state.errors.desciption}
              helperText = {this.state.errors.desciption}
              value={this.state.LinkGroupDesc}
              onChange={e => this.handleChange("LinkGroupDesc", e)}
              fullWidth
              variant="outlined"
              InputLabelProps={{ shrink: true }}
            />
          </Grid>

          <Grid item md={3}>
              <CopyLinkGroupButton
                linkGroupId={this.state.data.linkGroupId}
              />
            </Grid>

          <Grid item md={12}>
            <ReactTable
            noDataText="No Modifiers"
              className="-highlight -striped"
              defaultPageSize={10}
              data={this.state.LinkGroupItems}
              columns={[
                {
                  Header: "PLU",
                  accessor: "item.scanCode",
                  show: true,
                  style: { textAlign: "center" },
                },
                {
                  Header: "Modifier",
                  accessor: "item.productDesc",
                  show: true,
                  style: { textAlign: "center" },
                },
                {
                  Header: "Brand",
                  accessor: "item.brandName",
                  show: true,
                  style: { textAlign: "center" },
                },
                {
                  Header: "InstructionListId",
                  accessor: "instructionListId",
                  show: false,
                  style: { textAlign: "center" },
                },
                {
                  Header: "Cooking Instructions",
                  accessor: "instructionListId",
                  show: true,
                  style: { textAlign: "center" },
                  Cell: cellInfo => (
                    <InstructionListPicker
                      SelectOptions={this.state.InstructionsList}
                      SelectedOption={cellInfo.original.instructionListId || -1}
                      LinkGroupId={cellInfo.original.linkGroupId}
                      handleSelectionChanged={e => {
                        const data = [...this.state.LinkGroupItems];
                        const item = data[cellInfo.index]
                        item[cellInfo.column.id!] =
                          e.target.value;
                        this.setState({ ...this.state, LinkGroupItems: data });
                        this.handleModifierEdit(item);
                      }}
                    />
                  )
                },
                {
                  style: { textAlign: "center" },
                  show: true,
                  Cell: row => (
                    <Button color="secondary" onClick = {() => this.removeLinkGroupItem(row.original.linkGroupItemId)}>
                      <Delete/>
                    </Button>
                  ),
                  Footer: row => (
                    <Button
                      variant="text"
                      color="primary"
                      onClick={() =>
                        this.setState({
                          showAddModifiers: !this.state.showAddModifiers
                        })
                      }
                    >
                      ADD MODIFIER
                    </Button>
                  )
                }
              ]}
            />
          </Grid>
          <Grid item xs = {12}>
          <Grid container justify="space-between">
            <Grid item md={3}>
              <Button
                variant="outlined"
                color="default"
                onClick={this.props.handleCancelClick}
                fullWidth
              >
                Cancel
              </Button>
            </Grid>
            
            <Grid item md={3}>
              <Button variant="contained" color="primary" onClick={this.handleSaveLinkGroup} fullWidth>
                Save
              </Button>
            </Grid>
          </Grid>
          </Grid>
        </Grid>
        <AddItemToLinkGroupDialog 
        isOpen = {this.state.showAddModifiers} 
        linkGroupItems = {this.state.LinkGroupItems}
        handleAddModifiers={this.handleAddModifiers}
        onClose = {() => this.setState({showAddModifiers: false})}/>
        </DialogContent>)}
}

export default withSnackbar(EditLinkGroup);
