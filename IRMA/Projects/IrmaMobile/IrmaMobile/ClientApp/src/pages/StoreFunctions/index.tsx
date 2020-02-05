import React, { useContext, useState, useEffect } from 'react';
import './styles.scss';
import { Modal } from 'semantic-ui-react';
import BasicModal from '../../layout/BasicModal';
import { AppContext, types, IMenuItem } from "../../store";

interface StoreFunctionsProps {
  history: any;
}

const StoreFunctions:React.FC<StoreFunctionsProps> = (props) => {
    // @ts-ignore 
    const {state, dispatch} = useContext(AppContext);
    const [isSelected, setSelected] = useState(false);
    const [alertIsOpen, setAlertOpen] = useState(false);
    const [alert, setAlert] = useState<any>({open:false, alertMessage:'', type: 'default', header: 'IRMA Mobile', confirmAction:()=> {}});
    const {subteams} = state;
    const {history} = props;

    useEffect(() => {
      dispatch({ type: types.TOGGLECOG, showCog: true }); 
      const settingsItems = [
        { id: 1, order: 0, text: "Change Store", path: "/", disabled: false } as IMenuItem
      ] as IMenuItem[];

      dispatch({ type: types.SETSETTINGSITEMS, settingsItems: settingsItems });
      return () => {
        dispatch({ type: types.TOGGLECOG, showCog: false }); 
      };
    }, [dispatch]); 

    const deleteSession = () =>{
      localStorage.removeItem('shrinkItems');
      localStorage.removeItem('sessionSubType');
      dispatch({ type: types.SETSHRINKITEMS, shrinkItems: [] }); 
      dispatch({ type: types.SETSHRINKTYPE, shrinkType: {} }); 
      dispatch({ type: types.SETSUBTEAMSESSION, subteamSession: {...state.subteamSession, isPrevSession: false}});
      setAlert({...alert, 
        open:false
      });
      history.push('/shrink');
    }

    const deleteWarning = () =>{
        setAlert({...alert, 
          open:true, 
          alertMessage: 'Are you sure you want to delete you saved Session?', 
          type: 'confirm', 
          header:'Delete Session?',
          confirmAction: deleteSession
        });
    }

    const confirm = () =>{
      setAlert({...alert, 
        open:false
      });
      dispatch({ type: types.SETSUBTEAMSESSION, subteamSession: {...state.subteamSession, isPrevSession: true}}); 
      history.push('/shrink');
    }
    
    const checkLocalStorage = () =>{
        setAlert({...alert, 
          open:true, 
          alertMessage: 'Would you like to reload your previous Session? Clicking No will delete the old session.', 
          type: 'confirm', 
          header:'Previous Session Exists',
          cancelAction: deleteWarning,
          confirmAction: confirm
          });
    }

    const handleClick = (e:any) =>{
      if(!isSelected){
        setAlertOpen(true);
      }
      else {
        let shrinkItems = [];
        if(localStorage.getItem('shrinkItems')){
          // @ts-ignore
          shrinkItems = JSON.parse(localStorage.getItem('shrinkItems'));
        }
        if(shrinkItems.length > 0 && e.target.value === 'shrink'){
          checkLocalStorage();
        } else history.push(`/${e.target.value}`);
      }
    }

    const handleTransferClick = () => {
      if(!isSelected) {
        setAlertOpen(true);
      } else {
        history.push('/transfer/index/');
      }
    }

    const toggleAlert = (e:any) =>{
        setAlertOpen(!alertIsOpen);
    }
    const setSubteam = (value:any) =>{
      setSelected(true);
      dispatch({ type: types.SETSUBTEAM , subteam: JSON.parse(value) });  
      dispatch({ type: types.SETSUBTEAMNO, subteamNo:JSON.parse(value).subteamNo });
    }
    return (
    <div className="store-functions">
      <div className="page-title-wrapper">   
        <div className="message-container">
          <p>Set your subteam, then select a function</p>
        </div>
      </div>
      <div className="subteam-select">
        <select onChange={(e)=> setSubteam(e.target.value)} >
          <option>--Select Subteam--</option>
          {subteams.map(team =>
          // @ts-ignore 
            <option key={team.subteamNo} value={JSON.stringify(team)}>{team.subteamName}</option>
          )}
          
        </select>
      </div>
      <div className="subteam-buttons">
        <button className="wfm-btn" value="shrink" onClick={handleClick}>Shrink</button>
        <button className="wfm-btn" value="transfer" onClick={handleTransferClick}>Transfer</button>
        <button className="wfm-btn" value="receive" onClick={handleClick}>Receive</button>
      </div>
      <Modal
          open={alertIsOpen}
          header='IRMA Mobile'
          content='Please select a subteam'
          actions={['OK']}
          onActionClick={toggleAlert}

        />
      <BasicModal alert={alert} setAlert={setAlert}/>  
    </div>)
  }

  export default StoreFunctions;