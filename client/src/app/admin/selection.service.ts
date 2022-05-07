import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { of, ReplaySubject } from 'rxjs';
import { map, take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { AccountService } from '../account/account.service';
import { selDecisionToAddDto } from '../shared/dtos/selDecisionToAddDto';
import { ISelectionMsgsAndEmploymentsDto } from '../shared/dtos/selectionMsgsAndEmploymentsDto';
import { IMessage } from '../shared/models/message';
import { IEmployment, ISelectionDecision } from '../shared/models/selectionDecision';
import { ISelectionStatus } from '../shared/models/selectionStatus';
import { IUser } from '../shared/models/user';
import { IPaginationSelPending, paginationSelPending } from '../shared/pagination/paginationSelPending';
import { SelDecisionSpecParams } from '../shared/params/selDecisionSpecParams';

@Injectable({
  providedIn: 'root'
})
export class SelectionService {

  apiUrl = environment.apiUrl;
  user: IUser;
  private currentUserSource = new ReplaySubject<IUser>(1);
  currentUser$ = this.currentUserSource.asObservable();
  sParams = new SelDecisionSpecParams();

  pagination = new paginationSelPending();

  cache = new Map();

  constructor(private activatedRoute: ActivatedRoute,
      private http: HttpClient, 
      private accountService: AccountService) {
        this.accountService.currentUser$.pipe(take(1)).subscribe(user => {
          this.user = user;
        })
  }

  setParams(params: SelDecisionSpecParams) {
    this.sParams = params;
   }

   getParams(): SelDecisionSpecParams {
     return this.sParams;
   }

  private getHttpParams(): HttpParams {
    
    let params = new HttpParams();
    if (this.sParams.orderItemId !== 0) 
      params = params.append('orderItemId', this.sParams.orderItemId.toString());
    
    if (this.sParams.categoryId !== 0) 
      params = params.append('categoryId', this.sParams.categoryId.toString());
    if (this.sParams.categoryName !== '') 
      params = params.append('categoryName', this.sParams.categoryName);
    if (this.sParams.orderId !== 0) 
      params = params.append('orderId', this.sParams.orderId.toString());
    if (this.sParams.orderNo !== 0) 
      params = params.append('orderNo', this.sParams.orderNo.toString());
    if (this.sParams.candidateId !== 0) 
      params = params.append('candidateId', this.sParams.candidateId.toString());
    if (this.sParams.applicationNo !== 0) 
      params = params.append('applicationNo', this.sParams.applicationNo.toString());
    if (this.sParams.cVRefId !== 0) 
      params = params.append('cVRefId', this.sParams.cVRefId.toString());
    if (this.sParams.includeEmploymentData === true) 
      params = params.append('includeEmploymentData', "true");
    if (this.sParams.search) 
      params = params.append('search', this.sParams.search);
    
    params = params.append('sort', this.sParams.sort);
    params = params.append('pageIndex', this.sParams.pageIndex.toString());
    params = params.append('pageSize', this.sParams.pageSize.toString());
    
    return params;
  }

  getPendingSelections(useCache: boolean) {

    if (useCache === false) this.cache = new Map();

    if (this.cache.size > 0 && useCache) {
      if (this.cache.has(Object.values(this.sParams).join('-'))) {
        this.pagination.data = this.cache.get(Object.values(this.sParams).join('-'));
        return of(this.pagination);
      }
    }
    
    var params = this.getHttpParams();

    return this.http.get<IPaginationSelPending>(this.apiUrl + 'selectionDecision/pendingseldecisions', {observe: 'response', params})
    .pipe(
      map(response => {
        this.cache.set(Object.values(this.sParams).join('-'), response.body.data);
        this.pagination = response.body;
        return response.body;
      })
    )

  }


  registerSelectionDecisions(selDecisions: selDecisionToAddDto[]) {
  
   
    return this.http.post<ISelectionMsgsAndEmploymentsDto>(this.apiUrl + 'selectiondecision', selDecisions);

  }

  getSelectionDecisions(selDecision: SelDecisionSpecParams)
  {
       var params=this.getHttpParams();

      return this.http.get<ISelectionDecision[]>(this.apiUrl + 'seldecision', {observe: 'response', params})
  }

  editSelectionDecision(seldecision: ISelectionDecision) {
    return this.http.put<boolean>(this.apiUrl + 'selectiondecision', seldecision);
  }

  deleteSelectionDecision(id: number) {
    
    return this.http.delete<boolean>(this.apiUrl + 'selectiondecision/' + id);
  }

  getSelectionStatus() {
    return this.http.get<ISelectionStatus[]>(this.apiUrl + 'selectiondecision/selectionstatus');
  }

  getEmployment(cvrefid: number) {
    return this.http.get<IEmployment>(this.apiUrl + 'selectiondecision/employment/' + cvrefid);
  }

  getEmploymentFromSelectionId(id: number) {
    return this.http.get<IEmployment>(this.apiUrl + 'selectiondecision/employmentfromSelId/' + id);
    
  }

  updateEmployment(emp: IEmployment) {
    return this.http.put<boolean>(this.apiUrl + 'selectiondecision/employment', emp);
  }

  getSelectionDtoByOrderNo(orderno: number) {
    
  }
}