const Config =
{
    baseUrl: process.env.REACT_APP_BASE_URL,
    useAuthToken: (process.env.REACT_APP_USE_AUTH_TOKEN === 'true'),
    fakeUser: {
        "samaccountname": "Blake.Jones",
        "wfm_employee_id": "1111111",
        "email": "blake.jones@wholefoods.com",
        "name": "Blake Jones (1111111)",
        "given_name": "Blake",
        "family_name": "Jones",
        "wfm_location": {
            "id": "50001",
            "code": "CEN",
            "name": "Central Support"
        },
    },
    logRocketId: process.env.REACT_APP_LOGROCKET_ID
};

export default Config;