import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { UsersWithRolesComponent } from './users-with-roles/users-with-roles.component';
import { UserTransactionComponent } from './user-transaction/user-transaction.component';

const routes: Routes = [
  {path: 'login', component: LoginComponent,data: {breadcrumb: {alias: 'logIn'}}},
  {path: 'register', component: RegisterComponent,data: {breadcrumb: {alias: 'registerUser'}}},
  {path: 'userswithroles', component: UsersWithRolesComponent, data: {breadcrumb: {alias: 'UsersWithRoles'}}},
  {path: 'transactions', component: UserTransactionComponent, data: {breadcrumb: {alias: 'Transactions'}}}
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [
    RouterModule
  ]
})
export class AccountRoutingModule { }
