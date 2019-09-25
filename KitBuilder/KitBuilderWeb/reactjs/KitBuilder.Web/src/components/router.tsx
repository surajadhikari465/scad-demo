import * as React from 'react';
import { Route, HashRouter, Switch } from 'react-router-dom';
import '../css/site.css'
import { MuiThemeProvider } from '@material-ui/core';
import { createMuiTheme } from '@material-ui/core/styles';
import Header from './Header/header';
import Footer from './PageStyle/Footer';
import JssProvider from 'react-jss/lib/JssProvider';
import { createGenerateClassName } from '@material-ui/core/styles';
import LoginPage from './Login/Login'
import LinkGroupsPage from './LinkGroups/LinkGroupsPage';
import KitListPage from './Kits';
import InstructionListsPage from './InstructionLists/InstructionListsPage';
import AssignKitsToLocale from './AssignKits/AssignKitsToLocale';
import KitLinkGroupPage from './KitLinkGroups/KitLinkGroupsPage';
import withSnackbar from './PageStyle/withSnackbar';
import CreateKitPage from './Kits/CreateKitPage';
import ViewKit from './Kits/ViewKits/ViewKit';
//import EnsureLoggedInContainer from './RouterComponents/EnsureLoggedInContainer';

const generateClassName = createGenerateClassName({
  dangerouslyUseGlobalCSS: true
});

const theme = createMuiTheme({
  overrides: {
    MuiSelect: {
      select: {
        padding: 10,
      },
    },
    MuiOutlinedInput: {
      input: {
        padding: "10px !important",
      },
    },
    MuiFormControl: {
      root: {
        width: "100%",
      },
    },
  },
  props: {
    MuiInputLabel: {
      shrink: true,
    },
  },
  palette: {
    primary: {
      light: "#88BDBC",
      main: "#254E58",
      contrastText: "#fff",
    },
    secondary: {
      light: "#ff7961",
      main: "#f44336",
      dark: "#ba000d",
      contrastText: "#000"
    }
  }
});

class AppRouter extends React.Component {
  render() {
    return (
      <JssProvider generateClassName={generateClassName}>
        <HashRouter>
          <MuiThemeProvider theme={theme}>
            <div className="container-fluid">
              <Header />
              <Switch>         
                {/* <Route exact path="/Login" component={LoginPage} />
                <Route component={EnsureLoggedInContainer}  /> */}
                <Route exact path="/" component={InstructionListsPage} />
                <Route exact path="/Login" component={LoginPage} />
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
              </Switch>
              <Footer />
            </div>
          </MuiThemeProvider>
        </HashRouter>
      </JssProvider>
    );
  }
}

export default AppRouter;