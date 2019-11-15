import * as React from "react";
import SelectLocaleSearch from "./SelectLocaleSearch";
import SelectLocaleDisplay from "./SelectLocaleDisplay";
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

interface ISearchLocaleProps {
  openPopUp: boolean,
  onlocaleSelect: any,
  closePopUp:any
}

interface ISearchLocaleState {
  searchResults: Array<any>,
  error: string,
  message: string,
  searchLocaleName: string,
  searchStoreAbbreviation: string,
  searchBusinessUnitId: string
}



class LocaleSearchDialog extends React.PureComponent<ISearchLocaleProps, ISearchLocaleState> {
  constructor(props: ISearchLocaleProps) {
    super(props);

    this.state = {
      searchResults: [],
      error: "",
      message: "",
      searchLocaleName: "",
      searchStoreAbbreviation: "",
      searchBusinessUnitId: ""
    };
    this.onSearch = this.onSearch.bind(this);
    this.localeNameChange = this.localeNameChange.bind(this);
    this.storeAbbreviationChange = this.storeAbbreviationChange.bind(this);
    this.businessUnitIdChange = this.businessUnitIdChange.bind(this);
    this.onlocaleSelect = this.onlocaleSelect.bind(this);
  }

  localeNameChange(event: any) {
    this.setState({ searchLocaleName: event.target.value });
  }

  storeAbbreviationChange(event: any) {
    this.setState({ searchStoreAbbreviation: event.target.value });
}

  businessUnitIdChange(event: any) {
    var inputValue = event.target.value;
    if (isNaN(inputValue))
    {
      inputValue = "";
    }
    this.setState({ searchBusinessUnitId: inputValue });
}

  onSearch() {
    var urlStart = KbApiMethod("LocalesSearch");
    if (this.state.searchLocaleName == "" && this.state.searchStoreAbbreviation == "" && this.state.searchBusinessUnitId == "") {
        this.setState({
            error: "Please enter at least one select criteria.", message: ""
        });

        return;

    }
    var urlParam ="";

    if (this.state.searchLocaleName != "")
        urlParam = urlParam + "LocaleName=" + this.state.searchLocaleName + "&"

    if (this.state.searchStoreAbbreviation != "")
        urlParam =urlParam + "StoreAbbreviation=" + this.state.searchStoreAbbreviation + "&"

    if (this.state.searchBusinessUnitId != "")
        urlParam =urlParam + "BusinessUnitId=" + this.state.searchBusinessUnitId + "&"
    
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

   onlocaleSelect = (item: any) => {

    this.setState({ searchResults: [],searchLocaleName:"", searchStoreAbbreviation:"", 
      searchBusinessUnitId:"" })
      this.props.onlocaleSelect(item);   
  }

  handleClose = () => {
    this.setState({ searchResults: [],searchLocaleName:"", searchStoreAbbreviation:"", 
      searchBusinessUnitId:"" });

    this.props.closePopUp();
  }

  render() {
    console.log(this.state.searchLocaleName);
    return (
      <React.Fragment>
        <Dialog 
        open={this.props.openPopUp}
        onClose={this.handleClose}       
        >
          <Grid container justify="center">
            <DialogTitle id="form-dialog-title">
             Search Location
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
                <SelectLocaleSearch
                  onSearch={this.onSearch}
                  LocaleName = { this.localeNameChange}
                  StoreAbbreviation = { this.storeAbbreviationChange}
                  BusinessUnitId= { this.businessUnitIdChange}
                  LocaleNameValue= { this.state.searchLocaleName}
                  StoreAbbreviationValue= { this.state.searchStoreAbbreviation}
                  BusinessUnitIdValue= { this.state.searchBusinessUnitId}
                />
              </Grid>
                <Grid item md={12}>
                  <SelectLocaleDisplay
                    data={this.state.searchResults}
                    onSelected={this.onlocaleSelect}
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

export default LocaleSearchDialog;
