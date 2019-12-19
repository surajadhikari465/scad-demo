import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { WfmUIModule } from '@wfm/ui-angular';

import { RegionSelectComponent } from './region-select.component';

describe('RegionSelectComponent', () => {
  let component: RegionSelectComponent;
  let fixture: ComponentFixture<RegionSelectComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [WfmUIModule.forRoot()],
      declarations: [ RegionSelectComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegionSelectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

