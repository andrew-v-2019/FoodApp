import { Component, OnInit } from '@angular/core';
import { MenuService } from '../services/menu/menu.service';
import { Menu } from "app/models/menu/menu";
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import * as moment from 'moment';
import { Moment } from "moment";
import * as _ from "lodash";


const vsDateFormat: string = "YYYY-MM-DD";


@Component({
  selector: 'app-create-menu',
  templateUrl: './create-menu.component.html',
  styleUrls: ['./create-menu.component.css'],
  providers: [MenuService, BsDropdownModule]
})



export class CreateMenuComponent implements OnInit {

  constructor(private menuService: MenuService) {

  }
  menu: Menu;
  date: Date;

  ngOnInit() {
    let observer = this.menuService.getLastMenu();
    observer.subscribe(value => this.map(value));
  }

  map(value: Menu) {
    let momentObj = moment(value.lunchDate, vsDateFormat).toString();
    this.date = new Date(momentObj);
    this.menu = value;
  }

  getEmptyMenu() {
    this.menu = null;
    let observer = this.menuService.getEmptyMenu();
    observer.subscribe(value => this.map(value));
  }

  getTemplateMenu() {
    this.menu = null;
    let observer = this.menuService.getTemplateMenu();
    observer.subscribe(value => this.map(value));
  }

  save() {
    
    let m = this.menu;
    let momentObj = moment(this.date, vsDateFormat).format(vsDateFormat);
    m.lunchDate =  momentObj;
  
    if (!m) return;
    var incorrectSection = _.find(m.sections, function (section) { return section.items.length && section.items[0].name.trim().length == 0 });
    if (incorrectSection) {
      console.log('incorrect section');
      return;
    }
    let observer = this.menuService.updateMenu(m);
    observer.subscribe(value => this.menuUpdateEvent(value));
  }

  menuUpdateEvent(r) {

  }
}
