import * as React from "react";
import ReactTable from "react-table";
import "react-table/react-table.css";
import { withStyles } from "@material-ui/core/styles";
import { Grid, Button } from "@material-ui/core";
import { LinkedGroup } from "../../../../types/LinkGroup";

const styles = (theme: any) => ({
  root: {
    marginTop: 50
  },
  searchButtons: {
    marginTop: "10px",
    marginBottom: "10px"
  }
});

interface IDisplayProps {
  searchResults: Array<LinkedGroup>;
  onQueue: any;
  onDeQueue: any;
  queuedLinkedGroups: Array<LinkedGroup>;
  kitLinkGroup: Array<LinkedGroup>;
  classes: any;
}

class KitItemAddModalDisplay extends React.PureComponent<IDisplayProps, {}> {
  isAlreadyQueued = (linkedGroup : LinkedGroup) => 
    this.props.queuedLinkedGroups.some(
      x => x.linkGroupId === linkedGroup.linkGroupId
    );

  handleSelect = (linkedGroup: LinkedGroup) => {
    if (!this.isAlreadyQueued(linkedGroup)) this.props.onQueue([linkedGroup]);
  };

  isAlreadyAddedToKit = (linkedGroup: LinkedGroup) => {
    const r = this.props.kitLinkGroup.some(
      lg => lg.linkGroupId === linkedGroup.linkGroupId
    );
    console.log(r);
    return r;
  };
  render() {
    return (
      <React.Fragment>
        <ReactTable
          data={this.props.searchResults}
          columns={[
            {
              Header: "LinkGroupId",
              accessor: "linkGroupId",
              show: false
            },
            {
              Header: () => (
                <div style={{ textAlign: "center" }}> Link Group Name</div>
              ),
              Cell: row => (
                <div style={{ textAlign: "center" }}>{row.value}</div>
              ),

              accessor: "groupName"
            },
            {
              Header: "Select",
              accessor: "select",
              Cell: row => (
                <Grid container justify="center" alignItems="center">
                  <Grid item>
                    <Button
                      disabled={this.isAlreadyQueued(row.original) || this.isAlreadyAddedToKit(row.original)}
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
          data={this.props.queuedLinkedGroups}
          columns={[
            {
              Header: "SelectedLinkGroupID",
              accessor: "linkGroupId",
              show: false
            },
            {
              Header: () => (
                <div style={{ textAlign: "center" }}> Selected Link Groups</div>
              ),
              Cell: row => (
                <div style={{ textAlign: "center" }}>{row.value}</div>
              ),
              accessor: "groupName"
            },
            {
              Header: "Remove",
              id: "remove",
              Cell: row => (
                <Grid container justify="center" alignItems="center">
                  <Button
                    color="secondary"
                    onClick={() => this.props.onDeQueue(row.original)}
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
      </React.Fragment>
    );
  }
}

export default withStyles(styles, { withTheme: true })(KitItemAddModalDisplay);
