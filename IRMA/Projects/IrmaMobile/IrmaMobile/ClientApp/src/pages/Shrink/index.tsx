import React, { Fragment, useContext, useState, useEffect } from 'react';
// @ts-ignore 
import { BarcodeScanner } from '@wfm/mobile';
import './styles.scss';
import { AppContext, types, IMenuItem } from "../../store";
import { Modal, Button, Header } from 'semantic-ui-react';
import CurrentLocation from "../../layout/CurrentLocation";
import LoadingComponent from '../../layout/LoadingComponent';
import Config from '../../config';


const initialState = {
  isSelected:false, 
  itemDescription: null, 
  retailUnitName: null,
  packageUnitAbbreviation: null,
  quantity: 0,
  skipConfirm: true,
  identifier: null,
  queued: 0,
  upcValue: '',
  shrinkType:'',
  packageDesc1:'',
  packageDesc2:'',
  dupItem:[]
};

const Shrink:React.FC = () => {
    const [shrinkState, setShrinkState] = useState(initialState);
    const {state, dispatch} = useContext<any>(AppContext);
    const {isLoading} = state;
    const {subteam, region, storeNumber, subteamNo, shrinkTypes, shrinkType} = state;
    const [alert, setAlert] = useState<any>({open:false, alertMessage:'', type: 'default', header: 'IRMA Mobile', confirmAction:()=> {}, cancelAction:()=> {}});
    const [warningOverride, setWarningOverride] = useState<boolean>(false);
    const newMenuItems = [
      { id: 1, order: 0, text: "Review", path: "/shrink/review", disabled: false } as IMenuItem,
   ] as IMenuItem[];
    let textInput:any = React.createRef<HTMLInputElement>();
    let qtyInput:any = React.createRef<HTMLInputElement>();

    useEffect(() => {
      BarcodeScanner.registerHandler(function(data:any){
        try{
          setUpc(parseInt(data, 10));
        }catch(ex){
          setAlert({...alert, 
            open:true, 
            alertMessage: ex.message, 
            type: 'default', 
            header:'Scan'});
        }
      });
      dispatch({ type: types.SETMENUITEMS, menuItems: newMenuItems});
      if(shrinkState.isSelected){
        dispatch({ type: types.SHOWSHRINKHEADER, showShrinkHeader: true }); 
      }
      return () => {
        //unmount and unregister handler
      };
      // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [shrinkState, dispatch]);

    const setUpc = (value?:any) =>{  
      let upc = value && typeof value !== 'object' ? value: shrinkState.upcValue;
      setIsLoading(true);
      fetch(`${Config.baseUrl}/${region}/storeitems?storeNo=${storeNumber}&subteamNo=${subteamNo}&scanCode=${upc}`)
      .then(res => res.json())
      .then((result) => {
        if(result.itemKey === 0 && result.itemDescription === null){
          setAlert({...alert, 
            open:true, 
            alertMessage: 'Item not found',
            type: 'default', 
            header:'Irma Mobile'});
        } else { 
          if(result.retailSubteamName !== subteam.subteamName && warningOverride === false){
            setAlert({...alert, 
              open:true, 
              alertMessage: `This is a ${result.retailSubteamName} item and you are shrinking it to ${subteam.subteamName}. Are you sure you want to to do this?`,
              type: 'confirm', 
              header:'Subteam Mismatch', 
              confirmAction: confirmAdd, 
              cancelAction: cancel});
          } 
          setShrinkItem(result, upc);
        }
       })
      .finally(() => setIsLoading(false))
    }

    const setShrinkType = (value:any) =>{
      setShrinkState({...shrinkState, isSelected:true});
      dispatch({ type: 'SETSHRINKTYPE', shrinkType:JSON.parse(value) });
    }

    const setIsLoading = (result: boolean) => {
      dispatch({ type: types.SETISLOADING, isLoading: result })
    }
   const setShrinkItem = (result:any, upc: any) =>{
      let dupItem = dupItemCheck(result);
      if(dupItem.length > 0){
        // @ts-ignore
        setShrinkState({...shrinkState, ...result, upcValue:upc,queued: dupItem[0].quantity, quantity:1, dupItem});
      } else {
        setShrinkState({...shrinkState, ...result, upcValue:upc, quantity:1, dupItem});
      }
    }
    const dupItemCheck = (result:any)=>{
        let shrinkItems:any = [];
        let shrinkItem = [];
        if(localStorage.getItem('shrinkItems')){
          // @ts-ignore
          shrinkItems = JSON.parse(localStorage.getItem('shrinkItems'));
          shrinkItem = shrinkItems.filter((item: any) => item.identifier === result.identifier);
        }
        return shrinkItem;
    }
    const toggleAlert = (e:any) =>{
        setAlert({...alert, 
          open:!alert.open, 
          alertMessage: alert.alertMessage,
          type: 'default', 
          header:'Irma Mobile'});
    }
    const confirmAllItems = (e:any) => {
        setWarningOverride(true);
        setAlert({...alert, 
          open: false, 
          cancelAction: cancel});
    }
    const confirmAdd = (e:any) => {
      setAlert({open:true, 
        header: 'Warning Override', 
        type: 'confirm', 
        alertMessage:'Do this for all items in this session?', 
        confirmAction: confirmAllItems, 
        cancelAction: cancel.bind(e, false)});
    }
    
    const setQty = (e:any) =>{
      setShrinkState({...shrinkState, quantity: e.target.value });
    }
    const skipConfirm = (e:any) =>{
      setShrinkState({...shrinkState, skipConfirm: !shrinkState.skipConfirm});
    }
    const clear = () =>{
      setShrinkState({...initialState, isSelected:true, skipConfirm: shrinkState.skipConfirm});
    }
    const setUpcValue = (e:any) =>{
      setShrinkState({...initialState, upcValue: e.target.value, isSelected:true, skipConfirm: shrinkState.skipConfirm});
    }
    const save = (e:any) =>{
      const { isSelected, skipConfirm, queued, dupItem, ...shrinkItem} = shrinkState;
      shrinkItem.shrinkType = shrinkType.shrinkSubTypeMember;
      shrinkItem.quantity =  +(shrinkState.queued) + +(shrinkState.quantity)
      let shrinkItems: any = [];    

      if(localStorage.getItem("shrinkItems") !== null){
        // @ts-ignore
        shrinkItems = JSON.parse(localStorage.getItem("shrinkItems"));
      }
      
      if(shrinkState.dupItem.length > 0){
        // if dup item found, replace the shrink Item with a new quantity
        
        setAlert({...alert, 
          open: true, 
          // @ts-ignore
          alertMessage: `${shrinkState.dupItem[0].quantity} of this item already queued in this session. Tap Add, Overwrite or Cancel`, 
          type: 'prevScanned', 
          header: 'Previously Scanned Item', 
          cancelAction: cancel});
      } else {
        // else add a new shrink Item
        shrinkItems.push(shrinkItem);
        saveItems(shrinkItems);
      }
    }
    const getShrinkItems = (quantity: number) => {
      const { isSelected, skipConfirm, queued, dupItem, ...shrinkItem} = shrinkState;
      shrinkItem.shrinkType = shrinkType.shrinkSubTypeMember;
      shrinkItem.quantity =  quantity;
       // @ts-ignore
      let shrinkItems = JSON.parse(localStorage.getItem("shrinkItems"));
      let localShrinkItems = shrinkItems.filter((item: any) => item.identifier !== shrinkItem.identifier);
      return [ ...localShrinkItems, shrinkItem];
    }
    const add = (e:any) => {
      setAlert({...alert, open:false});
       // @ts-ignore
      let shrinkItems:[] = getShrinkItems(+(shrinkState.queued) + +(shrinkState.quantity));
      saveItems(shrinkItems);
    }
    const overwrite = (e:any) => {
      setAlert({...alert, open:false});
       // @ts-ignore
      let shrinkItems:[] = getShrinkItems(shrinkState.quantity);
      saveItems(shrinkItems);
    }
    const cancel = (clearData=true, e:any) => {
        setAlert({...alert, open: false});
        if(clearData){
          clear(); 
        }
    }
    const saveItems = (shrinkItems: []) => {
      localStorage.setItem("shrinkItems", JSON.stringify(shrinkItems));
      if(!shrinkState.skipConfirm){
        setAlert({...alert, 
          open:true, 
          alertMessage: 'Shrink Item Saved', 
          type: 'default', 
          header:'Irma Mobile'});
      }
      clear();
    }
    if(isLoading) {
      return ( <LoadingComponent content="Loading Item..."/> )
    }
    return (   
      <Fragment>
      <CurrentLocation/> 
      <div className="shrink-page">
        {!shrinkState.isSelected ?
          (<div className='shrink-container'>
            <h1>Subteam:{subteam.subteamName}</h1>
            <div className="shrink-buttons">
              {shrinkTypes.map((shrinkType:string)=>
                // @ts-ignore 
                <button className="wfm-btn" key={shrinkType.shrinkSubTypeId} value={JSON.stringify(shrinkType)} onClick={(e)=> setShrinkType(e.target.value)}>{shrinkType.shrinkSubTypeMember}</button>
              )}
            </div>
          </div>): (
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
                    onChange={setUpcValue}
                    onKeyPress={(e)=> e.key === 'Enter' ? setUpc() : ''}
                    ref={textInput}/>        
                  <button className='submit-upc' onClick={setUpc}>>></button>
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
                  <input className='qty-input' type='number' min="0" step="1" onChange={setQty} value={shrinkState.quantity} ref={qtyInput}></input>
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
                  <input onClick={skipConfirm} type="checkbox" defaultChecked={shrinkState.skipConfirm}/>
                  <label>Skip Confirm</label>
                </div>
              </section>
              <section className='entry-section'>
                <div className='shrink-buttons'>
                  <button className="wfm-btn" onClick={clear}>Clear</button>
                  <button className="wfm-btn" disabled={shrinkState.quantity===0 || shrinkState.identifier === null} onClick={save}>Save</button>
                </div>
              </section>
            </div>)
        }
      </div>
      <Modal
          open={alert.open}
      >   
        <Header content={alert.header} />
        <Modal.Content>
          {alert.alertMessage}
        </Modal.Content>
        <Modal.Actions>
          {alert.type==='default' ? (
            <Button onClick={toggleAlert}> OK </Button>
          ): alert.type==='confirm' ? (
            <Fragment>
              <Button onClick={alert.confirmAction}> Yes </Button>
              <Button onClick={alert.cancelAction}> No </Button>
            </Fragment>
          ):(
          <Fragment>
            <Button onClick={add}> Add </Button>
            <Button onClick={overwrite}> Overwrite </Button>
            <Button onClick={alert.cancelAction}> Cancel </Button>
          </Fragment>
          )}
        </Modal.Actions>
      </Modal>
      </Fragment>
    )
  }

  export default Shrink;