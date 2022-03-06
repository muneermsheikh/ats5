import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { IProfession } from '../shared/models/profession';
import { IQualification } from '../shared/models/qualification';

@Injectable({
  providedIn: 'root'
})
export class CommonService {

  apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getProfessions() {
    return this.http.get<IProfession[]>(this.apiUrl + 'categories');
  }

  getQualifications() {
    return this.http.get<IQualification[]>(this.apiUrl + 'masters/qlist');
  }

  
  
}
