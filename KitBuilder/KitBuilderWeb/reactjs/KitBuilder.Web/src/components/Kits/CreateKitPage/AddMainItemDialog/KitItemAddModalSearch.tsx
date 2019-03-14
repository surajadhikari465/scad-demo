import * as React from "react";
import { Grid, Button, TextField } from "@material-ui/core";
import { withStyles } from "@material-ui/core/styles";

const styles = (theme: any) => ({
  searchButtons: {
    width: "100%"
  }
});

interface IKitItemAddModalLabelSearchBoxState {
    nameSearchBox: string,
    pluSearchBox: string,
}

interface IKitItemAddModalLabelSearchBoxProps {
    onSearch: any,
    classes: any,
}

class KitItemAddModalLabelSearchBox extends React.PureComponent<IKitItemAddModalLabelSearchBoxProps,IKitItemAddModalLabelSearchBoxState> {
  constructor(props: any) {
    super(props);
    this.state = {
        nameSearchBox: "",
        pluSearchBox: "",
    };
  }

  handleNameSearchBoxChange = (e: any) => {
    this.setState({ nameSearchBox: e.target.value });
  }

  handlePluSeachBoxChange = (e: any) => {
      this.setState({ pluSearchBox: e.target.value });
  }

  handleSearchSubmit = () => {
      const { nameSearchBox, pluSearchBox } = this.state;
      console.log("nameSearchBox");
      this.props.onSearch(nameSearchBox, pluSearchBox);
  }

  render() {
    return (
      <React.Fragment>
        <Grid container justify="flex-start" spacing={16}>
          <Grid item md={6}>
            <TextField
              label="Name"
              variant="outlined"
              InputLabelProps={{ shrink: true }}
              onChange={this.handleNameSearchBoxChange}
              value={this.state.nameSearchBox}
            />
          </Grid>
          <Grid item md={6}>
            <TextField
              label="PLU"
              variant="outlined"
              InputLabelProps={{ shrink: true }}
              onChange={this.handlePluSeachBoxChange}
              value={this.state.pluSearchBox}
            />
          </Grid>

          <Grid item md={12}>
            <Grid container justify="flex-end">
              <Button
                variant="contained"
                color="primary"
                className={this.props.classes.searchButtons}
                onClick={this.handleSearchSubmit}
              >
                Search
              </Button>
            </Grid>
          </Grid>
        </Grid>
      </React.Fragment>
    );
  }
}
export default withStyles(styles, { withTheme: true })(
  KitItemAddModalLabelSearchBox
);
