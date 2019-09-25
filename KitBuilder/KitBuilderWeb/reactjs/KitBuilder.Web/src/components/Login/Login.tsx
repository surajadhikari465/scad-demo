import * as React from 'react';
import { Redirect } from 'react-router-dom';
import Avatar from '@material-ui/core/Avatar';
import Button from '@material-ui/core/Button';
import CssBaseline from '@material-ui/core/CssBaseline';
import FormControl from '@material-ui/core/FormControl';
import Input from '@material-ui/core/Input';
import InputLabel from '@material-ui/core/InputLabel';
import LockOutlinedIcon from '@material-ui/icons/LockOutlined';
import Paper from '@material-ui/core/Paper';
import Typography from '@material-ui/core/Typography';
import withStyles from '@material-ui/core/styles/withStyles';
import Axios from 'axios';
import { KbApiMethod } from "../helpers/kbapi";
import { FormHelperText, Grid } from "@material-ui/core";
import { connect } from 'react-redux';
import { loggedInState } from 'src/redux/reducers/logInReducer';
import { Dispatch } from 'redux';

const styles = (theme: any) => ({
    main: {
        width: 'auto',
        display: 'block', // Fix IE 11 issue.
        marginLeft: theme.spacing.unit * 3,
        marginRight: theme.spacing.unit * 3,
        [theme.breakpoints.up(400 + theme.spacing.unit * 3 * 2)]: {
            width: 400,
            marginLeft: 'auto',
            marginRight: 'auto',
        },
    },
    paper: {
        marginTop: theme.spacing.unit * 8,
        display: 'flex',
        flexDirection: 'column' as 'column',
        alignItems: 'center',
        padding: `${theme.spacing.unit * 2}px ${theme.spacing.unit * 3}px ${theme.spacing.unit * 3}px`,
    },
    avatar: {
        margin: theme.spacing.unit,
        backgroundColor: theme.palette.secondary.main,
    },
    form: {
        width: '100%', // Fix IE 11 issue.
        marginTop: theme.spacing.unit,
    },
    submit: {
        marginTop: theme.spacing.unit * 3,
    },
});
class LoginDetails {
    username: string;
    password: string;
    constructor() { }
}

interface ILoginPageState {
    loginDetails: LoginDetails,
    loggedInState: loggedInState, 
    error: string,
    loggedIn: boolean
}
interface ILoginProps {
    onChange: (e: any) => void,
    updateRedux(loggedIn:boolean): void;
    currentURL:any,
    classes: any
}

class LoginPage extends React.Component<ILoginProps, ILoginPageState> {


    constructor(props: any) {
        super(props);
        this.state = {
            loginDetails: new LoginDetails(),
            loggedInState: { loggedIn: false, url:""},
            error:  "",
            loggedIn: false
        }
    }

    onLoginInputChange = (event: any) => {
        var temp = this.state.loginDetails
        temp[event.target.name] = event.target.value;
        this.setState({ ...this.state, loginDetails: temp, error: "", loggedIn: false });
    }

    onSubmit = (e: any) => {
        e.preventDefault();    

        var details = this.state.loginDetails;
        if( details.username && details.password)
        {
            Axios.post(KbApiMethod("Login"),{
                Username: details.username, 
                Password: details.password
            }).then((d) => {
                if(d.data=="Unauthenticated")
                    this.setState( { loggedIn: false, error: "User cannot be authenticated. Please enter a valid user name and password."})
                else if(d.data=="Unauthorizeded")
                    this.setState( { loggedIn: false, error: "User is not authorized for the Kit Builder access. "})
                else if(d.data=="Authorized")
                    this.setLoggedInState()
                else
                    this.setState( { loggedIn: false, error: "Something wrong with the KB Web API. User's permission cannot be determined at this time. "})
            })
        }
    }

    setLoggedInState = () => {
        this.setState( { loggedIn: true, error: ""},()=>{
            let loggedIn = this.state.loggedIn;
            this.props.updateRedux(loggedIn)});
    };

    generateErrorElements = () => {
        return <FormHelperText error={true}>{this.state.error}</FormHelperText> 
      };

    render() {
        if(this.state.loggedIn)
        {
            let url = ".."
            if (this.props.currentURL)
                url = url + this.props.currentURL;
            else
                url = url + "/Instructions"
            
            return <Redirect to= {url} />
        }
        
        const errorElements = this.generateErrorElements();
        return (
            <main className={this.props.classes.main}>
                <CssBaseline />
                <Paper className={this.props.classes.paper}>
                    <Avatar className={this.props.classes.avatar}>
                        <LockOutlinedIcon />
                    </Avatar>
                    <Typography component="h1" variant="h5">
                        Sign in
                    </Typography>
                    <Grid item xs={12}>
                        {errorElements}
                    </Grid>
                    <form className={this.props.classes.form}>
                        <FormControl margin="normal" required fullWidth>
                            <InputLabel htmlFor="username" >User Name</InputLabel>
                            <Input id="username" name="username" autoComplete="username" autoFocus onChange={this.onLoginInputChange} />
                        </FormControl>
                        <FormControl margin="normal" required fullWidth>
                            <InputLabel htmlFor="password">Password</InputLabel>
                            <Input name="password" type="password" id="password" autoComplete="current-password" onChange={this.onLoginInputChange} />
                        </FormControl>
                        <Button
                            type="button"
                            fullWidth
                            variant="contained"
                            color="primary"
                            className={this.props.classes.submit}
                            onClick={this.onSubmit}
                        >
                            Sign in
                </Button>
                    </form>
                </Paper>
            </main>


        )
    }
}

const mapStateToProps = (state: any) => {
return  {
    loggedInState: state.logInReducer,
    currentURL:state.logInReducer.url
    }
};

const mapDispatchToProps = (dispatch: Dispatch) => ({
    updateRedux: (loggedIn:Boolean) => dispatch({ type: "LOGGED_IN", payload: {loggedIn}}),
})

export default connect(mapStateToProps, mapDispatchToProps)(withStyles(styles)(LoginPage));