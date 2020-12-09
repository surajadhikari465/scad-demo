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
import { AppContext, reducer, initialState, usePersistedReducer } from "./store";
import ReceivePurchaseOrder from './pages/Receive/PurchaseOrder/PurchaseOrder/ReceivePurchaseOrder';
import { ToastContainer } from 'react-toastify';
import MainMenu from './layout/MainMenu';
import InvoiceData from './pages/Receive/PurchaseOrder/InvoiceData/InvoiceData';
import ReceivingList from './pages/Receive/PurchaseOrder/ReceivingList/ReceivingList';
import ReceivingListClosePartial from './pages/Receive/PurchaseOrder/ReceivingList/components/ReceivingListClosePartial';
import TransferHome from './pages/Transfer/Index';
import ReceivePurchaseOrderDetailsClearScreen from './pages/Receive/PurchaseOrder/PurchaseOrder/components/ReceivePurchaseOrderDetailsClearScreen';
import TransferScan from './pages/Transfer/Scan/TransferScan';
import TransferReview from './pages/Transfer/Review/TransferReview';
import TransferLineItemDetails from './pages/Transfer/LineItemDetails/TransferLineItemDetails';
import TransferUpdate from './pages/Transfer/Update/TransferUpdate';
import TransferIndexNewOrder from './pages/Transfer/Index/components/TransferIndexNewOrder';
import TransferScanDeleteOrder from './pages/Transfer/Scan/components/TransferScanDeleteOrder';
import TransferScanSaveOrder from './pages/Transfer/Scan/components/TransferScanSaveOrder';
//@ts-ignore
import { LifecycleManager, AuthHandler } from '@wfm/mobile';
//@ts-ignore
import decode from 'jwt-decode';
import Config from './config';
import LogRocket from 'logrocket';
import { identifyLogRocketUser } from './logger';

const App: React.FC = () => {
  //@ts-ignore
  const [state, dispatch] = usePersistedReducer(reducer, initialState);
  const { menuItems, settingsItems, Title } = state;

  //register authorization
  useEffect(() => {
    if (Config.useAuthToken) {
      LifecycleManager.onReady(function () {
        try {
          AuthHandler.onTokenReceived(function (token: string) {
            let decodedToken = decode(token);
            localStorage.setItem('authToken', JSON.stringify(decodedToken));
            identifyLogRocketUser(decodedToken);
          });
        } catch (err) {
          console.error(err);
        }
      });
    } else {
      localStorage.setItem('authToken', JSON.stringify(Config.fakeUser));
    }
  }, []);

  return (
    <Fragment>
      <ToastContainer position="top-right" autoClose={3000} />
      {/* 
      // @ts-ignore */}
      <AppContext.Provider value={{ state, dispatch }}>
        <Router>
          <wfm-toolbar>
            {Title === '' ? "IRMA Mobile" : Title}
            <MainMenu disabled={menuItems && menuItems.length === 0} menuItems={menuItems} icon="bars" slot="start" />
            <MainMenu disabled={!state.showCog} style={!state.showCog ? { visibility: 'hidden' } : {}} menuItems={settingsItems} icon="cog" slot="end" />
          </wfm-toolbar>
          <div className="App">
            <Switch>
              <Route exact path="/" component={RegionSelect} />
              <Route exact path="/store" component={StoreSelect} />
              <Route exact path="/functions" component={StoreFunctions} />
              <Route exact path="/shrink" component={Shrink} />
              <Route exact path="/shrink/review" component={ReviewShrink} />
              <Route exact path="/shrink/update" component={UpdateShrink} />
              <Route exact path="/receive/PurchaseOrder/clearscreen" component={ReceivePurchaseOrderDetailsClearScreen} />
              <Route exact path="/receive/PurchaseOrder/:openOrderInformation?" component={ReceivePurchaseOrder} />
              <Route exact path="/receive/InvoiceData/:purchaseOrderNumber" component={InvoiceData} />
              <Route exact path="/receive/List/:purchaseOrderNumber" component={ReceivingList} />
              <Route exact path="/receive/List/:purchaseOrderNumber/closePartial" component={ReceivingListClosePartial} />
              <Route exact path="/transfer/index/:comingFromScan?" component={TransferHome} />
              <Route exact path="/transfer/scan" component={TransferScan} />
              <Route exact path="/transfer/review" component={TransferReview} />
              <Route exact path="/transfer/viewDetails" component={TransferLineItemDetails} />
              <Route exact path="/transfer/update" component={TransferUpdate} />
              <Route exact path="/transfer/newOrder" component={TransferIndexNewOrder} />
              <Route exact path='/transfer/scan/deleteOrder' component={TransferScanDeleteOrder} />
              <Route exact path='/transfer/scan/saveOrder' component={TransferScanSaveOrder} />
            </Switch>
          </div>
        </Router>
      </AppContext.Provider>
    </Fragment>
  );
}

export default App;
