import React, { Fragment, useEffect } from 'react';
import StoreFunctions from './pages/StoreFunctions';
import RegionSelect from './pages/RegionSelect';
import StoreSelect from './pages/StoreSelect';
import Shrink from './pages/Shrink';
import ReviewShrink from './pages/Shrink/ReviewShrink';
import UpdateShrink from './pages/Shrink/UpdateShrink';
import {
  BrowserRouter as Router,
  Switch,
  Route,
} from "react-router-dom";
import { faCog } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

import { AppContext, reducer, initialState, types, usePersistedReducer } from "./store";
import Receive from './pages/Receive/Index';
import ReceiveDocument from './pages/Receive/Document/ReceiveDocument';
import ReceivePurchaseOrder from './pages/Receive/PurchaseOrder/PurchaseOrder/ReceivePurchaseOrder';
import { ToastContainer } from 'react-toastify';
import MainMenu from './layout/MainMenu';
import InvoiceData from './pages/Receive/PurchaseOrder/InvoiceData/InvoiceData';
import ReceivingList from './pages/Receive/PurchaseOrder/ReceivingList/ReceivingList';
import ReceivingListClosePartial from './pages/Receive/PurchaseOrder/ReceivingList/components/ReceivingListClosePartial';
import TransferHome from './pages/Transfer/Index';
import ReceivePurchaseOrderDetailsClearScreen from './pages/Receive/PurchaseOrder/PurchaseOrder/components/ReceivePurchaseOrderDetailsClearScreen';


const App: React.FC = () => { 
  const [state, dispatch] = usePersistedReducer(reducer, initialState);
  const { menuItems, Title } = state;

  useEffect(() => {
    dispatch({ type: types.SETMENUITEMS, menuItems: [] });
  }, [dispatch]);

  useEffect(() => {
    dispatch({ type: types.SETTITLE, Title: 'IRMA Mobile' });
    return () => {
      dispatch({ type: types.SETTITLE, Title: 'IRMA Mobile' });
    };
  }, [dispatch]);

  return (
    <Fragment>
      <ToastContainer position="bottom-right" />
      {/* 
      // @ts-ignore */}
      <AppContext.Provider value={{ state, dispatch }}> 
        <Router>
          <wfm-toolbar>
            {Title === '' ? "IRMA Mobile" : Title}
              <wfm-button slot="end">
                <FontAwesomeIcon icon={faCog} />
              </wfm-button>
              <MainMenu disabled={menuItems && menuItems.length === 0}/>
          </wfm-toolbar>
          <div className="App">
            <Switch>
              <Route exact path="/" component={RegionSelect} />
              <Route exact path="/store" component={StoreSelect} />
              <Route exact path="/functions" component={StoreFunctions} />
              <Route exact path="/shrink" component={Shrink} />
              <Route exact path="/shrink/review" component={ReviewShrink} />
              <Route exact path="/shrink/update" component={UpdateShrink} />
              <Route exact path="/receive" component={Receive} />
              <Route exact path="/receive/Document" component={ReceiveDocument} />
              <Route exact path="/receive/PurchaseOrder/clearscreen" component={ReceivePurchaseOrderDetailsClearScreen} />
              <Route exact path="/receive/PurchaseOrder/:openOrderInformation?" component={ReceivePurchaseOrder} />
              <Route exact path="/receive/InvoiceData/:purchaseOrderNumber" component={InvoiceData} />
              <Route exact path="/receive/List/:purchaseOrderNumber" component={ReceivingList} />
              <Route exact path="/receive/List/:purchaseOrderNumber/closePartial" component={ReceivingListClosePartial} />
              <Route exact path="/transfer" component={TransferHome} />
            </Switch>
          </div>
        </Router>
      </AppContext.Provider>
    </Fragment>
  );
}

export default App;
