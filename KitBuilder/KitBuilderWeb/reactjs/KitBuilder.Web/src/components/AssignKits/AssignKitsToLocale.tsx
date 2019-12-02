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
import LocaleSearchDialog from '../Locales/SearchLocale/LocaleSearchDialog';
import TextField from '@material-ui/core/TextField';
import StyledPanel from "src/components/PageStyle/StyledPanel";
import { withStyles } from '@material-ui/core/styles';
import PageTitle from "../PageTitle";
import FormControl from '@material-ui/core/FormControl';
import { connect } from 'react-redux';
import { KitTreeState } from 'src/redux/reducers/kitTreeReducer';
import { Dispatch } from 'redux';

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
          fontSize: 16
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
     copyFromLocale: number;
     copyToLocales: number[];
     showPublishConfirm: boolean;
     showSaveConfirm: boolean;
     showCopyConfirm: boolean;
     isReadyToPublish: boolean;
     kitSearchDialogOpen: boolean;
     localeSearchDialogOpen: boolean;
     localeId: number;
     localeName: string;
     chainId: number;
     regionId: number;
     metroId: number;
     storeId: number;
     localeSelected: boolean;
     kitTree: KitTreeState;
}

interface IAssignKitsToLocaleProps {

     showAlert: any,
     classes: any,
     history: any,
     updateRedux(kitId: number, localeId: number, data:[], resetLocale: boolean, chainId: number, regionId: number, metroId: number, storeId: number): void;
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
               copyFromLocale: 0,
               copyToLocales: [],
               showPublishConfirm: false,
               showSaveConfirm: false,
               showCopyConfirm: false,
               isReadyToPublish: true,
               kitSearchDialogOpen: false,
               localeSearchDialogOpen: false,
               localeId: 0,
               localeName: "",
               chainId: 0,
               regionId: 0,
               metroId: 0,
               storeId: 0,
               localeSelected: false,
               kitTree: { resetLocale: false }
          }
          this.onkitSelected = this.onkitSelected.bind(this);
          this.onlocaleSelected = this.onlocaleSelected.bind(this);
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
               kitSearchDialogOpen: false,
               localeSelected: false,
          }, this.loadDataAfterKitSelected);

     }

     onlocaleSelected(row: any) {
          this.setState({
               localeId: row.localeId,
               localeName: row.localeName,
               chainId: row.chainId,
               regionId: row.regionId,
               metroId: row.metroId,
               storeId: row.storeId,
               localeSearchDialogOpen: false,
               localeSelected: true,
          }, this.loadDataAfterKitSelected);

     }
     loadDataAfterKitSelected() {  
          let path = `/AssignKits/` + this.state.kitId.toString();
          this.props.history.push(path);
          this.loadData();
          this.props.updateRedux(this.state.kitId, this.state.localeId, this.state.data, this.state.localeSelected, this.state.chainId, this.state.regionId, this.state.metroId, this.state.storeId); 
          this.loadKit();
     }

     toggleShowPublishConfirm = () => {
          this.setState({ showPublishConfirm: !this.state.showPublishConfirm })
     }

     toggleShowSaveConfirm = () => {
          this.setState({ showSaveConfirm: !this.state.showSaveConfirm })
     }

     toggleShowCopyConfirm = () => {
          this.setState({ showCopyConfirm: !this.state.showCopyConfirm })
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

     toggleCopyToLocales = (localeId: number) => {
          const oldCopyToLocales = this.state.copyToLocales;
          let copyToLocales;

          if (oldCopyToLocales.includes(localeId)) {
               copyToLocales = oldCopyToLocales.filter((id) => id !== localeId);
          }
          else {
               copyToLocales = [...oldCopyToLocales, localeId];
          }
          this.setState({ copyToLocales });
     }

     toggleCopyFromLocale = (localeId: number) => {
          if (this.state.copyFromLocale == localeId)
               this.setState({copyFromLocale: 0})
          else if (this.state.copyFromLocale == 0)
               this.setState({copyFromLocale: localeId})
          else
               this.props.showAlert("Only one Copy From location can be selected.", "error")
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
                    else
                    {
                         this.setState({ isSimplekitType: false });
                    }
               }).catch((error) => {

                    this.props.showAlert("Error in displaying data.", "error")
               });
     }

     selectKit = () => {
          this.setState({ kitSearchDialogOpen: true });
     }
     onKitClose = () => {
          this.setState({ kitSearchDialogOpen: false });
     }
     onLocaleClose = () => {
          this.setState({ localeSearchDialogOpen: false });
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

     selectLocale = () => {
          this.setState({ localeSearchDialogOpen: true });
     }
     onLocaleSearchClose = () => {
          this.setState({ localeSearchDialogOpen: false });
     }
     loadLocaleData = () => {
          const { localeId } = this.state;
          let url = urlStart;

          url = url + '/' + localeId;
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

          if ( data.length == 0) {
               this.props.showAlert("There are no venues in the system.Please add venues from Icon.", "error");
               return;
          }

          for (var i = 0; i < data.length; i++) {
               data[i].childs = [];
               map[data[i].localeId] = data[i];

               if ((data[i].isAssigned && data[i].statusId != 3)) {
                    if (data[i].statusId != 5 && data[i].statusId != 7 && data[i].statusId != 9) {
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

          if (!disablePublish && isAssignedToOneLocation) {
               this.setState({ isReadyToPublish: true })
          }
          else {
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
                    else if (error.response.status == "409") {
                         this.props.showAlert(error.response.data, "error");
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

     copyData = () => {
          var { kitId } = this.state;
          //var dest: Array<any>;

          this.state.copyFromLocale
          this.state.copyToLocales
          let toLocalesdest = JSON.stringify(this.state.copyToLocales);
          let urlKitCopy = urlKit + "/" + kitId + "/" + "CopyKitProperties/" + this.state.copyFromLocale
         
          var headers = {
               'Content-Type': 'application/json', "Access-Control-Allow-Origin": "*"
          }

          axios.post(urlKitCopy, toLocalesdest,
               {
                    headers: headers
               }).then(response => {

                    this.props.showAlert("Kit properties copied succesfully.", "success")
                    this.setState({ copyFromLocale: 0, copyToLocales: [] })
                    this.loadData();
                    this.toggleShowCopyConfirm();
               }).catch(error => {

                    this.props.showAlert("Error in saving data.", "error");
                    this.toggleShowCopyConfirm();
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
                         <KitSearchDialog
                              openPopUp={this.state.kitSearchDialogOpen}
                              closePopUp={this.onKitClose}
                              onkitSelect={this.onkitSelected}
                         />

                         <Grid container spacing={16}>
                              <Grid xs={12} md={12} item>
                              </Grid>
                              <Grid xs={12} md={"auto"} item></Grid>
                              <Grid xs={12} md={3} item>
                              <FormControl className={this.props.classes.formControl}>
                                   <TextField
                                        disabled
                                        variant='outlined'
                                        label='Selected Kit'
                                        className='search-textfield'
                                        InputLabelProps={{ shrink: true,
                                             FormLabelClasses: {
                                                 root: this.props.classes.labelRoot
                                               } }}
                                        value={this.state.kitName}>
                                   </TextField>
                                   </FormControl>
                              </Grid>
                              <Grid xs={12} md={2} item>
                                   <Button variant="contained" color="primary" className={this.props.classes.button} onClick={() => this.selectKit()} >
                                        Select Kit
                                    </Button>
                              </Grid>
                              <Grid xs={2} md={1} item>
                              </Grid>
                              <LocaleSearchDialog
                              openPopUp={this.state.localeSearchDialogOpen}
                              closePopUp={this.onLocaleClose}
                              onlocaleSelect={this.onlocaleSelected}
                              />
                              <Grid xs={12} md={3} item>
                              <FormControl className={this.props.classes.formControl}>
                                   <TextField
                                        disabled
                                        variant='outlined'
                                        label='Selected Locale'
                                        className='search-textfield'
                                        InputLabelProps={{ shrink: true,
                                             FormLabelClasses: {
                                                 root: this.props.classes.labelRoot
                                               } }}
                                        value={this.state.localeName}>
                                   </TextField>
                                   </FormControl>
                              </Grid>
                              <Grid xs={12} md={2} item>
                                   <Button variant="contained" color="primary" disabled={this.state.kitName ==""} className={this.props.classes.button} onClick={() => this.selectLocale()} >
                                        Select Locale
                                    </Button>
                              </Grid>   
                              <Grid xs={2} md={5} item>
                              </Grid>    
                              <Grid xs={12} md={2} item>
                                   <Button disabled={!isReadyToPublish} className={this.props.classes.button} variant="contained" color="primary" onClick={this.toggleShowPublishConfirm} >
                                        Publish
                                   </Button>
                              </Grid>
                              <Grid xs={12} md={2} item>
                                   <Button disabled={!(this.state.copyFromLocale > 0 && this.state.copyToLocales.length > 0)} className={this.props.classes.button} variant="contained" color="primary" onClick={this.toggleShowCopyConfirm} >
                                        Copy
                                   </Button>
                              </Grid>
                              <Grid xs={12} md={2} item>
                                   <Button variant="contained" color="primary" className={this.props.classes.button} onClick={this.toggleShowSaveConfirm} >
                                        Save Assignments
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
                              toggleCopyFromLocale={this.toggleCopyFromLocale}
                              toggleCopyToLocales={this.toggleCopyToLocales}
                              copyFromLocale={this.state.copyFromLocale}
                              copyToLocales={this.state.copyToLocales}
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

                         <ConfirmDialog
                              message="Are you sure you want to copy kit properties?"
                              open={this.state.showCopyConfirm}
                              onConfirm={this.copyData}
                              onClose={this.toggleShowCopyConfirm}
                         />
                    </React.Fragment>
               </StyledPanel>
          );
     }
}
     
const mapStateToProps = (state: any) => ({
     kitTree: state.kitTree,
 });
 
 const mapDispatchToProps = (dispatch: Dispatch) => ({
     updateRedux: (selectedKitId: number, resetlocaleId: number, localeTreedata:[], resetLocale: boolean, chainId: number, regionId: number, metroId: number, storeId: number) => dispatch({ type: "RESET_LOCALE", payload: {selectedKitId, resetlocaleId, localeTreedata, resetLocale, chainId, regionId, metroId, storeId}}),
 })
 
export default connect(mapStateToProps, mapDispatchToProps)(withStyles(styles, { withTheme: true })(withSnackbar(AssignKitsToLocale)));