import * as React from 'react';
import { Grid, Dialog, DialogActions, DialogContent, DialogTitle, Button, Radio, RadioGroup, FormControlLabel, FormControl, FormLabel, TextField, Snackbar } from '@material-ui/core';
import SearchInstructionLists from './SearchInstructionLists';
import DisplayInstructionsLists from './DisplayInstructionLists';
import "@babel/polyfill";
import axios from 'axios';
const hStyle = { color: 'red' };
const sucesssStyle = { color: 'blue' };
import { KbApiMethod } from '../helpers/kbapi'
import Swal from 'sweetalert2'

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
}

interface IInstructionListsPageProps {
}

export class InstructionListsPage extends React.Component<IInstructionListsPageProps, IInstructionListsPageState>
{
     constructor(props: any) {
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
          }

          this.onChange = this.onChange.bind(this);
          this.renderEditable = this.renderEditable.bind(this);
          this.onInstructionDelete = this.onInstructionDelete.bind(this);
          this.onSearch = this.onSearch.bind(this);
          this.onAddNewList = this.onAddNewList.bind(this);
          this.onAddMember = this.onAddMember.bind(this);
          this.deleteInstruction = this.deleteInstruction.bind(this);
          this.InstructionNameChange = this.InstructionNameChange.bind(this);
          this.onSaveChanges = this.onSaveChanges.bind(this);
          this.onPublishChanges = this.onPublishChanges.bind(this);
     }

     onChange(event: any) {
          this.setState({ selectedInstructionTypeIdvalue: event.target.value });
          this.setState({
               error: null
          })

          this.setState({
               message: null
          })
     }

     InstructionNameChange(event: any) {
          this.setState({ currentInstructionTypeValue: event.target.value });
     }
     componentDidMount() {
          this.getInstructionTypes();
     }

     onInstructionDelete(row: any) {
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

     renderEditable(cellInfo: any) {
          return (
               <div
                    style={{ backgroundColor: "#fafafa" }}
                    contentEditable
                    suppressContentEditableWarning
                    onBlur={e => {
                         const data = [...this.state.instructionList];
                         data[cellInfo.index][cellInfo.column.id] = e.target.innerHTML;
                         this.setState({ instructionList: data });
                    }}
                    dangerouslySetInnerHTML={{
                         __html: this.state.instructionList[cellInfo.index][cellInfo.column.id]
                    }}
               />
          );
     }

     getInstructionTypes() {

          fetch(urlStart)
               .then(response => {
                    return response.json();
               }).catch(error => {
                    console.log(error);
                    this.setState({
                         instructionListDto: ["error"]
                    });
               })
               .then(data =>
                    this.setState({
                         instructionListDto: data
                    }));
     }
     Refresh(name: any) {
          fetch(urlStart)
               .then(response => {
                    return response.json();
               }).catch(error => {
                    console.log(error);
               })
               .then(data =>
                    this.setState({
                         instructionListDto: data
                    }, () => { this.setControl(name) }));
     }

     setControl(name: any) {
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

     onSearch() {
          if (this.state.selectedInstructionTypeIdvalue == "") {
               this.setState({
                    error: "Please Select Instruction List Name."
               });

               this.setState({
                    message: null
               })
               return;

          }

          var result = this.state.instructionListDto.find(obj => {
               return obj.InstructionListId === this.state.selectedInstructionTypeIdvalue
          })
          this.setState({
               currentInstructionTypeValue: result.Name
          });
          this.setState({
               instructionTypeName: result.InstructionTypeName
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
                         error: error.response.data
                    });

                    this.setState({
                         message: null
                    })
               });
     }

     onAddMember() {
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
               instructionList: data
          })
     }


     deleteInstruction() {
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
                              error: null
                         })

                         this.setState({
                              message: null
                         })
                    }
               });
     }

     deleteInstructionData() {
          var headers =
          {
               'Content-Type': 'application/json', "Access-Control-Allow-Origin": "*"
          }
          var urlParam = this.state.selectedInstructionTypeIdvalue;

          if (this.state.selectedInstructionTypeIdvalue == "") {
               this.setState({
                    error: "Please Select Instruction List Name."
               });
               this.setState({
                    message: null
               })
               return;

          }

          var urlDelete = urlStart + urlParam;

          axios.delete(urlDelete, { headers },
          ).then(() => {
               this.setState({
                    message: "Data Deleted Successfully."
               })
               this.setState({
                    error: null
               })
               this.setState({
                    selectedInstructionTypeIdvalue: ""
               })

               this.setState({
                    currentInstructionTypeValue: ""
               }, () => this.Refresh(this.state.currentInstructionTypeValue))

          })
               .catch((error) => {
                    console.log(error);
                    this.setState({
                         error: "Error in Deleting Instruction List."
                    })
                    this.setState({
                         message: null
                    })
               })
     }
     onAddNewList() {
          this.setState({
               dialogOpen: true,
               newInstructionList: {
                    InstructionTypeId: "0",
                    Name: ""
               }
          });
     }
     
     validateData(row: any) {
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

     onSaveChanges() {
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
                              message: null
                         })
                         this.setState({
                              error: error
                         })
                         return ;
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
                                   message: null
                              })
                              this.setState({
                                   error: error
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
                              message: "Data Saved Sucessfully."
                         })
                         this.setState({
                              error: null
                         })
                         this.insertMembers(dataInsert, urlAdd, headers);
                    })
                    .catch((error) => {
                         console.log(error);
                         this.setState({
                              error: "Error in Saving new Instruction List and members."
                         })
                         this.setState({
                              message: null
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

     saveInstructionName() {

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

     insertMembers(dataInsert: any[], urlAdd: any, headers: any) {
          axios.post(urlAdd, JSON.stringify(dataInsert),
               {
                    headers: headers
               })
               .then(() => {
                    this.setState({
                         message: "Data Saved Sucessfully."
                    })
                    this.setState({
                         error: null
                    })
                    this.saveInstructionName();
               })
               .catch((error) => {
                    console.log(error);
                    this.setState({
                         error: "Error in Saving new Instruction List and members."
                    })
                    this.setState({
                         message: null
                    })
               })

     }
     onPublishChanges() {
     }

     handleDialogClose = () => {
          this.setState({
               dialogOpen: false
          });
     };

     handleDialogSave = () => {
          let newInstructionList = this.state.newInstructionList;

          if (newInstructionList.Name == "") {
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

          this.setState({
               dialogOpen: false
          });

          newInstructionList.InstructionTypeId = Number(newInstructionList.InstructionTypeId);

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
                    this.setState({
                         message: null
                    })
                    this.Refresh(newInstructionList.Name)

               })
               .catch((error) => {
                    this.setState({
                         error: error.response.data
                    }
                    )
                    this.setState({
                         message: null
                    })
                    return;
               });

     };

     handleInstructionTypeIdChange = (event: any) => {
          this.setState({
               newInstructionList: {
                    InstructionTypeId: event.target.value,
                    Name: this.state.newInstructionList.Name
               }
          });
     };

     handleInstructionNameChange = (event: any) => {
          this.setState({
               newInstructionList: {
                    InstructionTypeId: this.state.newInstructionList.InstructionTypeId,
                    Name: event.target.value
               }
          });
     };

     render() {
          const { instructionListDto } = this.state;
          const vertical = "bottom";
          const horizontal = "center";

          return (

               <React.Fragment>
                    <Grid container justify="center">
                         <Grid container md={12} justify="center">
                              <div className="error-message" >
                                   <span style={hStyle}> {this.state.error}</span>
                              </div>
                         </Grid>
                         <Grid container md={12} justify="center">
                              <div className="Suncess-message" >
                                   <span style={sucesssStyle}> {this.state.message}</span>
                              </div>
                         </Grid>
                    </Grid>
                    <SearchInstructionLists
                         onChange={this.onChange}
                         options={instructionListDto}
                         onSearch={this.onSearch}
                         onAddNewList={this.onAddNewList}
                         value={this.state.selectedInstructionTypeIdvalue}
                    />


                    <DisplayInstructionsLists
                         data={this.state.instructionList}
                         renderEditable={this.renderEditable}
                         onDelete={this.onInstructionDelete}
                         instructionValue={this.state.currentInstructionTypeValue}
                         instructionTypeName={this.state.instructionTypeName}
                         onAddMember={this.onAddMember}
                         deleteInstruction={this.deleteInstruction}
                         InstuctionNameChange={this.InstructionNameChange}
                         onPublishChanges={this.onPublishChanges}
                         onSaveChanges={this.onSaveChanges} />

                    <Dialog
                         open={this.state.dialogOpen}
                         onClose={this.handleDialogClose}
                         aria-labelledby="form-dialog-title"
                    >
                         <DialogTitle id="form-dialog-title">Add New Instruction List</DialogTitle>
                         <DialogContent>
                              <FormControl >
                                   <FormLabel>Type</FormLabel>
                                   <RadioGroup
                                        aria-label="type"
                                        name="type"
                                        value={this.state.newInstructionList.InstructionTypeId}
                                        onChange={this.handleInstructionTypeIdChange}
                                        row
                                   >
                                        <FormControlLabel
                                             value="1"
                                             control={<Radio color="primary" />}
                                             label="Cooking"
                                        />
                                        <FormControlLabel
                                             value="2"
                                             control={<Radio color="primary" />}
                                             label="General"
                                        />
                                   </RadioGroup>
                              </FormControl>
                              <br />
                              <FormControl >
                                   <TextField
                                        id="standard-name"
                                        label="Name"
                                        value={this.state.newInstructionList.Name}
                                        onChange={this.handleInstructionNameChange}
                                   />
                              </FormControl>
                         </DialogContent>
                         <DialogActions>
                              <Button onClick={this.handleDialogClose} color="primary">
                                   Cancel
                         </Button>
                              <Button onClick={this.handleDialogSave} color="primary">
                                   Save
                         </Button>
                         </DialogActions>
                    </Dialog>

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