import * as React from 'react'
import { withStyles } from '@material-ui/core/styles';
import ViewKitSearch from './ViewKitSearch';
import { KbApiMethod } from '../../helpers/kbapi'
import { Grid } from '@material-ui/core';
import axios from 'axios';
import KitResults from './KitResults'
import KitSearchDialog from './SelectKit/KitSearchDialog'
var urlStart = KbApiMethod("Kits");
var urlLocalesSearch = KbApiMethod("Locales");
import { isNumber } from '../../KitLinkGroups/ValidateFunctions';
import './style.css';
import PageStyle from './PageStyle';
import PageTitle from '../../PageTitle';
import withSnackbar from 'src/components/PageStyle/withSnackbar';


const styles = (theme: any) => ({
    root: {
        marginTop: 20
    },
    marginbottom: {
        marginBottom: 40
    },
    label: {
        fontSize: 20,
        textAlign: "right" as 'right',
        marginBottom: 0 + ' !important',
        paddingRight: 10 + 'px'
    },
    button: {
        margin: theme.spacing.unit,
    },
    formControl:
    {
        margin: theme.spacing.unit,
        minWidth: 120,
        width: '100%'
    }
});

interface IViewKitPageState {
    open: boolean,
    regions: Array<any>,
    metros: Array<any>,
    stores: Array<any>,
    regionValue: string,
    storeValue: string,
    metroValue: string,
    selectedKitId: number,
    selectedKitDescription: string,
    kitsViewData: any
    showDisplay: Boolean,
    maximumCalories: string,
    price: string,
    minimumCalories: string,
    disableViewKitButton: boolean,
    disableSaveButton: boolean

}

interface IViewKitPageProps {
    showAlert: any,
}

export class ViewKit extends React.Component<IViewKitPageProps, IViewKitPageState>
{
    constructor(props: IViewKitPageProps) {
        super(props);

        this.state = {
            open: false,
            regions: [],
            metros: [],
            stores: [],
            regionValue: "",
            storeValue: "",
            metroValue: "",
            selectedKitId: 0,
            selectedKitDescription:"",
            kitsViewData: {},
            showDisplay: true,
            maximumCalories: "0",
            price:"0",
            minimumCalories: "0",
            disableSaveButton: true,
            disableViewKitButton:false
        }

        this.onkitSelected = this.onkitSelected.bind(this);
        this.showPopUp = this.showPopUp.bind(this);
        this.onSaveChanges = this.onSaveChanges.bind(this);
        this.onClose = this.onClose.bind(this);
        this.onSearch = this.onSearch.bind(this);
        this.onMaximumCaloriesChange = this.onMaximumCaloriesChange.bind(this);

    }

    onSaveChanges() {
        let maximumCalories = this.state.maximumCalories;
        let kitLocaleId = this.state.kitsViewData.kitLocaleId;

        if (isNumber(String(maximumCalories))) {
            let urlKitSave = urlStart + "/" + kitLocaleId + "/UpdateMaximumCalories"
            var headers = {
                'Content-Type': 'application/json', "Access-Control-Allow-Origin": "*"
            }

            axios.post(urlKitSave, maximumCalories,
                {
                    headers: headers
                }).then(response => {
                    this.props.showAlert("Data saved successfully.", "success");
                }).catch(error => {
                    this.props.showAlert("Error in saving data.","error");
                });
        }
        else {
            this.props.showAlert("Maximum calories must be numeric.","error");
        }
    }
    onkitSelected(row: any) {
        this.setState({ 
            selectedKitId: row.kitId, 
            kitsViewData: {}, 
            selectedKitDescription: row.description, 
            disableSaveButton: true, 
            open: false, 
            maximumCalories:"0", 
            price:'', 
            minimumCalories:""})
    }

    showPopUp() {
        this.setState({ open: true });
    }

    onClose() {
        this.setState({ open: false });
    }

    onMaximumCaloriesChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        this.setState({ maximumCalories: event.target.value });
    }
    onRegionChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        this.setState({
            regionValue: event.target.value,
            disableSaveButton: true,
            kitsViewData: {}
        }, this.populateMetros);
    }

    onMetroChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        this.setState({
            metroValue: event.target.value,
            disableSaveButton: true,
            kitsViewData: {}
        }, this.populateStores);
    }

    onStoreChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        this.setState({
            storeValue: event.target.value,
            kitsViewData: {},
            disableSaveButton: true
        });
    }

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
                }))
            .catch(error => {
                console.log(error);
                this.props.showAlert("Error loading regions.", "error");
            })
        this.setState({ regionValue: "", metros: [], metroValue: "", stores: [], storeValue: "" })
    }

    populateMetros = () => {

        var regionId = this.state.regionValue;
        var urlMetros = urlLocalesSearch + "/GetMetrosByRegionId?RegionId=" + regionId;

        fetch(urlMetros)
            .then(response => {
                return response.json();
            })
            .then(data =>
                this.setState({
                    metros: data
                }))
            .catch(error => {
                this.props.showAlert("Error loading metros.","error");
            })
        this.setState({ metroValue: "", stores: [], storeValue: "" })
    }

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
                }))
            .catch(error => {
                this.props.showAlert("Error loading stores.");
            })
        this.setState({ storeValue: "" })
    }

    onSearch() {
        this.setState({ disableViewKitButton: true })

        let selectedKitId = this.state.selectedKitId;
        let selectedStoreId = this.state.storeValue;
        if (selectedKitId === 0) {
            this.props.showAlert("Please select kit.");
            this.setState({ disableViewKitButton: false })
            return;
        }
        if (selectedStoreId === "") {
            this.props.showAlert("Please select store.");
            this.setState({ disableViewKitButton: false })
            return;
        }

        var url = urlStart + "/" + selectedKitId + "/" + selectedStoreId + "/GetKitView"

        fetch(url)
            .then(response => {
                return response.json();
            }).then((data) => {

                if (typeof (data) == undefined || data == null || data.length == 0) {
                    this.props.showAlert("No data found.");
                    this.setState({
                        showDisplay: true, kitsViewData: {}
                    });

                }

                else if (data.errorMessage) {
                    this.props.showAlert(data.errorMessage, "error")
                    this.setState({
                        showDisplay: true, kitsViewData: {}
                    });
                }

                else {
                    this.setState({
                        kitsViewData: data, showDisplay: true,
                        maximumCalories: data.maximumCalories, disableSaveButton: false,
                        price:data.kitPrice, minimumCalories:data.minimumCalories
                    });
                }
                this.setState({ disableViewKitButton: false })
            })
            .catch((error) => {
                this.props.showAlert(error.response.data, "error");
                this.setState({
                    disableViewKitButton: false
                })
            });
    }

    render() {
        const { kitsViewData } = this.state;
        return (

            <React.Fragment>
                <PageStyle>
                    <PageTitle icon="format_list_bulleted">View Kit By Store</PageTitle>
                    <div className="search-container">
                        <ViewKitSearch
                            regions={this.state.regions}
                            metros={this.state.metros}
                            stores={this.state.stores}
                            regionValue={this.state.regionValue}
                            storeValue={this.state.storeValue}
                            metroValue={this.state.metroValue}
                            selectedKit={this.state.selectedKitDescription}
                            onRegionChange={this.onRegionChange}
                            onMetroChange={this.onMetroChange}
                            onStoreChange={this.onStoreChange}
                            selectKit={this.showPopUp}
                            disableViewKitButton={this.state.disableViewKitButton}
                            ViewKit={this.onSearch}
                        />


                        <Grid xs={12} md={12} item>
                        </Grid>
                        {
                            this.state.showDisplay ?
                                <KitResults
                                    kitsViewData={kitsViewData.linkGroups}
                                    minimumCaloriesValue={this.state.minimumCalories}
                                    maximumCaloriesValue={this.state.maximumCalories}
                                    maximumCalories={this.onMaximumCaloriesChange}
                                    onSaveChanges={this.onSaveChanges}
                                    price={this.state.price}
                                    disableSaveButton={this.state.disableSaveButton}
                                />
                                : <></>}
                    </div>
                </PageStyle>

                <KitSearchDialog
                    openPopUp={this.state.open}
                    closePopUp={this.onClose}
                    onkitSelect={this.onkitSelected}

                />

            </React.Fragment>
        )
    }
}

export default withStyles(styles, { withTheme: true })(withSnackbar(ViewKit));