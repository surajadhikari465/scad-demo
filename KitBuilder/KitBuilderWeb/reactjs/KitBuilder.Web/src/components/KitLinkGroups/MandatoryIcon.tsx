import React from "react";
import { withStyles, Chip } from "@material-ui/core";

const styles = {
  root: {
    height: 24
  },
  label: {
    paddingRight: 5,
    paddingLeft: 5
  }
};

interface IMandatoryIconProps {
  classes: any;
  disabled: boolean;
}

function MandatoryIcon(props: IMandatoryIconProps) {
  return (
    <Chip
      label="MANDATORY"
      color={props.disabled ? "default" : "primary"}
      classes={{ root: props.classes.root, label: props.classes.label }}
    />
  );
}

export default withStyles(styles)(MandatoryIcon);
