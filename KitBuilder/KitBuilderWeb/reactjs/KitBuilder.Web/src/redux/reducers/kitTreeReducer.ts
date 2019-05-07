export type KitTreeState = {
        [kitId: number] : {
            [localeId: number]: boolean
        }
}

export default (state: KitTreeState = {}, action: any) => {
    switch(action.type) {
        case "TOGGLE_LOCALE":
            const {localeId, kitId} = action.payload;
            if(state[kitId]) {
                //toggleState
                state[kitId] = { 
                    ...state[kitId], 
                    [localeId]: !state[kitId][localeId]
                };
            } else {
                //add new kitId
                state[kitId] = {
                    [localeId]: true,
                } 
            }
            return { ...state };
        default:
            return state;
    }
}