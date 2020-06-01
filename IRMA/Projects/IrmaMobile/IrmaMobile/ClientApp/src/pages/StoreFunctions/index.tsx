import React, { Fragment, useContext, useState, useEffect } from 'react';
import './styles.scss';
import { Modal } from 'semantic-ui-react';
import BasicModal from '../../layout/BasicModal';
import CurrentLocation from "../../layout/CurrentLocation";
import { AppContext, types, IMenuItem } from "../../store";
import { AuthHandler } from '@wfm/mobile';

interface StoreFunctionsProps {
  history: any;
}

const StoreFunctions: React.FC<StoreFunctionsProps> = (props) => {
  // @ts-ignore 
  const { state, dispatch } = useContext(AppContext);
  const [isSelected, setSelected] = useState(false);
  const [alertIsOpen, setAlertOpen] = useState(false);
  const [alert, setAlert] = useState<any>({ open: false, alertMessage: '', type: 'default', header: 'IRMA Mobile', confirmAction: () => { } });
  const { subteams, user, subteamSession } = state;
  const { history } = props;
  let sessionIndex = subteamSession.findIndex((session:any) => session.sessionUser.userName === user?.userName);

  useEffect(() => {
    const logout = () => {
			dispatch({ type: types.RESETSTATE });
			//do nothing with the clearToken callback
			AuthHandler.clearToken(() => { });
		};
    
    const settingsItems = [
      { id: 1, order: 0, text: "Change Store", path: "/", disabled: false } as IMenuItem,
			{ id: 2, order: 1, text: "Log Out", path: "#", disabled: false, onClick: logout } as IMenuItem
    ] as IMenuItem[];

    dispatch({ type: types.TOGGLECOG, showCog: true });
    dispatch({ type: types.SETSETTINGSITEMS, settingsItems: settingsItems });
    dispatch({ type: types.SETMENUITEMS, menuItems: [] });
    dispatch({ type: types.SHOWSHRINKHEADER, showShrinkHeader: false });
    dispatch({ type: types.SETTITLE, Title: 'IRMA Main Menu' });

    return () => {
      dispatch({ type: types.TOGGLECOG, showCog: false });
    };
  }, [dispatch]);

  const deleteSession = () => {
    localStorage.removeItem('sessionSubType');
    dispatch({ type: types.SETSHRINKTYPE, shrinkType: {} });
    subteamSession[sessionIndex] = { ...subteamSession[sessionIndex], shrinkItems: [], isPrevSession: false };
    dispatch({ type: types.SETSUBTEAMSESSION, subteamSession });
    setAlert({
      ...alert,
      open: false
    });
    history.push('/shrink');
  }

  const deleteWarning = () => {
    setAlert({
      ...alert,
      open: true,
      alertMessage: 'Are you sure you want to delete you saved Session?',
      type: 'confirm',
      header: 'Delete Session?',
      confirmAction: deleteSession
    });
  }

  const confirm = () => {
    setAlert({
      ...alert,
      open: false
    });
    subteamSession[sessionIndex] = { ...subteamSession[sessionIndex], isPrevSession: true };
    dispatch({ type: types.SETSUBTEAMSESSION, subteamSession });
    history.push('/shrink');
  }

  const checkLocalStorage = () => {
    setAlert({
      ...alert,
      open: true,
      alertMessage: `Would you like to reload your previous Session? (${state.subteamSession[sessionIndex].sessionUser.userName} for ${state.subteamSession[sessionIndex].sessionSubteam?.subteamName}) Clicking No will delete the old session.`,
      type: 'confirm',
      header: 'Previous Session Exists',
      cancelAction: deleteWarning,
      confirmAction: confirm
    });
  }

  const handleClick = (e: any) => {
    if (!isSelected) {
      setAlertOpen(true);
    }
    else {
      let shrinkItems = [];
      if (subteamSession[sessionIndex].shrinkItems) {
        shrinkItems = subteamSession[sessionIndex].shrinkItems;
      }
      if (shrinkItems.length > 0 && e.target.value === 'shrink') {
        checkLocalStorage();
      } else {
        history.push(`/${e.target.value}`);
      }
    }
  }

  const handleReceiveClick = () => {
    history.push('/receive/PurchaseOrder/');
  }

  const handleTransferClick = () => {
    if (!isSelected) {
      setAlertOpen(true);
    } else {
      history.push('/transfer/index/');
    }
  }

  const toggleAlert = (e: any) => {
    setAlertOpen(!alertIsOpen);
  }
  const setSubteam = (value: any) => {
    setSelected(true);
    dispatch({ type: types.SETSUBTEAM, subteam: JSON.parse(value) });
    dispatch({ type: types.SETSUBTEAMNO, subteamNo: JSON.parse(value).subteamNo });
  }

  return (
    <Fragment>
      <CurrentLocation />
      <div className="store-functions">
        <div className="page-title-wrapper">
          <div className="message-container">
            <p>Set your subteam, then select a function</p>
          </div>
        </div>
        <div className="subteam-select">
          <select onChange={(e) => setSubteam(e.target.value)} >
            <option>--Select Subteam--</option>
            {subteams.map(team =>
              // @ts-ignore 
              <option key={team.subteamNo} value={JSON.stringify(team)}>{team.subteamName}</option>
            )}

          </select>
        </div>
        <div className="subteam-buttons">
          <button className="irma-btn" value="shrink" onClick={handleClick} hidden={!user!.isShrink && !user!.isSuperUser && !user!.isCoordinator} disabled={!user!.isShrink && !user!.isSuperUser && !user!.isCoordinator}>Shrink</button>
          <button className="irma-btn" value="transfer" onClick={handleTransferClick} hidden={!user!.isBuyer && !user!.isSuperUser && !user!.isCoordinator} disabled={!user!.isBuyer && !user!.isSuperUser && !user!.isCoordinator}>Transfer</button>
          <button className="irma-btn" value="receive/PurchaseOrder" onClick={handleReceiveClick} hidden={!user!.isDistributor && !user!.isSuperUser && !user!.isCoordinator} disabled={!user!.isDistributor && !user!.isSuperUser && !user!.isCoordinator}>Receive</button>
        </div>
        <Modal
          open={alertIsOpen}
          header='IRMA Main Menu'
          content='Please select a subteam.'
          actions={['OK']}
          onActionClick={toggleAlert}

        />
        <BasicModal alert={alert} setAlert={setAlert} />
      </div>
    </Fragment>)
}

export default StoreFunctions;