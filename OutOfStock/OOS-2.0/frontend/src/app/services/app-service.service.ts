import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Store } from '../app.interfaces';

import { WFM_REGION, WFM_STORE } from 'src/app/constants';
import { environment } from '../../environments/environment';

const BASE_URL = environment.baseURL;
let allowScan = false;
let enableScanListMenuOptions = false;

@Injectable({
  providedIn: 'root'
})
export class AppService {
  constructor(private http: HttpClient) { }
  // API SERVICES
  submitListItems(wfmScanItems: string[]): Observable<any> {
    const url = `${BASE_URL}/items`;

    const wfmRegionId = this.currentRegion();
    const wfmStoreName = this.currentStore().name;

    if (!wfmScanItems) {
      throw new Error('submitListItems requires scan items');
    }

    const user = JSON.parse(localStorage.getItem('user'));

    return this.http.post(url, {
      regionCode: wfmRegionId,
      storeName: wfmStoreName,
      items: wfmScanItems,
      userName: user.samaccountname,
      userEmail: user.email
    });
  }

  getStoresByRegion(wfmRegionId: string): Observable<Store[]> {
    const url = `${BASE_URL}/stores/region/${wfmRegionId}`;

    return this.http.get<Store[]>(url);
  }

  saveItem(key: string, value: any) {
    if (typeof (value) === 'object') {
      value = JSON.stringify(value)
    }
    localStorage.setItem(key, value);
  }

  getItem(key: string): string {
    return localStorage.getItem(key);
  }

  currentStore(): Store {
    return JSON.parse(this.getItem(WFM_STORE));
  }

  currentRegion(): string {
    return this.getItem(WFM_REGION);
  }

  setScan(allowScanning: boolean) {
    console.log("scanning", allowScanning);
    allowScan = allowScanning;
  }

  getScan(): boolean {
    return allowScan;
  }

  getEnvironment(): string {
    return environment.name;
  }

  setEnableScanListMenuOptions(enabled: boolean) {
    enableScanListMenuOptions = enabled;
  }

  getEnableScanListMenuOptions() {
    return enableScanListMenuOptions;
  }
}
