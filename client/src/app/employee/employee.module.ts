import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { EmployeelistComponent } from './employeelist.component';
import { EmployeeItemComponent } from './employee-item/employee-item.component';
import { EmployeeDetailComponent } from './employee-detail/employee-detail.component';
import { SharedModule } from '../shared/shared.module';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { EmployeeRoutingModule } from './employee-routing.module';



@NgModule({
  declarations: [
    EmployeelistComponent,
    EmployeeItemComponent,
    EmployeeDetailComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    TabsModule,
    FormsModule,
    ReactiveFormsModule,
    EmployeeRoutingModule
  ],
  providers: [DatePipe]
})
export class EmployeeModule { }
