export type KitTreeState = {
    resetLocale: boolean,
    [kitId: number]: {
        [localeId: number]: boolean
    }
}

export default (state: KitTreeState = { resetLocale: false }, action: any) => {
    switch (action.type) {      
        case "TOGGLE_LOCALE":
            const { localeId, kitId, data } = action.payload;
            if (state[kitId] ) {
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

                if (typeof data[0] === 'undefined') {
                    console.log("passed data is undefined")
                }
                else {
                    data[0].childs.filter((s: any) => s.parentLocaleId == localeId).forEach((function (element: any) {
                        state[kitId] =
                            {
                                ...state[kitId],
                                [localeId]: !state[kitId][element.localeId]
                            };
                    }));
                }
            }
            return { ...state };
        case "RESET_LOCALE":
            const { resetlocaleId, selectedKitId, localeTreedata, resetLocale, chainId, regionId, metroId, storeId} = action.payload;
            if (resetLocale)
            {
                state[selectedKitId] = [];
                if (typeof localeTreedata[0] === 'undefined') {
                    console.log("passed data is undefined")
                }
                else {
                        state[selectedKitId] =
                        {
                            ...state[selectedKitId],
                            [resetlocaleId]: true
                        };
                    
                        if (chainId != null)
                        {
                            state[selectedKitId] =
                            {
                            ...state[selectedKitId],
                            [chainId]: true
                            };
                        }
                        
                        if (regionId != null)
                        {
                            state[selectedKitId] =
                            {
                            ...state[selectedKitId],
                            [regionId]: true
                            };
                        }

                        if (metroId != null)
                        {
                            state[selectedKitId] =
                            {
                            ...state[selectedKitId],
                            [metroId]: true
                            };
                        }

                        if (storeId != null)
                        {
                            state[selectedKitId] =
                            {
                            ...state[selectedKitId],
                            [storeId]: true
                            };
                        }
                    }
                }
    
            return { ...state };
        default:
            return state;
    }
}