import { Component, OnInit, ViewContainerRef } from '@angular/core';
import { MenuService } from '../services/menu/menu.service';
import { Menu } from "app/models/menu/menu";
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import * as moment from 'moment';
import { Moment } from "moment";
import * as _ from "lodash";
import { ToastsManager } from 'ng2-toastr/ng2-toastr';


const vsDateFormat: string = "YYYY-MM-DD";


@Component({
  selector: 'app-create-menu',
  templateUrl: './create-menu.component.html',
  styleUrls: ['./create-menu.component.css'],
  providers: [MenuService, BsDropdownModule]
})

export class CreateMenuComponent implements OnInit {

  constructor(private toastr: ToastsManager, private menuService: MenuService, private vcr: ViewContainerRef) {
    this.toastr.setRootViewContainerRef(vcr);
  }
  menu: Menu;
  date: Date;
  loading: boolean;

  ngOnInit() {
    this.loading = true;
    let observer = this.menuService.getLastMenu();
    observer.subscribe(value => this.map(value), err => this.error(err));
  }

  error(err: any) {
    var er = err.json();
    this.toastr.error(er.Message, 'Ошибка', { enableHTML: true, animate: 'flyRight', showCloseButton: true });
    this.loading = false;
  }

  map(value: Menu) {
    let momentObj = moment(value.lunchDate, vsDateFormat).toString();
    this.date = new Date(momentObj);
    this.menu = value;
    this.loading = false;
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
    if (!m) return;
    let momentObj = moment(this.date, vsDateFormat).format(vsDateFormat);
    m.lunchDate = momentObj;
    var incorrectSection = _.find(m.sections, function (section) { return section.items.length && section.items[0].name.trim().length == 0 });
    if (incorrectSection) {
      return;
    }
    this.loading = true;
    let observer = this.menuService.updateMenu(m);
    observer.subscribe(value => this.menuUpdateEvent(value), err => this.error(err));
  }

  menuUpdateEvent(r) {
    this.toastr.success('Сохранено', null, { enableHTML: true, animate: 'flyRight', showCloseButton: true });
    this.loading = false;
  }
}
