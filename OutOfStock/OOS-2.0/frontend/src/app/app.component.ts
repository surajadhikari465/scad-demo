import { Component, OnInit } from '@angular/core';
import { AppService } from './services/app-service.service';
import { Router, NavigationEnd } from '@angular/router';
import { filter } from 'rxjs/operators';
import { LifecycleManager, AuthHandler } from '@wfm/mobile';
import decode from 'jwt-decode';
import { environment } from '../environments/environment';
import { LogRocketService } from './services/logrocket.service';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {

  constructor(private appService: AppService, private router: Router, private logRocket: LogRocketService) { }

  ngOnInit() {
    if (environment.useAuthToken) {
      LifecycleManager.onReady(function () {
        try {
          AuthHandler.onTokenReceived(function (token: string) {
            let decodedToken = decode(token);
            this.appService.saveItem('user', decodedToken);
            this.logRocket.init();
          }.bind(this));
        } catch (err) {
          console.error(err);
        }
      }.bind(this));
    } else {
      this.appService.saveItem('user', environment.fakeUser);
    }

    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe((event) => { this.processNavigationEvent(event); });

    const wfmRegion = this.appService.currentRegion();
    const wfmStore = this.appService.currentStore();

    if (!wfmRegion) {

      this.router.navigateByUrl('/settings');
      return
    }

    if (!wfmStore) {
      this.router.navigateByUrl('/settings/stores');
      return
    }
  }

  processNavigationEvent(event: any) {
    if (event.url == "/list") {      
      this.appService.setScan(true);
      this.appService.setEnableScanListMenuOptions(true);

      const wfmRegion = this.appService.currentRegion();
      const wfmStore = this.appService.currentStore();
      const user = JSON.parse(localStorage.getItem('user'));

      this.logRocket.init();
      var identity = {
        username: user.samaccountname, 
        useremail: user.email, 
        region: wfmRegion, 
        storeBU: wfmStore
      }

      this.logRocket.identify(identity)
      
      

    } else {
      this.appService.setScan(false);
      this.appService.setEnableScanListMenuOptions(false);

    }
  }
}


