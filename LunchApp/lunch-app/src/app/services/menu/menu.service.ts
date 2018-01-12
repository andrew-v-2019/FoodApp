import { Injectable } from '@angular/core';

 import { Http, Response, Headers, RequestOptions } from '@angular/http';
 import { Observable } from "rxjs/Observable";
 import { Menu } from "app/models/menu/menu";
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';

@Injectable()
export class MenuService {
  
  private menuUrl = 'http://localhost:5000/api/menus/last';
  private emptyMenuUrl = 'http://localhost:5000/api/menus/empty';
  private templateMenuUrl = 'http://localhost:5000/api/menus/template';

  private updateMenuUrl = 'http://localhost:5000/api/menus/update';

  constructor(private http: Http) { 

  }
 

  getLastMenu() : Observable<Menu> {
         return this.http.get(this.menuUrl)
                         .map((res:Response) => {return res.json()})
                         .catch((error:any) => 'Server error');
     }

  getEmptyMenu() : Observable<Menu> {
      return this.http.get(this.emptyMenuUrl)
                      .map((res:Response) => {return res.json()})
                      .catch((error:any) => 'Server error');
     
  }

  getTemplateMenu() : Observable<Menu> {
    return this.http.get(this.templateMenuUrl)
                    .map((res:Response) => {return res.json()})
                    .catch((error:any) => 'Server error');

}

updateMenu(menu:Menu) : Observable<Menu> {
  return this.http.post(this.updateMenuUrl, menu)
                  .map((res:Response) => {return res.json()})
                  .catch((error:any) => error);

}

}
