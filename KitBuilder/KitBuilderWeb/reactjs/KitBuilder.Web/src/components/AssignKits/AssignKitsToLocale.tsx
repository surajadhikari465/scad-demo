import * as React from 'react';
import { Grid, Button } from '@material-ui/core';
import { AssignKitsTreeTable } from './AssignKitsTreeTable';
import axios from 'axios';
import { KbApiMethod } from '../helpers/kbapi'
import Swal from 'sweetalert2'
const hStyle = { color: 'red' };
const sucesssStyle = { color: 'blue' };
var urlStart = KbApiMethod("AssignKit");
var urlKit = KbApiMethod("Kits");

interface IAssignKitsToLocaleState {
     data: any,
     error: any,
     message: any,
     kitId: number,
     kitName: string,
}

interface IAssignKitsToLocaleProps {
     match: any,
}

export class AssignKitsToLocale extends React.Component<IAssignKitsToLocaleProps, IAssignKitsToLocaleState>
{
     constructor(props: any) {
          super(props);

          this.state = {
               data: [],
               error: null,
               message: null,
               kitId:0,
               kitName: "",
          }

          this.updateData = this.updateData.bind(this);
          this.saveChanges = this.saveChanges.bind(this);
     }

     componentDidMount() {
          var pathArray = window.location.href.split('/');
          pathArray = pathArray.reverse();
          const kitIdPassed = parseInt(pathArray[0]);
          this.setState({kitId:kitIdPassed}, () => {
          this.loadData();
          this.loadKit();
          });
     }

     loadKit() {
          const { kitId } = this.state;
          let url = urlKit;
                   url = url + '/' + kitId;
          fetch(url)
               .then(response => {
                    return response.json();
                    if (response.status === 404) {
                         console.log("Not Found");
                         this.setState({
                              error: "Kit Not Found"
                         })

                         this.setState({
                              message: null
                         })
                    }
               }).then(data => {
                    if(data.length > 0) this.setState({kitName: data[0].description});
               }).catch((error) => {
                    this.setState({
                         error: "Error in Displaying Data."
                    })

                    this.setState({
                         message: null
                    })
               });
     }

     loadData() {
          const { kitId } = this.state;
          let url = urlStart;

          url = url + '/' + kitId;
          fetch(url)
               .then(response => {
                    return response.json();
                    if (response.status === 404) {
                         console.log("Not Found");
                         this.setState({
                              error: "Data Not Found"
                         })

                         this.setState({
                              message: null
                         })
                    }
               }).then(data => {
                    this.parseData(data);
               }).catch((error) => {
                    this.setState({
                         error: "Error in Displaying Data."
                    })

                    this.setState({
                         message: null
                    })
               });
     }

     parseData(data: any) {
          var parsed_data = [];

          var map = {};
          for (var i = 0; i < data.length; i++) {
               data[i].childs = [];
               data[i].collapsed = true;
               map[data[i].localeId] = data[i];

               if (data[i].localeTypeId == 1) {
                    parsed_data.push(data[i]);
               }

               if (data[i].parentLocaleId == -1 || data[i].parentLocaleId == undefined || data[i].parentLocaleId == null) {
                    continue;
               }

               map[data[i].parentLocaleId].childs.push(data[i]);
          }

          this.setState({
               data: parsed_data
          });
     }

     putData(dest: Array<any>, data: Array<any>) {
          for (var i = 0; i < data.length; i++) {
               var item = JSON.parse(JSON.stringify(data[i]));
               delete item.collapsed;
               delete item.childs;
               if (item.isAssigned == true || item.isExcluded == true)
                    dest.push(item);
               this.putData(dest, data[i].childs);
          }
     }

     saveChanges() {
          
          Swal({
               title: 'Are you sure you want to save your changes?',
               type: 'info',
               showCancelButton: true,
               confirmButtonColor: '#3085d6',
               cancelButtonColor: '#d33',
               confirmButtonText: 'Ok'
             })
             .then((result:any) => {
               if (result.value) { 
                    this.saveData();
               } 
               else {
                    this.setState({
                         error: null
                    })

                    this.setState({
                         message: null
                    })
               }
             });
     }

     saveData() {
          var { data, kitId } = this.state;
          var dest: Array<any>;
          dest = [];
          let urlKitSave = urlKit + "/" + kitId + "/" + "AssignUnassignLocations"
          this.putData(dest, data);
          var headers = {
               'Content-Type': 'application/json', "Access-Control-Allow-Origin": "*"
          }

          axios.post(urlKitSave, JSON.stringify(dest),
               {
                    headers: headers
               }).then(response => {
                    this.setState({
                         error: null
                    })

                    this.setState({
                         message: "Data Saved Succesfully."
                    }, ()=>this.loadData())
               }).catch(error => {
                    this.setState({
                         error: "Error in Saving Data."
                    })

                    this.setState({
                         message: null
                    })
               });
     }

     updateData() {
          this.setState({
               data: this.state.data
          });
     }

     render() {
          const { data } = this.state;

          return (
               <React.Fragment>
                    <Grid container md={12} justify="center">
                         <div className="error-message" >
                              <span style={hStyle}> {this.state.error}</span>
                         </div>
                    </Grid>
                    <Grid container md={12} justify="center">
                         <div className="Success-message" >
                              <span style={sucesssStyle}> {this.state.message}</span>
                         </div>
                    </Grid>
                    <h3>Kit Name: {this.state.kitName}</h3>
                    <Grid container justify="flex-end">
                         <Button variant="contained" color="primary" onClick={() => this.saveChanges()} >
                              Save Changes
                         </Button>
                    </Grid>

                    <AssignKitsTreeTable kitId = {this.state.kitId} disabled={false} data={data} updateData={this.updateData} />
               </React.Fragment>
          );
     }
}
export default AssignKitsToLocale;