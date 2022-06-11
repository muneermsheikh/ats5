import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { IUser } from 'src/app/shared/models/user';
import { AccountService } from '../account.service';
import { AdminService } from '../admin.service';

@Component({
  selector: 'app-roles',
  templateUrl: './roles.component.html',
  styleUrls: ['./roles.component.css']
})
export class RolesComponent implements OnInit {
  user: IUser;
  identityRoles: string[];
  newRoleAdded: string;
  roleEdited: string;

  editing: boolean=false;

  constructor(private activatedRoute: ActivatedRoute, private toastr: ToastrService,
      private accountService: AccountService, private adminService: AdminService) { 
      this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
  }

  ngOnInit(): void {
    this.activatedRoute.data.subscribe(data => { 
      this.identityRoles = data.roles;
    })
  }

  deleteRole(role: string) {
    this.adminService.deleteRole(role).subscribe(response => {
      if(response) {
        //delete role from identityRoles
        var indx = this.identityRoles.indexOf(role);
        if (indx===-1) {
          this.toastr.info('failed to find index of role to edit');
          return;
        }
        var deleted = this.identityRoles.splice(indx, 1);
        this.toastr.success('Role ' + deleted + ' deleted from list of roles');
      }
    })
  }

  editRole(role: string) {
      //if (!this.editing) return;
      
      this.editing=!this.editing;
      console.log('entered editRole, editing value is:', this.editing);
      if(this.editing) {
        if (this.roleEdited === '') {
          this.toastr.warning('Edit the role to a new name');
          return;
        } else {
          this.adminService.editrRole(role, this.roleEdited).subscribe(response => {
            if(response) {
              this.roleEdited='';
              this.editing=false;
              this.toastr.success('role changed from ' + role + ' to ' + this.roleEdited);
            } else {
              this.toastr.warning('failed to edit the role name');
            }
          }, error => {
            this.toastr.error('failed to edit the role');
          })
        }
      }
  }

  
    addNewRole() {
      if(this.newRoleAdded==='') {
        this.toastr.warning('No Role name provided');
        return;
      }
  
      this.adminService.addNewRole(this.newRoleAdded).subscribe(response => {
        if(response) {
          this.newRoleAdded='';
          this.toastr.success('new Role ' + this.newRoleAdded + ' added' );
        } else {
          this.toastr.warning('failed to add new Role ' + this.newRoleAdded);
        }
      }, error => {
        this.toastr.error('error in adding new Role ' + this.newRoleAdded);
      })
    }
  
  
}
