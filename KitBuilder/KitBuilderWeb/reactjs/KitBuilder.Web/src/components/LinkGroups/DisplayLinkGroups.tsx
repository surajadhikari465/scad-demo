import * as React from 'react'
import { withStyles } from '@material-ui/core/styles';

import ReactTable from "react-table";
import "react-table/react-table.css";
import Button from '@material-ui/core/Button'

import DeleteIcon from '@material-ui/icons/Delete'


interface IProps {
    DisplayData: []
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
        console.log(row);
        console.log(column);
        if (column.Header !== "Action") {
            if (row !== null) {
                this.LinkGroupSelectedForEdit(row.original.linkGroupId);
            }
        }
    }

    LinkGroupSelectedForEdit(id:number) {
        console.log("selecting " + id)
    }

    render() {
        return (
            <React.Fragment>
                <ReactTable
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
                            show: true
                        },
                        {
                            Header: "Link Group Desc",
                            accessor: "groupDescription",
                            show: true
                        },
                        {
                            Header: "Regional Association",
                            accessor: "regionalAssociation",
                            show: true, 
                            Cell: (row) => {return "will come from another PBI"}
                        }, {
                            Header: "Action",
                            accessor: "Action",
                            show: true,
                            Cell: (row) => (
                                <Button variant="contained" size="small" color="secondary" 
                                onClick={(e:any) => { e.preventDefault(); alert("click!"); return false;}}>
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