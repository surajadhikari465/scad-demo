import * as React from 'react';
import { Grid, Button } from '@material-ui/core';
import { AssignKitsTreeTable } from './AssignKitsTreeTable';
import axios from 'axios';
import { KbApiMethod } from '../helpers/kbapi'
import Swal from 'sweetalert2';
import withSnackbar from '../PageStyle/withSnackbar';
var urlStart = KbApiMethod("AssignKit");
var urlKit = KbApiMethod("Kits");

interface IAssignKitsToLocaleState {
     data: any,
     kitId: number,
     kitName: string,
     kitType:string,
     isSimplekitType: boolean
}

interface IAssignKitsToLocaleProps {

     showAlert:any
}

class AssignKitsToLocale extends React.Component<IAssignKitsToLocaleProps, IAssignKitsToLocaleState>
{
     constructor(props: IAssignKitsToLocaleProps) {
          super(props);

          this.state = {
               data: [],
               kitId:0,
               kitName: "",
               kitType:"",
               isSimplekitType :false,
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
                         this.props.showAlert("Kit Not Found.", "error")
                   
                    }
               }).then(data => {
                    if(data.length > 0) 
                    this.setState({kitName: data[0].description, kitType:data[0].kitType}
                   
                    );
 ;
                    if(data[0].kitType==1)
                    {
                    
                         this.setState({isSimplekitType: true});
                    }
               }).catch((error) => {
          
                    this.props.showAlert("Error in displaying data.", "error")
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
                         console.log("Not found");
                         this.props.showAlert("Data not found.", "error")
                    }
               }).then(data => {
                    this.parseData(data);
               }).catch((error) => {
                    this.props.showAlert("Error in displaying data.", "error")
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
     publishChanges()
     {
          var headers = {
               'Content-Type': 'application/json', "Access-Control-Allow-Origin": "*"
          }
          var urlParam = this.state.kitId;
          var url = urlKit;

               axios.put(url, urlParam,
                    {
                         headers: headers
                    })
                    .then(() => {
                         this.props.showAlert("Kit queued successfully.", "success");
                         this.loadData();
                       
                    })
                    .catch((error) => {
                  
                         if(error.response.status == "412")
                         {    
                              this.props.showAlert("Kit properties not set for all assigned locales.","error");
                         }
                         else   if(error.response.status == "404")
                         {
                              this.props.showAlert("Kit properties not set for any assigned locales.", "error");
                         }
                         else{
                              this.props.showAlert("Error in queueing Kit.", "error");
                         }
                        
                         return;
                    })
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

                    this.props.showAlert("Data saved succesfully.", "success")
                    this.loadData();
               }).catch(error => {
               
                    this.props.showAlert("Error in saving data.", "error")
                   
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
                    <h3>Kit Name: {this.state.kitName}</h3>
                    <Grid container justify= "flex-end" spacing={16}>
                    <Grid item>
                    <Button variant="contained" color="primary" onClick={() => this.publishChanges()} >
                              Publish
                         </Button>
                    </Grid>
                         <Grid item>
                         <Button variant="contained" color="primary" onClick={() => this.saveChanges()} >
                              Save Changes
                         </Button>
                         </Grid>
                    </Grid>

                    <AssignKitsTreeTable kitId = {this.state.kitId} isSimplekitType= {this.state.isSimplekitType} disabled={false} data={data} updateData={this.updateData} />
               </React.Fragment>
          );
     }
}
export default withSnackbar(AssignKitsToLocale) ;