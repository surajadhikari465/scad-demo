import * as React from 'react';
//import '../css/site.css';

export const PlaceHolderPage: React.StatelessComponent<{}> = () => {
  return (
    <div className="row">
      <div className="jumbotron">
        <p className="lead">Something else will go here...</p>
        <ul className="list-group">
          <li  className="list-group-item"><a href="https://getbootstrap.com/docs/4.0/components/buttons/">Bootstrap 4</a></li>
          <li  className="list-group-item"><a href="https://www.typescriptlang.org/docs/home.html">TypeScript 3.2.1</a></li>
          <li  className="list-group-item"><a href="https://reactjs.org/docs/getting-started.html">ReactJS 16.6.3</a></li>
          <li  className="list-group-item">and other stuff...</li>
        </ul>
      </div>
    </div>
  );
}

export default PlaceHolderPage