import React, { FormEvent, Fragment, useContext, useEffect, useState, useCallback } from "react";
import { RouteComponentProps } from "react-router";
import { Button, CheckboxProps, Form, Grid, Input, InputOnChangeData } from "semantic-ui-react";
import agent from "../../../../api/agent";
import LoadingComponent from "../../../../layout/LoadingComponent";
import { AppContext } from "../../../../store";
import InvoiceDataAddCharge from "./components/InvoiceDataAddCharge";
import InvoiceDataCloseModal from "./components/InvoiceDataCloseModal";
import InvoiceDataRemoveCharge from "./components/InvoiceDataRemoveCharge";
import Charge from "../types/Charge";
import { toast } from "react-toastify";
import dateFormat from 'dateformat'

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
    const { state } = useContext(AppContext);
    const { orderDetails, region } = state;

    enum radioOptions {
        Invoice,
        Other,
        None
    }

    const [radioSelection, setRadioSelection] = useState(radioOptions.Invoice);
    const { purchaseOrderNumber } = match.params;
    const [selectedDate, setSelectedDate] = useState(
        dateFormat(new Date(), "yyyy-mm-dd") 
    );
    const [charges, setCharges] = useState<Charge[]>([]);
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [loadingMessage, setLoadingMessage] = useState("");
    // const currencies = [{ key: 0, text: "USD", value: "USD" }];

    const [currencies, setCurrencies] = useState<Currency[]>([])
    const [invoiceNumber, setInvoiceNumber] = useState<number|string|undefined>();
    const [invoiceCharges, setInvoiceCharges] = useState<number>(0);
    const [subteamTotal, setSubteamTotal] = useState<number>(0);
    const [difference, setDifference] = useState<number>(0);
    const [invoiceTotal, setInvoiceTotal] = useState<number>(0);

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
                ChargeId: ec.chargeID
            }}) : [])

            if(existingCharges) {
                let lineItemCharges = 0;
                let allocatedCharges = 0;
                let nonAllocatedCharges = 0;

                existingCharges.forEach((charge: any) => {
                    switch(charge.sacType){
                        case "Allocated":
                            allocatedCharges += charge.chargeValue;
                            break;
                        case "Not Allocated":
                            nonAllocatedCharges += charge.chargeValue;
                            break;
                        case "Line Item":
                            lineItemCharges += charge.chargeValue;
                            break;
                        default: 
                            break;
                    }
                });

                let totalCharges = allocatedCharges + nonAllocatedCharges + lineItemCharges;
                setInvoiceCharges(totalCharges);

                if(orderDetails) {
                    setSubteamTotal(orderDetails.InvoiceCost - (orderDetails.InvoiceFreight + allocatedCharges + nonAllocatedCharges + lineItemCharges))
                    if(orderDetails.InvoiceNumber !== ''){
                        setInvoiceNumber(orderDetails.InvoiceNumber);
                    }

                    let difference: number = 0;
                    if(orderDetails.InvoiceCost - totalCharges === orderDetails.AdjustedReceivedCost) {
                        difference = 0;
                    } else {
                        if(allocatedCharges + nonAllocatedCharges > 0) {
                            difference = orderDetails.InvoiceCost - orderDetails.AdjustedReceivedCost - totalCharges;
                        } else {
                            difference = orderDetails.InvoiceCost - orderDetails.AdjustedReceivedCost + totalCharges;
                        }
                    }

                    setDifference(difference);

                    //if ordertypeId is a transfer (3) or distribution (2)
                    if(orderDetails.OrderTypeId === 3 || orderDetails.OrderTypeId === 2) {
                        setInvoiceTotal(orderDetails.OrderedCost);
                    } else if(orderDetails.EInvId !== 0) {
                        setInvoiceTotal(orderDetails.InvoiceCost + nonAllocatedCharges);
                    } else {
                        setInvoiceTotal(orderDetails.InvoiceCost);
                    }
                }
            }
        }
        finally {
            setIsLoading(false);
        }
    }, [purchaseOrderNumber, region, orderDetails]);
    
    useEffect(() => {
        getOrderInvoiceCharges()
    }, [getOrderInvoiceCharges])

    const getCurrencies = useCallback(async () => {
        const allCurrencies = await agent.InvoiceData.getCurrencies(region);
        setCurrencies(allCurrencies && allCurrencies.map((c: any) => { 
            return {key: c.currencyID, value: c.currencyCode, text: c.currencyCode} }));
    }, [setCurrencies, region])

    useEffect(() => {
        getCurrencies()
    }, [getCurrencies])

    const closeOrder = async () => {
        //await agent.InvoiceData.closeOrder('SP', parseInt(purchaseOrderNumber), 1)
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
                toast.error(`Failed removing charge, ${result.errorMessage}`);
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
        setInvoiceNumber(parseInt(data.value));
    }

    return (
        <Fragment>
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
                        <Form.Field><label style={{textAlign: 'right', width: '83px'}}>Invoice #:</label><Input type='number' disabled={(orderDetails && orderDetails.EInvRequired) || radioSelection === radioOptions.None} value={invoiceNumber} onChange={handleInvoiceChange} transparent style={{backgroundColor: 'white', border: '1px solid black', width: '100px'}}/></Form.Field>
                        <select disabled={radioSelection === radioOptions.None} style={{ marginLeft: '50px', marginTop: '10px', backgroundColor: 'white', width: '90px', border: '1px solid black'}}>
                            {currencies && currencies.map(c => <option key={c.key} value={c.value}>{c.text}</option>)}</select>
                        <Form.Field ><label style={{textAlign: 'right', width: '80px'}}>Date:</label> <Input disabled={(orderDetails && orderDetails.EInvRequired) || radioSelection === radioOptions.None} max={dateFormat(new Date(), "yyyy-mm-dd")} 
                                    type="date"
                                    style={{width: '155px', border: '1px solid black'}}
                                    value={selectedDate}
                                    onChange={(event, { value }) => {
                                        setSelectedDate(value);
                                    }}/></Form.Field>
                        <Grid>
                            <Grid.Column width={9}>
                                <Form.Field style={{marginTop: '5px'}}><label style={{textAlign: 'right', width: '80px'}}>Inv. Total:</label> <Input value={invoiceTotal} disabled={(orderDetails && orderDetails.EInvRequired) || radioSelection === radioOptions.None} transparent type='number' style={{width: '80px', backgroundColor: 'white', border: '1px solid black'}}/></Form.Field>
                                <Form.Field style={{marginTop: '5px'}}><label style={{textAlign: 'right', width: '80px'}}>Charges:</label> <Input value={invoiceCharges} disabled={radioSelection === radioOptions.None} type='number' style={{width: '80px', backgroundColor: 'lightgreen', border: '1px solid black'}} transparent/></Form.Field>
                                <Form.Field style={{marginTop: '5px'}}><label style={{textAlign: 'right', width: '80px'}}>{orderDetails ? orderDetails.Subteam : "<Undefined SUBTEAM>"}:</label> <Input value={subteamTotal} disabled={radioSelection === radioOptions.None} type='number' style={{width: '80px', backgroundColor: 'lightgreen', border: '1px solid black'}} transparent/></Form.Field>
                                <Form.Field style={{marginTop: '5px'}}><label style={{textAlign: 'right', width: '80px'}}>Difference:</label> <Input value={difference} disabled={radioSelection === radioOptions.None} type='number' style={{width: '80px', backgroundColor: 'lightgreen', border: '1px solid black'}} transparent/></Form.Field>
                            </Grid.Column>
                            <Grid.Column width={7}>
                                <div style={{marginTop: '10px', marginRight: '20px', border: '1px solid black'}}>
                                    <Button disabled={orderDetails ? orderDetails.EInvId === 0 : false} style={{margin: '10px', backgroundColor:'lightgreen'}}>Reparse eInv</Button>
                                    <Button style={{margin: '10px', backgroundColor:'lightgreen'}}>Refuse Order</Button>
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
                    { isLoading ? 
                    <LoadingComponent content={loadingMessage}/> :
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
                    }
                </div>
            </Form>

            <div style={{position: 'absolute', bottom: '5px', width: '100%', textAlign: 'center'}}>
                <InvoiceDataCloseModal invoiceDate={selectedDate} handleClose={closeOrder}/>
            </div>

        </Fragment>
    );
};

export default InvoiceData;
