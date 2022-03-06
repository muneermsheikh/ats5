import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of, ReplaySubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { IIndustryType } from '../shared/models/industryType';
import { mastersParams } from '../shared/models/mastersParams';
import { IPaginationCategory, PaginationCategory } from '../shared/models/paginationCategory';
import { IPaginationQualification, PaginationQualification } from '../shared/models/paginationQualification';
import { IProfession } from '../shared/models/profession';
import { IQualification } from '../shared/models/qualification';
import { IUser } from '../shared/models/user';

@Injectable({
  providedIn: 'root'
})
export class MastersService {

  apiUrl = environment.apiUrl;
  private currentUserSource = new ReplaySubject<IUser>(1);
  currentUser$ = this.currentUserSource.asObservable();
  mParams = new mastersParams();
  paginationCategory = new PaginationCategory();
  paginationQ = new PaginationQualification();
  professions: IProfession[]=[];
  qualifications: IQualification[]=[];
  cacheCat = new Map();
  cacheQ = new Map();
  
  constructor(private http: HttpClient) { }

  getCategories(useCache: boolean) { 

    if (useCache === false) {
      this.cacheCat = new Map();
    }

    if (this.cacheCat.size > 0 && useCache === true) {
      if (this.cacheCat.has(Object.values(this.mParams).join('-'))) {
        this.paginationCategory.data = this.cacheCat.get(Object.values(this.mParams).join('-'));
        return of(this.paginationCategory);
      }
    }

    let params = new HttpParams();
    if (this.mParams.name !== "") {
      params = params.append('name', this.mParams.name);
    }
    if (this.mParams.id !== 0) {
      params = params.append('id', this.mParams.id.toString());
    }
    if (this.mParams.search) {
      params = params.append('search', this.mParams.search);
    }
    
    params = params.append('pageIndex', this.mParams.pageNumber.toString());
    params = params.append('pageSize', this.mParams.pageSize.toString());

    return this.http.get<IPaginationCategory>(this.apiUrl + 'masters/categorypages', {observe: 'response', params})
      .pipe(
        map(response => {
          this.cacheCat.set(Object.values(this.mParams).join('-'), response.body.data);
          this.paginationCategory = response.body;
          return response.body;
        })
      )
  }

  getQualifications(useCache: boolean) { 

    if (useCache === false) {
      this.cacheQ = new Map();
    }

    if (this.cacheQ.size > 0 && useCache === true) {
      if (this.cacheQ.has(Object.values(this.mParams).join('-'))) {
        this.paginationQ.data = this.cacheQ.get(Object.values(this.mParams).join('-'));
        return of(this.paginationQ);
      }
    }

    let params = new HttpParams();
    if (this.mParams.name !== "") {
      params = params.append('name', this.mParams.name);
    }
    if (this.mParams.id !== 0) {
      params = params.append('id', this.mParams.id.toString());
    }
    if (this.mParams.search) {
      params = params.append('search', this.mParams.search);
    }
    
    params = params.append('pageIndex', this.mParams.pageNumber.toString());
    params = params.append('pageSize', this.mParams.pageSize.toString());

    return this.http.get<IPaginationQualification>(this.apiUrl + 'masters/qualificationpages', {observe: 'response', params})
      .pipe(
        map(response => {
          this.cacheCat.set(Object.values(this.mParams).join('-'), response.body.data);
          this.paginationCategory = response.body;
          return response.body;
        })
      )
  }
  
  getCategory(id: number) {
    let category: IProfession;
    this.cacheCat.forEach((categories: IProfession[]) => {
      //console.log(product);
      category = categories.find(p => p.id === id);
    })

    if (category) {
      return of(category);
    }

    return this.http.get<IProfession>(this.apiUrl + 'masters/category/' + id);
  }

  getQualification(id: number) {
    let qualification: IQualification;
    this.cacheQ.forEach((qualifications: IQualification[]) => {
      //console.log(product);
      qualification = qualifications.find(p => p.id === id);
    })

    if (qualification) {
      return of(qualification);
    }

    return this.http.get<IQualification>(this.apiUrl + 'masters/qualification/' + id);
  }

  getIndustry(id:number) {
    return this.http.get<IIndustryType>(this.apiUrl + 'masters/industry')
  }

  setCurrentUser(user: IUser) {
     localStorage.setItem('token', user.token);
    this.currentUserSource.next(user);
  }

  updateCategory(id: number, name: string) {
    var prof: IProfession = {id: id, name: name};
    return this.http.put<IProfession>(this.apiUrl + 'masters/editcategory', prof);
  }
  
  updateQualification(id: number, name: string) {
    var q: IQualification = {id: id, name: name};
    return this.http.put<boolean>(this.apiUrl + 'masters/editqualification', q);
  }
  updateIndustry(id: number, name: string) {
    var ind: IIndustryType = {id: id, name: name};
    return this.http.put<boolean>(this.apiUrl + 'masters/editindustry', ind);
  }
  setParams(params: mastersParams) {
    this.mParams = params;
  }
  getParams() {
    return this.mParams;
  }
}
