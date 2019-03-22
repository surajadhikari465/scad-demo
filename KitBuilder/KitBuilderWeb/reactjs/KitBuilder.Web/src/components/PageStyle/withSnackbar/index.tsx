import * as React from "react";
import { Snackbar, SnackbarContent } from "@material-ui/core";
import './style.css';

const withSnackbar = (WrappedComponent: any) => {
  return (props: any) => {
    const [message, setMessage] = React.useState("");
    const [type, setType] = React.useState(""); //error or success
    const [open, setOpen] = React.useState(false);

    const validTypes = ["success", "error", "default"];

    const showAlert = (message: string, type: string = "") => {
      setMessage(message);
      if(validTypes.includes(type.toLowerCase())) 
        setType(type);
      else
        setType("default");
      setOpen(true);
    };

    const handleClose = () => setOpen(false);
    return (
      <>
        <WrappedComponent {...props} showAlert={showAlert} />
        <Snackbar
          anchorOrigin={{
            vertical: "top",
            horizontal: "center"
          }}
          open={open}
          autoHideDuration={6000}
          onClose={handleClose}
          ContentProps={{
            "aria-describedby": "message-id"
          }}
        >
        <SnackbarContent className={`snackbar-display snackbar-${type}`} message={<span>{message}</span>}/>
        </Snackbar>
      </>
    );
  };
};
export default withSnackbar;
