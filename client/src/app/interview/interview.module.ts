import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InterviewlistComponent } from './interviewlist/interviewlist.component';
import { InterviewItemComponent } from './interview-item/interview-item.component';
import { InterviewEditComponent } from './interview-edit/interview-edit.component';
import { InterviewCategoriesComponent } from './interview-categories/interview-categories.component';
import { SharedModule } from '../shared/shared.module';
import { InterviewIndexComponent } from './interview-index/interview-index.component';
import { InterviewAddComponent } from './interview-add/interview-add.component';
import { InterviewRoutingModule } from './interview-routing.module';



@NgModule({
  declarations: [
    InterviewlistComponent,
    InterviewItemComponent,
    InterviewEditComponent,
    InterviewCategoriesComponent,
    InterviewIndexComponent,
    InterviewAddComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    InterviewRoutingModule
  ]
})
export class InterviewModule { }
