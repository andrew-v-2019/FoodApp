import { Component, OnInit } from '@angular/core';
import { MenuService }  from '../services/menu/menu.service';
import { Menu } from "app/models/menu/menu";

import { DatePickerOptions, DateModel } from 'ng2-datepicker';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import * as moment from 'moment';
import { Moment } from "moment";
import * as _ from "lodash";

const vsDateFormat: string = "YYYY-MM-DD";
const vmDateFormat: string = "DD.MM.YYYY";


@Component({
  selector: 'app-create-menu',
  templateUrl: './create-menu.component.html',
  styleUrls: ['./create-menu.component.css'],
  providers: [MenuService, BsDropdownModule]
})



export class CreateMenuComponent implements OnInit {

  constructor(private menuService:MenuService) {
    this.dateOptions = new DatePickerOptions({format:vmDateFormat, locale:'ru', selectYearText:'Год'});
   }
   menu :Menu;
   dateOptions :DatePickerOptions;
   date:DateModel;

  ngOnInit() {
    let observer = this.menuService.getLastMenu();   
    observer.subscribe(value =>this.map(value));
  }

  map(value:Menu){
    let dateModel:DateModel = new DateModel();
    let momentObj = moment(value.lunchDate,vsDateFormat);
    dateModel.momentObj = momentObj;
    dateModel.formatted = momentObj.format(vmDateFormat);
    this.date = dateModel;
    this.menu=value;   
  }

  getEmptyMenu(){
    this.menu = null;
    let observer = this.menuService.getEmptyMenu();   
    observer.subscribe(value =>this.map(value));
  }

  getTemplateMenu(){
    this.menu = null;
    let observer = this.menuService.getTemplateMenu();   
    observer.subscribe(value =>this.map(value));
  }

  save(){
    let m = this.menu;
    if (!m) return;
  
    var incorrectSection= _.find(m.sections, function(section){ return section.items.length && section.items[0].name.trim().length==0});
    if (incorrectSection){
      console.log('incorrect section');
      return;
    }
    let observer =this.menuService.updateMenu(m);
    observer.subscribe(value =>this.menuUpdateEvent(value));
  }

  menuUpdateEvent(r){
     
  }
}
