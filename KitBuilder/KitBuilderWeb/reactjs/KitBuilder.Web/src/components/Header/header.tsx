import * as React from 'react';
import { Link } from 'react-router-dom';

export const Header: React.StatelessComponent<{}> = () => {
    return (
        <div className="row bg-dark">
            <nav className="navbar navbar-expand-lg   navbar-dark bg-dark">
                <a className="navbar-brand" href="#">KitBuilder</a>
                <button className="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                    <span className="navbar-toggler-icon"></span>
                </button>
                <div className="collapse navbar-collapse" id="navbarSupportedContent">
                    <ul className="navbar-nav mr-auto">
                        <li className="nav-item">
                            <Link className="nav-link" to="/Instructions"> Instructions</Link>
                        </li>
                        <li className="nav-item ">
                            <Link className="nav-link" to="/LinkGroups"> Link Groups </Link>
                        </li>
                        <li className="nav-item ">
                            <Link className="nav-link" to="/Kits"> Kits </Link>
                        </li>
                        <li className="nav-item ">
                            <Link className="nav-link" to="/CreateKits"> Create Kit</Link>
                        </li>
                        <li className="nav-item ">
                            <Link className="nav-link" to="/ViewKit"> View Kit</Link>
                        </li>
                        <li className="nav-item ">
                            <Link className="nav-link" to="/AssignKits"> Assign Kits</Link>
                        </li>
                    </ul>
                </div>
            </nav>
        </div>
    );
}

export default Header;

