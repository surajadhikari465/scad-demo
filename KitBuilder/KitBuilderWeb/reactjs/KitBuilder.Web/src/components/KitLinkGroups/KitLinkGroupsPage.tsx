import * as React from 'react';
import axios from 'axios';
import { Grid } from '@material-ui/core';
import { KitLinkGroupProperties } from "./KitLinkGroupProperties";
import { KbApiMethod } from '../helpers/kbapi'
import { isNumber, checkDuplicateInObject, checkValueLessThanOrEqualToMaxValue, checkValueMoreThanOrEqualToMinValue } from '../KitLinkGroups/ValidateFunctions'
const hStyle = { color: 'red' };
var urlStart = KbApiMethod("Kits");

interface IKitLinkGroupPageState {
    error: any,
    message: any,
    kitDetails: any,
    disableSaveButton: boolean,
    validationErrors: any
}

interface IKitLinkGroupPageProps {
    kitId: number,
    localeId: number
}

export class KitLinkGroupPage extends React.Component<IKitLinkGroupPageProps, IKitLinkGroupPageState>
{
    constructor(props: any) {
        super(props)
        this.state = {
            error: "",
            message: "",
            kitDetails: {},
            disableSaveButton: false,
            validationErrors: []
        }
        this.handleSaveButton = this.handleSaveButton.bind(this);
        this.updateError = this.updateError.bind(this);
    }

    updateError(error: string) {
        let messageWithAddedNote: string = '';
        if (error != '')
            messageWithAddedNote = messageWithAddedNote.concat("Please Note: ", error)

        this.setState({
            error: null, message: messageWithAddedNote
        })

    }


