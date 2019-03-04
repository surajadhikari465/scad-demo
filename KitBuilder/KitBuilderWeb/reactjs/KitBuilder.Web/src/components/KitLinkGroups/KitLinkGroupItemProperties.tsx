import * as React from 'react';
import { isNumber, overFlow, checkValueLessThanOrEqualToMaxValue } from '../KitLinkGroups/ValidateFunctions'
export class KitLinkGroupItemProperties extends React.Component<any>
{
    /* constructor */
    constructor(props: any) {
        super(props)
    }

    /* setState methods */
     onfocusOut(event: any)
     {
        this.props.updateError("");
     }
    handleLinkGroupItemMinimum(event: any) {
        let minimum = event.target.value;
        if (isNumber(String(minimum)) && checkValueLessThanOrEqualToMaxValue(minimum, 10)) {
            this.setState({ kitLinkGroupDetails: this.props.kitLinkGroupItemsJson.properties.Minimum = parseInt(minimum) })
            
            if (parseInt(minimum) > 0) {
                this.setState({ kitLinkGroupDetails: this.props.kitLinkGroupItemsJson.properties.MandatoryItem = "true" })
            }
            else {
                this.setState({ kitLinkGroupDetails: this.props.kitLinkGroupItemsJson.properties.MandatoryItem = "false"})
            }
        }
        else {
            this.props.updateError("Minimum for modifier must be numeric and can have values from 0 to 10.");

        }

    }

    handleLinkGroupItemMaximum(event: any) {
        let maximum = event.target.value;

        if (isNumber(String(maximum)) && checkValueLessThanOrEqualToMaxValue(maximum, 10)) {
            this.setState({ kitLinkGroupDetails: this.props.kitLinkGroupItemsJson.properties.Maximum = parseInt(maximum) })
        }
        else {
            this.props.updateError("Maximum for modifier must be numeric and can have values from 1 to 10.");
        }
    }

    handleLinkGroupItemDisplayOrder(event: any) {
        if (isNumber(String(event.target.value)) && !overFlow(event.target.value) ) {
            this.setState({ kitLinkGroupDetails: this.props.kitLinkGroupItemsJson.displaySequence = parseInt(event.target.value) })
        }
        else {
            this.props.updateError("Display order for modifier must be numeric.");
        }
    }

    handleLinkGroupItemNumOfFreePortions(event: any) {
        let numberOfFreePortions = event.target.value;

        if (isNumber(String(numberOfFreePortions)) && checkValueLessThanOrEqualToMaxValue(numberOfFreePortions, 10)) {
         
            this.setState({ kitLinkGroupDetails: this.props.kitLinkGroupItemsJson.properties.NumOfFreePortions = parseInt(numberOfFreePortions) })
        }
        else {
            this.props.updateError("Number of free portions for modifier must be numeric and can have values from 0 to 10.");
        }
    }

    handleLinkGroupItemDefaultPortions(event: any) {
        if (isNumber(String(event.target.value)) && checkValueLessThanOrEqualToMaxValue(event.target.value, 10)) {
            this.setState({ kitLinkGroupDetails: this.props.kitLinkGroupItemsJson.properties.DefaultPortions = parseInt(event.target.value) })
        }
        else {
            this.props.updateError("Default portions for modifier must be numeric and can have values from 0 to 10.");
        }
    }

    handleLinkGroupItemExclude() {
        let changedState = !this.props.kitLinkGroupItemsJson.excluded
        this.setState({ kitLinkGroupDetails: this.props.kitLinkGroupItemsJson.excluded = changedState })
    }

    handleLinkGroupItemMandatoryItem(event: any) {
        let changedState = ""
        if (this.props.kitLinkGroupItemsJson.properties.MandatoryItem == "true") {
            changedState = "false"
        }
        else if (this.props.kitLinkGroupItemsJson.properties.MandatoryItem == "false") {
            changedState = "true"
        }
        this.setState({ kitLinkGroupDetails: this.props.kitLinkGroupItemsJson.properties.MandatoryItem = changedState })
    }

    /* html render method */
    render() {
        return <React.Fragment>
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
                <div className="row col-lg-12">
                    <div className="col-lg-2 col-md-2 mt-md-3">{/* Kit Link Group Item Name */}
                        <h6 className="text-left ">{this.props.kitLinkGroupItemsJson.name}</h6>
                    </div>
                    <div className="col-lg-10 col-md-10 mt-md-3">{/* Kit Link Group Details */}
                        <form className="wrapper">
                            <fieldset disabled={this.props.kitLinkGroupItemsJson.isDisabled}>
                                <label className="col-lg-3 col-md-3">
                                    <input className="col-lg-4 col-md-4" type="number" min="0" max="10" value={this.props.kitLinkGroupItemsJson.properties.Minimum} onChange={this.handleLinkGroupItemMinimum.bind(this)} onBlur={this.onfocusOut.bind(this)} /> Minimum
                            </label>
                                <label className="col-lg-3 col-md-3">
                                    <input className="col-lg-4 col-md-4" type="number" min="0" max="10" value={this.props.kitLinkGroupItemsJson.properties.Maximum} onChange={this.handleLinkGroupItemMaximum.bind(this)} onBlur={this.onfocusOut.bind(this)} /> Maximum
                            </label>
                                <label className="col-lg-3 col-md-3">
                                    <input className="col-lg-4 col-md-4" type="number" min="1"  value={this.props.kitLinkGroupItemsJson.displaySequence} onChange={this.handleLinkGroupItemDisplayOrder.bind(this)} onBlur={this.onfocusOut.bind(this)}/> Display Order
                            </label>
                                <label className="col-lg-3 col-md-3">
                                    <input className="col-lg-4 col-md-4" type="number" min="0" max="10"  value={this.props.kitLinkGroupItemsJson.properties.NumOfFreePortions} onChange={this.handleLinkGroupItemNumOfFreePortions.bind(this)} onBlur={this.onfocusOut.bind(this)}/> # of Free Portions
                            </label>
                                <label className="col-lg-3 col-md-3">
                                    <input className="col-lg-4 col-md-4" type="text"  min="0" max="10" value={this.props.kitLinkGroupItemsJson.properties.DefaultPortions} onChange={this.handleLinkGroupItemDefaultPortions.bind(this)} onBlur={this.onfocusOut.bind(this)} /> Default Portions
                            </label>
                                <label className="col-lg-3 col-md-3">
                                    <input disabled={true} className="col-lg-2 col-md-2" type="checkbox" checked={this.props.kitLinkGroupItemsJson.properties.MandatoryItem == "true" ? true : false} onClick={this.handleLinkGroupItemMandatoryItem.bind(this)} /> Mandatory Item
                            </label>
                                <label className="col-lg-3 col-md-3">
                                    <input className="col-lg-2 col-md-2" type="checkbox" checked={this.props.kitLinkGroupItemsJson.excluded} onClick={this.handleLinkGroupItemExclude.bind(this)} /> Exclude
                            </label>
                            </fieldset>
                        </form>
                    </div>
                </div>
            </div>

        </React.Fragment>;
    }

}