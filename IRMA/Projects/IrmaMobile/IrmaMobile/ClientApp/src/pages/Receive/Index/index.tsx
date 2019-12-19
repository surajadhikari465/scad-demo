import React, { Fragment } from 'react'
import { Header, Segment } from "semantic-ui-react";
import './styles.scss';
import { Link, useHistory } from 'react-router-dom';

const Receive: React.FC = () => {
    let history = useHistory();

    return (
        <Fragment>
            <Segment vertical>
                <Header as='h2' textAlign="center">Receive PO or Receiving Document</Header>
            </Segment>
            <Segment vertical>
                <button className='wfmButton receive-button'><Link to='/receive/PurchaseOrder'>Receive PO</Link></button>
            </Segment>
            <Segment vertical>
                <button className='wfmButton receive-button'><Link to='/receive/Document'>Receiving Document</Link></button>
            </Segment>
            <Segment vertical>
                <button className='wfmButton receive-button' onClick={() => history.goBack()}>Cancel</button>
            </Segment>
        </Fragment>
    ) 
}

export default Receive;
