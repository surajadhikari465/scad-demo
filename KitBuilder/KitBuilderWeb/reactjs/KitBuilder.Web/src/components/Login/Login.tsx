import * as React from 'react';
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
//import crypto from 'crypto-js';
import Axios from 'axios';
import { KbApiMethod } from "../helpers/kbapi";

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
    loginDetails: LoginDetails
}
interface ILoginProps {
    onChange: (e: any) => void,
    classes: any
}

class LoginPage extends React.Component<ILoginProps, ILoginPageState> {


    constructor(props: any) {
        super(props);
        this.state = {
            loginDetails: new LoginDetails()
        }
    }

    onLoginInputChange = (event: any) => {
        var temp = this.state.loginDetails
        temp[event.target.name] = event.target.value;
        this.setState({ ...this.state, loginDetails: temp });

    }
    onSubmit = (e: any) => {
        e.preventDefault();
        var details = this.state.loginDetails;
        console.log(details);
        Axios.post(KbApiMethod("Login"),{
            Username: details.username, 
            Password: details.password
        }).then((d) => {
            console.log(d);
        })
    }

    render() {
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
                    <form className={this.props.classes.form}>
                        <FormControl margin="normal" required fullWidth>
                            <InputLabel htmlFor="username" >Email Address</InputLabel>
                            <Input id="email" name="username" autoComplete="email" autoFocus onChange={this.onLoginInputChange} />
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


export default withStyles(styles)(LoginPage);