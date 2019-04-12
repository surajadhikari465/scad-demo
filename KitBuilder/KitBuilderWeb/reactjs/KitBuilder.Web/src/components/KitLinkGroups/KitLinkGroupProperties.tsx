import * as React from "react";
import { KitLinkGroupItemProperties } from "./KitLinkGroupItemProperties";
import { KeyboardArrowDown, KeyboardArrowUp } from "@material-ui/icons";
import {
  Paper,
  Grid,
  DialogContent,
  TextField,
  Divider,
  FormHelperText,
  Checkbox
} from "@material-ui/core";
import DisplayPosition from "./DisplayPosition";

interface IKitLinkGroupPropertiesProps {
  kitLinkGroupDetails: any;
  updateKitLinkGroupProperties: any;
  updateKitLinkGroupItemProperties: any;
  updateError: any;
  kitLinkGroupErrors: any[];
  kitLinkGroupItemErrors: any[];
}

interface IKitLinkGroupPropertiesState {
  showModifiers: boolean;
}

class KitLinkGroupProperties extends React.Component<
  IKitLinkGroupPropertiesProps,
  IKitLinkGroupPropertiesState
> {
  constructor(props: IKitLinkGroupPropertiesProps) {
    super(props);
    this.state = {
      showModifiers: true
    };
  }

  toggleShowModifiers = () => {
    const showModifiers = !this.state.showModifiers;
    this.setState({ showModifiers });
  };

  handleUpdateProperties = (properties: any) =>
    this.props.updateKitLinkGroupProperties(
      this.props.kitLinkGroupDetails.kitLinkGroupId,
      properties
    );

  onfocusOut = (event: any) => {
    this.props.updateError("");
  }
  /* setState methods */
  handleLinkGroupMinimum = (event: React.ChangeEvent<HTMLInputElement>) => {
    let minimum = parseInt(event.target.value);

    if (minimum <= 10 && minimum >= 0) {
      this.handleUpdateProperties({
        properties: { Minimum: minimum }
      });
    }
  }

  handleLinkGroupMaximum = (event: React.ChangeEvent<HTMLInputElement>) => {
    let maximum = parseInt(event.target.value);

    if (maximum <= 10 && maximum > 0) {
      this.handleUpdateProperties({
        properties: { Maximum: maximum }
      });
    }
  }

  handleLinkGroupDisplayOrder = (displayOrder: number) => {
    if (displayOrder > 0) {
      this.handleUpdateProperties({ displaySequence: displayOrder });
    }
  }

  handleLinkGroupNumOfFreeToppings = (event: React.ChangeEvent<HTMLInputElement>) => {
    let numberOfFreeToppings = parseInt(event.target.value);
    if (
      numberOfFreeToppings >= 0 &&
      numberOfFreeToppings <= 10
    ) {
      this.handleUpdateProperties({
        properties: { NumOfFreeToppings: numberOfFreeToppings }
      });
    }
  }

  toggleLinkGroupExclude = () => {
    this.handleUpdateProperties({
      excluded: !this.props.kitLinkGroupDetails.excluded
    });
  };

  generateErrorElements = () => {
    return this.props.kitLinkGroupErrors.map((error: any) => (
      <FormHelperText error={true}>{error.message}</FormHelperText>
    ));
  };

  render() {
    const disabled = this.props.kitLinkGroupDetails.excluded;
    const errorElements = this.generateErrorElements();
    return (
      <Paper square>
        <DialogContent>
          <Grid
            container
            style={
              this.props.kitLinkGroupDetails.excluded ? { color: "grey" } : {}
            }
          >
            <Grid item xs={12}>
              {errorElements}
            </Grid>

            <Grid item xs={12}>
              <Grid container spacing={16}>
                <Grid item>
                  <h4 style={errorElements.length > 0 ? {color: "red"} : {}}>{this.props.kitLinkGroupDetails.name}</h4>
                </Grid>
              </Grid>
            </Grid>
            <Grid item xs={12}>
              <Grid
                container
                justify="space-between"
                alignItems="flex-end"
                alignContent="flex-end"
              >
                <Grid item>
                  Exclude
                  <Checkbox
                    checked={disabled}
                    onClick={this.toggleLinkGroupExclude}
                  />
                </Grid>
                <Grid item>
                  <TextField
                    label="Min"
                    disabled={disabled}
                    className="number-input"
                    type="number"
                    value={this.props.kitLinkGroupDetails.properties.Minimum}
                    onChange={this.handleLinkGroupMinimum.bind(this)}
                  />
                </Grid>
                <Grid item>
                  <TextField
                    label="Max"
                    disabled={disabled}
                    className="number-input"
                    type="number"
                    value={this.props.kitLinkGroupDetails.properties.Maximum}
                    onChange={this.handleLinkGroupMaximum.bind(this)}
                  />
                </Grid>
                <Grid item style={{ minWidth: 50 }} />
                <Grid item>
                  <TextField
                    label="Free"
                    disabled={disabled}
                    className="number-input"
                    type="number"
                    value={
                      this.props.kitLinkGroupDetails.properties
                        .NumOfFreeToppings
                    }
                    onChange={this.handleLinkGroupNumOfFreeToppings.bind(this)}
                  />
                </Grid>
                <Grid item>
                  <DisplayPosition
                    disabled={disabled}
                    position={this.props.kitLinkGroupDetails.displaySequence}
                    onPositionChange={this.handleLinkGroupDisplayOrder}
                  />
                </Grid>
              </Grid>
            </Grid>
          </Grid>

          <Grid item xs={12}>
            <Grid item xs={12}>
              <Grid container spacing={16}>
                <Divider
                  style={{
                    width: "100%",
                    marginTop: "16px",
                    marginBottom: "16px"
                  }}
                />
                <Grid item xs={12}>
                  <Grid container justify="flex-end">
                    <Grid item>
                      {this.state.showModifiers ? (
                        <KeyboardArrowUp onClick={this.toggleShowModifiers} />
                      ) : (
                        <KeyboardArrowDown onClick={this.toggleShowModifiers} />
                      )}
                    </Grid>
                  </Grid>
                </Grid>
                {!this.state.showModifiers ? null : this.props.kitLinkGroupDetails.kitLinkGroupItemLocaleList.map(
                  (kitLGI: any) => (
                    <Grid item xs={12}>
                      <KitLinkGroupItemProperties
                        isParentExcluded={
                          this.props.kitLinkGroupDetails.excluded
                        }
                        errors={this.props.kitLinkGroupItemErrors.filter(
                          (error: any) =>
                            error.kitLinkGroupItemId ===
                            kitLGI.kitLinkGroupItemId
                        )}
                        updateError={this.props.updateError}
                        kitLinkGroupItemsJson={kitLGI}
                        onUpdateItem={(properties: any) =>
                          this.props.updateKitLinkGroupItemProperties(
                            this.props.kitLinkGroupDetails.kitLinkGroupId,
                            kitLGI.kitLinkGroupItemId,
                            properties
                          )
                        }
                      />
                    </Grid>
                  )
                )}
              </Grid>
            </Grid>
          </Grid>
        </DialogContent>
      </Paper>
    );
  }
}

export default KitLinkGroupProperties;
