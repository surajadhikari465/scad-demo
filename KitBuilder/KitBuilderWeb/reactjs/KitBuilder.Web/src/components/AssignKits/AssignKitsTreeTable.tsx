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
    toggleLocale(kitId: number, localeId: number): void;
    assignedLocales: number[];
    excludedLocales: number[];
    toggleLocaleAssigned(localeId: number): void;
    toggleLocaleExcluded(localeId: number): void;
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
                            <div style={LocaleTypeID != venueLocaleTypeId ? { width: 'calc(100% - 480px)' } : { width: 'calc(100% - 450px)' }}>Exclude</div>
                            <div style={{ width: '140px' }}>Status</div>
                            </React.Fragment>
                            :
                            <React.Fragment>
                               
                                <div style={{ width: '100px' }}>Exclude</div>
                                <div style={{ width: '140px' }}>Status</div>
                                <div style={LocaleTypeID != venueLocaleTypeId ? { width: 'calc(100% - 630px)' } : { width: 'calc(100% - 600px)' }}>View</div>
                            </React.Fragment>
                    }
                </div>
                <div className="tbl-body">
                    {data.map((item: any) => {
                        const isAssigned = this.props.assignedLocales.includes(item.localeId);
                        const isExcluded = this.props.excludedLocales.includes(item.localeId);
                        
                        const checkboxesDisabled = this.props.disabled || (isAssigned && item.statusId > LocaleStatus.READYTOPUBLISH);
                        const excludeDisabled = this.props.excludeDisabled || (isAssigned && item.statusId > LocaleStatus.READYTOPUBLISH); 
                        return <React.Fragment key={item.localeId}>
                            <div className="tbl-body-item">
                                {
                                    LocaleTypeID != venueLocaleTypeId ?
                                        <div style={{ width: '30px' }}>
                                            <div className="collapse-btn" onClick={() => this.props.toggleLocale(this.props.kitId, item.localeId)}>{this.localeIsOpen(this.props.kitId, item.localeId) ? ' - ' : ' + '}</div>
                                        </div>
                                        : <></>
                                }
                                <div style={{ width: '120px' }}>{item.localeName}</div>
                                <div style={{ width: '100px' }}>{item.localeAbbreviation}</div>
                                <div style={{ width: '80px' }}>
                                    <input type="checkbox" disabled={checkboxesDisabled} style={chkboxStyle} checked={isAssigned} onChange={() => this.props.toggleLocaleAssigned(item.localeId)} />
                                </div>
                               
                                {
                                    !showView ?
                                    <React.Fragment>
                                        <div style={LocaleTypeID != venueLocaleTypeId ? { width: 'calc(100% - 480px)' } : { width: 'calc(100% - 450px)' }}>
                                            <input disabled={excludeDisabled} type="checkbox" style={chkboxStyle} checked={isExcluded} onChange={() => this.props.toggleLocaleExcluded(item.localeId)} />
                                        </div>
                                         <div style={{ width: '140px'}}><KitStatusIcon status={item.statusId}/></div>
                                         </React.Fragment>
                                        :
                                        <React.Fragment>
                                            <div style={{ width: '150px' }}>
                                                <input disabled={excludeDisabled} type="checkbox" style={chkboxStyle} checked={this.props.excludedLocales.includes(item.localeId)} onChange={() => this.props.toggleLocaleExcluded(item.localeId)} />
                                            </div>
                                            <div style={{ width: '140px' }}><KitStatusIcon status={item.statusId}/></div>
                                            <div className="btnCenter" style={LocaleTypeID != venueLocaleTypeId ? { width: 'calc(100% - 630px)' } : { width: 'calc(100% - 600px)' }}>
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
                                            kitTree={this.props.kitTree} 
                                            toggleLocale={this.props.toggleLocale}
                                            isSimplekitType = {this.props.isSimplekitType} 
                                            kitId ={this.props.kitId}
                                            data={item.childs} 
                                            disabled={isExcluded || this.props.disabled}
                                            excludeDisabled={isExcluded || this.props.excludeDisabled ||item.statusId > LocaleStatus.READYTOPUBLISH}
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
    toggleLocale: (kitId: number, localeId: number) => dispatch({ type: "TOGGLE_LOCALE", payload: {kitId, localeId}}),
})

export default connect(mapStateToProps, mapDispatchToProps)(AssignKitsTreeTable);