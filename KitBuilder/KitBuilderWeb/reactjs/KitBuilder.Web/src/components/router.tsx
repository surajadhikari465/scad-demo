import * as React from 'react';
import { Route, HashRouter, Switch } from 'react-router-dom';
import { App } from '../App';
import { PlaceHolderPage } from './PlaceHolder/PlaceHolderPage';
import { LinkGroupPage } from './LinkGroups/LinkGroupPage';
import { Hello } from './Hello/Hello'
import '../css/site.css'


class AppRouter extends React.Component {
  render() {
    return (
      <HashRouter>
        <div className="container-fluid">
          <Route component={App} />
          <Switch>
            <Route exact path="/" component={() => <Hello name="KitBuilder" enthusiasmLevel={5} />} />
            <Route path="/Instructions" component={PlaceHolderPage} />
            <Route path="/LinkGroups" component={LinkGroupPage} />
            <Route path="/Kits" component={PlaceHolderPage} />
            <Route path="/KitAssignment" component={PlaceHolderPage} />
          </Switch>
        </div>
      </HashRouter>
    )
  }
}

export default AppRouter;
