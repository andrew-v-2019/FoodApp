import { NgModule }      from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { HomeComponent }  from './home/home.component';
import { CreateMenuComponent }  from './create-menu/create-menu.component';
import { EditLunchComponent }  from './edit-lunch/edit-lunch.component';
import { ManageOrdersComponent }  from './manage-orders/manage-orders.component';

const routes: Routes = [
  { path: 'home', component: HomeComponent },
  { path: 'create-menu', component: CreateMenuComponent },
  { path: 'edit-lunch', component: EditLunchComponent },
  { path: 'manage-orders', component: ManageOrdersComponent },
  { path: '', redirectTo: '/index ', pathMatch: 'full' },
  { path: '**', component: HomeComponent }
] 

@NgModule({
  imports: [ RouterModule.forRoot(routes) ],
  exports: [ RouterModule ]
})
export class AppRoutingModule{ } 