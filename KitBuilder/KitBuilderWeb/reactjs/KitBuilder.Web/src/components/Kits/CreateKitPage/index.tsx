import * as React from "react";
import MainItemDisplay from "./MainItem";
import { Grid, TextField, Button, Switch } from "@material-ui/core";
import StyledPanel from "src/components/PageStyle/StyledPanel";
import KitTypeSelector from "./KitTypeSelector";
import LinkGroupTable from "./LinkGroupsTable";
import AddMainItemDialog from "./AddMainItemDialog";
import { LinkGroup, Item, LinkGroupItem } from "../../../types/LinkGroup";
import LinkgroupKitAddModal from "./LinkGroupAddKitModal";
import { KbApiMethod } from "src/components/helpers/kbapi";
import Axios from "axios";
import { InstructionList } from "src/types/InstructionList";
import { SelectInstructions } from "./SelectInstructions";
import PageTitle from "src/components/PageTitle";
import ConfirmSaveDialog from "./ConfirmSaveDialog";

interface ICreateKitPageState {
  addItemIsOpen: boolean;
  addLinkGroupIsOpen: boolean;
  confirmCreatKitIsOpen: boolean;
  errors: any;
  loading: boolean;
  editMode: boolean;
  error: string;
  message: string;
  kitId: number;
  description: string;
  kitType: number;
  LinkGroups: Array<LinkGroup>;
  selectedLinkGroupItems: Array<LinkGroupItem>;
  item: Item | null;
  Instructions: Array<any>;
  showRecipe: boolean;
  isDisplayMandatory: boolean;
}

interface ICreateKitPageProps {
  match: any;
  showAlert: any;
}

const newErrorsObject = () => ({
  description: "",
  linkGroup: []
});

class CreateKitPage extends React.PureComponent<
  ICreateKitPageProps,
  ICreateKitPageState
