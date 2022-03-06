import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { IChooseAgentDto } from '../shared/models/chooseAgentDto';

import { IOrderItemToFwdDto } from '../shared/models/orderItemToFwdDto';
import { IUser } from '../shared/models/user';
import { IOrderItemsAndAgentsToFwdDto } from '../shared/models/orderItemsAndAgentsToFwdDto';

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
    return this.http.get<IChooseAgentDto[]>(this.baseUrl + 'customers/getagentdetails');
  }

  //forward DL to agents
  forwardDLtoSelectedAgents(itemsAndAgents: IOrderItemsAndAgentsToFwdDto) {
    return this.http.post(this.baseUrl + 'DLForward', itemsAndAgents )
  }

}