    validateDataBeforeSaving() {
        let dataToValidate: any = this.state.kitDetails.kitLinkGroupLocaleList;
        let error: any = [];
        if (checkDuplicateInObject("displaySequence", dataToValidate)) {
            error.push("Display Order must be unique at link group level.")
        }
        dataToValidate.forEach(function (element: any) {
            let validateSumMinToMax: Boolean = true;
            let compareMinMax: Boolean = true;
            let isMaxLgNumber: Boolean = true;
            let validateSumDefaultToMax: Boolean = true;
            let validateSumMaxModifierToMin: Boolean = true;

            if (checkDuplicateInObject("displaySequence", element.kitLinkGroupItemLocaleList)) {
                error.push("Display Order must be unique for modifiers of link group:".concat(element.name, "."))
            }

            if (!isNumber(String(element.properties.Minimum)) || !checkValueLessThanOrEqualToMaxValue(element.Minimum, 10) || !checkValueMoreThanOrEqualToMinValue(element.Minimum, 0)) {
                error.push("Minimum for link group:".concat(element.name, " must be numeric and can have values from 0 to 10."));
                compareMinMax = false;
                isMaxLgNumber = false;
                validateSumMaxModifierToMin = false;
            }

            if (!isNumber(String(element.properties.Maximum)) || !checkValueLessThanOrEqualToMaxValue(element.Maximum, 10) || !checkValueMoreThanOrEqualToMinValue(element.Maximum, 1)) {
                error.push("Maximum for link group:".concat(element.name, " must be numeric and can have values from 1 to 10."));
                compareMinMax = false;
                validateSumMinToMax = false;
                validateSumDefaultToMax = false;
            }

            if (compareMinMax && parseInt(element.properties.Maximum) < parseInt(element.properties.Minimum)) {
                error.push("Maximum for link group:".concat(element.name, " cannot be less than Minimum."));
            }

            if (!isNumber(String(element.displaySequence)) || !checkValueMoreThanOrEqualToMinValue(element.displaySequence, 1)) {
                error.push("Display Order for link group:".concat(element.name, " must be numeric and greater than 0."));
            }
            
            if (!isNumber(String(element.properties.NumOfFreeToppings)) || (isMaxLgNumber && !checkValueLessThanOrEqualToMaxValue(element.properties.NumOfFreeToppings, element.properties.Maximum))) {
                error.push("Number of Free Toppings for link group:".concat(element.name, " must be numeric and cannot be more than maximum."));
            }

            if (!isNumber(String(element.properties.NumOfFreeToppings)) ||!checkValueMoreThanOrEqualToMinValue(element.properties.NumOfFreeToppings, 0)) {
                error.push("Number of Free Toppings for link group:".concat(element.name, " must be numeric and greater than 0."));
            }

            let sumOfMinimum: number = 0;
            let sumOfDefaults: number = 0;
            let sumOfMaximum: number = 0;
            element.kitLinkGroupItemLocaleList.forEach(function (innerElement: any) {

                let compareMinMaxforModifier: Boolean = true;
                let isMaxNumber: Boolean = true;
                let isDefaultPortionNumber: Boolean = true;
                let isMinNumber: Boolean = true;
                if (!isNumber(String(innerElement.properties.Minimum)) || !checkValueLessThanOrEqualToMaxValue(innerElement.properties.Minimum, 10) || !checkValueMoreThanOrEqualToMinValue(innerElement.properties.Minimum, 0)) {
                    error.push("Minimum for modifier:".concat(innerElement.name, " must be numeric and can have values from 0 to 10."));
                    compareMinMaxforModifier = false;
                    validateSumMinToMax = false;
                    isMinNumber = false;
                }
                else {
                    sumOfMinimum = sumOfMinimum + parseInt(innerElement.properties.Minimum);
                }

                if (!isNumber(String(innerElement.properties.Maximum)) || !checkValueLessThanOrEqualToMaxValue(innerElement.properties.Maximum, 10) || !checkValueMoreThanOrEqualToMinValue(innerElement.properties.Maximum, 1)) {
                    error.push("Maximum for modifier:".concat(innerElement.name, " must be numeric and can have values from 1 to 10."));
                    compareMinMaxforModifier = false;
                    isMaxNumber = false;
                    validateSumMaxModifierToMin = false;
                }
                else {
                    sumOfMaximum = sumOfMaximum + parseInt(innerElement.properties.Maximum);
                }

                if (compareMinMaxforModifier && parseInt(innerElement.properties.Maximum) < parseInt(innerElement.properties.Minimum)) {
                    error.push("Maximum for modifier:".concat(innerElement.name, " cannot be less than Minimum."));
                }
                if (!isNumber(String(innerElement.displaySequence)) || !checkValueMoreThanOrEqualToMinValue(innerElement.displaySequence, 1)) {
                    error.push("Display Order for modifier:".concat(innerElement.name, " must be numeric and greater than 0."));
                }
                if (!isNumber(String(innerElement.properties.NumOfFreePortions)) || (isMaxNumber && !checkValueLessThanOrEqualToMaxValue(innerElement.properties.NumOfFreePortions, innerElement.properties.Maximum))) {

                    error.push("Number of Free Toppings for modifier:".concat(innerElement.name, " must be numeric and cannot be more than Maximum."));
                }

                if (!isNumber(String(innerElement.properties.NumOfFreePortions)) || !checkValueMoreThanOrEqualToMinValue(innerElement.properties.NumOfFreePortions, 0)) {

                    error.push("Number of Free Toppings for modifier:".concat(innerElement.name, " must be numeric and greater than 0."));
                }

                if (!isNumber(String(innerElement.properties.DefaultPortions)) || (isMaxNumber && !checkValueLessThanOrEqualToMaxValue(innerElement.properties.DefaultPortions, innerElement.properties.Maximum))) {
                    error.push("Default Portions for modifier:".concat(innerElement.name, " must be numeric and cannot be more than Maximum."));
                    isDefaultPortionNumber = false;
                    validateSumDefaultToMax = false;

                }
                else {
                    sumOfDefaults = sumOfDefaults + parseInt(innerElement.properties.DefaultPortions);
                }

                if (isDefaultPortionNumber && isMaxNumber && innerElement.properties.DefaultPortions > innerElement.properties.Maximum) {
                    error.push("Default Portions for modifier:".concat(innerElement.name, " cannot be more than Maximum."));
                }

                if (isDefaultPortionNumber && isMinNumber && !checkValueMoreThanOrEqualToMinValue(innerElement.properties.DefaultPortions, innerElement.properties.Minimum)) {
                    error.push("Default Portions for modifier:".concat(innerElement.name, " cannot be less than Minimum."));
                }
            })

            if (validateSumMaxModifierToMin && sumOfMaximum < element.properties.Minimum) {
                error.push("Sum of Maximum of modifiers cannot be less than Minimum of link group:".concat(element.name, "."));
            }

            if (validateSumMinToMax && sumOfMinimum > element.properties.Maximum) {
                error.push("Sum of Minimum of modifiers cannot be more than Maximum of link group:".concat(element.name, "."));
            }
            if (validateSumDefaultToMax && sumOfDefaults > element.properties.Maximum) {
                error.push("Sum of Default Portions of modifiers cannot be more than Maximum of link group:".concat(element.name, "."));
            }

        });

        if (error.length > 0) {

            this.setState({ validationErrors: error })
            return false;
        }

        return true;
    }

