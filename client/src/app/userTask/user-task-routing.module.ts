import { NgModule } from '@angular/core';
import { UserTaskComponent } from './user-task/user-task.component';
import { UserTaskEditComponent } from './user-task-edit/user-task-edit.component';
import { RouterModule } from '@angular/router';
import { UserTaskResolver } from '../resolvers/userTaskResolver';


const routes = [
  {path: '', component: UserTaskComponent, resolve: {paginatedTask: UserTaskResolver}},
  {path: 'add', component: UserTaskEditComponent , data: {breadcrumb: {alias: 'TaskAdd'}}},
  {path: 'edit/:id', component: UserTaskEditComponent, data: {breadcrumb: {alias: 'TaskEdit'}}},
  {path: 'view/:id', component: UserTaskEditComponent , data: {breadcrumb: {alias: 'TaskView'}}}
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
export class UserTaskRoutingModule { }
