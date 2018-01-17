import { Component, OnInit, ViewContainerRef } from '@angular/core';
import { ToastsManager } from 'ng2-toastr';
import { OrderService } from 'app/services/order/order.service';
import { Order } from 'app/models/order/order';
import { Constants } from 'app/Constants';
import { ErrorHandler } from '@angular/core';

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
    observer.subscribe(value => this.map(value), err => this.error(err));
  }

  map(value: Order) {
    this.order = value;
    this.loading = false;
  }

  save() {
    this.loading = true;
    let observer = this.orderService.update(this.order);
    observer.subscribe(value => this.orderUpdatedEvent(value), err => this.error(err));
  }

  submit() {
    this.loading = true;
    let observer = this.orderService.submit(this.order);
    observer.subscribe(value => this.orderUpdatedEvent(value), err => this.error(err));
  }

  error(err: any) {
    var er = err.json();
    this.toastr.error(er.Message, Constants.errorTitle, { showCloseButton: true });
    this.loading = false;
  }

  orderUpdatedEvent(r) {
    this.map(r);
    this.toastr.success(Constants.successTitle, null, { showCloseButton: true });
  }

}
