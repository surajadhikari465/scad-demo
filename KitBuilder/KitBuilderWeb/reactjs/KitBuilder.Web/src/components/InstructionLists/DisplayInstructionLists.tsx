import * as React from "react";
import ReactTable from "react-table";
import "react-table/react-table.css";
import { Grid } from '@material-ui/core';
import { withStyles } from '@material-ui/core/styles';
import Button from '@material-ui/core/Button';
import { isNumberError, isLettersNumbers } from '../EditableInput';
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

interface DisplayInstructionListsProps {
    data: Array<any>,
    renderEditable: any,
    onMemberDelete: Function,
    instructionValue: string,
    instructionTypeName: string, 
    onAddMember: Function,
    deleteInstruction: Function,
    onPublishChanges: Function,
    onSaveChanges: Function,
    isLoaded: boolean,
    isPublishDisabled: boolean,
    isSaveDisabled: boolean,
    classes: any,
}

function DisplayInstructionLists(props: DisplayInstructionListsProps) {
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
                                Cell: props.renderEditable(() => {}),
                                show: false
                            },
                            {
                                Header: "Instruction List Member Id",
                                accessor: "instructionListMemberId",
                                Cell: props.renderEditable(() => {}),
                                show: false
                            },
                            {
                                Header: "Group",
                                accessor: "group",
                                Cell: props.renderEditable(isLettersNumbers, 'Group Id')
                            },
                            {
                                Header: "Sequence",
                                accessor: "sequence",
                                Cell: props.renderEditable(isNumberError, 'Sequence')
                            },
                            {
                                Header: "Member",
                                accessor: "member",
                                Cell: props.renderEditable(isLettersNumbers, "Member Desc")
                            },
                            {
                                Header: "Action",
                                id: "action",
                                Cell: row => (
                                    <Grid container justify="center" alignItems="center">
                                        <Button variant="text" color='secondary' className={props.classes.deleteButton} onClick={() => props.onMemberDelete(row)}>
                                            DELETE
                                        </Button>
                                    </Grid>
                                ),
                                Footer: row => (
                                    <Grid container justify="center" alignItems="center">
                                        <Button disabled={!props.isLoaded} variant="text" color='primary' className={props.classes.deleteButton} onClick={() => props.onAddMember(row)}>
                                            ADD MEMBER
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
            <Grid container justify="flex-end" spacing={16}>
                <Grid item xs={12} md={3}>
                    <Button disabled={props.isSaveDisabled} variant="contained" color="primary" className={props.classes.button} onClick={() => props.onSaveChanges()} >
                        Save Changes
                </Button>
                </Grid>
                <Grid item xs={12} md={3}>
                    <Button disabled={props.isPublishDisabled} variant="outlined" color="primary" className={props.classes.button} onClick={() => props.onPublishChanges()} >
                        Publish Changes
                </Button>
                </Grid>
            </Grid>
        </div>
    );
}

export default withStyles(styles, { withTheme: true })(DisplayInstructionLists);