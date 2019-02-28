import * as React from "react";
import Grid from "@material-ui/core/Grid";
import Typography from "@material-ui/core/Typography";
import Avatar from "@material-ui/core/Avatar";
import Icon from "@material-ui/core/Icon";
import "./style.css";

export default function PageTitle(props: any) {
  return (
    <Grid
      container
      justify="flex-start"
      alignItems="center"
      spacing={16}
      className="search-title"
    >
      <Grid item>
        <Avatar className="instructions-avatar">
          <Icon>{props.icon}</Icon>
        </Avatar>
      </Grid>
      <Grid item>
        <Typography variant="h6" color="textSecondary">
          {props.children}
        </Typography>
      </Grid>
    </Grid>
  );
}
