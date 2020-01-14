import React, { useContext, useEffect } from 'react';
import './styles.scss';
import { AppContext, types } from "../../store";
import agent from '../../api/agent';
import { toast } from "react-toastify";
import LoadingComponent from '../../layout/LoadingComponent';
import {WfmButton, WfmToggleGroup} from '@wfm/ui-react';

const REGION_LIST = ['SP', 'MW', 'MA', 'SW', 'SO', 'NC', 'RM', 'NE', 'NA', 'FL', 'PN', 'EU'];
// @ts-ignore 
interface RegionProps {
  history: any;
}

const RegionSelect:React.FC<RegionProps> = (props) => {
    const {history} = props;
     // @ts-ignore 
    const {state, dispatch} = useContext(AppContext);
    const {isLoading} = state;
    const setStores = (result:any) =>{;
      dispatch({ type: 'SETSTORES', stores:result });  
    }

    useEffect(() => {
      dispatch({ type: types.SETMENUITEMS, menuItems: [] });
    }, [dispatch]);

    const setIsLoading = (result: boolean) => {
        dispatch({ type: types.SETISLOADING, isLoading: result })
    }
    const setShrinkTypes = (result: any) => {
        dispatch({ type: types.SETSHRINKTYPES, shrinkTypes: result });
    }
    const getStores= async() =>{
      const {region} = state;
      try {
        setIsLoading(true);
        var stores: any = await agent.RegionSelect.getStores(region);
        var shrinkSubTypes: any = await agent.RegionSelect.getShrinkSubtypes(region);
        if(!stores) {
          toast.error("Store could not be loaded. Try again.", { autoClose: false });
          return;
        }
        if(!shrinkSubTypes) {
          toast.error("Shrink Types could not be loaded. Try again.", { autoClose: false });
          return;
        }
        try {
          setStores(stores);
          setShrinkTypes(shrinkSubTypes);
        }catch(err) {
          toast.error("Unable to Set Stores", { autoClose: false })
        }
      }
      finally {
        history.push('/store');
        setIsLoading(false);
      }
    }

    const selectRegion = (e:any) =>{
      dispatch({ type: types.SETREGION, region: e.detail.value });    
    }
    if(isLoading) {
      return ( <LoadingComponent content="Loading stores..."/> )
    }
    return (
    <div>
      <div className="page-title-wrapper">
        <p>Select your Region</p>
      </div>
      <div className="region-container">
        <div className="toggle-group">
          <WfmToggleGroup buttons={REGION_LIST} direction="vertical" onWfmToggleButtonSelected={selectRegion} flex/>
        </div>
      </div>  
      <div className="region-select">
        <WfmButton disabled={state.region===''} onClick={getStores}>Select Region</WfmButton>
      </div>
    </div>)
    
  };

  export default RegionSelect;