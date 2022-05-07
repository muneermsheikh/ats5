import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import { take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { AccountService } from '../account/account.service';
import { ICandidateAssessmentWithErrorStringDto } from '../shared/dtos/candidateAssessmentWithErrorStringDto';
import { ICandidateAssessment } from '../shared/models/candidateAssessment';
import { ICandidateAssessmentAndChecklist } from '../shared/models/candidateAssessmentAndChecklist';
import { IOrderItemAssessmentQ } from '../shared/models/orderItemAssessmentQ';
import { IUser } from '../shared/models/user';

@Injectable({
  providedIn: 'root'
})
export class CvAssessService {

  apiUrl = environment.apiUrl;
  private currentUserSource = new ReplaySubject<IUser>(1);
  currentUser$ = this.currentUserSource.asObservable();
  user: IUser;
  header: HttpHeaders;
  
  

  constructor(private http: HttpClient, private accountService: AccountService
    ) {
      this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
  }

  updateCVAssessment(model: any) {
    return this.http.put(this.apiUrl + 'candidateassessment/assess', model);
  }

  getOrderItemAssessmentQs(orderitemid: number) {
    return this.http.get<IOrderItemAssessmentQ[]>(this.apiUrl + 'orderassessment/itemassessmentQ/' + orderitemid);
  }

  updateCVAssessmentHeader(model: ICandidateAssessment) {
    return this.http.put<ICandidateAssessmentWithErrorStringDto>(this.apiUrl + 'candidateassessment', model);
  }

  insertCVAssessmentHeader(requireReview: boolean, candidateid: number, orderitemid: number, dt: Date) {
    
    return this.http.post<ICandidateAssessmentWithErrorStringDto>(this.apiUrl + 'candidateassessment/assess/' 
      +  requireReview + '/' + candidateid + '/' + orderitemid, {});
  }

  getCVAssessmentObject(requireReview: boolean, candidateid: number, orderitemid: number, dt: Date) {
    return this.http.get<ICandidateAssessment>(this.apiUrl + 'candidateassessment/assessobject/' +  requireReview + '/' + candidateid + '/' + orderitemid);
  }


  insertCVAssessment(model: any) {
    return this.http.post(this.apiUrl + 'candidateassessment/assess', model);
  }

  getCVAssessment(cvid: number, orderitemid: number) {
    return this.http.get<ICandidateAssessment>(this.apiUrl + 'candidateassessment/' + orderitemid + '/' + cvid);
  }

  getCVAssessmentAndChecklist(cvid: number, orderitemid: number) {
    return this.http.get<ICandidateAssessmentAndChecklist>(this.apiUrl + 'candidateassessment/assessmentandchecklist/' + orderitemid + '/' + cvid);
  }

  deleteAssessment(assessmentid: number) {
    return this.http.delete<boolean>(this.apiUrl + 'candidateassessment/assess/' + assessmentid);
  }
}
