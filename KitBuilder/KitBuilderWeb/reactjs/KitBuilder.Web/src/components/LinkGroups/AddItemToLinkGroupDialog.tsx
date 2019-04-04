import * as React from "react";
import ReactTable from "react-table";
import "react-table/react-table.css";
import {
  Grid,
  Button,
  Dialog,
  DialogContent,
  TextField,
  DialogActions
} from "@material-ui/core";
import { Item, LinkGroupItem } from "src/types/LinkGroup";
import { KbApiMethod } from "../helpers/kbapi";
import Axios, { AxiosResponse } from "axios";

interface IAddItemToLinkGroupDialogProps {
  isOpen: boolean;
  linkGroupItems: LinkGroupItem[];
  handleAddModifiers(items: Item[]): void;
  onClose(): void;
}

interface IAddItemToLinkGroupDialogState {
  searchResults: Array<Item>;
  queuedItems: Array<Item>;
  name: string;
  plu: string;
}

class AddItemToLinkGroupDialog extends React.PureComponent<
  IAddItemToLinkGroupDialogProps,
  IAddItemToLinkGroupDialogState
> {
  constructor(props: IAddItemToLinkGroupDialogProps) {
    super(props);
    this.state = {
      name: "",
      plu: "",
      searchResults: [],
      queuedItems: []
    };
  }

  onSearch = () => {
    const { name, plu } = this.state;
    const url = `${KbApiMethod("Items")}?ProductDesc=${name}&ScanCode=${plu}`;
    Axios.get(url).then((results: AxiosResponse<Item[]>) => {
      this.setState({ searchResults: results.data });
    });
  };

  isAlreadyQueued = (item: Item) =>
    this.state.queuedItems.some(x => x.itemId === item.itemId) ||
    this.props.linkGroupItems.some(x => x.item.itemId === item.itemId);

  onQueue = (linkGroup: Item) => {
    if (!this.isAlreadyQueued(linkGroup)) {
      const queuedItems = [...this.state.queuedItems, linkGroup];
      this.setState({ queuedItems });
    }
  };

  onDeQueue = (linkGroup: Item) => {
    const { queuedItems } = this.state;
    const queuedWithoutDeleted = queuedItems.filter(
      x => x.itemId !== linkGroup.itemId
    );
    this.setState({ queuedItems: queuedWithoutDeleted });
  };

  handleSelect = (linkGroup: Item) => {
    if (!this.isAlreadyQueued(linkGroup)) this.onQueue(linkGroup);
  };

  onAddToLinkGroup = () => {
    const { queuedItems } = this.state;
    this.props.handleAddModifiers(queuedItems);
  };

  render() {
    return (
      <Dialog open={this.props.isOpen} maxWidth="md" fullWidth={true}>
        <DialogContent>
          <Grid container justify="space-between" className="mb-3">
            <Grid item md={3}>
              <TextField
                label="Item Name"
                value={this.state.name}
                onChange={e => this.setState({ name: e.target.value })}
                fullWidth
                variant="outlined"
                InputLabelProps={{ shrink: true }}
              />
            </Grid>
            <Grid item md={3}>
              <TextField
                label="Item Plu"
                value={this.state.plu}
                onChange={e => this.setState({ plu: e.target.value })}
                fullWidth
                variant="outlined"
                InputLabelProps={{ shrink: true }}
              />
            </Grid>
            <Grid item md={3}>
              <Button
                variant="contained"
                color="primary"
                onClick={this.onSearch}
                fullWidth
              >
                Search
              </Button>
            </Grid>
          </Grid>
          <ReactTable
            data={this.state.searchResults}
            columns={[
              {
                Header: "Description",
                accessor: "productDesc"
              },
              {
                Header: "PLU",
                accessor: "scanCode"
              },
              {
                accessor: "select",
                Cell: row => (
                  <Grid container justify="center" alignItems="center">
                    <Grid item>
                      <Button
                        disabled={this.isAlreadyQueued(row.original)}
                        onClick={() => this.handleSelect(row.original)}
                      >
                        Select
                      </Button>
                    </Grid>
                  </Grid>
                )
              }
            ]}
            defaultPageSize={5}
            className="-striped -highlight"
          />

          <ReactTable
            data={this.state.queuedItems}
            columns={[
              {
                Header: "Description",
                accessor: "productDesc",
                className: "mui--text-center"
              },
              {
                Header: "PLU",
                accessor: "scanCode"
              },
              {
                Cell: row => (
                  <Grid container justify="center" alignItems="center">
                    <Button
                      color="secondary"
                      onClick={() => this.onDeQueue(row.original)}
                    >
                      Remove
                    </Button>
                  </Grid>
                )
              }
            ]}
            defaultPageSize={5}
            className="-striped -highlight"
          />
          <DialogActions>
            <Button variant="outlined" onClick={this.props.onClose}>
              {" "}
              Cancel{" "}
            </Button>
            <Button
              variant="contained"
              onClick={this.onAddToLinkGroup}
              color="primary"
            >
              {" "}
              Add To Link Group{" "}
            </Button>
          </DialogActions>
        </DialogContent>
      </Dialog>
    );
  }
}

export default AddItemToLinkGroupDialog;
