import { Component, OnInit } from '@angular/core';
import { ModalController } from '@ionic/angular'

@Component({
  selector: 'app-list-name',
  templateUrl: './list-name.component.html',
  styleUrls: ['./list-name.component.scss']
})
export class ListNameComponent implements OnInit {

  constructor(private modalController: ModalController) { }

  ngOnInit() {
  }

  editListName(value: string){
    this.modalController.dismiss(value, 'submit')
  }

}
