import * as React from 'react'
import ReactTable from "react-table";
import "react-table/react-table.css";
import Button from '@material-ui/core/Button'
import Axios from 'axios';
import { KbApiMethod } from 'src/components/helpers/kbapi';
import withSnackbar from '../PageStyle/withSnackbar';


interface IProps {
    DisplayData: [],
    onSearch(name: string, desc: string, modName: string, plu: string, region: string): void;
    onEdit(id: number): void;
    showAlert(message: string, type?: string): void;
}
interface IState { }

class DisplayLinkGroups extends React.Component<IProps, IState> {
    constructor(props: any) {
        super(props);
        this.state = {
            DisplayData: []
        }
    }

    onSearchResultsCellClick = (row: any) => {
        this.props.onEdit(row.original.linkGroupId);
    }

    deleteKit = (linkGroupRow: any) => {
        var linkGroupId = linkGroupRow.original.linkGroupId;
        var url = KbApiMethod("LinkGroups") + "/" + linkGroupId;
        
        Axios.delete(url)
            .then(response => {
                if (response.status= 204) {
                    this.props.showAlert("Member deleted");
                }
            })
            .catch(error => {
                let message;
                if (error.response.data.includes("409")) {
                    message = 'Link Group is in use. Please make sure this kit is not assigned to any kit.'
                } else {
                    message = error.response.data
                }
                this.props.showAlert(message, "error");
            })
    }

    render() {
        return (
            <React.Fragment>
                <ReactTable
                    className="-highlight -striped"
                    defaultPageSize={10}
                    data={this.props.DisplayData}
                    columns={[
                        {
                            Header: "Link Group Name",
                            accessor: "groupName",
                            show: true,
                            style: { textAlign: "center" },
                        },
                        {
                            Header: "Link Group Desc",
                            accessor: "groupDescription",
                            show: true,
                            style: { textAlign: "center" },
                        },
                        {
                            Header: "Regional Association",
                            accessor: "regions",
                            show: true,
                            style: { textAlign: "center" },
                        }, 
                        {
                            style: { textAlign: "center" },
                            show: true,
                            width: 100,
                            Cell: (row) => (
                                <Button color="primary" onClick={() => this.onSearchResultsCellClick(row)}>
                                    Edit
                                </Button>
                            )

                        },
                        {
                            style: { textAlign: "center" },
                            show: true,
                            width: 100,
                            Cell: (row) => (
                                <Button color="secondary" onClick={() => this.deleteKit(row)}>
                                    Delete
                                </Button>
                            )

                        }
                    ]}
                />
                
            </React.Fragment>

            
        )
    }
}

export default withSnackbar(DisplayLinkGroups);