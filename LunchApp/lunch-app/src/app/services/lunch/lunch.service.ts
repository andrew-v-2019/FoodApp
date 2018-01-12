import { Injectable } from '@angular/core';
import { Observable } from "rxjs/Observable";
import { UserLunch } from 'app/models/userLunch/userLunch';
import { LunchResource } from "app/resources/lunchResource";

@Injectable()
export class LunchService {
  constructor(private lunchResource: LunchResource) {

  }

  get(): Observable<UserLunch> {
    var obs = this.lunchResource.get().$observable;
    return obs;
  }

  updateLunch(lunch: UserLunch): Observable<UserLunch> {
    var obs = this.lunchResource.update(lunch).$observable;
    return obs;
  }

}
