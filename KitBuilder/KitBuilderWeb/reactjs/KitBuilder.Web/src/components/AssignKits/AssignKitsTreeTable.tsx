import * as React from 'react';
import './AssignKitsTreeTable.css';
import LocaleStatus from 'src/types/Localestatus';
import { connect } from 'react-redux';
import { KitTreeState } from 'src/redux/reducers/kitTreeReducer';
import { Dispatch } from 'redux';
// import {KitLinkGroupPage} from '../KitLinkGroups/KitLinkGroupsPage'
import KitStatusIcon from '../Kits/KitStatusIcon'

interface IAssignKitsTreeTableState {
}

interface IAssignKitsTreeTableProps {
    data: any;
    disabled: boolean;
    kitId: number;
    isSimplekitType:boolean;
    kitTree: KitTreeState;
    toggleLocale(kitId: number, localeId: number, data:[]): void;
    assignedLocales: number[];
    excludedLocales: number[];
    toggleLocaleAssigned(localeId: number): void;
    toggleLocaleExcluded(localeId: number): void;
    copyFromLocale: number;
    copyToLocales: number[];
    toggleCopyToLocales(localeId: number): void;
    toggleCopyFromLocale(localeId: number): void;
    excludeDisabled: boolean;
}

export class AssignKitsTreeTable extends React.Component<IAssignKitsTreeTableProps, IAssignKitsTreeTableState>
{
    localeIsOpen = (kitId: number, localeId: number) => {
        const {kitTree} = this.props;
        return kitTree[kitId] && kitTree[kitId][localeId];
    }

