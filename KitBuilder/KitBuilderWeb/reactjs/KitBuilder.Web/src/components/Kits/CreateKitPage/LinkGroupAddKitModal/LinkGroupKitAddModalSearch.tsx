import * as React from "react";
import { Grid, Button, TextField } from "@material-ui/core";
import { withStyles } from "@material-ui/core/styles";

const styles = (theme: any) => ({
  searchButtons: {
    width: "100%",
  }
});

interface IAddLinkedGroupDialogProps {
  classes: any;
  onSubmit: (name: string, plu: string) => void;
}

interface IAddLinkedGroupDialogState {
  name: string;
  plu: string;
}

class LinkGroupKitAddModalSearch extends React.PureComponent<
  IAddLinkedGroupDialogProps,
  IAddLinkedGroupDialogState
> {
  constructor(props: IAddLinkedGroupDialogProps) {
    super(props);
    this.state = {
      name: "",
      plu: ""
    };
  }

  handleNameChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const name = e.target.value;
    this.setState({ name });
  };

  handlePluChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const plu = e.target.value;
    this.setState({ plu });
  };

  handleSubmit = () => {
    const { name, plu } = this.state;
    this.props.onSubmit(name, plu);
    this.setState({name: "", plu: ""})
  };

  handlePressEnterToSearch = (e: React.KeyboardEvent) => {
    e = e || window.event;
    const ENTER = 13;

    if(e.keyCode == ENTER){
       this.handleSubmit();
    }
}
  render() {
    return (
      <React.Fragment>
        <Grid container justify="space-between" spacing ={16} onKeyDown={this.handlePressEnterToSearch}>
          <Grid item xs={12} md={3}>
            <TextField
              label="Name"
              className="full-width"
              onChange={this.handleNameChange}
              value={this.state.name}
              variant="outlined"
              InputLabelProps={{shrink: true}}
            />
          </Grid>
          <Grid item xs={12} md={3}>
            <TextField
              label="PLU"
              className="full-width"
              onChange={this.handlePluChange}
              value={this.state.plu}
              variant="outlined"
              InputLabelProps={{shrink: true}}
            />
          </Grid>

          <Grid item xs={12} sm={12} md={3}>
              <Button
                variant="contained"
                color="primary"
                className={this.props.classes.searchButtons}
                onClick={this.handleSubmit}
              >
                Search
              </Button>
          </Grid>
        </Grid>
      </React.Fragment>
    );
  }
}
export default withStyles(styles, { withTheme: true })(
  LinkGroupKitAddModalSearch
);
