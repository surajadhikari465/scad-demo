import * as React from 'react';
import {Grid} from '@material-ui/core';
import DisplayKits from './DisplayKits';
import "@babel/polyfill";
import SearchKits from './SearchKits';
import { KbApiMethod } from '../helpers/kbapi';
var urlStart = KbApiMethod("Kits");

import "@babel/polyfill";
import axios from 'axios';
import KitFooter from './KitFooter';
import PageTitle from '../PageTitle';
import withSnackbar from '../PageStyle/withSnackbar';

interface IKitListsPageState {
    isLoaded: boolean,
    kits: Array<any>,
    searchMainItemName: string,
    searchScanCode: string,
    searchLinkGroupName: string,
    searchkitDescription: string
}

interface IKitsProps {
    showAlert(message: string, type?: string): void;
}

class KitListPage extends React.Component<IKitsProps, IKitListsPageState>
{
    constructor(props: any) {
        super(props);

        this.state = {
            isLoaded: false,
            kits: [],
            searchMainItemName: "",
            searchScanCode: "",
            searchLinkGroupName: "",
            searchkitDescription : ""
        }

        this.onSearch = this.onSearch.bind(this);
        this.clear = this.clear.bind(this);
        this.mainItemChange = this.mainItemChange.bind(this);
        this.scanCodeChange = this.scanCodeChange.bind(this);
        this.linkGroupChange = this.linkGroupChange.bind(this);
        this.kitDescriptionChange = this.kitDescriptionChange.bind(this);
        
        this.onDelete = this.onDelete.bind(this);
    }
    mainItemChange(event: React.ChangeEvent<HTMLInputElement>) {
        this.setState({ searchMainItemName: event.target.value });
    }

    scanCodeChange(event: React.ChangeEvent<HTMLInputElement>) {
        this.setState({ searchScanCode: event.target.value });
    }

    linkGroupChange(event: React.ChangeEvent<HTMLInputElement>) {
        this.setState({ searchLinkGroupName: event.target.value });
    }

    handleClick = (e: React.MouseEvent<HTMLButtonElement>) => {
        
    }

    kitDescriptionChange(event: React.ChangeEvent<HTMLInputElement>) {
        this.setState({ searchkitDescription: event.target.value });
    }

    clear() {
        this.setState({ searchMainItemName: "" });
        this.setState({ searchScanCode: "" });
        this.setState({ searchkitDescription: "" });
        this.setState({ searchLinkGroupName: "" });
        this.setState({ kits: [] });
    }

    onDelete(row:any) {
        let data = this.state.kits;

            var headers = {
                 'Content-Type': 'application/json', "Access-Control-Allow-Origin": "*"
             }
          
            var url = urlStart +"/"+ row.row._original.kitId;
            axios.delete(url, { headers },
            ).then(response=>{   
            for (let i = 0 ; i < data.length ; i ++) {
                 if (data[i] != row.row._original) {
                      continue;
                 }
  
                 data.splice(i, 1);
                 break;
            }
            
       this.setState({
            kits: data
       });
        }) 
        .then(()=> this.props.showAlert("Kit deleted successfully"))
        
        .catch((error) => {
            if(error.response.data.includes("409")) { 
                this.props.showAlert("This kit cannot be deleted because it is assigned to a locale.", "error");
            }  
            else {
                this.props.showAlert("Error In Deleting Kit.", "error");
            }  
                 return;
            });
        }

    createKit() {
         window.location.hash = "#/CreateKits";
    }

    onEdit(kitId: number) {
        window.location.hash = "#/EditKit/" + kitId;
       }

    onSearch() {

        if (this.state.searchMainItemName == "" && this.state.searchScanCode == "" && this.state.searchkitDescription == "" && this.state.searchLinkGroupName == "") {
            this.props.showAlert("Please enter at least one select criteria.");
            return;
        }
        var urlParam ="";

        if (this.state.searchMainItemName != "")
            urlParam = urlParam + "ItemDescription=" + this.state.searchMainItemName + "&"

        if (this.state.searchScanCode != "")
            urlParam =urlParam + "ItemScanCode=" + this.state.searchScanCode + "&"

        if (this.state.searchkitDescription != "")
            urlParam =urlParam + "KitDescription=" + this.state.searchkitDescription + "&"

        if (this.state.searchLinkGroupName != "")
            urlParam = urlParam + "LinkGroupName=" + this.state.searchLinkGroupName+ "&"
        
            urlParam = urlParam.substring(0, urlParam.length - 1);

        var url = urlStart +"?"+  urlParam;

        fetch(url)
            .then(response => {
                return response.json();
            }).then((data)=> 
            {
                if(typeof(data) == undefined || data == null || data.length == 0)
                { 
                    this.props.showAlert("No Data Found.");
                    this.setState({
                        kits: []
                        });
                } else {                      
                    this.setState({ kits: data });
                }     
             }
            )
            .catch((error) => {
                this.props.showAlert(error.response.data, "error");
            });
    }

    render() {
        return (

            <React.Fragment>
                <Grid container justify="center">
                    <Grid item xs={10}>
                    <PageTitle icon="search">Search Kits</PageTitle>
                        <SearchKits
                            MainItemName={this.mainItemChange}
                            MainItemScanCode={this.scanCodeChange}
                            LinkGroupName={this.linkGroupChange}
                            onSearch={this.onSearch}
                            clear={this.clear}
                            LinkGroupValue = {this.state.searchLinkGroupName}
                            ScanCodeValue = {this.state.searchScanCode}
                            MainItemValue = {this.state.searchMainItemName}
                            KitDescription = {this.kitDescriptionChange}
                            KitDescriptionValue = {this.state.searchkitDescription}
                        />
                    </Grid>
                    <Grid item xs={10}>
                        <DisplayKits 
                            onDelete={this.onDelete}
                            onEdit={this.onEdit}
                            data = {this.state.kits}
                        />
                    </Grid>
                    <Grid item xs = {10}>
                        <KitFooter createKit={this.createKit}/>
                    </Grid>
                </Grid>
            </React.Fragment>
        )
    }
}

export default withSnackbar(KitListPage);