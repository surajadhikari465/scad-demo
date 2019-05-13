import * as React from "react";
import ReactTable from "react-table";
import "react-table/react-table.css";
import KitStatusIcon from './KitStatusIcon';
import { Grid } from '@material-ui/core';
import { withStyles } from '@material-ui/core/styles';
import { Delete, Edit } from '@material-ui/icons';
import Button from '@material-ui/core/Button';

const styles = (theme: any) => ({
    root: {
      //  marginTop: theme.spacing.unit*2,
    },
    deleteButton: {
        margin: 0,
        padding: 0
    },
    instructionValue: {
        fontSize: 24
    }

});

function DisplayKits(props: any) {
    return (
        <div className={props.classes.root}>
            <ReactTable
                data={props.data}
                columns={[
                    {
                        Header: "Kit Id",
                        accessor: "kitId",
                        Cell: props.renderEditable,
                        show: false
                    },
                    {
                        Header: () => (
                            <div style={{ textAlign: "center" }}>Status</div>
                          ),
                        accessor: "kitStatus",
                        Cell: row => (
                            <div style={{textAlign: "center", fontWeight: "bold"}}><KitStatusIcon status={row.value}/></div>
                          )
                    },
                    {
                        Header: () => (
                            <div style={{ textAlign: "center" }}>Main Item</div>
                          ),
                        accessor: "item.productDesc",
                        Cell: row => (
                            <div style={{ textAlign: "center" }}>{row.value}</div>
                          )
                    },
                    {
                        Header: () => (
                            <div style={{
                                textAlign: "center"
                            }}>
                               Kit Description
                              </div>
                        ),
                        accessor: "description",
                        Cell: row => (
                            <div style={{ textAlign: "center" }}>{row.value}</div>
                          )
                    },
 
                    {
                        Header: "Edit",
                        id: "action",
                        Cell: row => (
                            <Grid container justify="center" alignItems="center">
                                <Button color="primary" className={props.classes.editbutton} onClick={() => props.onEdit(row.original.kitId)}>
                                    <Edit/>
                    </Button>
                            </Grid>
                        )
                    },

                    {
                        Header: "Delete",
                        id: "action",
                        Cell: row => (
                            <Grid container justify="center" alignItems="center">
                                <Button color="secondary" className={props.classes.deleteButton} onClick={() => props.onDelete(row)}>
                                    <Delete/>
                    </Button>
                            </Grid>
                        )
                    }
                ]}
                defaultPageSize={10}
                className="-striped -highlight"
            />
           </div>
    );
}

export default withStyles(styles, { withTheme: true })(DisplayKits);