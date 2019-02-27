import * as React from 'react'
import Grid from '@material-ui/core/Grid'
import TextField from '@material-ui/core/TextField'
import Button from '@material-ui/core/Button'
import { withStyles } from '@material-ui/core/styles'
import Select from '@material-ui/core/Select';
import MenuItem from '@material-ui/core/MenuItem';
import Checkbox from '@material-ui/core/Checkbox';
import ListItemText from '@material-ui/core/ListItemText';
import { Input, InputLabel } from '@material-ui/core';


const styles = (theme: any) => ({
    root: {
        flexGrow: 1
    },
    label: {
        fontSize: 20,
        textAlign: "right" as 'right',
        marginBottom: 0 + ' !important',
        paddingRight: 10 + 'px'
    },
    button: {
        margin: theme.spacing.unit,

    },
    searchButtons: {
        width: '150px',
        marginRight: '20px',
        marginTop: '10px'

    },
    formControl:
    {
        margin: theme.spacing.unit,
        minWidth: 120,
        width: '100%'
    },

});

const regions = [
    'FL',
    'MA',
    'MW',
    'NA',
    'NE',
    'PN',
    'RM',
    'SO',
    'SP',
    'SW',
    'UK'
    
  ];

  const ITEM_HEIGHT = 48;
const ITEM_PADDING_TOP = 8;
  const MenuProps = {
    PaperProps: {
      style: {
        maxHeight: ITEM_HEIGHT * 4.5 + ITEM_PADDING_TOP,
        width: 250,
      },
    },
  };

interface IProps {
    classes: any,
    searchOptions: any,
    onChange(event: any): void,
    onSearch(): void,
    onAddNew():void,
    showSearchProgress: boolean
}

interface IState {
    inputLinkGroupName: string,
    inputLinkGroupDesc: string,
    inputRegion: string,
    inputModifierName: string,
    inputModifierPLU: string,
    selectedRegions: Array<string> ,
   
}

class SearchLinkGroups extends React.Component<IProps, IState> {
    constructor(props: any) {
        super(props);
        this.state = {
            inputLinkGroupName: "",
            inputLinkGroupDesc: "",
            inputRegion: "",
            inputModifierName: "",
            inputModifierPLU: "",
            selectedRegions: [],
     
        }
        this.handleChange = this.handleChange.bind(this);
    }


    handleChange(event:any) {
        this.setState({selectedRegions: event.target.value})
    }

  

    render() {

        return (
            <React.Fragment>
                <Grid container spacing={24} justify='center'>
                    <Grid item md={3}>
                        <TextField id="LinkGroupName"
                            label="Link Group Name"
                            fullWidth
                            name="LinkGroupName"
                            onChange={this.props.onChange}
                        />
                        <TextField id="LinkGroupDesc"
                            label="Link Group Desc"
                            fullWidth
                            name="LinkGroupDesc"
                            onChange={this.props.onChange}
                        />
                        {/* <TextField id="Region"
                            label="Region"
                            fullWidth
                            name="Region"
                            onChange={this.props.onChange}
                        /> */}

<InputLabel htmlFor="c" >Regions</InputLabel>
        
        <Select
          multiple
          fullWidth
          value={this.state.selectedRegions}
          onChange={this.handleChange}
          input={<Input id="aaa" fullWidth   />}
        renderValue={(selected:[]) => {  console.log(selected); return selected.join(', ')}}
       
          MenuProps={MenuProps}
        >
          {regions.map((region:string) => (
            <MenuItem key={region} value={region}>
              <Checkbox checked={ this.state.selectedRegions.indexOf(region) > -1} />
              <ListItemText primary={region} />
            </MenuItem>
          ))}
        </Select>
      
                        
                    </Grid>
                    <Grid item md={3}>
                        <TextField id="ModifierName"
                            label="Modifier Name"
                            fullWidth
                            name="ModifierName"
                            onChange={this.props.onChange}
                        />
                        <TextField id="ModifierPLU"
                            label="Modifier PLU"
                            fullWidth
                            name="ModifierPLU"
                            onChange={this.props.onChange}
                        />
                        <Button
                            variant="contained"
                            color="primary"
                            className={this.props.classes.searchButtons}
                            onClick={() => this.props.onSearch()}
                        >
                            Search
                            </Button>
                        <Button
                            variant="contained"
                            color="secondary"
                            className={this.props.classes.searchButtons}
                            onClick={() => { this.props.onAddNew() } }
                        >
                            Add New
                            </Button>
                    </Grid>
                </Grid>
                <br />
                <br />
            </React.Fragment>
        )
    }
}

export default withStyles(styles)(SearchLinkGroups);