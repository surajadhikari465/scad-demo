import React, { Fragment, useContext, useState } from 'react';
import { Link } from "react-router-dom";
import { AppContext, types } from "../../../store";
import BasicModal from '../../../layout/BasicModal';
import './styles.scss';

interface UpdateShrinkProps {
  history: any;
}

const UpdateShrink:React.FC<UpdateShrinkProps> = (props) => {
    // @ts-ignore
    const {state, dispatch} = useContext(AppContext);
    const {selectedShrinkItem} = state;
    const [alert, setAlert] = useState<any>({open:false, alertMessage:'', type: 'default', header: 'IRMA Mobile', confirmAction:()=> {}, cancelAction:()=> {}});
    const [initialQuantity] = useState(selectedShrinkItem.quantity);
    const [shrinkType] = useState(selectedShrinkItem.shrinkType);
    const {history} = props;
    const updateQuantity = (e:any) =>{
      let quantity = parseInt(e.target.value);
      dispatch({ type: types.SETSELECTEDSHRINKITEM, selectedShrinkItem: {...selectedShrinkItem, quantity} });
    }
    const changeSubtype = (e:any) =>{
      let shrinkType = e.target.value;
      dispatch({ type: types.SETSELECTEDSHRINKITEM, selectedShrinkItem: {...selectedShrinkItem, shrinkType} });
    }
    const update = () =>{
      if(selectedShrinkItem.quantity>999){
        setAlert({...alert, 
          open:true, 
          alertMessage: 'Quantity cannot be greater than 999',
          type: 'default', 
          header:'Update Shrink'});
      }else if(selectedShrinkItem.quantity===0){
        setAlert({...alert, 
          open:true, 
          alertMessage: 'Quantity cannot be 0',
          type: 'default', 
          header:'Update Shrink'});
      } else {
        let shrinkItems = state.shrinkItems.map((shrinkItem:any)=>shrinkItem.identifier === selectedShrinkItem.identifier? selectedShrinkItem: shrinkItem);
        dispatch({ type: types.SETSHRINKITEMS, shrinkItems: shrinkItems }); 
        localStorage.setItem("shrinkItems", JSON.stringify(shrinkItems));
        history.push('/shrink/review');
      }
    }
    const checkQty = (e:any) =>{
      if(selectedShrinkItem.costedByWeight && (e.charCode < 48 || e.charCode > 57)){
          e.preventDefault();
      }
    }
    const {shrinkTypes} = state;
    return (  
      <Fragment>
        <div className='update-shrink-page'>
          <h3>Update Shrink</h3>

          <section className='product-info'>
            <div>
              <label>UPC:</label>
              <span>{selectedShrinkItem.identifier}</span> 
            </div>
            <div>
              <label>Desc:</label>
              <span>{selectedShrinkItem.signDescription}</span>
            </div>
          </section>
          <section className='quantity-info'>
            <div>
              <label>Quantity Recorded:</label>
              <span className="initial-quantity">{initialQuantity}</span>
            </div>
            <div>
              <label>New Quantity:</label>
              <input type='number' 
                    value={selectedShrinkItem.quantity.toString()} 
                    min="1" 
                    maxLength={3}
                    step={selectedShrinkItem.costedByWeight ? 1:'any'}
                    onKeyPress={checkQty}  
                    onChange={updateQuantity}/>
            </div>
            <div>
              <label>UOM:</label>
              <span>{selectedShrinkItem.packageDesc1}/{selectedShrinkItem.packageDesc2} {selectedShrinkItem.packageUnitAbbreviation}</span>
            </div>
          </section> 
          <section className='subtype-info'>
            <label>Sub Type:</label>
            <select defaultValue={shrinkType} onChange={changeSubtype}>
              <option>--Select Sub Type--</option>
              {shrinkTypes.map(team =>
              // @ts-ignore 
                <option key={team.shrinkSubTypeId} value={team.shrinkSubTypeMember}>{team.shrinkSubTypeMember}</option>
              )}
            </select>
          </section>
          <div className="update-buttons">
            <button className="wfm-btn" ><Link to="/shrink/review">Cancel</Link></button>
            <button className="wfm-btn" onClick={update}>Update</button>
          </div>
        </div>
        <BasicModal alert={alert} setAlert={setAlert}/>  
      </Fragment>
    )
};

export default UpdateShrink;