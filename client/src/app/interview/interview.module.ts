import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InterviewlistComponent } from './interviewlist/interviewlist.component';
import { InterviewItemComponent } from './interview-item/interview-item.component';
import { InterviewEditComponent } from './interview-edit/interview-edit.component';
import { InterviewCategoriesComponent } from './interview-categories/interview-categories.component';
import { SharedModule } from '../shared/shared.module';



@NgModule({
  declarations: [
    InterviewlistComponent,
    InterviewItemComponent,
    InterviewEditComponent,
    InterviewCategoriesComponent
  ],
  imports: [
    CommonModule,
    SharedModule
  ]
})
export class InterviewModule { }
