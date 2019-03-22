import * as React from 'react';
import { Grid, Snackbar } from '@material-ui/core';
import SearchInstructionLists from './SearchInstructionLists';
import DisplayInstructionsLists from './DisplayInstructionLists';
import "@babel/polyfill";
import axios from 'axios';
const hStyle = { color: 'red' };
const sucesssStyle = { color: 'blue' };
import { KbApiMethod } from '../helpers/kbapi'
import Swal from 'sweetalert2'
import './style.css';
import PageStyle from './PageStyle';
import PageTitle from '../PageTitle';
import CreateEdiInstructionDialog from './CreateEditInstructionsDialog';
import EditableContent from '../EditableInput';
import { InstructionList } from 'src/types/InstructionList';
import withSnackbar from '../PageStyle/withSnackbar';

var urlStart = KbApiMethod("InstructionList");

interface IInstructionListsPageState {
     error: any,
     message: any
     isLoaded: boolean,
     instructionTypes: Array<any>,
     selectedInstructionTypeIdvalue: number,
     currentInstructionTypeValue: string,
     instructionListDto: Array<any>,
     instructionList: Array<any>,
     dialogOpen: boolean,
     newInstructionList: any,
     snackOpen: boolean,
     instructionTypeName: "",
     isEditInstructions: boolean,
     isSaveDisabled: boolean,
     isPublishDisabled: boolean,
}

interface IInstructionListsPageProps {
     showAlert: any,
}

class InstructionListsPage extends React.PureComponent<IInstructionListsPageProps, IInstructionListsPageState>
{
     constructor(props: IInstructionListsPageProps) {
          super(props);

          this.state = {
               error: null,
               message: null,
               isLoaded: false,
               instructionTypes: [],
               selectedInstructionTypeIdvalue: 0,
               currentInstructionTypeValue: "",
               instructionListDto: [],
               instructionList: [],
               dialogOpen: false,
               newInstructionList: {},
               snackOpen: false,
               instructionTypeName: "",
               isEditInstructions: false,
               isSaveDisabled: true,
               isPublishDisabled: true,
          }
     }

     setInstructionListHack = (data: Array<any>) => {
      
          // This is a hack to fix a bug in ReactTable
          // that is caused by the table using index as the key
          // for the rows.
          this.setState({
               instructionList: [],
          }, () => {
          this.setState({
               instructionList: data
          });});
     }

     onChange = (event: React.ChangeEvent<HTMLInputElement>) => {
          this.setState({ 
               selectedInstructionTypeIdvalue: parseInt(event.target.value),
               error: null,
               message: null,
          }, this.onSearch);
     }

     updateCurrentInstructionName = (name: string) => {
          this.setState({ currentInstructionTypeValue: name, isSaveDisabled: false }, this.handleDialogClose);
     }
     componentDidMount() {
          this.getInstructionTypes();
     }

     onMemberDelete = (row: any) => {
          let data = [...this.state.instructionList];
          if (row.row._original.instructionListId == 0) {
               for (let i = 0; i < data.length; i++) {
                    if (data[i] != row.row._original) {
                         continue;
                    }

                    data.splice(i, 1);
                    break;
               }
               this.setInstructionListHack(data);
          }

          else {
               var headers = {
                    'Content-Type': 'application/json', "Access-Control-Allow-Origin": "*"
               }

               var url = urlStart + row.row._original.instructionListId + '/InstructionListMember/' + row.row._original.instructionListMemberId;
               axios.delete(url, { headers },
               ).then(response => {
                    for (let i = 0; i < data.length; i++) {
                         if (data[i] != row.row._original) {
                              continue;
                         }

                         data.splice(i, 1);
                         break;
                    }

                    this.setInstructionListHack(data);
               })
                    .then(() => {
                         this.props.showAlert("Instruction Member Deleted Successfully.");
                         this.onSearch();
                    })
                    .catch((error) => {
                         this.props.showAlert("Error in deleting Instruction List Member.", "error");
                         return;
                    });
          }


     }

     handleRowChange = (value: string, index: any, id: any) => {
          const data = [...this.state.instructionList];
          data[index][id] = value;
          this.setState({ instructionList: data, isSaveDisabled: false });
     }

