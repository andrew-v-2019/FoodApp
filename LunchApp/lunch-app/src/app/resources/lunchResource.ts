import { Injectable } from '@angular/core';
import { Resource, ResourceParams, ResourceAction, ResourceMethod } from 'ngx-resource';
import { RequestMethod } from '@angular/http';
import { environment } from 'environments/environment';
import { UserLunch } from 'app/models/userLunch/userLunch';


@Injectable()
@ResourceParams({
    url: environment.apiEndpoint + '/userlunch/'
})

export class LunchResource extends Resource {
    @ResourceAction({
        path: 'get'
    })
    get: ResourceMethod<{}, UserLunch>;

    @ResourceAction({
        method: RequestMethod.Post,
        path: 'update'
    })
    update: ResourceMethod<UserLunch, any>;
}

