import { Injectable } from '@angular/core';
import { Resource, ResourceParams, ResourceAction, ResourceMethod } from 'ngx-resource';
import { RequestMethod } from '@angular/http';
import { environment } from 'environments/environment';
import { Order } from 'app/models/order/order';


@Injectable()
@ResourceParams({
    url: environment.apiEndpoint + '/orders/'
})

export class OrderResource extends Resource {
    @ResourceAction({
        path: ''
    })
    get: ResourceMethod<{}, Order>;

    @ResourceAction({
        method: RequestMethod.Post,
        path: ''
    })
    update: ResourceMethod<Order, any>;

    @ResourceAction({
        method: RequestMethod.Post,
        path: 'submit'
    })
    submit: ResourceMethod<Order, any>;
}

