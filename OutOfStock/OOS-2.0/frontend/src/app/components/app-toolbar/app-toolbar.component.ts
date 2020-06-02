import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { MenuController, ModalController } from '@wfm/ui-angular'
import { environment } from '../../../environments/environment'
import { AlertModalComponent } from 'src/app/components/alert-modal/alert-modal.component';
import { AppSettingsComponent } from './app-settings/app-settings.component'

@Component({
  selector: 'app-toolbar',
  templateUrl: './app-toolbar.component.html',
  styleUrls: ['./app-toolbar.component.scss']
})
export class AppToolbarComponent implements OnInit {

  private tapCount: number;

  @Output() onMenuDismissed = new EventEmitter();

  constructor(public menuController: MenuController,
    public modalController: ModalController) { }

  ngOnInit() {
    this.tapCount = 0;
  }

  tap() {
    this.tapCount += 1;
    if (this.tapCount >= 5) {
      var msg = JSON.stringify({ baseUrl: environment.baseURL}, null, 2);
      this.displayModal("Environment Info", msg);
      this.tapCount = 0;
    }
  }

  async displayModal(title: string, message: string) {
    const modal = await this.modalController.create({
      component: AlertModalComponent,
      componentProps: { title, message }
    });

    modal.present()
  }

  async presentMenu(ev: UIEvent) {
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
