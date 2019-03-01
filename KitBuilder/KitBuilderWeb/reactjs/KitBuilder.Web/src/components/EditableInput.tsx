import * as React from "react";
import { FormControl, Input, FormHelperText, Grid } from "@material-ui/core";

interface EditableContentProps {
  isValid: (value: string) => string;
  onChange: (value: string) => void;
  value: string;
  emptyLabel: string;
}

interface EditableContentState {
  editMode: boolean;
  error: string;
  value: string;
}

export const isNumberError = (value: string) => {
  const result = /^[0-9]+$/.test(value);
  return result ? "" : "Input must be a number";
};

export const isLettersNumbers = (value: string) => {
  const result = /^[a-zA-Z0-9-_! '"*)(]+$/.test(value);
  return result ? "" : "Invalid input";
};

export default class EditableContent extends React.PureComponent<
  EditableContentProps,
  EditableContentState
> {
  constructor(props: EditableContentProps) {
    super(props);
    this.state = {
      editMode: false,
      error: "",
      value: props.value
    };
  }

  handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const value = event.target.value;
    const error = this.props.isValid(value);
    this.setState({ error, value });

    // only update the value if it is valid
    if (!error) {
      this.props.onChange(value);
    }
  };

  handleBlur = () => {
    const { value } = this.props;
    this.setState({ value, editMode: false });
  };

  handleClick = () => {
    this.setState({ editMode: true, error: "" });
  };

  render() {
    const { editMode, value, error } = this.state;
    const { emptyLabel } = this.props;

    return (
      <Grid
        container
        style={{ height: "100%" }}
        justify="center"
        alignItems="center"
        onBlur={this.handleBlur}
        onClick={this.handleClick}
      >
        <Grid item>
          {editMode ? (
            <FormControl error={!!error}>
              {/* 
            // @ts-ignore  // we are dynamically forcing a focus  */}
              <Input
                inputRef={input => input && input.focus()}
                value={value}
                onChange={this.handleChange}
              />
              {error && <FormHelperText>{error}</FormHelperText>}
            </FormControl>
          ) : value ? (
            value
          ) : (
            <i style={{ color: "red" }}>{emptyLabel}</i>
          )}
        </Grid>
      </Grid>
    );
  }
}
