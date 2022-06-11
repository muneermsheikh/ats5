import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { UserTransactionComponent } from './user-transaction/user-transaction.component';
import { UserManagementComponent } from './user-management/user-management.component';
import { AdminGuard } from '../guards/admin.guard';
import { RolesComponent } from './roles/roles.component';
import { RolesResolver } from '../resolvers/rolesResolver';

const routes: Routes = [
  {path: '', component: LoginComponent,  data: {breadcrumb: 'Login Admin'}},
  {path: 'login', component: LoginComponent,data: {breadcrumb: {alias: 'logIn'}}},
  {path: 'register', component: RegisterComponent,data: {breadcrumb: {alias: 'registerUser'}}},
  {path: 'userswithroles', component: UserManagementComponent, canActivate: [AdminGuard], data: {breadcrumb: {alias: 'User Role Management'}}},
  {path: 'identityRoles', 
    resolve:{roles: RolesResolver},
    component: RolesComponent, canActivate: [AdminGuard], 
    data: {breadcrumb: {alias: 'Identity Roles'}}},
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
