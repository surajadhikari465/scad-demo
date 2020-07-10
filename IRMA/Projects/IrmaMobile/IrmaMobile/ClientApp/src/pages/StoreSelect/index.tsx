import React, { useContext, useEffect, useState } from 'react';
import { AppContext, types, IMenuItem, IStore } from "../../store";
import { WfmToggleGroup } from '@wfm/ui-react';
import { useHistory } from "react-router-dom";
import './styles.scss';
import Config from '../../config';
import LoadingComponent from '../../layout/LoadingComponent';
import { AuthHandler } from '@wfm/mobile';
import { toast } from 'react-toastify';

const StoreSelect: React.FC = () => {
  // @ts-ignore 
  const { state, dispatch } = useContext(AppContext);
  const { isLoading, user } = state;
  const [stores, setStores] = useState<IStore[]>();
  let history = useHistory();

  useEffect(() => {
    dispatch({ type: types.SETTITLE, Title: 'IRMA Mobile' });

    const logout = () => {
      try {
        //do nothing with the clearToken callback
        AuthHandler.clearToken(() => { });
      } catch (error) {
        toast.error(`Error logging out. ${error}`);
        console.error(`Error logging out. ${error}`);
      }
    };
    const settingsItems = [
      { id: 1, order: 0, text: "Log Out", path: "#", disabled: false, onClick: logout } as IMenuItem
    ] as IMenuItem[];
    if (user) {
      if (user.telxonStoreLimit === -1 || user.isSuperUser || user.isCoordinator) {
        setStores(state.stores);
      } else {
        setStores(state.stores.filter(s => parseInt(s.storeNo) === user.telxonStoreLimit));
      }
    }
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
          <WfmToggleGroup buttons={stores} direction="vertical" onWfmToggleButtonSelected={selectStore} flex />
        </div>
      </div>
      <div className="store-select">
        <button className="irma-btn" disabled={state.store === ''} onClick={getSubteams}>Select Store</button>
      </div>
    </div>)
}

export default StoreSelect;