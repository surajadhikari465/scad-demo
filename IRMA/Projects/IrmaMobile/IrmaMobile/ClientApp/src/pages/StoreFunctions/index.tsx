import React, { useContext } from 'react';
import './styles.scss';
import { AppContext, types, ITeam } from "../../store";
import {Link} from "react-router-dom";

const StoreFunctions:React.FC = () => {
    // @ts-ignore 
    const {state, dispatch} = useContext(AppContext);
    const {subteams, subteam} = state;
    const handleClick = () =>{
      if(!subteam){
        alert('Please select a subteam');
        return false;
      }
      return true;
    }
    const setSubteam = (value:any) =>{
      const subteam: any = state.subteams.filter((subteam: ITeam)=> value === subteam.subteamName.trim())[0];
      dispatch({ type: types.SETSUBTEAM , subteam: subteam });  
      dispatch({ type: types.SETSUBTEAMNO, subteamNo:subteam.subteamNo });
    }
    return (
    <div className="store-functions">
      <div className="page-title-wrapper">   
        <div className="message-container">
          <h1>Scan Items</h1>
          <p>Set your subteam, then select a function</p>
          <p>Having trouble? Enter the number manually by clicking the settings icon in the above toolbar</p>
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
        <button className="wfmButton" onClick={handleClick}>{subteam ? <Link to={'/shrink'}>Shrink</Link>:'Shrink'}</button>
        <button className="wfmButton" onClick={handleClick}><Link to={'/transfer'}>Transfer</Link></button>
        <button className="wfmButton" onClick={handleClick}>{subteam ? <Link to={'/receive'}>Receive</Link>:'Receive'}</button>
      </div>
    </div>)
  }

  export default StoreFunctions;