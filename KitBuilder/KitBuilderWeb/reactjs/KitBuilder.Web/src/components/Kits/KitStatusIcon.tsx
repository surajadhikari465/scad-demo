import React from 'react';
interface IKitStatusIconProps {
    status: number;
}

const KitStatusIcon = (props: IKitStatusIconProps) => {
    
    switch(props.status) {
        case 1: return null;
        case 2: return <>Building</>;
        case 3: return <>Publish Queued</>;
        case 4: return <>Published</>;
        case 5: return <>Modifying</>;
        case 6: return <>Publish Failed</>;
        case 7: return <>Publish Requeued</>;
        case 8: return <>Partially Published</>;
        default: return null;
    }
}
// @ts-ignore
export default KitStatusIcon;