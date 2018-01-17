import { Component, OnInit, Input } from '@angular/core';
import { MenuSection } from "app/models/menu/menu-section";

import * as _ from "lodash";
import { UserLunch } from "app/models/userLunch/userLunch";

@Component({
  selector: 'la-user-lunch-short-list',
  templateUrl: './user-lunch-short-list.component.html'
})
export class UserLunchShortListComponent implements OnInit {
  @Input() lunch: UserLunch;
  @Input() expanded: boolean;

  constructor() { }

  ngOnInit() {
  }

}