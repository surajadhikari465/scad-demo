import * as React from "react";
import { withStyles } from "@material-ui/core/styles";
import KitByStoreResults from "./KitByStoreResults";
import { KbApiMethod } from "../../helpers/kbapi";
import { Grid } from "@material-ui/core";
import SelectLocale from "./SelectLocale";
var urlStart = KbApiMethod("Kits");
var urlLocalesSearch = KbApiMethod("Locales");
import '../ViewKits/style.css';
import PageTitle from "../../PageTitle";
import withSnackbar from "src/components/PageStyle/withSnackbar";
import StyledPanel from "src/components/PageStyle/StyledPanel";
import { ExportCSV } from './ExportReactCSV'

const styles = (theme: any) => ({
  root: {
    marginTop: 20
  },
  marginbottom: {
    marginBottom: 40
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
  formControl: {
    margin: theme.spacing.unit,
    minWidth: 120,
    width: "100%"
  }
});

interface IKitsByStoreViewPageState {
  regions: Array<any>;
  metros: Array<any>;
  stores: Array<any>;
  regionValue: string;
  storeValue: string;
  storeId:number;
  storeName: string;
  metroValue: string;
  kitsByStoreData: Array<any>;
  disableViewKitButton: boolean;
  disableExportbutton: boolean;
  showDisplay: boolean;
}

interface IKitsByStoreViewPageProps {
  showAlert: any,
  history: any,
  reactTable: any
}

export class KitsByStoreView extends React.Component<
  IKitsByStoreViewPageProps,
  IKitsByStoreViewPageState
  > {
  constructor(props: IKitsByStoreViewPageProps) {
    super(props);

    this.state = {
      regions: [],
      metros: [],
      stores: [],
      regionValue: "",
      storeValue: "",
      storeId:0,
      storeName:"",
      metroValue: "",
      kitsByStoreData: [],
      disableViewKitButton: false,
      disableExportbutton: true,
      showDisplay: true
    };

    this.viewKitsByStore = this.viewKitsByStore.bind(this);
  }
  csvLink = React.createRef()
  onRegionChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    this.setState(
      {
        regionValue: event.target.value,
        kitsByStoreData: [],
        disableExportbutton: true
      },
      this.populateMetros
    );
  };

  onMetroChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    this.setState(
      {
        metroValue: event.target.value,
        kitsByStoreData: [],
        disableExportbutton: true
      },
      this.populateStores
    );
  };

  exportData = () => {
    this.props.reactTable.link.click();
  }

  onStoreChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    let store=event.target.value.split('-');
    var storeId=Number(store[0]); 
    var storeName=store[store.length-1]; 
    var storeValue= event.target.value

    this.setState({
      storeValue:storeValue,
      storeId:storeId,
      storeName:storeName,
      kitsByStoreData: [],
      disableExportbutton: true
    });
  };

  componentDidMount() {
    this.populateRegions();
  }

  populateRegions = () => {
    var urlRegions = urlLocalesSearch + "/GetLocaleByTypeId?localeType=Region";
    fetch(urlRegions)
      .then(response => {
        return response.json();
      })
      .then(data =>
        this.setState({
          regions: data
        })
      )
      .catch(error => {
        console.log(error);
        this.props.showAlert("Error loading regions.", "error");
      });
    this.setState({
      regionValue: "",
      metros: [],
      metroValue: "",
      stores: [],
      storeValue: "",
      storeId:0,
      storeName:""
    });
  };

  populateMetros = () => {
    var regionId = this.state.regionValue;
    var urlMetros =
      urlLocalesSearch + "/GetMetrosByRegionId?RegionId=" + regionId;

    fetch(urlMetros)
      .then(response => {
        return response.json();
      })
      .then(data =>
        this.setState({
          metros: data
        })
      )
      .catch(error => {
        this.props.showAlert("Error loading metros.", "error");
      });
    this.setState({ metroValue: "", stores: [], storeValue: "", storeName:"", storeId:0 });
  };

  populateStores = () => {
    var metroId = this.state.metroValue;
    var urlStores = urlLocalesSearch + "/GetStoresByMetroId?metroId=" + metroId;

    fetch(urlStores)
      .then(response => {
        return response.json();
      })
      .then(data =>
        this.setState({
          stores: data
        })
      )
      .catch(error => {
        this.props.showAlert("Error loading stores.");
      });
    this.setState({ storeValue: "" , storeName:"", storeId:0});
  };

  onSelected = (row: any) => {
    var kitId = row.kitId;
    const editUrl = `#/EditKit/${kitId}`
      window.open(editUrl, '_blank');
  }

  viewKitsByStore = () => {
    this.setState({ disableViewKitButton: true });
    let selectedStoreId = this.state.storeId;
    if (selectedStoreId ===0) {
      this.props.showAlert("Please select store.");
      this.setState({ disableViewKitButton: false });
      return;
    }

    var url =
      urlStart + "/" + selectedStoreId + "/GetAllKitsForStore";
    fetch(url)
      .then(response => {
        return response.json();
      })
      .then(data => {
        if (typeof data == undefined || data == null || data.length == 0) {
          this.props.showAlert("No kits have been assigned to the selected store.");
          this.setState({
            showDisplay: true,
            kitsByStoreData: []
          });
        } else if (data.errorMessage !=null && data.errorMessage.substring(0, 5) == "Error") {
          this.props.showAlert(data.errorMessage, "error");
          this.setState({
            showDisplay: true,
            kitsByStoreData: []
          });
        } else {
          this.setState({
            kitsByStoreData: data,
            disableExportbutton: false
          });
        }
        this.setState({
          disableViewKitButton: false
        });
      })
      .catch(error => {
        this.props.showAlert(error.response.data, "error");
        this.setState({
          disableViewKitButton: false
        });
      });
  }

  render() {
    var worksheetColumnsWidth = [
      { wch: 25 },
      { wch: 20 },
      { wch: 35 },
      { wch: 20 }
    ];
    const { kitsByStoreData, disableExportbutton } = this.state;
    const fileName= "KitsByStore-" + this.state.storeName;
    var excludeColumnsInExport = ['excluded', 'kitId']

    return (
      <>
        <StyledPanel
          header={
            <PageTitle icon="format_list_bulleted">Kits By Store</PageTitle>
          }
        >
          <SelectLocale
            regions={this.state.regions}
            metros={this.state.metros}
            stores={this.state.stores}
            regionValue={this.state.regionValue}
            storeValue={this.state.storeValue}
            metroValue={this.state.metroValue}
            onRegionChange={this.onRegionChange}
            onMetroChange={this.onMetroChange}
            onStoreChange={this.onStoreChange}
            viewKitsByStore={this.viewKitsByStore}
            disableViewKitButton={this.state.disableViewKitButton}
          />

          <Grid xs={12} md={12} item />
          {this.state.showDisplay && (
            <KitByStoreResults
              kitsByStoreData={kitsByStoreData}
              onSelected={this.onSelected}
            />
          )}
          <Grid container justify="flex-end" spacing={16}>
            <Grid item xs={12} sm={12} md={2}>
              <ExportCSV csvData={kitsByStoreData} fileName={fileName} disableExportbutton={disableExportbutton} worksheetColumnsWidth={worksheetColumnsWidth} excludeColumnsInExport = {excludeColumnsInExport} />
            </Grid>
          </Grid>

        </StyledPanel>
      </>
    );
  }
}

export default withStyles(styles, { withTheme: true })(withSnackbar(KitsByStoreView));
