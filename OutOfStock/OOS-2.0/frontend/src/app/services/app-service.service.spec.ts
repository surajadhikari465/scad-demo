import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { TestBed, getTestBed } from '@angular/core/testing';

import { AppService } from './app-service.service';
import { BASE_URL } from 'src/app/constants';

describe('AppService', () => {
  let injector: TestBed;
  let service: AppService;
  let httpMock: HttpTestingController;

  beforeEach(() => TestBed.configureTestingModule({
    imports: [ HttpClientTestingModule ],
    providers: [AppService],
  }));

  beforeEach(() => {
    injector = getTestBed();
    service = injector.get(AppService);
    httpMock = injector.get(HttpTestingController);

    // Creates a mock localStorage used by the AppService
    let store = {};
    const mockLocalStorage = {
      getItem: (key: string): string => {
        return key in store ? store[key] : null
      },
      setItem: (key: string, value: string) => {
        store[key] = value;
      }
    }

    // Jasmine Spy. If 'localStorage' is called, use the mockLocalStorage
    // instead
    spyOn(localStorage, 'getItem').and.callFake(mockLocalStorage.getItem);
    spyOn(localStorage, 'setItem').and.callFake(mockLocalStorage.setItem);
  })

  afterEach(() => {
    httpMock.verify();
  });

  describe('#getStoresByRegion', () => {
    it('should return an Observable<Store[]>', () => {
      const mockStores = [
        {name: 'Store A', number: 1234},
        {name: 'Store B', number: 5432}
      ]

      service.getStoresByRegion('SP').subscribe(stores => {
        expect(stores.length).toBe(2);
        expect(stores).toEqual(mockStores);
      })

      const req = httpMock.expectOne(`${BASE_URL}/stores/region/SP`);
      expect(req.request.method).toBe('GET');
      req.flush(mockStores);
    })
  });

  describe('#submitListItems', () => {
    it('should return an Observable<any>', () => {
      //Setup our mock storage values for the service to pull from
      localStorage.setItem('wfmRegion', 'SP');
      localStorage.setItem('wfmStore', '{"name":"Dundler Mifflin", "number": "234123"}')

      service.submitListItems('foo', ['3123123', '3123123123']).subscribe()

      const req = httpMock.expectOne(`${BASE_URL}/items`);
      expect(req.request.method).toBe('POST');
    })
  });

  describe('#saveItem', () => {
    it('should save a value to localStorage', () => {
      service.saveItem('testItem', 'testValue');

      expect(localStorage.getItem('testItem')).toEqual('testValue');
    })

    it('should save an object as a string value', () => {
      const objectValue = {foo: 'bar', buzz: 'bazz'};
      service.saveItem('testObject', objectValue);

      expect(localStorage.getItem('testObject'))
      .toEqual(JSON.stringify(objectValue))
    })
  });

  describe('#getItem', () => {
    it('should return a value for a given key from localStorage', () => {
      localStorage.setItem('foo', 'bar');

      expect(service.getItem('foo')).toEqual('bar');
    })
  });

  describe('#currentStore', () => {
    it('should return the current store saved in localStorage', () => {
      const store = {name:"Dundler Mifflin", number: 123123 };
      localStorage.setItem('wfmStore', JSON.stringify(store))

      expect(service.currentStore()).toEqual(store);
    })
  });

  describe('#currentRegion', () => {
    it('should return the current region save in localStorage', () => {
      const region = 'SP';
      localStorage.setItem('wfmRegion', region);

      expect(service.currentRegion()).toEqual(region);
    })
  });
});
