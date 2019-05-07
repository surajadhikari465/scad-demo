import * as React from 'react';
import * as ReactDOM from 'react-dom';
import {default as AppRouter }  from './components/router';
import './index.css';
import '../../node_modules/bootstrap/dist/css/bootstrap.css'
import registerServiceWorker from './registerServiceWorker';
import { createStore } from 'redux';
import { Provider } from 'react-redux';
import rootReducer from './redux/reducers';

const store = createStore(rootReducer);

ReactDOM.render(
    <Provider store={store}>
        <AppRouter /> 
    </Provider> ,  document.getElementById('root')
);
registerServiceWorker();
