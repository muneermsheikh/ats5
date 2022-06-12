import { NgModule } from '@angular/core';
import { InterviewsBriefResolver } from '../resolvers/interviewsBriefResolver';
import { RouterModule } from '@angular/router';
import { InterviewIndexComponent } from './interview-index/interview-index.component';
import { InterviewlistComponent } from './interviewlist/interviewlist.component';
import { InterviewAddComponent } from './interview-add/interview-add.component';
import { InterviewResolver } from '../resolvers/interviewResolver';
import { EmployeeIdsAndKnownAsResolver } from '../resolvers/employeeIdsAndKnownAsResolver';


const routes = [
  {path: '', component: InterviewIndexComponent},  
  {path: 'list', component: InterviewlistComponent,
      resolve: {interviews: InterviewsBriefResolver}},
  {path: 'edit/:orderid', component: InterviewAddComponent,
      resolve: {
      interview: InterviewResolver,
      employees: EmployeeIdsAndKnownAsResolver
    },
    data: {breadcrumb: {alias: 'OrderEdit'}}},
  {path: 'add', component: InterviewAddComponent}
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
export class InterviewRoutingModule { }
