import * as React from "react";
import ReactTable from "react-table";
import "react-table/react-table.css";
import { withStyles } from '@material-ui/core/styles';
import { Grid, TextField, Button } from '@material-ui/core';

const styles = (theme: any) => ({
  root: {
    marginTop: 50
  },
  labelRoot: {
    fontSize: 18,
    fontWeight: 600
  },
  searchButtons: {
    marginTop: '10px',
    marginBottom: '10px'
  }

});

function KitResults(props: any) {
  return (
    <React.Fragment>
      <ReactTable
        data={props.kitsViewData}
        noDataText = "No Link Groups"
        columns={[
          {
            Header: "LinkGroupId",
            accessor: "linkGroupId",
            show: false
          },
          {
            Header: () => (
              <div style={{ textAlign: "center" }}> Link Groups</div>
            ),

            Cell: row => (
              <div style={{ textAlign: "center" }}>{row.value}</div>
            ),

            accessor: "groupName"
          },
          {
            Header: () => (
              <div style={{ textAlign: "center" }}> Modifiers / Ingredients</div>
            ),
            minWidth: 200,
            Cell: row => (
              <div style={{ textAlign: "left" }}>
                {
                row.value.split('\r').map((line1: string) => 
                (!line1.includes("^") && (line1.includes('Unauthorized') || line1.includes('[ Calories'))) ?
                    <div style={{ color: 'red' }}>
                      {
                        line1.replace('Unauthorized','').split('\n').map((line: string) => 
                        
                          <div>
                            {line}
                          </div>
                        )
                      
                        }
                    </div> :
                    <div >
                      {line1.split('\n').map((line: string) =>
                      
                      <div>
                      {line}
                     </div>
                   )
                    }
                    </div>)
                } 
                </div>
            ),

            accessor: "formattedAllModifiersProperties"
          },

          {
            Header: () => (
              <div style={{ textAlign: "center" }}> Link Group Properties</div>
            ),
            Cell: row => (
              <div style={{ textAlign: "center" }}>{

                row.value.split('\n').map((line: string) => <div>{line}</div>)}
              </div>
            ),

            accessor: "formattedLinkGroupProperties"
          }

        ]}
        defaultPageSize={5}
        className="-striped -highlight"
      />

      <Grid container justify="space-between" spacing={16} className="px-3 pb-3">
        <Grid xs={12} md={12} item>
        </Grid>
        <Grid item xs={12} md={3}>
          <TextField disabled className='search-textfield'
            margin="dense" variant="outlined" label='Price' InputLabelProps={{
              shrink: true, FormLabelClasses: {
                root: props.classes.labelRoot
              }
            }} value={props.price}></TextField>
        </Grid>

        <Grid item xs={12} md={3}>
          <TextField disabled className={ props.newSearchStated == true && (props.minimumCaloriesValue.length == 0 || props.minimumCaloriesValue <= 0) ? 'error' : 'search-textfield' } 
            margin="dense" variant="outlined" label='Minimum Calories'  InputLabelProps={{
              shrink: true, FormLabelClasses: {
                root: props.classes.labelRoot
              }
            }} value={props.minimumCaloriesValue}></TextField>
        </Grid>

        <Grid item xs={12} md={3}>
          <TextField className='search-textfield' margin="dense" variant="outlined" label='Maximum Calories' InputLabelProps={{
            shrink: true,
            FormLabelClasses: {
              root: props.classes.labelRoot
            }
          }} onChange={props.maximumCalories} value={props.maximumCaloriesValue}></TextField>
        </Grid>

        <Grid item xs={12} md={3}>
        <Grid container alignItems="center" alignContent="center" style={{height: "100%"}}>
        <Grid item xs={12}>
          <Button fullWidth variant="outlined" disabled={props.disableSaveButton} color="primary" className={props.classes.button} onClick={() => props.onSaveChanges()} >
            Save Changes
                </Button>
                </Grid>
                </Grid>
        </Grid>
      </Grid>
    </React.Fragment>
  )
}

export default withStyles(styles, { withTheme: true })(KitResults);