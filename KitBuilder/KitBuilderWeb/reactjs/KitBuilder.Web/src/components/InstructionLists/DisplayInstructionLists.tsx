import * as React from "react";
import ReactTable from "react-table";
import "react-table/react-table.css";

import { Grid, TextField } from '@material-ui/core';
import { withStyles } from '@material-ui/core/styles';
import Button from '@material-ui/core/Button';
const marginBottom = { marginBottom: 20 };
const styles = (theme: any) => ({
    root: {
        marginTop: 30
    },
    deleteButton: {
        margin: 0,
        padding: 0
    },
    instructionValue: {
        fontSize: 24
    },
    input1: {
        height: 8
      },

});

function DisplayInstructionLists(props: any) {
    return (
        <div className={props.classes.root}>
            <Grid container alignItems="flex-start" style={marginBottom}>
                <Grid item md={4}>
                <span style={{marginRight: '8px', textAlign: 'center'}}> Instruction List Name:</span>
                    <TextField InputProps={{ classes: { input: props.classes.input1 } }} variant="outlined" className={props.classes.textField} onChange={props.InstuctionNameChange} value={props.instructionValue}></TextField>
                </Grid>
                <Grid item md={2} alignItems="flex-end">
                 <Button variant="contained" color="secondary" className={props.classes.button} onClick={() => props.deleteInstruction()} >
                            Delete Instruction
                </Button>

                </Grid>

                <Grid item md={3} justify="center">
                    <span> Instruction Type:</span>
                    <span> {props.instructionTypeName}</span>
                </Grid>

                <Grid item md={3}>
                    <Grid container justify="flex-end">
                        <Button variant="contained" color="primary" className={props.classes.button} onClick={() => props.onAddMember()} >
                            Add Member
                </Button>
                    </Grid>
                </Grid>
            </Grid>
            <ReactTable
                data={props.data}
                columns={[
                    {
                        Header: "Instruction List Id",
                        accessor: "instructionListId",
                        Cell: props.renderEditable,
                        show: false
                    },
                    {
                        Header: "Instruction List Member Id",
                        accessor: "instructionListMemberId",
                        Cell: props.renderEditable,
                        show: false
                    },
                    {
                        Header: "Group",
                        accessor: "group",
                        Cell: props.renderEditable
                    },
                    {
                        Header: "Sequence",
                        accessor: "sequence",
                        Cell: props.renderEditable
                    },
                    {
                        Header: "Member",
                        accessor: "member",
                        Cell: props.renderEditable
                    },
                    {
                        Header: "Action",
                        id: "action",
                        Cell: row => (
                            <Grid container justify="center" alignItems="center">
                                <Button variant="contained" color="secondary" className={props.classes.deleteButton} onClick={() => props.onDelete(row)}>
                                    Delete
                    </Button>
                            </Grid>
                        )
                    }
                ]}
                defaultPageSize={5}
               className="-striped -highlight"
            />
            <Grid container style={{marginTop: '20px'}}>
                <Grid item md={6}>
                    <Grid container>
                        <Button variant="contained" color="primary" className={props.classes.button} onClick={() => props.onSaveChanges()} >
                            Save Changes
                </Button>
                    </Grid>
                </Grid>
                <Grid item md={6}>
                    <Grid container justify="flex-end">
                        <Button variant="contained" color="primary" className={props.classes.button} onClick={() => props.onPublishChanges()} >
                            Publish Changes
                </Button>
                    </Grid>
                </Grid>
            </Grid>
        </div>
    );
}

export default withStyles(styles, { withTheme: true })(DisplayInstructionLists);