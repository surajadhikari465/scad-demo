import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http'
import { WfmUIModule } from '@wfm/ui-angular'
import { IonicModule } from '@ionic/angular';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { SettingsComponent } from './pages/settings/settings.component';
import { ScanListComponent } from './pages/scan-list/scan-list.component';
import { RegionSelectComponent } from './components/region-select/region-select.component'
import { StoreSelectComponent } from './components/store-select/store-select.component';
import { AppToolbarComponent } from './components/app-toolbar/app-toolbar.component';
import { AppSettingsComponent } from './components/app-toolbar/app-settings/app-settings.component';
import { DataEntryComponent } from './components/data-entry/data-entry.component';
import { AlertModalComponent } from './components/alert-modal/alert-modal.component';
import { ListNameComponent } from './components/list-name/list-name.component';
import { StoresComponent } from './pages/settings/stores/stores.component';
import { RegionsComponent } from './pages/settings/regions/regions.component';

@NgModule({
  // Content components for overlays (modal, menu) need to
  // be added to both 'entryComponents' and declerations
  entryComponents:[AppSettingsComponent, DataEntryComponent, AlertModalComponent, ListNameComponent],
  declarations: [
    AppComponent,
    ScanListComponent,
    RegionSelectComponent,
    StoreSelectComponent,
    AppToolbarComponent,
    AppSettingsComponent,
    DataEntryComponent,
    AlertModalComponent,
    ListNameComponent,
    SettingsComponent,
    StoresComponent,
    RegionsComponent,
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    WfmUIModule.forRoot(),
    IonicModule.forRoot({mode: 'md'})
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
