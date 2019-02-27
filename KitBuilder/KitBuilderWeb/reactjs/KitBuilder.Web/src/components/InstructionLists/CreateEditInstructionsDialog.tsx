import * as React from "react";
import {
  Dialog,
  DialogTitle,
  DialogContent,
  Grid,
  TextField,
  FormControl,
  FormLabel,
  RadioGroup,
  FormControlLabel,
  Button,
  Radio
} from "@material-ui/core";

interface CreateEdiInstructionDialogState {
  instructionName: string;
  instructionType: string;
}

interface CreateEdiInstructionDialogProps {
  isOpen: boolean;
  isEditInstructions: boolean;
  currentInstructionTypeValue: string;
  onCancel: () => void;
  updateCurrentInstructionName: (name: string) => void;
  createNewInstruction: (name: string, type: string) => void;
  onDelete: (event: React.MouseEvent<HTMLElement>) => void;
  onClose: (event: React.SyntheticEvent) => void;
}

export default class CreateEdiInstructionDialog extends React.PureComponent<
  CreateEdiInstructionDialogProps,
  CreateEdiInstructionDialogState
> {
  constructor(props: CreateEdiInstructionDialogProps) {
    super(props);
    this.state = {
      instructionName: "",
      instructionType: ""
    };
  }

  componentDidUpdate(prevProps: CreateEdiInstructionDialogProps) {
    // we want to make sure we change our state if the prop changes
    if (this.props.currentInstructionTypeValue !== prevProps.currentInstructionTypeValue) {
      this.setState({
        instructionName: this.props.currentInstructionTypeValue
      });
    } 
    // if we change the dialog to new mode, we should clear the list name input
    else if (this.props.isEditInstructions === false && prevProps.isEditInstructions === true) {
        this.setState({
            instructionName: "",
          }); 
    }
  }

  handleCreateNewInstruction = (event: React.MouseEvent<HTMLElement>) => {
    this.props.createNewInstruction(
      this.state.instructionName,
      this.state.instructionType
    );
  };

  handleUpdateCurrentInstructionName =  (event: React.MouseEvent<HTMLElement>) => {
    this.props.updateCurrentInstructionName(this.state.instructionName);
  }

  handleInstructionNameChange = (
    event: React.ChangeEvent<HTMLInputElement>
  ) => {
    this.setState({
      instructionName: event.target.value
    });
  };

  handleInstructionTypeChange = (
    event: React.ChangeEvent<HTMLInputElement>
  ) => {
    this.setState({
      instructionType: event.target.value
    });
  };

  handleCancel = (event: React.MouseEvent<HTMLElement>) => {
    const { currentInstructionTypeValue, onCancel } = this.props;
    this.setState({ instructionName: currentInstructionTypeValue }, onCancel);
  }

  render() {
    const {
      isOpen,
      onClose,
      onDelete,
      isEditInstructions
    } = this.props;
    const { instructionName, instructionType } = this.state;

    return (
      <Dialog
        open={isOpen}
        onClose={onClose}
        aria-labelledby="form-dialog-title"
      >
        <DialogTitle id="form-dialog-title">
          {isEditInstructions ? "Edit" : "New"} Instruction List
        </DialogTitle>
        <DialogContent>
          <Grid container className="form-container">
            <Grid item xs={12}>
              <FormControl>
                <TextField
                  id="standard-name"
                  label="Name"
                  variant="outlined"
                  InputLabelProps={{ shrink: true }}
                  value={instructionName}
                  onChange={this.handleInstructionNameChange}
                />
              </FormControl>
            </Grid>
            {!isEditInstructions && (
              <Grid item xs={12}>
                <FormControl>
                  <Grid container justify="space-between" alignItems="center">
                    <FormLabel>Type</FormLabel>
                    <RadioGroup
                      aria-label="type"
                      name="type"
                      value={instructionType}
                      onChange={this.handleInstructionTypeChange}
                      row
                    >
                      <FormControlLabel
                        value="1"
                        control={<Radio color="primary" />}
                        label="Cooking"
                      />
                      <FormControlLabel
                        value="2"
                        control={<Radio color="primary" />}
                        label="General"
                      />
                    </RadioGroup>
                  </Grid>
                </FormControl>
              </Grid>
            )}
            <Grid container id="add-edit-buttons">
              <Grid item xs={12} md={6} className="mb-3 pr-3">
                <Button
                  onClick={
                    isEditInstructions
                      ? this.handleUpdateCurrentInstructionName
                      : this.handleCreateNewInstruction
                  }
                  variant="contained"
                  color="primary"
                  className="full-width"
                >
                  {isEditInstructions ? "Done" : "Create"}
                </Button>
              </Grid>
              <Grid item xs={12} md={6} className="pr-2">
                <Button
                  onClick={this.handleCancel}
                  variant="outlined"
                  color="primary"
                  className="full-width"
                >
                  Cancel
                </Button>
              </Grid>
              {isEditInstructions && (
                <Grid item xs={12} md={6} className="pl-2">
                  <Button
                    onClick={onDelete}
                    variant="outlined"
                    color="secondary"
                    className="full-width"
                  >
                    Delete
                  </Button>
                </Grid>
              )}
            </Grid>
          </Grid>
        </DialogContent>
      </Dialog>
    );
  }
}
