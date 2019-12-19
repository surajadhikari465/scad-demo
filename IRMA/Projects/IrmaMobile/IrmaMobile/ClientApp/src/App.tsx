import React, { useReducer, Fragment, useEffect } from 'react';
import StoreFunctions from './pages/StoreFunctions';
import RegionSelect from './pages/RegionSelect';
import StoreSelect from './pages/StoreSelect';
import Shrink from './pages/Shrink';
import {
  BrowserRouter as Router,
  Switch,
  Route,
} from "react-router-dom";
import { faCog } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

import { AppContext, reducer, initialState, types } from "./store";
import Receive from './pages/Receive/Index';
import ReceiveDocument from './pages/Receive/Document/ReceiveDocument';
import ReceivePurchaseOrder from './pages/Receive/PurchaseOrder/ReceivePurchaseOrder';
import { ToastContainer } from 'react-toastify';
import MainMenu from './layout/MainMenu';


const App: React.FC = () => { 
  const [state, dispatch] = useReducer(reducer, initialState);
  const { menuItems } = state;

  useEffect(() => {
    dispatch({ type: types.SETMENUITEMS, menuItems: [] });
  }, []);

  return (
    <Fragment>
      <ToastContainer position="bottom-right" />
      {/* 
      // @ts-ignore */}
      <AppContext.Provider value={{ state, dispatch }}> 
        <Router>
          <wfm-toolbar>
            IRMA Mobile
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
              <Route exact path="/receive" component={Receive} />
              <Route exact path="/receive/Document" component={ReceiveDocument} />
              <Route exact path="/receive/PurchaseOrder" component={ReceivePurchaseOrder} />
            </Switch>
          </div>
        </Router>
      </AppContext.Provider>
    </Fragment>
  );
}

export default App;
