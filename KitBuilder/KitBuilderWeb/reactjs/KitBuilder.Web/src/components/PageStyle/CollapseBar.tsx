import React from "react";
import { Grid, Button } from "@material-ui/core";
import { KeyboardArrowDown, KeyboardArrowUp } from "@material-ui/icons";

const CollapseBar = (props: {
  collapsed: boolean;
  onOpen(): void;
  onClose(): void;
}) => {
  return (
    <Grid container justify="flex-end">
      <Grid item>
        {props.collapsed ? (
          <Button onClick={props.onOpen}>
            <KeyboardArrowDown />
          </Button>
        ) : (
          <Button onClick={props.onClose}>
            <KeyboardArrowUp />
          </Button>
        )}
      </Grid>
    </Grid>
  );
};
export default CollapseBar;
