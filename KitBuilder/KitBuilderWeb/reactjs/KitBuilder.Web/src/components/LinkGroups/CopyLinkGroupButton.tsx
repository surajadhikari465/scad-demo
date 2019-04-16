import * as React from "react";
import Button from "@material-ui/core/Button";
import { PerformLinkGroupSearch } from "./LinkGroupFunctions";
import withSnackbar from "../PageStyle/withSnackbar";
import {
  Dialog,
  DialogContent,
  DialogTitle,
  TextField,
  DialogActions
} from "@material-ui/core";
import { LinkGroup } from "src/types/LinkGroup";
import Axios from 'axios';
import { KbApiMethod } from '../helpers/kbapi';

interface IProps {
  linkGroupId: number;
  showAlert(message: string, type?: string): void;
}
interface IState {
  isOpen: boolean;
  name: string;
  description: string;
  errors: { name: string[]; description: string[] };
}

export class CopyLinkGroupButton extends React.Component<IProps, IState> {
  constructor(props: any) {
    super(props);
    this.state = {
      isOpen: false,
      name: "",
      description: "",
      errors: { name: [], description: [] }
    };
  }

  defaultErrors = () => {
    return { name: [], description: [] };
  }

  CopyLinkGroupClick = () => {
    const { description, errors } = this.state;
    const name = this.state.name.trim();
    if(name === "") {
      this.setState({
        errors: {
          ...errors,
          name: ["Name cannot be blank."]
        }
      });
      return;
    } else if (description==="") {
      this.setState({
        errors: {
          ...errors,
          description: ["Description cannot be blank."]
        }
      });
      return
    }


    PerformLinkGroupSearch(name, "", "", "", "")
      .then((result: LinkGroup[]) => {
        const hasSameName = result.some(
          linkGroup =>
            linkGroup.groupName.toLowerCase().trim() ===
            name.toLowerCase().trim()
        );
        return hasSameName;
      })
      .then(hasSameName => {
        if (hasSameName) {
          this.setState({
            errors: {
              ...errors,
              name: ["A Link Group with that name already exists."]
            }
          });
          return false;
        }
        return true;
      })
      .then(validationPassed => {
        const url = `${KbApiMethod("LinkGroups")}/${this.props.linkGroupId}/true`;


        if (validationPassed) { 

          Axios.get(url).then((response) => {
            const lg: LinkGroup = response.data;
            lg.linkGroupItemDto.forEach(i => {
              delete i.item
              delete i.instructionList
              i.linkGroupId = 0;
              i.linkGroupItemId = 0;
            });
            lg.linkGroupId = 0;
            lg.groupName = this.state.name;
            lg.groupDescription = this.state.description;

            Axios.post(KbApiMethod("LinkGroups"), lg).then(() => {
              this.props.showAlert("Success duplicating link group.", "success");
              this.setState({isOpen: false});
            })
            .catch(error => {
              this.props.showAlert(error.message, "error");
            });
          })
        }
      })
      .catch(error => {
        this.props.showAlert(error.message, "error");
      });
  };

  CopyLinkGroup(id: number, newLinkGroupName: string) {}

  render() {
    const { name, description, errors } = this.state;
    return (
      <React.Fragment>
        <Button
          variant="outlined"
          color="primary"
          onClick={() => this.setState({ isOpen: true })}
          fullWidth
        >
          Duplicate
        </Button>
        <Dialog
          open={this.state.isOpen}
          onClose={() => this.setState({ isOpen: false })}
        >
          <DialogTitle>Duplicate a Link Group</DialogTitle>
          <DialogContent>
            <TextField
              variant="outlined"
              label="Name"
              error = {!!errors.name.length}
              helperText={errors.name.join(" ,")}
              value={name}
              onChange={e => this.setState({ name: e.target.value, errors: this.defaultErrors() })}
              InputLabelProps={{ shrink: true }}
              fullWidth
              className="mt-3 mb-3"
            />

            <TextField
              variant="outlined"
              label="Description"
              error = {!!errors.description.length}
              helperText={errors.description.join(" ,")}
              value={description}
              onChange={e => this.setState({ description: e.target.value, errors: this.defaultErrors() })}
              InputLabelProps={{ shrink: true }}
              fullWidth
            />
          </DialogContent>
          <DialogActions>
            <Button
              variant="outlined"
              color="secondary"
              onClick={() => this.setState({ isOpen: false })}
            >
              Cancel
            </Button>
            <Button
              variant="contained"
              color="primary"
              onClick={this.CopyLinkGroupClick}
            >
              Create
            </Button>
          </DialogActions>
        </Dialog>
      </React.Fragment>
    );
  }
}

export default withSnackbar(CopyLinkGroupButton);
