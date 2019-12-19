import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { AppService } from '../../services/app-service.service'
import { Store } from '../../app.interfaces'

@Component({
  selector: 'app-store-select',
  templateUrl: './store-select.component.html',
  styleUrls: ['./store-select.component.scss']
})
export class StoreSelectComponent implements OnInit {
  stores: Store[];
  isError: boolean = false;
  errorMessage: String;
  isLoading: boolean = false;
  userSelectedRegion: string;
  selectedStore: any;

  @Output() onStoreSelected = new EventEmitter<Store>();
  @Output() onNavigateBack = new EventEmitter();

  constructor(private appService: AppService) {
    this.stores = []
  }

  ngOnInit() {
    this.userSelectedRegion = this.appService.currentRegion();
    if(this.userSelectedRegion){
      this.getStores(this.userSelectedRegion)
    }
  }

  getStores(region: string){
    this.isLoading = true;
    this.isError = false;
    this.errorMessage = '';

    this.appService.getStoresByRegion(region)
      .subscribe(
        (data: Store[]) => {
          this.stores = data;
          this.isLoading = false;
        },
        (err: any) => {
          this.isLoading = false;
          this.isError = true;
          if(err.name === 'HttpErrorResponse'){
            this.errorMessage = 'Unable to connect to the server'
          }
          console.error(err)
        }
    )

  }

  setSelectedStore(store: UIEvent){
    this.selectedStore = store.detail;
  }

  continue(){
    this.onStoreSelected.emit(this.selectedStore)
  }

  goBack(){
    this.onNavigateBack.emit();
  }

}
