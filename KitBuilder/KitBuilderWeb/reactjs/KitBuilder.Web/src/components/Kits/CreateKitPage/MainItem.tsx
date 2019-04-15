import * as React from "react";
import { Grid } from "@material-ui/core";
import { Item } from "src/types/LinkGroup";

export default function MainItemDisplay(props: {
  item: Item | null;
}) {
  return (
    <Grid container alignItems="center" spacing={16}>
     <Grid item>
        <b>{props.item ? props.item.scanCode : null}</b>
      </Grid>
      <Grid item>
        {props.item ? props.item.productDesc : null}
      </Grid>
    </Grid>
  );
}
