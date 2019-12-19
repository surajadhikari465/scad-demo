import { Component, OnInit, Input } from '@angular/core';
import { ModalController } from '@wfm/ui-angular';

@Component({
  selector: 'app-alert-modal',
  templateUrl: './alert-modal.component.html',
  styleUrls: ['./alert-modal.component.scss']
})
export class AlertModalComponent implements OnInit {
  @Input() title: string;
  @Input() message: string;

  constructor(private modalController: ModalController) { }

  ngOnInit() {}

  close(){
    this.modalController.dismiss()
  }

}
