import * as React from "react";
import { Grid, Button } from "@material-ui/core";
import { Item } from "src/types/LinkGroup";

export default function MainItemDisplay(props: {
  item: Item | null;
  selectMainItem: any;
}) {
  return (
    <Grid container justify="space-between" alignItems="center">
      <Grid item>
        {props.item ? props.item.productDesc : null}
      </Grid>
      <Grid item>
        <b>{props.item ? props.item.scanCode : null}</b>
      </Grid>
      <Grid item>
        <Button
          onClick={props.selectMainItem}
          variant="contained"
          color="primary"
        >
          Select Main Item
        </Button>
      </Grid>
    </Grid>
  );
}
