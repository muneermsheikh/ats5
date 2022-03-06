import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of, ReplaySubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { ICustomerNameAndCity } from '../shared/models/customernameandcity';
import { idAndDate } from '../shared/models/idAndDate';
import { IJDDto } from '../shared/models/jdDto';
import { IOrder } from '../shared/models/order';
import { IOrderCity } from '../shared/models/orderCity';
import { IOrderItemBriefDto, OrderItemBriefDto } from '../shared/models/orderItemBriefDto';
import { orderParams } from '../shared/models/orderParams';
import { IPaginationOrder, PaginationOrder } from '../shared/models/paginationOrder';
import { IProfession } from '../shared/models/profession';
import { IRemuneration } from '../shared/models/remuneration';
import { IRemunerationDto } from '../shared/models/remunerationDto';
import { IUser } from '../shared/models/user';

@Injectable({
  providedIn: 'root'
})

export class OrderService {
  apiUrl = environment.apiUrl;
  private currentUserSource = new ReplaySubject<IUser>(1);
  currentUser$ = this.currentUserSource.asObservable();
  oParams = new orderParams();
  pagination = new PaginationOrder();
  cities: IOrderCity[]=[];
  customers: ICustomerNameAndCity[]=[];
  professions: IProfession[]=[];
  cache = new Map();

  constructor(private http: HttpClient) { }


  getOrders(useCache: boolean) { 

    if (useCache === false) {
      this.cache = new Map();
    }

    if (this.cache.size > 0 && useCache === true) {
      if (this.cache.has(Object.values(this.oParams).join('-'))) {
        this.pagination.data = this.cache.get(Object.values(this.oParams).join('-'));
        return of(this.pagination);
      }
    }

    let params = new HttpParams();
    if (this.oParams.city !== "") {
      params = params.append('city', this.oParams.city);
    }
    if (this.oParams.categoryId !== 0) {
      params = params.append('categoryId', this.oParams.categoryId.toString());
    }

    if (this.oParams.search) {
      params = params.append('search', this.oParams.search);
    }
    
    params = params.append('sort', this.oParams.sort);
    params = params.append('pageIndex', this.oParams.pageNumber.toString());
    params = params.append('pageSize', this.oParams.pageSize.toString());

    return this.http.get<IPaginationOrder>(this.apiUrl + 'orders/ordersbriefpaginated', {observe: 'response', params})
      .pipe(
        map(response => {
          this.cache.set(Object.values(this.oParams).join('-'), response.body.data);
          this.pagination = response.body;
          return response.body;
        })
      )
    }

    getOrderItemsBriefDto() {
      return this.http.get<IOrderItemBriefDto[]>(this.apiUrl + 'orders/openorderitemsnotpaged');
    }

    getOrder(id: number) {
      return this.http.get<IOrder>(this.apiUrl + 'orders/byid/' + id);
    }

    getJD(orderitemid: number) {
      return this.http.get<IJDDto>(this.apiUrl + 'orders/jd/' + orderitemid);
    }
    
    updateJD(model: any) {
      return this.http.put(this.apiUrl + 'orders/jd', model);

    }

    getRemuneration(orderitemid: number) {
      return this.http.get<IRemunerationDto>(this.apiUrl + 'orders/remuneration/' + orderitemid);
    }
    
    updateRemuneration(model: any) {
      return this.http.put(this.apiUrl + 'orders/remuneration', model);

    }

    register(model: any) {
      return this.http.post(this.apiUrl + 'orders/register', model);  // also composes email msg to customer
      }
    
    UpdateOrder(model: any) {
      return this.http.put(this.apiUrl + 'orders', model)
    }
      
    setOParams(params: orderParams) {
      this.oParams = params;
    }
    
    getOParams() {
      return this.oParams;
    }

    
    getOrderCities() {
      if (this.cities.length > 0) {
        return of(this.cities);
      }
    
      return this.http.get<IOrderCity[]>(this.apiUrl + 'orders/ordercities' ).pipe(
        map(response => {
          this.cities = response;
          return response;
        })
      )
    }

    updateOrderWithDLFwdToHROn(orderid: number, dt: Date) {
      var obj = new idAndDate();
      obj.orderId=orderid;
      obj.dateForwarded=dt;
      return this.http.put(this.apiUrl + 'orders/updatedlfwd' , obj);
    }

}
