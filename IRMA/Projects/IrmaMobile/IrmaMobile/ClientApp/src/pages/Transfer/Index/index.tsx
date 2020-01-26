import React, { Fragment, useState, useContext, useEffect, useCallback } from 'react'
import { AppContext, ITeam, IStore, types, IMenuItem } from "../../../store";
import { Header, Segment, Dropdown, Form, InputOnChangeData, DropdownProps } from 'semantic-ui-react'
import { WfmButton } from '@wfm/ui-react'
import dateFormat from 'dateformat'
import { toast } from 'react-toastify';
import agent from '../../../api/agent';
import ITransferData from '../types/ITransferData';
import Config from '../../../config';
import ConfirmModal from '../../../layout/ConfirmModal';
import { useHistory, RouteComponentProps } from 'react-router-dom';

interface RouteParams {
    comingFromScan: string;
}

interface IProps extends RouteComponentProps<RouteParams> {}

const TransferHome: React.FC<IProps> = ({ match }) => {
    //@ts-ignore
    const { state, dispatch } = useContext(AppContext);
    const { subteams, stores, region, subteamNo, storeNumber } = state;
    const { comingFromScan } = match.params;

    const [subteamsMapped, setSubteamsMapped] = useState<any>([]);
    const [storesMapped, setStoresMapped] = useState<any>([]);
    const [supplyTypesMapped, setSupplyTypesMapped] = useState<any>([]);
    const [showSaved, setShowSaved] = useState<boolean>(false);
    const [showConfirmDelete, setShowConfirmDelete] = useState<boolean>(false);
    const [savedTransferData, setSavedTransferData] = useState<ITransferData>();

    const [fromStore, setFromStore] = useState<number>(parseInt(storeNumber));
    const [fromSubteam, setFromSubteam] = useState<number>(parseInt(subteamNo));
    const [toStore, setToStore] = useState<number>(parseInt(storeNumber));
    const [toSubteam, setToSubteam] = useState<number>(parseInt(subteamNo));
    const [productType, setProductType] = useState<number>(-1);
    const [supplyType, setSupplyType] = useState<number>(-1);
    const [expectedDate, setExpectedDate] = useState<string>(dateFormat(new Date(), "yyyy-mm-dd"));

    let history = useHistory();

    useEffect(() => {
        dispatch({ type: types.SETTITLE, Title: 'Transfer Main' });
        return () => {
          dispatch({ type: types.SETTITLE, Title: 'IRMA Mobile' });
        };
      }, [dispatch]);

    const setMenuItems = useCallback(() => {
        const newMenuItems = [
            { id: 1, order: 0, text: "New Order", path: "/transfer/clearscreen", disabled: true } as IMenuItem, //Create a new page that simply deletes "transferScan" from localstorage and goes back in history. Follow ReceivePurchaseOrderDetailsClearScreen's lead
            { id: 2, order: 1, text: "Exit Transfer", path: `/functions`, disabled: false } as IMenuItem,
         ] as IMenuItem[];

        dispatch({ type: types.SETMENUITEMS, menuItems: newMenuItems });
    }, [dispatch]);

    useEffect(() => {
        setMenuItems()

        return () => {
            dispatch({ type: types.SETMENUITEMS, menuItems: [] });
        }
    }, [setMenuItems, dispatch]);

    const productTypesMapped = [ 
        { key: 1, value: 1, text: 'Product' },
        { key: 2, value: 2, text: 'Packaging Supplies' },
        { key: 3, value: 3, text: 'Other Supplies' },
    ]

    const goToReview = useCallback(() => {
            alert("I should be going to the review screen. Instead, I will be going to Transfer Scan");
            history.push('/transfer/scan');
            // history.push('/transfer/review');
    }, [history]);

    const loadSavedSession = useCallback(() => {
        if(savedTransferData){
            setFromStore(savedTransferData.FromStoreNo);
            setFromSubteam(savedTransferData.FromSubteamNo);

            setToStore(savedTransferData.ToStoreNo);
            setToSubteam(savedTransferData.ToSubteamNo);

            setProductType(savedTransferData.ProductType);
            setSupplyType(savedTransferData.SupplyType);
            setExpectedDate(dateFormat(savedTransferData.ExpectedDate, 'UTC:yyyy-mm-dd'));
        }
    }, [savedTransferData])

    useEffect(() => {
        const storageData = localStorage.getItem('transferData');

        if(storageData !== null) {
            const transferData: ITransferData = JSON.parse(storageData);
            if(transferData) {
                setSavedTransferData(transferData);
                if(!comingFromScan) {
                    setShowSaved(true);
                }
            }
        }
    }, [setSavedTransferData, setShowSaved, goToReview, comingFromScan])

    useEffect(() => {
        if(comingFromScan) {
            loadSavedSession();
        }
    }, [loadSavedSession, comingFromScan])

    useEffect(() => {
        dispatch({ type: types.SETTITLE, Title: 'Transfer Main' });
        return () => {
          dispatch({ type: types.SETTITLE, Title: 'IRMA Mobile' });
        };
      }, [dispatch]);

    useEffect(() => {
        if(subteams) {
            setSubteamsMapped(subteams.map((subteam: ITeam) => { return { key: subteam.subteamNo, value: subteam.subteamNo, text: subteam.subteamName }; } ));
        }
    }, [subteams]);

    useEffect(() => {
        const loadSupplyTypes = async () => {
            const supplyTypesRaw = await agent.Transfer.getSubteamByProductType(region, 3)
            
            if(supplyTypesRaw) {
                setSupplyTypesMapped(supplyTypesRaw.map((type: any) => { return { key: type.subteamNo, value: type.subteamNo, text: type.subteamName } }))
            }
        }

        loadSupplyTypes();
    }, [setSupplyTypesMapped, region])

    useEffect(() => {
        if(stores) {
            setStoresMapped(stores.map((store: IStore) => { return { key: store.storeNo, value: store.storeNo, text: store.name }; }))
        }
    }, [stores])

    const handleCreatePoClick = () => {
        if(fromStore === -1) {
            toast.error('Please set From Store', { autoClose: false });
            return;
        } else if(fromSubteam === -1) {
            toast.error('Please set From Subteam', { autoClose: false });
            return;
        } else if(toStore === -1) {
            toast.error('Please set To Store', { autoClose: false });
            return;
        } else if(toSubteam === -1) {
            toast.error('Please set To Subteam', { autoClose: false });
            return;
        } else if(productType === -1) {
            toast.error('Please set Product Type', { autoClose: false });
            return;
        } else if (productType === 3 && supplyType === -1) {
            toast.error('Please set Supply Type', { autoClose: false });
            return;
        } else if( fromStore === toStore && fromSubteam === toSubteam ) {
            toast.error('From Store/Subteam cannot be the same as To Store/Subteam', { autoClose: false });
            return;
        }

        const transferData = {
            FromStoreVendorId: 0,
            FromStoreNo: fromStore,
            FromStoreName: storesMapped.filter((store: any) => store.key === fromStore)[0].text.trim(),
            FromSubteamNo: fromSubteam,
            FromSubteamName: subteamsMapped.filter((subteam: any) => subteam.key === fromSubteam)[0].text.trim(),

            ProductType: productType,
            SupplyType: productType === 3 ? supplyType : 0,

            ToStoreNo: toStore,
            ToStoreName: storesMapped.filter((store: any) => store.key === toStore)[0].text.trim(),
            ToSubteamNo: toSubteam,
            ToSubteamName: subteamsMapped.filter((subteam: any) => subteam.key === toSubteam)[0].text.trim(),

            CreatedBy: Config.userId,
            ExpectedDate: new Date(expectedDate),

            Items: []
        } as ITransferData;

        if(comingFromScan && savedTransferData) {
            transferData.Items = savedTransferData.Items;
        }

        localStorage.setItem("transferData", JSON.stringify(transferData)); 

        history.push('/transfer/scan');
    }

    const handleFromStoreChange = (event: React.SyntheticEvent<HTMLElement, Event>, data: DropdownProps) => {
        if(data.value){
            setFromStore(parseInt(data.value?.toString()));
        }
    }

    const handleFromSubteamChange = (event: React.SyntheticEvent<HTMLElement, Event>, data: DropdownProps) => {
        if(data.value){
            setFromSubteam(parseInt(data.value?.toString()));
        }
    }

    const handleToStoreChange = (event: React.SyntheticEvent<HTMLElement, Event>, data: DropdownProps) => {
        if(data.value){
            setToStore(parseInt(data.value?.toString()));
        }
    }

    const handleToSubteamChange = (event: React.SyntheticEvent<HTMLElement, Event>, data: DropdownProps) => {
        if(data.value){
            setToSubteam(parseInt(data.value?.toString()));
        }
    }

    const handleProductTypeChange = (event: React.SyntheticEvent<HTMLElement, Event>, data: DropdownProps) => {
        if(data.value){
            const value = parseInt(data.value?.toString());
            setProductType(value);
            if(value !== 3) {
                setSupplyType(-1);
            }
        }
    }

    const handleSupplyTypeChange = (event: React.SyntheticEvent<HTMLElement, Event>, data: DropdownProps) => {
        if(data.value){
            setSupplyType(parseInt(data.value?.toString()));
        }
    }

    const handleExpectedDateChange = (event: React.ChangeEvent<HTMLInputElement>, { value }: InputOnChangeData) => {
        setExpectedDate(value);
    }

    

    const clearSavedSession = () => {
        localStorage.removeItem('transferData');
        toast.info('Ordered deleted.');
    }

    const confirmDelete = () => {
        setShowConfirmDelete(true);
    }

    return (
        <Fragment>
            <ConfirmModal handleConfirmClose={goToReview} handleCancelClose={confirmDelete} setOpenExternal={setShowSaved} showTriggerButton={false} noGreyClick={true}
                                openExternal={showSaved} headerText='Previous Session Exists' cancelButtonText='No' confirmButtonText='Yes' 
                                lineOne={`Would you like to reload your previous Order Session? (From ${savedTransferData?.FromStoreName}/${savedTransferData?.FromSubteamName} to ${savedTransferData?.ToStoreName}/${savedTransferData?.ToSubteamName})`}
                                lineTwo={'Clicking No will delete the old session.'} />
            <ConfirmModal handleConfirmClose={clearSavedSession} handleCancelClose={() => {history.goBack();}} setOpenExternal={setShowConfirmDelete} showTriggerButton={false} noGreyClick={true}
                                openExternal={showConfirmDelete} headerText='Delete Session?' cancelButtonText='No' confirmButtonText='Yes' 
                                lineOne={`Are you sure you want to delete your saved Order? (From ${savedTransferData?.FromStoreName}/${savedTransferData?.FromSubteamName} to ${savedTransferData?.ToStoreName}/${savedTransferData?.ToSubteamName})`}/>
            <div style={{marginTop: '20px', marginLeft: '5px', marginRight: '5px'}}>
                <Header style={{padding: '0px', paddingLeft: '5px', backgroundColor: 'lightgrey'}} as ='h5' attached='top'>From</Header>
                <Segment attached>
                    <Dropdown selection placeholder='Store' fluid options={storesMapped} value={fromStore} onChange={handleFromStoreChange}></Dropdown>
                    <Dropdown selection placeholder='Subteam' style={{marginTop: '10px'}} fluid options={subteamsMapped} value={fromSubteam} onChange={handleFromSubteamChange}></Dropdown>
                </Segment>
            </div>
            <div style={{marginTop: '15px', marginLeft: '5px', marginRight: '5px'}}>
                <Header style={{padding: '0px', paddingLeft: '5px', backgroundColor: 'lightgrey'}} as ='h5' attached='top'>To</Header>
                <Segment attached>
                    <Dropdown selection placeholder='Store' fluid options={storesMapped} value={toStore} onChange={handleToStoreChange}></Dropdown>
                    <Dropdown selection placeholder='Subteam' style={{marginTop: '10px'}} fluid options={subteamsMapped} value={toSubteam} onChange={handleToSubteamChange}></Dropdown>
                </Segment>
            </div>
            <div style={{marginTop: '5px', marginLeft: '5px', marginRight: '5px'}}>
                <Form.Dropdown selection label='Product Type' fluid options={productTypesMapped} value={productType} onChange={handleProductTypeChange}></Form.Dropdown>
            </div>
            {productType === 3 ?
                    <div style={{ height: '57px', marginTop: '5px', marginLeft: '5px', marginRight: '5px'}}>
                        <Form.Dropdown selection label='Supply Type' fluid options={supplyTypesMapped} onChange={handleSupplyTypeChange} value={supplyType}></Form.Dropdown>
                    </div>
                :
                    <div style={{height: '57px', marginTop: '5px'}} />
            }
            <div style={{marginTop: '5px', marginLeft: '5px', marginRight: '5px'}}>
                <Form.Input type='date' value={expectedDate} onChange={handleExpectedDateChange} min={dateFormat(new Date(), "UTC:yyyy-mm-dd")} label='Expected Date' fluid></Form.Input>
            </div>
            <span style={{ display: "flex", justifyContent: "center", alignItems: "center" }}>
                <WfmButton style={{marginTop: '10px'}} onClick={handleCreatePoClick}>Create PO</WfmButton>
            </span>
        </Fragment>
    )
}

export default TransferHome;