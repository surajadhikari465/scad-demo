import * as React from "react";
import ReactTable from "react-table";
import "react-table/react-table.css";
import { withStyles } from "@material-ui/core/styles";
import { Grid, Button } from "@material-ui/core";
import { Add, Delete } from "@material-ui/icons";
import { LinkGroup } from "../../../../types/LinkGroup";

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
  searchResults: Array<LinkGroup>;
  onQueue: any;
  onDeQueue: any;
  queuedLinkGroups: Array<LinkGroup>;
  kitLinkGroup: Array<LinkGroup>;
  classes: any;
}

class KitItemAddModalDisplay extends React.PureComponent<IDisplayProps, {}> {
  isAlreadyQueued = (linkGroup : LinkGroup) => 
    this.props.queuedLinkGroups.some(
      x => x.linkGroupId === linkGroup.linkGroupId
    );

  handleSelect = (linkGroup: LinkGroup) => {
    if (!this.isAlreadyQueued(linkGroup)) this.props.onQueue([linkGroup]);
  };

  isAlreadyAddedToKit = (linkGroup: LinkGroup) => {
    const r = this.props.kitLinkGroup.some(
      lg => lg.linkGroupId === linkGroup.linkGroupId
    );
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
                      <Add/>
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
          data={this.props.queuedLinkGroups}
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
                    <Delete/>
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
