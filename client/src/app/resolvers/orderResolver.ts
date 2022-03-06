import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve } from "@angular/router";
import { Observable } from "rxjs";
import { OrderService } from "../orders/order.service";
import { IOrder } from "../shared/models/order";

@Injectable({
     providedIn: 'root'
 })
 export class OrderResolver implements Resolve<IOrder> {
 
     constructor(private orderService: OrderService) {}
 
     resolve(route: ActivatedRouteSnapshot): Observable<IOrder> {
        return this.orderService.getOrder(+route.paramMap.get('id'));
     }
 
 }