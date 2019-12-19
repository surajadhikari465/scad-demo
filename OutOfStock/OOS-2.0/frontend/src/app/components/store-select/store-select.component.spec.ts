import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { WfmUIModule } from '@wfm/ui-angular';
import { StoreSelectComponent } from './store-select.component';
import { from } from 'rxjs';
import { AppService } from 'src/app/services/app-service.service';

describe('StoreSelectComponent', () => {
  let component: StoreSelectComponent;
  let fixture: ComponentFixture<StoreSelectComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [WfmUIModule.forRoot()],
      declarations: [ StoreSelectComponent ],
      providers: [
        {provide: AppService, useValue: {
          currentRegion: () => 'SP',
          getStoresByRegion: () => from(
            [
              {name: 'mockStore', number: 1234},
              {name: 'mockStore2', number: 54634},
            ]
          )
        }}
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StoreSelectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

