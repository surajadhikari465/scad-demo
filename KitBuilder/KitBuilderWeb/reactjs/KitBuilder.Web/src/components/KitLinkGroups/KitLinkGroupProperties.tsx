import * as React from 'react';
import { KitLinkGroupItemProperties } from './KitLinkGroupItemProperties';
import { isNumber, overFlow, checkValueLessThanOrEqualToMaxValue } from '../KitLinkGroups/ValidateFunctions'

export class KitLinkGroupProperties extends React.Component<any>
{
    /* constructor */
    constructor(props: any) {
        super(props)
    }
    onfocusOut(event: any)
    {
       this.props.updateError("");
    }
    /* setState methods */
    handleLinkGroupMinimum(event: any) {
        let minimum = event.target.value;

        if (isNumber(String(minimum)) && checkValueLessThanOrEqualToMaxValue(minimum, 10)) {
            this.setState({ kitLinkGroupDetails: this.props.kitLinkGroupDetails.properties.Minimum = parseInt(event.target.value) })
        }
        else {
            this.props.updateError("Minimum for link group must be numeric and can have values from 0 to 10.");
        }
    }

    handleLinkGroupMaximum(event: any) {
        let maximum = event.target.value;

        if (isNumber(String(maximum)) && checkValueLessThanOrEqualToMaxValue(maximum, 10)) {
            this.setState({ kitLinkGroupDetails: this.props.kitLinkGroupDetails.properties.Maximum = parseInt(event.target.value) })
        }
        else {
            this.props.updateError("Maximum for link group must be numeric and can have values from 1 to 10.");
        }
    }

    handleLinkGroupDisplayOrder(event: any) {
        let displayOrder = event.target.value;

        if (isNumber(String(displayOrder)) && !overFlow(displayOrder)) {
            this.setState({ kitLinkGroupDetails: this.props.kitLinkGroupDetails.displaySequence = parseInt(displayOrder) })
        }
        else {
            this.props.updateError("Display Order for link group must be numeric.");
        }
    }

    handleLinkGroupNumOfFreeToppings(event: any) {
        let numberOfFreeToppings = event.target.value;
        if (isNumber(String(numberOfFreeToppings))&& checkValueLessThanOrEqualToMaxValue(numberOfFreeToppings, 10)) {
        this.setState({ kitLinkGroupDetails: this.props.kitLinkGroupDetails.properties.NumOfFreeToppings = parseInt(numberOfFreeToppings)})
        }
        else{
            this.props.updateError("Number Of Free Toppings for link group must be numeric and can have values from 0 to 10.");
        }
    }

    handleLinkGroupExclude() {
        let changedState = !this.props.kitLinkGroupDetails.excluded
        this.props.kitLinkGroupDetails.kitLinkGroupItemLocaleList.forEach(function (element: any) {
            element.isDisabled = changedState;
        });
        this.setState({ kitLinkGroupDetails: this.props.kitLinkGroupDetails })
        this.setState({ kitLinkGroupDetails: this.props.kitLinkGroupDetails.excluded = changedState })

    }

    /* html render method */
    render() {
        return <React.Fragment>
            {/*add KitLinkGroup Properties 
            1. Minimum
            2. Maximum
            3. # of Free Toppings
            4. Display Order
            5. Exclude
             */}
            <div className="row mt-md-1 border-bottom">
                <div className="col-lg-2 col-md-2 mt-md-3"> {/* Kit Link Group Name */}
                    <h4 className="text-left">{this.props.kitLinkGroupDetails.name}</h4>
                </div>
                <div className="col-lg-10 col-md-10 mt-md-3">{/* Kit Link Group Details */}
                    <form className="wrapper">
                        <label className="col-lg-2 col-md-2">
                            <input className="col-lg-5 col-md-5" type="number" min="0" value={this.props.kitLinkGroupDetails.properties.Minimum} onChange={this.handleLinkGroupMinimum.bind(this)} onBlur={this.onfocusOut.bind(this)} /> Minimum
                            </label>
                        <label className="col-lg-2 col-md-2">
                            <input className="col-lg-5 col-md-5 ml-md-1"type="number" min="1" max="10" value={this.props.kitLinkGroupDetails.properties.Maximum} onChange={this.handleLinkGroupMaximum.bind(this)} onBlur={this.onfocusOut.bind(this)}/> Maximum
                            </label>
                        <label className="col-lg-3 col-md-3">
                            <input className="col-lg-3 col-md-3 ml-md-4"type="number" min="1" value={this.props.kitLinkGroupDetails.displaySequence} onChange={this.handleLinkGroupDisplayOrder.bind(this)} onBlur={this.onfocusOut.bind(this)}/> Display Order
                            </label>
                        <label className="col-lg-3 col-md-3">
                            <input className="col-lg-3 col-md-3" type="number" min="0"  value={this.props.kitLinkGroupDetails.properties.NumOfFreeToppings} onChange={this.handleLinkGroupNumOfFreeToppings.bind(this)} onBlur={this.onfocusOut.bind(this)}/> # of Free Toppings
                            </label>
                        <label className="col-lg-2 col-md-2">
                            <input className="col-lg-2 col-md-2" type="checkbox" checked={this.props.kitLinkGroupDetails.excluded} onClick={this.handleLinkGroupExclude.bind(this)} /> Exclude
                            </label>
                    </form>
                </div>
                <div className="col-lg-1 col-md-1"></div>
                <div className="col-lg-11 mb-1"> {/* Kit Link Group Items */}
                    {this.props.kitLinkGroupDetails.kitLinkGroupItemLocaleList.map((kitLGI: any) => <KitLinkGroupItemProperties updateError={this.props.updateError} kitLinkGroupItemsJson={kitLGI}></KitLinkGroupItemProperties>)}
                </div>
            </div>
        </React.Fragment>;
    }
}