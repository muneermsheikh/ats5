import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { candidateHistoryParams } from '../shared/models/candidateHistoryParams';
import { IContactResult } from '../shared/models/contactResult';
import { PaginationCandidateHistory } from '../shared/models/paginationCandidateHistory';
import { IUser } from '../shared/models/user';
import { IUserHistory } from '../shared/models/userHistory';
import { IUserHistoryDto } from '../shared/models/userHistoryDto';
import { IUserHistorySearch } from '../shared/models/userHistorySearch';
import { userHistorySpecParams } from '../shared/models/userHistorySpecParams';

@Injectable({
  providedIn: 'root'
})
export class CandidateHistoryService {
  apiUrl = environment.apiUrl;
  private currentUserSource = new ReplaySubject<IUser>(1);
  currentUser$ = this.currentUserSource.asObservable();
  cvParams = new candidateHistoryParams();
  userParams = new userHistorySpecParams();

  pagination = new PaginationCandidateHistory();

  constructor(private http: HttpClient) { }

  getCandidateHistory(id: number) {
    return this.http.get<IUserHistory>(this.apiUrl + 'UserHistory/bycandidateid/'+id);
  }


  getUserHistories(model: IUserHistorySearch) {
    var specParams: any = new userHistorySpecParams();
    specParams.aadharNo = model.aadharNo;
    specParams.applicationNo = model.applicationNo;
    specParams.emailId = model.emailId;
    specParams.customerOfficialId = model.customerOfficialId;
    specParams.createNewIfNull=false;
    return this.http.get<IUserHistoryDto[]>(this.apiUrl + 'UserHistory/historyDto', specParams);
  }

  getCandidateHistoryByHistoryId(id: number) {
    return this.http.get<IUserHistory>(this.apiUrl + 'UserHistory/bycandidateid/' + id);
  }

  getCandidateHistoryByCandidateId(id: number) {
    return this.http.get<IUserHistory>(this.apiUrl + 'UserHistory/bycandidateid/' + id);
  }

  getUserHistoriesByPhoneNo(phoneno: string) {
    return this.http.get<IUserHistoryDto[]>(this.apiUrl + 'UserHistory/fromphoneno/' + phoneno);
  }

  getUserHistoriesByOfficialId(officialid: number) {
    return this.http.get<IUserHistoryDto[]>(this.apiUrl + 'UserHistory/fromofficialid/' + officialid);
  }

  getUserHistoriesByAadharNo(aadharno: string) {
    return this.http.get<IUserHistoryDto[]>(this.apiUrl + 'UserHistory/fromaadharno/' + aadharno);
  }

  getUserHistoriesByEmail(email: string) {
    return this.http.get<IUserHistoryDto[]>(this.apiUrl + 'UserHistory/fromemail/' + email);
  }

  getUserHistoriesByCandidateId(candidateid: number) {
    return this.http.get<IUserHistoryDto[]>(this.apiUrl + 'UserHistory/fromcandidateid/' + candidateid);
  }
  
  getUserHistoriesByApplicationNo(applicationno: number) {
    return this.http.get<IUserHistoryDto[]>(this.apiUrl + 'UserHistory/fromapplicationno/' + applicationno);
  }
  
  getContactResults() {
    return this.http.get<IContactResult[]>(this.apiUrl + 'UserHistory/contactresults')
  }

  updateCandidateHistory(model: any) {
    //console.log('updatecandidatehistory in service', model);
    return this.http.put(this.apiUrl + 'UserHistory', model);
  }

  setUserParams(params: userHistorySpecParams) {
      this.userParams = params;
    }
    
  getUserParams() {
      return this.userParams;
    }

}
