import * as React from 'react';
import axios from 'axios';
import {Grid} from '@material-ui/core';
import { KitLinkGroupProperties } from "./KitLinkGroupProperties";
import { KbApiMethod } from '../helpers/kbapi'

var urlStart = KbApiMethod("Kits");

interface IKitLinkGroupPageState {
    error : any,
    message : any,
    kitDetails: any,
    disableSaveButton:boolean
}

interface IKitLinkGroupPageProps {
    kitId: number,
    localeId : number
}

export class KitLinkGroupPage extends React.Component<IKitLinkGroupPageProps ,IKitLinkGroupPageState >
{   
    constructor(props: any)
    {
        super(props)
        this.state = {
            error: "",
            message: "",
            kitDetails : {},
            disableSaveButton:false
        }
        this.handleSaveButton = this.handleSaveButton.bind(this);
    }

    loadData(isSavingData:Boolean) 
    {    
                // render the kitId and localeId from hierarchy page
                var pathArray = window.parent.location.href.split('/');
                pathArray = pathArray.reverse();
                // calling the API to get the required information

                let url =urlStart+"/"+parseInt(pathArray[1])+"/GetKitProperties/"+parseInt(pathArray[0]);
                axios.get(url,{})
                .then(response => {
                    let kitLinkGroupsDetail = response.data;  
                    kitLinkGroupsDetail.kitLinkGroupLocaleList.map((linkGroup:any)=>{
                        let disableControls:boolean = false;
                        if(linkGroup.properties == null)
                        {
                            let newLinkGroupProperties = {
                                Minimum : "0",
                                Maximum : "0",
                                NumOfFreeToppings : "0",
                            }
                            linkGroup.properties = newLinkGroupProperties
                            linkGroup.excluded = false
                            linkGroup.childDisabled = false;
                            linkGroup.displaySequence = 0 // default display sequence
                        }
                        else
                        {   linkGroup.childDisabled = linkGroup.excluded;
                            linkGroup.properties = JSON.parse(linkGroup.properties)
                        }
                        disableControls = linkGroup.childDisabled;
                        linkGroup.kitLinkGroupItemLocaleList.map((linkGroupItem: any) => {
                            if(linkGroupItem.properties == null)
                            {
                                let newLinkGroupItemProps = {
                                    Minimum:"0",
                                    Maximum:"0",
                                    NumOfFreePortions:"0",
                                    DefaultPortions:"0",
                                    MandatoryItem:"false",
                                }
                                linkGroupItem.properties = newLinkGroupItemProps
                                linkGroupItem.excluded = false
                                linkGroupItem.isDisabled = disableControls
                                linkGroupItem.displaySequence = 0 // default display sequence
                            }
                            else
                            {   linkGroupItem.isDisabled = disableControls;
                                linkGroupItem.properties = JSON.parse(linkGroupItem.properties)
                            }
                        })
                    })
                
                    this.setState({kitDetails :kitLinkGroupsDetail},()=>{
                        if(isSavingData)
                        {
                        this.setState({
                            error: null, message: "Data Saved Succesfully.",  disableSaveButton:false
                       })
                    }})
     
                }).catch((error) => {
                 
                    this.setState({
                         error: "Error in getting data from API."
                    }) 
                    this.setState({
                         message: null
                    }) 
                  
                    return;
                  })  

    }

    componentDidMount ()
    {
      this.loadData(false)
    }
    saveData()
    {
        this.state.kitDetails.kitLinkGroupLocaleList.map((linkGroup:any)=>{
            if(linkGroup.properties != null)
            {
                linkGroup.properties = JSON.stringify(linkGroup.properties)
                linkGroup.lastModifiedBy = "Priyanka"
            }
            linkGroup.kitLinkGroupItemLocaleList.map((linkGroupItem: any) => {
                if(linkGroupItem.properties != null)
                {
                    linkGroupItem.properties = JSON.stringify(linkGroupItem.properties)
                    linkGroupItem.lastModifiedBy = "Priyanka"
                }
            })
        })
        let { kitDetails } = this.state;
        //alert(JSON.stringify(kitDetails))
        //sending data into database
        let urlKitSave = urlStart + "/" + kitDetails.kitId 
        var headers = {
             'Content-Type': 'application/json', "Access-Control-Allow-Origin": "*"
        }
        axios.post(urlKitSave, JSON.stringify(kitDetails),
             {
                  headers: headers
             }).then(response => {
                this.loadData(true)
         
             }).catch(error => {
                  this.setState({
                       error: "Error in Saving Data.", message: null, disableSaveButton:false
                  })
             });
      
    }
    handleSaveButton(event:any)
    {
        //setting the properties
        this.setState({disableSaveButton:true},()=>{
            this.saveData();
        })
    }

    render()
    { const kitLinkGroupLocaleList = this.state.kitDetails.kitLinkGroupLocaleList;

        if(typeof kitLinkGroupLocaleList =="undefined")
        {
          return <div></div>
        }
        else
        {

        return  <React.Fragment> 
            {/*add the inner components into this components
            1. Location search dropdown
            2. KitLinkGroup
            3. KitLinkGroupItem
            4. buttons to perform save */} 
       
            <Grid container justify="center">
                <Grid container md={12}  justify="center">
                    <div className="error-message" >
                    <span className = "text-danger"> {this.state.error}</span>
                    </div>
                </Grid>
                <Grid container md={12}  justify="center">
                    <div className="Suncess-message" >
                    <span className = "text-success"> {this.state.message}</span>
                    </div>
                </Grid>
            </Grid>
            <div className = "mt-md-4 mb-md-4">
                <div className = "row">
                    <div className="col-lg-4 col-md-5">  {/* Kit Name*/}
                        <h2 className="text-center font-italic font-weight-bold mt-md-5">{this.state.kitDetails.description}</h2>
                    </div>
                    <div className="col-lg-4 col-md-3"> {/* location dropdown */}
                        <h5 className="text-center mt-md-3">{this.state.kitDetails.localeName}<br></br></h5>
                    </div> 
                </div>
                <div className = "row">
                    <div className="col-lg-1 col-md-1 col-sm-1"></div> {/* added for the right spacing */}      
                    <div className="col-lg-10 col-md-10 col-sm-10">  {/* add main components here */}
                        <form className = "mb-md-4 border-top">
                           {
                                this.state.kitDetails.kitLinkGroupLocaleList.map((kitLG:any)=><KitLinkGroupProperties kitLinkGroupDetails = {kitLG}/>)
                            }
                        </form> 
                        <div className = "row">{/* Save and Publish buttons  */}
                            <div className = "col-lg-2 col-md-2 col-sm-2"></div>
                            <button className = "col-lg-2 col-md-2 col-sm-2 btn btn-primary" type = "button" onClick = {this.handleSaveButton}> Publish </button>
                            <div className = "col-lg-4 col-md-4 col-sm-4"></div>
                            <button disabled={this.state.disableSaveButton} className = "col-lg-2 col-md-2 col-sm-2 btn btn-success" type = "button" onClick = {this.handleSaveButton}> Save Changes </button>
                            <div className = "col-lg-2 col-md-2 col-sm-2"></div>
                        </div> 
                    </div>
                    <div className="col-lg-1 col-md-1 col-sm-1"></div> {/* added for the left spacing*/} 
                </div>
            </div>
        </React.Fragment>;
        }
        } 
    }

export default KitLinkGroupPage;
