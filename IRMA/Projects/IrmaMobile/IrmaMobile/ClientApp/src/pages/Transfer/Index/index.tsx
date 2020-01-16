import React, { Fragment, useState, useContext, useEffect } from 'react'
import { AppContext, ITeam, IStore, ITransferData, types } from "../../../store";
import { Header, Segment, Dropdown, Form, InputOnChangeData, DropdownProps } from 'semantic-ui-react'
import CurrentLocation from '../../../layout/CurrentLocation'
import { WfmButton } from '@wfm/ui-react'
import dateFormat from 'dateformat'
import { toast } from 'react-toastify';
// import { useHistory } from 'react-router-dom';

const TransferHome: React.FC = () => {
    //@ts-ignore
    const { state, dispatch } = useContext(AppContext);
    const { subteams, stores } = state;
    const [subteamsMapped, setSubteamsMapped] = useState();
    const [storesMapped, setStoresMapped] = useState();

    const [fromStore, setFromStore] = useState<number>(-1);
    const [fromSubteam, setFromSubteam] = useState<number>(-1);
    const [toStore, setToStore] = useState<number>(-1);
    const [toSubteam, setToSubteam] = useState<number>(-1);
    const [productType, setProductType] = useState<number>(-1);
    const [expectedDate, setExpectedDate] = useState<string>(dateFormat(new Date(), "yyyy-mm-dd"));

    // let history = useHistory();

    const productTypesMapped = [ 
        { key: 1, value: 1, text: 'Product' },
        { key: 2, value: 2, text: 'Packaging Supplies' },
        { key: 3, value: 3, text: 'Other Supplies' },
    ]

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
    }, [subteams])

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
        } else if( fromStore === toStore && fromSubteam === toSubteam ) {
            toast.error('From Store/Subteam cannot be the same as To Store/Subteam', { autoClose: false });
            return;
        }

        const transferData = {
            FromStoreNo: fromStore,
            FromSubteamNo: fromSubteam,
            ToStoreNo: toStore,
            ToSubteamNo: toSubteam,
            ProductType: productType,
            ExpectedDate: new Date(expectedDate)
        } as ITransferData;

        dispatch({ type: types.SETTRANSFERDATA , TransferData: transferData }); 

        /*
            TO-DO: Create Purchase order
        */

        //history.push('/transfer/scan');
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
            setProductType(parseInt(data.value?.toString()));
        }
    }

    const handleExpectedDateChange = (event: React.ChangeEvent<HTMLInputElement>, { value }: InputOnChangeData) => {
        setExpectedDate(value);
    }

    return (
        <Fragment>
            <CurrentLocation/>
            <div style={{marginTop: '20px', marginLeft: '5px', marginRight: '5px'}}>
                <Header style={{padding: '0px', paddingLeft: '5px', backgroundColor: 'lightgrey'}} as ='h5' attached='top'>From</Header>
                <Segment attached>
                    <Dropdown placeholder='Store' fluid selection options={storesMapped} onChange={handleFromStoreChange}></Dropdown>
                    <Dropdown placeholder='Subteam' style={{marginTop: '10px'}} fluid selection options={subteamsMapped} onChange={handleFromSubteamChange}></Dropdown>
                </Segment>
            </div>
            <div style={{marginTop: '15px', marginLeft: '5px', marginRight: '5px'}}>
                <Header style={{padding: '0px', paddingLeft: '5px', backgroundColor: 'lightgrey'}} as ='h5' attached='top'>To</Header>
                <Segment attached>
                    <Dropdown placeholder='Store' fluid selection options={storesMapped} onChange={handleToStoreChange}></Dropdown>
                    <Dropdown placeholder='Subteam' style={{marginTop: '10px'}} fluid selection options={subteamsMapped} onChange={handleToSubteamChange}></Dropdown>
                </Segment>
            </div>
            <div style={{marginTop: '15px', marginLeft: '5px', marginRight: '5px'}}>
                <Form.Dropdown label='Product Type' fluid selection options={productTypesMapped} onChange={handleProductTypeChange}></Form.Dropdown>
            </div>
            <div style={{marginTop: '15px', marginLeft: '5px', marginRight: '5px'}}>
                <Form.Input type='date' value={expectedDate} onChange={handleExpectedDateChange} min={dateFormat(new Date(), "yyyy-mm-dd")} label='Expected Date' fluid selection></Form.Input>
            </div>
            <span style={{ display: "flex", justifyContent: "center", alignItems: "center" }}>
                <WfmButton style={{marginTop: '20px'}} onClick={handleCreatePoClick}>Create PO</WfmButton>
            </span>
        </Fragment>
    )
}

export default TransferHome;