     renderEditable = (filter : (value : string) => string, emptyLabel = "Empty") => {
          return (cellInfo: any) => {
               return (
                    <EditableContent
                    isValid={filter}
                    emptyLabel = {emptyLabel}
                    value = {this.state.instructionList[cellInfo.index][cellInfo.column.id]} 
                    onChange ={(value: string) => this.handleRowChange(value, cellInfo.index, cellInfo.column.id)}
                    />
               );
          }
     }

     getInstructionTypes = () => {

          fetch(urlStart)
               .then(response => {
                    return response.json();
               })
               .then(data =>
                    this.setState({
                         instructionListDto: data
                    }))
               .catch(error => {
                    console.log(error);
                    this.props.showAlert("Error loading Instruction Lists.", "error");
               })

     }
     Refresh = (name: any) => {
          fetch(urlStart)
               .then(response => {
                    return response.json();
               })
               .then(data =>
                    this.setState({
                         instructionListDto: data
                    }, () => { this.setControl(name) }))
               .catch(error => {
                         this.props.showAlert("Error refreshing Instruction Lists.","error");
               });
     }

     setControl = (name: string) => {
          var result = this.state.instructionListDto.find((obj:InstructionList) => {
               return obj.name === name
          })
          if (typeof result === "undefined" || result == null) {
               this.setState({ selectedInstructionTypeIdvalue: 0 });
               this.setState({ instructionTypeName: "" });
               this.setState({ instructionList: [] });
          }
          else {
               
               this.setState({ selectedInstructionTypeIdvalue: result.instructionListId }, () => { this.onSearch() });
          }


     }

     onSearch = () => {
          if (this.state.selectedInstructionTypeIdvalue === 0) {
                    this.props.showAlert("Please Select Instruction List Name.")
               return;
          }

          var result = this.state.instructionListDto.find((obj: InstructionList) => {
               // @ts-ignore
               return obj.instructionListId === this.state.selectedInstructionTypeIdvalue
          })
          this.setState({
               currentInstructionTypeValue: result.name,
               instructionTypeName: result.instructionTypeName,
               isLoaded: true,
          });

          var urlParam = this.state.selectedInstructionTypeIdvalue;

          var url = urlStart + urlParam + '/InstructionListMembers';

          fetch(url)
               .then(response => {
                    return response.json();
               }).then(data => 
                       { 
                    this.setInstructionListHack(data)
                       }
               ).then(() => {

                    this.setState({
                         error: null
                    })
               })
               .catch((error) => {
                    this.props.showAlert(JSON.stringify(error.response.data), "error");
               });
     }

     onAddMember = () => {
          let data = [...this.state.instructionList];

               data.push({
                    instructionListId: 0, instructionListMemberId: 0, group: "", sequence: "", member: "", action: ""
               });

          this.setInstructionListHack(data);
          this.setState({
               isSaveDisabled: false,
          })
     }


     deleteInstruction = () => {
          this.setState({ dialogOpen: false, }, () => {
          Swal({
               title: 'Are you sure that you want to delete this Instructions list?',
               type: 'info',
               showCancelButton: true,
               confirmButtonColor: '#3085d6',
               cancelButtonColor: '#d33',
               confirmButtonText: 'Ok'
          })
               .then((result: any) => {
                    if (result.value) {
                         this.deleteInstructionData();
                    }
                    else {
                         this.setState({
                              error: null,
                              message: null,
                         });
                    }
               });
          });
     }

