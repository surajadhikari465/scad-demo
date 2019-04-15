import * as React from "react";
import Grid from "@material-ui/core/Grid";
import TextField from "@material-ui/core/TextField";
import Button from "@material-ui/core/Button";
import { withStyles } from "@material-ui/core/styles";
import Select from "@material-ui/core/Select";
import MenuItem from "@material-ui/core/MenuItem";
import Checkbox from "@material-ui/core/Checkbox";
import ListItemText from "@material-ui/core/ListItemText";
import { OutlinedInput, InputLabel, FormControl } from "@material-ui/core";

const styles = (theme: any) => ({
  root: {
    flexGrow: 1
  },
  label: {
    fontSize: 20,
    textAlign: "right" as "right",
    marginBottom: 0 + " !important",
    paddingRight: 10 + "px"
  },
  button: {
    margin: theme.spacing.unit
  },
  searchButtons: {
    width: "100%",
  },
  formControl: {
    margin: theme.spacing.unit,
    minWidth: 120,
    width: "100%"
  }
});

const ITEM_HEIGHT = 48;
const ITEM_PADDING_TOP = 8;
const MenuProps = {
  PaperProps: {
    style: {
      maxHeight: ITEM_HEIGHT * 4.5 + ITEM_PADDING_TOP,
      width: 250
    }
  }
};

interface IProps {
  regions: string[];
  classes: any;
  searchOptions: any;
  onChange(event: any): void;
  onSearch(name: string, desc: string, modName: string, plu: string, region: string): void;
  onAddNew(): void;
  onRegionsChanged(selectedRegions: string[]): void;
  showSearchProgress: boolean;
}

interface IState {
  name: string;
  description: string;
  inputRegion: string;
  plu: string;
  selectedRegions: Array<string>;
  searchBy: string;
}

class SearchLinkGroups extends React.Component<IProps, IState> {
  constructor(props: IProps) {
    super(props);
    this.state = {
      name: "",
      description: "",
      inputRegion: "",
      plu: "",
      selectedRegions: [],
      searchBy: "LinkGroup"
    };
  }

  handleChange = (event: any) => {
    this.setState({ selectedRegions: event.target.value });
  };

  handleInputChange = (event: any) => {
    const { searchBy } = this.state;
    const field = event.target.name;
    if (field === "name") {
        this.setState({ name: event.target.value })
    } else { 
        searchBy === "Modifier"
        ?
            this.setState({ plu: event.target.value })
        :
            this.setState({ description: event.target.value });
}
  };

  handleSearchByChange = (event: any) => {
    const searchBy = event.target.value;

    if (searchBy === "LinkGroup") {
      this.setState({ name: "", plu: "", searchBy });
    } else if (searchBy === "Modifier") {
      this.setState({
        name: "",
        description: "",
        searchBy
      });
    }
    this.setState({ searchBy });
  };

  handleSearch = () =>{
      const { name, description, plu, searchBy, selectedRegions } = this.state;
    if (searchBy === "LinkGroup") {
        this.props.onSearch(name, description, "", "", selectedRegions.join(","))
    } else if (searchBy === "Modifier") {
        this.props.onSearch("", "", name, plu, selectedRegions.join(","))
    }
  }

  handlePressEnterToSearch = (e: React.KeyboardEvent) => {
    e = e || window.event;
    const ENTER = 13;

    if(e.keyCode == ENTER){
       this.handleSearch();
    }
}

  render() {
      const { searchBy, name, description, plu } = this.state;
    return (
      <React.Fragment>
        <Grid container spacing={16} justify="space-between" className="px-3 mb-3" alignContent="center" onKeyUp = {this.handlePressEnterToSearch}>
          <Grid item md={2}>
          <FormControl>
          <InputLabel
           variant="outlined"
           htmlFor="outlined-search-by"
          >
            Search By
          </InputLabel>
            <Select
              value ={this.state.searchBy}
              renderValue={value => value}
              fullWidth
              onChange={this.handleSearchByChange}
              input={
                <OutlinedInput
                  fullWidth
                  labelWidth={70}
                  id="outlined-search-by"
                  />
              }
            >
              <MenuItem value="LinkGroup">Link Group</MenuItem>
              <MenuItem value="Modifier">Modifier</MenuItem>
            </Select>
            </FormControl>
          </Grid>
          <Grid item md={3}>
            <TextField
              label= {searchBy === "LinkGroup" ? "Link Group Name" : "Modifier Name"}
              fullWidth
              name="name"
              value = {name}
              onChange={this.handleInputChange}
              variant="outlined"
              InputLabelProps={{ shrink: true }}
            />
          </Grid>
          <Grid item md={3}>
            <TextField
              label= {searchBy === "LinkGroup" ? "Link Group Description" : "Modifier PLU"}
              fullWidth
              name="descOrPlu"
              value = {searchBy === "LinkGroup" ? description : plu}
              onChange={this.handleInputChange}
              variant="outlined"
              InputLabelProps={{ shrink: true }}
            />
          </Grid>
          <Grid item md={2}>
          <FormControl>
          <InputLabel
          variant="outlined"
           shrink = {true}
            htmlFor="outlined-regions"
          >
            Regions
          </InputLabel>
            <Select
              multiple
              fullWidth
              value={this.state.selectedRegions}
              onChange={this.handleChange}
              input={
                <OutlinedInput
                  fullWidth
                  labelWidth ={60}
                  notched={true}
                  id="outlined-regions"/>
              }
              renderValue={(selected: []) => {
                return selected.length > 0 ? selected.join(", ") : "All Regions";
              }}
              MenuProps={MenuProps}
            >
              {this.props.regions.map((region: any) => (
                <MenuItem key={region.regionCode} value={region.regionCode}>
                  <Checkbox
                    checked={
                      this.state.selectedRegions.indexOf(region.regionCode) > -1
                    }
                  />
                  <ListItemText primary={region.regionCode} />
                </MenuItem>
              ))}
            </Select>
            </FormControl>
          </Grid>
          <Grid item md={2}>
          <Grid container alignContent="center">
           <Grid item md={12}>
           
            <Button
              variant="contained"
              color="primary"
              fullWidth
              onClick={this.handleSearch}
              >
              Search
            </Button>
          </Grid>
              </Grid>
             </Grid>
        </Grid>
      </React.Fragment>
    );
  }
}

export default withStyles(styles)(SearchLinkGroups);
