export type loggedInState = {
    loggedIn : boolean,
    url:string
}

export default (state: loggedInState = { loggedIn: false, url:""}, action: any) => {
    switch (action.type) {
        case "LOGGED_IN":
            return state={
                loggedIn:action.payload.loggedIn,
                url:state.url
            }
            case "URL":
                return state={
                    loggedIn:state.loggedIn,
                    url:action.payload.url
                }
        default:
            return state;
    }
}