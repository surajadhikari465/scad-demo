import React, { Fragment } from 'react';
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
import TransferScan from './pages/Transfer/Scan/TransferScan';
import TransferReview from './pages/Transfer/Review/TransferReview';
import TransferLineItemDetails from './pages/Transfer/LineItemDetails/TransferLineItemDetails';
import TransferUpdate from './pages/Transfer/Update/TransferUpdate';
import TransferIndexNewOrder from './pages/Transfer/Index/components/TransferIndexNewOrder';
import TransferScanDeleteOrder from './pages/Transfer/Scan/components/TransferScanDeleteOrder';
import TransferScanSaveOrder from './pages/Transfer/Scan/components/TransferScanSaveOrder';


const App: React.FC = () => {
  const [state, dispatch] = usePersistedReducer(reducer, initialState);
  const { menuItems, settingsItems, Title } = state;
  return (
    <Fragment>
      <ToastContainer position="bottom-right" />
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
              <Route exact path="/receive" component={Receive} />
              <Route exact path="/receive/Document" component={ReceiveDocument} />
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
