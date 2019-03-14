import * as React from "react";
import MainItemDisplay from "./MainItem";
import { Grid, TextField, Button, Switch } from "@material-ui/core";
import StyledPanel from "src/components/PageStyle/StyledPanel";
import KitTypeSelector from "./KitTypeSelector";
import LinkedGroupTable from "./LinkedGroupsTable";
import AddMainItemDialog from "./AddMainItemDialog";
import { LinkedGroup, Item, LinkedGroupItem } from "../../../types/LinkGroup";
import LinkgroupKitAddModal from "./LinkGroupAddKitModal";
import { KbApiMethod } from "src/components/helpers/kbapi";
import Axios from "axios";
import { InstructionList } from "src/types/InstructionList";
import { SelectInstructions } from "./SelectInstructions";
import PageTitle from "src/components/PageTitle";
import ConfirmSaveDialog from "./ConfirmSaveDialog";

interface ICreateKitPageState {
  addItemIsOpen: boolean;
  addLinkedGroupIsOpen: boolean;
  confirmCreatKitIsOpen: boolean;

  error: string;
  message: string;
  description: string;
  kitType: number;
  kitLinkGroup: Array<LinkedGroup>;
  selectedLinkedGroupItems: Array<LinkedGroupItem>;
  item: Item | null;
  Instructions: Array<any>;
  isSimpleItem: boolean;
  isFixedItem: boolean;
  isCustomizable: boolean;
  showRecipe: boolean;
  isDisplayMandatory: boolean;
}

interface ICreateKitPageProps {}

export default class CreateKitPage extends React.PureComponent<
  ICreateKitPageProps,
  ICreateKitPageState
