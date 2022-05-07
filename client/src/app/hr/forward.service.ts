import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IOrder } from '../shared/models/order';
import { IOrderBriefDto } from '../shared/models/orderBriefDto';
import { OrderItemsAndAgentsToFwdDto } from '../shared/models/orderItemsAndAgentsToFwdDto';
import { orderParams } from '../shared/params/orderParams';
import { IUser } from '../shared/models/user';

@Injectable({
  providedIn: 'root'
})
export class ForwardService {
  apiUrl = environment.apiUrl;
  private currentUserSource = new ReplaySubject<IUser>(1);
  currentUser$ = this.currentUserSource.asObservable();
  fParams = new OrderItemsAndAgentsToFwdDto();
  user: IUser;

  oParams = new orderParams();

  cache = new Map();

  constructor(private http: HttpClient) { }

  updateDLForward(fs: any) {
    
    return this.http.put(this.apiUrl + 'dlforward', fs);
  }

  forwardDLtOHRDept(order: IOrder) {
      
  }

  getOrderBrief(id: number) {
    return this.http.get<IOrderBriefDto>(this.apiUrl + 'orders/orderbriefdto/' + id);
  }
  
/*  
  getProduct(id: number) {
    let product: IProduct;
    this.productCache.forEach((products: IProduct[]) => {
      console.log(product);
      product = products.find(p => p.id === id);
    })

    if (product) {
      return of(product);
    }

    return this.http.get<IProduct>(this.baseUrl + 'products/' + id);
  }
*/

}