    loadData(isSavingData: Boolean) {
        // render the kitId and localeId from hierarchy page
        var pathArray = window.parent.location.href.split('/');
        pathArray = pathArray.reverse();
        // calling the API to get the required information
      
        let url = urlStart + "/" + parseInt(pathArray[1]) + "/GetKitProperties/" + parseInt(pathArray[0]);
     
        axios.get(url, {})
            .then(response => {
                let kitLinkGroupsDetail = response.data;
                kitLinkGroupsDetail.kitLinkGroupLocaleList.map((linkGroup: any) => {
                    let disableControls: boolean = false;
                    if (linkGroup.properties == null) {
                        let newLinkGroupProperties = {
                            Minimum: "0",
                            Maximum: "0",
                            NumOfFreeToppings: "0",
                        }
                        linkGroup.properties = newLinkGroupProperties
                        linkGroup.excluded = false
                        linkGroup.childDisabled = false;
                        linkGroup.displaySequence = 0 // default display sequence
                    }
                    else {
                        linkGroup.childDisabled = linkGroup.excluded;
                        linkGroup.properties = JSON.parse(linkGroup.properties)
                    }
                    disableControls = linkGroup.childDisabled;
                    linkGroup.kitLinkGroupItemLocaleList.map((linkGroupItem: any) => {
                        if (linkGroupItem.properties == null) {
                            let newLinkGroupItemProps = {
                                Minimum: "0",
                                Maximum: "0",
                                NumOfFreePortions: "0",
                                DefaultPortions: "0",
                                MandatoryItem: "false",
                            }
                            linkGroupItem.properties = newLinkGroupItemProps
                            linkGroupItem.excluded = false
                            linkGroupItem.isDisabled = disableControls
                            linkGroupItem.displaySequence = 0 // default display sequence
                        }
                        else {
                            linkGroupItem.isDisabled = disableControls;
                            linkGroupItem.properties = JSON.parse(linkGroupItem.properties)
                        }
                    })
                })

                this.setState({ kitDetails: kitLinkGroupsDetail }, () => {
                    if (isSavingData) {
                        this.setState({
                            error: null, message: "Data Saved Succesfully.", disableSaveButton: false,
                             validationErrors: []
                        })
                    }
                })

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

    componentDidMount() {
        this.loadData(false)
    }
    saveData() {

        if (!this.validateDataBeforeSaving()) {
            this.setState({
                disableSaveButton: false
            })
            return;
        }
        this.state.kitDetails.kitLinkGroupLocaleList.map((linkGroup: any) => {
            if (linkGroup.properties != null) {
                linkGroup.properties = JSON.stringify(linkGroup.properties)
                linkGroup.lastModifiedBy = "Priyanka"
            }
            linkGroup.kitLinkGroupItemLocaleList.map((linkGroupItem: any) => {
                if (linkGroupItem.properties != null) {
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
                this.loadData(false);
                this.setState({
                    error: "Error in Saving Data.", message: null, disableSaveButton: false
                })
            });

    }
    handleSaveButton(event: any) {
        //setting the properties
        this.setState({ disableSaveButton: true }, () => {
            this.saveData();
        })
    }

    render() {
        const kitLinkGroupLocaleList = this.state.kitDetails.kitLinkGroupLocaleList;
        const validationErrors = this.state.validationErrors || [];
        if (typeof kitLinkGroupLocaleList == "undefined") {
            return <div></div>
        }
        else {

            return <React.Fragment>
                {/*add the inner components into this components
            1. Location search dropdown
            2. KitLinkGroup
            3. KitLinkGroupItem
            4. buttons to perform save */}
                {validationErrors.length > 0 ?

                    <Grid container justify="center">
                        <ul className="error-message">
                            {validationErrors.map((e: any, i: any) => <li style={hStyle} key={i}>{e}</li>)}
                        </ul>
                    </Grid>
                    : <></>}


                <Grid container justify="center">
                    <Grid container md={12} justify="center">
                        <div className="error-message" ref="error">

                            <span className="text-danger"> {this.state.error}</span>
                        </div>
                    </Grid>
                    <Grid container md={12} justify="center">
                        <div className="Suncess-message" >
                            <span className="text-success"> {this.state.message}</span>
                        </div>
                    </Grid>
                </Grid>
                <div className="mt-md-4 mb-md-4">
                    <div className="row">
                        <div className="col-lg-4 col-md-5">  {/* Kit Name*/}
                            <h2 className="text-center font-italic font-weight-bold mt-md-5">{this.state.kitDetails.description}</h2>
                        </div>
                        <div className="col-lg-4 col-md-3"> {/* location dropdown */}
                            <h5 className="text-center mt-md-3">{this.state.kitDetails.localeName}<br></br></h5>
                        </div>
                    </div>
                    <div className="row">
                        <div className="col-lg-1 col-md-1 col-sm-1"></div> {/* added for the right spacing */}
                        <div className="col-lg-10 col-md-10 col-sm-10">  {/* add main components here */}
                            <form className="mb-md-4 border-top">
                                {
                                    this.state.kitDetails.kitLinkGroupLocaleList.map((kitLG: any) => <KitLinkGroupProperties updateError={this.updateError} kitLinkGroupDetails={kitLG} />)
                                }
                            </form>
                            <div className="row">{/* Save and Publish buttons  */}
                                <div className="col-lg-2 col-md-2 col-sm-2"></div>
                                <button className="col-lg-2 col-md-2 col-sm-2 btn btn-primary" type="button" onClick={this.handleSaveButton}> Publish </button>
                                <div className="col-lg-4 col-md-4 col-sm-4"></div>
                                <button disabled={this.state.disableSaveButton} className="col-lg-2 col-md-2 col-sm-2 btn btn-success" type="button" onClick={this.handleSaveButton}> Save Changes </button>
                                <div className="col-lg-2 col-md-2 col-sm-2"></div>
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
