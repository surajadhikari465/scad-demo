import { Component, OnInit } from '@angular/core';
import { AppService } from './services/app-service.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {

  constructor(private appService: AppService, private router: Router){}

  ngOnInit(){
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
}
