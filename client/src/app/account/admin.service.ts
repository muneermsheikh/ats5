import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { IChooseAgentDto } from '../shared/models/chooseAgentDto';

import { IOrderItemToFwdDto } from '../shared/models/orderItemToFwdDto';
import { IUser } from '../shared/models/user';
import { IOrderItemsAndAgentsToFwdDto } from '../shared/models/orderItemsAndAgentsToFwdDto';
import { ICustomerOfficialDto } from '../shared/models/customerOfficialDto';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getUsersWithRoles() {
    //console.log('calling api for getuserswithroles');
    return this.http.get<Partial<IUser[]>>(this.baseUrl + 'admin/users-with-roles');
  }

  updateUserRoles(email: string, roles: string[]) {
    //return this.http.post(this.baseUrl + 'admin/edit-roles/' + email + '?roles=' + roles, {});
    return this.http.post(this.baseUrl + 'admin/edit-roles/'+ email + '?roles=' + roles,{});
  }

  
  getIdentityRoles() {
    return this.http.get<string[]>(this.baseUrl + 'admin/identityroles');
  }

  getOfficialDto() {
    //return this.http.get<IChooseAgentDto[]>(this.baseUrl + 'customers/agentdetails');
    return this.http.get<ICustomerOfficialDto[]>(this.baseUrl + 'customers/agentdetails');
  }

  //forward DL to agents
  forwardDLtoSelectedAgents(itemsAndAgents: IOrderItemsAndAgentsToFwdDto) {
    return this.http.post(this.baseUrl + 'DLForward', itemsAndAgents )
  }

  addNewRole(newRoleName: string) {

    return this.http.post(this.baseUrl + 'admin/role/' + newRoleName, {});
  }

  deleteRole(roleName: string) {
    return this.http.delete(this.baseUrl + 'admin' + roleName);
  }

  editrRole(existingRoleName: string, newRoleName: string) {
    return this.http.put(this.baseUrl + 'role/' + existingRoleName + '/' + newRoleName, {});
  }
}
