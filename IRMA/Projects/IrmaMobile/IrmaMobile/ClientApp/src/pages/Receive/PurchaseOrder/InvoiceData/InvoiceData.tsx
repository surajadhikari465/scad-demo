import React, { FormEvent, Fragment, useContext, useEffect, useState, useCallback, ChangeEvent } from "react";
import { RouteComponentProps } from "react-router";
import { Button, CheckboxProps, Form, Grid, Input, InputOnChangeData } from "semantic-ui-react";
import agent from "../../../../api/agent";
import LoadingComponent from "../../../../layout/LoadingComponent";
import { AppContext, types, IMenuItem } from "../../../../store";
import InvoiceDataAddCharge from "./components/InvoiceDataAddCharge";
import InvoiceDataRemoveCharge from "./components/InvoiceDataRemoveCharge";
import Charge from "../types/Charge";
import { toast } from "react-toastify";
import dateFormat from 'dateformat'
import InvoiceDataRefuseCodes from "./components/InvoiceDataRefuseCodes";
import ConfirmModal from "../../../../layout/ConfirmModal";
import orderUtil from "../util/Order";
import isMinDate from "../util/MinDate";
import { useHistory } from "react-router-dom";
import Config from "../../../../config";
import OrderItem from "../types/OrderItem";

interface RouteParams {
    purchaseOrderNumber: string;
}

interface IProps extends RouteComponentProps<RouteParams> {}

interface Currency {
    key: number;
    value: string;
    text: string;
}

