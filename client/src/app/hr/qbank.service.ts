import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { of, ReplaySubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { IAssessmentQBank } from '../shared/models/assessmentQBank';
import { assessmentQBankParams } from '../shared/models/assessmentQBankParams';
import { IProfession } from '../shared/models/profession';
import { IUser } from '../shared/models/user';
import { IPaginationQ, PaginationQ } from '../shared/pagination/paginationQ';

@Injectable({
  providedIn: 'root'
})
export class QbankService {

  apiUrl = environment.apiUrl;
  private currentUserSource = new ReplaySubject<IUser>(1);
  currentUser$ = this.currentUserSource.asObservable();
  qParams = new assessmentQBankParams();
  qs: IAssessmentQBank[];
  q: IAssessmentQBank;
  pagination = new PaginationQ();
  routeId: string;
  user: IUser;
  cache = new Map();

  constructor(private http: HttpClient, private toastr: ToastrService) { }

  getQs(useCache: boolean) {
    if (useCache === false) this.cache = new Map();

    if (this.cache.size > 0 && useCache === true) {
      if(this.cache.has(Object.values(this.qParams).join('-'))) {
        this.pagination.data = this.cache.get(Object.values(this.qParams).join('-'));
        return of(this.pagination);
      } 
    }

    let params = new HttpParams();
    if (this.qParams.categoryId > 0) params = params.append('categoryId', this.qParams.categoryId.toString());

    params = params.append('sort', this.qParams.sort);
    params = params.append('pageIndex', this.qParams.pageNumber.toString());
    params = params.append('pageSize', this.qParams.pageSize.toString());

    return this.http.get<IPaginationQ>(this.apiUrl + 'assessmentqbank/qbank', {observe: 'response', params})
      .pipe(
        map(response => {
          this.cache.set(Object.values(this.qParams).join('-'), response.body.data);
          this.pagination = response.body;
          return response.body;
        })
      )
  }

  setQParams(params: assessmentQBankParams) {
    this.qParams = params;
  }

  getQParams() {
    return this.qParams;
  }

  getQ(id: number) {
    var i=0;
    let q: IAssessmentQBank;
    if (id === 0 ) {
      return this.http.get<IAssessmentQBank>(this.apiUrl + 'assessmentqbank/byid/' + id);  
    }

    this.cache.forEach((qs: IAssessmentQBank[]) => {
      q = qs.find(p => p.categoryId === id);
      i++;
    })

    console.log('total attempts', i);
    if (q) {
      return of(q);
    }
    
    return this.http.get<IAssessmentQBank>(this.apiUrl + 'assessmentqbank/byid/' + id);
  }

  update(model: IAssessmentQBank) {
    return this.http.put<IAssessmentQBank>(this.apiUrl + 'assessmentqbank', model);
  }

  insert(model: IAssessmentQBank) {
    return this.http.post<IAssessmentQBank>(this.apiUrl + 'assessmentqbank', model);
  }
}
