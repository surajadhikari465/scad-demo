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
import ReasonCodeModal from "../../../../../layout/ReasonCodeModal";
import "./styles.scss";
import { toast } from "react-toastify";
import { QuantityAddMode } from "../../types/QuantityAddMode";
import ReceivePurchaseOrderDetailsQtyModal from "./ReceivePurchaseOrderDetailsQtyModal";
import OrderItem from "../../types/OrderItem";
import ConfirmModal from "../../../../../layout/ConfirmModal";
import { Textfit } from 'react-textfit';

interface IProps {
    costedByWeight: boolean;
}

const ReceivePurchaseOrderDetails: React.FC<IProps> = ({ costedByWeight }) => {
    // @ts-ignore
    const { state, dispatch } = useContext(AppContext);
    const { orderDetails, region, mappedReasonCodes, user } = state;
    const [receivingOrder, setReceivingOrder] = useState<boolean>(false);
    const [showQtyModal, setShowQtyModal] = useState<boolean>(false);
    const [showHighQtyModal, setShowHighQtyModal] = useState<boolean>(false);
    const overrideHighQty = useRef(false);
    const quantityMode = useRef(QuantityAddMode.None);
    const [quantity, setQuantity] = useState<any>(1);
    const [weight, setWeight] = useState<number | string>(orderDetails ? quantity * orderDetails?.PkgWeight : '');

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

            mappedReasonCodes.unshift({key:0, text: ' ', value: 0})
            dispatch({
                type: types.SETMAPPEDREASONCODES,
                mappedReasonCodes: mappedReasonCodes
            });
            dispatch({ type: types.SETREASONCODES, reasonCodes: reasonCodes });
        };

        loadReasonCodes();
    }, [dispatch, region]);

    useEffect(() => {
        if (orderDetails?.PkgWeight !== undefined) {
            setWeight(quantity * orderDetails?.PkgWeight);
        }
    }, [orderDetails, quantity, setWeight])


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

            if (parseInt(quantity) === 0) {
                toast.error("Please enter a quantity of at least 1.", { autoClose: false });
                return;
            }

            if (!overrideHighQty.current && orderDetails.QtyReceived !== 0 && quantityMode.current === QuantityAddMode.None) {
                setShowQtyModal(true);
                return;
            }

            const newQuantity: number = quantityMode.current === QuantityAddMode.AddTo ? quantity + orderDetails.QtyReceived : quantity;
            const parsedWeight: number = typeof weight === 'number' ? weight : parseFloat(weight);
            const newWeight: number = quantityMode.current === QuantityAddMode.AddTo ? parsedWeight + orderDetails.Weight : parsedWeight;

            if (!overrideHighQty.current && newQuantity > orderDetails.QtyOrdered) {
                setShowHighQtyModal(true);
                return;
            }

            try {
                setReceivingOrder(true);
                var result = await agent.PurchaseOrder.receiveOrder(
                    region,
                    newQuantity,
                    newWeight,
                    new Date(),
                    false,
                    orderDetails.OrderItemId,
                    orderDetails.Code,
                    0,
                    user!.userId
                );

                if (result && result.status) {
                    if (quantityMode.current === QuantityAddMode.AddTo) {
                        toast.success("Quantity Added");
                    } else if (quantityMode.current === QuantityAddMode.Overwrite) {
                        toast.success("Quantity Overwritten");
                    } else {
                        toast.success("Receive PO Successful");
                    }

                    orderDetails.OrderItems.filter((oi: OrderItem) => oi.OrderItemId === orderDetails.OrderItemId)[0].QtyReceived = newQuantity;
                    dispatch({
                        type: types.SETORDERDETAILS, orderDetails: {
                            ...orderDetails,
                            QtyReceived: undefined,
                            ItemLoaded: false,
                            OrderItems: orderDetails.OrderItems,
                            Description: '',
                            Weight: undefined,
                            Uom: undefined,
                            Code: undefined,
                            QtyOrdered: undefined,
                            EInvQty: undefined,
                            OrderItemId: undefined,
                            PkgWeight: undefined,
                            PkgQuantity: undefined,
                            OrderUom: undefined
                        }
                    });
                }
            }
            finally {
                setQuantity(1);
                setWeight('');
                setReceivingOrder(false);
                overrideHighQty.current = false;
                quantityMode.current = QuantityAddMode.None;

                dispatch({ type: types.SETPURCHASEORDERUPC, purchaseOrderUpc: '' });
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

        if (decision === QuantityAddMode.None) {
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
                <ReceivePurchaseOrderDetailsQtyModal handleQuantityDecision={handleQuantityDecision} open={showQtyModal} quantity={orderDetails.QtyReceived}/>
                <ReasonCodeModal />
                <div className={'receive-order'}>
                    <Segment
                        disabled={!orderDetails.ItemLoaded}
                        inverted
                        color="teal"
                        textAlign="center"
                        style={{ fontWeight: "bold", lineHeight: '0.5'}}
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
                            <Grid.Column style={{ paddingTop: '5px', paddingBottom: '5px' }} width={6} textAlign="right">
                                Package:
                            </Grid.Column>
                            <Grid.Column style={{ paddingTop: '5px', paddingBottom: '5px' }} textAlign="left">
                                {orderDetails.PkgWeight} {orderDetails.ItemLoaded ? "/ " : ""}
                                {orderDetails.PkgQuantity} {orderDetails.Uom}
                            </Grid.Column>
                        </Grid.Row>
                        <Grid.Row>
                            <Grid.Column style={{ paddingTop: '5px', paddingBottom: '5px' }} width={6} textAlign="right">
                                Subteam:
                            </Grid.Column>
                            <Grid.Column style={{ paddingTop: '5px', paddingBottom: '5px' }} textAlign="left">
                                <Textfit mode='single' min={8} max={14}>
                                    {orderDetails.Subteam
                                        ? orderDetails.IsReturnOrder
                                            ? "(C)"
                                            : "(P)"
                                        : ""}{" "}
                                    {orderDetails.Subteam}
                                </Textfit>
                            </Grid.Column>
                        </Grid.Row>
                        <Grid.Row>
                            <Grid.Column style={{ paddingTop: '5px', paddingBottom: '5px' }} width={6} textAlign="right">
                                Vendor:
                            </Grid.Column>
                            <Grid.Column style={{ paddingTop: '5px', paddingBottom: '5px' }} textAlign="left">
                                <Textfit mode='single' min={8} max={14}>
                                    {orderDetails.Vendor}
                                </Textfit>
                            </Grid.Column>
                        </Grid.Row>
                        <Grid.Row>
                            <Grid.Column style={{ paddingTop: '5px', paddingBottom: '5px' }} width={6} textAlign="right">
                                Order UOM:
                            </Grid.Column>
                            <Grid.Column style={{ paddingTop: '5px', paddingBottom: '5px' }} textAlign="left">
                                {orderDetails.OrderUom}
                            </Grid.Column>
                        </Grid.Row>
                    </Grid>
                    <Grid columns={2}>
                        <Grid.Column style={{ paddingTop: "0px", paddingBottom: '0px' }}>
                            <Grid columns={2} celled>
                                <Grid.Row>
                                    <Grid.Column
                                        style={{ paddingTop: '0px', paddingBottom: '5px' }}
                                        verticalAlign="middle"
                                        width={8}
                                        textAlign="right"
                                    >
                                        Quantity:
                                    </Grid.Column>
                                    <Grid.Column textAlign="left" style={{ paddingTop: '5px', paddingBottom: '5px' }}>
                                        <Input
                                            type="number"
                                            name="Quantity"
                                            onChange={(e) => setQuantity(parseInt(e.target.value))}
                                            onKeyPress={validateIntegerInput}
                                            onFocus={(event: any) =>
                                                event.target.select()
                                            }
                                            onKeyDown={(e: any) => e.key === 'Enter' ? e.target.blur() : ''}
                                            value={orderDetails?.ItemLoaded ? quantity : ''}
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
                                        style={{ paddingTop: '5px', paddingBottom: '5px' }}
                                    >
                                        Weight:
                                    </Grid.Column>
                                    <Grid.Column textAlign="left" style={{ paddingTop: '5px', paddingBottom: '5px' }}>
                                        <Input
                                            type="number"
                                            name="Weight"
                                            onChange={(e) => setWeight(parseFloat(e.target.value))}
                                            onKeyPress={validateDecimalInput}
                                            onFocus={(event: any) =>
                                                event.target.select()
                                            }
                                            onKeyDown={(e: any) => e.key === 'Enter' ? e.target.blur() : ''}
                                            value={(orderDetails?.ItemLoaded && costedByWeight) ? weight : ''}
                                            fluid
                                            disabled={!orderDetails.ItemLoaded || !costedByWeight}
                                            size="small"
                                        ></Input>
                                    </Grid.Column>
                                </Grid.Row>
                                <Grid.Row>
                                    <Grid.Column
                                        width={8}
                                        textAlign="right"
                                        verticalAlign="middle"
                                        style={{ paddingTop: '5px', paddingBottom: '5px' }}
                                    >
                                        <button
                                            type="button"
                                            className="link-button"
                                            onClick={reasonCodeClick}
                                        >
                                            Code:
                                        </button>
                                    </Grid.Column>
                                    <Grid.Column textAlign="left" style={{ padding: '5px' }}>
                                        <Dropdown
                                            style={{ paddingRight: '0px', verticalAlign: 'middle', minHeight:'auto', height:'34px' }}
                                            fluid
                                            selection
                                            item
                                            options={mappedReasonCodes}
                                            defaultValue={0}
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
                                <Grid.Row style={{ backgroundColor:'#f0f0f0' }}>
                                    <Grid.Column
                                        verticalAlign="middle"
                                        width={8}
                                        textAlign="right"
                                        style={{ paddingTop: '5px', paddingBottom: '5px' }}
                                    >
                                        Ordered:
                                    </Grid.Column>
                                    <Grid.Column
                                        verticalAlign="middle"
                                        textAlign="left"
                                        style={{ paddingTop: '5px', paddingBottom: '5px' }}
                                    >
                                        {orderDetails.QtyOrdered}
                                    </Grid.Column>
                                </Grid.Row>
                                <Grid.Row style={{ backgroundColor:'#f0f0f0' }}>
                                    <Grid.Column
                                        verticalAlign="middle"
                                        width={8}
                                        textAlign="right"
                                        style={{ paddingTop: '5px', paddingBottom: '5px' }}
                                    >
                                        <div style={{ color: "red" }}>
                                            Received:
                                        </div>
                                    </Grid.Column>
                                    <Grid.Column
                                        verticalAlign="middle"
                                        textAlign="left"
                                        style={{ paddingTop: '5px', paddingBottom: '5px' }}
                                    >
                                        <div style={{ color: "red" }}>
                                            {orderDetails.QtyReceived}
                                        </div>
                                    </Grid.Column>
                                </Grid.Row>
                                <Grid.Row style={{ backgroundColor:'#f0f0f0' }}>
                                    <Grid.Column
                                        verticalAlign="middle"
                                        width={8}
                                        textAlign="right"
                                        style={{ paddingTop: '5px', paddingBottom: '5px' }}
                                    >
                                        Qty:
                                    </Grid.Column>
                                    <Grid.Column
                                        verticalAlign="middle"
                                        textAlign="left"
                                        style={{ paddingTop: '5px', paddingBottom: '5px' }}
                                    >
                                        {orderDetails.EInvQty}
                                    </Grid.Column>
                                </Grid.Row>
                            </Grid>
                        </Grid.Column>
                    </Grid>

                    <span style={{ display: "flex", justifyContent: "center", alignItems: "center" }}>
                        <Button className='wfmButton' style={{ width: '100%',backgroundColor: 'transparent'}} disabled={receivingOrder || !orderDetails.ItemLoaded} loading={receivingOrder} onClick={receiveOrder}>
                            Receive
                        </Button>
                    </span>
                </div>
            </Fragment>
        );
    }

    return <div />;
};

export default ReceivePurchaseOrderDetails;
