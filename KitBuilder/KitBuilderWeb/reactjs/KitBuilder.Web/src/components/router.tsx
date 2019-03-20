import * as React from 'react';
import { Route, HashRouter, Switch } from 'react-router-dom';
import { LinkGroupsPage } from './LinkGroups/LinkGroupsPage';
import { InstructionListsPage } from './InstructionLists/InstructionListsPage';
import KitListPage from './Kits';
import '../css/site.css'
import EditKit from './Kits/EditKit';
import { AssignKitsToLocale} from './AssignKits/AssignKitsToLocale';
import {KitLinkGroupPage} from './KitLinkGroups/KitLinkGroupsPage';
import { MuiThemeProvider } from '@material-ui/core';
import { createMuiTheme } from '@material-ui/core/styles';
import Header from './Header/header';
import CreateKitPage from './Kits/CreateKitPage';
import {ViewKit} from './Kits/ViewKits/ViewKit';

const theme = createMuiTheme({
  palette: {
    primary: {
      light: '#88BDBC',
      main: '#254E58',
      dark: '#112D32',
      contrastText: '#fff',
    },
    secondary: {
      light: '#ff7961',
      main: '#f44336',
      dark: '#ba000d',
      contrastText: '#000',
    },
  },
});

class AppRouter extends React.Component {
  render() {
    return (
      <HashRouter>
        <MuiThemeProvider theme = {theme}>
        <div className="container-fluid">
          <Header/>
          <Switch>
            <Route exact path="/" component={InstructionListsPage} />
            <Route path="/Instructions" component={InstructionListsPage} />
            <Route path="/LinkGroups" component={LinkGroupsPage} />
            <Route path="/Kits" component={KitListPage} />
            <Route path="/EditKit" component={EditKit} />
             <Route path="/AssignKits" component={AssignKitsToLocale} />
            <Route path="/CreateKits" component={CreateKitPage} />
            <Route path="/KitLinkGroups" component ={KitLinkGroupPage}/>
            <Route path="/ViewKit" component ={ViewKit}/>
            
          </Switch>
        </div>
        </MuiThemeProvider>
      </HashRouter>
    )
  }
}

export default AppRouter;
