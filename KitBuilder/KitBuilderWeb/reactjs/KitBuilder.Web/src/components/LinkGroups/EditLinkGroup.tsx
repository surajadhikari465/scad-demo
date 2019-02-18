import * as React from 'react'
import Grid from '@material-ui/core/Grid';
import ReactTable from 'react-table';
import DeleteIcon from '@material-ui/icons/Delete'
import SaveIcon from '@material-ui/icons/Save'
import CancelIcon from '@material-ui/icons/Cancel'
import InstructionListPicker from './InstructionListPicker';
import TextField from '@material-ui/core/TextField';
import Button from '@material-ui/core/Button';
import withStyles from '@material-ui/core/styles/withStyles';
import CopyLinkGroupButton from './CopyLinkGroupButton';
import * as LinkGroupFunctions from './LinkGroupFunctions';
import AddModifiersToLinkGroupsPage from './AddModifiersToLinkGroupsPage';


interface IState {
    InstructionsList: any,
    data: any,
    LinkGroupItems: Array<any>,
    LinkGroupName: string,
    LinkGroupDesc: string,
    showAddModifiers: boolean
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
            LinkGroupDesc: " ",
            showAddModifiers: false

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

    escFunction(event: any) {
        if (event.keyCode === 27) {
            this.props.handleCancelClick();
        }
    }


    componentWillUnmount() {
        document.removeEventListener("keydown", this.escFunction, false);
    }

    loadCookingInstructionsList() {

        LinkGroupFunctions.LoadCookingInstructions()
            .then(result => { this.setState({ InstructionsList: result }) })


    }

    handleChange(name: string, event: any) {
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
                        <CopyLinkGroupButton
                            className={this.props.classes.searchButtons}
                            linkGroupId={this.state.data.linkGroupId}
                        />
                        <Button variant="contained" color="secondary"
                            className={this.props.classes.searchButtons}
                            onClick={() => { this.setState({ showAddModifiers: !this.state.showAddModifiers }) }}
                        >
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
                         <CancelIcon className={this.props.classes.IconLeftMargin} />
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

                {!this.state.showAddModifiers ||
                    <AddModifiersToLinkGroupsPage onCancel={() =>{this.setState({showAddModifiers: !this.state.showAddModifiers})}} />}

                {/* <pre>{JSON.stringify(this.state, null, 4)}</pre> */}
            </React.Fragment>
        )
    }
}


export default withStyles(styles)(EditLinkGroup);