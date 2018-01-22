import { Injectable } from '@angular/core';
import { Resource, ResourceParams, ResourceAction, ResourceMethod } from 'ngx-resource';
import { RequestMethod } from '@angular/http';
import { environment } from 'environments/environment';
import { Menu } from 'app/models/menu/menu';

@Injectable()
@ResourceParams({
    url: environment.apiEndpoint + '/menus/'
})

export class MenuResource extends Resource {
    @ResourceAction({
        path: 'last'
    })
    getLast: ResourceMethod<{}, Menu>;

    @ResourceAction({
        path: 'template'
    })
    getTemplate: ResourceMethod<{}, Menu>;

    @ResourceAction({
        method: RequestMethod.Post,
        path: 'update'
      })
      updateMenu: ResourceMethod<Menu, any>;

      @ResourceAction({
        method: RequestMethod.Post,
        path: 'saveToDoc'
      })
      saveToDoc: ResourceMethod<Menu, any>;
}

