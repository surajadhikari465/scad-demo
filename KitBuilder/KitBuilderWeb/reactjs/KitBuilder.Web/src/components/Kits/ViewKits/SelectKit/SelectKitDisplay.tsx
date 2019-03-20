import * as React from "react";
import ReactTable from "react-table";
import "react-table/react-table.css";
import { Grid, Button } from '@material-ui/core';
import { withStyles } from '@material-ui/core/styles';

const styles = (theme: any) => ({
    root: {
        marginTop: 50
    }
});

function SelectKitDisplay(props: any) {
    return (
        <ReactTable
            data={props.data}
            columns={[
                {
                    Header: "Kit Id",
                    accessor: "KitId",
                    show: false
                },
                {
                    Header: () => (
                        <div style={{
                            textAlign: "center"
                        }}>
                           Kit Description
                          </div>
                    ),
                    accessor: "Description",
                    Cell: row => (
                        <div style={{ textAlign: "center" }}>{row.value}</div>
                      )
                },
                {
                    Header: () => (
                        <div style={{ textAlign: "center" }}>Main Item Scan Code</div>
                      ),
                    accessor: "Item.scanCode",
                    Cell: row => (
                        <div style={{ textAlign: "center" }}>{row.value}</div>
                      )
                },
               
               
                {
                    Header: "Select",
                    id: "select",
                    Cell: row => (
                        <Grid container justify="center" alignItems="center">
                            <Button color="primary" onClick={() => props.onSelected(row.original)}>
                                Select
                            </Button>
                        </Grid>
                    )
                }
            ]}
            defaultPageSize={5}
            className="-striped -highlight"
        />
    )
}

export default withStyles(styles, { withTheme: true })(SelectKitDisplay);