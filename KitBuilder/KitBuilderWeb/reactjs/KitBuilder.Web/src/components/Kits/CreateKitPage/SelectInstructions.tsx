import * as React from 'react';
import { FormControl, InputLabel, Select, MenuItem, Checkbox, ListItemText, Input } from '@material-ui/core';
import { KbApiMethod } from 'src/components/helpers/kbapi';
import { InstructionList } from '../../../types/InstructionList';

interface ISelectInstructionsProps {
    selectedInstructionLists: Array<InstructionList>;
    onSelectInstructionList: (selectedLists: Array<InstructionList>) => void;
}

export const SelectInstructions = (props: ISelectInstructionsProps) => {

    const [instructionLists, setInstructionLists ] = React.useState([]);

    React.useEffect(() => {
        const url = KbApiMethod("InstructionList");
        fetch(url)
        .then(response => response.json())
        .then((lists) => setInstructionLists(lists))
    }, [])
    
    const getIsSelected = (instruction: InstructionList) => {
        return props.selectedInstructionLists.includes(instruction);
    }

    const handleSelectInstructions = (e: React.ChangeEvent<HTMLSelectElement>) => {
        const lists = e.target.value;
        // @ts-ignore Select component returns an array
        props.onSelectInstructionList(lists);
    }

    return (
        <FormControl variant="outlined">
          <InputLabel htmlFor="select-multiple-checkbox" variant="outlined">Select Instructions</InputLabel>
          <Select
            multiple
            variant="outlined"
            value={props.selectedInstructionLists}
            onChange={handleSelectInstructions}
            input={<Input id="select-multiple-checkbox" />}
                // @ts-ignore TODO fix pascal json issue
            renderValue={(selected: Array<InstructionList>) => selected.map(s => s.Name).join(', ')}
          >
            {instructionLists.map((instruction: InstructionList, index) => (
                // @ts-ignore value can return object
              <MenuItem key={instruction.InstructionListId} value={instruction}>
                <Checkbox checked={getIsSelected(instruction)} />
                {/* 
                // @ts-ignore TODO fix pascal json issue */}
                <ListItemText primary={instruction.Name} />
              </MenuItem>
            ))}
          </Select>
        </FormControl>
    );
}