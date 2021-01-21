const Config =
{
    baseUrl: process.env.REACT_APP_BASE_URL,
    useAuthToken: (process.env.REACT_APP_USE_AUTH_TOKEN === 'true'),
    fakeUser: {
        "SamAccountName": "Min.Zhao",
        "wfm_employee_id": "1111111",
        "email": "min.zhao@wholefoods.com",
        "name": "Min Zhao",
        "given_name": "Min",
        "family_name": "Zhao",
        "wfm_location": {
            "id": "50001",
            "code": "CEN",
            "name": "Central Support"
        },
    },
    logRocketId: process.env.REACT_APP_LOGROCKET_ID
};

export default Config;