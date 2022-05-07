import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of, ReplaySubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { IOrderItem } from '../shared/models/orderItem';
import { IOrderItemBriefDto } from '../shared/models/orderItemBriefDto';
import { IUser } from '../shared/models/user';
import { orderItemParams } from '../shared/params/orderItemParams';

@Injectable({
  providedIn: 'root'
})
export class OrderitemsService {
  apiUrl = environment.apiUrl;
  private currentUserSource = new ReplaySubject<IUser>(1);
  currentUser$ = this.currentUserSource.asObservable();
  oParams = new orderItemParams();
  cache = new Map();
  orderitems: IOrderItemBriefDto[];


  constructor(private http: HttpClient) { }

    getOrderItems(orderid: number, useCache: boolean) {
      if(useCache === false) this.cache = new Map();
      if(this.cache.size > 0 && useCache === true) {
          if (this.cache.has(Object.values(this.oParams).join('-'))) {
            this.orderitems = this.cache.get(Object.values(this.oParams).join('-'));
            return of(this.orderitems);
        }
      }

      return this.http.get<IOrderItemBriefDto[]>(this.apiUrl + 'orders/orderitemsbyorderid/'+orderid)
        .pipe(
          map(response => {
            this.cache.set(Object.values(this.oParams).join('-'), response);
            this.orderitems = response;
            return response;
          })
        )
    }

/*
    getOItem(orderitemid: number) {
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
  getOrderItem(orderitem: number) {
    let item: IOrderItemBriefDto;
    this.cache.forEach((items: IOrderItemBriefDto[]) => {
      item = items.find(p => p.orderItemId === orderitem);
    })
    if (item) return of(item);

    return this.http.get<IOrderItemBriefDto>(this.apiUrl + 'orders/itemdtobyid/' + orderitem);
  }

  

}

  