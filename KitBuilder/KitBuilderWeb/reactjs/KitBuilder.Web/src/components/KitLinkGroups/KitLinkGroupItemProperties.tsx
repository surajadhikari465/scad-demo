import * as React from 'react';

export class KitLinkGroupItemProperties extends React.Component<any>
{
    kitLinkGroupItems : any[]
    kitLinkGroupItemName : string

    constructor(props: any)
    {
        super(props)
        this.kitLinkGroupItems = this.props.kitLinkGroupItemsJson
    }

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
                        <h6 className = "text-left ">{this.props.kitLinkGroupItemsJson.linkGroupItem.item.productDesc}</h6>
                    </div>
                    <div className = "col-lg-10 col-md-10 mt-md-3">{/* Kit Link Group Details */}
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
                                <input className="col-lg-3 col-md-3" type ="text" defaultValue = ""/> # of Free Portions
                            </label>
                            <label className="col-lg-3 col-md-3">
                                <input className="col-lg-2 col-md-2" type ="checkbox" checked = {false} /> Excluded
                            </label>
                            <label className="col-lg-3 col-md-3">
                                <input className="col-lg-2 col-md-2" type ="checkbox" checked = {false} /> Default Item
                            </label>
                            <label className="col-lg-3 col-md-3">
                                <input className="col-lg-2 col-md-2" type ="checkbox" checked = {false} /> Mandatory Item
                            </label>
                        </form> 
                    </div>
                </div>
            </div>
            
            </React.Fragment>;
    }

}