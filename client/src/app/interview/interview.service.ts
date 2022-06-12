import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of, ReplaySubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { IInterview } from '../shared/models/hr/interview';
import { IInterviewItemDto } from '../shared/models/hr/interviewItemDto';
import { IOrder } from '../shared/models/order';
import { IUser } from '../shared/models/user';
import { IPaginationInterview, PaginationInterview } from '../shared/pagination/paginationInterview';
import { interviewParams } from '../shared/params/interviewParams';


@Injectable({
  providedIn: 'root'
})
export class InterviewService {
  apiUrl = environment.apiUrl;
  //private currentUserSource = new ReplaySubject<IUser>(1);
  //currentUser$ = this.currentUserSource.asObservable();
  params = new interviewParams();
  pagination = new PaginationInterview();
  cache = new Map();

  constructor(private http: HttpClient) { }

  getInterviews(useCache: boolean) {

    if (useCache === false) {
       this.cache = new Map();
       this.params = new interviewParams();
    }

    if (this.cache.size > 0 && useCache === true) {
      if (this.cache.has(Object.values(this.params).join('-'))) {
        this.pagination.data = this.cache.get(Object.values(this.params).join('-'));
        return of(this.pagination);
      }
    }

    let params = new HttpParams();
    if (this.params.orderNo !== 0) params = params.append('orderNo', this.params.orderNo.toString());
    if (this.params.orderId !== 0) params = params.append('orderId', this.params.orderId.toString());
    if (this.params.customerId !== 0) params = params.append('orderId', this.params.orderId.toString());
    if (this.params.customerNameLike !== '') params = params.append('customerNameLike', this.params.customerNameLike);
    if (this.params.interviewVenue !== '') params = params.append('interviewVenue', this.params.interviewVenue);
    if (this.params.search) params = params.append('search', this.params.search);
    
    params = params.append('sort', this.params.sort);
    params = params.append('pageIndex', this.params.pageNumber.toString());
    params = params.append('pageSize', this.params.pageSize.toString());
    
    return this.http.get<IPaginationInterview>(this.apiUrl + 'interview/interviews', {observe: 'response', params})
      .pipe(
        map(response => {
          this.cache.set(Object.values(this.params).join('-'), response.body.data);
          this.pagination = response.body;
          return response.body;
        })
      )
    }

  
  getInterviewById(id: number) {
    return this.http.get<IInterview>(this.apiUrl + 'interview/interviewById/' + id);
  }
  
  addInterview(model: IInterview) {
    return this.http.post<IInterview>(this.apiUrl + 'interview/addinterview', model);
  }

  updateInterview(model: IInterview) {
    return this.http.put<IInterview>(this.apiUrl + 'interview/editInterview', model);
  }

  deleteInterview(id: number) {
    return this.http.delete<boolean>(this.apiUrl + 'interview/deleteInterviewbyid/' + id);
  }

  getInterviewItemCatAndCandidates(interviewItemId: number) {
    return this.http.get<IInterviewItemDto[]>(this.apiUrl + 'interview/catandcandidates/' + interviewItemId );
  }
  
  //GetOrCreateInterview
  //if the Interview data exists in DB, returns the same
  //if it does not exist, creates an Object and returns it
  getOrCreateInterview(orderid: number) { //returns itnerview + interviewItems
    return this.http.get<IInterview>(this.apiUrl + 'interview/getorcreateinterview/' + orderid);
  }
  
  getParams(){
    return this.params;
  }

  setParams(p: interviewParams) {
    this.params = p;
  }
}
