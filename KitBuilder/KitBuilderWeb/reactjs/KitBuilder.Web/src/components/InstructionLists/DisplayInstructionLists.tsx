import * as React from "react";
import ReactTable from "react-table";
import "react-table/react-table.css";
import { Grid } from '@material-ui/core';
import { withStyles } from '@material-ui/core/styles';
import Button from '@material-ui/core/Button';
const marginBottom = { marginBottom: 20 };
const styles = (theme: any) => ({
    deleteButton: {
        margin: 0,
        padding: 0
    },
    instructionValue: {
        fontSize: 24,
    },
    input1: {
        height: 8
    },
    button: {
        width: '100%'
    }
});

function DisplayInstructionLists(props: any) {
    return (
        <div >

            <Grid container justify="center">
                <Grid item xs={12}>
                    <ReactTable
                        data={props.data}
                        className = '-striped -highlight instructions-table'
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
                                        <Button variant="text" color='secondary' className={props.classes.deleteButton} onClick={() => props.onDelete(row)}>
                                            DELETE
                                        </Button>
                                    </Grid>
                                )
                            }
                        ]}
                        defaultPageSize={10}
                    />
                </Grid>
            </Grid>
            <Grid container justify="center" alignItems="flex-start" style={marginBottom}>

<Grid item xs={12}>
    <Grid container justify="flex-end">
        <Grid item xs={12} sm={12} md={5} lg={2} >
           {/*<Button variant="contained" color="primary" className={props.classes.button} onClick={() => props.onAddMember()} >
                Add Member
                    </Button>*/}
        </Grid>
    </Grid>
</Grid>

</Grid>
            <Grid container justify="flex-end">
                <Grid item xs={12} md={3} className='pr-3'>
                    <Button disabled={true} variant="contained" color="primary" className={props.classes.button} onClick={() => props.onSaveChanges()} >
                        Save Changes
                </Button>
                </Grid>
                <Grid item xs={12} md={3}>
                    <Button disabled={true} variant="outlined" color="primary" className={props.classes.button} onClick={() => props.onPublishChanges()} >
                        Publish Changes
                </Button>
                </Grid>
            </Grid>
        </div>
    );
}

export default withStyles(styles, { withTheme: true })(DisplayInstructionLists);