const InvoiceData: React.FC<IProps> = ({ match }) => {
    //@ts-ignore
    const { state, dispatch } = useContext(AppContext);
    const { orderDetails, region } = state;
    let history = useHistory();

    enum radioOptions {
        Invoice,
        Other,
        None
    }

    const [radioSelection, setRadioSelection] = useState(radioOptions.Invoice);
    const { purchaseOrderNumber } = match.params;
    const [invoiceDate, setInvoiceDate] = useState<string>(dateFormat(new Date(), 'UTC:yyyy-mm-dd'));
    const [invoiceDateEdited, setInvoiceDateEdited] = useState<boolean>(false);
    const [charges, setCharges] = useState<Charge[]>([]);
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [showRefuseModal, setShowRefuseModal] = useState<boolean>(false);
    const [showReparseModal, setShowReparseModal] = useState<boolean>(false);
    const [refuseCode, setRefuseCode] = useState<string>('');
    const [refuseCodeId, setRefuseCodeId] = useState<number>(-1);
    const [loadingMessage, setLoadingMessage] = useState("");

    const [currencies, setCurrencies] = useState<Currency[]>([]);
    const [selectedCurrency, setSelectedCurrency] = useState<string>('');
    const [invoiceNumber, setInvoiceNumber] = useState<string|undefined>();
    const [invoiceNumberEdited, setInvoiceNumberEdited] = useState<boolean>(false)
    const [invoiceCharges, setInvoiceCharges] = useState<number>(0);
    const [subteamTotal, setSubteamTotal] = useState<number>(0);
    const [difference, setDifference] = useState<number>(0);
    const [invoiceTotal, setInvoiceTotal] = useState<number>(0);
    const [nonAllocatedCharges, setNonAllocatedCharges] = useState<number>(0);
    const [invoiceTotalEdited, setInvoiceTotalEdited] = useState<boolean>(false);
    const [canCloseOrder, setCanCloseOrder] = useState<boolean>(false);

    const setMenuItems = useCallback(() => {
        const newMenuItems = [
            { id: 1, order: 1, text: "Receive Order", path: `/receive/purchaseorder/`, disabled: false } as IMenuItem,
         ] as IMenuItem[];

        dispatch({ type: types.SETMENUITEMS, menuItems: newMenuItems });
    }, [dispatch])

    useEffect(() => {
        setMenuItems()

        return () => {
            dispatch({ type: types.SETMENUITEMS, menuItems: [] });
        }
    }, [setMenuItems, dispatch]);

    useEffect(() => {
        if(orderDetails) {
            setCanCloseOrder(orderDetails.OrderItems.filter((oi: OrderItem) => oi.QtyReceived > 0).length > 0);
        }
    }, [orderDetails, setCanCloseOrder])

    const loadValuesAndState = useCallback(() => {
        if(charges) {
            let lineItemCharges = 0;
            let allocatedCharges = 0;
            let nonAllocatedChargesLocal = 0;

            charges.forEach((charge: any) => {
                switch(charge.SACType){
                    case "Allocated":
                        if(charge.IsAllowance === 'False'){
                            allocatedCharges += charge.ChargeValue;
                        }
                        break;
                    case "Not Allocated":
                        nonAllocatedChargesLocal += charge.ChargeValue;
                        break;
                    case "Line Item":
                        lineItemCharges += charge.ChargeValue;
                        break;
                    default: 
                        break;
                }
            });

            let totalCharges = allocatedCharges + nonAllocatedChargesLocal + lineItemCharges;
            setNonAllocatedCharges(nonAllocatedChargesLocal);
            setInvoiceCharges(totalCharges);

            if(!orderDetails) {
                return;
            }

            if(!invoiceNumberEdited) {
                switch(radioSelection) { 
                    case radioOptions.Invoice:
                        if(orderDetails.InvoiceNumber !== ''){
                            setInvoiceNumber(orderDetails.InvoiceNumber);
                        }
                        break;
                    case radioOptions.Other:
                        if(orderDetails.VendorDocId){
                            setInvoiceNumber(orderDetails.VendorDocId.toString());
                        }
                        break;
                }
            }

            if(!invoiceDateEdited) {
                switch(radioSelection) { 
                    case radioOptions.Invoice:
                        if(orderDetails.InvoiceDate && !isMinDate(orderDetails.InvoiceDate)) {
                            setInvoiceDate(dateFormat(orderDetails.InvoiceDate, 'yyyy-mm-dd'));
                        } else {
                            setInvoiceDate(dateFormat(new Date(), 'yyyy-mm-dd'));
                        }
                        break;
                    case radioOptions.Other:
                        if(orderDetails.VendorDocDate) {
                            setInvoiceDate(dateFormat(orderDetails.VendorDocDate, 'yyyy-mm-dd'));
                        }
                        break;
                }
            }
                
            setSubteamTotal(orderDetails.InvoiceCost - (orderDetails.InvoiceFreight + allocatedCharges + nonAllocatedChargesLocal + lineItemCharges))

            let difference: number = 0;
            if(orderDetails.InvoiceCost - totalCharges === orderDetails.AdjustedReceivedCost) {
                difference = 0;
            } else {
                if(allocatedCharges + nonAllocatedChargesLocal > 0) {
                    difference = orderDetails.InvoiceCost - orderDetails.AdjustedReceivedCost - totalCharges;
                } else {
                    difference = orderDetails.InvoiceCost - orderDetails.AdjustedReceivedCost + totalCharges;
                }
            }

            setDifference(difference);

            if(!invoiceTotalEdited) {
                //if ordertypeId is a transfer (3) or distribution (2)
                if(orderDetails.OrderTypeId === 3 || orderDetails.OrderTypeId === 2) {
                    setInvoiceTotal(parseFloat(orderDetails.OrderedCost.toFixed(2)));
                } else if(orderDetails.EInvId !== 0) {
                    setInvoiceTotal(parseFloat((orderDetails.InvoiceCost + nonAllocatedChargesLocal).toFixed(2)));
                } else {
                    setInvoiceTotal(parseFloat(orderDetails.InvoiceCost.toFixed(2)));
                }
            }

            if(!orderDetails.CurrencyId || orderDetails.CurrencyId === 0){
                switch(region) {
                    case 'EU':
                    case 'UK':
                        setSelectedCurrency("GBP");
                        break;
                    default:
                        setSelectedCurrency("USD");
                }
            } else {
                switch(orderDetails.CurrencyId) {
                    case 1:
                        if(region in ['EU', 'UK']) {
                            setSelectedCurrency('GBP');
                        } else {
                            setSelectedCurrency("USD")
                        }
                        break;
                    case 2:
                        setSelectedCurrency("CAD");
                }
            }
            
        }
    }, [charges, radioOptions, orderDetails, radioSelection, region, invoiceTotalEdited, invoiceDateEdited, invoiceNumberEdited]); 

    const getOrderInvoiceCharges = useCallback(async () => {
        try {
            setIsLoading(true);
            setLoadingMessage('Loading charges...');
            const existingCharges = await agent.InvoiceData.getOrderInvoiceCharges(region, parseInt(purchaseOrderNumber));
            setCharges(existingCharges ? existingCharges.map((ec: any) => { return {
                SACType: ec.sacType,
                GLPurchaseAccount: ec.glPurchaseAccount,
                Description: ec.description,
                ChargeValue: ec.chargeValue,
                ChargeId: ec.chargeID,
                IsAllowance: ec.isAllowance
            }}) : [])
        }
        finally {
            setIsLoading(false);
        }
    }, [purchaseOrderNumber, region]);
    
    useEffect(() => {
        getOrderInvoiceCharges();
    }, [getOrderInvoiceCharges])

    useEffect(() => {
        dispatch({ type: types.SETTITLE, Title: 'Invoice Data' });
        return () => {
          dispatch({ type: types.SETTITLE, Title: 'IRMA Mobile' });
        };
      }, [dispatch]);

    const getCurrencies = useCallback(async () => {
        const allCurrencies = await agent.InvoiceData.getCurrencies(region);
        setCurrencies(allCurrencies && allCurrencies.map((c: any) => { 
            return {key: c.currencyID, value: c.currencyCode, text: c.currencyCode} }));
    }, [setCurrencies, region])

    useEffect(() => {
        getCurrencies()
    }, [getCurrencies])

    useEffect(() => {
        loadValuesAndState()
    }, [loadValuesAndState])

    const closeOrder = async () => {
        if(!orderDetails) {
            return;
        }

        if(!invoiceNumber) {
            toast.error("Please enter an invoice number", { autoClose: false});
            return;
        }

        try {
            setLoadingMessage("Closing Order...");
            setIsLoading(true);

            const invoiceCost = invoiceTotal - nonAllocatedCharges - orderDetails.InvoiceFreight;

            var updateResult = await agent.InvoiceData.updateOrderBeforeClosing(region, orderDetails.OrderId, invoiceNumber.toString(), new Date(invoiceDate), invoiceCost, 
                                                                                    orderDetails.VendorDocId ? orderDetails.VendorDocId.toString() :  '', orderDetails.VendorDocDate, orderDetails.SubteamNo, 
                                                                                    orderDetails.PartialShipment)

            if((updateResult && !updateResult.status) || !updateResult) {
                toast.error(`Error when updating order before closing: ${updateResult.errorMessage || 'No message given'}`, { autoClose: false })
                return;
            }

            var result = await agent.InvoiceData.closeOrder(region, parseInt(purchaseOrderNumber), Config.userId)

            if(result && result.status) {
                toast.info('Order Closed', { autoClose: false });

                dispatch({ type: types.SETPURCHASEORDERUPC, purchaseOrderUpc: '' });
                dispatch({ type: types.SETPURCHASEORDERNUMBER, purchaseOrderNumber: '' });
                dispatch({ type: types.SETORDERDETAILS, orderDetails: null });

                history.push("/receive/PurchaseOrder");
            } else {
                toast.error(`Error when closing order: ${result.errorMessage || 'No message given'}`, { autoClose: false })
            }
        }
        finally {
            setIsLoading(false);
        }
    }

    const handleChange = (
        event: FormEvent<HTMLInputElement>,
        data: CheckboxProps
    ) => {
        let val: radioOptions;
        switch (data.value) {
            case 0:
                val = radioOptions.Invoice;
                break;
            case 1:
                val = radioOptions.Other;
                break;
            case 2:
                val = radioOptions.None;
                break;
            default:
                throw new Error("Unknown radio option");
        }

        setRadioSelection(val);
        loadValuesAndState();
    };

    const handleAddCharge = async (sacType: number, description: string, subteamGLAccount: number, amount: number, isAllowance: boolean) => {
        const newChargeId = charges.length > 0 ? Math.max(...charges.map(ch => ch.ChargeId)) + 1 : 1;
        const newCharge: Charge = {ChargeId: newChargeId, SACType: sacType, GLPurchaseAccount: subteamGLAccount, Description: description, ChargeValue: amount}

        try {
            setIsLoading(true);
            setLoadingMessage('Adding charge...');
            await agent.InvoiceData.addInvoiceCharge(region, parseInt(purchaseOrderNumber), sacType, newCharge.Description, newCharge.GLPurchaseAccount, isAllowance, amount);
        }
        finally {
            setIsLoading(false);
        }

        await getOrderInvoiceCharges()
    }
    
    const handleRemoveCharge = async (chargeId: number) => {
        try {
            setIsLoading(true);
            setLoadingMessage('Removing charge...');
            const result = await agent.InvoiceData.removeInvoiceCharge(region, chargeId);

            if (!result.status) {
                toast.error(`Failed removing charge, ${result.errorMessage || 'No message given'}`, { autoClose: false });
                return;
            }

            await getOrderInvoiceCharges();
        }
        finally {
            setIsLoading(false);
        }
    }

    const handleInvoiceChange = (event: FormEvent<HTMLInputElement>,
        data: InputOnChangeData) => {
        if(!orderDetails?.EInvRequired) {
            if(data.value.includes('|')){
                toast.error("The invoice number cannot contain a '|' character");
                return;
            }
            setInvoiceNumberEdited(true);
            setInvoiceNumber(data.value);
        }
    }

    const handleSelectRefuse = (code: string, id: number) => {
        setRefuseCode(code);
        setRefuseCodeId(id);
        setShowRefuseModal(true);
    }

    const handleConfirmRefuse = async () => {
        const result = await agent.InvoiceData.refuseOrder(region, parseInt(purchaseOrderNumber), Config.userId, refuseCodeId)

        if(result.status) {
            toast.info('Order Refused')
        } else {
            toast.error(`Error when refusing order: ${result.errorMessage || 'No message given'}`, { autoClose: false })
        }
    }

    const handleReparseEInv = async () => {
        if(!orderDetails) {
            return;
        }

        try {
            setIsLoading(true);
            setLoadingMessage('Reparsing eInvoice...');

            var result = await agent.InvoiceData.reparseEInvoice(region, orderDetails.EInvId);

            if(result.status) {
                toast.info("eInvoice Reparsed");
            } else {
                toast.error(`Error when reparsing eInvoice: ${result.errorMessage || 'No message given'}`, { autoClose: false })
            }

            var order = await agent.PurchaseOrder.detailsFromPurchaseOrderNumber(region, parseInt(purchaseOrderNumber));
            var reparsedOrderDetails = orderUtil.MapOrder(order);

            dispatch({ type: types.SETORDERDETAILS, orderDetails: reparsedOrderDetails });
            setInvoiceNumberEdited(false);
            setInvoiceDateEdited(false);
            setInvoiceTotalEdited(false);
        }
        finally {
            setIsLoading(false);
        }
    }

    const handleCurrencyChange = (event: ChangeEvent<HTMLSelectElement>) => {
        setSelectedCurrency( event.target.value ? event.target.value.toString() : "");
    }

    const handleInvoiceTotalChange = (event: React.ChangeEvent<HTMLInputElement>, data: InputOnChangeData) => {
        if(!orderDetails?.EInvRequired && radioSelection === radioOptions.Invoice) {
            setInvoiceTotalEdited(true);
            setInvoiceTotal(parseFloat(data.value));
        }
    }

    return (
        
        <Fragment>
            { isLoading ? 
            <LoadingComponent content={loadingMessage}/> :
            <Fragment>
                <ConfirmModal handleConfirmClose={handleConfirmRefuse} setOpenExternal={setShowRefuseModal} showTriggerButton={false} 
                                openExternal={showRefuseModal} headerText='Invoice Data' cancelButtonText='No' confirmButtonText='Yes' 
                                lineOne={`Refuse PO ${purchaseOrderNumber} with Reason Code ${refuseCode}?`} />
                <ConfirmModal handleConfirmClose={handleReparseEInv} setOpenExternal={setShowReparseModal} showTriggerButton={false} 
                                openExternal={showReparseModal} headerText='Invoice Data' cancelButtonText='No' confirmButtonText='Yes' 
                                lineOne={'Reparse eInvoice?'} />
                <Form>
                    <div style={{position: "relative", left: "20%"}}>
                        <Form.Group inline style={{marginTop: '10px'}}>
                            <Form.Radio
                                label="Invoice"
                                value={radioOptions.Invoice}
                                checked={radioSelection === radioOptions.Invoice}
                                onChange={handleChange}
                            />
                            <Form.Radio
                                label="Other"
                                value={radioOptions.Other}
                                checked={radioSelection === radioOptions.Other}
                                onChange={handleChange}
                            />
                            <Form.Radio
                                label="None"
                                value={radioOptions.None}
                                checked={radioSelection === radioOptions.None}
                                onChange={handleChange}
                            />
                        </Form.Group>
                    </div>
                    <div style={{backgroundColor: 'lightgrey', border: '1px solid black'}}>
                        <Form.Group inline>
                            <Form.Field><label style={{textAlign: 'right', width: '83px'}}>Invoice #:</label><Input disabled={(orderDetails && orderDetails.EInvRequired) || radioSelection === radioOptions.None} value={invoiceNumber || ""} onChange={handleInvoiceChange} transparent style={{backgroundColor: 'white', border: '1px solid black', width: '100px'}}/></Form.Field>
                            <select value={selectedCurrency} onChange={handleCurrencyChange} disabled={radioSelection === radioOptions.None} style={{ marginLeft: '50px', marginTop: '10px', backgroundColor: 'white', width: '90px', border: '1px solid black'}}>
                                {currencies && currencies.map(c => <option key={c.key} value={c.value}>{c.text}</option>)}</select>
                            <Form.Field ><label style={{textAlign: 'right', width: '80px'}}>Date:</label> <Input disabled={(orderDetails?.EInvRequired) || radioSelection === radioOptions.None} max={dateFormat(new Date(), "yyyy-mm-dd")} 
                                        type="date"
                                        style={{width: '155px', border: '1px solid black'}}
                                        value={radioSelection === radioOptions.None ? "" : invoiceDate}
                                        onChange={(event, { value }) => {
                                            setInvoiceDateEdited(true); 
                                            setInvoiceDate(value);
                                        }}/></Form.Field>
                            <Grid>
                                <Grid.Column width={9}>
                                    <Form.Field style={{marginTop: '5px'}}><label style={{textAlign: 'right', width: '80px'}}>Inv. Total:</label> <Input onChange={handleInvoiceTotalChange} value={radioSelection === radioOptions.Invoice ? invoiceTotal : ""} disabled={(orderDetails && orderDetails.EInvRequired) || radioSelection === radioOptions.None} transparent type='number' style={{width: '80px', backgroundColor: 'white', border: '1px solid black'}}/></Form.Field>
                                    <Form.Field style={{marginTop: '5px'}}><label style={{textAlign: 'right', width: '80px'}}>Charges:</label> <Input value={radioSelection === radioOptions.Invoice ? invoiceCharges.toFixed(2) : ""} disabled={radioSelection === radioOptions.None} type='number' style={{width: '80px', backgroundColor: 'lightgreen', border: '1px solid black'}} transparent/></Form.Field>
                                    <Form.Field style={{marginTop: '5px'}}><label style={{textAlign: 'right', width: '80px'}}>{orderDetails ? orderDetails.Subteam : "<Undefined SUBTEAM>"}:</label> <Input value={radioSelection === radioOptions.Invoice ? subteamTotal.toFixed(2) : ""} disabled={radioSelection === radioOptions.None} type='number' style={{width: '80px', backgroundColor: 'lightgreen', border: '1px solid black'}} transparent/></Form.Field>
                                    <Form.Field style={{marginTop: '5px'}}><label style={{textAlign: 'right', width: '80px'}}>Difference:</label> <Input value={radioSelection === radioOptions.Invoice ? difference.toFixed(2) : ""} disabled={radioSelection === radioOptions.None} type='number' style={{width: '80px', backgroundColor: 'lightgreen', border: '1px solid black'}} transparent/></Form.Field>
                                </Grid.Column>
                                <Grid.Column width={7}>
                                    <div style={{marginTop: '10px', marginRight: '20px', border: '1px solid black'}}>
                                        <Button disabled={orderDetails?.EInvId === 0 || radioSelection !== radioOptions.Invoice} style={{margin: '10px', backgroundColor:'lightgreen'}} onClick={() => setShowReparseModal(true)}>Reparse eInv</Button>
                                        <InvoiceDataRefuseCodes handleSelectRefuse={handleSelectRefuse}/>
                                    </div>
                                </Grid.Column>
                            </Grid>
                        </Form.Group>

                        <div style={{backgroundColor: 'grey', border: '1px solid black'}}>
                            <Grid columns="equal">
                                <Grid.Column textAlign="center" floated='left' style={{marginTop: '10px', marginBottom: '10px'}}>
                                    <InvoiceDataAddCharge disabled={radioSelection === radioOptions.None} handleAddCharge={handleAddCharge} orderId={parseInt(purchaseOrderNumber)}/>
                                </Grid.Column>
                                <Grid.Column textAlign="center" verticalAlign="middle" style={{fontWeight: 'bold', fontSize: '16px'}}>
                                    Charges
                                </Grid.Column>
                                <Grid.Column textAlign="center" floated='right' style={{marginTop: '10px', marginBottom: '10px'}}>
                                    <InvoiceDataRemoveCharge disabled={radioSelection === radioOptions.None} handleRemoveCharge={handleRemoveCharge} charges={charges}/>
                                </Grid.Column>
                            </Grid>
                        </div>
                    </div>
                    <div style={{height: '145px', border: '1px solid black', backgroundColor: 'darkgrey'}}>                    
                        <Grid divided columns={4} style={{marginLeft: '0px', marginRight: '0px', marginTop: '0px', overflow: 'auto', maxHeight: '145px'}}>
                            <Grid.Row style={{backgroundColor: 'lightgrey', padding: '0px', borderBottom: '1px solid black'}}>
                                <Grid.Column width={4}>Type</Grid.Column>
                                <Grid.Column width={4} style={{fontSize: "12px"}}>GL Acct</Grid.Column>
                                <Grid.Column width={5}>Desc</Grid.Column>
                                <Grid.Column width={3}>Value</Grid.Column>
                            </Grid.Row>

                            {charges.map(charge => 
                            <Grid.Row key={charge.ChargeId} style={{backgroundColor:'white', padding: '0px', borderBottom: '1px solid black'}}>
                                <Grid.Column width={4} style={{fontSize: '12px'}}>{(charge.SACType === 1 || charge.SACType === "Allocated") ? "Allocated" : "Non-Alloc."}</Grid.Column>
                                <Grid.Column width={4}>{charge.GLPurchaseAccount <= 0 ? "" : charge.GLPurchaseAccount}</Grid.Column>
                                <Grid.Column width={5} style={{fontSize: '12px'}}>{charge.Description}</Grid.Column>
                                <Grid.Column width={3}>{charge.ChargeValue}</Grid.Column>
                            </Grid.Row>)}
                        </Grid>
                    </div>
                </Form>

                <div style={{position: 'absolute', bottom: '5px', width: '100%', textAlign: 'center'}}>
                    <ConfirmModal triggerButtonText="Close Order" handleConfirmClose={closeOrder} lineOne={`Invoice Date: ${dateFormat(new Date(), 'mm/dd/yyyy')}`} 
                                    lineTwo="Close this order?" headerText='Invoice Data' confirmButtonText="OK" cancelButtonText="Cancel" enableButton={canCloseOrder}/>
                </div>
            </Fragment>
            }
        </Fragment>
    );
};

export default InvoiceData;