    AssignKitProperties(item: any) {
        window.location.hash = "#/KitLinkGroups/"+ this.props.kitId +"/" +  item.localeId;
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
                    <div style={{ width: '120px' }}>{localeNameHeaderMap[LocaleTypeID]}</div>
                    <div style={{ width: '100px' }}>Abbreviation</div>
                    <div style={{ width: '80px' }}>Assign</div>
                    {
                        !showView ?
                        <React.Fragment>
                            <div style={LocaleTypeID != venueLocaleTypeId ? { width: 'calc(100% - 580px)' } : { width: 'calc(100% - 550px)' }}>Exclude</div>
                            <div style={{ width: '100px' }}>Copy To</div>
                            <div style={{ width: '140px' }}>Status</div>
                            </React.Fragment>
                            :
                            <React.Fragment>
                               
                                <div style={{ width: '100px' }}>Exclude</div>
                                <div style={{ width: '100px' }}>Copy From</div>
                                <div style={{ width: '100px' }}>Copy To</div>
                                <div style={{ width: '140px' }}>Status</div>
                                <div style={LocaleTypeID != venueLocaleTypeId ? { width: 'calc(100% - 830px)' } : { width: 'calc(100% - 800px)' }}>View</div>
                            </React.Fragment>
                    }
                </div>
                <div className="tbl-body">
                    {data.map((item: any) => {
                        const isAssigned = this.props.assignedLocales.includes(item.localeId);
                        const isExcluded = this.props.excludedLocales.includes(item.localeId);
                        const isCopiedTo = this.props.copyToLocales.includes(item.localeId);
                        
                        const checkboxesDisabled = this.props.disabled || (isAssigned && item.statusId > LocaleStatus.READYTOPUBLISH);
                        const excludeDisabled = this.props.excludeDisabled || (isAssigned && item.statusId > LocaleStatus.READYTOPUBLISH) || isCopiedTo; 
                        const copyFromEnabled = !this.props.disabled && isAssigned && item.statusId > LocaleStatus.BUILDING && !this.props.isSimplekitType ;
                        const copyToDisabled = isAssigned || item.statusId > 0 || isExcluded || this.props.isSimplekitType;

                        return <React.Fragment key={item.localeId}>
                            <div className="tbl-body-item">
                                {
                                    LocaleTypeID != venueLocaleTypeId ?
                                        <div style={{ width: '30px' }}>
                                            <div className="collapse-btn" onClick={() => this.props.toggleLocale(this.props.kitId, item.localeId, data)}>{this.localeIsOpen(this.props.kitId, item.localeId) ? ' - ' : ' + '}</div>
                                        </div>
                                        : <></>
                                }
                                <div style={{ width: '120px' }}>{item.localeName}</div>
                                <div style={{ width: '100px' }}>{item.localeAbbreviation}</div>
                                <div style={{ width: '80px' }}>
                                    <input type="checkbox" disabled={checkboxesDisabled || isCopiedTo} style={chkboxStyle} checked={isAssigned} onChange={() => this.props.toggleLocaleAssigned(item.localeId)} />
                                </div>
                               
                                {
                                    !showView ?
                                    <React.Fragment>
                                        <div style={LocaleTypeID != venueLocaleTypeId ? { width: 'calc(100% - 580px)' } : { width: 'calc(100% - 550px)' }}>
                                            <input disabled={excludeDisabled} type="checkbox" style={chkboxStyle} checked={isExcluded} onChange={() => this.props.toggleLocaleExcluded(item.localeId)} />
                                        </div>
                                        <div style={{ width: '100px' }}>
                                                <input disabled={copyToDisabled} type="checkbox" style={chkboxStyle} onChange={() => this.props.toggleCopyToLocales(item.localeId)} checked={this.props.copyToLocales.includes(item.localeId)}/>
                                           
                                        </div>
                                        <div style={{ width: '140px'}}><KitStatusIcon status={item.statusId}/></div>
                                        </React.Fragment>
                                        :
                                        <React.Fragment>
                                            <div style={{ width: '100px' }}>
                                                <input disabled={excludeDisabled} type="checkbox" style={chkboxStyle} checked={this.props.excludedLocales.includes(item.localeId)} onChange={() => this.props.toggleLocaleExcluded(item.localeId)} />
                                            </div>
                                            <div style={{ width: '100px' }}>
                                                <input disabled={!copyFromEnabled} type="checkbox" style={chkboxStyle} onChange={() => this.props.toggleCopyFromLocale(item.localeId)} checked={this.props.copyFromLocale == item.localeId} />
                                            </div>
                                            <div style={{ width: '100px' }}>
                                                <input disabled={copyToDisabled} type="checkbox" style={chkboxStyle} onChange={() => this.props.toggleCopyToLocales(item.localeId)} checked={this.props.copyToLocales.includes(item.localeId)}/>
                                            </div>
                                            <div style={{ width: '140px' }}><KitStatusIcon status={item.statusId}/></div>
                                            <div className="btnCenter" style={LocaleTypeID != venueLocaleTypeId ? { width: 'calc(100% - 780px)' } : { width: 'calc(100% - 750px)' }}>
                                                {item.isAssigned && !this.props.isSimplekitType && !item.IsDirty ? <input type="button" style={btnStyle} onClick={() => this.AssignKitProperties(item)} value="Assign Kit Properties" /> : <></>}
                                            </div>
                                        </React.Fragment>
                                }
                            </div>
                            {
                                this.localeIsOpen(this.props.kitId, item.localeId) &&
                                        <div className="tbl-child">
                                            <AssignKitsTreeTable 
                                            assignedLocales={this.props.assignedLocales}
                                            excludedLocales={this.props.excludedLocales}
                                            toggleLocaleAssigned = {this.props.toggleLocaleAssigned}
                                            toggleLocaleExcluded = {this.props.toggleLocaleExcluded}
                                            toggleCopyToLocales = {this.props.toggleCopyToLocales}
                                            toggleCopyFromLocale = {this.props.toggleCopyFromLocale}
                                            kitTree={this.props.kitTree} 
                                            toggleLocale={this.props.toggleLocale}
                                            isSimplekitType = {this.props.isSimplekitType} 
                                            kitId ={this.props.kitId}
                                            data={item.childs} 
                                            disabled={isExcluded || this.props.disabled}
                                            excludeDisabled={isExcluded || this.props.excludeDisabled ||item.statusId > LocaleStatus.READYTOPUBLISH}
                                            copyFromLocale = {this.props.copyFromLocale}
                                            copyToLocales = {this.props.copyToLocales}
                                            />
                                        </div>
                            }
                        </React.Fragment>
                    })}
                </div>
            </div>
        );
    }
}

const mapStateToProps = (state: any) => ({
    kitTree: state.kitTree,
});

const mapDispatchToProps = (dispatch: Dispatch) => ({
    toggleLocale: (kitId: number, localeId: number, data:[]) => dispatch({ type: "TOGGLE_LOCALE", payload: {kitId, localeId,data}}),
})

export default connect(mapStateToProps, mapDispatchToProps)(AssignKitsTreeTable);