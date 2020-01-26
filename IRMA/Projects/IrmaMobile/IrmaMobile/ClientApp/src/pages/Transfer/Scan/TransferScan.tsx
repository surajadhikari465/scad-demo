import React, { Fragment, useState, useEffect, useContext, useCallback } from 'react'
import { AppContext, types, IMenuItem } from '../../../store'
import LoadingComponent from '../../../layout/LoadingComponent';
import { Grid, Input, Button, Segment, InputOnChangeData, Dropdown, DropdownProps } from 'semantic-ui-react';
import './styles.scss';
// @ts-ignore 
import { BarcodeScanner } from '@wfm/mobile';
import ITransferData from '../types/ITransferData';
import agent from '../../../api/agent';
import ITransferItem from '../types/ITransferItem';
import { toast } from 'react-toastify';
import ReasonCodeModal from '../../../layout/ReasonCodeModal';
import { ReasonCode } from '../../Receive/PurchaseOrder/types/ReasonCode';

const TransferScan: React.FC = () => {
    //@ts-ignore
    const { state, dispatch } = useContext(AppContext);
    const { region, mappedReasonCodes } = state;

    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [quantity, setQuantity] = useState(1);
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
    const [reasonCode, setReasonCode] = useState(-1);

    useEffect(() => {
        dispatch({ type: types.SETTITLE, Title: 'Transfer Scan' });
        return () => {
          dispatch({ type: types.SETTITLE, Title: 'IRMA Mobile' });
        };
      }, [dispatch]);

    const setMenuItems = useCallback(() => {
        const newMenuItems = [
            { id: 1, order: 0, text: "Delete Order", path: "/transfer/scan/delete", disabled: true } as IMenuItem,
            { id: 2, order: 1, text: "Save Order", path: `/transfer/scan/save`, disabled: true } as IMenuItem,
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
            if(transferData) {
                const filteredItems = transferData.Items.filter((item: ITransferItem) => item.Upc === upc);
                let loadingItem = undefined;

                if(filteredItems.length === 0) {
                    const itemRaw = await agent.Transfer.getTransferItem(region, upc, transferData.ProductType, transferData.FromStoreNo, transferData.FromStoreVendorId, transferData.FromSubteamNo, transferData.SupplyType); 
                    
                    if(!itemRaw) {
                        toast.error('Error loading item. Please try again.', { autoClose: false });
                        setUpc('');
                        return;
                    }
                    
                    loadingItem = {
                        AdjustedCost: 0, //don't know what this is
                        AdjustedReason: '', //don't know what this is
                        Description: itemRaw.itemDescription,
                        ItemKey: itemRaw.itemKey,
                        Quantity: 1,
                        RetailUom: itemRaw.retailUnitName,
                        RetailUomId: itemRaw.retailUnitId,
                        SoldByWeight: itemRaw.soldByWeight,
                        TotalCost: 0, //don't know what this is
                        Upc: upc,
                        VendorCost: itemRaw.vendorCost,
                        VendorPack: itemRaw.vendorPack
                    } as ITransferItem;

                    setQueuedQuantity(0);
                } else {
                    loadingItem = filteredItems[0];
                    
                    setQueuedQuantity(loadingItem.Quantity);
                }

                setItem(loadingItem);

                setDescription(loadingItem.Description);
                
                setQuantity(1);

                setVendorPack(loadingItem.VendorPack);
                setRetail(loadingItem.RetailUom)
                
                if(loadingItem.VendorCost === 0) {
                    toast.info("The cost for this item is 0.00, or it could not be determined. If this is incorrect, please enter the Adjusted Cost for the item.", { autoClose: false });
                    setVendorCost('0');
                    setCostLabel('Adjusted Cost:');
                } else {
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
        BarcodeScanner.registerHandler((data: any) => {
            if(upc === data) {
                setQuantity((q: any) => q + 1);
                return;
            } else {
                setUpc(data);
                setSearchFlag(true);
            }
          });

        return () => {
            BarcodeScanner.scanHandler = () => {};
        }      
    });

    useEffect(() => {
        if(searchFlag) {
            upcSearch();
            setSearchFlag(false);
        }
    }, [searchFlag, upcSearch])

    useEffect(() => {
        const rawData = localStorage.getItem("transferData");
        if(rawData !== null) {
            setTransferData(JSON.parse(rawData) as ITransferData);
        }
    }, [setTransferData]); 

    const handleQuantityChange = (event: React.ChangeEvent<HTMLInputElement>, { value }: InputOnChangeData) => {
        setQuantity(parseInt(value));
    }

    const handleAdjustedCostChange = (event: React.ChangeEvent<HTMLInputElement>, { value }: InputOnChangeData) => {
        setAdjustedCost(parseInt(value));
    }

    const handleUpcChange = (event: React.ChangeEvent<HTMLInputElement>, { value }: InputOnChangeData) => {
        setUpc(value);
    }

    const upcSearchClick = async () => {
        await upcSearch();
    }

    const handleSave = () => {
        if(!transferData || !item) {
            toast.error('No data to save.', { autoClose: false });
            return; 
        }

        const filteredItems = transferData.Items.filter((item: ITransferItem) => item.Upc === upc);
        if(filteredItems.length === 0){
            transferData.Items.push(item);
        } else {
            transferData.Items = [...transferData.Items.filter((item: ITransferItem) => item.Upc !== upc), { ...filteredItems[0], Quantity: quantity }];
            alert("I need add to/overwrite/cancel logic");
        }

        localStorage.setItem("transferData", JSON.stringify(transferData));

        toast.info(`Transfer for UPC ${upc} saved successfully with quantity of ${quantity}.`);
        clearScreen();
    }

    const clearScreen = () => {
        setUpc('');
        setDescription('');
        setQuantity(1);
        setVendorPack('');
        setRetail('');
        setVendorCost('');
        setCostLabel('Vendor Cost:');
        setQueuedQuantity('');
        setItem(undefined);
    }

    const reasonCodeClick = () => {
        dispatch({
            type: types.SETMODALOPEN,
            modalOpen: true
        });
    }

    const handleReasonCodeChange = (event: React.SyntheticEvent<HTMLElement, Event>, data: DropdownProps) => {
        if(data && data.value) {
            setReasonCode(parseInt(data.value.toString()));
        }
    }

    const handleReviewClick = () => 
    {
        alert("This opens the review screen. I need logic");
        //history.push('/transfer/review');
    }

    return (
        <Fragment>
            {isLoading ? <LoadingComponent content='Loading item...' />
            :
            <Fragment>
                <ReasonCodeModal />
                <Grid style={{marginTop: '5px'}}>
                    <Grid.Row>
                        <Grid.Column width={4} verticalAlign='middle' textAlign='right'>
                            UPC:
                        </Grid.Column>
                        <Grid.Column width={8} style={{padding: '0px'}}>
                            <Input onFocus={(event: any) => event.target.select()} type='number' placeholder='UPC' value={upc || ''} onChange={handleUpcChange} fluid/>
                        </Grid.Column>
                        <Grid.Column width={2}>
                            <Button className='wfmButton' onClick={upcSearchClick}>>></Button>
                        </Grid.Column>
                    </Grid.Row>
                    <Grid.Row>
                        <Grid.Column verticalAlign='middle' textAlign='right' width={4}>
                            Desc:
                        </Grid.Column>
                        <Grid.Column width={12} style={{padding: '0px'}}>
                            {description}
                        </Grid.Column>
                    </Grid.Row>
                    <Grid.Row>
                        <Grid.Column verticalAlign='middle' textAlign='right' width={4}>
                            Quantity:
                        </Grid.Column>
                        <Grid.Column width={3} style={{padding: '0px'}}>
                            <Input onFocus={(event: any) => event.target.select()} type='number' placeholder='Qty' value={quantity} onChange={handleQuantityChange} fluid/>
                        </Grid.Column>
                        <Grid.Column width={2} verticalAlign='middle' textAlign='right'>
                            Retail:
                        </Grid.Column>
                        <Grid.Column width={2} verticalAlign='middle' textAlign='left' style={{fontWeight: 'bold'}}>
                            {retail}
                        </Grid.Column>
                    </Grid.Row>
                    <Grid.Row>
                        <Grid.Column verticalAlign='middle' textAlign='right' width={4}>
                            Vendor Pack:
                        </Grid.Column>
                        <Grid.Column width={10} verticalAlign='middle' textAlign='left' style={{fontWeight: 'bold', padding: '0px'}}>
                            {vendorPack}
                        </Grid.Column>
                    </Grid.Row>
                    <Grid.Row>
                        <Grid.Column verticalAlign='middle' textAlign='right' width={4}>
                            {costLabel}
                        </Grid.Column>
                        <Grid.Column width={4} verticalAlign='middle' textAlign='left' style={{fontWeight: 'bold', paddingLeft: '0px'}}>
                            {parseFloat(vendorCost) === 0 ? 
                                <Input onFocus={(event: any) => event.target.select()} fluid value={adjustedCost} onChange={handleAdjustedCostChange} placeholder='Adjusted Cost'/>
                                :
                                vendorCost
                            }
                        </Grid.Column>
                        {
                            parseFloat(vendorCost) === 0 ? 
                            <Fragment>
                                <Grid.Column width={2} verticalAlign='middle'>
                                    <button
                                            type="button"
                                            className="link-button"
                                            onClick={reasonCodeClick}
                                        >
                                            Reason:
                                        </button>
                                </Grid.Column>
                                <Grid.Column textAlign="left" style={{padding: '5px'}} width={4}>
                                    <Dropdown
                                        style={{paddingRight: '0px', marginLeft: '25px', paddingLeft: '0px', verticalAlign: 'middle'}}
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
                            : <div/>
                        }
                    </Grid.Row>
                    <Grid.Row>
                        <Grid.Column width={8} verticalAlign='middle' textAlign='left' style={{fontWeight: 'bold'}}>
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
                    <Button className='wfmButton' style={{width:'100%', marginTop: '50px'}} onClick={handleReviewClick}>Review</Button>
                </span>
            </Fragment>
            }
        </Fragment>
    )
}

export default TransferScan;