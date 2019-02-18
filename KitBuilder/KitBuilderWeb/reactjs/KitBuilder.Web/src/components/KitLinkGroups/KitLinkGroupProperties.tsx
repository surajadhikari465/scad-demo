import * as React from 'react';
import { KitLinkGroupItemProperties } from './KitLinkGroupItemProperties';

export class KitLinkGroupProperties extends React.Component<any>
{
    /* constructor */
    constructor(props: any)
    {
        super(props)
    }

    /* setState methods */   
    handleLinkGroupMinimum(event:any){
        this.setState({kitLinkGroupDetails: this.props.kitLinkGroupDetails.properties.Minimum = event.target.value })
    }

    handleLinkGroupMaximum(event:any){
        this.setState({kitLinkGroupDetails: this.props.kitLinkGroupDetails.properties.Maximum = event.target.value })
    }

    handleLinkGroupDisplayOrder(event:any){
        this.setState({kitLinkGroupDetails: this.props.kitLinkGroupDetails.displaySequence = event.target.value })
    }

    handleLinkGroupNumOfFreeToppings(event:any){
        this.setState({kitLinkGroupDetails: this.props.kitLinkGroupDetails.properties.NumOfFreeToppings = event.target.value })
    }

    handleLinkGroupExclude(){
        let changedState = !this.props.kitLinkGroupDetails.excluded
        this.setState({kitLinkGroupDetails: this.props.kitLinkGroupDetails.excluded = changedState})
        
    }

    /* html render method */
    render()
    {
        return  <React.Fragment>
            {/*add KitLinkGroup Properties 
            1. Minimum
            2. Maximum
            3. # of Free Toppings
            4. Display Order
            5. Exclude
             */}  
             <div className="row mt-md-1 border-bottom">
                    <div className = "col-lg-2 col-md-2 mt-md-3"> {/* Kit Link Group Name */}
                        <h4 className = "text-left">{this.props.kitLinkGroupDetails.name}</h4>
                    </div>
                    <div className = "col-lg-10 col-md-10 mt-md-3">{/* Kit Link Group Details */}
                        <form className ="wrapper"> 
                            <label className="col-lg-2 col-md-2">
                                <input className="col-lg-5 col-md-5" type ="text" value = {this.props.kitLinkGroupDetails.properties.Minimum} onChange = {this.handleLinkGroupMinimum.bind(this)}/> Minimum
                            </label>
                            <label className="col-lg-2 col-md-2">
                                <input className="col-lg-5 col-md-5 ml-md-1" type ="text" value = {this.props.kitLinkGroupDetails.properties.Maximum} onChange = {this.handleLinkGroupMaximum.bind(this)}/> Maximum
                            </label>
                            <label className="col-lg-3 col-md-3">
                                <input className="col-lg-3 col-md-3 ml-md-4" type ="text" value = {this.props.kitLinkGroupDetails.displaySequence} onChange = {this.handleLinkGroupDisplayOrder.bind(this)}/> Display Order
                            </label>
                            <label className="col-lg-3 col-md-3">
                                <input className="col-lg-3 col-md-3" type ="text" value = {this.props.kitLinkGroupDetails.properties.NumOfFreeToppings} onChange = {this.handleLinkGroupNumOfFreeToppings.bind(this)}/> # of Free Toppings
                            </label>
                            <label className="col-lg-2 col-md-2">
                                <input className="col-lg-2 col-md-2" type ="checkbox" checked = {this.props.kitLinkGroupDetails.excluded} onClick = {this.handleLinkGroupExclude.bind(this)}/> Exclude
                            </label>
                        </form> 
                    </div>
                    <div className = "col-lg-1 col-md-1"></div>
                    <div className = "col-lg-11 mb-1"> {/* Kit Link Group Items */}
                        {this.props.kitLinkGroupDetails.kitLinkGroupItemLocaleList.map((kitLGI:any)=><KitLinkGroupItemProperties kitLinkGroupItemsJson = {kitLGI}></KitLinkGroupItemProperties>)}
                    </div>
             </div> 
        </React.Fragment>;
    }
}