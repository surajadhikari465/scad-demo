import * as React from 'react';
import axios from 'axios';
import { KitLinkGroupProperties } from "./KitLinkGroupProperties";
import { KbApiMethod } from '../helpers/kbapi'

var urlStart = KbApiMethod("Kits");
var kitId = 12
var localeId = 59

export class KitLinkGroupPage extends React.Component<{}, {kitLinkGroupDetails : any[], kitName : any}>
{   
    constructor(props: any)
    {
        super(props)
        this.state = {
            kitName : "",
            kitLinkGroupDetails : []
        }
    }

    componentWillMount(){
        return axios.get(urlStart+"/"+kitId+"/ViewKit/"+localeId+"?loadChildObjects="+"true",{})
        .then(response => {
            this.setState({kitLinkGroupDetails : response.data.kitLinkGroup.map((t:any)=>t), kitName: response.data.description})
        })
    }

    render()
    {
        return  <React.Fragment> 
            {/*add the inner components into this components
            1. Location search dropdown
            2. KitLinkGroup
            3. KitLinkGroupItem
            4. buttons to perform save */} 
            <div className = "mt-md-4 mb-md-4">
                <div className = "row mt-md-4">
                    <div className="col-lg-6 col-md-6 col-sm-6">  {/* logo spacing */}
                        <h4 className="text-center">image / logo of the kit</h4>
                    </div>
                    <div className="col-lg-6 col-md-6 col-sm-6"> {/* location dropdown */}
                        <h4 className="text-center">Location<br></br></h4>
                    </div> 
                </div>
                <div className = "row mb-md-4">
                    <div className="col-lg-1 col-md-1 col-sm-1"></div> {/* added for the right spacing */}      
                    <div className="col-lg-10 col-md-10 col-sm-10">  {/* add main components here */}
                        <h2 className="text-center">{this.state.kitName}</h2>
                        <form className = "mt-md-4 mb-md-4">
                            {this.state.kitLinkGroupDetails.map((kitLG)=> <KitLinkGroupProperties kitLinkGroupDetails = {kitLG}/> )}
                            {/* <ul>{this.state.linkGroupDetails.map((x)=> <li>{x.groupName}</li>)}</ul> */}
                        </form> 
                        <div className = "row">{/* buttons  */}
                            <div className = "col-lg-2 col-md-2 col-sm-2"></div>
                            <button className = "col-lg-2 col-md-2 col-sm-2 btn btn-primary" type = "button"> Publish </button>
                            <div className = "col-lg-4 col-md-4 col-sm-4"></div>
                            <button className = "col-lg-2 col-md-2 col-sm-2 btn btn-success" type = "button"> Save Changes </button>
                            <div className = "col-lg-2 col-md-2 col-sm-2"></div>
                        </div> 
                    </div>
                    <div className="col-lg-1 col-md-1 col-sm-1"></div>    {/* added for the left spacing */}
                </div>
            </div>
        </React.Fragment>;
    }
}

export default KitLinkGroupPage;
