import { NgModule } from '@angular/core';
import { AdminindexComponent } from './adminindex/adminindex.component';
import { CvrefComponent } from './cvref/cvref.component';
import { RouterModule } from '@angular/router';
import { AssessedAndApprovedCVsResolver } from '../resolvers/assessedAndApprovedCVsResolver';
import { SelectionComponent } from './selection/selection.component';
import { OrdersBriefResolver } from '../resolvers/ordersBriefResolver';
import { EmploymentsComponent } from './employments/employments.component';

const routes = [
  {path: '', component:AdminindexComponent,  data: {breadcrumb: 'Admin Division'}},
  {path: 'cvforward', component: CvrefComponent,
    resolve: {assessedcvs: AssessedAndApprovedCVsResolver},
    data: {breadcrumb: {breadcrumb: 'CV Forward to clients'}}
  },
  {path: 'selections', component: SelectionComponent, data: {breadcrumb: 'Selections'}},
  {path: 'employments', component: EmploymentsComponent, 
      resolve: {ordersbrief: OrdersBriefResolver },
      data: {breadcrumb: 'Employments'}}
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
export class AdminRoutingModule { }
