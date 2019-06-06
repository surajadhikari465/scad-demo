import * as React from 'react';
import { Grid, Button } from '@material-ui/core';
import AssignKitsTreeTable from './AssignKitsTreeTable';
import axios from 'axios';
import { KbApiMethod } from '../helpers/kbapi'
import withSnackbar from '../PageStyle/withSnackbar';
import ConfirmDialog from '../ConfirmDialog';
var urlStart = KbApiMethod("AssignKit");
var urlKit = KbApiMethod("Kits");
import KitSearchDialog from '../Kits/ViewKits/SelectKit/KitSearchDialog';
import TextField from '@material-ui/core/TextField';
import StyledPanel from "src/components/PageStyle/StyledPanel";
import { withStyles } from '@material-ui/core/styles';
import PageTitle from "../PageTitle";


const styles = (theme: any) => ({
     root: {
          marginTop: 24
     },
     label: {
          textAlign: "right" as 'right',
          marginBottom: 0 + ' !important',
          paddingRight: 10 + 'px'
     },
     labelRoot: {
          fontSize: 18,
          fontWeight: 600
     },
     button: {
          width: '100%',
     },
     editButton: {
          width: '100%',
          marginLeft: '16px'
     },
     formControl:
     {
          width: '100%'
     }
});

interface IAssignKitsToLocaleState {
     data: any;
     kitId: number;
     kitName: string;
     kitType: string;
     isSimplekitType: boolean;
     assignedLocales: number[];
     excludedLocales: number[];
     showPublishConfirm: boolean;
     showSaveConfirm: boolean;
     isReadyToPublish: boolean;
     open: boolean;
}

interface IAssignKitsToLocaleProps {

     showAlert: any,
     classes: any,
     history: any
}

class AssignKitsToLocale extends React.Component<IAssignKitsToLocaleProps, IAssignKitsToLocaleState>
{
     constructor(props: IAssignKitsToLocaleProps) {
          super(props);

          this.state = {
               data: [],
               kitId: 0,
               kitName: "",
               kitType: "",
               isSimplekitType: false,
               assignedLocales: [],
               excludedLocales: [],
               showPublishConfirm: false,
               showSaveConfirm: false,
               isReadyToPublish: true,
               open: false
          }
          this.onkitSelected = this.onkitSelected.bind(this);
     }

     componentDidMount() {
          var pathArray = window.location.href.split('/');
          pathArray = pathArray.reverse();
          const kitIdPassed = parseInt(pathArray[0]);
          if (isNaN(kitIdPassed)) {
               this.props.showAlert("Please select kit.", "info");
          }
          else {
               this.setState({ kitId: kitIdPassed }, () => {
                    this.loadData();
                    this.loadKit();
               });
          }
     }

     onkitSelected(row: any) {
          this.setState({
               kitId: row.kitId,
               kitName: row.description,
               open: false,
          }, this.loadDataAfterKitSelected);

     }

     loadDataAfterKitSelected() {
          let path = `/AssignKits/` + this.state.kitId.toString();
          this.props.history.push(path);
          this.loadData();
          this.loadKit();
     }

     toggleShowPublishConfirm = () => {
          this.setState({ showPublishConfirm: !this.state.showPublishConfirm })
     }

     toggleShowSaveConfirm = () => {
          this.setState({ showSaveConfirm: !this.state.showSaveConfirm })
     }

     toggleLocaleExcluded = (localeId: number) => {
          const oldExcludedLocales = this.state.excludedLocales;
          const { assignedLocales } = this.state;
          let excludedLocales;

          if (oldExcludedLocales.includes(localeId)) {
               excludedLocales = oldExcludedLocales.filter((id) => id !== localeId);
          }
          else {
               excludedLocales = [...oldExcludedLocales, localeId];
               if (assignedLocales.includes(localeId)) {
                    this.toggleLocaleAssigned(localeId);
               }
          }
          this.setState({ excludedLocales });
     }

     toggleLocaleAssigned = (localeId: number) => {
          const oldAssignedLocales = this.state.assignedLocales;
          const { excludedLocales } = this.state;
          let assignedLocales;

          if (oldAssignedLocales.includes(localeId)) {
               assignedLocales = oldAssignedLocales.filter((id) => id !== localeId);
          }
          else {
               assignedLocales = [...oldAssignedLocales, localeId];
               if (excludedLocales.includes(localeId)) {
                    this.toggleLocaleExcluded(localeId);
               }
          }
          this.setState({ assignedLocales });
     }

