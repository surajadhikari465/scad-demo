import * as React from 'react'
import { withStyles } from '@material-ui/core/styles';
import ReactTable from "react-table";
import "react-table/react-table.css";
import Button from '@material-ui/core/Button'
import DeleteIcon from '@material-ui/icons/Delete'


interface IProps {
    DisplayData: [], 
    handleSearchRowClick(linkGroupId:number): void
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

    onSearchResultsCellClick(row:any, column:any) {
        if (column.Header !== "Action") {
            if (row !== undefined) {
            
                this.props.handleSearchRowClick(row.original.linkGroupId);
            }
        }
    }

    render() {
        return (
            <React.Fragment>
                <ReactTable
                className="-highlight -striped"
                getTdProps={(state:any, rowInfo:any, column:any, instance:any) => ({
                        onClick: (e:any) => {
                           this.onSearchResultsCellClick(rowInfo,column)
                        }
                        })}
                    defaultPageSize={10}
                    data={this.props.DisplayData}
                    columns={[
                        {
                            Header: "Link Group Name",
                            accessor: "groupName",
                            show: true,
                            style: {cursor: "pointer"}
                        },
                        {
                            Header: "Link Group Desc",
                            accessor: "groupDescription",
                            show: true,
                            style: {cursor: "pointer"}
                        },
                        {
                            Header: "Regional Association",
                            accessor: "regionalAssociation",
                            show: true, 
                            style: {cursor: "pointer"},
                            Cell: (row) => {return "will come from another PBI"}
                        }, {
                            Header: "Action",
                            accessor: "Action",
                            style:  {textAlign: "center"},
                            show: true,
                            width: 100,
                            Cell: (row) => (
                                <Button variant="text" size="small" color="secondary">
                                Delete
                                <DeleteIcon  />
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