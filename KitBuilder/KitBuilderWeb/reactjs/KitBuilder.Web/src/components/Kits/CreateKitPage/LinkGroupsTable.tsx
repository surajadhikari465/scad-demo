import * as React from "react";
import LinkedGroupsRow from "./LinkGroupsRow";
import "./style.css";
import { LinkGroup, LinkGroupItem } from "../../../types/LinkGroup";
import StyledPanel from 'src/components/PageStyle/StyledPanel';
import { Grid, Typography } from '@material-ui/core';

interface ILinkGroupTableProps {
  linkGroups: Array<LinkGroup>;
  errors: Array<any>;
  onItemSelected: (item: LinkGroupItem) => void;
  onItemUnselected: (item: LinkGroupItem) => void;
  onLinkGroupRemoved: (linkedGroup: LinkGroup) => void;
  selectedLinkGroupItems: Array<LinkGroupItem>;
  disabledLinkGroups: Array<any>;
  onBulkSelectChange: any;
}

export default function LinkGroupTable(props: ILinkGroupTableProps) {
  return (<StyledPanel>
    <div className="striped-table">
      {
        props.linkGroups.length ?
          (
            props.linkGroups.map((linkGroup, index) => (
              <div className={['striped-table-row', props.disabledLinkGroups.indexOf(linkGroup.linkGroupId) >= 0 ? 'nonePointer'  :'normalPointer'].join(" ") } key={index}>
                <LinkedGroupsRow
                  error={props.errors.find(x => x.linkGroupId === linkGroup.linkGroupId)}
                  linkedGroup={linkGroup}
                  selectedLinkedGroupItems={props.selectedLinkGroupItems}
                  onItemSelected={props.onItemSelected}
                  onItemUnselected={props.onItemUnselected}
                  onLinkedGroupDeleted={props.onLinkGroupRemoved}
                  disabledLinkGroups={props.disabledLinkGroups}
                />
              </div>
            ))
          ) : (
            <Grid container justify="center" alignItems="center" className="linked-groups-table-placeholder">
              <Grid item>
                <Typography color="textSecondary">No Link Groups</Typography>
              </Grid>
            </Grid>)
      }
    </div>
  </StyledPanel>);
}
