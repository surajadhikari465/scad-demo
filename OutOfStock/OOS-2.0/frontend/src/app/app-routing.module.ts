import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { SettingsComponent } from './pages/settings/settings.component';
import { ScanListComponent } from './pages/scan-list/scan-list.component';
import { StoresComponent } from './pages/settings/stores/stores.component';
import { RegionsComponent } from './pages/settings/regions/regions.component'

const routes: Routes = [
  {
    path: 'settings',
    component: SettingsComponent,
    children:[
      { path: '', component: RegionsComponent},
      { path: 'stores', component: StoresComponent}
    ]},
  {
    path: 'list',
    component: ScanListComponent
  },
  { path: '',
    redirectTo: '/settings',
    pathMatch: 'full'
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
