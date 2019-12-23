import React, { useContext, useEffect } from 'react';
import './styles.scss';
import { AppContext, types } from "../../store";
import { Config } from '../../config';
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
    const setStores = (result:any) =>{
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
    const getStores=()=>{
      const {region} = state;
      setIsLoading(true);
      fetch(`${Config.baseUrl}/${region}/stores/`)
      .then(res => res.json())
      .then(result =>  {setStores(result)})
      .then(()=>history.push('/store') )
        .finally(() => setIsLoading(false))
        fetch(`${Config.baseUrl}/${region}/shrinksubtypes/`)
            .then(res => res.json())
            .then(result => { setShrinkTypes(result) })
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