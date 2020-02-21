import React, { useState, useContext, Fragment } from "react";
import ITransferItem from "../types/ITransferItem";
import ITransferData from "../types/ITransferData";
import { AppContext } from "../../../store";
import { useHistory } from "react-router-dom";
import BasicModal from "../../../layout/BasicModal";

const TransferUpdate: React.FC = () => {
  //@ts-ignore
  const { state } = useContext(AppContext);
  const [transferData] = useState<ITransferData>(JSON.parse(localStorage.getItem('transferData')!));
  const [transferItem] = useState<ITransferItem>(state.transferLineItem!);
  const [newQuantity, setNewQuanity] = useState<number>(1);
  let history = useHistory();
  const [alert, setAlert] = useState<any>({ open: false, alertMessage: '', type: 'default', header: 'IRMA Mobile', confirmAction: () => { }, cancelAction: () => { } });

  const updateQuantity = (e: any) => {
    let quantity = parseFloat(e.target.value);
    if (!transferItem.SoldByWeight) {
      setNewQuanity(parseInt(e.target.value.replace(/[^\w\s]|_/g, "")));
    } else {
      setNewQuanity(quantity);
    }
  }

  const handleUpdateClick = () => {
    if (newQuantity > 999) {
      setAlert({
        ...alert,
        open: true,
        alertMessage: `${transferItem.SoldByWeight ? 'Weight' : 'Quantity'} cannot be greater than 999`,
        type: 'default',
        header: 'Update Transfer'
      });
    } else if (newQuantity === 0) {
      setAlert({
        ...alert,
        open: true,
        alertMessage: `${transferItem.SoldByWeight ? 'Weight' : 'Quantity'} cannot be 0`,
        type: 'default',
        header: 'Update Transfer'
      });
    } else if (isNaN(newQuantity)) {
      setAlert({
        ...alert,
        open: true,
        alertMessage: `Please enter a numeric value for the ${transferItem.SoldByWeight ? 'Weight' : 'Quantity'}`,
        type: 'default',
        header: 'Update Transfer'
      });
    } else {
      const totalCost = transferItem.VendorCost !== 0 ? newQuantity * transferItem.VendorCost : newQuantity * transferItem.AdjustedCost;
      const newTransferItem = { ...transferItem, Quantity: newQuantity, TotalCost: totalCost };
      const transferItemIndex = transferData.Items.findIndex(i => i.Upc === transferItem.Upc);
      const newTransferData = { ...transferData };
      newTransferData.Items[transferItemIndex] = newTransferItem;
      localStorage.setItem('transferData', JSON.stringify(newTransferData));
      history.push('/transfer/review');
    }
  }
  const clearInvalid = (e: any) => {
    //android behavior fix, to stop invalid punctuation 
    if (e.target.value === '') {
      e.target.value = '';
    }
  }

  return (
    <Fragment>
      <div className="update-shrink-page">
        <h3>Update Transfer</h3>

        <section className="product-info">
          <div>
            <label>UPC:</label>
            <span>{transferItem.Upc}</span>
          </div>
          <div>
            <label>Desc:</label>
            <span>{transferItem.Description}</span>
          </div>
        </section>
        <section className="quantity-info">
          <div>
            <label>{`${transferItem.SoldByWeight ? "Weight Recorded" : "Quantity Recorded:"}`}</label>
            <span className="initial-quantity">{transferItem.Quantity}</span>
          </div>
          <div>
            <label>{`${transferItem.SoldByWeight ? "New Weight:" : "New Quantity:"}`}</label>
            <input
              type="number"
              value={newQuantity.toString()}
              min="1"
              maxLength={3}
              step={transferItem.SoldByWeight ? "any" : 1}
              onKeyPress={clearInvalid}
              onChange={updateQuantity}
            />
          </div>
          <div>
            <label>UOM:</label>
            <span>
              {transferItem.RetailUom}
            </span>
          </div>
        </section>
        <div className="update-buttons">
          <button className="wfm-btn" onClick={() => history.goBack()}>Cancel</button>
          <button className="wfm-btn" onClick={handleUpdateClick}>Update</button>
        </div>
      </div>
      <BasicModal alert={alert} setAlert={setAlert} />
    </Fragment>
  );
};

export default TransferUpdate;
