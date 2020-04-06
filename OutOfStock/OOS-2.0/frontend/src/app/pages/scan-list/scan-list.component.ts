import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { ModalController } from '@wfm/ui-angular';
import {
  ToastController as IonicToastController,
  ModalController as IonicModalController,
} from '@ionic/angular';


import { AppService } from 'src/app/services/app-service.service'
import { DataEntryComponent } from 'src/app/components/data-entry/data-entry.component'
import { AlertModalComponent } from 'src/app/components/alert-modal/alert-modal.component'

import { Store } from 'src/app/app.interfaces'
import ScanCodeProcessor from 'src/scanning/ScanCodeProcessor';
// @ts-ignore 
import { BarcodeScanner, IBarcodeScannedEvent } from '@wfm/mobile';

@Component({
  selector: 'app-scan-list',
  templateUrl: './scan-list.component.html',
  styleUrls: ['./scan-list.component.scss']
})
export class ScanListComponent implements OnInit {
  private items: string[];
  public wfmStore: Store;
  public wfmRegion: string;
  public shouldBeDisabled: boolean = false;
  public isLoading: boolean = false;

  constructor(
    private appService: AppService,
    public toastController: IonicToastController,
    public ionicModalController: IonicModalController,
    public modalController: ModalController,
    private ref: ChangeDetectorRef,
  ) {
    this.items = []
  }

  ngOnInit() {
    const self = this;
    BarcodeScanner.registerHandler(function (data: IBarcodeScannedEvent) {
      const allowScan = self.appService.getScan();
      if (allowScan) {
        self.isLoading = true;
        try {
          let scanCode = ScanCodeProcessor.parseScanCode(data.Data, data.Symbology);
          self.addWfmItem(scanCode)
        } catch (ex) {
          alert(ex.message);
          self.isLoading = false;
        }
      }
    })
    this.wfmRegion = this.appService.currentRegion();
    this.wfmStore = this.appService.currentStore();
    if (localStorage.getItem('items')) {
      this.items = JSON.parse(localStorage.getItem('items'));
    }
  }

  executeMenuOption(option) {
    switch (option.data) {
      case 'clearList': {
        this.clearList();
        break;
      }
      case 'enterManualData': {
        this.openManualDataEntry();
        break;
      }
    }
  }

  addWfmItem(itemUPC: string) {
    try {

      let item = '0000000000000';
      if (itemUPC.length <= 13) {
        item = item.substring(0, item.length - itemUPC.length) + itemUPC;
      }
      else item = itemUPC;

      this.items.push(item);
      localStorage.setItem('items', JSON.stringify(this.items));
      this.isLoading = false;

      // since the scanning is being called
      // by the native app we need to tell
      // angular to check for changes in order
      // for the new item to render consistently
      this.ref.detectChanges()

      this.presentToast(`${itemUPC} added`)
    } catch (ex) {
      console.log(ex.stacktrace)
    }
  }

  removeItem(item: string) {
    const index = this.items.indexOf(item);
    this.items.splice(index, 1);
    localStorage.setItem('items', JSON.stringify(this.items));
    this.ref.detectChanges();
  }

  clearList() {
    this.items = []
    localStorage.setItem('items', JSON.stringify(this.items));
  }

  async openManualDataEntry() {
    const modal = await this.modalController.create({
      component: DataEntryComponent,
    });

    // Get the value from the modal
    modal.onDidDismiss().then(value => {
      const { data, role } = value
      if (data && role === 'submit') {
        this.addWfmItem(data);
      }
    })

    modal.present();
  }

  async presentToast(message: string) {
    const toast = await this.toastController.create({
      message,
      duration: 1500,
      position: 'top',
    });
    toast.present();
  }

  displaySuccessModal() {
    this.displayModal('Success', 'You successfully submitted your list!')
  }

  displayErrorModal() {
    this.displayModal('Error!', 'There was an error saving your list')
  }

  async displayModal(title: string, message: string) {
    const modal = await this.modalController.create({
      component: AlertModalComponent,
      componentProps: { title, message }
    });

    modal.present()
  }

  submitListItems() {
    this.isLoading = true;
    this.appService.submitListItems(this.items)
      .subscribe({
        next: () => {
          this.isLoading = false;
          this.displaySuccessModal();
          this.clearList();
        },
        error: () => {
          this.isLoading = false;
          this.displayErrorModal();
        }
      })
  }

}
