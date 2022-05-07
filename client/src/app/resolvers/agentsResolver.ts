import { Injectable } from "@angular/core";
import { Resolve } from "@angular/router";
import { Observable } from "rxjs";
import { ClientService } from "../client/client.service";
import { ICustomerOfficialDto } from "../shared/models/customerOfficialDto";

@Injectable({
     providedIn: 'root'
 })
 export class AgentsResolver implements Resolve<ICustomerOfficialDto[]> {
 
     constructor(private customerService: ClientService) {}
 
     resolve(): Observable<ICustomerOfficialDto[]> {
        return this.customerService.getAgents();
     }
 
 }