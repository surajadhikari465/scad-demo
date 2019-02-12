import * as React from 'react'
import { Grid, TextField, Button } from '@material-ui/core';
import ReactTable from 'react-table';
import AddCircleOutline from '@material-ui/icons/AddCircleOutline'
import SaveIcon from '@material-ui/icons/Save'
import CancelIcon from '@material-ui/icons/Cancel'
import RemoveCircleOutline from '@material-ui/icons/RemoveCircleOutline'
import * as LinkGroupFunctions from './LinkGroupFunctions';
import swal from 'sweetalert2';
import Typeography from '@material-ui/core/Typography'



interface IProps {
    onCancel(): void
}
interface IState {
    ModifierSearchResults: Array<any>,
    ModifiersToInclude: Array<any>,
    ModifierPLU: string,
    ModifierName: string
}

export class AddModifiersToLinkGroupsPage extends React.Component<IProps, IState> {
    constructor(props: any) {
        super(props)

        this.state = {
            ModifierPLU: "",
            ModifierName: "",
            ModifierSearchResults: [],
            ModifiersToInclude: []
        }

        this.AddModifierToIncludeList = this.AddModifierToIncludeList.bind(this);
        this.RemoveModifierFromIncludeList = this.RemoveModifierFromIncludeList.bind(this);
        this.deleteFromList = this.deleteFromList.bind(this);
        this.closeAddModifiers = this.closeAddModifiers.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.preformLinkGruopModifierSearch = this.preformLinkGruopModifierSearch.bind(this);

    }


    handleChange(name: string, event: any) {
        this.setState({ ...this.state, [name]: event.target.value })
    }


    closeAddModifiers() {

        this.setState({
            ModifierSearchResults: [],
            ModifiersToInclude: []
        })

        this.props.onCancel();
    }

    AddModifierToIncludeList(item: any) {
        var includedModifiers = this.state.ModifiersToInclude;
        var matchedModifiers = this.state.ModifiersToInclude.filter((value, index, array) => item.scanCode === value.scanCode);
        if (matchedModifiers.length == 0) includedModifiers.unshift(item);
        this.setState({ ModifiersToInclude: includedModifiers });
    }

    RemoveModifierFromIncludeList(item: any) {
        var includedModifiers = this.state.ModifiersToInclude.filter((value, index, array) => item.scanCode !== value.scanCode);
        this.setState({ ModifiersToInclude: includedModifiers });
    }

    deleteFromList(index: number, list: Array<any>): any {
        var temp = list;
        temp.splice(index, 1);
        return temp;
    }

    preformLinkGruopModifierSearch(modiferPLU: string, modifierName: string) {

        LinkGroupFunctions.PerformLinkGroupModifierSearch(modiferPLU, modifierName)
            .then((res: Array<any>) => {
                this.setState({ ModifierSearchResults: res })
            })
            .catch((error) => {
                swal({
                    allowEnterKey: true,
                    allowEscapeKey: true,
                    allowOutsideClick: false,
                    type: "error",
                    text: error.message
                })
            })
    }

    render() {
        return (
            <React.Fragment>
                <Grid container spacing={16} justify="center">
                <Grid item md={12}>
                        <Typeography style={{ fontVariant: "small-caps" }} variant="h5">modifier search</Typeography>
                    </Grid>
                    <Grid item md={4} >
                        <TextField id="ModifierPLU"
                            label="Modifier PLU"
                            name="ModifierPLU"
                            value={this.state.ModifierPLU || ""}
                            onChange={(e) => this.handleChange('ModifierPLU', e)}
                            fullWidth
                        /></Grid>
                    <Grid item md={4} >
                        <TextField id="ModifierName"
                            label="Modifier Name"
                            name="ModifierName"
                            value={this.state.ModifierName || ""}
                            onChange={(e) => this.handleChange('ModifierName', e)}
                            fullWidth
                        />
                    </Grid>
                    <Grid item md={4} >
                        <Button variant="contained" color="primary"
                            //className={this.props.classes.searchButtons}
                            onClick={() => { this.preformLinkGruopModifierSearch(this.state.ModifierPLU, this.state.ModifierName) }}
                        >
                            Search
                        </Button>
                    </Grid>
                   
                    <Grid item md={12}>
                        <ReactTable
                            className="-highlight -striped"
                            defaultPageSize={10}

                            data={this.state.ModifierSearchResults}
                            columns={[
                                {
                                    Header: "PLU",
                                    accessor: "scanCode",
                                    show: true,
                                    style: { cursor: "pointer" }
                                },
                                {
                                    Header: "Modifier",
                                    accessor: "productDesc",
                                    show: true,
                                    style: { cursor: "pointer" }
                                },
                                {
                                    Header: "Brand",
                                    accessor: "brandName",
                                    show: true,
                                    style: { cursor: "pointer" }
                                },
                                {
                                    Header: "Include",
                                    show: true,

                                    Cell: (cellInfo) => (
                                        <div style={{ textAlign: "center" }}>
                                            <Button onClick={() => { this.AddModifierToIncludeList(cellInfo.original) }}>
                                                <AddCircleOutline style={{ color: "green" }}></AddCircleOutline>
                                            </Button>
                                        </div>
                                    )
                                }

                            ]}
                        />
                    </Grid>
                    <Grid item md={12}>
                        <Typeography style={{ fontVariant: "small-caps" }} variant="h5">selcted modifiers</Typeography> </Grid>
                    <Grid item md={12}>
                        <ReactTable
                            className="-highlight -striped"
                            defaultPageSize={10}
                            data={this.state.ModifiersToInclude}
                            columns={[
                                {
                                    Header: "PLU",
                                    accessor: "scanCode",
                                    show: true,
                                    style: { cursor: "pointer" }
                                },
                                {
                                    Header: "Modifier",
                                    accessor: "productDesc",
                                    show: true,
                                    style: { cursor: "pointer" }
                                },
                                {
                                    Header: "Brand",
                                    accessor: "brandName",
                                    show: true,
                                    style: { cursor: "pointer" }
                                },
                                {
                                    Header: "Remove",
                                    show: true,

                                    Cell: (cellInfo) => (
                                        <div style={{ textAlign: "center" }}>
                                            <Button onClick={() => { this.RemoveModifierFromIncludeList(cellInfo.original) }}>
                                                <RemoveCircleOutline style={{ color: "red" }}></RemoveCircleOutline>
                                            </Button>
                                        </div>
                                    )
                                }

                            ]}
                        />
                    </Grid>
                    <Grid item md={12}>
                        <Grid container justify="space-between" >
                            <Grid item >

                                <Button variant="contained" color="default" onClick={this.closeAddModifiers}>
                                    Cancel
                         <CancelIcon
                                    //className={this.props.classes.IconLeftMargin} 
                                    />
                                </Button>
                            </Grid>
                            <Grid item  >

                                <Button
                                    variant="contained"
                                    color="primary"
                                >
                                    Save Modifiers to Link Group
                            <SaveIcon
                                    //className={this.props.classes.IconLeftMargin} 
                                    />
                                </Button>
                            </Grid>
                        </Grid>
                    </Grid>
                </Grid>
            </React.Fragment>
        )
    }
}

export default AddModifiersToLinkGroupsPage