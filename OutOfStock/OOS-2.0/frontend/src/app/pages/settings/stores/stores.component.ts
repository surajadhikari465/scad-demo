import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Location } from '@angular/common'

import { Store } from 'src/app/app.interfaces';
import { AppService } from 'src/app/services/app-service.service'

@Component({
  selector: 'app-stores',
  templateUrl: './stores.component.html',
  styleUrls: ['./stores.component.scss']
})
export class StoresComponent {

  constructor(
    private appService: AppService,
    private router: Router,
    private location: Location
  ) { }

  ngOnInit(){
    if(localStorage.getItem('wfmStore')){
      this.router.navigateByUrl('/list');
    }  
  }

  saveStore(selectedStore: Store){
    this.appService.saveItem('wfmStore', selectedStore);
    this.appService.toggleScanOn();
    this.router.navigateByUrl('/list');
  }

  goBack(){
    this.location.back();
  }

}
