import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { WfmUIModule } from '@wfm/ui-angular';

import { DataEntryComponent } from './data-entry.component';

describe('DataEntryComponent', () => {
  let component: DataEntryComponent;
  let fixture: ComponentFixture<DataEntryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DataEntryComponent ],
      imports : [
        WfmUIModule.forRoot()
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DataEntryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
