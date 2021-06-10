import React, { Fragment, useContext, useState, useEffect } from 'react';
import './styles.scss';
import { Modal } from 'semantic-ui-react';
import BasicModal from '../../layout/BasicModal';
import CurrentLocation from "../../layout/CurrentLocation";
import { AppContext, types, IMenuItem } from "../../store";
import { AuthHandler } from '@wfm/mobile';
import { toast } from 'react-toastify';
import { initializeLogRocket } from '../../logger';

interface StoreFunctionsProps {
  history: any;
}

const StoreFunctions: React.FC<StoreFunctionsProps> = (props) => {
  // @ts-ignore 
  const { state, dispatch } = useContext(AppContext);
  const [isSelected, setSelected] = useState(false);
  const [alertIsOpen, setAlertOpen] = useState(false);
  const [alert, setAlert] = useState<any>({ open: false, alertMessage: '', type: 'default', header: 'IRMA Mobile', confirmAction: () => { } });
  const { subteams, user } = state;
  const { history } = props;

  useEffect(() => {
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

  const deleteWarning = () => {
    setAlert({
      ...alert,
      open: true,
      alertMessage: 'Are you sure you want to delete you saved Session?',
      type: 'confirm',
      header: 'Delete Session?',
      confirmAction: () => {
        dispatch({ type: types.SETSHRINKTYPE, shrinkType: {} });
        const shrinkSessions = state.shrinkSessions;
        const shrinkSessionsCopy = shrinkSessions.filter(s => s.sessionUser.userName !== user?.userName);
        shrinkSessionsCopy.push({ shrinkItems: [], isPrevSession: false, sessionShrinkType: '', sessionNumber: 0, sessionSubteam: undefined, sessionStore: '', sessionRegion: '', sessionUser: user, forceSubteamSelection: true });
        dispatch({ type: types.SETSHRINKSESSIONS, shrinkSessions: shrinkSessionsCopy });
        history.push('/shrink');
      }
    });
  }

  const handleShrinkClick = () => {
    initializeLogRocket()
    if (!isSelected) {
      setAlertOpen(true);
    }
    else {
      const shrinkSession = state.shrinkSessions.find(session => session.sessionUser.userName === user?.userName);
      if (shrinkSession && shrinkSession.shrinkItems && shrinkSession.shrinkItems.length > 0) {
        setAlert({
          ...alert,
          open: true,
          alertMessage: `Would you like to reload your previous Session? (${user?.userName}) for (${shrinkSession.sessionSubteam?.subteamName}) Clicking No will delete the old session.`,
          type: 'confirm',
          header: 'Previous Session Exists',
          cancelAction: deleteWarning,
          confirmAction: () => {
            dispatch({ type: types.SETSUBTEAM, subteam: shrinkSession.sessionSubteam });
            dispatch({ type: types.SETSUBTEAMNO, subteamNo: shrinkSession.sessionSubteam?.subteamNo });
            const shrinkSessionCopy = { ...shrinkSession, isPrevSession: true };
            const shrinkSessions = state.shrinkSessions;
            const shrinkSessionsCopy = shrinkSessions.filter(s => s.sessionUser.userName !== user?.userName);
            shrinkSessionsCopy.push(shrinkSessionCopy);
            dispatch({ type: types.SETSHRINKSESSIONS, shrinkSessions: shrinkSessionsCopy });
            history.push('/shrink');
          }
        });
      } else {
        const shrinkSessions = state.shrinkSessions;
        const shrinkSessionsCopy = shrinkSessions.filter(s => s.sessionUser.userName !== user?.userName);
        shrinkSessionsCopy.push({ shrinkItems: [], isPrevSession: false, sessionShrinkType: '', sessionNumber: 0, sessionSubteam: undefined, sessionStore: '', sessionRegion: '', sessionUser: user, forceSubteamSelection: true });
        dispatch({ type: types.SETSHRINKSESSIONS, shrinkSessions: shrinkSessionsCopy });
        history.push('/shrink');
      }
    }
  }

  const handleReceiveClick = () => {
    initializeLogRocket()
    history.push('/receive/PurchaseOrder/');
  }

  const handleTransferClick = () => {
    initializeLogRocket()
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
          <button className="irma-btn" value="shrink" onClick={handleShrinkClick} hidden={!user!.isShrink && !user!.isSuperUser && !user!.isCoordinator} disabled={!user!.isShrink && !user!.isSuperUser && !user!.isCoordinator}>Shrink</button>
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