import * as React from "react";
import axios from "axios";
import { Grid, Button, Paper } from "@material-ui/core";
import KitLinkGroupProperties from "./KitLinkGroupProperties";
import { KbApiMethod } from "../helpers/kbapi";
import "./style.css";
import {
  isNumber,
  checkDuplicateInObject,
  checkValueLessThanOrEqualToMaxValue,
  checkValueMoreThanOrEqualToMinValue
} from "../KitLinkGroups/ValidateFunctions";
import PageTitle from "../PageTitle";
import Footer from '../PageStyle/Footer';
import withSnackbar from '../PageStyle/withSnackbar';

var urlStart = KbApiMethod("Kits");

interface IKitLinkGroupPageState {
  kitDetails: any;
  disableSaveButton: boolean;
  errors: {
    kitLinkGroups: { kitLinkGroupId: number , message: string }[],
    kitLinkGroupItems: { kitLinkGroupItemId: number, message: string }[],
  }
}

interface IKitLinkGroupPageProps {
  kitId: number;
  localeId: number;
  showAlert(messag: string, type?: string): void;
}

interface IKitLinkGroupLocale {
  properties?: {
    Minimum?: number;
    Maximum?: number;
    NumOfFreeToppings?: number;
  };
  excluded?: boolean;
  childDisabled?: boolean;
  displaySequence?: number;
}

class KitLinkGroupPage extends React.Component<
  IKitLinkGroupPageProps,
  IKitLinkGroupPageState
