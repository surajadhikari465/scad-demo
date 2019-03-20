import * as React from "react";
import SelectKitSearch from "./SelectKitSearch";
import SelectKitDisplay from "./SelectKitDisplay";
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

interface ISearchKitProps {
  openPopUp: boolean,
  onkitSelect: any,
  closePopUp:any
}

interface ISearchKitState {
  searchResults: Array<any>,
  error: string,
  message: string,
  searchMainItemName: string,
  searchScanCode: string,
  searchLinkGroupName: string,
  searchkitDescription: string
}



class KitSearchDialog extends React.PureComponent<ISearchKitProps, ISearchKitState> {
  constructor(props: ISearchKitProps) {
    super(props);

    this.state = {
      searchResults: [],
      error: "",
      message: "",
      searchMainItemName: "",
      searchScanCode: "",
      searchLinkGroupName: "",
      searchkitDescription : ""
    };
    this.onSearch = this.onSearch.bind(this);
    this.mainItemChange = this.mainItemChange.bind(this);
    this.scanCodeChange = this.scanCodeChange.bind(this);
    this.linkGroupChange = this.linkGroupChange.bind(this);
    this.kitDescriptionChange = this.kitDescriptionChange.bind(this);
    this.onkitSelect = this.onkitSelect.bind(this);
  }

  mainItemChange(event: any) {
    this.setState({ searchMainItemName: event.target.value });
  }

scanCodeChange(event: any) {
    this.setState({ searchScanCode: event.target.value });
}

linkGroupChange(event: any) {
    this.setState({ searchLinkGroupName: event.target.value });
}

kitDescriptionChange(event: any) {
    this.setState({ searchkitDescription: event.target.value });
}

  onSearch() {
    var urlStart = KbApiMethod("Kits");
    if (this.state.searchMainItemName == "" && this.state.searchScanCode == "" && this.state.searchkitDescription == "" && this.state.searchLinkGroupName == "") {
        this.setState({
            error: "Please enter at least one select criteria.", message: ""
        });

        return;

    }
    var urlParam ="";

    if (this.state.searchMainItemName != "")
        urlParam = urlParam + "ItemDescription=" + this.state.searchMainItemName + "&"

    if (this.state.searchScanCode != "")
        urlParam =urlParam + "ItemScanCode=" + this.state.searchScanCode + "&"

    if (this.state.searchkitDescription != "")
        urlParam =urlParam + "KitDescription=" + this.state.searchkitDescription + "&"

    if (this.state.searchLinkGroupName != "")
        urlParam = urlParam + "LinkGroupName=" + this.state.searchLinkGroupName+ "&"
    
        urlParam = urlParam.substring(0, urlParam.length - 1);

    var url = urlStart +"?"+  urlParam;

    fetch(url)
        .then(response => {
            return response.json();
        }).then((data)=> 
        {
            if(typeof(data) == undefined || data == null || data.length == 0)
            { 
                this.setState({
                    error: "No data found.", message: "", searchResults: []
                });
            }
           
            else
            {                   
                this.setState({
                error: "", message: "", searchResults: data
            });
            }
           
         }
        )
        .catch((error) => {
            console.log(error.response.data);
            this.setState({
                error: error.response.data, message: ""
            });
        });
}

   onkitSelect = (item: any) => {

    this.setState({ searchResults: [],searchMainItemName:"", searchScanCode:"", 
    searchkitDescription:"", searchLinkGroupName:"" })
      this.props.onkitSelect(item);   
  }

  handleClose = () => {
    this.setState({ searchResults: [],searchMainItemName:"", searchScanCode:"", 
                    searchkitDescription:"", searchLinkGroupName:"" });

    this.props.closePopUp();
  }

  render() {
    return (
      <React.Fragment>
        <Dialog 
        open={this.props.openPopUp}
        onClose={this.handleClose}       
        >
          <Grid container justify="center">
            <DialogTitle id="form-dialog-title">
             Search Kits
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

            <Grid container justify="flex-start" style={ModalStyle}>
              <Grid item md={12}>
                <SelectKitSearch
                  onSearch={this.onSearch}
                  mainItemName = { this.mainItemChange}
                  mainItemScanCode = { this.scanCodeChange}
                  linkGroupName= { this.linkGroupChange}
                  kitDescription= { this.kitDescriptionChange}
                  mainItemValue= { this.state.searchMainItemName}
                  scanCodeValue= { this.state.searchScanCode}
                  linkGroupValue= { this.state.searchLinkGroupName}
                  kitDescriptionValue= { this.state.searchkitDescription}
                />
              </Grid>
                <Grid item md={12}>
                  <SelectKitDisplay
                    data={this.state.searchResults}
                    onSelected={this.onkitSelect}
                  />
                </Grid>
            </Grid>
          </DialogContent>
          <DialogActions>
            <Button onClick={this.handleClose} color="primary">
              Cancel
            </Button>
          </DialogActions>
        </Dialog>
      </React.Fragment>
    );
  }
}

export default KitSearchDialog;
