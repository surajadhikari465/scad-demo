import * as React from 'react';
import {Grid} from '@material-ui/core';
import DisplayKits from './DisplayKits';
import "@babel/polyfill";
import SearchKits from './SearchKits';;
const hStyle = { color: 'red' };
const sucesssStyle = { color: 'blue' };
import { KbApiMethod } from '../helpers/kbapi'
var urlStart = KbApiMethod("Kits");

import "@babel/polyfill";
import axios from 'axios';

interface IKitListsPageState {
    error: any,
    message: any
    isLoaded: boolean,
    kits: Array<any>,
    searchMainItemName: string,
    searchScanCode: string,
    searchLinkGroupName: string
}

interface IKitsProps {
}

export class KitListPage extends React.Component<IKitsProps, IKitListsPageState>
{
    constructor(props: any) {
        super(props);

        this.state = {
            error: null,
            message: null,
            isLoaded: false,
            kits: [],
            searchMainItemName: "",
            searchScanCode: "",
            searchLinkGroupName: ""
        }

        this.onSearch = this.onSearch.bind(this);
        this.clear = this.clear.bind(this);
        this.mainItemChange = this.mainItemChange.bind(this);
        this.scanCodeChange = this.scanCodeChange.bind(this);
        this.linkGroupChange = this.linkGroupChange.bind(this);
        this.onDelete = this.onDelete.bind(this);
    }
    mainItemChange(event: any) {
        this.setState({ searchMainItemName: event.target.value });
    }

    scanCodeChange(event: any) {
        this.setState({ searchScanCode: event.target.value });
    }

    linkGroupChange(event: any) {
        this.setState({ searchLinkGroupName: event.target.value });
    }

    clear() {
        this.setState({ searchMainItemName: "" });
        this.setState({ searchScanCode: "" });
        this.setState({ searchLinkGroupName: "" });
        this.setState({ error: null });
        this.setState({ message: null });
        this.setState({ kits: [] });
    }

    onDelete(row:any) {
        let data = this.state.kits;

            var headers = {
                 'Content-Type': 'application/json', "Access-Control-Allow-Origin": "*"
             }
          
            var url = urlStart +"/"+ row.row._original.KitId;
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
        .then(()=> this.setState({message: "Kit Deleted Sucessfully"}))
        .then(()=> this.setState({error: null}))
        
        .catch((error) => 
        {
            if(error.response.data.includes("409"))  
            { 
                this.setState({
                error: "Kit is in use. Please make sure this kit is not assigned to any locale."
           }) 
            }   
            else{
                this.setState({
                    error: "Error in Deleting Kit."
               }) 
            }  
            
                 this.setState({
                      message: null
                 }) 
                 return;
               });
        }

    createKit() {
         window.location.hash = "#/CreateKit";
    }

    onEdit() {
        window.location.hash = "#/EditKit";
       }

    onSearch() {

        if (this.state.searchMainItemName == "" && this.state.searchScanCode == "" && this.state.searchLinkGroupName == "") {
            this.setState({
                error: "Please enter atleast one select criteria."
            });

            this.setState({
                message: null
            })
            return;

        }
        var urlParam ="";

        if (this.state.searchMainItemName != "")
            urlParam = urlParam + "ItemDescription=" + this.state.searchMainItemName + "&"

        if (this.state.searchScanCode != "")
            urlParam =urlParam + "ItemScanCode=" + this.state.searchScanCode + "&"

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
                    this.setState({
                        error: "No Data Found."
                    });
        
                    this.setState({
                        message: null
                    })

                    this.setState({
                        kits: []
                        })
                    
                }
               
                else
                {                      
                    this.setState({
                    error: null
                });
    
                this.setState({
                    message: null
                })
                    
                    
                    this.setState({
                    kits: data
                    })

                }
               
             }
            )
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

    render() {

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
                    <Grid item md={10}>
                        <SearchKits
                            MainItemName={this.mainItemChange}
                            MainItemScanCode={this.scanCodeChange}
                            LinkGroupName={this.linkGroupChange}
                            onSearch={this.onSearch}
                            clear={this.clear}
                            LinkGroupValue = {this.state.searchLinkGroupName}
                            ScanCodeValue = {this.state.searchScanCode}
                            MainItemValue = {this.state.searchMainItemName}
                        />
                    </Grid>
                    <Grid item md={10}>
                        <DisplayKits
                            createKit={this.createKit}
                            onDelete={this.onDelete}
                            onEdit={this.onEdit}
                            data = {this.state.kits}
                        />
                    </Grid>
                </Grid>
            </React.Fragment>
        )
    }
}