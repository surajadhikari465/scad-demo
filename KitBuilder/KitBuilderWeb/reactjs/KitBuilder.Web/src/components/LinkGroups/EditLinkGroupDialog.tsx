import React from "react";
import { Dialog } from "@material-ui/core";
import EditLinkGroup from "./EditLinkGroup";

const EditLinkGroupDialog = (props: any) => {
  return (
    <Dialog
    fullWidth = {true}
    maxWidth = "lg"
    open = {props.isOpen}
    onClose = {props.onClose}
    >
      <EditLinkGroup
        data={props.selectedLinkGroup}
        handleCancelClick={props.onClose}
        clearSearchResults={props.clearSearchResults}
        />
    </Dialog>
  );
};

export default EditLinkGroupDialog;
