import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { WfmUIModule } from '@wfm/ui-angular';

import { AppToolbarComponent } from './app-toolbar.component';

describe('AppToolbarComponent', () => {
  let component: AppToolbarComponent;
  let fixture: ComponentFixture<AppToolbarComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AppToolbarComponent ],
      imports: [
        WfmUIModule.forRoot()
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AppToolbarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

