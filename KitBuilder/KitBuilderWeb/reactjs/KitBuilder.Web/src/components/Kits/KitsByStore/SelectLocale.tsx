import * as React from 'react'
import Grid from '@material-ui/core/Grid';
import Button from '@material-ui/core/Button';
import FormControl from '@material-ui/core/FormControl';
import MenuItem from '@material-ui/core/MenuItem';
import TextField from '@material-ui/core/TextField';
import { withStyles } from '@material-ui/core/styles';

const styles = (theme: any) => ({
    root: {
        marginTop: 24
    },
    label: {
        textAlign: "right" as 'right',
        marginBottom: 0 + ' !important',
        paddingRight: 10 + 'px'
    },
    labelRoot: {
        fontSize: 18,
        fontWeight:600
      },
    button: {
        width: '100%',
    },
    editButton: {
        width: '100%',
        marginLeft: '16px'
    },
    formControl:
    {
        width: '100%'
    }
});

const getAllRegions = (regions: Array<any>) => 
regions.map((item: any) => 
        <MenuItem key={item.localeId} value={item.localeId}> {item.localeName} </MenuItem>
    );

    const getMetros = (metros: Array<any>) => 
    metros.map((item: any) => 
        <MenuItem key={item.localeId} value={item.localeId}> {item.localeName} </MenuItem>
    );

    const getStores = (stores: Array<any>) => 
    stores.map((item: any) => 
        <MenuItem key={item.localeId} value={item.localeId}> {item.localeName} </MenuItem>
    );


function SelectLocale(props: any) {
    const { regions,metros,stores, regionValue,storeValue,metroValue } = props;
    const handlePressEnterToSearch = (e: React.KeyboardEvent) => {
        e = e || window.event;
        const ENTER = 13;
    
        if(e.keyCode == ENTER){
            props.ViewKitsByStore();
        }
    }
    return (<div className="search-container">
            <Grid container justify="space-between" alignItems="center" spacing={16}>
          
                <Grid xs = {12} md={1} item>
                </Grid>
                        <Grid item xs={12} sm={12 } md={2}>
                            <FormControl className={props.classes.formControl} onKeyUp={handlePressEnterToSearch}>
                                <TextField
                                    select
                                    onChange={props.onRegionChange}
                                    variant='outlined'
                                    label='Select Region'
                                    className = 'search-textfield'
                                    InputLabelProps={{ shrink: true,
                                        FormLabelClasses: {
                                            root: props.classes.labelRoot
                                          }  }}
                                    value={regionValue}>
                                    {getAllRegions(regions)}
                                </TextField>
                            </FormControl>
                            </Grid>
                            <Grid item xs={12} sm={12} md={2}>
                            <FormControl className={props.classes.formControl} onKeyUp={handlePressEnterToSearch}>
                                <TextField
                                    select
                                    onChange={props.onMetroChange}
                                    variant='outlined'
                                    label='Select Metro'
                                    className = 'search-textfield'
                                    InputLabelProps={{ shrink: true,
                                        FormLabelClasses: {
                                            root: props.classes.labelRoot
                                          } 
                                     }}
                                    value={metroValue}>
                                    {getMetros(metros)}
                                </TextField>
                            </FormControl>
                            </Grid>
                            <Grid item xs={12} sm={12} md={3}>
                            <FormControl className={props.classes.formControl} onKeyUp={handlePressEnterToSearch}>
                                <TextField
                                    select
                                    onChange={props.onStoreChange}
                                    variant='outlined'
                                    label='Select Store'
                                    className = 'search-textfield'
                                    InputLabelProps={{ shrink: true,
                                        FormLabelClasses: {
                                            root: props.classes.labelRoot
                                          }  }}
                                    value={storeValue}>
                                    {getStores(stores)}
                                </TextField>
                            </FormControl>
                        </Grid>
                        <Grid item xs={12} sm={12} md={2}>
                            <Button variant="contained" color="primary" disabled={props.disableViewKitButton} className = "full-width" onClick={props.viewKitsByStore}>
                                {props.disableViewKitButton ? "Loading..." : "View Kits"}
                            </Button>
                        </Grid>
                        <Grid xs = {12} md={2} item>
                    </Grid>
                    </Grid>
 
        </div>)
}

export default withStyles(styles, { withTheme: true })(SelectLocale);