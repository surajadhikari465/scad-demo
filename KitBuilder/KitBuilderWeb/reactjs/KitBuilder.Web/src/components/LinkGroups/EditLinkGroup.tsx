import * as React from 'react'
//import Card from '@material-ui/core/Card';
//import CardContent from '@material-ui/core/CardContent';
import Grid from '@material-ui/core/Grid';
import ReactTable from 'react-table';
//import { CardHeader } from '@material-ui/core';
import DeleteIcon from '@material-ui/icons/Delete'
import SaveIcon from '@material-ui/icons/Save'
import CancelIcon from '@material-ui/icons/Cancel'
import axios from 'axios';
import { KbApiMethod } from '../helpers/kbapi';
import InstructionListPicker from './InstructionListPicker';
import TextField from '@material-ui/core/TextField';
import Button from '@material-ui/core/Button';
import withStyles from '@material-ui/core/styles/withStyles';


interface IState {
    InstructionsList: any,
    data: any,
    LinkGroupItems: Array<any>,
    LinkGroupName: string,
    LinkGroupDesc: string
}

interface IProps {
    classes: any,
    data: any,
    handleCancelClick(): void
}

const styles = (theme: any) => ({
    root: {
        flexGrow: 1
    },
    label: {
        fontSize: 20,
        textAlign: "right" as 'right',
        marginBottom: 0 + ' !important',
        paddingRight: 10 + 'px'
    },
    button: {
        margin: theme.spacing.unit,

    },
    IconLeftMargin: {
        marginLeft: '10px'
    },
    searchButtons: {
        width: '175px',
        marginLeft: '60px',
        marginTop: '10px'

    },
    actionButtons: {
        width: '175px'


    },
    formControl:
    {
        margin: theme.spacing.unit,
        minWidth: 120,
        width: '100%'
    },

});


export class EditLinkGroup extends React.Component<IProps, IState> {

    constructor(props: any) {
        super(props)
        this.loadCookingInstructionsList = this.loadCookingInstructionsList.bind(this);

        this.state = {
            InstructionsList: [],
            data: [],
            LinkGroupItems: [],
            LinkGroupName: " ",
            LinkGroupDesc: " "

        }
        this.handleChange = this.handleChange.bind(this);
        this.escFunction = this.escFunction.bind(this);
    }


    componentDidMount() {
        document.addEventListener("keydown", this.escFunction, false);
        this.loadCookingInstructionsList();
        
    }

    static getDerivedStateFromProps(Props: any, State: any) {
        if (Props.data !== State.data) {

            return {
                data: Props.data,
                LinkGroupItems: Props.data.linkGroupItemDto,
                LinkGroupName: Props.data.groupName,
                LinkGroupDesc: Props.data.groupDescription
            }
        }
        return null;
    }

    escFunction(event:any){
        if(event.keyCode === 27) {
          this.props.handleCancelClick();
        }
      }

    
      componentWillUnmount(){
        document.removeEventListener("keydown", this.escFunction, false);
      }

    loadCookingInstructionsList() {

        axios.get(KbApiMethod("InstructionListByType"), {
            params: {
                instructionListType: "Cooking"
            }
        })
            .then(res => {
                this.setState({ InstructionsList: res.data })
            }).catch(error => {

                console.log(error)
            });

    }

    handleChange(name: string, event: any) {
        console.log(name);
        this.setState({ ...this.state, [name]: event.target.value })
    }

    render() {
        return (
            <React.Fragment>

                <Grid container spacing={16} justify="center">
                    <Grid item md={3} >
                        <TextField id="LinkGroupName"
                            label="Link Group Name"
                            name="LinkGroupName"
                            value={this.state.LinkGroupName || ""}
                            onChange={(e) => this.handleChange('LinkGroupName', e)}
                            fullWidth
                        />

                        <TextField id="LinkGroupDesc"
                            label="Link Group Description"
                            name="LinkGroupDesc"
                            value={this.state.LinkGroupDesc || ""}
                            onChange={(e) => this.handleChange('LinkGroupDesc', e)}
                            fullWidth
                        />
                    </Grid>


                    <Grid item md={3}  >
                        <Button variant="contained" color="primary"
                            className={this.props.classes.searchButtons}>
                            Copy Link Group
                        </Button>
                        <Button variant="contained" color="secondary"
                            className={this.props.classes.searchButtons}>
                            Add Modifier
                        </Button>

                    </Grid>

                    <Grid item md={12} >
                        <ReactTable
                            className="-highlight -striped"
                            defaultPageSize={10}
                            data={this.state.LinkGroupItems}
                            columns={[
                                {
                                    Header: "PLU",
                                    accessor: "item.scanCode",
                                    show: true,
                                    style: { cursor: "pointer" }
                                },
                                {
                                    Header: "Modifier",
                                    accessor: "item.productDesc",
                                    show: true,
                                    style: { cursor: "pointer" }
                                },
                                {
                                    Header: "Brand",
                                    accessor: "item.brandName",
                                    show: true,
                                    style: { cursor: "pointer" }
                                },
                                {
                                    Header: "InstructionListId",
                                    accessor: "instructionListId",
                                    show: false,
                                    style: { cursor: "pointer" }
                                },
                                {
                                    Header: "Cooking Instructions",
                                    accessor: "instructionListId",
                                    show: true,
                                    style: { cursor: "pointer" },
                                    Cell: cellInfo => (
                                        <InstructionListPicker
                                            SelectOptions={this.state.InstructionsList}
                                            SelectedOption={cellInfo.original.instructionListId || -1}
                                            LinkGroupId={cellInfo.original.linkGroupId}
                                            handleSelectionChanged={(e) => {
                                                const data = [...this.state.LinkGroupItems];
                                                data[cellInfo.index][cellInfo.column.id!] = e.target.value
                                                this.setState({ ...this.state, LinkGroupItems: data })
                                            }
                                            }
                                        />)
                                },
                                {
                                    Header: "Action",
                                    accessor: "Action",
                                    style: { textAlign: "center" },
                                    show: true,
                                    width: 100,
                                    Cell: (row) => (
                                        <Button variant="text" size="small" color="secondary">
                                            Delete
                                        <DeleteIcon />
                                        </Button>
                                    )
                                }
                            ]}
                        />
                    </Grid>
                    <Grid container justify="space-between" >
                    <Grid item >
                         
                        <Button variant="contained" color="default"
                         
                            onClick={this.props.handleCancelClick}
                        >
                            Cancel (Esc)
                         <CancelIcon  className={this.props.classes.IconLeftMargin} />
                        </Button>
                    </Grid>
                    <Grid item  >

                        <Button
                            variant="contained"
                            color="primary"
                           

                        >
                            Save
                            <SaveIcon className={this.props.classes.IconLeftMargin} />
                        </Button> 
                    </Grid>
                    </Grid>
                </Grid>

                {/* <pre>{JSON.stringify(this.state, null, 4)}</pre> */}
            </React.Fragment>
        )
    }
}


export default withStyles(styles)(EditLinkGroup);