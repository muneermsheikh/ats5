import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of, ReplaySubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { IProspectiveRegisterToAddDto } from '../shared/dtos/prospectiveRegisterToAddDto';
import { IUser } from '../shared/models/user';
import { IPaginationProspectiveCandidates, PaginationProspectiveCandidates } from '../shared/pagination/paginationProspectiveCandidates';
import { prospectiveCandidateParams } from '../shared/params/prospectiveCandidateParams';

@Injectable({
  providedIn: 'root'
})
export class ProspectiveService {

  apiUrl = environment.apiUrl;
  private currentUserSource = new ReplaySubject<IUser>(1);
  currentUser$ = this.currentUserSource.asObservable();
  oParams = new prospectiveCandidateParams();
  pagination = new PaginationProspectiveCandidates();
  cache = new Map();
  
  constructor(private http: HttpClient) { }

  getProspectiveCandidates(useCache: boolean) { 

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
    if (this.oParams.id !== 0 && this.oParams.id !== undefined)  params = params.append('id', this.oParams.id.toString());
    if (this.oParams.categoryRef !== ''  && this.oParams.categoryRef !== undefined)  params = params.append('categoryRef', this.oParams.categoryRef);
    if (this.oParams.candidateNameLike !== ''  && this.oParams.candidateNameLike !== undefined)  params = params.append('candidateNameLike', this.oParams.candidateNameLike);
    if ((this.oParams.dateAdded?.getFullYear() < 2000 &&  this.oParams.dateAdded !== undefined) )  params = params.append('dateAdded', this.oParams.dateAdded.toDateString());
    if(this.oParams.status !== '') params = params.append('status', this.oParams.status);
    
    if (this.oParams.search) params = params.append('search', this.oParams.search);
    
    params = params.append('sort', this.oParams.sort);
    params = params.append('pageIndex', this.oParams.pageNumber.toString());
    params = params.append('pageSize', this.oParams.pageSize.toString());

    return this.http.get<IPaginationProspectiveCandidates>(this.apiUrl + 'prospectivecandidates', {observe: 'response', params})
      .pipe(
        map(response => {
          this.cache.set(Object.values(this.oParams).join('-'), response.body.data);
          this.pagination = response.body;
          return response.body;
        })
      )
    }
  
  setParams(params: prospectiveCandidateParams) {
    this.oParams = params;
  }
  
  getParams() {
    return this.oParams;
  }

  displayModalUserContact() {
    
  }

  createCandidateFromprospective(model: IProspectiveRegisterToAddDto) {
    console.log('reaching api with model', model);
    return this.http.post(this.apiUrl + 'prospectivecandidates', model )
  }

}
