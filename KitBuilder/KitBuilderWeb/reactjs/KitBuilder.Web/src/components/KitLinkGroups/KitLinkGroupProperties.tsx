import * as React from 'react';
import { KitLinkGroupItemProperties } from './KitLinkGroupItemProperties';

export class KitLinkGroupProperties extends React.Component<any>
{
    kitLinkGroup: any 

    constructor(props: any)
    {
        super(props)
        this.kitLinkGroup = this.props.kitLinkGroupDetails
    }

    render()
    {
        return  <React.Fragment>
            {/*add KitLinkGroup Properties 
            1. Minimum
            2. Maximum
            3. # of Free Toppings
            4. Display Order
            5. Excluded
             */}  
             <div className="row mt-md-1 border border-info">
                    <div className = "col-lg-3 col-md-3 mt-md-3"> {/* Kit Link Group Name */}
                        <h4 className = "text-left">{this.props.kitLinkGroupDetails.linkGroup.groupName}</h4>
                    </div>
                    <div className = "col-lg-9 col-md-9 mt-md-3">{/* Kit Link Group Details */}
                        <form className ="wrapper border"> 
                            <label className="col-lg-3 col-md-3">
                                <input className="col-lg-3 col-md-3" type ="text" defaultValue = ""/> Minimum
                            </label>
                            <label className="col-lg-3 col-md-3">
                                <input className="col-lg-3 col-md-3" type ="text" defaultValue = ""/> Maximum
                            </label>
                            <label className="col-lg-3 col-md-3">
                                <input className="col-lg-3 col-md-3" type ="text" defaultValue = ""/> Display Order
                            </label>
                            <label className="col-lg-3 col-md-3">
                                <input className="col-lg-3 col-md-3" type ="text" defaultValue = ""/> # of Free Toppings
                            </label>
                            <label className="col-lg-3 col-md-3">
                                <input className="col-lg-2 col-md-2" type ="checkbox" checked = {false} /> Excluded
                            </label>
                        </form> 
                    </div>
                    <div className = "col-lg-1 col-md-1"></div>
                    <div className = "col-lg-11 mb-1"> {/* Kit Link Group Items */}
                        {this.kitLinkGroup.kitLinkGroupItem.map((kitLGI:any)=><KitLinkGroupItemProperties kitLinkGroupItemsJson = {kitLGI}></KitLinkGroupItemProperties>)}
                    </div>
             </div> 
        </React.Fragment>;
    }
}