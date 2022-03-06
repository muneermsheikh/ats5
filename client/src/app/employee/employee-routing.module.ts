import { NgModule } from '@angular/core';
import { EmployeelistComponent } from './employeelist.component';
import { EmployeeDetailComponent } from './employee-detail/employee-detail.component';
import { RouterModule } from '@angular/router';

const routes = [
  {path: '', component: EmployeelistComponent},
  {path: 'add', component:EmployeeDetailComponent , data: {breadcrumb: {alias: 'employeeAdd'}}},
  {path: 'edit/:id', component: EmployeeDetailComponent, data: {breadcrumb: {alias: 'employeeEdit'}}},
  {path: 'view/:id', component: EmployeeDetailComponent , data: {breadcrumb: {alias: 'employeeView'}}}
]


@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [
    RouterModule
  ]
})
export class EmployeeRoutingModule { }
