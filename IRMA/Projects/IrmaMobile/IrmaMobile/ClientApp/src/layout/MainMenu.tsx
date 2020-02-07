import React from "react";
import { Dropdown } from "semantic-ui-react";
import { Link } from "react-router-dom";

interface IProps {
    disabled: boolean;
    icon: string;
    slot: string;
    menuItems: any[];
    style?: object;
}

const MainMenu: React.FC<IProps> = ({ disabled, icon, slot, menuItems, style }) => {
    const itemHeight = 60;

    const varStyle = {
        minHeight: (menuItems.length * itemHeight) + 'px',
        backgroundColor: 'rgb(248,248,255)',
        right: slot ==='end'? 0:'auto',
        left: slot === 'start' ? 0 :'auto'
    }

    return (
        <Dropdown icon={icon} slot={slot} disabled={disabled} style={style}>
            <Dropdown.Menu style={varStyle}>
                {menuItems && menuItems.sort((a, b) => {return a.order - b.order}).map(item => (
                    <Dropdown.Item style={{height: itemHeight + 'px', fontSize: '28px'}} key={item.id} as={Link} to={item.path} disabled={item.disabled} onClick={item.onClick}>{item.text}</Dropdown.Item>
                ))}
            </Dropdown.Menu>
        </Dropdown>
    );
};

export default MainMenu;
