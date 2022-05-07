import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve } from "@angular/router";
import { Observable } from "rxjs";
import { ICustomerNameAndCity } from "../shared/models/customernameandcity";
import { SharedService } from "../shared/services/shared.service";

@Injectable({
     providedIn: 'root'
 })
 export class CustomerNameCityResolver implements Resolve<ICustomerNameAndCity[]> {
 
     constructor(private service: SharedService) {}
 
     resolve(route: ActivatedRouteSnapshot): Observable<ICustomerNameAndCity[]> {
        return this.service.getCustomers();
     }
 
 }