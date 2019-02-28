import * as React from 'react';
import { Route, HashRouter, Switch } from 'react-router-dom';
import { App } from '../App';
import { LinkGroupsPage } from './LinkGroups/LinkGroupsPage';
import { InstructionListsPage } from './InstructionLists/InstructionListsPage';
import KitListPage from './Kits';
import '../css/site.css'
import EditKit from './Kits/EditKit';
import { AssignKitsToLocale} from './AssignKits/AssignKitsToLocale';
import { CreateKit} from './Kits/KitItemAddModal/CreateKit';
import {KitLinkGroupPage} from './KitLinkGroups/KitLinkGroupsPage';

class AppRouter extends React.Component {
  render() {
    return (
      <HashRouter>
        <div className="container-fluid">
          <Route component={App} />
          <Switch>
            <Route exact path="/" component={InstructionListsPage} />
            <Route path="/Instructions" component={InstructionListsPage} />
            <Route path="/LinkGroups" component={LinkGroupsPage} />
            <Route path="/Kits" component={KitListPage} />
            <Route path="/EditKit" component={EditKit} />
             <Route path="/AssignKits" component={AssignKitsToLocale} />
            <Route path="/CreateKits" component={CreateKit} />
            <Route path="/KitLinkGroups" component ={KitLinkGroupPage}/>
          </Switch>
        </div>
      </HashRouter>
    )
  }
}

export default AppRouter;
