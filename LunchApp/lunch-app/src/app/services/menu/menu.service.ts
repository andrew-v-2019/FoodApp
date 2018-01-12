import { Injectable } from '@angular/core';

import { Observable } from "rxjs/Observable";
import { Menu } from "app/models/menu/menu";
import { MenuResource } from "app/resources/menuResource";

@Injectable()
export class MenuService {

  constructor(private menuResource: MenuResource) {
  }

  getLastMenu(): Observable<Menu> {
    var obs = this.menuResource.getLast().$observable;
    return obs;
  }

  getEmptyMenu(): Observable<Menu> {
    var obs = this.menuResource.getEmpty().$observable;
    return obs;
  }

  getTemplateMenu(): Observable<Menu> {
    var obs = this.menuResource.getTemplate().$observable;
    return obs;
  }

  updateMenu(menu: Menu): Observable<Menu> {
    var obs = this.menuResource.updateMenu(menu).$observable;
    return obs;
  }

}