     deleteInstructionData = () => {
          var headers =
          {
               'Content-Type': 'application/json', "Access-Control-Allow-Origin": "*"
          }
          var urlParam = this.state.selectedInstructionTypeIdvalue;

          if (this.state.selectedInstructionTypeIdvalue === 0) {
                    this.props.showAlert("Please Select Instruction List Name.");
               return;

          }

          var urlDelete = urlStart + urlParam;

          axios.delete(urlDelete, { headers },
          ).then(() => {
               this.props.showAlert("Instruction List Deleted Successfully.", "success");
               this.setState({
                    selectedInstructionTypeIdvalue: 0,
                    currentInstructionTypeValue: "",
                    isLoaded: false,
               }, () => this.Refresh(this.state.currentInstructionTypeValue))
          })
               .catch((error) => {
                    console.log(error);
                    if (error.response.status === 409) {
                         this.props.showAlert("Instruction List is in use.", "error");
                     }
                    else{
                         this.props.showAlert("Error in Deleting Instruction List.", "error");
                    }
                   
               })
     }
     onAddNewList = () => {
          this.setState({
               dialogOpen: true,
               newInstructionList: {
                    InstructionTypeId: "0",
                    Name: ""
               },
               isEditInstructions: false,
          });
     }
     
     validateData = (row: any) => {
          var data;

          if(row.group.length>60)
          {
               return "Group length cannot be more than 60."
          }

          if(row.member.length>15)
          {
               return "Member length cannot be more than 15."
          }
          data = parseInt(row.sequence, 10);
          if( !(data > 0))
          {
             return "Sequence must be a number."
          }
          return  "";
     }

     onSaveChanges = () => {
          let data = this.state.instructionList;
          let dataUpdate = [];
          let dataInsert: any[] = [];
          var error = "";

          for (let i = 0; i < data.length; i++) {
               if (data[i].instructionListMemberId == 0) {
                    error = this.validateData(data[i]);

                    if (error == "") {
                         dataInsert.push(data[i]);
                    }
                    else {
                         this.props.showAlert(error, "error");
                         return;
                    }

               }
               else {
                    error = this.validateData(data[i]);
                    if (error == "") {
                         dataUpdate.push(data[i]);
                    }
                    else {
                         {
                              this.props.showAlert(error, "error");
                              return ;
                         }

                    }

               }
          }

          var headers = {
               'Content-Type': 'application/json', "Access-Control-Allow-Origin": "*"
          }
          var urlParam = this.state.selectedInstructionTypeIdvalue;
          var urlAdd = urlStart + urlParam + '/InstructionListMembers';
          var urlUpdate = urlStart + urlParam + '/InstructionListMembers';
  
          if (dataUpdate.length > 0) {
               axios.put(urlUpdate, JSON.stringify(dataUpdate),
                    {
                         headers: headers
                    })
                    .then(() => {
                         this.props.showAlert("Instruction List Saved Successfully.", "success");
                         this.setState({
                              isSaveDisabled: true,
                              isPublishDisabled: false,
                         })
                         this.insertMembers(dataInsert, urlAdd, headers);
                        
                    })
                    .catch((error) => {
                         this.props.showAlert("Error in Saving new Instruction List and members.", "error");
                         return;
                    })
          }
          else if (dataInsert.length > 0) {
               this.insertMembers(dataInsert, urlAdd, headers);
          }
          else
          {
               this.saveInstructionName();
          }
     };

     saveInstructionName = () => {

          var updatedInstructionList = {
               InstructionListId: this.state.selectedInstructionTypeIdvalue,
               Name: this.state.currentInstructionTypeValue
          }

          var url = urlStart + this.state.selectedInstructionTypeIdvalue;

          var headers = {
               'Content-Type': 'application/json', "Access-Control-Allow-Origin": "*"
          }
          axios.put(url, JSON.stringify(updatedInstructionList),
               {
                    headers: headers
               })
               .then(response => {

                    this.Refresh(this.state.currentInstructionTypeValue)
               })
               .then(() => {
                    this.props.showAlert("Instruction List Saved Sucessfully.", "success");
               })
               .catch((error) => {
                    this.props.showAlert("Error in Saving data.", "error");
                    return;
               });
     }

     insertMembers = (dataInsert: any[], urlAdd: any, headers: any) => {
          if(dataInsert.length > 0)
          {
          axios.post(urlAdd, JSON.stringify(dataInsert),
               {
                    headers: headers
               })
               .then(() => {
                    this.props.showAlert("Instruction List Saved Successfully.", "success")
                    this.setState({
                         isSaveDisabled: true,
                         isPublishDisabled: false,
                    })
                    this.saveInstructionName();
               })
               .catch((error) => {
                    this.props.showAlert("Error in Saving new Instruction List and members.", "error");
               })
          }
          else
          {
               this.saveInstructionName();
          }
     }
     onPublishChanges = () => {
          // TODO: Add logic for publishing data
          this.props.showAlert("List Published Successfully.", "success")
          this.setState({ isPublishDisabled: true });
     }

