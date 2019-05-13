import React from 'react';
interface IKitStatusIconProps {
    status: number;
}

const KitStatusIcon = (props: IKitStatusIconProps) => {
    
    switch(props.status) {
        case 1: return <>Disabled</>;
        case 2: return <>Building</>;
        case 3: return <>Ready To Publish</>;
        case 4: return <>Publish Queued</>;
        case 5: return <>Published</>;
        case 6: return <>Modifying</>;
        case 7: return <>Publish Failed</>;
        case 8: return <>Publish Requeued</>;
        case 9: return <>Partially Published</>;
        case 10: return <>Unauthorized</>;
        case 11: return <>Processed</>;
        case 11: return <>UnProcessed</>;
        default: return null;
    }
}
// @ts-ignore
export default KitStatusIcon;