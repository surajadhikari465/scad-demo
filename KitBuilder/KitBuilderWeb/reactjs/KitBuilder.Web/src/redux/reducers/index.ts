import kitTreeReducer from './kitTreeReducer';

const rootReducer = (state= { kitTree: {} }, action: any) => ({
    kitTree: kitTreeReducer(state.kitTree, action)
})

export default rootReducer;