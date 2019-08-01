export type KitTreeState = {
    [kitId: number]: {
        [localeId: number]: boolean
    }
}

export default (state: KitTreeState = {}, action: any) => {
    switch (action.type) {
        case "TOGGLE_LOCALE":
            const { localeId, kitId, data } = action.payload;

            if (state[kitId]) {
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
        default:
            return state;
    }
}