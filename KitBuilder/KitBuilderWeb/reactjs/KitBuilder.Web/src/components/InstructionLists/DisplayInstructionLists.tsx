import * as React from "react";
import ReactTable from "react-table";
import "react-table/react-table.css";
import Hidden from '@material-ui/core/Hidden';
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
    }
});

function DisplayInstructionLists(props: any) {
    return (
        <div className={props.classes.root}>
            <Grid container justify="center" spacing={8} alignItems="flex-start" style={marginBottom}>
                <Grid item xs={1} sm={1}>
                </Grid>
                <Grid item xs={11} sm={2} md={1}>

                    <span style={{ textAlign: 'center' }}> Instruction List Name:</span>

                </Grid>

                <Hidden only={['sm', 'md', 'lg', 'xl']}>
                    <Grid item sm >

                    </Grid>
                </Hidden>
                <Grid item xs={11} sm={3} md={2} className={props.classes.label}>
                    <TextField InputProps={{ classes: { input: props.classes.input1 } }} variant="outlined" className={props.classes.textField} onChange={props.InstuctionNameChange} value={props.instructionValue}></TextField>
                </Grid>

                <Hidden only={['sm', 'md', 'lg', 'xl']}>
                    <Grid item sm >

                    </Grid>
                </Hidden>
                <Grid item mt-xs-3 xs={11} sm={6} md={3} >
                    <Button variant="contained" color="secondary" className={props.classes.button} onClick={() => props.deleteInstruction()} >
                        Delete Instruction
                </Button>

                </Grid>
                <Hidden only={['md', 'lg', 'xl']}>
                    <Grid item sm >

                    </Grid>
                </Hidden>

                <Grid item xs={11} sm={2} md={1} justify="center">
                    <span> Instruction Type:</span>

                </Grid>

                <Hidden only={['sm', 'md', 'lg', 'xl']}>
                    <Grid item sm >

                    </Grid>
                </Hidden>
                <Grid item xs={11} sm={9} md={2} justify="center">
                    <span> {props.instructionTypeName}</span>
                </Grid>

                <Grid item xs={12} sm={12} md={1}>
                    <Grid container justify="flex-end">
                        <Button variant="contained" color="primary" className={props.classes.button} onClick={() => props.onAddMember()} >
                            Add Member
                </Button>
                    </Grid>
                </Grid>
                <Grid item xs={12} sm={1}>
                </Grid>
            </Grid>
            <Grid container justify="center">
                <Grid item xs={12} md={10} justify="center">
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
                </Grid>
            </Grid>
            <Grid container spacing={8} justify="center" style={{ marginTop: '20px' }}>
                <Grid item xs={12} md={1}>
                </Grid>
                <Grid item xs={12} md={6}>
                    <Button variant="contained" color="primary" className={props.classes.button} onClick={() => props.onSaveChanges()} >
                        Save Changes
                </Button>
                </Grid>
                <Grid item xs={12} md={4}>
                    <Button variant="contained" color="primary" className={props.classes.button} onClick={() => props.onPublishChanges()} >
                        Publish Changes
                </Button>
                </Grid>
            </Grid>
        </div>
    );
}

export default withStyles(styles, { withTheme: true })(DisplayInstructionLists);