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

function SelectLocaleDisplay(props: any) {
    return (
        <ReactTable
            data={props.data}
            columns={[
                {
                    Header: "Locale Id",
                    accessor: "localeId",
                    show: false
                },
                {
                    Header: "Chain Id",
                    accessor: "chainId",
                    show: false
                },
                {
                    Header: "Region Id",
                    accessor: "regionId",
                    show: false
                },
                {
                    Header: "Metro Id",
                    accessor: "metroId",
                    show: false
                },
                {
                    Header: "Store Id",
                    accessor: "storeId",
                    show: false
                },
                {
                    Header: () => (
                        <div style={{
                            textAlign: "center"
                        }}>
                           Name
                          </div>
                    ),
                    accessor: "localeName",
                    Cell: row => (
                        <div style={{ textAlign: "center" }}>{row.value}</div>
                      )
                },
                {
                    Header: () => (
                        <div style={{
                            textAlign: "center"
                        }}>
                           Region
                          </div>
                    ),
                    accessor: "regionCode",
                    Cell: row => (
                        <div style={{ textAlign: "center" }}>{row.value}</div>
                      )
                },
                {
                    Header: () => (
                        <div style={{ textAlign: "center" }}>Metro</div>
                      ),
                    accessor: "metro",
                    Cell: row => (
                        <div style={{ textAlign: "center" }}>{row.value}</div>
                      )
                },
                {
                    Header: () => (
                        <div style={{ textAlign: "center" }}>Store</div>
                      ),
                    accessor: "storeAbbreviation",
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

export default withStyles(styles, { withTheme: true })(SelectLocaleDisplay);