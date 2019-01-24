import * as React from 'react'
import Modal from '@material-ui/core/Modal';
import Card from '@material-ui/core/Card';
import Button from '@material-ui/core/Button';
import CardActions from '@material-ui/core/CardActions';
import CardContent from '@material-ui/core/CardContent';
import TextField from '@material-ui/core/TextField';
import Typography from '@material-ui/core/Typography';
import Grid from '@material-ui/core/Grid';
import LinearProgress from '@material-ui/core/LinearProgress';
import Fade from '@material-ui/core/Fade';
import axios from 'axios'
import { KbApiMethod } from '../helpers/kbapi';

interface IProps {
    open: boolean,
    closeModal(): void
    onCreated(id: number): void
}
interface IState {
    LinkGroupName: string,
    LinkGroupDesc: string,
    error: string,
    loading: boolean
}

const ModalStyle = { margin: "auto", marginTop: "200px", width: "400px" }
const Hidden = { display: "none" }
const ButtonStyle = { width: "150px" }
const ErrorStyle = { color: "red" }

export class NewLinkGroup {
    GroupName: string
    GroupDescription: string
}

export class AddNewLinkGroupModal extends React.Component<IProps, IState> {

    constructor(props: any) {
        super(props);

        this.state = {
            LinkGroupDesc: "",
            LinkGroupName: "",
            error: "",
            loading: false
        }

        this.handleChange = this.handleChange.bind(this);
        this.escFunction = this.escFunction.bind(this);
        this.close = this.close.bind(this);
        this.save= this.save.bind(this);
        this.reset= this.reset.bind(this);
        this.saveNewLinkGroup = this.saveNewLinkGroup.bind(this);
    }


    componentDidMount() {
        document.addEventListener("keydown", this.escFunction, false);
        this.setState({error: ""})
    }

    handleChange(event: any) {
        this.setState({ ...this.state, [event.target.name]: event.target.value });
    }

    reset() {
        this.setState({error: "", loading:false})
    }

    close() {
        if (!this.state.loading)
            this.reset();
            this.props.closeModal();
    }

    save() {
     
        var linkGroup:NewLinkGroup = {
            GroupName: this.state.LinkGroupName, 
            GroupDescription: this.state.LinkGroupDesc
        }

        if (linkGroup.GroupName === "" || linkGroup.GroupDescription === "") 
        {
            this.setState({ error: "Group Name and Description are required.", loading: false})         
            return;   
        } else {
        
            this.saveNewLinkGroup(linkGroup)
            .then(result => {
                this.setState({  loading: false  })
                this.close()
                this.props.onCreated(Number(result))
            })
            .catch(error => {
                this.setState({ loading: false, error: error  })
            })
        }
    }

    saveNewLinkGroup( newlinkGroup: NewLinkGroup ) {
        return new Promise((resolve, reject) => {
             axios.post(KbApiMethod("LinkGroups"), {
                    GroupName: newlinkGroup.GroupName, 
                    GroupDescription: newlinkGroup.GroupDescription, 
             }).then(res => {
                console.log(res.data)
                 resolve(res.data.linkGroupId);
             }).catch(error => {
                 console.log(error)
                 reject(error.message);
             });
        })
    }


    escFunction(event: any) {
        if (event.keyCode === 27) {
            this.close();
        }
    }


    render() {
        return (
            <React.Fragment>
                <Modal
                    style={ModalStyle}
                    open={this.props.open}
                >
                    <Card >
                        <CardContent>
                            <Typography>Create a new Link Group</Typography>
                            <div>
                                <TextField id="LinkGroupName"
                                    label="Link Group Name"
                                    name="LinkGroupName"
                                    fullWidth
                                    onChange={this.handleChange}
                                    inputRef={(input) => { if (!input) return; input.focus() }}
                                />
                            </div>
                            <div>
                                <TextField id="LinkGroupDesc"
                                    label="Link Group Description"
                                    name="LinkGroupDesc"
                                    fullWidth
                                    onChange={this.handleChange}
                                />
                            </div>                   
                        </CardContent>
                        <CardActions>
                            <Grid container spacing={24} justify="space-between">
                                <Grid item md={12} style={ this.state.loading  ?  undefined : Hidden }>
                                    <Fade
                                        in={this.state.loading}
                                        style={{ transitionDelay: this.state.loading ? '0ms' : '0ms', }}
                                        unmountOnExit
                                    >
                                        <LinearProgress />
                                    </Fade>
                                </Grid>
                                <Grid item md={12} style={ this.state.error === ""  ?  Hidden : ErrorStyle } >
                                    <span>{this.state.error}</span>
                                </Grid>
                                <Grid item md={12} >
                                    <Grid container spacing={24} justify="space-between">
                                        <Button
                                            style={ButtonStyle}
                                            size="small"
                                            variant="outlined"
                                            disabled={this.state.loading}
                                            onClick={() => { this.close() }} >
                                            Cancel (ESC)
                                        </Button>

                                        <Button
                                            style={ButtonStyle}
                                            size="small"
                                            variant="outlined"
                                            disabled={this.state.loading}
                                            onClick={() => { this.save() }}>
                                            Save
                                        </Button>
                                    </Grid>
                                </Grid>
                            </Grid>
                        </CardActions>
                    </Card>
                </Modal>
            </React.Fragment>
        )
    }
}