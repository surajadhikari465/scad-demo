import * as React from 'react'
import { withStyles } from '@material-ui/core/styles';
import KitItemAddModal from './KitItemAddModal';
import { KbApiMethod } from '../../helpers/kbapi'
var urlStart = KbApiMethod("Items");
var urlLinkgroupSearch = KbApiMethod("LinkGroupsSearch:");
import { Grid, Button } from '@material-ui/core';
const hStyle = { color: 'red' };
const successStyle = { color: 'blue' };
import axios from 'axios';
import LinkgroupKitAddModal from '../LinkGroupAddKitModal/LinkgroupKitAddModal'

const styles = (theme: any) => ({
    root: {
        marginTop: 20
    },
    marginbottom: {
        marginBottom: 40
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
    formControl:
    {
        margin: theme.spacing.unit,
        minWidth: 120,
        width: '100%'
    }
});

interface ICreateKitPageState {
    error: any,
    message: any,
    mainItemName: string,
    open: Boolean,
    mainItemlabelName: string,
    scanCodeName: string,
    scanCodelabelName: string,
    mainItemValue: string,
    scanCodeValue: string,
    selectedItemiId: number
    itemsdata: []
    showDisplay: Boolean,
    errorKitItem: any,
    messageKitItem: any,

    linkGrouplabelName: string,
    linkGroupName: string,
    modifierPluName: string,
    modifierPlulabelName: string,
    searchLinkGroupText: string,
    searchModifierPluText: string,
    linkGroupData:any[],
    selectedData:any[],
    openLinkGroup:Boolean,
    errorlinkGroupAdd: any,
    messagelinkGroupAdd: any,
}

interface ICreateKitPageProps {

}

export class CreateKit extends React.Component<ICreateKitPageProps, ICreateKitPageState>
{
    constructor(props: any) {
        super(props);

        this.state = {
            error: null,
            message: null,
            mainItemName: "Main Item Name:",
            open: false,
            mainItemlabelName: "Main Item Name:",
            scanCodeName: "Scan Code:",
            scanCodelabelName: "Scan Code:",
            itemsdata: [],
            mainItemValue: "",
            scanCodeValue: "",
            selectedItemiId: 0,
            showDisplay: false,
            linkGrouplabelName: "Link Group Name",
            modifierPluName: "Modifier PLU",
            modifierPlulabelName: "Modifier PLU",
            searchLinkGroupText: "",
            searchModifierPluText: "",
            linkGroupData: [],
            selectedData: [],
            linkGroupName: "Link Group Name",
            errorKitItem: "",
            messageKitItem: "",
            errorlinkGroupAdd: "",
            messagelinkGroupAdd: "",
            openLinkGroup:false

        }

        this.onScanCodeChange = this.onScanCodeChange.bind(this);
        this.onMainItemChange = this.onMainItemChange.bind(this);
        this.onChecked = this.onChecked.bind(this);
        this.onSearch = this.onSearch.bind(this);
        this.showPopUp = this.showPopUp.bind(this);
        this.onClose = this.onClose.bind(this);
        this.onLinkGroupSearch = this.onLinkGroupSearch.bind(this);
        this.onRemove = this.onRemove.bind(this);
        this.onSelect = this.onSelect.bind(this);
        this.onLinkGroupChange = this.onLinkGroupChange.bind(this);
        this.onModifierPluChange = this.onModifierPluChange.bind(this);
        this.onLinkGroupClose =  this.onLinkGroupClose.bind(this);
        this.queueLinkGroups =  this.queueLinkGroups.bind(this);
        this.addToKit =  this.addToKit.bind(this);
        this.showLinkGroupPopUp =  this.showLinkGroupPopUp.bind(this);
    }

    onChecked(row: any) {
        alert(row.row._original.ItemId);
        this.setState({ open: false });
    }

    onLinkGroupSearch() {
        let linkGroupName = this.state.searchLinkGroupText;
        let modifierPlu = this.state.searchModifierPluText;

        if (linkGroupName == "" && modifierPlu == "") {
            this.setState({
                errorlinkGroupAdd: "Please enter atleast one select criteria."
            });
          
            this.setState({
                messagelinkGroupAdd: null
            })
            return;
        }
        else{
            this.setState({
                errorlinkGroupAdd: null,
                messagelinkGroupAdd: null
            });
           
            }
            
        var urlParam = "";

        if (linkGroupName != "")
            urlParam = urlParam + "LinkGroupName=" + linkGroupName + "&"

        if (modifierPlu != "")
            urlParam = urlParam + "ModifierPlu=" + modifierPlu + "&"

        urlParam = urlParam.substring(0, urlParam.length - 1);

        var url = urlLinkgroupSearch + "?" + urlParam;

         return axios.get(url)
        .then(response => {
            if (typeof (response.data) == undefined || response.data == null || response.data.length == 0) 
            {
                this.setState({
                    errorlinkGroupAdd: "No Data Found."
                });

                this.setState({
                    messagelinkGroupAdd: null
                })
            }
            else{
                this.setState({linkGroupData: response.data})        
               
            }        
        })
    }

    queueLinkGroups()
    {
       let alreadySelected:any[]  = this.state.selectedData

       let selected:any[] = this.state.linkGroupData.filter((linkGroup:any)=>{
           return linkGroup.Select == true;
       });

       let remaining:any[] = this.state.linkGroupData.filter((linkGroup:any)=>{
        return linkGroup.Select != true;
    });

       alreadySelected.push(selected);

       this.setState({selectedData: alreadySelected})
       this.setState({linkGroupData: remaining})
    }
   
    addToKit()
    {

    }
    onModifierPluChange(event: any) {
        this.setState({ searchModifierPluText: event.target.value });
    }

    onLinkGroupClose()
    {
        this.setState({ openLinkGroup: false });
    }
    onLinkGroupChange(event: any) {
        this.setState({ searchLinkGroupText: event.target.value });
    }

    onRemove(row: any) {
        row.Remove = !row.Remove;       
        let alreadySelected:any[]  = this.state.selectedData;

        var remaining = alreadySelected.filter((linkGroup:any)=>{
                return linkGroup.Remove == false;
            });
            this.setState({selectedData: remaining})
    }

    onSelect(row: any) {
       row.Select = !row.Select;       
    }

    showPopUp() {
        this.setState({
            errorKitItem: null, messageKitItem:null
        });
       
        this.setState({ open: true });
    }

    showLinkGroupPopUp() {
        this.setState({ openLinkGroup: true });
    }
    onClose() {
        this.setState({ open: false });
    }

    onScanCodeChange(event: any) {
        this.setState({ scanCodeValue: event.target.value });
    }

    onMainItemChange(event: any) {
        this.setState({ mainItemValue: event.target.value });
    }

    onSearch() {
  
        let scanCode = this.state.scanCodeValue;
        let mainItem = this.state.mainItemValue;
 
        if (scanCode == "" && mainItem == "") {

            this.setState({
                errorKitItem: "Please enter atleast one select criteria."
            });
            this.setState({
                messageKitItem: null
            })
            return;
        }

        var urlParam = "";

        if (mainItem != "")
            urlParam = urlParam + "ProductDesc=" + mainItem + "&"

        if (scanCode != "")
            urlParam = urlParam + "ScanCode=" + scanCode + "&"

        urlParam = urlParam.substring(0, urlParam.length - 1);

        var url = urlStart + "?" + urlParam;

        fetch(url)
            .then(response => {
                return response.json();
            }).then((data) => {
                if (typeof (data) == undefined || data == null || data.length == 0) {
                    this.setState({
                        errorKitItem: "No Data Found."
                    });

                    this.setState({
                        messageKitItem: null
                    })

                    this.setState({
                        itemsdata: []
                    })

                    this.setState({
                        showDisplay: false
                    })
                }

                else {
                    this.setState({
                        error: null
                    });

                    this.setState({
                        errorKitItem: null, messageKitItem:null
                    });
                   
                    this.setState({
                        message: null
                    })
                    this.setState({
                        itemsdata: data
                    })
                    this.setState({
                        showDisplay: true
                    })
                }
            })
    }

    render() {
        const { itemsdata } = this.state;
        return (

            <React.Fragment>
                <Grid container justify="center">
                    <Grid container justify="center">
                        <div className="error-message" >
                            <span style={hStyle}> {this.state.error}</span>
                        </div>
                    </Grid>
                    <Grid container justify="center">
                        <div className="Success-message" >
                            <span style={successStyle}> {this.state.message}</span>
                        </div>
                    </Grid>
                </Grid>
                <Button
                    variant="contained"
                    color="secondary"
                    onClick={this.showPopUp}
                />
                <Button
                    variant="contained"
                    color="primary"
                    onClick={this.showLinkGroupPopUp}
                />
                <KitItemAddModal
                    mainItemName={this.state.mainItemName}
                    open={this.state.open}
                    mainItemlabelName={this.state.mainItemName}
                    scanCodeName={this.state.scanCodeName}
                    scanCodelabelName={this.state.scanCodeName}
                    data={itemsdata}
                    onSearch={this.onSearch}
                    mainItem={this.state.mainItemValue}
                    scanCode={this.state.scanCodeValue}
                    onChecked={this.onChecked}
                    showDisplay={this.state.showDisplay}
                    onScanCodeChange={this.onScanCodeChange}
                    onMainItemChange={this.onMainItemChange}
                    onClose={this.onClose}
                    errorKitItem = {this.state.errorKitItem}
                    messageKitItem = {this.state.messageKitItem}
                />

            <LinkgroupKitAddModal
                        linkGrouplabelName = {this.state.linkGrouplabelName}
                        onLinkGroupChange = {this.onLinkGroupChange}
                        modifierPluName = {this.state.modifierPluName}
                        modifierPlulabelName = {this.state.modifierPlulabelName}
                        onModifierPluChange = {this.onModifierPluChange}
                        onLinkGroupSearch = {this.onLinkGroupSearch}
                        searchLinkGroupText = {this.state.searchLinkGroupText}
                        searchModifierPluText = {this.state.searchModifierPluText}
                        linkGroupData = {this.state.linkGroupData}
                        selectedData = {this.state.linkGroupData}
                        onRemove = {this.onRemove}
                        onSelect={this.onSelect}
                        onLinkGroupClose = {this.onLinkGroupClose}
                        queueLinkGroups = {this.queueLinkGroups}
                        addToKit = {this.addToKit}
                        open={this.state.openLinkGroup}
                    />   
            </React.Fragment>
        )
    }
}

export default withStyles(styles, { withTheme: true })(CreateKit);