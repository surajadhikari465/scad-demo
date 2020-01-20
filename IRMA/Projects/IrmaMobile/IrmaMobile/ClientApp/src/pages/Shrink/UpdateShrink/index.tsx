import React, { Fragment } from 'react';

const UpdateShrink:React.FC = () => {
    return (  
      <Fragment>
        <div className='update-shrink-container'>
        <h3>Update Shrink</h3>

        <label>UPC:</label>
        <span>123123</span> 
        <label>Desc:</label>
        <span>pinot</span>

        <label>Quantity Recorded:</label>
        <span></span>
        <label>New Quantity:</label>
        <span></span>
        <label>UOM:</label>
        <span></span>

        <label>Sub Type:</label>
        <select></select>
        </div>
      </Fragment>
    )
};

export default UpdateShrink;