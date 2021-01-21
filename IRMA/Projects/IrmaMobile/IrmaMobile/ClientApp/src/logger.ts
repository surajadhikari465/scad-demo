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
  EmployeeId: string,
  SamAccountName: string,
  DisplayName: string,
  Email: string,
  GivenName: string,
  FamilyName: string,
  wfm_location: {
    id: string
  },
  LocationId: string
}
export const identifyLogRocketUser = (user: IUser) => {
  try{
    const {
      EmployeeId,
      SamAccountName,
      DisplayName,
      Email,
      GivenName,
      FamilyName,
      wfm_location,
      LocationId
    } = user;
    
    LogRocket.identify(EmployeeId, {
      name: DisplayName,
      accountName: SamAccountName,
      email: Email,
      firstName: GivenName,
      lastName: FamilyName,
      storeId: wfm_location? wfm_location.id : LocationId
    });
  
  }catch({message}){
    console.error(`Unable to identify application user: ${message}`);
  }
}