> {
  constructor(props: ICreateKitPageProps) {
    super(props);
    this.state = {
      editMode: false,
      loading: false,
      errors: { description: null, linkGroup: [] },

      confirmCreatKitIsOpen: false,
      error: "",
      message: "",
      addItemIsOpen: false,
      addLinkGroupIsOpen: false,
      kitId: 0,
      description: "",
      kitType: 1,
      LinkGroups: [],
      selectedLinkGroupItems: [],
      item: null,
      Instructions: [],
      showRecipe: false,
      isDisplayMandatory: false
    };
  }

  componentDidMount() {
    const editKitId = this.props.match.params.kitId;

    if (editKitId) {
      const kitsUrl = KbApiMethod("Kits") + "/" + editKitId + "/true";
      this.setState({
        kitId: parseInt(editKitId),
        editMode: true,
        loading: true
      });
      fetch(kitsUrl)
        .then(response => response.json())
        .then(response => response[0])
        .then(response => {
          const kitLinkGroup = response.kitLinkGroup.map(
            (lg: any) => lg.linkGroup
          );
          const selectedLinkGroupItems = response.kitLinkGroup
            .map((lg: any) => lg.kitLinkGroupItem)
            .reduce((final: any, next: any) => [...final, ...next], [])
            .map((klgi: any) => klgi.linkGroupItem);
          const Instructions = response.kitInstructionList.map(
            (kitInstruction: any) => kitInstruction.instructionList
          );
          const {
            kitType,
            isDisplayMandatory,
            showRecipe,
            description,
            item
          } = response;

          this.setState({
            selectedLinkGroupItems: selectedLinkGroupItems,
            Instructions,
            kitType,
            isDisplayMandatory,
            showRecipe,
            description,
            item
          });
          return kitLinkGroup;
        })
        .then(LinkGroups => {
          this.setState({ LinkGroups, loading: false });
        });
    }
  }
  validateForm = () => {
    const { description, LinkGroups, selectedLinkGroupItems } = this.state;
    const errors = newErrorsObject();
    let errorSet = false;

    if (!description) {
      errors.description = "Must provide a kit description.";
      errorSet = true;
    }

    LinkGroups.forEach((lg: LinkGroup) => {
      if (
        !selectedLinkGroupItems
          .map((lgi: LinkGroupItem) => lgi.linkGroupId)
          .includes(lg.linkGroupId)
      ) {
        errorSet = true;
        errors.linkGroup.push({
          // @ts-ignore
          linkGroupId: lg.linkGroupId,
          // @ts-ignore
          error: "Must select at least one modifier, or remove link group."
        });
      }
    });
    this.setState({errors})
    return !errorSet;
  };

  handleAddKit = () => {
    this.setState({ message: "", error: "" });

    if (this.validateForm()) {
      Axios.post(KbApiMethod("Kits"), this.buildAddKitPayload())
        .then(r => {
          if (r.status === 200) {
            this.props.showAlert("Kit Created Succesfully", "success");
          }
        })
        .catch(e => {
          if (e.response) this.props.showAlert(e.response.data, "error");
        });
    }
  };

  buildAddKitPayload = () => {
    const {
      LinkGroups,
      Instructions,
      description,
      item,
      kitType,
      showRecipe,
      isDisplayMandatory,
      kitId,
      selectedLinkGroupItems
    } = this.state;

    const reformatedKitLinkGroup = LinkGroups.map(linkGroup => {
      // @ts-ignore
      const results = selectedLinkGroupItems.filter(
        item => item.linkGroupId === linkGroup.linkGroupId
      );
      return {
        ...linkGroup,
        KitLinkGroupItem: results
      };
    });

    // @ts-ignore
    return {
      kitId,
      Description: description,
      // @ts-ignore
      ItemId: item.ItemId || item.itemId,
      KitLinkGroup: reformatedKitLinkGroup,
      KitInstructionList: Instructions,
      KitType: kitType,
      showRecipe,
      isDisplayMandatory
    };
  };

  handleAddLinkGroup = (linkGroupsToAdd: Array<LinkGroup>) => {
    const { LinkGroups } = this.state;

    const url = KbApiMethod("LinkGroups");
    const linkGroupsPromises = linkGroupsToAdd.map(linkGroup => {
      //axios returns a promise, so I am using map to create an array of promises
      return Axios.get(`${url}/${linkGroup.linkGroupId}/true`).then(
        resp => resp.data
      );
    });

    Promise.all(linkGroupsPromises).then(newlinkGroups => {
      this.setState({
        LinkGroups: [...LinkGroups, ...newlinkGroups],
        addLinkGroupIsOpen: false
      });
    });
  };

  handleRemoveLinkGroup = (linkGroupToRemove: LinkGroup) => {
    const { LinkGroups } = this.state;
    const linkGroupsLeft = LinkGroups.filter(
      linkGroup => linkGroup.linkGroupId !== linkGroupToRemove.linkGroupId
    );
    this.setState({ LinkGroups: linkGroupsLeft });
  };

  handleSelectMainItem = (item: Item) => {
    this.setState({ item: item, addItemIsOpen: false });
  };

  handleKitTypeChange = (kitType: number) => {
    //remove link groups if they select a simple item kit
    if (kitType === 0) this.setState({ kitType, LinkGroups: [] });
    else this.setState({ kitType });
  };

  handleKitDescriptionChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    this.setState({ description: e.target.value });
  };

  handleLinkGroupItemSelected = (itemToAdd: LinkGroupItem) => {
    const { selectedLinkGroupItems } = this.state;
    if (!selectedLinkGroupItems.includes(itemToAdd)) {
      this.setState({
        selectedLinkGroupItems: [...selectedLinkGroupItems, itemToAdd]
      });
    }
  };

  handleLinkGroupItemDeselected = (itemToRemove: LinkGroupItem) => {
    const { selectedLinkGroupItems } = this.state;
    const itemsLeftAfterFilter = selectedLinkGroupItems.filter(
      item => item.linkGroupItemId !== itemToRemove.linkGroupItemId
    );

    this.setState({ selectedLinkGroupItems: itemsLeftAfterFilter });
  };

  handleSelectInstructionList = (list: Array<InstructionList>) => {
    this.setState({ Instructions: [...list] });
  };

  render() {
    const { error, message, item, loading, editMode, description } = this.state;
    return (
      <>
        {loading ? (
          "Loading..."
        ) : (
          <>
            {error && <div className="create-kit-error">{error}</div>}
            {message && <div className="create-kit-message">{message}</div>}
            <StyledPanel
              header={
                <PageTitle icon="build">
                  {editMode ? "Edit" : "Create"} Kit
                </PageTitle>
              }
            >
              <Grid
                container
                spacing={16}
                alignItems="center"
                className="create-kit-inner-container"
              >
                <Grid item xs={12} md={6}>
                  <Grid
                    container
                    justify="space-between"
                    alignItems="center"
                    className="create-kit-inner-row-container"
                  >
                    <Grid item>Main Item:</Grid>
                    <Grid item xs={8}>
                      <MainItemDisplay
                        item={this.state.item}
                        selectMainItem={() =>
                          this.setState({ addItemIsOpen: true })
                        }
                      />
                    </Grid>
                  </Grid>
                </Grid>

                <Grid item xs={12} md={6}>
                  <Grid
                    container
                    justify="space-between"
                    alignItems="center"
                    className="create-kit-inner-row-container"
                  >
                    <Grid item>Kit Type:</Grid>
                    <Grid item xs={10}>
                      <KitTypeSelector
                        onKitTypeChange={this.handleKitTypeChange}
                        kitType={this.state.kitType}
                      />
                    </Grid>
                  </Grid>
                </Grid>
                <Grid item xs={12} md={6}>
                  <Grid
                    container
                    justify="space-between"
                    alignItems="center"
                    className="create-kit-inner-row-container"
                  >
                    <Grid item xs={12} md={4}>
                      Show Recipe:
                    </Grid>
                    <Grid item>
                      <Switch
                        checked={this.state.showRecipe}
                        onChange={e =>
                          this.setState({ showRecipe: e.target.checked })
                        }
                        color="primary"
                      />
                    </Grid>
                  </Grid>
                </Grid>
                <Grid item xs={12} md={6}>
                  <Grid
                    container
                    justify="space-between"
                    alignItems="center"
                    className="create-kit-inner-row-container"
                  >
                    <Grid item xs={12} md={4}>
                      Mandatory Display:
                    </Grid>
                    <Grid item>
                      <Switch
                        color="primary"
                        checked={this.state.isDisplayMandatory}
                        onChange={e =>
                          this.setState({
                            isDisplayMandatory: e.target.checked
                          })
                        }
                      />
                    </Grid>
                  </Grid>
                </Grid>
                <Grid item xs={12} md={6}>
                  <Grid
                    container
                    justify="space-between"
                    alignItems="center"
                    alignContent="center"
                    className="create-kit-inner-row-container"
                  >
                    <Grid item>Description:</Grid>
                    <Grid item xs={10}>
                      <TextField
                        variant="outlined"
                        label="Kit Description"
                        value={description}
                        error={!!this.state.errors.description}
                        helperText={this.state.errors.description}
                        InputLabelProps={{ shrink: true }}
                        onChange={this.handleKitDescriptionChange}
                      />
                    </Grid>
                  </Grid>
                </Grid>
                <Grid item xs={12} md={6}>
                  <Grid
                    container
                    justify="space-between"
                    alignItems="center"
                    className="create-kit-inner-row-container"
                  >
                    <Grid item>Instructions:</Grid>
                    <Grid item xs={10}>
                      <SelectInstructions
                        selectedInstructionLists={this.state.Instructions}
                        onSelectInstructionList={
                          this.handleSelectInstructionList
                        }
                      />
                    </Grid>
                  </Grid>
                </Grid>
                <Grid item xs={12}>
                  <Grid container justify="center" className="my-5">
                    <Grid item xs={12}>
                      <LinkGroupTable
                        // @ts-ignore
                        errors={this.state.errors.linkGroup}
                        linkGroups={this.state.LinkGroups}
                        onLinkGroupRemoved={this.handleRemoveLinkGroup}
                        selectedLinkGroupItems={
                          this.state.selectedLinkGroupItems
                        }
                        onItemSelected={this.handleLinkGroupItemSelected}
                        onItemUnselected={this.handleLinkGroupItemDeselected}
                      />
                    </Grid>
                  </Grid>
                </Grid>
                <Grid item xs={12}>
                  <Grid container justify="flex-end">
                    <Grid item>
                      <Button
                        disabled={this.state.kitType < 2}
                        onClick={() =>
                          this.setState({ addLinkGroupIsOpen: true })
                        }
                      >
                        Add Link Group
                      </Button>
                    </Grid>
                    <Grid item>
                      <Button
                        disabled={!item}
                        onClick={() =>
                          this.setState({ confirmCreatKitIsOpen: true })
                        }
                        variant="outlined"
                      >
                        {editMode ? "Save" : "Create"} Kit
                      </Button>
                    </Grid>
                  </Grid>
                </Grid>
              </Grid>
            </StyledPanel>
            <LinkgroupKitAddModal
              onAddToKit={this.handleAddLinkGroup}
              kitLinkGroup={this.state.LinkGroups}
              isOpen={this.state.addLinkGroupIsOpen}
              onCancel={() => this.setState({ addLinkGroupIsOpen: false })}
            />
            <AddMainItemDialog
              onMainItemSelected={this.handleSelectMainItem}
              open={this.state.addItemIsOpen}
              onClose={() => {
                this.setState({ addItemIsOpen: false });
              }}
            />
            <ConfirmSaveDialog
              open={this.state.confirmCreatKitIsOpen}
              onClose={() => this.setState({ confirmCreatKitIsOpen: false })}
              onCreateKit={() => {
                this.setState({ confirmCreatKitIsOpen: false });
                this.handleAddKit();
              }}
            />
          </>
        )}
      </>
    );
  }
}

export default CreateKitPage;
