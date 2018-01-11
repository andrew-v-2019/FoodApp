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

import { Http, Response, Headers, RequestOptions } from '@angular/http';

import { HttpModule } from '@angular/http';
import { SectionEditorComponent } from './components/section-editor/section-editor.component';

import { DatePickerModule } from 'ng2-datepicker';

import { BsDropdownModule } from 'ngx-bootstrap/dropdown';

@NgModule({
  imports: [     
      BrowserModule,
	  FormsModule,
	  AppRoutingModule, 
	  HttpModule,
	  DatePickerModule,
	  BsDropdownModule.forRoot()
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
  providers: [ MenuService, LunchService ],
  bootstrap: [ AppComponent ]
})
export class AppModule { } 