     handleDialogClose = () => {
          this.setState({
                    dialogOpen: false,
               });
     };

     handleCreateNewInstructionList = (name: string, type: string) => {
          if (name == "") {
               this.setState({
                    snackOpen: true
               });
               let self = this;
               setTimeout(function () {
                    self.setState({
                         snackOpen: false
                    });
               }, 3000);
               return;
          }

          this.handleDialogClose();

          const newInstructionList = 
          {
               name,
               instructionTypeId: Number(type),
          }

          var headers = {
               'Content-Type': 'application/json', "Access-Control-Allow-Origin": "*"
          }
          axios.post(urlStart, JSON.stringify(newInstructionList),
               {
                    headers: headers
               }).then(response => {
                    if (response.status === 409) {
                         this.props.showAlert("Instruction List with this name alreadys exists.", "error");
                         return;
                    }

                    this.setState({
                         message: null
                    })

                    this.Refresh(newInstructionList.name)

               })
               .catch((error) => {
                    this.props.showAlert(JSON.stringify(error.response.data), "error")
                    return;
               });

     };

     onEdit = () => {
          this.setState({ isEditInstructions: true, dialogOpen: true });
     }

     render() {
          const { instructionListDto } = this.state;
          const vertical = "bottom";
          const horizontal = "center";

          return (

               <React.Fragment>
                    <Grid container justify="center">
                         <Grid container justify="center">
                              <div className="error-message" >
                                   <span style={hStyle}> {this.state.error}</span>
                              </div>
                         </Grid>
                         <Grid container justify="center">
                              <div className="Suncess-message" >
                                   <span style={sucesssStyle}> {this.state.message}</span>
                              </div>
                         </Grid>
                    </Grid>
                    <PageStyle>
                    <PageTitle icon="format_list_bulleted">Instructions</PageTitle>
                    <SearchInstructionLists
                         className = 'search-container'
                         onChange={this.onChange}
                         options={instructionListDto}
                         onSearch={this.onSearch}
                         onEdit={this.onEdit}
                         onAddNewList={this.onAddNewList}
                         value={this.state.selectedInstructionTypeIdvalue}
                         type={this.state.instructionTypeName}
                    />


                    <DisplayInstructionsLists
                         data={this.state.instructionList}
                         renderEditable={this.renderEditable}
                         onMemberDelete={this.onMemberDelete}
                         instructionValue={this.state.currentInstructionTypeValue}
                         instructionTypeName={this.state.instructionTypeName}
                         onAddMember={this.onAddMember}
                         deleteInstruction={this.deleteInstruction}
                         onPublishChanges={this.onPublishChanges}
                         onSaveChanges={this.onSaveChanges}
                         isLoaded={this.state.isLoaded}
                         isSaveDisabled = {this.state.isSaveDisabled}
                         isPublishDisabled = {this.state.isPublishDisabled}
                          />
                    </PageStyle>

                    <CreateEdiInstructionDialog 
                         isOpen = { this.state.dialogOpen}
                         isEditInstructions = {this.state.isEditInstructions}
                         currentInstructionTypeValue = {this.state.currentInstructionTypeValue}
                         onCancel = {this.handleDialogClose}
                         updateCurrentInstructionName = {this.updateCurrentInstructionName}
                         createNewInstruction = {this.handleCreateNewInstructionList}
                         onDelete = {this.deleteInstruction}
                         onClose = {this.handleDialogClose}
                    />

                    <Snackbar
                         anchorOrigin={{ vertical, horizontal }}
                         open={this.state.snackOpen}
                         ContentProps={{
                              'aria-describedby': 'message-id',
                         }}
                         message={<span id="message-id">Please input Name!</span>}
                    />
               </React.Fragment>
          );
     }
}
export default withSnackbar(InstructionListsPage);