import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { MenuController } from '@wfm/ui-angular'
//import { PopoverController } from '@ionic/angular';

import { AppSettingsComponent } from './app-settings/app-settings.component'

@Component({
  selector: 'app-toolbar',
  templateUrl: './app-toolbar.component.html',
  styleUrls: ['./app-toolbar.component.scss']
})
export class AppToolbarComponent implements OnInit {

  @Output() onMenuDismissed = new EventEmitter();

  constructor(public menuController: MenuController) { }
  //constructor(public popoverController: PopoverController) { }

  ngOnInit() {}

  async presentMenu(ev: UIEvent){
    //const menu = await this.popoverController.create({
    const menu = await this.menuController.create({
      event: ev,
      component: AppSettingsComponent,
    })

    menu.onDidDismiss().then(selectedMenuOption => {
      this.onMenuDismissed.emit(selectedMenuOption)
    })

    menu.present()
  }
}
