import * as React from "react";
import ReactTable from "react-table";
import "react-table/react-table.css";
import { withStyles } from '@material-ui/core/styles';
import { Grid, Button } from '@material-ui/core';

const styles = (theme: any) => ({
  root: {
    marginTop: 50
  },
  labelRoot: {
    fontSize: 18,
    fontWeight: 600
  },
  searchButtons: {
    marginTop: '10px',
    marginBottom: '10px'
  }

});

function KitByStoreResults(props: any) {
  return (
    <React.Fragment>
      <ReactTable
        data={props.kitsByStoreData}
        noDataText="No Kit Found."
        columns={[
          {
            Header: () => (
              <div style={{ textAlign: "center" }}>Kit Id</div>
            ),

            Cell: row => (
              <div style={{ textAlign: "center" }}>{row.value}</div>
            ),
            show: false,
            accessor: "kitId"
          },

          {
            Header: () => (
              <div style={{ textAlign: "center" }}>Kit Description</div>
            ),

            Cell: row => (
              <div style={{ textAlign: "center" }}>{row.value}</div>
            ),

            accessor: "kitDescription"
          },
          {
            Header: () => (
              <div style={{ textAlign: "center" }}> Scan Code</div>
            ),

            Cell: row => (
              <div style={{ textAlign: "center" }}>{row.value}</div>
            ),

            accessor: "scanCode"
          },
          {
            Header: () => (
              <div style={{ textAlign: "center" }}> Product Description</div>
            ),
            Cell: row => (
              <div style={{ textAlign: "center" }}>{row.value}</div>
            ),

            accessor: "productDescription"
          },
          {
            Header: () => (
              <div style={{ textAlign: "center" }}> Status</div>
            ),
            Cell: row => (
              <div style={{ textAlign: "center" }}>{row.value}</div>
            ),

            accessor: "status"
          },
          {
            Header: "View",
            id: "View",
            Cell: row => (
              <Grid container justify="center" alignItems="center">
                <Button color="primary" onClick={() => props.onSelected(row.original)}>
                  View Kit
                    </Button>
              </Grid>
            )
          }

        ]}
        defaultPageSize={50}
        className="-striped -highlight"
        minRows = {0}
      />
    </React.Fragment>
  )
}

export default withStyles(styles, { withTheme: true })(KitByStoreResults);