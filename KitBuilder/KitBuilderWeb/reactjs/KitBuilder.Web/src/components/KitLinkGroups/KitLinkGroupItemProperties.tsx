import * as React from 'react';

export class KitLinkGroupItemProperties extends React.Component<any>
{
     /* constructor */
    constructor(props: any)
    {
        super(props)
    }

    /* setState methods */  

    handleLinkGroupItemMinimum(event:any){
        this.setState({kitLinkGroupDetails: this.props.kitLinkGroupItemsJson.properties.Minimum = event.target.value })
    }

    handleLinkGroupItemMaximum(event:any){
        this.setState({kitLinkGroupDetails: this.props.kitLinkGroupItemsJson.properties.Maximum = event.target.value })
    }

    handleLinkGroupItemDisplayOrder(event:any){
        this.setState({kitLinkGroupDetails: this.props.kitLinkGroupItemsJson.displaySequence = event.target.value })
    }

    handleLinkGroupItemNumOfFreePortions(event:any){
        this.setState({kitLinkGroupDetails: this.props.kitLinkGroupItemsJson.properties.NumOfFreePortions = event.target.value })
    }
    
    handleLinkGroupItemDefaultPortions(event:any){
        this.setState({kitLinkGroupDetails: this.props.kitLinkGroupItemsJson.properties.DefaultPortions = event.target.value})
    }

    handleLinkGroupItemExclude(){
        let changedState = !this.props.kitLinkGroupItemsJson.excluded
        this.setState({kitLinkGroupDetails: this.props.kitLinkGroupItemsJson.excluded = changedState})
    }

    handleLinkGroupItemMandatoryItem(event: any){
        let changedState = ""
        if(this.props.kitLinkGroupItemsJson.properties.MandatoryItem == "true")
        {
            changedState = "false"
        } 
        else if (this.props.kitLinkGroupItemsJson.properties.MandatoryItem == "false")
        {
            changedState = "true"
        }
        this.setState({kitLinkGroupDetails: this.props.kitLinkGroupItemsJson.properties.MandatoryItem = changedState})
    }

     /* html render method */
    render()
    {
        return  <React.Fragment> 
            {/*add the KitLinkGroupItem Properties
            1. Display Order
            2. Minimum
            3. Maximum
            4. Default Item
            5. Mandatory Item
            6. # of Free Portions
            7. Excluded
            */}  
            <div className="row">
                <div className ="row col-lg-12"> 
                    <div className = "col-lg-2 col-md-2 mt-md-3">{/* Kit Link Group Item Name */}
                        <h6 className = "text-left ">{this.props.kitLinkGroupItemsJson.name}</h6>
                    </div>
                    <div className = "col-lg-10 col-md-10 mt-md-3">{/* Kit Link Group Details */}
                        <form className ="wrapper"> 
                            <label className="col-lg-3 col-md-3">
                                <input className="col-lg-4 col-md-4" type ="text" value = {this.props.kitLinkGroupItemsJson.properties.Minimum} onChange = {this.handleLinkGroupItemMinimum.bind(this)}/> Minimum
                            </label>
                            <label className="col-lg-3 col-md-3">
                                <input className="col-lg-4 col-md-4" type ="text" value = {this.props.kitLinkGroupItemsJson.properties.Maximum} onChange = {this.handleLinkGroupItemMaximum.bind(this)}/> Maximum
                            </label>
                            <label className="col-lg-3 col-md-3">
                                <input className="col-lg-4 col-md-4" type ="text" value = {this.props.kitLinkGroupItemsJson.displaySequence} onChange = {this.handleLinkGroupItemDisplayOrder.bind(this)}/> Display Order
                            </label>
                            <label className="col-lg-3 col-md-3">
                                <input className="col-lg-4 col-md-4" type ="text" value = {this.props.kitLinkGroupItemsJson.properties.NumOfFreePortions} onChange = {this.handleLinkGroupItemNumOfFreePortions.bind(this)}/> # of Free Portions
                            </label>
                            <label className="col-lg-3 col-md-3">
                                <input className="col-lg-4 col-md-4" type ="text" value = {this.props.kitLinkGroupItemsJson.properties.DefaultPortions} onChange = {this.handleLinkGroupItemDefaultPortions.bind(this)}/> Default Portions
                            </label>
                            <label className="col-lg-3 col-md-3">
                                <input className="col-lg-2 col-md-2" type ="checkbox" checked = {this.props.kitLinkGroupItemsJson.properties.MandatoryItem == "true" ? true : false} onClick = {this.handleLinkGroupItemMandatoryItem.bind(this)}/> Mandatory Item 
                            </label>
                            <label className="col-lg-3 col-md-3">
                                <input className="col-lg-2 col-md-2" type ="checkbox" checked = {this.props.kitLinkGroupItemsJson.exclude} onClick = {this.handleLinkGroupItemExclude.bind(this)} /> Exclude
                            </label>
                        </form> 
                    </div>
                </div>
            </div>
            
            </React.Fragment>;
    }

}