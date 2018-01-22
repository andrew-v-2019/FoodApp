import { Component, OnInit, ViewContainerRef } from '@angular/core';
import { MenuService } from '../services/menu/menu.service';
import { Menu } from "app/models/menu/menu";
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import * as moment from 'moment';
import { Moment } from "moment";
import * as _ from "lodash";
import { ToastsManager } from 'ng2-toastr/ng2-toastr';
import { Constants } from 'app/Constants';
import { MenuItem } from 'app/models/menu/menu-item';


import { Http, Headers } from '@angular/http';
import 'rxjs/add/operator/toPromise';
import { saveAs } from 'file-saver/FileSaver';


@Component({
  selector: 'app-create-menu',
  templateUrl: './create-menu.component.html',
  styleUrls: ['./create-menu.component.css'],
  providers: [MenuService, BsDropdownModule]
})

export class CreateMenuComponent implements OnInit {

  constructor(private toastr: ToastsManager, private menuService: MenuService, private vcr: ViewContainerRef, private http: Http) {
    this.toastr.setRootViewContainerRef(vcr);
  }
  menu: Menu;
  date: Date;
  loading: boolean;
  dateFormatted: string;

  ngOnInit() {
    this.loading = true;
    let observer = this.menuService.getLastMenu();
    observer.subscribe(value => this.map(value, value.menuId), err => this.error(err));
  }

  error(err: any) {
    var er = err.json();
    this.toastr.error(er.Message, Constants.errorTitle, { showCloseButton: true });
    this.loading = false;
  }

  map(value: Menu, currentMenuId: number) {
    let momentObj = moment(value.lunchDate, Constants.vsDateFormat).toString();
    this.date = new Date(momentObj);
    this.menu = value;
    this.menu.menuId = currentMenuId;
    this.loading = false;
    this.dateFormatted = moment(value.lunchDate, Constants.vsDateFormat).format(Constants.vmDateFormat);
  }

  getEmptyMenu() {
    var m = this.menu;
    _.each(this.menu.sections, function (section) {
      section.items = [];
      var menuItem = new MenuItem(m.menuId, section.menuSectionId);
      section.items.push(menuItem);
    });
  }

  getTemplateMenu() {
    var curMenuId = this.menu.menuId;
    this.menu = null;
    let observer = this.menuService.getTemplateMenu();
    observer.subscribe(value => this.map(value, curMenuId));
  }

  
  saveToDoc() {
    debugger;
    const headers = new Headers();
    headers.append('Accept', 'application/json');
    headers.append('Content-Type', 'application/json');
    this.http.post('http://localhost:50734/api/menus/saveToDoc',this.menu ,{ headers: headers })
      .toPromise()
      .then(response => this.saveToFileSystem(response));
  }

  private saveToFileSystem(response) {
    debugger;
    //const contentDispositionHeader: string = response.headers.get('Content-Disposition');
    //const parts: string[] = contentDispositionHeader.split(';');
    //const filename = parts[1].split('=')[1];
    const blob = new Blob([response._body], {type: "octet/stream"});
    saveAs(blob, "test.docx");
  }


  save() {
    let m = this.menu;
    if (!m) return;
    let momentObj = moment(this.date, Constants.vsDateFormat).format(Constants.vsDateFormat);
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
    debugger;
    this.toastr.success(Constants.successTitle, null, { showCloseButton: true });
    this.loading = false;
  }
}