> {
  constructor(props: any) {
    super(props);
    this.state = {
      kitDetails: {},
      disableSaveButton: false,
      errors: {
        kitLinkGroups: [],
        kitLinkGroupItems: [],
      },
    };
    this.handleSaveButton = this.handleSaveButton.bind(this);
    this.updateError = this.updateError.bind(this);
  }

  updateKitLinkGroupProperties = (
    kitLinkGroupId: number,
    linkGroupLocale: IKitLinkGroupLocale
  ) => {
    let properties = linkGroupLocale.properties || {};

    const kitLinkGroupLocaleList = this.state.kitDetails.kitLinkGroupLocaleList.map(
      (lg: any) =>
        lg.kitLinkGroupId === kitLinkGroupId
          ? {
              ...lg,
              ...linkGroupLocale,
              properties: { ...lg.properties, ...properties }
            }
          : lg
    );

    this.setState({
      kitDetails: { ...this.state.kitDetails, kitLinkGroupLocaleList }
    });
  };

  updateKitLinkGroupItemProperties = (
    kitLinkGroupId: number,
    kitLinkGroupItemId: number,
    linkGroupLocale: IKitLinkGroupLocale
  ) => {
    let properties = linkGroupLocale.properties || {};

    const kitLinkGroupLocale = this.state.kitDetails.kitLinkGroupLocaleList.find(
      (lg: any) => lg.kitLinkGroupId === kitLinkGroupId
    );

    if (kitLinkGroupLocale) {
      const kitLinkGroupItemLocaleList = kitLinkGroupLocale.kitLinkGroupItemLocaleList.map(
        (item: any) =>
          item.kitLinkGroupItemId === kitLinkGroupItemId
            ? {
                ...item,
                ...linkGroupLocale,
                properties: { ...item.properties, ...properties }
              }
            : item
      );

      this.updateKitLinkGroupProperties(kitLinkGroupId, {
        ...kitLinkGroupLocale,
        kitLinkGroupItemLocaleList
      });
    }
  };

  updateError(error: string) {
    let messageWithAddedNote: string = "";
    if (error != "")
      messageWithAddedNote = messageWithAddedNote.concat(
        "Please Note: ",
        error
      );

    this.props.showAlert(messageWithAddedNote, "error");
  }
  filterByExcluded = (item: any) => !item.excluded;

  validateDataBeforeSaving() {

    const kitLinkGroups: { kitLinkGroupId: number , message: string }[] = [];
    const kitLinkGroupItems:  { kitLinkGroupItemId: number , message: string }[] = [];

    let dataToValidate: any = this.state.kitDetails.kitLinkGroupLocaleList;
    let error: any = [];

    if (
      checkDuplicateInObject(
        "displaySequence",
        dataToValidate.filter(this.filterByExcluded)
      )
    ) {
      error.push("Display Order must be unique at link group level.");
    }
    let self = this;

    dataToValidate.forEach(function(element: any) {
      let validateSumMinToMax: Boolean = true;
      let compareMinMax: Boolean = true;
      let isMaxLgNumber: Boolean = true;
      let validateSumDefaultToMax: Boolean = true;
      let validateSumMaxModifierToMin: Boolean = true;
      let LinkGroupExcluded: Boolean = element.excluded;
      let validateLinkGroupMaxToSumMaxModifier: Boolean = true;

      if (!LinkGroupExcluded) {
        if (
          element.kitLinkGroupItemLocaleList.filter(self.filterByExcluded)
            .length == 0
        ) {
          kitLinkGroups.push({kitLinkGroupId: element.kitLinkGroupId, message: "Must have at least one included modifier"});
        }

        if (
          checkDuplicateInObject(
            "displaySequence",
            element.kitLinkGroupItemLocaleList.filter(self.filterByExcluded)
          )
        ) {
          kitLinkGroups.push({kitLinkGroupId: element.kitLinkGroupId, message: "Display Order must be unique for modifiers of link group"});
        }

        if (
          !isNumber(String(element.properties.Minimum)) ||
          !checkValueLessThanOrEqualToMaxValue(element.Minimum, 10) ||
          !checkValueMoreThanOrEqualToMinValue(element.Minimum, 0)
        ) {
          kitLinkGroups.push({kitLinkGroupId: element.kitLinkGroupId, message: "Minimum for link group must be 0 to 10"});
          compareMinMax = false;
          isMaxLgNumber = false;
          validateSumMaxModifierToMin = false;
        }

        if (
          !isNumber(String(element.properties.Maximum)) ||
          !checkValueLessThanOrEqualToMaxValue(element.Maximum, 10) ||
          !checkValueMoreThanOrEqualToMinValue(element.Maximum, 1)
        ) {
          kitLinkGroups.push({kitLinkGroupId: element.kitLinkGroupId, message: "Maximum for link group must be 0 to 10."});
          compareMinMax = false;
          validateSumMinToMax = false;
          validateLinkGroupMaxToSumMaxModifier = false;
          validateSumDefaultToMax = false;
        }

        if (
          compareMinMax &&
          parseInt(element.properties.Maximum) <
            parseInt(element.properties.Minimum)
        ) {
          kitLinkGroups.push({kitLinkGroupId: element.kitLinkGroupId, message: "Maximum for link group cannot be less than Mimimum."});
        }

        if (
          !isNumber(String(element.displaySequence)) ||
          !checkValueMoreThanOrEqualToMinValue(element.displaySequence, 1)
        ) {
          kitLinkGroups.push({kitLinkGroupId: element.kitLinkGroupId, message: "Display order for link group must be greater than 0."});
        }

        if (
          !isNumber(String(element.properties.NumOfFreeToppings)) ||
          (isMaxLgNumber &&
            !checkValueLessThanOrEqualToMaxValue(
              element.properties.NumOfFreeToppings,
              element.properties.Maximum
            ))
        ) {
          kitLinkGroups.push({kitLinkGroupId: element.kitLinkGroupId, message: "Free portions must not be more than maximum."});
        }

        if (
          !isNumber(String(element.properties.NumOfFreeToppings)) ||
          !checkValueMoreThanOrEqualToMinValue(
            element.properties.NumOfFreeToppings,
            0
          )
        ) {
          kitLinkGroups.push({kitLinkGroupId: element.kitLinkGroupId, message: "Free portions must be greater or equal to 0."});
        }

        let sumOfMinimum: number = 0;
        let sumOfDefaults: number = 0;
        let sumOfMaximum: number = 0;
        element.kitLinkGroupItemLocaleList.forEach(function(innerElement: any) {
          let compareMinMaxforModifier: Boolean = true;
          let isMaxNumber: Boolean = true;
          let isDefaultPortionNumber: Boolean = true;
          let isMinNumber: Boolean = true;
          let isLinkGroupItemExcluded: Boolean = innerElement.excluded;
          if (!isLinkGroupItemExcluded) {
            if (
              !isNumber(String(innerElement.properties.Minimum)) ||
              !checkValueLessThanOrEqualToMaxValue(
                innerElement.properties.Minimum,
                10
              ) ||
              !checkValueMoreThanOrEqualToMinValue(
                innerElement.properties.Minimum,
                0
              )
            ) {
              kitLinkGroupItems.push({ kitLinkGroupItemId: innerElement.kitLinkGroupItemId, message: "Minimum must be between 1 and 10."});
              compareMinMaxforModifier = false;
              validateSumMinToMax = false;
              isMinNumber = false;
            } else {
              sumOfMinimum =
                sumOfMinimum + parseInt(innerElement.properties.Minimum);
            }
            if (innerElement.properties.Maximum > element.properties.Maximum) {
              kitLinkGroupItems.push({ kitLinkGroupItemId: innerElement.kitLinkGroupItemId, message: "Modifier maximum cannot be greater than link group maximum."});

            }
            if (
              !isNumber(String(innerElement.properties.Maximum)) ||
              !checkValueLessThanOrEqualToMaxValue(
                innerElement.properties.Maximum,
                10
              ) ||
              !checkValueMoreThanOrEqualToMinValue(
                innerElement.properties.Maximum,
                1
              )
            ) {
              kitLinkGroupItems.push({ kitLinkGroupItemId: innerElement.kitLinkGroupItemId, message: "Maximum must be between 1 and 10."});
              compareMinMaxforModifier = false;
              isMaxNumber = false;
              validateSumMaxModifierToMin = false;
              validateLinkGroupMaxToSumMaxModifier = false;
            } else {
              sumOfMaximum =
                sumOfMaximum + parseInt(innerElement.properties.Maximum);
            }

            if (
              compareMinMaxforModifier &&
              parseInt(innerElement.properties.Maximum) <
                parseInt(innerElement.properties.Minimum)
            ) {
              kitLinkGroupItems.push({ kitLinkGroupItemId: innerElement.kitLinkGroupItemId, message: "Maximum cannot be less than minimum."});
            }
            if (
              !isNumber(String(innerElement.displaySequence)) ||
              !checkValueMoreThanOrEqualToMinValue(
                innerElement.displaySequence,
                1
              )
            ) {
              kitLinkGroupItems.push({ kitLinkGroupItemId: innerElement.kitLinkGroupItemId, message: "Display order must be greater than 0."});
            }
            if (
              !isNumber(String(innerElement.properties.NumOfFreePortions)) ||
              (isMaxNumber &&
                !checkValueLessThanOrEqualToMaxValue(
                  innerElement.properties.NumOfFreePortions,
                  innerElement.properties.Maximum
                ))
            ) {
              kitLinkGroupItems.push({ kitLinkGroupItemId: innerElement.kitLinkGroupItemId, message: "Free portions cannot be greater than Maximum."});
            }

            if (
              !isNumber(String(innerElement.properties.NumOfFreePortions)) ||
              !checkValueMoreThanOrEqualToMinValue(
                innerElement.properties.NumOfFreePortions,
                0
              )
            ) {
              kitLinkGroupItems.push({ kitLinkGroupItemId: innerElement.kitLinkGroupItemId, message: "Free portions must be greater or equal to 0."});
            }

            if (
              !isNumber(String(innerElement.properties.DefaultPortions)) ||
              (isMaxNumber &&
                !checkValueLessThanOrEqualToMaxValue(
                  innerElement.properties.DefaultPortions,
                  innerElement.properties.Maximum
                ))
            ) {
              kitLinkGroupItems.push({ kitLinkGroupItemId: innerElement.kitLinkGroupItemId, message: "Default portions cannot be more than maximum."});
              isDefaultPortionNumber = false;
              validateSumDefaultToMax = false;
            } else {
              sumOfDefaults =
                sumOfDefaults +
                parseInt(innerElement.properties.DefaultPortions);
            }

            if (
              isDefaultPortionNumber &&
              isMaxNumber &&
              innerElement.properties.DefaultPortions >
                innerElement.properties.Maximum
            ) {
              kitLinkGroupItems.push({ kitLinkGroupItemId: innerElement.kitLinkGroupItemId, message: "Default portions cannot be more than maximum."});
            }

            if (
              isDefaultPortionNumber &&
              isMinNumber &&
              !checkValueMoreThanOrEqualToMinValue(
                innerElement.properties.DefaultPortions,
                innerElement.properties.Minimum
              )
            ) {
              kitLinkGroupItems.push({ kitLinkGroupItemId: innerElement.kitLinkGroupItemId, message: "Default portions cannot be less than minimum."});
            }
          }
        });

        if (
          validateSumMaxModifierToMin &&
          sumOfMaximum < element.properties.Minimum
        ) {
          kitLinkGroups.push({ kitLinkGroupId: element.kitLinkGroupId, message: "Total maximum of all modifiers cannot be less than link group minimum."});
        }

        if (
          validateLinkGroupMaxToSumMaxModifier &&
          sumOfMaximum < element.properties.Maximum
        ) {
          kitLinkGroups.push({ kitLinkGroupId: element.kitLinkGroupId, message: "Total maximum of modifiers cannot be less than maximum for the link group."});
        }

        if (validateSumMinToMax && sumOfMinimum > element.properties.Maximum) {
          kitLinkGroups.push({ kitLinkGroupId: element.kitLinkGroupId, message: "Total minimum of modifiers cannot be more than maximum for the link group."});
        }
        if (
          validateSumDefaultToMax &&
          sumOfDefaults > element.properties.Maximum
        ) {
          kitLinkGroups.push({ kitLinkGroupId: element.kitLinkGroupId, message: "Total default portions of modifiers cannot be more than maximum for the link group."});
        }
      }
    });

    if (kitLinkGroups.length > 0 || kitLinkGroupItems.length > 0) {
      this.setState({ errors: { ...this.state.errors, kitLinkGroups, kitLinkGroupItems } });
      this.props.showAlert("There were input validation errors, please correct and try again.", "error")
      return false;
    }

    return true;
  }

  loadData(isSavingData: Boolean) {
    // render the kitId and localeId from hierarchy page
    var pathArray = window.parent.location.href.split("/");
    pathArray = pathArray.reverse();
    // calling the API to get the required information

    let url =
      urlStart +
      "/" +
      parseInt(pathArray[1]) +
      "/GetKitProperties/" +
      parseInt(pathArray[0]);

    axios
      .get(url, {})
      .then(response => {
        let kitLinkGroupsDetail = response.data;
        kitLinkGroupsDetail.kitLinkGroupLocaleList.map((linkGroup: any) => {
          let disableControls: boolean = false;
          if (linkGroup.properties == null) {
            let newLinkGroupProperties = {
              Minimum: "0",
              Maximum: "0",
              NumOfFreeToppings: "0"
            };
            linkGroup.properties = newLinkGroupProperties;
            linkGroup.excluded = false;
            linkGroup.childDisabled = false;
            linkGroup.displaySequence = 0; // default display sequence
          } else {
            linkGroup.childDisabled = linkGroup.excluded;
            linkGroup.properties = JSON.parse(linkGroup.properties);
          }
          disableControls = linkGroup.childDisabled;
          linkGroup.kitLinkGroupItemLocaleList.map((linkGroupItem: any) => {
            if (linkGroupItem.properties == null) {
              let newLinkGroupItemProps = {
                Minimum: "0",
                Maximum: "0",
                NumOfFreePortions: "0",
                DefaultPortions: "0",
                MandatoryItem: "false"
              };
              linkGroupItem.properties = newLinkGroupItemProps;
              linkGroupItem.excluded = false;
              linkGroupItem.isDisabled = disableControls;
              linkGroupItem.isExcludeDisabled = disableControls;
              linkGroupItem.displaySequence = 0; // default display sequence
            } else {
              linkGroupItem.isDisabled =
                disableControls || linkGroupItem.excluded;
              linkGroupItem.isExcludeDisabled = disableControls;
              linkGroupItem.properties = JSON.parse(linkGroupItem.properties);
            }
          });
        });

        this.setState({ kitDetails: kitLinkGroupsDetail }, () => {
          if (isSavingData) {
            this.setState({
              disableSaveButton: false,
            });
            this.props.showAlert("Data saved succesfully.", "success")
          }
        });
      })
      .catch(error => {
        this.props.showAlert("Error in getting data from API.");
        return;
      });
  }

  componentDidMount() {
    this.loadData(false);
  }
  saveData() {
    if (!this.validateDataBeforeSaving()) {
      this.setState({
        disableSaveButton: false
      });
      return;
    }
    this.state.kitDetails.kitLinkGroupLocaleList.map((linkGroup: any) => {
      if (linkGroup.properties != null) {
        linkGroup.properties = JSON.stringify(linkGroup.properties);
        linkGroup.lastModifiedBy = "Priyanka";
      }
      linkGroup.kitLinkGroupItemLocaleList.map((linkGroupItem: any) => {
        if (linkGroupItem.properties != null) {
          linkGroupItem.properties = JSON.stringify(linkGroupItem.properties);
          linkGroupItem.lastModifiedBy = "Priyanka";
        }
      });
    });
    let { kitDetails } = this.state;
    let urlKitSave = urlStart + "/" + kitDetails.kitId;
    var headers = {
      "Content-Type": "application/json",
      "Access-Control-Allow-Origin": "*"
    };
    axios
      .post(urlKitSave, JSON.stringify(kitDetails), {
        headers: headers
      })
      .then(response => {
        this.loadData(true);
      })
      .catch(error => {
        this.loadData(false);
        this.setState({
          disableSaveButton: false
        });
        this.props.showAlert("Error in Saving Data.", "error");
      });
  }
  handleSaveButton(event: any) {
    //setting the properties
    this.setState({ disableSaveButton: true }, () => {
      this.saveData();
    });
  }

  render() {
    const kitLinkGroupLocaleList = this.state.kitDetails.kitLinkGroupLocaleList;
    if (typeof kitLinkGroupLocaleList == "undefined") {
      return <div />;
    } else {
      return (
        <React.Fragment>
          <Grid container justify="center">
            <Grid item xs={10}>
              <PageTitle>
                {this.state.kitDetails.localeName} >{" "}
                {this.state.kitDetails.description}
              </PageTitle>
            </Grid>
          </Grid>
                <Grid container spacing={16} justify="center">
                  {this.state.kitDetails.kitLinkGroupLocaleList.map(
                    (kitLG: any) => (
                      <Grid item xs={10} key={kitLG.kitLinkGroupId}>
                        <KitLinkGroupProperties
                          kitLinkGroupErrors={this.state.errors.kitLinkGroups.filter((e) => e.kitLinkGroupId === kitLG.kitLinkGroupId)}
                          kitLinkGroupItemErrors={this.state.errors.kitLinkGroupItems}
                          updateError={this.updateError}
                          kitLinkGroupDetails={kitLG}
                          updateKitLinkGroupItemProperties={
                            this.updateKitLinkGroupItemProperties
                          }
                          updateKitLinkGroupProperties={
                            this.updateKitLinkGroupProperties
                          }
                        />
                      </Grid>
                    )
                  )}
                 
                </Grid>
                <Footer/>
          <Paper square style={{position: "fixed", bottom: 0, width: "100%", padding: 8}}>
          <Grid container justify="center">
          <Grid item xs={10}>
          <Grid container justify="flex-end">
          <Grid item>
                  <Button
                  variant="contained"
                  color="primary"
                    disabled={this.state.disableSaveButton}
                    onClick={this.handleSaveButton}
                  >
                    {" "}
                    Save Changes{" "}
                  </Button>
                  </Grid>
                  </Grid>
                  </Grid>
                  </Grid>
                  </Paper>
                  
        </React.Fragment>
      );
    }
  }
}

export default withSnackbar(KitLinkGroupPage);
