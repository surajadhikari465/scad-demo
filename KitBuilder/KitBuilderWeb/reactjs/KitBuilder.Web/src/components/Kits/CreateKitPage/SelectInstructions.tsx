import * as React from 'react';
import { FormControl, InputLabel, Select, MenuItem, Checkbox, ListItemText, Input } from '@material-ui/core';
import { KbApiMethod } from 'src/components/helpers/kbapi';
import { InstructionList } from '../../../types/InstructionList';

interface ISelectInstructionsProps {
    selectedInstructionLists: Array<InstructionList>;
    onSelectInstructionList: (selectedLists: Array<InstructionList>) => void;
}

export const SelectInstructions = (props: ISelectInstructionsProps) => {

    const [instructionLists, setInstructionLists ] = React.useState<Array<InstructionList>>([]);

    React.useEffect(() => {
        const url = KbApiMethod("InstructionList");
        fetch(url)
        .then(response => response.json())
        .then((lists: Array<InstructionList>) => lists.filter((list: InstructionList) => list.instructionTypeId === 1))
        .then((lists: Array<InstructionList>) => setInstructionLists(lists))
    }, [])
    
    const getIsSelected = (instruction: InstructionList) => {
    const id = instruction.instructionListId;
        return props.selectedInstructionLists.map(i => i.instructionListId).includes(id);
    }

    const handleSelectInstructions = (e: React.ChangeEvent<HTMLSelectElement>) => {
        const lists = e.target.value;
        // @ts-ignore
        const selectedInstructionLists = instructionLists.filter(i => lists.includes(i.instructionListId));
        props.onSelectInstructionList(selectedInstructionLists);
    }

    return (
        <FormControl variant="outlined">
          <InputLabel htmlFor="select-multiple-checkbox" variant="outlined">Select Instructions</InputLabel>
          <Select
            multiple
            variant="outlined"
            value={props.selectedInstructionLists.map(i => i.instructionListId)}
            onChange={handleSelectInstructions}
            input={<Input id="select-multiple-checkbox" />}
            // @ts-ignore
            renderValue={(selected: Array<InstructionList>) => instructionLists.filter(i => selected.includes(i.instructionListId)).map(s => s.name).join(', ')}
          >
            {instructionLists.map((instruction: InstructionList, index) => (
              // @ts-ignore We can assign an object as a value here
              <MenuItem key={instruction.instructionListId} value={instruction.instructionListId}>
                <Checkbox checked={getIsSelected(instruction)} />
                <ListItemText primary={instruction.name} />
              </MenuItem>
            ))}
          </Select>
        </FormControl>
    );
}