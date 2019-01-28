import * as React from "react";
import ReactTable from "react-table";
import "react-table/react-table.css";
import Checkbox from '@material-ui/core/Checkbox';
import { Grid } from '@material-ui/core';
import { withStyles } from '@material-ui/core/styles';
const styles = (theme: any) => ({
    root: {
        marginTop: 50
    }
});

function KitItemAddModalDisplay(props: any) {
    return (
        <ReactTable
            data={props.data}
            columns={[
                {
                    Header: "Main Item Id",
                    accessor: "ItemId",
                    show: false
                },
                {
                    Header: "Main Item",
                    accessor: "ProductDesc"
                },
                {
                    Header: "Scan Code",
                    accessor: "ScanCode"
                },
                {
                    Header: "Select",
                    id: "select",
                    Cell: row => (
                        <Grid container justify="center" alignItems="center">
                            <Checkbox color="primary" value="false" onChange={() => props.onChecked(row)} />
                        </Grid>
                    )
                }
            ]}
            defaultPageSize={5}
            className="-striped -highlight"
        />
    )
}

export default withStyles(styles, { withTheme: true })(KitItemAddModalDisplay);