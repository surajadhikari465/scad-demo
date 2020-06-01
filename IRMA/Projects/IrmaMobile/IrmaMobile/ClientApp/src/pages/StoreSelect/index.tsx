import React, { useContext, useEffect } from 'react';
import { AppContext, types, IMenuItem } from "../../store";
import { WfmButton, WfmToggleGroup } from '@wfm/ui-react';
import { useHistory } from "react-router-dom";
import './styles.scss';
import Config from '../../config';
import LoadingComponent from '../../layout/LoadingComponent';
import { AuthHandler } from '@wfm/mobile';

const StoreSelect: React.FC = () => {
  // @ts-ignore 
  const { state, dispatch } = useContext(AppContext);
  const { isLoading } = state;
  let history = useHistory();

  useEffect(() => {
    dispatch({ type: types.SETTITLE, Title: 'IRMA Mobile' });
    
		const logout = () => {
			dispatch({ type: types.RESETSTATE });
			//do nothing with the clearToken callback
			AuthHandler.clearToken(() => { });
		};
		const settingsItems = [
			{ id: 1, order: 0, text: "Log Out", path: "#", disabled: false, onClick: logout } as IMenuItem
		] as IMenuItem[];

		dispatch({ type: types.TOGGLECOG, showCog: true });
		dispatch({ type: types.SETSETTINGSITEMS, settingsItems: settingsItems });
	}, [dispatch]);

  const setSubteams = (result: any) => {
    dispatch({ type: 'SETSUBTEAMS', subteams: result });
  }

  const setIsLoading = (result: boolean) => {
    dispatch({ type: types.SETISLOADING, isLoading: result })
  }

  const getSubteams = () => {
    const { region } = state;
    setIsLoading(true);
    fetch(`${Config.baseUrl}/${region}/subteams`)
      .then(res => res.json())
      .then(result => { setSubteams(result) })
      .then(() => history.push('/functions'))
      .finally(() => setIsLoading(false))
  }

  const selectStore = (e: any) => {
    dispatch({ type: types.SETSTORE, store: e.detail.name });
    dispatch({ type: types.SETSTORENUMBER, storeNumber: e.detail.storeNo });
  }

  if (isLoading) {
    return (<LoadingComponent content="Loading subteams..." />)
  }
  return (
    <div>
      <div className="page-title-wrapper">
        <p>Select your Store</p>
      </div>
      <div className="store-container">
        <div className="toggle-group">
          <WfmToggleGroup buttons={state.stores} direction="vertical" onWfmToggleButtonSelected={selectStore} flex />
        </div>
      </div>
      <div className="store-select">
        <button className="irma-btn" disabled={state.store === ''} onClick={getSubteams}>Select Store</button>
      </div>
    </div>)
}

export default StoreSelect;