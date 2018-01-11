/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { EditLunchComponent } from './edit-lunch.component';

describe('EditLunchComponent', () => {
  let component: EditLunchComponent;
  let fixture: ComponentFixture<EditLunchComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditLunchComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditLunchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
