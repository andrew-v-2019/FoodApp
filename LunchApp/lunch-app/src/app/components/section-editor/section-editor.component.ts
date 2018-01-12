import { Component, OnInit, Input } from '@angular/core';
import { MenuSection } from "app/models/menu/menu-section";

import * as _ from "lodash";
import { MenuItem } from "app/models/menu/menu-item";

@Component({
  selector: 'la-section-editor',
  templateUrl: './section-editor.component.html',
  styleUrls: ['./section-editor.component.css']
})
export class SectionEditorComponent implements OnInit {
  @Input() section: MenuSection;
  @Input() editable: boolean;
  @Input() loading: boolean;

  constructor() { }

  ngOnInit() {
  }

  addEmptyItem(event) {
    var last = _.last(this.section.items);
    if (last.name.trim().length ==0) return; 
    var menuItem = new MenuItem();
    menuItem.menuId = this.section.menuId;
    menuItem.menuSectionId = this.section.menuSectionId;
    menuItem.menuItemId = 0;
    menuItem.number = 1;
    menuItem.name = '';
    if (last){
      menuItem.number = last.number+1;
    }
    this.section.items.push(menuItem);
  }

  removeItem(item) {
    if (this.section.items.length<=1) return;
    this.section.items.splice(item.number-1, 1);
    this.reindex();
  }

  reindex(){
    _.each(this.section.items, function(item, idx){
      item.number = idx+1;
    });
  }

}
