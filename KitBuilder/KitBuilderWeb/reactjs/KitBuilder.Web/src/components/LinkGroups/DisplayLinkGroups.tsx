import * as React from 'react'
import { withStyles } from '@material-ui/core/styles';
import ReactTable from "react-table";
import "react-table/react-table.css";
import Button from '@material-ui/core/Button'
import DeleteIcon from '@material-ui/icons/Delete'
import Axios from 'axios';
import { KbApiMethod } from 'src/components/helpers/kbapi';
import Swal, { SweetAlertType } from 'sweetalert2'


interface IProps {
    DisplayData: [],
    handleSearchRowClick(linkGroupId: number): void,
    onSearch(): void
}
interface IState { }

const styles = (theme: any) => ({

});

class DisplayLinkGroups extends React.Component<IProps, IState> {
    constructor(props: any) {
        super(props);
        this.state = {
            DisplayData: []
        }
    }

    onSearchResultsCellClick(row: any, column: any) {
        if (column.Header !== "Action") {
            if (row !== undefined) {

                this.props.handleSearchRowClick(row.original.linkGroupId);
            }
        }
    }

    deleteKit(linkGroupRow: any) {

        var linkGroupId = linkGroupRow.original.linkGroupId;
        var url = KbApiMethod("LinkGroups") + "/" + linkGroupId;
        let message = "";
        let messageType: SweetAlertType | undefined = undefined;
        console.log(url);
        Axios.delete(url)
            .then(response => {
                if (response.status= 204) {
                    message = 'Kit Deleted.'
                    messageType = "success";
                    this.props.onSearch()
                    Swal({
                        title: message,
                        type: messageType,
                        showCancelButton: false,
                        confirmButtonColor: '#3085d6',
                        cancelButtonColor: '#d33',
                        confirmButtonText: 'Ok'
                    })
                }
            })
            .catch(error => {
                if (error.response.data.includes("409")) {
                    message = 'Link Group is in use. Please make sure this kit is not assigned to any kit.'
                    messageType = "error";
                } else {
                    message = error.response.data
                    messageType = "error";
                }

                Swal({
                    title: message,
                    type: messageType,
                    showCancelButton: false,
                    confirmButtonColor: '#3085d6',
                    cancelButtonColor: '#d33',
                    confirmButtonText: 'Ok'
                })

            })
    }

    render() {
        return (
            <React.Fragment>
                <ReactTable
                    className="-highlight -striped"
                    getTdProps={(state: any, rowInfo: any, column: any, instance: any) => ({
                        onClick: (e: any) => {
                            this.onSearchResultsCellClick(rowInfo, column)
                        }
                    })}
                    defaultPageSize={10}
                    data={this.props.DisplayData}
                    columns={[
                        {
                            Header: "Link Group Name",
                            accessor: "groupName",
                            show: true,
                            style: { cursor: "pointer" }
                        },
                        {
                            Header: "Link Group Desc",
                            accessor: "groupDescription",
                            show: true,
                            style: { cursor: "pointer" }
                        },
                        {
                            Header: "Regional Association",
                            accessor: "regionalAssociation",
                            show: true,
                            style: { cursor: "pointer" },
                            Cell: (row) => { return "will come from another PBI" }
                        }, {
                            Header: "Action",
                            accessor: "Action",
                            style: { textAlign: "center" },
                            show: true,
                            width: 100,
                            Cell: (row) => (
                                <Button variant="text" size="small" color="secondary" onClick={() => this.deleteKit(row)}>
                                    Delete
                                <DeleteIcon />
                                </Button>
                            )

                        }
                    ]}
                />
            </React.Fragment>
        )
    }
}

export default withStyles(styles)(DisplayLinkGroups);