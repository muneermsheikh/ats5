import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { IEmploymentDto } from '../shared/dtos/employmentDto';

@Injectable({
  providedIn: 'root'
})
export class EmploymentService {
  apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getEmploymentsFromOrderNo(orderno: number) {
    return this.http.get<IEmploymentDto[]>(this.apiUrl + 'employment/employmentsbyorderno/' + orderno);
  }

  getEmploymentsFromDates(fromdate: Date, uptodate: Date) {
    return this.http.get<IEmploymentDto[]>(this.apiUrl + 'employment/employmentsbydates/' + fromdate + '/' + uptodate);
  }

  getEmploymentsFromCVRefId(cvrefid: number) {
    return this.http.get<IEmploymentDto[]>(this.apiUrl + 'employment/employmentsbycvref/' + cvrefid);
  }
}
