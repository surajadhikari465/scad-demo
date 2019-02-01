import * as React from 'react'
import { withStyles } from '@material-ui/core/styles';
import KitItemAddModal from './KitItemAddModal';
import { KbApiMethod } from '../../helpers/kbapi'
var urlStart = KbApiMethod("Items");
import { Grid, Button } from '@material-ui/core';
const hStyle = { color: 'red' };
const sucesssStyle = { color: 'blue' };

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

interface ICreateKitPageState {
    error: any,
    message: any,
    mainItemName: string,
    open: Boolean,
    mainItemlabelName: string,
    scanCodeName: string,
    scanCodelabelName: string,
    mainItemValue: string,
    scanCodeValue: string,
    selectedItemiId: number
    itemsdata: []
    showDisplay: Boolean,
}

interface ICreateKitPageProps {

}

export class CreateKit extends React.Component<ICreateKitPageProps, ICreateKitPageState>
{
    constructor(props: any) {
        super(props);

        this.state = {
            error: null,
            message: null,
            mainItemName: "Main Item Name:",
            open: false,
            mainItemlabelName: "Main Item Name:",
            scanCodeName: "Scan Code:",
            scanCodelabelName: "Scan Code:",
            itemsdata: [],
            mainItemValue: "",
            scanCodeValue: "",
            selectedItemiId: 0,
            showDisplay: false

        }

        this.onScanCodeChange = this.onScanCodeChange.bind(this);
        this.onMainItemChange = this.onMainItemChange.bind(this);
        this.OnChecked = this.OnChecked.bind(this);
        this.onSearch = this.onSearch.bind(this);
        this.showPopUp = this.showPopUp.bind(this);
        this.onClose = this.onClose.bind(this);
        
    }

    OnChecked(row: any) {
         alert(row.row._original.ItemId);
         this.setState({ open: false });
    }

    showPopUp() {
        this.setState({ open: true });

    }
    onClose() {
        this.setState({ open: false });

    }
    onScanCodeChange(event: any) {
        this.setState({ scanCodeValue: event.target.value });
    }

    onMainItemChange(event: any) {
        this.setState({ mainItemValue: event.target.value });
    }

    onSearch() {

        let scanCode = this.state.scanCodeValue;
        let mainItem = this.state.mainItemValue;
        if (scanCode == "" && mainItem == "") {
            this.setState({
                error: "Please enter atleast one select criteria."
            });
            this.setState({
                message: null
            })
            return;
        }

        var urlParam = "";

        if (mainItem != "")
            urlParam = urlParam + "ProductDesc=" + mainItem + "&"

        if (scanCode != "")
            urlParam = urlParam + "ScanCode=" + scanCode + "&"

        urlParam = urlParam.substring(0, urlParam.length - 1);

        var url = urlStart + "?" + urlParam;

        fetch(url)
            .then(response => {
                return response.json();
            }).then((data) => {
                if (typeof (data) == undefined || data == null || data.length == 0) {
                    this.setState({
                        error: "No Data Found."
                    });

                    this.setState({
                        message: null
                    })

                    this.setState({
                        itemsdata: []
                    })

                    this.setState({
                        showDisplay: false
                    })
                }

                else {
                    this.setState({
                        error: null
                    });

                    this.setState({
                        message: null
                    })
                    this.setState({
                        itemsdata: data
                    })
                    this.setState({
                        showDisplay: true
                    })
                }
            })
    }
    render() {
        const { itemsdata } = this.state;
        return (

            <React.Fragment>
                <Grid container justify="center">
                    <Grid container justify="center">
                        <div className="error-message" >
                            <span style={hStyle}> {this.state.error}</span>
                        </div>
                    </Grid>
                    <Grid container justify="center">
                        <div className="Suncess-message" >
                            <span style={sucesssStyle}> {this.state.message}</span>
                        </div>
                    </Grid>
                </Grid>
                <Button
                    variant="contained"
                    color="secondary"
                    onClick={this.showPopUp}                
                />
            
                <KitItemAddModal                 
                    mainItemName={this.state.mainItemName}
                    open={this.state.open}
                    mainItemlabelName={this.state.mainItemName}
                    scanCodeName={this.state.scanCodeName}
                    scanCodelabelName={this.state.scanCodeName}
                    data={itemsdata}
                    onSearch={this.onSearch}
                    mainItem={this.state.mainItemValue}
                    scanCode={this.state.scanCodeValue}
                    onChecked={this.OnChecked}
                    showDisplay={this.state.showDisplay}
                    onScanCodeChange = {this.onScanCodeChange}
                    onMainItemChange = {this.onMainItemChange}
                    onClose = {this.onClose}
                />
            </React.Fragment>
        )
    }
}

export default withStyles(styles, { withTheme: true })(CreateKit);