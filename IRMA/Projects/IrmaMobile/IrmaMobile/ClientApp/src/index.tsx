import 'react-toastify/dist/ReactToastify.min.css'
import React from 'react';
import ReactDOM from 'react-dom';
import LogRocket from 'logrocket';
//@ts-ignore
import setupLogRocketReact from 'logrocket-react';
import App from './App';
import * as serviceWorker from './serviceWorker';
import 'semantic-ui-css/semantic.min.css';
import './index.scss';
import 'react-table/react-table.css';
import { logRocketInitOptions } from './logger';
import config from './config';

/**
 * LogRocket Setup
 */
const { logRocketId } = config;

if (logRocketId) {
  // Allow React components to retain their names during logging
  setupLogRocketReact(LogRocket);
  // Initialize LogRocket
  LogRocket.init(logRocketId, logRocketInitOptions);
}  

ReactDOM.render(<App />, document.getElementById('root'));

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://bit.ly/CRA-PWA
serviceWorker.unregister();
