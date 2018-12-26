import * as React from 'react';
import { Route, HashRouter, Switch } from 'react-router-dom';
import { App } from '../App';
import { LinkGroupPage } from './LinkGroups/LinkGroupPage';
import { InstructionListsPage } from './InstructionLists/InstructionListsPage';
import { KitListPage } from './Kits/KitListPage';
import { Hello } from './Hello/Hello'
import '../css/site.css'
import EditKit from './Kits/EditKit';
import CreateKit from './Kits/CreateKit';


class AppRouter extends React.Component {
  render() {
    return (
      <HashRouter>
        <div className="container-fluid">
          <Route component={App} />
          <Switch>
            <Route exact path="/" component={() => <Hello name="KitBuilder" enthusiasmLevel={5} />} />
            <Route path="/Instructions" component={InstructionListsPage} />
            <Route path="/LinkGroups" component={LinkGroupPage} />
            <Route path="/Kits" component={KitListPage} />
            <Route path="/KitAssignment" component={InstructionListsPage} />
            <Route path="/EditKit" component={EditKit} />
            <Route path="/CreateKit" component={CreateKit} />
          </Switch>
        </div>
      </HashRouter>
    )
  }
}

export default AppRouter;
