import * as React from "react";
import ReactTable from "react-table";
import "react-table/react-table.css";
import Checkbox from '@material-ui/core/Checkbox';
import { withStyles } from '@material-ui/core/styles';
import { Grid, Button } from '@material-ui/core';


const styles = (theme: any) => ({
    root: {
        marginTop: 50
    },
    searchButtons: {
        marginTop: '10px',
        marginBottom: '10px'
    }
    
});

function KitItemAddModalDisplay(props: any) {

    return (
   
        <React.Fragment>

            <ReactTable
                data={props.linkGroupData}
                columns={[
                    {
                        Header: "LinkGroupId",
                        accessor: "linkGroupId",
                        show: false
                    },
                    { 
                        Header: () => (
                        <div style={{ textAlign: "center" }}> Link Group Name</div>
                      ),
                      Cell: row => (
                        <div style={{ textAlign: "center" }}>{row.value}</div>
                      ),
               
                        accessor: "groupName"
                    },
                    {
                        Header: "Select",
                        accessor:"select",
                                  Cell: row => (
                            <Grid container justify="center" alignItems="center">
                                 <Checkbox color="primary" checked = {row.value} onChange={() => props.onSelect(row)} />   
                            </Grid>
                        )
                    }
                ]}
                defaultPageSize={5}
                className="-striped -highlight"
            />

            < Grid container justify="flex-end">       
             
                    <Button
                        variant="contained"
                        color="primary"
                        className={props.classes.searchButtons}
                        onClick={() =>
                            {
                            props.queueLinkGroups()}} 
                    >
                        Queue Link Groups
                            </Button>

              
            </Grid>
            <ReactTable
                data={props.selectedData}
                columns={[
                    {
                        Header: "SelectedLinkGroupID",
                        accessor: "linkGroupId",
                        show: false
                    },
                    {
                        Header: () => (
                            <div style={{ textAlign: "center" }}> Selected Link Groups</div>
                          ),
                          Cell: row => (
                            <div style={{ textAlign: "center" }}>{row.value}</div>
                          ),
                        accessor: "groupName"
                    },
                    {
                        Header: "Remove",
                        id: "remove",
                        Cell: row => (
                            <Grid container justify="center" alignItems="center">
                                <Button variant="contained" color="primary"  onClick = {() => props.onRemove(row)} >
                                Remove
                            </Button>
                            </Grid>
                        )
                    }
                ]}
                defaultPageSize={5}
                className="-striped -highlight"
            />
            < Grid container justify="flex-end">
                   <Button
                        variant="contained"
                        color="primary"
                        className={props.classes.searchButtons}
                        onClick={() => props.addToKit()}
                    >
                        Add to Kit
                            </Button>

            </Grid>
        </React.Fragment>
    )
}

export default withStyles(styles, { withTheme: true })(KitItemAddModalDisplay);