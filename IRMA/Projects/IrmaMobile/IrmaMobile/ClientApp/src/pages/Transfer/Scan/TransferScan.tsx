import React, { Fragment, useState, useEffect, useContext, useCallback } from 'react'
import { AppContext, types, IMenuItem } from '../../../store'
import LoadingComponent from '../../../layout/LoadingComponent';
import { Grid, Input, Button, Segment, InputOnChangeData, Dropdown, DropdownProps } from 'semantic-ui-react';
import './styles.scss';
// @ts-ignore 
import { BarcodeScanner, IBarcodeScannedEvent } from '@wfm/mobile';
import ITransferData from '../types/ITransferData';
import agent from '../../../api/agent';
import ITransferItem from '../types/ITransferItem';
import { toast } from 'react-toastify';
import ReasonCodeModal from '../../../layout/ReasonCodeModal';
import { ReasonCode } from '../../Receive/PurchaseOrder/types/ReasonCode';
import BasicModal from '../../../layout/BasicModal';
import { useHistory } from 'react-router-dom';

const TransferScan: React.FC = () => {
    //@ts-ignore
    const { state, dispatch } = useContext(AppContext);
    const { region, mappedReasonCodes } = state;

    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [quantity, setQuantity] = useState<string>('1');
    const [upc, setUpc] = useState();
    const [queuedQuantity, setQueuedQuantity] = useState();
    const [costLabel, setCostLabel] = useState('Vendor Cost:');
    const [transferData, setTransferData] = useState<ITransferData>();
    const [item, setItem] = useState<ITransferItem>();
    const [searchFlag, setSearchFlag] = useState<boolean>(false);

    const [description, setDescription] = useState();
    const [retail, setRetail] = useState();
    const [vendorPack, setVendorPack] = useState();
    const [vendorCost, setVendorCost] = useState();
    const [adjustedCost, setAdjustedCost] = useState<number>(0);
    const [reasonCode, setReasonCode] = useState(0);
    const [alert, setAlert] = useState<any>({ open: false, alertMessage: '', type: 'default', header: 'IRMA Mobile', confirmAction: () => { }, cancelAction: () => { } });
    let history = useHistory();

    useEffect(() => {
        dispatch({ type: types.SETTITLE, Title: 'Transfer Scan' });
        return () => {
            dispatch({ type: types.SETTITLE, Title: 'IRMA Mobile' });
        };
    }, [dispatch]);

    const setMenuItems = useCallback(() => {
        const newMenuItems = [
            { id: 1, order: 0, text: "Delete Order", path: "/transfer/scan/deleteOrder", disabled: false } as IMenuItem,
            { id: 2, order: 1, text: "Save Order", path: `/transfer/scan/saveOrder`, disabled: false } as IMenuItem,
            { id: 3, order: 2, text: "Back", path: `/transfer/index/true`, disabled: false } as IMenuItem,
        ] as IMenuItem[];

        dispatch({ type: types.SETMENUITEMS, menuItems: newMenuItems });
    }, [dispatch]);

    useEffect(() => {
        setMenuItems()

        return () => {
            dispatch({ type: types.SETMENUITEMS, menuItems: [] });
        }
    }, [setMenuItems, dispatch]);

    useEffect(() => {
        const loadReasonCodes = async () => {
            const reasonCodes: ReasonCode[] = await agent.Transfer.getReasonCodes(region);

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

    const upcSearch = useCallback(async () => {
        setIsLoading(true);

        try {
            if (transferData) {
                const filteredItems = transferData.Items.filter((item: ITransferItem) => item.Upc === upc);
                let loadingItem = undefined;

                const itemRaw = await agent.Transfer.getTransferItem(region, upc, transferData.ProductType, transferData.FromStoreNo, transferData.FromStoreVendorId, transferData.FromSubteamNo, transferData.SupplyType);

                if (!itemRaw || itemRaw.itemKey <= 0) {
                    toast.error('Item not found.');
                    setUpc('');
                    clearScreen();
                    return;
                }
                if (itemRaw.GLAcct === 0 && transferData.ProductType !== 1) {
                    if (transferData.ProductType === 2) {
                        toast.error(`This item cannot be transferred because it's from subteam ${itemRaw.RetailSubteamName}, which doesn't have a GL Packaging Account set up yet.`, { autoClose: false });
                    } else {
                        toast.error(`This item cannot be transferred because it's from subteam ${itemRaw.RetailSubteamName}, which doesn't have a GL Supplies Account set up yet.`, { autoClose: false });
                    }
                    setUpc('');
                    return;
                }

                loadingItem = {
                    AdjustedCost: itemRaw.adjustedCost,
                    AdjustedReason: -1,
                    Description: itemRaw.itemDescription,
                    ItemKey: itemRaw.itemKey,
                    Quantity: 1,
                    RetailUom: itemRaw.retailUnitName,
                    RetailUomId: itemRaw.retailUnitId,
                    SoldByWeight: itemRaw.soldByWeight,
                    TotalCost: 0,
                    Upc: upc,
                    VendorCost: itemRaw.vendorCost,
                    VendorPack: itemRaw.vendorPack
                } as ITransferItem;

                if (!transferData.FromStoreVendorId || transferData.FromStoreVendorId === 0) {
                    transferData.FromStoreVendorId = itemRaw.vendorID;
                    localStorage.setItem('transferData', JSON.stringify(transferData));
                    setTransferData({ ...transferData, FromStoreVendorId: itemRaw.vendorID });
                }
                if ((!transferData.SupplyType || transferData.SupplyType === 0) && transferData.ProductType !== 1) {
                    transferData.SupplyType = itemRaw.retailSubteamNo;
                    localStorage.setItem('transferData', JSON.stringify(transferData));
                    setTransferData({ ...transferData, SupplyType: itemRaw.retailSubteamNo });
                }

                if (filteredItems.length === 0) {
                    setQueuedQuantity(0);
                } else {
                    setQueuedQuantity(filteredItems[0].Quantity);
                }

                setItem(loadingItem);

                setDescription(loadingItem.Description);

                setQuantity('1');

                setVendorPack(loadingItem.VendorPack);
                setRetail(loadingItem.RetailUom);
                setAdjustedCost(loadingItem.AdjustedCost);

                if ((loadingItem.VendorCost === 0) && (loadingItem.AdjustedCost === 0)) {
                    toast.info("The cost for this item is 0.00, or it could not be determined. If this is incorrect, please enter the Adjusted Cost for the item.", { autoClose: false });
                    setVendorCost('0');
                    setCostLabel('Adjusted Cost:');
                } else if (loadingItem.VendorCost === 0) {
                    setVendorCost('0');
                    setCostLabel('Adjusted Cost:');
                }
                else {
                    setVendorCost(loadingItem.VendorCost);
                    setCostLabel('Vendor Cost:');
                }
            }
        }
        finally {
            setIsLoading(false);
        }
    }, [region, transferData, upc]);

    useEffect(() => {
        BarcodeScanner.registerHandler((data: IBarcodeScannedEvent) => {
            let scannedUpc = parseInt(upc.Data).toString();
            if (upc === scannedUpc) {
                setQuantity((q: any) => q + 1);
                return;
            } else {
                setUpc(scannedUpc);
                setSearchFlag(true);
            }
        });

        return () => {
            BarcodeScanner.scanHandler = () => { };
        }
    });

    useEffect(() => {
        if (searchFlag) {
            upcSearch();
            setSearchFlag(false);
        }
    }, [searchFlag, upcSearch])

    useEffect(() => {
        const rawData = localStorage.getItem("transferData");
        if (rawData !== null) {
            setTransferData(JSON.parse(rawData) as ITransferData);
        }
    }, [setTransferData]);

    const handleQuantityOnKeyPress = (e: any) => {

        if (item) {
            if (quantity !== null && quantity !== undefined) {
                if (item.SoldByWeight) {
                    const floatRegEx = /^(\d{0,4}(\.\d{0,2})?|\.?\d{1,2})$/;
                    if (!floatRegEx.test(quantity + e.key)) {
                        e.preventDefault();
                    }
                } else {
                    const intRegEx = /^\d{0,3}$/

                    if (!intRegEx.test(quantity + e.key)) {
                        e.preventDefault();
                    }
                }
            }
        }
    }

    const clearInvalid = (e: any) => {
        //android behavior fix, to stop invalid punctuation 
        if (e.target.value === '') {
            e.target.value = '';
        }
    }

    const handleQuantityChange = (event: React.ChangeEvent<HTMLInputElement>, { value }: InputOnChangeData) => {
        let quantity = parseFloat(value);
        if (!item?.SoldByWeight)
            setQuantity(parseInt(value.replace(/[^\w\s]|_/g, "")).toString());
        else
            setQuantity(quantity.toString());
    }

    const handleAdjustedCostChange = (event: React.ChangeEvent<HTMLInputElement>, { value }: InputOnChangeData) => {
        setAdjustedCost(parseFloat(value));
    }

    const handleUpcChange = (event: React.ChangeEvent<HTMLInputElement>, { value }: InputOnChangeData) => {
        setUpc(value);
    }

    const upcSearchClick = async () => {
        if (upc !== null && upc !== '') {
            await upcSearch();
        }
    }

    const handleSave = () => {
        if (!transferData || !item) {
            toast.error('No data to save.');
            return;
        }

        const quantityNumber = item.SoldByWeight ? parseFloat(quantity) : parseInt(quantity);
        if (isNaN(quantityNumber) || quantityNumber <= 0) {
            setAlert({
                ...alert,
                open: true,
                alertMessage: `Quantity must be a valid number.`,
                type: 'default',
                header: 'Invalid Quantity'
            });
        } else if (quantityNumber > 999.99) {
            setAlert({
                ...alert,
                open: true,
                alertMessage: `Quantity must be less than 1000.`,
                type: 'default',
                header: 'Invalid Quantity'
            });
        } else if (vendorCost === '0' && (isNaN(adjustedCost) || adjustedCost <= 0)) {
            setAlert({
                ...alert,
                open: true,
                alertMessage: `An adjusted cost needs to be entered with a reason.  Please select a valid number.`,
                type: 'default',
                header: 'Missing Adjusted Cost'
            });
        } else if (vendorCost === '0' && (isNaN(reasonCode) || reasonCode <= 0)) {
            setAlert({
                ...alert,
                open: true,
                alertMessage: `An adjusted cost needs to be entered with a reason.  Please select a reason code.`,
                type: 'default',
                header: 'Missing Adjusted Cost Reason'
            });
        } else {
            const filteredItems = transferData.Items.filter((i: ITransferItem) => i.Upc === item.Upc);
            if (filteredItems.length === 0) {
                item.TotalCost = item.VendorCost !== 0 ? quantityNumber * item.VendorCost : quantityNumber * adjustedCost;
                item.AdjustedCost = adjustedCost;
                item.AdjustedReason = reasonCode;
                item.Quantity = quantityNumber;

                transferData.Items.push(item);

                localStorage.setItem("transferData", JSON.stringify(transferData));

                clearScreen();

                toast.info(`Transfer for UPC ${item.Upc} saved successfully with quantity of ${quantity}.`);
            } else {
                setAlert({
                    ...alert,
                    open: true,
                    alertMessage: `${queuedQuantity} of this item already queued in this session. Tap Add, Overwrite or Cancel`,
                    type: 'prevScanned',
                    header: 'Previously Scanned Item',
                    cancelAction: cancel.bind(undefined, true)
                });
            }
        }
    }

    const clearScreen = () => {
        setUpc('');
        setDescription('');
        setQuantity('1');
        setVendorPack('');
        setRetail('');
        setVendorCost('');
        setCostLabel('Vendor Cost:');
        setQueuedQuantity('');
        setAdjustedCost(0);
        setReasonCode(0);
        setItem(undefined);
    }

    const reasonCodeClick = () => {
        dispatch({
            type: types.SETMODALOPEN,
            modalOpen: true
        });
    }

    const handleReasonCodeChange = (event: React.SyntheticEvent<HTMLElement, Event>, data: DropdownProps) => {
        if (data && data.value) {
            setReasonCode(parseInt(data.value.toString()));
        }
    }

    const handleReviewClick = () => {
        history.push('/transfer/review');
    }

    const add = () => {
        setAlert({ ...alert, open: false });
        if (transferData && item) {
            const quantityNumber = item.SoldByWeight ? parseFloat(quantity) : parseInt(quantity);
            const filteredItems = transferData.Items.filter((i: ITransferItem) => i.Upc === item.Upc);
            filteredItems[0].Quantity += quantityNumber;
            filteredItems[0].TotalCost = item.VendorCost !== 0 ? filteredItems[0].Quantity * item.VendorCost : filteredItems[0].Quantity * adjustedCost;
            filteredItems[0].AdjustedCost = adjustedCost;
            filteredItems[0].AdjustedReason = reasonCode;
            localStorage.setItem("transferData", JSON.stringify(transferData));
            toast.info(`Transfer for UPC ${item.Upc} saved successfully with quantity of ${filteredItems[0].Quantity}.`);
            clearScreen();
        }
    }

    const overwrite = () => {
        setAlert({ ...alert, open: false });
        if (transferData && item) {
            const quantityNumber = item.SoldByWeight ? parseFloat(quantity) : parseInt(quantity);
            const filteredItems = transferData.Items.filter((i: ITransferItem) => i.Upc === item.Upc);
            filteredItems[0].Quantity = quantityNumber;
            filteredItems[0].TotalCost = item.VendorCost !== 0 ? quantityNumber * item.VendorCost : quantityNumber * adjustedCost;
            filteredItems[0].AdjustedCost = adjustedCost;
            filteredItems[0].AdjustedReason = reasonCode;
            localStorage.setItem("transferData", JSON.stringify(transferData));
            toast.info(`Transfer for UPC ${item.Upc} saved successfully with quantity of ${filteredItems[0].Quantity}.`);
            clearScreen();
        }
    }

    const cancel = () => {
        setAlert({ ...alert, open: false });

        clearScreen();
    }


    return (
        <Fragment>
            {isLoading ? <LoadingComponent content='Loading item...' />
                :
                <Fragment>
                    <ReasonCodeModal />
                    <Grid style={{ marginTop: '5px' }}>
                        <Grid.Row>
                            <Grid.Column width={4} verticalAlign='middle' textAlign='right'>
                                UPC:
                        </Grid.Column>
                            <Grid.Column width={8} style={{ padding: '0px' }}>
                                <Input type='number' placeholder='UPC' value={upc || ''} onChange={handleUpcChange} onKeyDown={(e: any) => e.key === 'Enter' ? upcSearchClick() : ''} fluid />
                            </Grid.Column>
                            <Grid.Column width={2}>
                                <Button className='wfmButton' onClick={upcSearchClick}>>></Button>
                            </Grid.Column>
                        </Grid.Row>
                        <Grid.Row>
                            <Grid.Column verticalAlign='middle' textAlign='right' width={4}>
                                Desc:
                        </Grid.Column>
                            <Grid.Column width={12} style={{ padding: '0px' }}>
                                {description}
                            </Grid.Column>
                        </Grid.Row>
                        <Grid.Row>
                            <Grid.Column verticalAlign='middle' textAlign='right' width={4}>
                                Quantity:
                        </Grid.Column>
                            <Grid.Column width={3} style={{ padding: '0px' }}>
                                <Input type='number'
                                    placeholder='Qty'
                                    value={quantity}
                                    onChange={handleQuantityChange}
                                    onKeyPress={clearInvalid}
                                    onKeyDown={(e: any) => e.key === 'Enter' ? e.target.blur() : ''}
                                    fluid />
                            </Grid.Column>
                            <Grid.Column width={2} verticalAlign='middle' textAlign='right'>
                                Retail:
                        </Grid.Column>
                            <Grid.Column width={2} verticalAlign='middle' textAlign='left' style={{ fontWeight: 'bold' }}>
                                {retail}
                            </Grid.Column>
                        </Grid.Row>
                        <Grid.Row>
                            <Grid.Column verticalAlign='middle' textAlign='right' width={4}>
                                Vendor Pack:
                        </Grid.Column>
                            <Grid.Column width={10} verticalAlign='middle' textAlign='left' style={{ fontWeight: 'bold', padding: '0px' }}>
                                {vendorPack}
                            </Grid.Column>
                        </Grid.Row>
                        <Grid.Row>
                            <Grid.Column verticalAlign='middle' textAlign='right' width={4}>
                                {costLabel}
                            </Grid.Column>
                            <Grid.Column width={4} verticalAlign='middle' textAlign='left' style={{ fontWeight: 'bold', paddingLeft: '0px' }}>
                                {parseFloat(vendorCost) === 0 ?
                                    <Input type='number' fluid value={adjustedCost} onChange={handleAdjustedCostChange} />
                                    :
                                    vendorCost
                                }
                            </Grid.Column>
                            {
                                parseFloat(vendorCost) === 0 ?
                                    <Fragment>
                                        <Grid.Column width={2} verticalAlign='middle'>
                                            <button type="button" className="link-button" onClick={reasonCodeClick}>Reason:</button>
                                        </Grid.Column>
                                        <Grid.Column textAlign="left" style={{ padding: '5px' }} width={4}>
                                            <Dropdown
                                                style={{ paddingRight: '0px', marginLeft: '25px', paddingLeft: '0px', verticalAlign: 'middle' }}
                                                fluid
                                                selection
                                                item
                                                options={mappedReasonCodes}
                                                name="Code"
                                                onChange={handleReasonCodeChange}
                                                value={reasonCode}
                                            />
                                        </Grid.Column>
                                    </Fragment>
                                    : <div />
                            }
                        </Grid.Row>
                        <Grid.Row>
                            <Grid.Column width={8} verticalAlign='middle' textAlign='left' style={{ fontWeight: 'bold' }}>
                                <Segment color='grey' inverted>Queued: {queuedQuantity}</Segment>
                            </Grid.Column>
                        </Grid.Row>
                        <Grid.Row>
                            <Grid.Column textAlign='center' width={8}>
                                <Button className='wfmButton' fluid onClick={clearScreen}>Clear</Button>
                            </Grid.Column>
                            <Grid.Column textAlign='center' width={8}>
                                <Button className='wfmButton' fluid onClick={handleSave}>Save</Button>
                            </Grid.Column>
                        </Grid.Row>
                    </Grid>
                    <span style={{ display: "flex", justifyContent: "center", alignItems: "center" }}>
                        <Button className='wfmButton' style={{ width: '100%', marginTop: '50px' }} onClick={handleReviewClick}>Review</Button>
                    </span>
                </Fragment>
            }
            <BasicModal alert={alert} add={add} overwrite={overwrite} setAlert={setAlert} />
        </Fragment>
    )
}

export default TransferScan;