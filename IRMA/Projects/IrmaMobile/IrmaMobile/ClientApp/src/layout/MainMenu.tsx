import React, { useContext } from "react";
import { AppContext } from "../store";
import { Dropdown } from "semantic-ui-react";
import { Link } from "react-router-dom";

interface IProps {
    disabled: boolean;
}

const MainMenu: React.FC<IProps> = ({ disabled }) => {
    const { state } = useContext(AppContext);
    const { menuItems } = state;

    return (
        <Dropdown icon="bars" slot="start" disabled={disabled}>
            <Dropdown.Menu>
                {menuItems && menuItems.sort((a, b) => {return a.order - b.order}).map(item => (
                    <Dropdown.Item key={item.id} as={Link} to={item.path} disabled={item.disabled} onClick={item.onClick}>{item.text}</Dropdown.Item>
                ))}
            </Dropdown.Menu>
        </Dropdown>
    );
};

export default MainMenu;
