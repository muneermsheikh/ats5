import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { ICustomerReview } from '../shared/models/customerReview';
import { ICustomerReviewData } from '../shared/models/customerReviewData';


@Injectable({
  providedIn: 'root'
})
export class ClientReviewService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  getCustomerReview(id: number){
    return this.http.get<ICustomerReview>(this.baseUrl + 'customerreview/' + id);
  }

  updateCustomerReview(model: any) {
    return this.http.put(this.baseUrl + 'CustomerReview', model)
  }

  getCustomerReviewStatusData() {
    return this.http.get<ICustomerReviewData[]>(this.baseUrl + 'customerreview/customerReviewData');
  }
}
