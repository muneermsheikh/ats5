import { NgModule } from '@angular/core';
import { InterviewlistComponent } from './interviewlist/interviewlist.component';
import { InterviewsBriefResolver } from '../resolvers/interviewsBriefResolver';
import { RouterModule } from '@angular/router';


const routes = [
  {path: '', component: InterviewlistComponent,  
    resolve: {interviews: InterviewsBriefResolver},
    data: {breadcrumb: 'Open Interviews'}
  },

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
