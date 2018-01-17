import { Injectable } from '@angular/core';
import { Observable } from "rxjs/Observable";
import { UserLunch } from 'app/models/userLunch/userLunch';
import { OrderResource } from "app/resources/orderResource";
import { Order } from 'app/models/order/order';

@Injectable()
export class OrderService {
    constructor(private orderResource: OrderResource) {

    }

    get(): Observable<Order> {
        var obs = this.orderResource.get().$observable;
        return obs;
    }

    update(order: Order): Observable<Order> {
        var obs = this.orderResource.update(order).$observable;
        return obs;
    }

    submit(order: Order): Observable<Order> {
        var obs = this.orderResource.submit(order).$observable;
        return obs;
    }

}