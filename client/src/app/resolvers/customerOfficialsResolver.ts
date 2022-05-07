import { Injectable } from "@angular/core";
import { Resolve } from "@angular/router";
import { Observable } from "rxjs";
import { AdminService } from "../account/admin.service";
import { ICustomerOfficialDto } from "../shared/models/customerOfficialDto";


@Injectable({
     providedIn: 'root'
 })
 export class CustomerOfficialsResolver implements Resolve<ICustomerOfficialDto[]> {
 
     constructor(private service: AdminService) {}
 
     resolve(): Observable<ICustomerOfficialDto[]> {
        return this.service.getOfficialDto();
     }
 
 }