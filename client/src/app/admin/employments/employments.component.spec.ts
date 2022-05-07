import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EmploymentsComponent } from './employments.component';

describe('EmploymentsComponent', () => {
  let component: EmploymentsComponent;
  let fixture: ComponentFixture<EmploymentsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EmploymentsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EmploymentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
