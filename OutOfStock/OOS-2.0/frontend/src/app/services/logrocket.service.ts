import { Injectable, NgZone } from '@angular/core';
import * as LogRocket from 'logrocket';

import { environment } from 'src/environments/environment';
import {  REDACTED } from 'src/app/constants';

@Injectable({
    providedIn: 'root',
})
export class LogRocketService {
    constructor(private zone: NgZone) { }

    init() {
        if (!!environment.logRocketId && !navigator.webdriver) {
            this.zone.runOutsideAngular(() => {
                LogRocket.init(environment.logRocketId, {
                    release: "latest",
                    network: {
                        requestSanitizer: (request) => {
                            // Redact authorization header for security
                            if (request.headers.Authorization) {
                                request.headers.Authorization = REDACTED;
                            }
                            // Redact request body for login
                            if (request.url.toLowerCase().indexOf('/api/login') !== -1) {
                                request.body = REDACTED;
                            }
                            // otherwise log the request normally
                            return request;
                        },
                        responseSanitizer: (response) => {
                            // Redact api body response for login
                            // @ts-ignore
                            if (response.url.toLowerCase().indexOf('/api/login') !== -1) {
                                response.body = REDACTED;
                            }
                            // otherwise log the response normally
                            return response;
                        },
                    },
                });
            });
        }
    }

    identify({  username, useremail, region, storeBU }) {
        LogRocket.identify(username, {
            username,
            useremail,
            region,
            storeBU
        });
    }
}
