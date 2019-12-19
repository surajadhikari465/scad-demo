import { Component, OnInit, ViewChild } from '@angular/core';
import { ModalController } from '@wfm/ui-angular';

@Component({
  selector: 'app-data-entry',
  templateUrl: './data-entry.component.html',
  styleUrls: ['./data-entry.component.scss']
})
export class DataEntryComponent implements OnInit {


  constructor(private modalController: ModalController) { }

  ngOnInit() {}

  addUPCEntry(value){
    if(isNaN(value)){
      alert('values must be numbers')
      return
    }
    this.modalController.dismiss(value, 'submit')
  }

  dismiss(){
    this.modalController.dismiss()
  }

}
