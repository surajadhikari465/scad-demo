import * as React from 'react';
import './AssignKitsTreeTable.css';
import LocaleStatus from 'src/types/Localestatus';
// import {KitLinkGroupPage} from '../KitLinkGroups/KitLinkGroupsPage'

interface IAssignKitsTreeTableState {
}

interface IAssignKitsTreeTableProps {
    data: any,
    updateData: any,
    disabled: boolean,
    kitId: number,
    isSimplekitType:boolean
}

export class AssignKitsTreeTable extends React.Component<IAssignKitsTreeTableProps, IAssignKitsTreeTableState>
{
    constructor(props: any) {
        super(props);

        this.state = {
        }
    }

    componentDidMount() {
    }

    AssignKitProperties(item: any) {
        window.location.hash = "#/KitLinkGroups/"+ this.props.kitId +"/" +  item.localeId;
    }

    onAssignClicked(item: any) {
        item.isAssigned = !item.isAssigned;
        item.IsDirty = true;
        if (item.isAssigned == true) {
            item.isChildDisabled = false;
            this.enableDisableControls(item.childs, false);
        }
        item.isExcluded = false;
        this.props.updateData();
    }

    onExcludeClicked(item: any) {
        item.isExcluded = !item.isExcluded;

        if (item.isExcluded == true) {
            item.isChildDisabled = true;
            this.UncheckData(item.childs, false);
        }
        else {
            item.isChildDisabled = false;
            this.UncheckData(item.childs, true);
        }

        item.isAssigned = false;
        this.props.updateData();
    }

    enableDisableControls(data: Array<any>, disablechild: boolean) {
        for (var i = 0; i < data.length; i++) {
            data[i].isChildDisabled = disablechild;
            this.enableDisableControls(data[i].childs, disablechild);
        }
    }

    UncheckData(data: Array<any>, enable: boolean) {
        for (var i = 0; i < data.length; i++) {
            data[i].isExcluded = false;
            data[i].isAssigned = false;
            data[i].isChildDisabled = !enable;
            this.UncheckData(data[i].childs, enable);
        }

    }
    onCollapseClicked(item: any) {
        if (item.collapsed == undefined) {
            item.collapsed = false;
        }
        item.collapsed = !item.collapsed;
        this.props.updateData();
    }

    enableDisableControlsOnLoad(item: any) {
        for (let i = 0; i < item.length; i++) {
            item.IsDirty = false;       
            if (item[i].isExcluded == true) {
                item[i].isChildDisabled = true;
                if (item[i].childs !== undefined) {
                    this.enableDisableControls(item[i].childs, true);
                }

            }
        }
    }
    render() {
        const { data } = this.props;

        if (data.length > 0) {
            this.enableDisableControlsOnLoad(data);
        }

        const tblStyle = {
            border: '1px solid #c8c8c8',
            width: 'calc(100% - 30px)',
            margin: '5px 5px 0 30px'
        };
        const tblHeaderStyle = {
            backgroundColor: 'grey',
            color: 'white'
        };

        const chkboxStyle = {
            display: 'flex',
            marginLeft: 'auto',
            marginRight: 'auto'
        }

        const btnStyle = {
            display: 'flex',
            marginLeft: 'auto',
            marginRight: 'auto'
        }

        var venueLocaleTypeId = 5;
        const localeNameHeaderMap = {
            1: 'Organization',
            2: 'Region',
            3: 'Metro',
            4: 'Store',
            5: 'Venue'
        };

        if (data.length == 0) {
            return <></>;
        }

        const LocaleTypeID = data[0].localeTypeId;
        let showView = false;

        for (let i = 0; i < data.length; i++) {
            if (data[i].isAssigned == true && !data[i].IsDirty) {
                showView = true;
                break;
            }
        }

        return (
            <div style={tblStyle}>
                <div className="tbl-header" style={tblHeaderStyle}>
                    {
                        LocaleTypeID != venueLocaleTypeId ? <div style={{ width: '30px' }}></div> : <></>
                    }
                    <div style={{ width: '150px' }}>{localeNameHeaderMap[LocaleTypeID]}</div>
                    <div style={{ width: '150px' }}>Abbreviation</div>
                    <div style={{ width: '150px' }}>Assign</div>
                    {
                        !showView ?
                            <div style={LocaleTypeID != venueLocaleTypeId ? { width: 'calc(100% - 480px)' } : { width: 'calc(100% - 450px)' }}>Exclude</div>
                            :
                            <React.Fragment>
                                <div style={{ width: '150px' }}>Exclude</div>
                                <div style={LocaleTypeID != venueLocaleTypeId ? { width: 'calc(100% - 630px)' } : { width: 'calc(100% - 600px)' }}>View</div>
                            </React.Fragment>
                    }
                </div>
                <div className="tbl-body">
                    {data.map((item: any) => {
                        const checkboxesDisabled = this.props.disabled || (item.isAssigned && item.statusId > LocaleStatus.BUILDING);
                        return <React.Fragment key={item.localeId}>
                            <div className="tbl-body-item">
                                {
                                    LocaleTypeID != venueLocaleTypeId ?
                                        <div style={{ width: '30px' }}>
                                            <div className="collapse-btn" onClick={() => this.onCollapseClicked(item)}>{!item.collapsed ? ' - ' : ' + '}</div>
                                        </div>
                                        : <></>
                                }
                                <div style={{ width: '150px' }}>{item.localeName}</div>
                                <div style={{ width: '150px' }}>{item.localeAbbreviation}</div>
                                <div style={{ width: '150px' }}>
                                    <input type="checkbox" disabled={checkboxesDisabled} style={chkboxStyle} checked={item.isAssigned} onChange={() => this.onAssignClicked(item)} />
                                </div>
                                {
                                    !showView ?
                                        <div style={LocaleTypeID != venueLocaleTypeId ? { width: 'calc(100% - 480px)' } : { width: 'calc(100% - 450px)' }}>
                                            <input disabled={checkboxesDisabled} type="checkbox" style={chkboxStyle} checked={item.isExcluded} onChange={() => this.onExcludeClicked(item)} />
                                        </div>
                                        :
                                        <React.Fragment>
                                            <div style={{ width: '150px' }}>
                                                <input disabled={checkboxesDisabled} type="checkbox" style={chkboxStyle} checked={item.isExcluded} onChange={() => this.onExcludeClicked(item)} />
                                            </div>
                                            <div className="btnCenter" style={LocaleTypeID != venueLocaleTypeId ? { width: 'calc(100% - 630px)' } : { width: 'calc(100% - 600px)' }}>
                                                {item.isAssigned && !this.props.isSimplekitType && !item.IsDirty ? <input type="button" style={btnStyle} onClick={() => this.AssignKitProperties(item)} value="Assign Kit Properties" /> : <></>}
                                            </div>
                                        </React.Fragment>
                                }
                            </div>
                            {
                                !item.collapsed ?
                                    !item.isChildDisabled ?
                                        <div className="tbl-child">
                                            <AssignKitsTreeTable isSimplekitType = {this.props.isSimplekitType} kitId ={this.props.kitId} disabled={false} data={item.childs} updateData={this.props.updateData}></AssignKitsTreeTable>
                                        </div>
                                        : <div className="tbl-child">
                                            <AssignKitsTreeTable isSimplekitType = {this.props.isSimplekitType}  kitId ={this.props.kitId}  disabled={true} data={item.childs} updateData={this.props.updateData}></AssignKitsTreeTable>
                                        </div>
                                    : <></>
                            }
                        </React.Fragment>
                    })}
                </div>
            </div>
        );
    }
}

export default AssignKitsTreeTable;