import { Component, OnInit, EventEmitter, Output } from '@angular/core';

const REGION_LIST = ['SP', 'MW', 'MA', 'SW', 'SO', 'NC', 'RM', 'NE', 'NA', 'FL', 'PN', 'UK']
@Component({
  selector: 'app-region-select',
  templateUrl: './region-select.component.html',
  styleUrls: ['./region-select.component.scss']
})
export class RegionSelectComponent implements OnInit {

  regions: any;
  selectedRegion: any;

  @Output() regionSelected = new EventEmitter<string>();

  constructor() {
    this.regions = REGION_LIST.sort();
  }

  ngOnInit() {}

  setSelected(selected){
    this.selectedRegion = selected.detail;
  }

  selectRegion(){
    this.regionSelected.emit(this.selectedRegion.name)
  }

}
