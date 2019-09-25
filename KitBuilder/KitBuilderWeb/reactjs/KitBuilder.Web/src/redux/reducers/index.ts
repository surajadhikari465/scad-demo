import { combineReducers } from 'redux';
import kitTreeReducer from './kitTreeReducer';
import loginReducer from './logInReducer';

//const rootReducer = (state= { kitTree: {} }, action: any) => ({
//    kitTree: kitTreeReducer(state.kitTree, action)
//})

const rootReducer = combineReducers({
    kitTree: kitTreeReducer,
    logInReducer: loginReducer
});

export default rootReducer;