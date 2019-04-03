import * as React from 'react'
import ReactTable from "react-table";
import "react-table/react-table.css";
import Button from '@material-ui/core/Button'
import withSnackbar from '../PageStyle/withSnackbar';


interface IProps {
    DisplayData: [],
    onSearch(name: string, desc: string, modName: string, plu: string, region: string): void;
    onEdit(id: number): void;
    showAlert(message: string, type?: string): void;
    deleteLinkGroup(row: any): void; 
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
                                <Button color="secondary" onClick={() => this.props.deleteLinkGroup(row)}>
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