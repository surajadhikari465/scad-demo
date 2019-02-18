import * as React from "react";
import ReactTable from "react-table";
import "react-table/react-table.css";
import Checkbox from '@material-ui/core/Checkbox';
import { withStyles } from '@material-ui/core/styles';
import { Grid, Button } from '@material-ui/core';

const styles = (theme: any) => ({
    root: {
        marginTop: 50
    }
});

function KitItemAddModalDisplay(props: any) {
    return (
        <React.Fragment>
            <ReactTable
                data={props.linkGroupdata}
                columns={[
                    {
                        Header: "LinkGroupId",
                        accessor: "LinkGroupId",
                        show: false
                    },
                    {
                        Header: "Link Group Name",
                        accessor: "GroupName"
                    },
                    {
                        Header: "Select",
                        id: "Select",
                        Cell: row => (
                            <Grid container justify="center" alignItems="center">
                                <Checkbox color="primary" value="false" onChange={() => props.onSelect(row)} />
                            </Grid>
                        )
                    }
                ]}
                defaultPageSize={5}
                className="-striped -highlight"
            />

            < Grid container justify="flex-end">
            < Grid item xs={12}>
            </ Grid>
                < Grid item xs={12}>
                    <Button
                        variant="contained"
                        color="primary"
                        className={props.classes.searchButtons}
                        onClick={() => props.queueLinkGroups()}
                    >
                        Queue Link Groups
                            </Button>

                </Grid>
            </Grid>
            <ReactTable
                data={props.selectedData}
                columns={[
                    {
                        Header: "SelectedLinkGroupID",
                        accessor: "LinkGroupId",
                        show: false
                    },
                    {
                        Header: "Selected Link Groups",
                        accessor: "GroupName"
                    },
                    {
                        Header: "Remove",
                        id: "Remove",
                        Cell: row => (
                            <Grid container justify="center" alignItems="center">
                                <Checkbox color="primary" value="false" onChange={() => props.onRemove(row)} />
                            </Grid>
                        )
                    }
                ]}
                defaultPageSize={5}
                className="-striped -highlight"
            />
            < Grid container justify="flex-end">
                < Grid item md={12}>
                    <Button
                        variant="contained"
                        color="primary"
                        className={props.classes.searchButtons}
                        onClick={() => props.addToKit()}
                    >
                        Add to Kit
                            </Button>

                </Grid>
            </Grid>
        </React.Fragment>
    )
}

export default withStyles(styles, { withTheme: true })(KitItemAddModalDisplay);