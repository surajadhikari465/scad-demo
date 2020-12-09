//@ts-ignore
import LogRocket from 'logrocket';

const REDACTED = 'REDACTED FROM LOGS';

export const logRocketInitOptions = {
  release: "1.0.0",
  network: {
    requestSanitizer: (request: any) => {
      // Ignore healthcheck requests
      if (request.url.toLowerCase().indexOf('healthcheck') !== -1) {
        return null;
      }

      // Redact authorization header for security
      if (request.headers['authorization']) {
        request.headers['authorization'] = REDACTED;
      }

      // otherwise log the request normally
      return request;
    },
    responseSanitizer: (response: any) => {
      // Ignore healthcheck requests
      if (response.url.toLowerCase().indexOf('healthcheck') !== -1) {
        return null;
      }

      // otherwise log the response normally
      return response;
    },
  },
};
  
interface IUser {
  wfm_employee_id: string,
  name: string,
  email: string,
  given_name: string,
  family_name: string,
  wfm_location: {
    id: string
  }
}
export const identifyLogRocketUser = (user: IUser) => {
  try{
    const {
      wfm_employee_id,
      name,
      email,
      given_name,
      family_name,
      wfm_location
    } = user;

    LogRocket.identify(wfm_employee_id, {
      name: name,
      email: email,
      firstName: given_name,
      lastName: family_name,
      storeId: wfm_location && wfm_location.id,
    });
  }catch({message}){
    console.error(`Unable to identify application user: ${message}`);
  }
}