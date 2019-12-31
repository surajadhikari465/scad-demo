import React, { useContext, useState, useEffect } from 'react';
// @ts-ignore 
import { BarcodeScanner } from '@wfm/mobile';
import './styles.scss';
import { AppContext, types, IMenuItem } from "../../store";
import LoadingComponent from '../../layout/LoadingComponent';
import { Config } from '../../config';


const initialState = {
  isSelected:false, 
  itemDescription: null, 
  retailUnitName: null,
  packageUnitAbbreviation: null,
  quantity: 0,
  skipConfirm: true,
  identifier: null,
  queued: 0,
  upcValue: ''
};

const Shrink:React.FC = () => {
    const [shrinkState, setShrinkState] = useState(initialState);
    // @ts-ignore 
    const {state, dispatch} = useContext(AppContext);
    const {isLoading} = state;
    const {subteam, region, storeNumber, subteamNo, shrinkTypes, shrinkType, store} = state;
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
          alert(ex.message)
        }
      });
      dispatch({ type: types.SETMENUITEMS, menuItems: newMenuItems});
      return () => {
        //unmount and unregister handler
      };
    }, [shrinkState]);
    const setShrinkType = (value:any) =>{
      setShrinkState({...shrinkState, isSelected:true});
      dispatch({ type: 'SETSHRINKTYPE', shrinkType:JSON.parse(value) });
    }
    const setIsLoading = (result: boolean) => {
      dispatch({ type: types.SETISLOADING, isLoading: result })
    }
    const setShrinkItem = (result:any, upc: any) =>{
        let shrinkItems:any = [];
        let shrinkItem = [];
        if(localStorage.getItem('shrinkItems')){
          // @ts-ignore
          shrinkItems = JSON.parse(localStorage.getItem('shrinkItems'));
          shrinkItem = shrinkItems.filter((item: any) => item.identifier === result.identifier);
        }
        if(shrinkItem.length > 0){
          setShrinkState({...shrinkState, ...result, upcValue:upc, quantity:1, queued: shrinkItem[0].quantity});
        }else{
          setShrinkState({...shrinkState, ...result, upcValue:upc, quantity:1, queued: 0});
        }
    }
    const setUpc = (value?:any) =>{  
      let upc = value && typeof value !== 'object' ? value: shrinkState.upcValue;
      setIsLoading(true);
      fetch(`${Config.baseUrl}/${region}/storeitems?storeNo=${storeNumber}&subteamNo=${subteamNo}&scanCode=${upc}`)
      .then(res => res.json())
      .then((result) => setShrinkItem(result, upc))
      .finally(() => setIsLoading(false))
    }
    const setQty = (e:any) =>{
      setShrinkState({...shrinkState, quantity: e.target.value });
    }
    const skipConfirm = (e:any) =>{
      setShrinkState({...shrinkState, skipConfirm: !shrinkState.skipConfirm});
    }
    const clear = () =>{
      qtyInput.current.value = '';
      setShrinkState({...initialState, isSelected:true});
    }
    const setUpcValue = (e:any) =>{
      setShrinkState({...initialState, upcValue: e.target.value, isSelected:true});
    }
    const save = (e:any) =>{
      const { isSelected, skipConfirm, queued, ...shrinkItem} = shrinkState;
      shrinkItem.quantity =  +(shrinkState.queued) + +(shrinkState.quantity)
      let shrinkItems: any = [];    
      if(localStorage.getItem("shrinkItems") !== null){
        // @ts-ignore
        shrinkItems = JSON.parse(localStorage.getItem("shrinkItems"));
      }
      if(shrinkItems.filter((item: any) => item.identifier === shrinkItem.identifier).length > 0){
        let localShrinkItems = shrinkItems.filter((item: any) => item.identifier !== shrinkItem.identifier);
        shrinkItems = [ ...localShrinkItems, shrinkItem]
      } else {
        shrinkItems.push(shrinkItem);
      }
      localStorage.setItem("shrinkItems", JSON.stringify(shrinkItems))
      if(!shrinkState.skipConfirm){
        window.alert('Shrink Item Saved');
      }
      clear();
    }
    if(isLoading) {
      return ( <LoadingComponent content="Loading Item..."/> )
    }
    return (    
      <div className="shrink-page">
        <header className='region-store-header'><span>Region: {region}</span><span>Store: {store}</span></header>
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
              <header className='shrink-type-header'><span>IRMA Shrink: {shrinkType.shrinkType}</span></header>
              <div></div>
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
                  <input className='qty-input' type='number' min="0" step="1" onChange={setQty} defaultValue={shrinkState.quantity} ref={qtyInput}></input>
                  {
                    shrinkState.packageUnitAbbreviation && 
                    <label className='qty-type'>Retail: {shrinkState.retailUnitName}</label>
                  }
                </div>
                {   
                  shrinkState.retailUnitName && 
                  <div className='description'>
                    <label>UOM:</label>
                    <span>{shrinkState.packageUnitAbbreviation}</span>
                  </div>
                } 
              </section>
              <section className='entry-section queued-section'>
                <h1>Queued: {shrinkState.queued}</h1>
                <div>
                  <input onClick={skipConfirm} type="checkbox" defaultChecked={true}/>
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
    )
  }

  export default Shrink;