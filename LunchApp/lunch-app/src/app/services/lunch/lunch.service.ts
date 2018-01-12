import { Injectable } from '@angular/core';

 import { Http, Response, Headers, RequestOptions } from '@angular/http';
 import { Observable } from "rxjs/Observable";
 import { Menu } from "app/models/menu/menu";
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import { UserLunch } from 'app/models/userLunch/userLunch';

@Injectable()
export class LunchService {
  private userLunchUrl = 'http://localhost:5000/api/userlunch/get';
  constructor(private http: Http) { 
    
      }

      get() : Observable<UserLunch> {
        return this.http.get(this.userLunchUrl)
                        .map((res:Response) => {return res.json()})
                        .catch((error:any) => 'Server error');
    }

  }
