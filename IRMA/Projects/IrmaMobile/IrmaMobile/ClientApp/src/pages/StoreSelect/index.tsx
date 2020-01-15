import React, { useContext } from 'react';
import { AppContext, types } from "../../store";
import {WfmButton, WfmToggleGroup} from '@wfm/ui-react';
import {
  useHistory
} from "react-router-dom";
import './styles.scss';
import Config from '../../config';
import LoadingComponent from '../../layout/LoadingComponent';

const StoreSelect:React.FC = () => {
  // @ts-ignore 
  const {state, dispatch} = useContext(AppContext);
  const {isLoading} = state;
  let history = useHistory();
  const setSubteams = (result:any) =>{
    dispatch({ type: 'SETSUBTEAMS', subteams:result });  
  }

  const setIsLoading = (result: boolean) => {
    dispatch({ type: types.SETISLOADING, isLoading: result })
  }

  const getSubteams=()=>{
    const {region} = state;
    setIsLoading(true);
    fetch(`${Config.baseUrl}/${region}/subteams`)
    .then(res => res.json())
    .then(result => {setSubteams(result)})
    .then(()=>history.push('/functions') )
    .finally(() => setIsLoading(false))
  }

  const selectStore = (e:any) =>{
    localStorage.removeItem('shrinkItems');
    dispatch({ type: types.SETSTORE, store: e.detail.name });
    dispatch({ type: types.SETSTORENUMBER, storeNumber: e.detail.storeNo });        
  }  

  if(isLoading) {
    return ( <LoadingComponent content="Loading subteams..."/> )
  }
  return (
  <div>
    <div className="page-title-wrapper">
      <p>Select your Store</p>
    </div>
    <div className="store-container">
      <div className="toggle-group">
        <WfmToggleGroup buttons={state.stores} direction="vertical" onWfmToggleButtonSelected={selectStore} flex/>        
      </div>
    </div>  
    <div className="store-select">
        <WfmButton disabled={state.store===''} onClick={getSubteams}>Select Store</WfmButton>
      </div>
  </div>)
}

export default StoreSelect;