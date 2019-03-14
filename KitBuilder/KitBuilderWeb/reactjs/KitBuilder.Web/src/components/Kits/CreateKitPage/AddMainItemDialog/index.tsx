import * as React from "react";
import KitItemAddModalLabelSearchBox from "./KitItemAddModalSearch";
import KitItemAddModalDisplay from "./KitItemAddModalDisplay";
import { Grid, Button } from "@material-ui/core";
import Dialog from "@material-ui/core/Dialog";
import DialogTitle from "@material-ui/core/DialogTitle";
import DialogContent from "@material-ui/core/DialogContent";
import DialogActions from "@material-ui/core/DialogActions";
import { KbApiMethod } from 'src/components/helpers/kbapi';
const hStyle = { color: "red" };
const successStyle = { color: "blue" };
const ModalStyle = {
  margin: "auto",
  width: "500px",
  marginTop: "10px",
  overlay: { zIndex: 10 }
};

interface IKitItemAddModelProps {
  open: boolean,
  onClose: any,
  onMainItemSelected: any,
}

interface IKitItemAddModelState {
  searchResults: Array<any>,
  error: string,
  message: string,
}

const buildItemSearchUrl = (name: string, plu: string) => {
  const base = KbApiMethod("Items");
  let searchBy, searchValue;
  
  if(name) {
      searchBy = "ProductDesc";
      searchValue = name;
  }
  else if(plu) {
      searchBy = "ScanCode";
      searchValue = plu;
  } else {
    return;
  }

  return `${base}?${searchBy}=${searchValue}`;
}

class AddMainItemDialog extends React.PureComponent<IKitItemAddModelProps, IKitItemAddModelState> {
  constructor(props: IKitItemAddModelProps) {
    super(props);
    this.state = {
      searchResults: [],
      error: "",
      message: "",
    };
  }

  handleSearch = (itemName: string, itemPlu: string) => {
    console.log(itemName);
    const url = buildItemSearchUrl(itemName, itemPlu);

    if(!url) return;

    fetch(url)
    .then(response => response.json())
    .then((searchResults) => {
      this.setState({searchResults})
    })
    .catch((error) => {
      this.setState( error );
    });
  }

  handleMainItemSelected = (item: any) => {
    this.props.onMainItemSelected(item);
  }

  handleClose = () => {
    this.setState({ searchResults: [] });
  }

  render() {
    return (
      <React.Fragment>
        <Dialog 
        open={this.props.open}
        onClose={this.handleClose}
        >
          <Grid container justify="center">
            <DialogTitle id="form-dialog-title">
              Kit Item Add Screen
            </DialogTitle>
          </Grid>
          <DialogContent>
            <Grid container justify="center">
              <Grid container justify="center">
                <div className="error-message">
                  <span style={hStyle}> {this.state.error}</span>
                </div>
              </Grid>
              <Grid container justify="center">
                <div className="Success-message">
                  <span style={successStyle}> {this.state.message}</span>
                </div>
              </Grid>
            </Grid>

            <Grid container justify="center" style={ModalStyle}>
              <Grid item md={10}>
                <KitItemAddModalLabelSearchBox
                  onSearch={this.handleSearch}
                />
              </Grid>
                <Grid item md={10}>
                  <KitItemAddModalDisplay
                    data={this.state.searchResults}
                    onSelected={this.props.onMainItemSelected}
                  />
                </Grid>
            </Grid>
          </DialogContent>
          <DialogActions>
            <Button onClick={this.props.onClose} color="primary">
              Cancel
            </Button>
          </DialogActions>
        </Dialog>
      </React.Fragment>
    );
  }
}

export default AddMainItemDialog;
