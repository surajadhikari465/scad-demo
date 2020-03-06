import React, { Fragment, useContext, useState, useEffect } from 'react';
// @ts-ignore 
import { BarcodeScanner, IBarcodeScannedEvent } from '@wfm/mobile';
import './styles.scss';
import { AppContext, types, IMenuItem } from "../../store";
import BasicModal from '../../layout/BasicModal';
import CurrentLocation from "../../layout/CurrentLocation";
import LoadingComponent from '../../layout/LoadingComponent';
import agent from "../../api/agent";


const initialState = {
  isSelected: false,
  itemDescription: null,
  retailUnitName: null,
  packageUnitAbbreviation: null,
  quantity: 0,
  skipConfirm: true,
  identifier: null,
  queued: 0,
  upcValue: '',
  shrinkType: '',
  packageDesc1: '',
  packageDesc2: '',
  dupItem: [],
  costedByWeight: false
};

const Shrink: React.FC = () => {
  const [shrinkState, setShrinkState] = useState(initialState);
  //@ts-ignore
  const { state, dispatch } = useContext(AppContext);
  const { isLoading, user } = state;
  const { subteam, region, storeNumber, subteamNo, shrinkTypes, shrinkType, subteamSession } = state;
  const [alert, setAlert] = useState<any>({ open: false, alertMessage: '', type: 'default', header: 'IRMA Mobile', confirmAction: () => { }, cancelAction: () => { } });
  const [warningOverride, setWarningOverride] = useState<boolean>(false);

  let textInput: any = React.createRef<HTMLInputElement>();
  let qtyInput: any = React.createRef<HTMLInputElement>();
  let sessionIndex = subteamSession.findIndex((session: any) => session.sessionUser.userName === user?.userName);

  useEffect(() => {
    BarcodeScanner.registerHandler(function (data: IBarcodeScannedEvent) {
      if (shrinkState.isSelected === true) {
        try {
          setUpc(parseInt(data.Data, 10), true);
        } catch (ex) {
          setAlert({
            ...alert,
            open: true,
            alertMessage: ex.message,
            type: 'default',
            header: 'Scan'
          });
        }
      }
    });

    if (subteamSession[sessionIndex].isPrevSession) {
      dispatch({ type: types.SETSHRINKTYPE, shrinkType: subteamSession[sessionIndex].sessionShrinkType });
      dispatch({ type: types.SETSUBTEAM, subteam: subteamSession[sessionIndex].sessionSubteam });
      dispatch({ type: types.SETSTORE, store: subteamSession[sessionIndex].sessionStore });
      dispatch({ type: types.SETSTORENUMBER, storeNumber: subteamSession[sessionIndex].sessionNumber.toString() });
      dispatch({ type: types.SETREGION, region: subteamSession[sessionIndex].sessionRegion });
    }

    if (shrinkState.isSelected || state.subteamSession[sessionIndex].isPrevSession) {
      dispatch({ type: types.SHOWSHRINKHEADER, showShrinkHeader: true });
    }
    return () => {
      //unmount and unregister handler
      shrinkState.isSelected = false;
      dispatch({ type: types.SHOWSHRINKHEADER, showShrinkHeader: false });
    };
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [shrinkState, dispatch]);
  useEffect(() => {
    const changeSubtype = () => {
      subteamSession[sessionIndex] = { ...subteamSession[sessionIndex], forceSubteamSelection: true };
      setShrinkState(initialState);
      dispatch({ type: types.SETSUBTEAMSESSION, subteamSession });
      dispatch({ type: types.SHOWSHRINKHEADER, showShrinkHeader: false });
    }
    const newMenuItems = (shrinkState.isSelected || state.subteamSession[sessionIndex].isPrevSession) && !state.subteamSession[sessionIndex].forceSubteamSelection ? [
      {
        id: 1, order: 0, path: "#", text: "Clear Session", onClick: () => {
          setAlert({
            ...alert,
            open: true,
            alertMessage: 'Would you like to delete the current session?',
            type: 'confirm',
            header: 'Scan',
            confirmAction: confirmDeleteSession,
            cancelAction: cancel.bind(undefined, false, false)
          })
        }
      } as IMenuItem,

      { id: 2, order: 1, text: "Exit Shrink", path: "/functions", disabled: false } as IMenuItem,
      { id: 3, order: 2, text: "Change Subtype", path: "#", onClick: changeSubtype, disabled: false } as IMenuItem,
      { id: 4, order: 3, text: "Review", path: "/shrink/review", disabled: false } as IMenuItem,
    ] as IMenuItem[] : [
      { id: 1, order: 0, text: "Cancel", path: "/functions", disabled: false } as IMenuItem,
    ] as IMenuItem[];

    dispatch({ type: types.SETMENUITEMS, menuItems: newMenuItems });

  }, [shrinkState, dispatch, alert, state.subteamSession]);

  const setUpc = async (value?: any, scan: boolean = false) => {
    let upc = value && typeof value !== 'object' ? value : shrinkState.upcValue;
    setIsLoading(true);
    try {
        let storeItem = await agent.StoreItem.getStoreItem(
          region,
          storeNumber,
          subteamNo,
          user!.userId,
          upc
      );    
      
      if (storeItem.itemKey === 0 && storeItem.itemDescription === null) {
        setAlert({
          ...alert,
          open: true,
          alertMessage: 'Item not found',
          type: 'default',
          header: 'Irma Mobile'
        });
      } else if (!subteam.isUnrestricted && storeItem.retailSubteamName !== subteam.subteamName) {
        setAlert({
          ...alert,
          open: true,
          alertMessage: `This is a ${storeItem.retailSubteamName} and you cannot shrink it to ${subteam.subteamName}. ${subteam.subteamName} is restricted to same-subteam items.`,
          type: 'default',
          header: 'Subteam Mismatch'
        });
      } else {
        let alreadyScanned = state.subteamSession[sessionIndex].shrinkItems.filter((item: any) => storeItem.retailSubteamName === item.retailSubteamName).length > 0 ? true : false;
        if (storeItem.retailSubteamName !== subteam.subteamName && warningOverride === false && !alreadyScanned) {
          setAlert({
            ...alert,
            open: true,
            alertMessage: `This is a ${storeItem.retailSubteamName} item and you are shrinking it to ${subteam.subteamName}. Are you sure you want to to do this?`,
            type: 'confirm',
            header: 'Subteam Mismatch',
            confirmAction: confirmAdd,
            cancelAction: cancel.bind(undefined, true, false)
          });
        }
        let quantity = shrinkState.quantity;
        if (scan) {
          quantity += 1;
        } else quantity = 1;
        setShrinkItem(storeItem, upc, quantity);
      }
    } finally {
      setIsLoading(false);
    }
  }

  const setShrinkType = (value: any) => {
    let shrinkType = JSON.parse(value);
    setShrinkState({ ...shrinkState, isSelected: true });
    subteamSession[sessionIndex] = { ...subteamSession[sessionIndex], forceSubteamSelection: false, sessionShrinkType: shrinkType };
    dispatch({ type: types.SETSUBTEAMSESSION, subteamSession });
    dispatch({ type: 'SETSHRINKTYPE', shrinkType });
  }


  const setShrinkItem = (result: any, upc: any, quantity: any) => {
    let dupItem = dupItemCheck(result);
    if (dupItem.length > 0) {
      setShrinkState({ ...shrinkState, ...result, upcValue: upc, queued: dupItem[0].quantity, quantity, dupItem });
    } else {
      setShrinkState({ ...shrinkState, ...result, upcValue: upc, quantity, dupItem });
    }
  }

  const setIsLoading = (result: boolean) => {
    dispatch({ type: types.SETISLOADING, isLoading: result })
  }
  const dupItemCheck = (result: any) => {
    let shrinkItem = [];
    if (subteamSession[sessionIndex] && subteamSession[sessionIndex].shrinkItems) {
      shrinkItem = subteamSession[sessionIndex].shrinkItems.filter((item: any) => item.identifier === result.identifier && state.shrinkType.shrinkSubTypeMember === item.shrinkType);
    }
    return shrinkItem;
  }

  const confirmAllItems = (e: any) => {
    setWarningOverride(true);
    setAlert({
      ...alert,
      open: false
    });
  }
  const confirmAdd = (e: any) => {
    setAlert({
      open: true,
      header: 'Warning Override',
      type: 'confirm',
      alertMessage: 'Do this for all items in this session?',
      confirmAction: confirmAllItems,
      cancelAction: cancel.bind(undefined, false, false)
    });
  }
  const confirmDeleteSession = (e: any) => {
    if (state.subteamSession[sessionIndex].shrinkItems?.length == null) {
      setAlert({
        ...alert,
        open: true,
        alertMessage: 'No session exists.',
        type: 'default',
        header: 'Scan Shrink'
      });
    } else {
      localStorage.clear();
      setShrinkState({ ...shrinkState, isSelected: false });
      subteamSession[sessionIndex] = { shrinkItems: [], isPrevSession: false, sessionShrinkType: '', sessionSubteam: undefined, sessionStore: '', sessionNumber: 0, sessionRegion: '', sessionUser: user, forceSubteamSelection: true };
      dispatch({ type: types.SETSUBTEAMSESSION, subteamSession })
      setAlert({
        ...alert,
        open: false
      });
    }
  }
  const setQty = (e: any) => {
    let quantity = parseFloat(e.target.value);
    if (!shrinkState.costedByWeight) {
      setShrinkState({ ...shrinkState, quantity: parseInt(e.target.value.replace(/[^\w\s]|_/g, "")) });
    }
    else setShrinkState({ ...shrinkState, quantity: quantity });
  }
  const skipConfirm = (e: any) => {
    setShrinkState({ ...shrinkState, skipConfirm: !shrinkState.skipConfirm });
  }
  const clear = () => {
    setShrinkState({ ...initialState, isSelected: true, skipConfirm: shrinkState.skipConfirm });
  }
  const clearRetainUPC = () => {
    setShrinkState({ ...initialState, upcValue: shrinkState.upcValue, ...skipConfirm, isSelected: true });
  }
  const setUpcValue = (e: any) => {
    let value = e.target.value;
    setShrinkState({ ...initialState, upcValue: value.replace(/[^\w\s]|_/g, ""), isSelected: true, skipConfirm: shrinkState.skipConfirm });
  }
  const save = (e: any) => {
    if (shrinkState.quantity > 999) {
      setAlert({
        ...alert,
        open: true,
        alertMessage: 'Quantity cannot be greater than 999',
        type: 'default',
        header: 'Scan Shrink'
      });
    } else if (shrinkState.quantity === 0) {
      setAlert({
        ...alert,
        open: true,
        alertMessage: 'Quantity cannot be 0',
        type: 'default',
        header: 'Scan Shrink'
      });
    } else if (isNaN(shrinkState.quantity)) {
      setAlert({
        ...alert,
        open: true,
        alertMessage: 'Please enter a numeric value for the Quantity',
        type: 'default',
        header: 'Update Shrink'
      });
    } else {
      const { isSelected, skipConfirm, queued, dupItem, ...shrinkItem } = shrinkState;
      shrinkItem.shrinkType = shrinkType.shrinkSubTypeMember;
      shrinkItem.quantity = parseFloat((+(shrinkState.queued) + +(shrinkState.quantity)).toPrecision(4));
      let shrinkItems: any = [];

      if (state.subteamSession[sessionIndex].shrinkItems) {
        // @ts-ignore
        shrinkItems = state.subteamSession[sessionIndex].shrinkItems;
      }

      if (shrinkState.dupItem.length > 0) {
        // if duplicate item found, replace the shrink Item with a new quantity
        setAlert({
          ...alert,
          open: true,
          // @ts-ignore
          alertMessage: `${shrinkState.dupItem[0].quantity} of this item already queued in this session. Tap Add, Overwrite or Cancel`,
          type: 'prevScanned',
          header: 'Previously Scanned Item',
          cancelAction: cancel.bind(undefined, false, true)
        });
      } else {
        // else add a new shrink Item
        shrinkItems.push(shrinkItem);
        saveItems(shrinkItems);
      }
    }
  }

  const getShrinkItems = (quantity: number) => {
    const { isSelected, skipConfirm, queued, dupItem, ...shrinkItem } = shrinkState;
    shrinkItem.shrinkType = shrinkType.shrinkSubTypeMember;
    shrinkItem.quantity = quantity;
    // @ts-ignore
    let shrinkItems = state.subteamSession[sessionIndex].shrinkItems;
    let localShrinkItems = shrinkItems.filter((item: any) => item.identifier !== shrinkItem.identifier || item.shrinkType !== state.shrinkType.shrinkSubTypeMember);
    return [...localShrinkItems, shrinkItem];
  }

  const add = (e: any) => {
    setAlert({ ...alert, open: false });
    let shrinkItems: any[] = getShrinkItems(parseFloat((+(shrinkState.queued) + +(shrinkState.quantity)).toPrecision(4)));
    saveItems(shrinkItems);
  }

  const overwrite = (e: any) => {
    setAlert({ ...alert, open: false });
    let shrinkItems: any[] = getShrinkItems(shrinkState.quantity);
    saveItems(shrinkItems);
  }

  const cancel = (clearData = true, cancelledAlert = true, e: any) => {
    setAlert({ ...alert, open: false });
    if (clearData) {
      clearRetainUPC();
    }
    if (cancelledAlert) {
      setAlert({
        ...alert,
        open: true,
        header: 'Shrink Save Cancelled',
        type: 'default',
        alertMessage: `Shrink for ${shrinkState.upcValue} UPC was not saved`
      });
    }
  }

  const saveItems = (shrinkItems: any[]) => {
    subteamSession[sessionIndex] = { ...subteamSession[sessionIndex], shrinkItems: shrinkItems, sessionNumber: parseInt(state.storeNumber), sessionShrinkType: state.shrinkType, sessionSubteam: state.subteam, sessionStore: state.store, sessionRegion: state.region, isPrevSession: true };

    dispatch({ type: types.SETSUBTEAMSESSION, subteamSession });
    if (!shrinkState.skipConfirm) {
      setAlert({
        ...alert,
        open: true,
        alertMessage: 'Shrink Item Saved',
        type: 'default',
        header: 'Irma Mobile'
      });
    }
    clear();
  }

  const clearInvalid = (e: any) => {
    //android behavior fix, to stop invalid punctuation 
    if (e.target.value === '') {
      e.target.value = '';
    }
  }

  if (isLoading) {
    return (<LoadingComponent content="Loading Item..." />)
  }
  return (
    <Fragment>
      <CurrentLocation />
      <div className="shrink-page">
        {((!state.subteamSession![sessionIndex].isPrevSession && !shrinkState.isSelected) || state.subteamSession[sessionIndex].forceSubteamSelection) ?
          (<div className='shrink-container'>
            <h1>Subteam: {subteam.subteamName}</h1>
            <div className="shrink-buttons">
              {shrinkTypes.map((shrinkType: string) =>
                // @ts-ignore 
                <button className="wfm-btn" key={shrinkType.shrinkSubTypeId} value={JSON.stringify(shrinkType)} onClick={(e) => setShrinkType(e.target.value)}>{shrinkType.shrinkSubTypeMember}</button>
              )}
            </div>
          </div>) : (
            <div className='shrink-container shrink-form'>
              <section className='entry-section'>
                <div className='input-line'>
                  <label>UPC</label>
                  <input
                    className='upc-input'
                    type='number'
                    min="0"
                    step="1"
                    value={shrinkState.upcValue}
                    onKeyPress={clearInvalid}
                    onChange={setUpcValue}
                    onKeyDown={(e) => e.key === 'Enter' ? setUpc() : ''}
                    ref={textInput} />
                  <button className='submit-upc wfmButton' onClick={setUpc}>>></button>
                </div>
                {
                  shrinkState.itemDescription &&
                  <div className='description'>
                    <label>Desc:</label>
                    <span>{shrinkState.itemDescription}</span>
                  </div>
                }
              </section>
              <section className='entry-section'>
                <div className='input-line'>
                  <label>Qty:</label>
                  <input className='qty-input'
                    type='number'
                    min="0"
                    onChange={setQty}
                    onKeyPress={clearInvalid}
                    maxLength={3}
                    onKeyDown={(e: any) => e.key === 'Enter' ? e.target.blur() : ''}
                    value={shrinkState.quantity.toString()}
                    ref={qtyInput}></input>
                  {
                    shrinkState.packageUnitAbbreviation &&
                    <label className='qty-type'>Retail: {shrinkState.retailUnitName}</label>
                  }
                </div>
                {
                  shrinkState.retailUnitName &&
                  <div className='description'>
                    <label>UOM:</label>
                    <span>{shrinkState.packageDesc1}/{shrinkState.packageDesc2} {shrinkState.packageUnitAbbreviation}</span>
                  </div>
                }
              </section>
              <section className='entry-section queued-section'>
                <h1>Queued: {shrinkState.queued}</h1>
                <div>
                  <input onClick={skipConfirm} type="checkbox" defaultChecked={shrinkState.skipConfirm} />
                  <label>Skip Confirm</label>
                </div>
              </section>
              <section className='entry-section'>
                <div className='shrink-buttons'>
                  <button className="wfm-btn" onClick={clear}>Clear</button>
                  <button className="wfm-btn" disabled={shrinkState.identifier === null} onClick={save}>Save</button>
                </div>
              </section>
            </div>)
        }
      </div>
      <BasicModal alert={alert} add={add} overwrite={overwrite} setAlert={setAlert} />
    </Fragment>
  )
}

export default Shrink;