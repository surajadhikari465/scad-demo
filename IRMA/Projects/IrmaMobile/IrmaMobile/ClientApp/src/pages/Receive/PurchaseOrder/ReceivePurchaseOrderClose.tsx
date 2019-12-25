import React, { FormEvent, Fragment, useContext, useEffect, useState, useCallback } from "react";
import { RouteComponentProps } from "react-router";
import { Button, CheckboxProps, Form, Grid, Input } from "semantic-ui-react";
import agent from "../../../api/agent";
import LoadingComponent from "../../../layout/LoadingComponent";
import { AppContext } from "../../../store";
import ReceivePurchaseOrderCloseAddCharge from "./components/ReceivePurchaseOrderCloseAddCharge";
import ReceivePurchaseOrderCloseModal from "./components/ReceivePurchaseOrderCloseModal";
import ReceivePurchaseOrderCloseRemoveCharge from "./components/ReceivePurchaseOrderCloseRemoveCharge";
import Charge from "./types/Charge";
import { toast } from "react-toastify";

interface RouteParams {
    purchaseOrderNumber: string;
}

interface IProps extends RouteComponentProps<RouteParams> {}

const ReceivePurchaseOrderClose: React.FC<IProps> = ({ match }) => {
    const { state } = useContext(AppContext);
    const { region } = state

    enum radioOptions {
        Invoice,
        Other,
        None
    }

    const [radioSelection, setRadioSelection] = useState(radioOptions.Invoice);
    const { purchaseOrderNumber } = match.params;
    const [selectedDate, setSelectedDate] = useState(
        new Date().toISOString().split("T")[0]
    );
    const [charges, setCharges] = useState<Charge[]>([]);
    const [isLoadingCharges, setIsLoadingCharges] = useState<boolean>(false);
    const [loadingChargesMessage, setLoadingChargesMessage] = useState("");

    const getOrderInvoiceCharges = useCallback(async () => {
        try {
            setIsLoadingCharges(true);
            setLoadingChargesMessage('Loading charges...');
            const existingCharges = await agent.PurchaseOrderClose.getOrderInvoiceCharges(region, parseInt(purchaseOrderNumber));
            setCharges(existingCharges ? existingCharges.map((ec: any) => { return {
                SACType: ec.sacType,
                GLPurchaseAccount: ec.glPurchaseAccount,
                Description: ec.description,
                ChargeValue: ec.chargeValue,
                ChargeId: ec.chargeID
            }}) : [])
        }
        finally {
            setIsLoadingCharges(false);
        }
    }, [purchaseOrderNumber, region]);

    useEffect(() => {
        getOrderInvoiceCharges()
    }, [getOrderInvoiceCharges])

    const closeOrder = async () => {
        //await agent.PurchaseOrderClose.closeOrder('SP', parseInt(purchaseOrderNumber), 1)
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

    const currencies = [{ key: 0, text: "USD", value: "USD" }];

    const handleAddCharge = async (sacType: number, description: string, subteamGLAccount: number, amount: number, isAllowance: boolean) => {
        const newChargeId = charges.length > 0 ? Math.max(...charges.map(ch => ch.ChargeId)) + 1 : 1;
        const newCharge: Charge = {ChargeId: newChargeId, SACType: sacType, GLPurchaseAccount: subteamGLAccount, Description: description, ChargeValue: amount}

        try {
            setIsLoadingCharges(true);
            setLoadingChargesMessage('Adding charge...');
            await agent.PurchaseOrderClose.addInvoiceCharge(region, parseInt(purchaseOrderNumber), sacType, newCharge.Description, newCharge.GLPurchaseAccount, isAllowance, amount);
        }
        finally {
            setIsLoadingCharges(false);
        }

        getOrderInvoiceCharges()
    }
    
    const handleRemoveCharge = async (chargeId: number) => {
        try {
            setIsLoadingCharges(true);
            setLoadingChargesMessage('Removing charge...');
            const result = await agent.PurchaseOrderClose.removeInvoiceCharge(region, chargeId);

            if (!result.status) {
                toast.error(`Failed removing charge, ${result.errorMessage}`);
                return;
            }

            await getOrderInvoiceCharges();
        }
        finally {
            setIsLoadingCharges(false);
        }
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
                        <Form.Field><label style={{textAlign: 'right', width: '83px'}}>Invoice #:</label><Input disabled={radioSelection === radioOptions.None} transparent style={{backgroundColor: 'white', border: '1px solid black', width: '100px'}}/></Form.Field>
                        <select disabled={radioSelection === radioOptions.None} style={{ marginLeft: '50px', marginTop: '10px', backgroundColor: 'white', width: '90px', border: '1px solid black'}}>
                            {currencies.map(c => <option key={c.key} value={c.value}>{c.text}</option>)}</select>
                        <Form.Field ><label style={{textAlign: 'right', width: '80px'}}>Date:</label> <Input disabled={radioSelection === radioOptions.None} max={new Date().toISOString().split("T")[0]} 
                                    type="date"
                                    style={{width: '155px', border: '1px solid black'}}
                                    value={selectedDate}
                                    onChange={(event, { value }) => {
                                        setSelectedDate(value);
                                    }}/></Form.Field>
                        <Grid>
                            <Grid.Column width={9}>
                                <Form.Field style={{marginTop: '5px'}}><label style={{textAlign: 'right', width: '80px'}}>Inv. Total:</label> <Input disabled={radioSelection === radioOptions.None} transparent type='number' style={{width: '80px', backgroundColor: 'white', border: '1px solid black'}}/></Form.Field>
                                <Form.Field style={{marginTop: '5px'}}><label style={{textAlign: 'right', width: '80px'}}>Charges:</label> <Input disabled={radioSelection === radioOptions.None} type='number' style={{width: '80px', backgroundColor: 'lightgreen', border: '1px solid black'}} transparent/></Form.Field>
                                <Form.Field style={{marginTop: '5px'}}><label style={{textAlign: 'right', width: '80px'}}>{"<SUBTEAM>:"}</label> <Input disabled={radioSelection === radioOptions.None} type='number' style={{width: '80px', backgroundColor: 'lightgreen', border: '1px solid black'}} transparent/></Form.Field>
                                <Form.Field style={{marginTop: '5px'}}><label style={{textAlign: 'right', width: '80px'}}>Refused:</label> <Input disabled={radioSelection === radioOptions.None} type='number' style={{width: '80px', backgroundColor: 'lightgreen', border: '1px solid black'}} transparent/></Form.Field>
                                <Form.Field style={{marginTop: '5px'}}><label style={{textAlign: 'right', width: '80px'}}>Difference:</label> <Input disabled={radioSelection === radioOptions.None} type='number' style={{width: '80px', backgroundColor: 'lightgreen', border: '1px solid black'}} transparent/></Form.Field>
                            </Grid.Column>
                            <Grid.Column width={7}>
                                <div style={{marginTop: '10px', marginRight: '20px', border: '1px solid black'}}>
                                    <Button style={{margin: '10px', backgroundColor:'lightgreen'}}>Reparse eInv</Button>
                                    <Button style={{margin: '10px', backgroundColor:'lightgreen'}}>Refuse Order</Button>
                                </div>
                            </Grid.Column>
                        </Grid>
                    </Form.Group>

                    <div style={{backgroundColor: 'grey', border: '1px solid black'}}>
                        <Grid columns="equal">
                            <Grid.Column textAlign="center" floated='left' style={{marginTop: '10px', marginBottom: '10px'}}>
                                <ReceivePurchaseOrderCloseAddCharge disabled={radioSelection === radioOptions.None} handleAddCharge={handleAddCharge} orderId={parseInt(purchaseOrderNumber)}/>
                            </Grid.Column>
                            <Grid.Column textAlign="center" verticalAlign="middle" style={{fontWeight: 'bold', fontSize: '16px'}}>
                                Charges
                            </Grid.Column>
                            <Grid.Column textAlign="center" floated='right' style={{marginTop: '10px', marginBottom: '10px'}}>
                                <ReceivePurchaseOrderCloseRemoveCharge disabled={radioSelection === radioOptions.None} handleRemoveCharge={handleRemoveCharge} charges={charges}/>
                            </Grid.Column>
                        </Grid>
                    </div>
                </div>
                <div style={{height: '145px', border: '1px solid black', backgroundColor: 'darkgrey'}}>
                    { isLoadingCharges ? 
                    <LoadingComponent content={loadingChargesMessage}/> :
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
                <ReceivePurchaseOrderCloseModal invoiceDate={selectedDate} handleClose={closeOrder}/>
            </div>

        </Fragment>
    );
};

export default ReceivePurchaseOrderClose;
