import * as React from 'react';
import { loggedInState } from 'src/redux/reducers/logInReducer';
import { connect } from 'react-redux';
import { Redirect } from 'react-router';


interface IAppProps {
    dispatch: any,
    redirectUrl: any,
    isLoggedIn: loggedInState,
    children:any
}


class App1 extends React.Component<IAppProps> {
  constructor(props: IAppProps) {
        super(props);
  }

  componentDidUpdate(prevProps: any) {
    //const { dispatch, redirectUrl } = this.props
    alert(JSON.stringify(prevProps));
    const isLoggingOut = prevProps.isLoggedIn.loggedIn && !this.props.isLoggedIn.loggedIn
    const isLoggingIn = !prevProps.isLoggedIn.loggedIn && this.props.isLoggedIn.loggedIn
    alert(isLoggingIn);
    if (isLoggingIn) {
      //dispatch(navigateTo(redirectUrl))
 
      <Redirect to = './Login/Login' />
    } else if (isLoggingOut) {
      // do any kind of cleanup or post-logout redirection here
    }
  }

  render() {
    return this.props.isLoggedIn.loggedIn
  }
}

function mapStateToProps(state: any) {
  return {
    isLoggedIn: state.logInReducer
  }
}

export default connect(mapStateToProps)(App1)