import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { IUser } from 'src/app/shared/models/user';
import { IUserRoleObj, userRoleObj } from 'src/app/shared/models/userRolesModal';
import { AdminService } from '../admin.service';
import { RolesModalComponent } from '../roles-modal/roles-modal.component';

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.css']
})
export class UserManagementComponent implements OnInit {

  users: IUser[]; // Partial<IUser[]>;
  bsModalRef: BsModalRef;
  existingRoles: string[]=[];
  sRoles: IUserRoleObj[]=[];
  constructor(private adminService: AdminService, private modalService: BsModalService) { }

  ngOnInit(): void {
    this.getUsersWithRoles();
  }

  getUsersWithRoles() {
    this.adminService.getUsersWithRoles().subscribe(response => {
      this.users = response;
    })
  }

  openRolesModal(user: IUser) {
    var returnvalue:any;
    const config = {
      class: 'modal-dialog-centered',
      initialState: {
        user,
        roles: this.getRolesArray(user)
      }
    }
    console.log('in openRolesModal, before modal, user is ', user);
    this.bsModalRef = this.modalService.show(RolesModalComponent, config);
    this.bsModalRef.content.updateSelectedRoles.subscribe(values => {
      const rolesToUpdate = {
        roles: [...values.filter(el => el.checked === true).map(el => el.name)]
      };
      returnvalue=values;
      if (rolesToUpdate) {
        const eml = user.username;
        this.adminService.updateUserRoles(user.username, rolesToUpdate.roles).subscribe(() => {
          user.roles = [...rolesToUpdate.roles]
        })
      }
    })
  }

  
  private getRolesArray(user: IUser) {
    const roles: any[] = [];
    let aroles: IUserRoleObj[]=[];
    let arole: IUserRoleObj;
    const userRoles = user.roles;
    this.adminService.getIdentityRoles().subscribe(response => {
      this.existingRoles = response;
      if(this.existingRoles.length==0) return;
      
      this.existingRoles.forEach(role => {
        arole = new userRoleObj();
        //arole.email = role.username;
        arole.name=role;
        arole.value=role;
        
        let isMatch = false;
        for (const userRole of userRoles) {
          if (role === userRole) {
            isMatch = true;
            arole.checked = true;
            aroles.push(arole);
            break;
          }
        }
        if (!isMatch) {
          arole.checked = false;
          aroles.push(arole);
        }
      })
      return aroles;
    }, error => {
      console.log('failed to retrieve roles array', error);
    })
    return aroles;
  }   

  }

