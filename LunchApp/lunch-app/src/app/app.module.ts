import { NgModule }   from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule }   from '@angular/forms';


import { AppComponent }  from './app.component';
import { HomeComponent }  from './home/home.component';
import { CreateMenuComponent }  from './create-menu/create-menu.component';
import { EditLunchComponent }  from './edit-lunch/edit-lunch.component';
import { ManageOrdersComponent }  from './manage-orders/manage-orders.component';
import { AppRoutingModule }  from './app-routing.module';

import { NavComponent }  from './nav/nav.component';


import { MenuService }  from './services/menu/menu.service';
import { LunchService }  from './services/lunch/lunch.service';
import { OrderService }  from './services/order/order.service';

import { Http, Response, Headers, RequestOptions } from '@angular/http';

import { HttpModule } from '@angular/http';
import { SectionEditorComponent } from './components/section-editor/section-editor.component';


import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import {ResourceModule} from 'ngx-resource';

import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {ToastModule} from 'ng2-toastr/ng2-toastr';



@NgModule({
  imports: [     
      BrowserModule,
	  FormsModule,
	  AppRoutingModule, 
	  HttpModule,
	  ResourceModule.forRoot(),
	  BsDropdownModule.forRoot(),
	  BrowserAnimationsModule,
	  ToastModule.forRoot()
  ],
  declarations: [
      AppComponent, 
	  HomeComponent,
	  CreateMenuComponent,
	  EditLunchComponent,
	  ManageOrdersComponent,
	  HomeComponent,
	  CreateMenuComponent,
	  EditLunchComponent,
	  ManageOrdersComponent,
	  NavComponent,
	  SectionEditorComponent
  ],
  providers: [ MenuService, LunchService, OrderService ],
  bootstrap: [ AppComponent ]
})
export class AppModule { } 