> {
  constructor(props: ICreateKitPageProps) {
    super(props);
    this.state = {
      confirmCreatKitIsOpen: false,
      error: "",
      message: "",
      addItemIsOpen: false,
      addLinkedGroupIsOpen: false,
      description: "",
      kitType: 0,
      kitLinkGroup: [],
      selectedLinkedGroupItems: [],
      item: null,
      Instructions: [],
      isSimpleItem: false,
      isFixedItem: false,
      isCustomizable: false,
      showRecipe: false,
      isDisplayMandatory: false
    };
  }

  handleAddKit = () => {
    this.setState({ message: "", error: "" });
    Axios.post(KbApiMethod("Kits"), this.buildAddKitPayload())
      .then(r => {
        if (r.status === 200) {
          this.setState({ message: "Kit Created Succesfully." });
        }
      })
      .catch(e => {
        if (e.response) this.setState({ error: e.response.data });
      });
  };

  buildAddKitPayload = () => {
    const {
      kitLinkGroup,
      Instructions,
      description,
      item,
      kitType,
      showRecipe,
      isDisplayMandatory
    } = this.state;

    const reformatedKitLinkGroup = kitLinkGroup.map(linkedGroup => {
      // @ts-ignore
      const results = this.state.selectedLinkedGroupItems.filter(
        item => item.linkGroupId === linkedGroup.linkGroupId
      );
      return {
        ...linkedGroup,
        KitLinkGroupItem: results
      };
    });

    // @ts-ignore
    return {
      Description: description,
      // @ts-ignore
      ItemId: item.ItemId,
      KitLinkGroup: reformatedKitLinkGroup,
      KitInstructionList: Instructions,
      KitType: kitType,
      showRecipe,
      isDisplayMandatory
    };
  };

  handleAddLinkedGroup = (linkedGroupsToAdd: Array<LinkedGroup>) => {
    const { kitLinkGroup: linkedGroups } = this.state;

    const url = KbApiMethod("LinkGroups");
    const linkedGroupsPromises = linkedGroupsToAdd.map(linkedGroup => {
      //axios returns a promise, so I am using map to create an array of promises
      return Axios.get(`${url}/${linkedGroup.linkGroupId}/true`).then(
        resp => resp.data
      );
    });

    Promise.all(linkedGroupsPromises).then(newlinkedGroups => {
      this.setState({
        kitLinkGroup: [...linkedGroups, ...newlinkedGroups],
        addLinkedGroupIsOpen: false
      });
    });
  };

  handleRemoveLinkedGroup = (linkedGroupToRemove: LinkedGroup) => {
    const { kitLinkGroup: linkedGroups } = this.state;
    const linkedGroupsLeft = linkedGroups.filter(
      linkedGroup => linkedGroup !== linkedGroupToRemove
    );
    this.setState({ kitLinkGroup: linkedGroupsLeft });
  };

  handleSelectMainItem = (item: Item) => {
    this.setState({ item: item, addItemIsOpen: false });
  };

  handleKitTypeChange = (kitType: number) => {
    //remove linked groups if they select a simple item kit
    if (kitType === 0) this.setState({ kitType, kitLinkGroup: [] });
    else this.setState({ kitType });
  };

  handleKitDescriptionChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    this.setState({ description: e.target.value });
  };

  handleLinkedGroupItemSelected = (itemToAdd: LinkedGroupItem) => {
    const { selectedLinkedGroupItems } = this.state;
    if (!selectedLinkedGroupItems.includes(itemToAdd)) {
      this.setState({
        selectedLinkedGroupItems: [...selectedLinkedGroupItems, itemToAdd]
      });
    }
  };

  handleLinkedGroupItemDeselected = (itemToRemove: LinkedGroupItem) => {
    const { selectedLinkedGroupItems } = this.state;
    const itemsLeftAfterFilter = selectedLinkedGroupItems.filter(
      item => item !== itemToRemove
    );

    this.setState({ selectedLinkedGroupItems: itemsLeftAfterFilter });
  };

  handleSelectInstructionList = (list: Array<InstructionList>) => {
    this.setState({ Instructions: [...list] });
  };

  render() {
    const { item, error, message } = this.state;
    return (
      <>
        {error && <div className="create-kit-error">{error}</div>}
        {message && <div className="create-kit-message">{message}</div>}
        <StyledPanel header={<PageTitle icon="build">Create Kit</PageTitle>}>
          <Grid
            container
            alignItems="center"
            className="create-kit-inner-container"
          >
            <Grid item xs={12}>
              <Grid
                container
                justify="space-between"
                alignItems="center"
                className="create-kit-inner-row-container"
              >
                <Grid item>Main Item:</Grid>
                <Grid item xs={12} md={4}>
                  <MainItemDisplay
                    item={this.state.item}
                    selectMainItem={() =>
                      this.setState({ addItemIsOpen: true })
                    }
                  />
                </Grid>
              </Grid>
            </Grid>
            <Grid item xs={12}>
              <Grid
                container
                justify="space-between"
                alignItems="center"
                className="create-kit-inner-row-container"
              >
                <Grid item xs={12} md={4}>
                  Item Type:
                </Grid>
                <Grid item xs={12} md={7} lg={6} xl={5}>
                  <KitTypeSelector
                    onKitTypeChange={this.handleKitTypeChange}
                    kitType={this.state.kitType}
                  />
                </Grid>
              </Grid>
            </Grid>
            <Grid item xs={12}>
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
            <Grid item xs={12}>
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
                      this.setState({ isDisplayMandatory: e.target.checked })
                    }
                  />
                </Grid>
              </Grid>
            </Grid>
            <Grid item xs={12}>
              <Grid
                container
                justify="space-between"
                alignItems="center"
                alignContent="center"
                className="create-kit-inner-row-container"
              >
                <Grid item xs={12} md={5}>
                  Description:
                </Grid>
                <Grid item xs={12} md={5}>
                  <TextField
                    variant="outlined"
                    label="Kit Description"
                    InputLabelProps={{ shrink: true }}
                    onChange={this.handleKitDescriptionChange}
                  />
                </Grid>
              </Grid>
            </Grid>
            <Grid item xs={12}>
              <Grid
                container
                justify="space-between"
                alignItems="center"
                className="create-kit-inner-row-container"
              >
                <Grid item xs={12} md={5}>
                  Instructions:
                </Grid>
                <Grid item xs={12} md={5}>
                  <SelectInstructions
                    selectedInstructionLists={this.state.Instructions}
                    onSelectInstructionList={this.handleSelectInstructionList}
                  />
                </Grid>
              </Grid>
            </Grid>
            <Grid item xs={12}>
              <Grid container justify="center" className="my-5">
                <Grid item xs={12}>
                  <LinkedGroupTable
                    linkedGroups={this.state.kitLinkGroup}
                    onLinkedGroupRemoved={this.handleRemoveLinkedGroup}
                    selectedLinkedGroupItems={
                      this.state.selectedLinkedGroupItems
                    }
                    onItemSelected={this.handleLinkedGroupItemSelected}
                    onItemUnselected={this.handleLinkedGroupItemDeselected}
                  />
                </Grid>
              </Grid>
            </Grid>
            <Grid item xs={12}>
              <Grid container justify="flex-end">
                <Grid item>
                  <Button
                    disabled={this.state.kitType < 1}
                    onClick={() =>
                      this.setState({ addLinkedGroupIsOpen: true })
                    }
                  >
                    Add Linked Group
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
                    Create Kit
                  </Button>
                </Grid>
              </Grid>
            </Grid>
          </Grid>
        </StyledPanel>
        <LinkgroupKitAddModal
          onAddToKit={this.handleAddLinkedGroup}
          kitLinkGroup={this.state.kitLinkGroup}
          isOpen={this.state.addLinkedGroupIsOpen}
          onCancel={() => this.setState({ addLinkedGroupIsOpen: false })}
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
    );
  }
}