     loadKit = () => {
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
                    if (data.length > 0)
                         this.setState({ kitName: data[0].description, kitType: data[0].kitType }

                         );
                    ;
                    if (data[0].kitType == 1) {

                         this.setState({ isSimplekitType: true });
                    }
               }).catch((error) => {

                    this.props.showAlert("Error in displaying data.", "error")
               });
     }

     selectKit = () => {
          this.setState({ open: true });
     }
     onClose = () => {
          this.setState({ open: false });
     }
     loadData = () => {
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

     parseData = (data: any) => {
          this.setState({ assignedLocales: [], excludedLocales: [] });
          var parsed_data = [];
          var isAssignedToOneLocation = false;
          var disablePublish = false;
          var map = {};

          for (var i = 0; i < data.length; i++) {
               data[i].childs = [];
               map[data[i].localeId] = data[i];


               if ((data[i].isAssigned && data[i].statusId != 3)) {
                    if (data[i].statusId != 5) {
                         disablePublish = true;
                         this.setState({ isReadyToPublish: false })
                    }
               }

               if (data[i].isAssigned) {
                    this.toggleLocaleAssigned(data[i].localeId);
                    isAssignedToOneLocation = true;
               }

               if (data[i].isExcluded) {
                    this.toggleLocaleExcluded(data[i].localeId);
                    isAssignedToOneLocation = true;
               }

               if (data[i].localeTypeId == 1) {
                    parsed_data.push(data[i]);
               }

               if (data[i].parentLocaleId == -1 || data[i].parentLocaleId == undefined || data[i].parentLocaleId == null) {
                    continue;
               }

               map[data[i].parentLocaleId].childs.push(data[i]);
          }

          var allPublished = data.filter(function (el: any) {
               return el.statusId != 5 && (el.isAssigned || el.isExcluded)
          });

          if (allPublished.length == 0 && isAssignedToOneLocation) {
               this.setState({ isReadyToPublish: false })
          }
          else if (!disablePublish && isAssignedToOneLocation) {
               this.setState({ isReadyToPublish: true })
          }
          else if (!isAssignedToOneLocation) {
               this.setState({ isReadyToPublish: false })
          }

          this.setState({
               data: parsed_data
          });
     }

     putData = (dest: Array<any>, data: Array<any>) => {
          const { excludedLocales, assignedLocales } = this.state;
          for (var i = 0; i < data.length; i++) {
               var item = JSON.parse(JSON.stringify(data[i]));
               delete item.childs;
               const isAssigned = assignedLocales.includes(item.localeId);
               const isExcluded = excludedLocales.includes(item.localeId);

               if (isAssigned || isExcluded)
                    dest.push({
                         ...item,
                         isAssigned,
                         isExcluded
                    });
               this.putData(dest, data[i].childs);
          }
     }
     publishChanges = () => {
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
                    this.toggleShowPublishConfirm();
               })
               .catch((error) => {

                    if (error.response.status == "412") {
                         this.props.showAlert("Kit properties not set for all assigned locales.", "error");
                    }
                    else if (error.response.status == "404") {
                         this.props.showAlert("Kit properties not set for any assigned locales.", "error");
                    }
                    else {
                         this.props.showAlert("Error in queueing Kit.", "error");
                    }

                    this.toggleShowPublishConfirm();
               })
     }

     saveData = () => {
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
                    this.toggleShowSaveConfirm();
               }).catch(error => {

                    this.props.showAlert("Error in saving data.", "error");
                    this.toggleShowSaveConfirm();
               });
     }

     render() {
          const { data } = this.state;
          const { isReadyToPublish } = this.state;

          return (
               <StyledPanel
                    header={
                         <PageTitle icon="format_list_bulleted">Assign Kit</PageTitle>
                    }
               >
                    <React.Fragment>
                         <Grid xs={12} md={6} item>
                         </Grid>
                         <Grid xs={12} md={6} item>
                         </Grid>
                         <KitSearchDialog
                              openPopUp={this.state.open}
                              closePopUp={this.onClose}
                              onkitSelect={this.onkitSelected}
                         />

                         <Grid container spacing={16}>
                              <Grid xs={12} md={12} item>
                              </Grid>
                              <Grid xs={12} md={1} item></Grid>
                              <Grid xs={12} md={2} item>
                                   <TextField
                                        disabled
                                        variant='outlined'
                                        label='Selected Kit'
                                        className='search-textfield'
                                        value={this.state.kitName}>
                                   </TextField>
                              </Grid>
                              <Grid xs={12} md={2} item>
                                   <Button variant="contained" color="primary" className={this.props.classes.button} onClick={() => this.selectKit()} >
                                        Select Kit
                                    </Button>
                              </Grid>
                              <Grid xs={12} md={3} item></Grid>
                              <Grid xs={12} md={2} item>
                                   <Button disabled={!isReadyToPublish} className={this.props.classes.button} variant="contained" color="primary" onClick={this.toggleShowPublishConfirm} >
                                        Publish
                                   </Button>
                              </Grid>
                              <Grid xs={12} md={2} item>
                                   <Button variant="contained" color="primary" className={this.props.classes.button} onClick={this.toggleShowSaveConfirm} >
                                        Save Changes
                                   </Button>
                              </Grid>
                         </Grid>
                         <Grid container spacing={16}>
                              <Grid xs={12} md={12} item>
                              </Grid>
                         </Grid>
                         <AssignKitsTreeTable
                              toggleLocaleAssigned={this.toggleLocaleAssigned}
                              toggleLocaleExcluded={this.toggleLocaleExcluded}
                              assignedLocales={this.state.assignedLocales}
                              excludedLocales={this.state.excludedLocales}
                              kitId={this.state.kitId}
                              isSimplekitType={this.state.isSimplekitType}
                              disabled={false}
                              excludeDisabled={false}
                              data={data} />

                         <ConfirmDialog
                              message="Are you sure you want to save your changes?"
                              open={this.state.showSaveConfirm}
                              onConfirm={this.saveData}
                              onClose={this.toggleShowSaveConfirm}
                         />

                         <ConfirmDialog
                              message="Are you sure you want to publish?"
                              open={this.state.showPublishConfirm}
                              onConfirm={this.publishChanges}
                              onClose={this.toggleShowPublishConfirm}
                         />
                    </React.Fragment>
               </StyledPanel>
          );
     }
}

export default withStyles(styles, { withTheme: true })(withSnackbar(AssignKitsToLocale));
