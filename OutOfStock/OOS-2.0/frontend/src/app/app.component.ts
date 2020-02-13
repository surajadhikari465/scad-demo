import { Component, OnInit } from '@angular/core';
import { AppService } from './services/app-service.service';
import { Router, NavigationStart, NavigationEnd } from '@angular/router';
import { Observable } from 'rxjs';
import { filter } from 'rxjs/operators';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {

  constructor(private appService: AppService, private router: Router) {}

  ngOnInit() {

    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe((event) => {this.processNavigationEvent(event);});
    
    const wfmRegion = this.appService.currentRegion();
    const wfmStore = this.appService.currentStore();

    if(!wfmRegion){
      
      this.router.navigateByUrl('/settings');
      return
    }

    if(!wfmStore){
      this.router.navigateByUrl('/settings/stores');
      return
    }
  }

  processNavigationEvent(event: any) {
    if (event.url == "/list") 
      this.appService.setScan(true)
    else 
    this.appService.setScan(false)
  }
}


 