import React, { Fragment, useEffect, useContext } from 'react'
import { Header, Segment } from "semantic-ui-react";
import './styles.scss';
import { Link, useHistory } from 'react-router-dom';
import { types, AppContext } from '../../../store';

const Receive: React.FC = () => {
    //@ts-ignore
    const { dispatch } = useContext(AppContext);

    let history = useHistory();

    useEffect(() => {
        dispatch({ type: types.SETTITLE, Title: 'IRMA Mobile' });
        return () => {
          dispatch({ type: types.SETTITLE, Title: 'IRMA Mobile' });
        };
      }, [dispatch]);

    return (
        <Fragment>
            <Segment vertical basic>
                <Header as='h2' textAlign="center">Receive PO or Receiving Document</Header>
            </Segment>
            <Segment vertical basic>
                <button style={{height: '80px', width: '100%'}} className='wfmButton receive-button'><Link to='/receive/PurchaseOrder'>Receive PO</Link></button>
            </Segment>
            <Segment vertical basic>
                <button disabled style={{height: '80px', backgroundColor: 'grey', width: '100%'}} className='wfmButton receive-button'>Receiving Document</button>
            </Segment>
            <Segment vertical basic>
                <button style={{height: '80px', width: '100%'}} className='wfmButton receive-button' onClick={() => history.goBack()}>Cancel</button>
            </Segment>
        </Fragment>
    ) 
}

export default Receive;