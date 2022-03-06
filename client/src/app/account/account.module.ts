import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { AccountRoutingModule } from './account-routing.module';
import { SharedModule } from '../shared/shared.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { RolesModalComponent } from './roles-modal/roles-modal.component';
import { UserManagementComponent } from './user-management/user-management.component';
import { UsersWithRolesComponent } from './users-with-roles/users-with-roles.component';
import { UserTransactionComponent } from './user-transaction/user-transaction.component';
import { SearchHistoryComponent } from './search-history/search-history.component';



@NgModule({
  declarations: [
    LoginComponent,
    RegisterComponent,
    RolesModalComponent,
    UserManagementComponent,
    UsersWithRolesComponent,
    UserTransactionComponent,
    SearchHistoryComponent,
  ],
  imports: [
    CommonModule,
    AccountRoutingModule,
    SharedModule,
    ReactiveFormsModule,
    FormsModule,
    TabsModule
  ]
})
export class AccountModule { }
