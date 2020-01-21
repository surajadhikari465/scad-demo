import React, {
    ChangeEvent,
    Fragment,
    SyntheticEvent,
    useContext,
    useEffect,
    useState,
    useRef
} from "react";
import { Dropdown, Grid, Input, Segment, Button } from "semantic-ui-react";
import agent from "../../../../../api/agent";
import { AppContext, types } from "../../../../../store";
import { ReasonCode } from "../../types/ReasonCode";
import ReceivePurchaseOrderReasonCodeModal from "./ReceivePurchaseOrderReasonCodeModal";
import "./styles.scss";
import { toast } from "react-toastify";
import { WfmButton } from "@wfm/ui-react";
import { QuantityAddMode } from "../../types/QuantityAddMode";
import ReceivePurchaseOrderDetailsQtyModal from "./ReceivePurchaseOrderDetailsQtyModal";
import Config from "../../../../../config";
import OrderItem from "../../types/OrderItem";
import ConfirmModal from "../../../../../layout/ConfirmModal";
import { Textfit } from 'react-textfit';

const ReceivePurchaseOrderDetails: React.FC = () => {
    // @ts-ignore
    const { state, dispatch } = useContext(AppContext);
    const { orderDetails, region, mappedReasonCodes, purchaseOrderUpc } = state;
    const [receivingOrder, setReceivingOrder] = useState<boolean>(false);
    const [showQtyModal, setShowQtyModal] = useState<boolean>(false);
    const [showHighQtyModal, setShowHighQtyModal] = useState<boolean>(false);
    const overrideHighQty = useRef(false);
    const quantityMode = useRef(QuantityAddMode.None);

    useEffect(() => {
        const loadReasonCodes = async () => {
            const reasonCodes: ReasonCode[] = await agent.PurchaseOrder.reasonCodes(
                region
            );

            const mappedReasonCodes = reasonCodes && reasonCodes.map((code: ReasonCode) => ({
                key: code.reasonCodeID,
                text: code.reasonCodeAbbreviation,
                value: code.reasonCodeID
            }));

            dispatch({
                type: types.SETMAPPEDREASONCODES,
                mappedReasonCodes: mappedReasonCodes
            });
            dispatch({ type: types.SETREASONCODES, reasonCodes: reasonCodes });
        };

        loadReasonCodes();
    }, [dispatch, region]);

    const handleInputChange = (event: ChangeEvent<HTMLInputElement>) => {
        const { name, value } = event.currentTarget;

        if(value === "" && purchaseOrderUpc !== "") {
            dispatch({
                type: types.SETORDERDETAILS,
                orderDetails: { ...orderDetails, [name]: 0 }
            });
        } else if(!isNaN(parseInt(value))) {
            dispatch({
                type: types.SETORDERDETAILS,
                orderDetails: { ...orderDetails, [name]: parseInt(value) }
            });
        } else {
            dispatch({
                type: types.SETORDERDETAILS,
                orderDetails: { ...orderDetails, [name]: value }
            });
        }
    };

    const handleDropdownChange = (
        event: SyntheticEvent<HTMLElement, Event>,
        { value }: any
    ) => {
        dispatch({
            type: types.SETORDERDETAILS,
            orderDetails: { ...orderDetails, Code: value }
        });
    };

    const reasonCodeClick = () => {
        dispatch({
            type: types.SETMODALOPEN,
            modalOpen: true
        });
    };

    const receiveOrder = async () => {
        if (orderDetails) {
            if (orderDetails.Code === -1) {
                toast.error("Please enter a Reason Code", { autoClose: false });
                return;
            }

            if(orderDetails.Quantity === 0) {
                toast.error("Please enter a quantity of at least 1.", { autoClose: false });
                return;
            }

            if(!overrideHighQty.current && orderDetails.QtyReceived !== 0 && quantityMode.current === QuantityAddMode.None) {
                setShowQtyModal(true);
                return;
            }

            const quantity: number = quantityMode.current === QuantityAddMode.AddTo ? orderDetails.Quantity + orderDetails.QtyReceived : orderDetails.Quantity;

            if(!overrideHighQty.current && quantity > orderDetails.QtyOrdered) {
                setShowHighQtyModal(true);
                return;
            }

            try {
                setReceivingOrder(true);
                var result = await agent.PurchaseOrder.receiveOrder(
                    region,
                    quantity,
                    orderDetails.Weight,
                    new Date(),
                    false,
                    orderDetails.OrderItemId,
                    orderDetails.Code,
                    0,
                    Config.userId
                );

                if (result && result.status) {
                    if(quantityMode.current === QuantityAddMode.AddTo) {
                        toast.success("Quantity Added");
                    } else if (quantityMode.current === QuantityAddMode.Overwrite) {
                        toast.success("Quantity Overwritten");
                    } else {
                        toast.success("Receive PO Successful");
                    }

                    orderDetails.OrderItems.filter((oi: OrderItem) => oi.OrderItemId === orderDetails.OrderItemId)[0].QtyReceived = quantity;
                    dispatch({type: types.SETORDERDETAILS, orderDetails: {...orderDetails, QtyReceived: quantity, OrderItems: orderDetails.OrderItems}});
                }
            }
            finally {
                setReceivingOrder(false);
                overrideHighQty.current = false;
                quantityMode.current = QuantityAddMode.None;

                dispatch({ type: types.SETPURCHASEORDERUPC, purchaseOrderUpc: '' });
                dispatch({ type: types.SETORDERDETAILS, orderDetails: null });
            }
        }
    }

    const validateDecimalInput = (event: any) => {
        var theEvent = event || window.event;

        // Handle paste
        var key;
        if (theEvent.type === "paste") {
            key = event.clipboardData.getData("text/plain");
        } else {
            // Handle key press
            key = theEvent.keyCode || theEvent.which;
            key = String.fromCharCode(key);
        }
        var regex = /[0-9]|\./;
        if (!regex.test(key)) {
            theEvent.returnValue = false;
            if (theEvent.preventDefault) theEvent.preventDefault();
        }
    };

    const validateIntegerInput = (event: any) => {
        var theEvent = event || window.event;

        // Handle paste
        var key;
        if (theEvent.type === "paste") {
            key = event.clipboardData.getData("text/plain");
        } else {
            // Handle key press
            key = theEvent.keyCode || theEvent.which;
            key = String.fromCharCode(key);
        }
        var regex = /[0-9]/;
        if (!regex.test(key)) {
            theEvent.returnValue = false;
            if (theEvent.preventDefault) theEvent.preventDefault();
        }
    };

    const handleQuantityDecision = async (decision: QuantityAddMode) => {
        setShowQtyModal(false);
        quantityMode.current = decision;

        if(decision === QuantityAddMode.None) {
            toast.info("Quantity not saved");
            dispatch({ type: types.SETPURCHASEORDERUPC, purchaseOrderUpc: '' });
            dispatch({ type: types.SETORDERDETAILS, orderDetails: null });
        } else {
            await receiveOrder();
        }
    }

    const handleHighQtyConfirmClick = async () => {
        overrideHighQty.current = true;
        await receiveOrder();
    }

    if (orderDetails) {
        return ( 
            <Fragment>
                <ConfirmModal handleConfirmClose={handleHighQtyConfirmClick} setOpenExternal={setShowHighQtyModal} showTriggerButton={false} 
                                    openExternal={showHighQtyModal} headerText='Verify Quantity' cancelButtonText='No' confirmButtonText='Yes' 
                                    lineOne={`Quantity Received (${quantityMode.current === QuantityAddMode.AddTo ? orderDetails.Quantity + orderDetails.QtyReceived : orderDetails.Quantity}) is greater than Quantity Ordered (${orderDetails.QtyOrdered}). Continue?`} /> 
                <ReceivePurchaseOrderDetailsQtyModal handleQuantityDecision={handleQuantityDecision} open={showQtyModal}/>
                <ReceivePurchaseOrderReasonCodeModal />
                <Segment
                    disabled={!orderDetails.ItemLoaded}
                    inverted
                    color="teal"
                    textAlign="center"
                    style={{ fontWeight: "bold" }}
                >
                    <Textfit mode='single' min={9} max={14}>
                        {orderDetails.Description}
                    </Textfit>
                </Segment>
                <Grid
                    columns={2}
                    celled
                    style={{ height: "15%", marginBottom: "0px" }}
                >
                    <Grid.Row>
                        <Grid.Column style={{paddingTop: '5px', paddingBottom: '5px'}} width={6} textAlign="right">
                            Package:
                        </Grid.Column>
                        <Grid.Column style={{paddingTop: '5px', paddingBottom: '5px'}} textAlign="left">
                            {orderDetails.PkgWeight} {orderDetails.ItemLoaded ? "/ " : ""}
                            {orderDetails.PkgQuantity} {orderDetails.Uom}
                        </Grid.Column>
                    </Grid.Row>
                    <Grid.Row>
                        <Grid.Column style={{paddingTop: '5px', paddingBottom: '5px'}} width={6} textAlign="right">
                            Subteam:
                        </Grid.Column>
                        <Grid.Column style={{paddingTop: '5px', paddingBottom: '5px'}} textAlign="left">
                            {orderDetails.Subteam
                                ? orderDetails.IsReturnOrder
                                    ? "(C)"
                                    : "(P)"
                                : ""}{" "}
                            {orderDetails.Subteam}
                        </Grid.Column>
                    </Grid.Row>
                    <Grid.Row>
                        <Grid.Column style={{paddingTop: '5px', paddingBottom: '5px'}} width={6} textAlign="right">
                            Vendor:
                        </Grid.Column>
                        <Grid.Column style={{paddingTop: '5px', paddingBottom: '5px'}} textAlign="left">
                            {orderDetails.Vendor}
                        </Grid.Column>
                    </Grid.Row>
                    <Grid.Row>
                        <Grid.Column style={{paddingTop: '5px', paddingBottom: '5px'}} width={6} textAlign="right">
                            Order UOM:
                        </Grid.Column>
                        <Grid.Column style={{paddingTop: '5px', paddingBottom: '5px'}} textAlign="left">
                            {orderDetails.OrderUom}
                        </Grid.Column>
                    </Grid.Row>
                </Grid>

                <Grid columns={2}>
                    <Grid.Column style={{ paddingTop: "0px", paddingBottom: '0px' }}>
                        <Grid columns={2} celled>
                            <Grid.Row>
                                <Grid.Column
                                    style={{paddingTop: '5px', paddingBottom: '5px'}}
                                    verticalAlign="middle"
                                    width={8}
                                    textAlign="right"
                                >
                                    Quantity:
                                </Grid.Column>
                                <Grid.Column textAlign="left" style={{paddingTop: '5px', paddingBottom: '5px'}}>
                                    <Input
                                        type="number"
                                        name="Quantity"
                                        onChange={handleInputChange}
                                        onKeyPress={validateIntegerInput}
                                        onFocus={(event: any) =>
                                            event.target.select()
                                        }
                                        value={orderDetails.ItemLoaded ? orderDetails.Quantity : ''}
                                        fluid
                                        size="small"
                                        disabled={!orderDetails.ItemLoaded}
                                    />
                                </Grid.Column>
                            </Grid.Row>
                            <Grid.Row>
                                <Grid.Column
                                    verticalAlign="middle"
                                    width={8}
                                    textAlign="right"
                                    style={{paddingTop: '5px', paddingBottom: '5px'}}
                                >
                                    Weight:
                                </Grid.Column>
                                <Grid.Column textAlign="left" style={{paddingTop: '5px', paddingBottom: '5px'}}>
                                    <Input
                                        type="number"
                                        name="Weight"
                                        onChange={handleInputChange}
                                        onKeyPress={validateDecimalInput}
                                        onFocus={(event: any) =>
                                            event.target.select()
                                        }
                                        value={orderDetails.Weight}
                                        fluid
                                        disabled={!orderDetails.ItemLoaded}
                                        size="small"
                                    ></Input>
                                </Grid.Column>
                            </Grid.Row>
                            <Grid.Row>
                                <Grid.Column
                                    width={8}
                                    textAlign="right"
                                    verticalAlign="middle"
                                    style={{paddingTop: '5px', paddingBottom: '5px'}}
                                >
                                    <button
                                        type="button"
                                        className="link-button"
                                        onClick={reasonCodeClick}
                                    >
                                        Code:
                                    </button>
                                </Grid.Column>
                                <Grid.Column textAlign="left" style={{padding: '5px'}}>
                                    <Dropdown
                                        style={{paddingRight: '0px', paddingLeft: '0px', verticalAlign: 'middle'}}
                                        fluid
                                        selection
                                        item
                                        options={mappedReasonCodes}
                                        name="Code"
                                        onChange={handleDropdownChange}
                                        disabled={!orderDetails.ItemLoaded}
                                        value={orderDetails.Code}
                                    />
                                </Grid.Column>
                            </Grid.Row>
                        </Grid>
                    </Grid.Column>
                    <Grid.Column stretched style={{ paddingTop: "0px", paddingBottom: '0px' }}>
                        <Grid celled columns={2}>
                            <Grid.Row color="grey">
                                <Grid.Column
                                    verticalAlign="middle"
                                    width={8}
                                    textAlign="right"
                                    style={{paddingTop: '5px', paddingBottom: '5px'}}
                                >
                                    Ordered:
                                </Grid.Column>
                                <Grid.Column
                                    verticalAlign="middle"
                                    textAlign="left"
                                    style={{paddingTop: '5px', paddingBottom: '5px'}}
                                >
                                    {orderDetails.QtyOrdered}
                                </Grid.Column>
                            </Grid.Row>
                            <Grid.Row color="grey">
                                <Grid.Column
                                    verticalAlign="middle"
                                    width={8}
                                    textAlign="right"
                                    style={{paddingTop: '5px', paddingBottom: '5px'}}
                                >
                                    <div style={{ color: "red" }}>
                                        Received:
                                    </div>
                                </Grid.Column>
                                <Grid.Column
                                    verticalAlign="middle"
                                    textAlign="left"
                                    style={{paddingTop: '5px', paddingBottom: '5px'}}
                                >
                                    <div style={{ color: "red" }}>
                                        {orderDetails.QtyReceived}
                                    </div>
                                </Grid.Column>
                            </Grid.Row>
                            <Grid.Row color="grey">
                                <Grid.Column
                                    verticalAlign="middle"
                                    width={8}
                                    textAlign="right"
                                    style={{paddingTop: '5px', paddingBottom: '5px'}}
                                >
                                    Qty:
                                </Grid.Column>
                                <Grid.Column
                                    verticalAlign="middle"
                                    textAlign="left"
                                    style={{paddingTop: '5px', paddingBottom: '5px'}}
                                >
                                    {orderDetails.EInvQty}
                                </Grid.Column>
                            </Grid.Row>
                        </Grid>
                    </Grid.Column>
                </Grid>

                <span style={{ display: "flex", justifyContent: "center", alignItems: "center" }}>
                    <Button style={{backgroundColor: 'transparent'}} disabled={receivingOrder || !orderDetails.ItemLoaded} loading={receivingOrder} as={WfmButton} onClick={receiveOrder}>
                        Receive
                    </Button>
                </span>
            </Fragment>
        );
    }

    return <div />;
};

export default ReceivePurchaseOrderDetails;
