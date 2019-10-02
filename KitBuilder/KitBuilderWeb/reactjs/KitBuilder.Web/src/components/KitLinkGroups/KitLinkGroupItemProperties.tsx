import * as React from "react";
import { Grid, TextField, Checkbox, FormHelperText } from "@material-ui/core";
import DisplayPosition from "./DisplayPosition";
import MandatoryIcon from './MandatoryIcon';
import * as Constants from '../Constants/KitPropertiesLimits'

interface LinkGroupItemError {
  kitLinkGroupItemId: number;
  message: string;
}

interface IKitLinkGroupItemPropertiesProps {
  updateError(message: string): void;
  onUpdateItem(properties: any): void;
  errors: LinkGroupItemError[];
  kitLinkGroupItemsJson: any;
  isParentExcluded: boolean;
}

export class KitLinkGroupItemProperties extends React.Component<
  IKitLinkGroupItemPropertiesProps
> {
  generateErrorHelperText = () => {
    const errors = this.props.errors;
    if (errors) {
      return errors.map((error: LinkGroupItemError) => (
        <FormHelperText error={true}>{error.message}</FormHelperText>
      ));
    } else return [];
  };
  /* setState methods */
  onfocusOut(event: React.ChangeEvent<HTMLElement>) {
    this.props.updateError("");
  }
  handleLinkGroupItemMinimum = (event: React.ChangeEvent<HTMLInputElement>) => {
    let minimum = parseInt(event.target.value);
    if (minimum <= Constants.KITPROPERTYNUMERICLIMITS.UPLIMIT && minimum >= Constants.KITPROPERTYNUMERICLIMITS.DOWNLIMIT) {
      this.props.onUpdateItem({
        properties: {
          Minimum: minimum,
          MandatoryItem: minimum > 0 ? "true" : "false"
        }
      });
    }
  };

  handleLinkGroupItemMaximum = (event: React.ChangeEvent<HTMLInputElement>) => {
    let maximum = parseInt(event.target.value);

    if (maximum <= Constants.KITMAXPROPERTYLIMITS.UPLIMIT && maximum >= Constants.KITMAXPROPERTYLIMITS.DOWNLIMIT) {
      this.props.onUpdateItem({ properties: { Maximum: maximum } });
    }
  };

  handleLinkGroupItemNumOfFreePortions = (
    event: React.ChangeEvent<HTMLInputElement>
  ) => {
    if (
      event.target.value.trim() !== ""
    ) {
      let numberOfFreePortions = parseInt(event.target.value);

      if (numberOfFreePortions <= Constants.KITPROPERTYNUMERICLIMITS.UPLIMIT && numberOfFreePortions >= Constants.KITPROPERTYNUMERICLIMITS.DOWNLIMIT) {
        this.props.onUpdateItem({
          properties: { NumOfFreePortions: numberOfFreePortions }
        });
      }
    }
    else
    {
      this.props.onUpdateItem({
        properties: { NumOfFreePortions: "" }
      })
    }
  };

  handleLinkGroupItemDefaultPortions = (
    event: React.ChangeEvent<HTMLInputElement>
  ) => {
    const freePortions = parseInt(event.target.value);
    if (freePortions <= Constants.KITPROPERTYNUMERICLIMITS.UPLIMIT && freePortions >= Constants.KITPROPERTYNUMERICLIMITS.DOWNLIMIT) {
      this.props.onUpdateItem({
        properties: { DefaultPortions: parseInt(event.target.value) }
      });
    }
  };

  handleLinkGroupItemExclude = () => {
    let changedState = !this.props.kitLinkGroupItemsJson.excluded;
    this.props.onUpdateItem({
      isDisabled: changedState,
      excluded: changedState
    });
  };

  handleLinkGroupItemMandatoryItem = () => {
    let changedState = "";
    if (this.props.kitLinkGroupItemsJson.properties.MandatoryItem == "true") {
      changedState = "false";
    } else if (
      this.props.kitLinkGroupItemsJson.properties.MandatoryItem == "false"
    ) {
      changedState = "true";
    }
    this.props.onUpdateItem({ properties: { MandatoryItem: changedState } });
  };

  handlePositionChange = (position: number) => {
    if(!this.props.kitLinkGroupItemsJson.excluded && position > 0) {
    this.props.onUpdateItem({ displaySequence: position });
    }
  };

  /* html render method */
  render() {
    const { kitLinkGroupItemsJson, isParentExcluded } = this.props;

    const disabled = kitLinkGroupItemsJson.excluded || isParentExcluded;

    return (
      <Grid
        id="linkgroup-item-root-container"
        container
        spacing={8}
        style={
          disabled ? { backgroundColor: "#f2f2f2", color: "grey" }
          : { backgroundColor: "#f2f2f2" }
        }
      >
        <Grid item>
          <Grid container alignItems="center" alignContent="center">
            <Grid item xs={12}>
              <Grid container spacing = {16}>
                <Grid item xs={12}>
                  {this.generateErrorHelperText()}
                </Grid>
                <Grid item>
                  <h6>{kitLinkGroupItemsJson.name}</h6>
                </Grid>
                <Grid item>
                {kitLinkGroupItemsJson.properties.Minimum > 0 && <MandatoryIcon disabled={disabled}/>}
                </Grid>
              </Grid>
            </Grid>
          </Grid>
        </Grid>
        <Grid item xs={12}>
          <Grid
            id="linkgroup-item-root-property-row-container"
            container
            spacing={8}
          >
            <Grid item xs={12}>
              <Grid
                id="linkgroup-item-properties-container"
                container
                spacing={8}
                justify="space-between"
                alignItems="flex-end"
                alignContent="flex-end"
                style={{ height: "100%", paddingBottom: 10 }}
              >
                <Grid item>
                  Exclude
                  <Checkbox
                    disabled = {isParentExcluded}
                    classes={{ input: "included-checkbox" }}
                    checked={kitLinkGroupItemsJson.excluded}
                    onChange={this.handleLinkGroupItemExclude}
                  />
                </Grid>
                <Grid item>
                  <TextField
                  label="Min"
                  disabled = {disabled}
                    style={{ border: "none" }}
                    className="number-input"
                    type="number"
                    value={kitLinkGroupItemsJson.properties.Minimum}
                    onChange={this.handleLinkGroupItemMinimum}
                  />
                </Grid>
                <Grid item>
                  <TextField
                  label="Max"
                  disabled = {disabled}
                    className="number-input"
                    type="number"
                    value={kitLinkGroupItemsJson.properties.Maximum}
                    onChange={this.handleLinkGroupItemMaximum}
                    fullWidth
                  />
                </Grid>

                <Grid item>
                  <TextField
                  disabled = {disabled}
                    label="Default"
                    className="number-input"
                    type="number"
                    value={kitLinkGroupItemsJson.properties.DefaultPortions}
                    onChange={this.handleLinkGroupItemDefaultPortions}
                    fullWidth
                  />
                </Grid>
                <Grid item>
                  <TextField
                  disabled = {disabled}
                    label="Free"
                    className="number-input"
                    type="number"
                    value={kitLinkGroupItemsJson.properties.NumOfFreePortions}
                    onChange={this.handleLinkGroupItemNumOfFreePortions}
                    fullWidth
                  />
                </Grid>
                <Grid item>
              <DisplayPosition
              disabled = {disabled}
                position={kitLinkGroupItemsJson.displaySequence}
                onPositionChange={this.handlePositionChange}
              />
            </Grid>
              </Grid>
            </Grid>
            
          </Grid>
        </Grid>
      </Grid>
    );
  }
}