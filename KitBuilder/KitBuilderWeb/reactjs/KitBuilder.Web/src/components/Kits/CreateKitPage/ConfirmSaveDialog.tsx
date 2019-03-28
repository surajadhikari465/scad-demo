import * as React from "react";
import {
  Dialog,
  DialogContent,
  DialogContentText,
  DialogActions,
  Button
} from "@material-ui/core";

interface IConfirmSaveDialogProps {
  onClose: any;
  onCreateKit: any;
  isEdit: boolean;
  open: boolean;
}

export default function ConfirmSaveDialog(props: IConfirmSaveDialogProps) {
  return (
    <Dialog
      open={props.open}
      onClose={props.onClose}
      aria-labelledby="alert-dialog-title"
      aria-describedby="alert-dialog-description"
    >
      <DialogContent>
        <DialogContentText>
          { props.isEdit ?
          "Are you sure you want to save this kit?" :
          "Are you sure you want to create a new kit?"
          }
        </DialogContentText>
      </DialogContent>
      <DialogActions>
        <Button onClick={props.onClose} color="primary">
          Cancel
        </Button>
        <Button
          onClick={props.onCreateKit}
          variant="outlined"
          color="primary"
          autoFocus
        >
          Confirm
        </Button>
      </DialogActions>
    </Dialog>
  );
}
