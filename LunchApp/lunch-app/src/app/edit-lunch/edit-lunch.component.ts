import { Component, OnInit, ViewContainerRef } from '@angular/core';
import { LunchService } from 'app/services/lunch/lunch.service';
import { UserLunch } from 'app/models/userLunch/userLunch';
import * as moment from 'moment';
import { Moment } from "moment";
import * as _ from "lodash";
import { FormGroup, FormControl, FormBuilder, Validators, ValidationErrors } from '@angular/forms';
import { ToastsManager } from 'ng2-toastr/ng2-toastr';
import { Constants } from 'app/Constants';



@Component({
  selector: 'app-edit-lunch',
  templateUrl: './edit-lunch.component.html',
  styleUrls: ['./edit-lunch.component.css'],
  providers: [LunchService]
})
export class EditLunchComponent implements OnInit {

  constructor(private userLunchService: LunchService, private vcr: ViewContainerRef, private toastr: ToastsManager) {
    this.toastr.setRootViewContainerRef(vcr);
  }

  lunch: UserLunch;
  loading: boolean;

  ngOnInit() {
    this.loading = true;
    let observer = this.userLunchService.get();
    observer.subscribe(value => this.map(value), err => this.error(err));
  }

  map(value: UserLunch) {
    this.lunch = value;
    this.reindex();
    this.lunch.lunchDate = moment(value.lunchDate, Constants.vsDateFormat).format(Constants.vmDateFormat);
    this.loading = false;
  }

  save(lunchForm) {
    var sectionsSelected = _.every(this.lunch.sections, function (sec) {
      return sec.checked
    });
    if (lunchForm.invalid || !sectionsSelected) return;
    this.loading = true;
    let observer = this.userLunchService.updateLunch(this.lunch);
    observer.subscribe(value => this.lunchUpdatedEvent(value), err => this.error(err));
  }

  error(err: any) {
    var er = err.json();
    this.toastr.error(er.Message, Constants.errorTitle, { showCloseButton: true });
    this.loading = false;
  }

  lunchUpdatedEvent(r) {
    this.toastr.success(Constants.successTitle, null, { showCloseButton: true });
    this.reindex();
    this.loading = false;
  }

  itemChecked(val, selectedItem, section) {
    var model = this.lunch;
    _.each(section.items, function (item, idx) {
      if (item.menuItemId != selectedItem.menuItemId) {
        item.checked = false;
      }
    });
    this.reindex();
  }

  reindex() {
    _.each(this.lunch.sections, function (sec, idx) {
      sec.checked = _.some(sec.items, function (item) {
        return item.checked == true;
      });
    });
  }
}
