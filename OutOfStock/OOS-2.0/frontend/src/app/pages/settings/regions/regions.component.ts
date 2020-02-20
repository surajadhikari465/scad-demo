import { Component } from '@angular/core';
import { AppService } from 'src/app/services/app-service.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-regions',
  templateUrl: './regions.component.html',
  styleUrls: ['./regions.component.scss']
})
export class RegionsComponent {

  constructor(private appService: AppService, private router: Router) { }

  ngOnInit() {
    if (localStorage.getItem('wfmRegion') && localStorage.getItem('wfmStore')) {
      this.router.navigateByUrl('/list');
    }
  }

  setRegion(selectedRegion) {
    this.appService.saveItem('wfmRegion', selectedRegion);
    this.router.navigateByUrl('/settings/stores');
  }
}