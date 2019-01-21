import * as React from 'react'

import Select from '@material-ui/core/Select';
import MenuItem from '@material-ui/core/MenuItem';


interface IProps {
    SelectOptions: Array<any>, 
    SelectedOption: any,
    LinkGroupId: any
    handleSelectionChanged(event:any): void
}
interface IState {
    
}

export class InstructionListPicker extends React.Component<IProps, IState> {
    constructor(props: any) {
        super(props)


    }

    renderSelectOptions(selectOptions: Array<any>) {
        if (!selectOptions) return;
        var options = selectOptions.map((o, i) =>
            <MenuItem value={o.instructionListId} key={i}>{o.name}</MenuItem>
        )

        options.unshift(<MenuItem key={-1} value="-1"><em>None</em></MenuItem>);
        return options;
    }

    render() {
        return (
            <span>
                   
                <Select
                    value={this.props.SelectedOption}
                    onChange={(event)=> {
                    
                       this.props.handleSelectionChanged(event);
                    }}
                    displayEmpty
                    fullWidth
                    name="InstructionListDropdown"
                >
                    {this.renderSelectOptions(this.props.SelectOptions)}
                </Select>

            </span>
            
        )
    }

}

export default InstructionListPicker