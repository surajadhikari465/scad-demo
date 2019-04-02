import * as React from "react";
import Grid from "@material-ui/core/Grid";
import ReactTable from "react-table";
import InstructionListPicker from "./InstructionListPicker";
import TextField from "@material-ui/core/TextField";
import Button from "@material-ui/core/Button";
import CopyLinkGroupButton from "./CopyLinkGroupButton";
import * as LinkGroupFunctions from "./LinkGroupFunctions";
import { DialogContent } from '@material-ui/core';
import AddItemToLinkGroupDialog from './AddItemToLinkGroupDialog';
import { Item, LinkGroup } from 'src/types/LinkGroup';
import { KbApiMethod } from '../helpers/kbapi';
import Axios from 'axios';
import withSnackbar from '../PageStyle/withSnackbar';

interface IState {
  InstructionsList: any;
  data: any;
  LinkGroupItems: Array<any>;
  LinkGroupName: string;
  LinkGroupDesc: string;
  showAddModifiers: boolean;
}

interface IProps {
  data: any;
  handleCancelClick(): void;
  showAlert(message: string, type?: string): void;
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
      showAddModifiers: false
    };
  }

  componentDidMount() {
    this.getLinkGroupInfo();
    this.loadCookingInstructionsList();
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

  removeLinkGroupItem = (itemIdToDelete: number) => {
    const { LinkGroupItems } = this.state;
    const allItemsButDeletedItem = LinkGroupItems.filter((item) => item.linkGroupItemId != itemIdToDelete);

    const url = `${KbApiMethod("LinkGroups")}/${this.props.data.linkGroupId}/LinkGroupItem/${itemIdToDelete}`;
    Axios.delete(url).then(() => {
      this.setState({ LinkGroupItems: allItemsButDeletedItem });
      this.props.showAlert("Modifier Deleted Successfully", "success");
    })
    .catch((error) => {
      this.props.showAlert(error.message, "error");
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
        this.props.showAlert("Modifier Added Succesfully", "success");
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
              value={this.state.LinkGroupName || ""}
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
              value={this.state.LinkGroupDesc || ""}
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
              className="-highlight -striped"
              defaultPageSize={10}
              data={this.state.LinkGroupItems}
              columns={[
                {
                  Header: "PLU",
                  accessor: "item.scanCode",
                  show: true,
                  style: { cursor: "pointer" }
                },
                {
                  Header: "Modifier",
                  accessor: "item.productDesc",
                  show: true,
                  style: { cursor: "pointer" }
                },
                {
                  Header: "Brand",
                  accessor: "item.brandName",
                  show: true,
                  style: { cursor: "pointer" }
                },
                {
                  Header: "InstructionListId",
                  accessor: "instructionListId",
                  show: false,
                  style: { cursor: "pointer" }
                },
                {
                  Header: "Cooking Instructions",
                  accessor: "instructionListId",
                  show: true,
                  style: { cursor: "pointer" },
                  Cell: cellInfo => (
                    <InstructionListPicker
                      SelectOptions={this.state.InstructionsList}
                      SelectedOption={cellInfo.original.instructionListId || -1}
                      LinkGroupId={cellInfo.original.linkGroupId}
                      handleSelectionChanged={e => {
                        const data = [...this.state.LinkGroupItems];
                        data[cellInfo.index][cellInfo.column.id!] =
                          e.target.value;
                        this.setState({ ...this.state, LinkGroupItems: data });
                      }}
                    />
                  )
                },
                {
                  style: { textAlign: "center" },
                  show: true,
                  Cell: row => (
                    <Button color="secondary" onClick = {() => this.removeLinkGroupItem(row.original.linkGroupItemId)}>
                      Delete
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
              <Button variant="contained" color="primary" fullWidth>
                Save
              </Button>
            </Grid>
          </Grid>
          </Grid>
        </Grid>
        <AddItemToLinkGroupDialog 
        isOpen = {this.state.showAddModifiers} 
        handleAddModifiers={this.handleAddModifiers}
        onClose = {() => this.setState({showAddModifiers: false})}/>
        </DialogContent>)}
}

export default withSnackbar(EditLinkGroup);
