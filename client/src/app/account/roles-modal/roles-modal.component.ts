import { Component, EventEmitter, Input, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { IUser } from 'src/app/shared/models/user';
import { IUserRoleObj } from 'src/app/shared/models/userRolesModal';

@Component({
  selector: 'app-roles-modal',
  templateUrl: './roles-modal.component.html',
  styleUrls: ['./roles-modal.component.css']
})
export class RolesModalComponent implements OnInit {

  @Input() updateSelectedRoles = new EventEmitter();
  user: IUser;
  roles: IUserRoleObj[]; 

  constructor(public bsModalRef: BsModalRef) { }

  ngOnInit(): void {

  }

  updateRoles() {
    this.updateSelectedRoles.emit(this.roles);
    this.bsModalRef.hide();
  }

}
