import React, { useContext, useState } from 'react';
import './styles.scss';
import { Modal } from 'semantic-ui-react'
import { AppContext, types, ITeam } from "../../store";

interface StoreFunctionsProps {
  history: any;
}

const StoreFunctions:React.FC<StoreFunctionsProps> = (props) => {
    // @ts-ignore 
    const {state, dispatch} = useContext(AppContext);
    const [isSelected, setSelected] = useState(false);
    const [alertIsOpen, setAlertOpen] = useState(false);
    const {subteams} = state;
    const {history} = props;
    const handleClick = (e:any) =>{
      if(!isSelected){
        setAlertOpen(true);
      }
      else history.push(`/${e.target.value}`);
    }
    const toggleAlert = (e:any) =>{
        setAlertOpen(!alertIsOpen);
    }
    const setSubteam = (value:any) =>{
      const subteam: any = state.subteams.filter((subteam: ITeam)=> value === subteam.subteamName.trim())[0];
      setSelected(true);
      dispatch({ type: types.SETSUBTEAM , subteam: subteam });  
      dispatch({ type: types.SETSUBTEAMNO, subteamNo:subteam.subteamNo });
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
            <option key={team.subteamNo}>{team.subteamName}</option>
          )}
          
        </select>
      </div>
      <div className="subteam-buttons">
        <button className="wfm-btn" value="shrink" onClick={handleClick}>Shrink</button>
        <button className="wfm-btn" value="transfer" onClick={handleClick}>Transfer</button>
        <button className="wfm-btn" value="receive" onClick={handleClick}>Receive</button>
      </div>
      <Modal
          open={alertIsOpen}
          header='IRMA Mobile'
          content='Please select a subteam'
          actions={['OK']}
          onActionClick={toggleAlert}

        />
    </div>)
  }

  export default StoreFunctions;