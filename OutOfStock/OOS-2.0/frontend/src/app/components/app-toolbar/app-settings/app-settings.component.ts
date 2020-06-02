import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MenuController } from '@wfm/ui-angular';
import { AppService } from 'src/app/services/app-service.service'
import { AuthHandler } from '@wfm/mobile';

@Component({
  selector: 'app-settings',
  templateUrl: './app-settings.component.html',
  styleUrls: ['./app-settings.component.scss'],
})
export class AppSettingsComponent implements OnInit{
  enableScanListMenuOptions = false;
  constructor(private router: Router, private menuController: MenuController, private appService: AppService) { }

  ngOnInit(): void {
    this.enableScanListMenuOptions = this.appService.getEnableScanListMenuOptions();
  }

  
  clearList() {
    this.menuController.dismiss('clearList');
  }

  changeStore() {
    this.router.navigateByUrl('/settings');
    localStorage.removeItem('wfmRegion');
    localStorage.removeItem('wfmStore');
    localStorage.removeItem('items');
    this.menuController.dismiss();
  }

  manualDataEntry() {
    this.menuController.dismiss('enterManualData');
  }

  logout() {
    AuthHandler.clearToken(() => console.log('logout successful'));
  }
}
