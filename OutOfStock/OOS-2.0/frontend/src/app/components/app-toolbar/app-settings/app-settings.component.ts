import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { MenuController } from '@wfm/ui-angular';
import { AppService } from 'src/app/services/app-service.service'
//import { PopoverController } from '@ionic/angular';

@Component({
  selector: 'app-settings',
  templateUrl: './app-settings.component.html',
  styleUrls: ['./app-settings.component.scss'],
})
export class AppSettingsComponent {

  constructor(private router: Router, private menuController: MenuController, private appService: AppService){}

  clearList(){
    this.menuController.dismiss('clearList')
  }

  changeStore(){
    this.appService.toggleScan();
    this.router.navigateByUrl('/settings')
    localStorage.removeItem('wfmRegion');
    localStorage.removeItem('wfmStore');
    localStorage.removeItem('items');
    this.menuController.dismiss()
  }

  manualDataEntry(){
    this.menuController.dismiss('enterManualData')
  }
}
