import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NotFoundComponent } from './core/not-found/not-found.component';
import { TestErrorComponent } from './core/test-error/test-error.component';
import { HomeComponent } from './home/home.component';
import { SeverErrorComponent } from './core/sever-error/sever-error.component';
import { CallrecordsComponent } from './shared/callrecords/callrecords.component';
import { TeachersComponent } from './orders/teachers/teachers.component';
import { MessagesComponent } from './admin/messages/messages.component';
import { AdminGuard } from './guards/admin.guard';
import { AuthorizedGuard } from './guards/authorized.guard';
import { ProspectiveListingComponent } from './prospectives/prospective-listing/prospective-listing.component';
import { InterviewlistComponent } from './interview/interviewlist/interviewlist.component';

const routes: Routes = [
  {path: '', component: HomeComponent, data: {breadcrumb: 'Home'}},
  //{path: 'test-error', component: TestErrorComponent, data: {breadcrumb: 'Errors'}},
  //{path: 'server-error', component: SeverErrorComponent, data: {breadcrumb: 'server-error'}},
  
  {path: 'candidate', loadChildren:() => import('./candidate/candidate.module').then(mod => mod.CandidateModule), data: {breadcrumb: 'candidate'}},
  {path: 'employee', loadChildren:() => import('./employee/employee.module').then(mod => mod.EmployeeModule), canActivate: [AdminGuard], data: {breadcrumb: 'employees'}},
  {path: 'orders', loadChildren:() => import('./orders/orders.module').then(mod => mod.OrdersModule), data: {breadcrumb: 'orders'}},
  {path: 'client', loadChildren: () => import('./client/client.module').then(mod => mod.ClientModule), canActivate:[AdminGuard], data: {breadcrumb: 'customers'}},
  {path: 'account', loadChildren: () => import('./account/account.module').then(mod => mod.AccountModule), data: {breadcrumb: {skip: true}}},
  {path: 'checkout', loadChildren: () => import('./checkout/checkout.module').then(mod => mod.CheckoutModule), data: {breadcrumb: 'checkout'}},
  {path: 'userTask', loadChildren: () => import('./userTask/user-task.module').then(mod => mod.UserTaskModule), data: {breadcrumb: 'userTask'}},
  //{path: 'admin', component: UserManagementComponent, canActivate: [AdminGuard]},
  {path: 'hr', loadChildren:() => import('./hr/hr.module').then(mod => mod.HrModule), data: {breadcrumb: 'HR'}},
  {path: 'admin', loadChildren:() => import('./admin/admin.module').then(mod => mod.AdminModule), canActivate: [AdminGuard], data: {breadcrumb: 'Admin'}},
  {path: 'callrecords',component: CallrecordsComponent, canActivate: [AuthorizedGuard], data: {breadcrumb: 'Call Records'}}, 
  {path: 'masters', loadChildren:() => import('./masters/masters.module').then(mod => mod.MastersModule), canActivate: [AdminGuard], data: {breadcrumb: 'Masters'}},
  {path: 'prospectives', loadChildren:() => import('./prospectives/prospective.module').then(mod => mod.ProspectiveModule), 
      canActivate: [AdminGuard], data: {breadcrumb: 'Prospective Candidatesssss'}},
  {path: 'notfound', component: NotFoundComponent, data: {breadcrumb: 'not-found errr'}},
  {path: 'messages', component: MessagesComponent, data: {breadcrumb: 'messages for loggedin user'}},
  {path: 'teachers', component: TeachersComponent},
  {path: 'process', loadChildren:() => import('./process/process.module').then(mod => mod.ProcessModule), data: {breadcrumb: 'Process'}},
  {path: 'interviews', loadChildren:() => import('./interview/interview.module').then(mod => mod.InterviewModule), data: {breadcrumb: 'Interviews'}},
  {path: 'servererror',component: SeverErrorComponent},
  {path: 'testerrors',component: TestErrorComponent},
  {path: '**', redirectTo: 'not-found', pathMatch: 'full'}
  
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
