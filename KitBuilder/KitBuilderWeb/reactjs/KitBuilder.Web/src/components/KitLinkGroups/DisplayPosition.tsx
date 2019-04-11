import React from "react";
import { TextField } from "@material-ui/core";

interface IDisplayPosistionProps {
  position: number;
  disabled?: boolean;
  onPositionChange(position: number): void;
}

const DisplayPosition = (props: IDisplayPosistionProps) => {

  const handlePositionChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const position = parseInt(event.target.value);
    props.onPositionChange(position);
  }

  return (
    <>
      <TextField label="Display" className = "number-input" disabled={props.disabled} type="number" onChange = {handlePositionChange} value={props.position} fullWidth/>
    </>
  );
};

export default DisplayPosition;
