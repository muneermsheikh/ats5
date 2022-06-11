import { NgModule } from '@angular/core';
import { AdminindexComponent } from './adminindex/adminindex.component';
import { CvrefComponent } from './cvref/cvref.component';
import { RouterModule } from '@angular/router';
import { AssessedAndApprovedCVsResolver } from '../resolvers/assessedAndApprovedCVsResolver';
import { SelectionComponent } from './selection/selection.component';
import { OrdersBriefResolver } from '../resolvers/ordersBriefResolver';
import { EmploymentsComponent } from './employments/employments.component';
import { CvfwdGuard } from '../guards/cvfwd.guard';
import { SelectionsGuard } from '../guards/selections.guard';
import { MessagesComponent } from './messages/messages.component';

const routes = [
  {path: '', component: AdminindexComponent,  data: {breadcrumb: 'Admin Division'}},
  {path: 'cvforward', component: CvrefComponent, canActivate: [CvfwdGuard],
    resolve: {assessedcvs: AssessedAndApprovedCVsResolver},
    data: {breadcrumb: {breadcrumb: 'CV Forward to clients'}}
  },
  {path: 'selections', component: SelectionComponent, canActivate: [SelectionsGuard], data: {breadcrumb: 'Selections'}},
  {path: 'employments', component: EmploymentsComponent, canActivate: [SelectionsGuard],
      resolve: {ordersbrief: OrdersBriefResolver },
      data: {breadcrumb: 'Employments'}},
  {path: 'messages', component: MessagesComponent, canActivate: [SelectionsGuard], data: {breadcrumb: 'Email Messages'}},
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
