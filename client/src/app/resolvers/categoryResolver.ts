import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve } from "@angular/router";
import { Observable } from "rxjs";
import { ClientService } from "../client/client.service";
import { MastersService } from "../masters/masters.service";
import { ICustomer } from "../shared/models/customer";
import { IProfession } from "../shared/models/profession";

@Injectable({
     providedIn: 'root'
 })
 export class CategoryResolver implements Resolve<IProfession> {
 
     constructor(private mastersService: MastersService) {}
 
     resolve(route: ActivatedRouteSnapshot): Observable<IProfession> {
        return this.mastersService.getCategory(+route.paramMap.get('id'));
     }
 
 }