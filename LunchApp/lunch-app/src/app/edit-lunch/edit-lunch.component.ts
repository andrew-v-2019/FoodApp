import { Component, OnInit, ViewContainerRef } from '@angular/core';
import { LunchService } from 'app/services/lunch/lunch.service';
import { UserLunch } from 'app/models/userLunch/userLunch';
import * as moment from 'moment';
import { Moment } from "moment";
import * as _ from "lodash";
import { FormGroup, FormControl, FormBuilder, Validators, ValidationErrors } from '@angular/forms';
import { ToastsManager } from 'ng2-toastr/ng2-toastr';


const vmDateFormat: string = "DD.MM.YYYY";
const vsDateFormat: string = "YYYY-MM-DD";

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
    observer.subscribe(value => this.map(value));
  }

  map(value: UserLunch) {
    if (_.isUndefined(value.lunchDate)){
      this.toastr.warning('Нет активных меню на сегодня...',null, {showCloseButton: true });
      return;
    }
    this.lunch = value;
    this.reindex();
    this.lunch.lunchDate = moment(value.lunchDate, vsDateFormat).format(vmDateFormat);
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
    this.toastr.error(er.Message, 'Ошибка', {showCloseButton: true });
    this.loading = false;
  }

  lunchUpdatedEvent(r) {
    this.toastr.success('Сохранено', null, {showCloseButton: true });
    debugger;
    //this.map(r);
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
