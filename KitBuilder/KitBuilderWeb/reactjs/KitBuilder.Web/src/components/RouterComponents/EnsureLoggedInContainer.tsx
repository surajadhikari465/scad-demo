import * as React from 'react';
import { connect } from 'react-redux';
import { History, LocationState } from "history";
import { Dispatch } from 'redux';
import LinkGroupsPage from '../LinkGroups/LinkGroupsPage';
import KitListPage from '../Kits';
import { Route } from 'react-router-dom';
import InstructionListsPage from '../InstructionLists/InstructionListsPage';
import AssignKitsToLocale from '../AssignKits/AssignKitsToLocale';
import KitLinkGroupPage from '../KitLinkGroups/KitLinkGroupsPage';
import withSnackbar from '../PageStyle/withSnackbar';
import CreateKitPage from '../Kits/CreateKitPage';
import ViewKit from '../Kits/ViewKits/ViewKit';

interface IAppProps {
    dispatch: any,
    redirectUrl: any,
    isLoggedIn: boolean,
    currentURL:any,
    location:any,
    updateRedux(url:string): void;
    history: History<LocationState>;
}

interface IAppState {
    url:string
}

class EnsureLoggedInContainer extends React.Component <IAppProps,IAppState> {
    constructor(props: IAppProps) {
          super(props);
          this.state = {
           url : ""
        }
    }
    
    componentDidMount() {
      if (!this.props.isLoggedIn) {
        this.props.updateRedux(this.props.location.pathname);
        this.props.history.push("/login")
      }
    }
  
    render() {
       
      if (this.props.isLoggedIn) {
        return   <div>
                <Route exact path="/" component={InstructionListsPage} />
                <Route path="/Instructions" component={InstructionListsPage} />
                <Route path="/LinkGroups" component={LinkGroupsPage} />
                <Route path="/Kits" component={KitListPage} />
                <Route
                  path="/EditKit/:kitId"
                  component={withSnackbar(CreateKitPage)}
                />
                <Route path="/AssignKits" component={AssignKitsToLocale} />
                <Route
                  path="/CreateKits"
                  component={withSnackbar(CreateKitPage)}
                />
                <Route path="/KitLinkGroups" component={KitLinkGroupPage} />
                <Route path="/ViewKit" component={ViewKit} />
            </div> 
      } else {
        return null
        
      }
    }
  }
  
  // Grab a reference to the current URL. If this is a web app and you are
  // using React Router, you can use `ownProps` to find the URL. Other
  // platforms (Native) or routing libraries have similar ways to find
  // the current position in the app.
  function mapStateToProps(state: any) {
    return {
        isLoggedIn: state.logInReducer.loggedIn,
        currentURL:state.logInReducer.url
    }
  }

  const mapDispatchToProps = (dispatch: Dispatch) => ({
    updateRedux: (url:string) => dispatch({ type: "URL", payload: {url}}),
})

  export default connect(mapStateToProps,mapDispatchToProps)(EnsureLoggedInContainer)