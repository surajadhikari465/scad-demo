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

var urlStart = KbApiMethod("InstructionList");

interface IInstructionListsPageState {
     error: any,
     message: any
     isLoaded: boolean,
     instructionTypes: Array<any>,
     selectedInstructionTypeIdvalue: string,
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
}

export class InstructionListsPage extends React.PureComponent<IInstructionListsPageProps, IInstructionListsPageState>
{
     constructor(props: IInstructionListsPageProps) {
          super(props);

          this.state = {
               error: null,
               message: null,
               isLoaded: false,
               instructionTypes: [],
               selectedInstructionTypeIdvalue: "",
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

     onChange = (event: React.ChangeEvent<HTMLInputElement>) => {
          this.setState({ 
               selectedInstructionTypeIdvalue: event.target.value,
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

     onInstructionDelete = (row: any) => {
          let data = this.state.instructionList;
          if (row.row._original.instructionListId == 0) {
               for (let i = 0; i < data.length; i++) {
                    if (data[i] != row.row._original) {
                         continue;
                    }

                    data.splice(i, 1);
                    break;
               }

               this.setState({
                    instructionList: data
               });
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

                    this.setState({
                         instructionList: data
                    });
               })
                    .then(() => this.setState({ message: "Instruction Member Deleted Sucessfully" }))
                    .then(() => this.setState({ error: null }))
                    .catch((error) => {
                         console.log("Error in deleting Instruction list member");
                         this.setState({
                              error: "Error in deleting Instruction List Member."
                         })
                         this.setState({
                              message: null
                         })
                         return;
                    });
          }


     }

     renderEditable = (cellInfo: any) => {
          return (
               <div
                    contentEditable
                    suppressContentEditableWarning
                    onBlur={e => {
                         const data = [...this.state.instructionList];
                         data[cellInfo.index][cellInfo.column.id] = e.target.innerHTML;
                         this.setState({ instructionList: data });
                    }}
                    dangerouslySetInnerHTML={{
                         __html: `<center>${this.state.instructionList[cellInfo.index][cellInfo.column.id]}</center>`
                    }}
               />
          );
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
                    this.setState({
                         error: "Error loading Instruction Lists."
                    });
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
                    console.log(error);
                    this.setState({
                         error: "Error refreshing Instruction Lists."
                    });
               });
     }

     setControl = (name: any) => {
          var result = this.state.instructionListDto.find(obj => {
               return obj.Name === name
          })
          if (typeof result === "undefined" || result == null) {
               this.setState({ selectedInstructionTypeIdvalue: "" });
               this.setState({ instructionTypeName: "" });
               this.setState({ instructionList: [] });
          }
          else {
               this.setState({ selectedInstructionTypeIdvalue: result.InstructionListId }, () => { this.onSearch() });
          }


     }

     onSearch = () => {
          if (this.state.selectedInstructionTypeIdvalue == "") {
               this.setState({
                    error: "Please Select Instruction List Name.",
                    message: null,
               });
               return;
          }

          var result = this.state.instructionListDto.find(obj => {
               return obj.InstructionListId === this.state.selectedInstructionTypeIdvalue
          })
          this.setState({
               currentInstructionTypeValue: result.Name,
               instructionTypeName: result.InstructionTypeName,
               isLoaded: true,
          });

          var urlParam = this.state.selectedInstructionTypeIdvalue;

          var url = urlStart + urlParam + '/InstructionListMembers';
          fetch(url)
               .then(response => {
                    return response.json();
               }).then(data => this.setState({
                    instructionList: data
               }))
               .then(() => {

                    this.setState({
                         error: null
                    })
               })
               .catch((error) => {
                    console.log(error.response.data);
                    this.setState({
                         error: error.response.data,
                         message: null,
                    })
               });
     }

     onAddMember = () => {
          let data = this.state.instructionList;
          if (navigator.userAgent.indexOf("Firefox") != -1) {
               data.push({
                    instructionListId: "0", instructionListMemberId: "0", group: "Group", sequence: "0", member: "Member", action: ""
               });

          }
          else {
               data.push({
                    instructionListId: "0", instructionListMemberId: "0", group: "", sequence: "", member: "", action: ""
               });

          }
          this.setState({
               instructionList: data,
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

          if (this.state.selectedInstructionTypeIdvalue == "") {
               this.setState({
                    error: "Please Select Instruction List Name.",
                    message: null,
               })
               return;

          }

          var urlDelete = urlStart + urlParam;

          axios.delete(urlDelete, { headers },
          ).then(() => {
               this.setState({
                    message: "Data Deleted Successfully.",
                    error: null,
                    selectedInstructionTypeIdvalue: "",
                    currentInstructionTypeValue: "",
                    isLoaded: false,
               }, () => this.Refresh(this.state.currentInstructionTypeValue))
          })
               .catch((error) => {
                    console.log(error);
                    this.setState({
                         error: "Error in Deleting Instruction List.",
                         message: null,
                    })
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
                         this.setState({
                              message: null,
                              error: error,
                         });
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
                              this.setState({
                                   message: null,
                                   error: error,
                              })
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
                         this.setState({
                              message: "Data Saved Sucessfully.",
                              error: null,
                              isSaveDisabled: true,
                              isPublishDisabled: false,
                         })
                         this.insertMembers(dataInsert, urlAdd, headers);
                    })
                    .catch((error) => {
                         console.log(error);
                         this.setState({
                              error: "Error in Saving new Instruction List and members.",
                              message: null,
                         })

                         return;
                    })
          }
          else if (dataInsert.length > 0) {
               this.insertMembers(dataInsert, urlAdd, headers);
          }

          else {
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
                    this.setState({
                         error: null
                    })
                    this.setState({
                         message: "Data Saved Sucessfully."
                    })
               })
               .catch((error) => {
                    console.log(error);
                    this.setState({
                         error: "Error in Saving data."
                    })

                    this.setState({
                         message: null
                    })
                    return;
               });
     }

     insertMembers = (dataInsert: any[], urlAdd: any, headers: any) => {
          axios.post(urlAdd, JSON.stringify(dataInsert),
               {
                    headers: headers
               })
               .then(() => {
                    this.setState({
                         message: "Data Saved Sucessfully.",
                         error: null,
                    })
                    this.saveInstructionName();
               })
               .catch((error) => {
                    console.log(error);
                    this.setState({
                         error: "Error in Saving new Instruction List and members.",
                         message: null,
                    })
               })

     }
     onPublishChanges = () => {
          // TODO: Add logic for publishing data
          this.setState({ isPublishDisabled: true, message: 'List Published Successfully.' });
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
               Name: name,
               InstructionTypeId: Number(type),
          }

          var headers = {
               'Content-Type': 'application/json', "Access-Control-Allow-Origin": "*"
          }
          axios.post(urlStart, JSON.stringify(newInstructionList),
               {
                    headers: headers
               }).then(response => {
                    if (response.status === 409) {
                         this.setState({
                              error: "Instruction List with this name alreadys exists."
                         })
                         this.setState({
                              message: null
                         })
                         return;
                    }

                    this.setState({
                         message: null
                    })

                    this.Refresh(newInstructionList.Name)

               })
               .catch((error) => {
                    this.setState({
                         error: error.response.data,
                         message: null,
                    })
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
                         onDelete={this.onInstructionDelete}
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
export default InstructionListsPage;