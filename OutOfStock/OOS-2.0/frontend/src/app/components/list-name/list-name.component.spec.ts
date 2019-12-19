import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { WfmUIModule } from '@wfm/ui-angular';

import { ListNameComponent } from './list-name.component';

describe('ListNameComponent', () => {
  let component: ListNameComponent;
  let fixture: ComponentFixture<ListNameComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [WfmUIModule.forRoot()],
      declarations: [
        ListNameComponent,
      ],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ListNameComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
