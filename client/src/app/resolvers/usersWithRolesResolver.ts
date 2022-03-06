import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve } from "@angular/router";
import { Observable } from "rxjs";
import { AdminService } from "../account/admin.service";
import { IUser } from "../shared/models/user";
import { IUserRoleObj } from "../shared/models/userRolesModal";

@Injectable({
     providedIn: 'root'
 })
 export class UsersWithRolesResolver implements Resolve<IUser[]> {
 
     constructor(private adminService: AdminService) {}
 
     resolve(route: ActivatedRouteSnapshot): Observable<IUser[]> {
        return this.adminService.getUsersWithRoles();
     }
 
 }