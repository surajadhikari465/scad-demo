import * as React from 'react';
import * as ReactDOM from 'react-dom';
import {default as AppRouter }  from './components/router';
import './index.css';
import '../../node_modules/bootstrap/dist/css/bootstrap.css'
import registerServiceWorker from './registerServiceWorker';

ReactDOM.render(
    <AppRouter />  ,  document.getElementById('root')
);
registerServiceWorker();
