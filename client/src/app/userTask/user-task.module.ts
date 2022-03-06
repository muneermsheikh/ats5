import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserTaskComponent } from './user-task/user-task.component';
import { UserTaskEditComponent } from './user-task-edit/user-task-edit.component';
import { UserTaskLineComponent } from './user-task-line/user-task-line.component';
import { SharedModule } from '../shared/shared.module';
import { UserTaskRoutingModule } from './user-task-routing.module';
import { TaskReminderModalComponent } from './task-reminder-modal/task-reminder-modal.component';



@NgModule({
  declarations: [
    UserTaskComponent,
    UserTaskEditComponent,
    UserTaskLineComponent,
    TaskReminderModalComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    UserTaskRoutingModule
  ]
})
export class UserTaskModule { }
