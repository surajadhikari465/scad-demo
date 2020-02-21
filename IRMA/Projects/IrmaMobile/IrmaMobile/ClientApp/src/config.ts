const Config =
{
    baseUrl: process.env.REACT_APP_BASE_URL,
    useAuthToken: (process.env.REACT_APP_USE_AUTH_TOKEN === 'true'),
    fakeUser: {
        "samaccountname": process.env.REACT_APP_SAM_ACCOUNT_NAME
    }
};

export default Config;