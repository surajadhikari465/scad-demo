import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { MenuController } from '@wfm/ui-angular';
//import { PopoverController } from '@ionic/angular';

@Component({
  selector: 'app-settings',
  templateUrl: './app-settings.component.html',
  styleUrls: ['./app-settings.component.scss'],
})
export class AppSettingsComponent {

  constructor(private router: Router, private menuController: MenuController){}

  clearList(){
    this.menuController.dismiss('clearList')
  }

  changeStore(){
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
