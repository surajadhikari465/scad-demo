import React, { Fragment, useState } from 'react'
import LoadingComponent from '../../../layout/LoadingComponent';

const TransferScan: React.FC = () => {

    const [isLoading, setIsLoading] = useState<boolean>(false);

    return (
        <Fragment>
            {isLoading ? <LoadingComponent content='Loading item...' />
            :
            <Fragment>
                
            </Fragment>
            }
        </Fragment>
    )
}

export default TransferScan;