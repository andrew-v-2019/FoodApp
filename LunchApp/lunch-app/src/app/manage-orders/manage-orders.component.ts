import { Component, OnInit, ViewContainerRef } from '@angular/core';
import { ToastsManager } from 'ng2-toastr';
import { OrderService } from 'app/services/order/order.service';
import { Order } from 'app/models/order/order';

@Component({
  selector: 'app-manage-orders',
  templateUrl: './manage-orders.component.html',
  styleUrls: ['./manage-orders.component.css']
})
export class ManageOrdersComponent implements OnInit {

  constructor(private orderService: OrderService, private toastr: ToastsManager, private vcr: ViewContainerRef) { 
    this.toastr.setRootViewContainerRef(vcr);
  }

  order: Order;
  loading: boolean;

  ngOnInit() {
    this.loading = true;
    let observer = this.orderService.get();
    observer.subscribe(value => this.map(value));
  }

  map(value: Order) {
    this.loading = false;
    debugger;
  }

}
