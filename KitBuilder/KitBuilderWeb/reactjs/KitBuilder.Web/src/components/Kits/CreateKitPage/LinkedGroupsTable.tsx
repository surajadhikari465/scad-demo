import * as React from "react";
import LinkedGroupsRow from "./LinkedGroupsRow";
import "./style.css";
import { LinkedGroup, LinkedGroupItem } from "../../../types/LinkGroup";
import StyledPanel from 'src/components/PageStyle/StyledPanel';
import { Grid, Typography } from '@material-ui/core';

interface ILinkedGroupTableProps {
  linkedGroups: Array<LinkedGroup>;
  onItemSelected: (item: LinkedGroupItem) => void;
  onItemUnselected: (item: LinkedGroupItem) => void;
  onLinkedGroupRemoved: (linkedGroup: LinkedGroup) => void;
  selectedLinkedGroupItems: Array<LinkedGroupItem>
}

export default function LinkedGroupTable(props: ILinkedGroupTableProps) {
  return (<StyledPanel>
    <div className="stripped-table">
      {
        props.linkedGroups.length ?
        (
          props.linkedGroups.map((linkGroup, index) => (
        <p key={index}>
          <LinkedGroupsRow
            linkedGroup={linkGroup}
            selectedLinkedGroupItems={props.selectedLinkedGroupItems}
            onItemSelected = {props.onItemSelected}
            onItemUnselected = {props.onItemUnselected}
            onLinkedGroupDeleted={props.onLinkedGroupRemoved}
          />
        </p>
      ))
        ) : (
        <Grid container justify="center" alignItems="center" className="linked-groups-table-placeholder">
          <Grid item>
          <Typography color="textSecondary">No Linked Groups</Typography>
          </Grid>
        </Grid>)
    }
    </div>
    </StyledPanel>);
}
