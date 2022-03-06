import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve } from "@angular/router";
import { Observable } from "rxjs";
import { ClientService } from "../client/client.service";
import { ICustomer } from "../shared/models/customer";

@Injectable({
     providedIn: 'root'
 })
 export class CustomerResolver implements Resolve<ICustomer> {
 
     constructor(private customerService: ClientService) {}
 
     resolve(route: ActivatedRouteSnapshot): Observable<ICustomer> {
        return this.customerService.getCustomer(+route.paramMap.get('id'));
     }
 
 }