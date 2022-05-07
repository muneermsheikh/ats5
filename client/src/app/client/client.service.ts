import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { ICustomer } from '../shared/models/customer';
import { ICustomerCity } from '../shared/models/customerCity';
import { ICustomerOfficialDto } from '../shared/models/customerOfficialDto';
import { customerParams } from '../shared/models/customerParams';
import { IPagination } from '../shared/pagination/pagination';


@Injectable({
  providedIn: 'root'
})
export class ClientService {
  baseUrl = environment.apiUrl;
  customerType: string = 'customer';

  constructor(private http: HttpClient) { }

  getCustomers(custParams: customerParams) {
    let params = new HttpParams();
    if (custParams.customerCityName !== "") {
      params = params.append('customerCityName', custParams.customerCityName);
    }
    if (custParams.customerIndustryId !== 0) {
      params = params.append('customerIndustryId', custParams.customerIndustryId.toString());
    }

    if (custParams.search) {
      params = params.append('search', custParams.search);
    }

    this.customerType = custParams.custType ?? "customer";
    params = params.append('customerType', this.customerType);

    
    params = params.append('sort', custParams.sort);
    
    params = params.append('pageIndex', custParams.pageNumber.toString());
    params = params.append('pageSize', custParams.pageSize.toString());
    
    return this.http.get<IPagination>(this.baseUrl + 'customers', {observe: 'response', params})
      .pipe(
        map(response => {
          return response.body;
        })
      )
  }

  getCustomer(id: number){
    return this.http.get<ICustomer>(this.baseUrl + 'customers/byid/' + id);
  }

  getCustomerCities() {
    return this.http.get<ICustomerCity[]>(this.baseUrl + 'customers/customerCities/' + this.customerType);
  }

  createCustomer(model: any) {
    return this.http.post(this.baseUrl + 'customers', model);
  }

  updateCustomer(model: any) {
    return this.http.put(this.baseUrl + 'customers', model);
  }

  //associates
  getAgents() {
    return this.http.get<ICustomerOfficialDto[]>(this.baseUrl + 'customers/agentdetails');
  }

}
