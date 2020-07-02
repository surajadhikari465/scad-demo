import React, { Fragment, useContext, useEffect, useState } from 'react';
import { Link } from "react-router-dom";
import { AppContext, types, IShrinkSession } from "../../../store";
import BasicModal from '../../../layout/BasicModal';
import './styles.scss';
import useNumberInput from '../../../hooks/useNumberInput';
import { Textfit } from 'react-textfit';

interface UpdateShrinkProps {
  history: any;
}

const UpdateShrink: React.FC<UpdateShrinkProps> = (props) => {
  // @ts-ignore
  const { state, dispatch } = useContext(AppContext);
  const { selectedShrinkItem, user, shrinkSessions } = state;
  const [alert, setAlert] = useState<any>({ open: false, alertMessage: '', type: 'default', header: 'IRMA Mobile', confirmAction: () => { }, cancelAction: () => { } });
  const [initialQuantity] = useState(selectedShrinkItem.quantity);
  const [initialShrinkType] = useState(selectedShrinkItem.shrinkType);
  const [shrinkType] = useState(selectedShrinkItem.shrinkType);
  const { history } = props;
  const [sessionIndex] = useState<number>(shrinkSessions.findIndex((s: IShrinkSession) => s.sessionUser.userName === user?.userName));
  const quantityInput = useNumberInput(0, '', selectedShrinkItem.costedByWeight || selectedShrinkItem.soldByWeight, false, '');

  useEffect(() => {
    dispatch({ type: types.SETTITLE, Title: 'Update Shrink' });

  }, [dispatch]);

  useEffect(() => {
    dispatch({ type: types.SETSELECTEDSHRINKITEM, selectedShrinkItem: { ...selectedShrinkItem, quantity: quantityInput.value } });
  }, [quantityInput.value])

  const updateQuantity = (e: React.ChangeEvent<HTMLInputElement>) => {
    quantityInput.setValueInput(e.target.value);
  }

  const changeSubtype = (e: any) => {
    let shrinkType = e.target.value;
    dispatch({ type: types.SETSELECTEDSHRINKITEM, selectedShrinkItem: { ...selectedShrinkItem, shrinkType } });
  }

  const update = () => {
    if (quantityInput.value > 999) {
      setAlert({
        ...alert,
        open: true,
        alertMessage: `${selectedShrinkItem.costedByWeight || selectedShrinkItem.soldByWeight ? 'Weight' : 'Quantity'} cannot be greater than 999`,
        type: 'default',
        header: 'Update Shrink'
      });
    } else if (quantityInput.value === 0) {
      setAlert({
        ...alert,
        open: true,
        alertMessage: `${selectedShrinkItem.costedByWeight || selectedShrinkItem.soldByWeight ? 'Weight' : 'Quantity'} cannot be 0`,
        type: 'default',
        header: 'Update Shrink'
      });
    } else if (isNaN(quantityInput.value)) {
      setAlert({
        ...alert,
        open: true,
        alertMessage: `Please enter a numeric value for the ${selectedShrinkItem.costedByWeight || selectedShrinkItem.soldByWeight ? 'Weight' : 'Quantity'}`,
        type: 'default',
        header: 'Update Shrink'
      });
    } else {
      let shrinkItems = state.shrinkSessions[sessionIndex].shrinkItems.map((shrinkItem: any) => shrinkItem.identifier === selectedShrinkItem.identifier && initialShrinkType === shrinkItem.shrinkType ? selectedShrinkItem : shrinkItem);
      let shrinkSessionsCopy = { ...state.shrinkSessions[sessionIndex], shrinkItems: shrinkItems };
      dispatch({ type: types.SETSHRINKSESSIONS, shrinkSessions: [...state.shrinkSessions.slice(0, sessionIndex), shrinkSessionsCopy, ...state.shrinkSessions.slice(sessionIndex + 1)] });
      history.push('/shrink/review');
    }
  }

  const { shrinkTypes } = state;

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
            <label>{`${selectedShrinkItem.costedByWeight || selectedShrinkItem.soldByWeight ? 'Weight Recorded' : 'Quantity Recorded:'}`}</label>
            <span className="initial-quantity">{initialQuantity}</span>
          </div>
          <div>
            <label>{`${selectedShrinkItem.costedByWeight || selectedShrinkItem.soldByWeight ? 'New Weight:' : 'New Quantity:'}`}</label>
            <input type='number'
              value={quantityInput.valueInput}
              min="1"
              maxLength={3}
              step={selectedShrinkItem.costedByWeight || selectedShrinkItem.soldByWeight ? 'any' : 1}
              onKeyDown={(e: any) => e.key === 'Enter' ? e.target.blur() : ''}
              onChange={updateQuantity} />
            <Textfit className="error-message" mode='multi' min={8} max={12}>{quantityInput.errorMessage}</Textfit>
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
              <option key={team.shrinkSubTypeId} value={team.shrinkSubTypeMember}>{team.shrinkSubTypeMember}</option>
            )}
          </select>
        </section>
        <div className="update-buttons">
          <button className="irma-btn"><Link to="/shrink/review">Cancel</Link></button>
          <button className="irma-btn" onClick={update} disabled={quantityInput.errorMessage !== '' || quantityInput.value === 0}>Update</button>
        </div>
      </div>
      <BasicModal alert={alert} setAlert={setAlert} />
    </Fragment>
  )
};

export default UpdateShrink;