import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ReplaySubject } from 'rxjs';
import { take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { AccountService } from '../account/account.service';
import { IAssessment } from '../shared/models/assessment';
import { IAssessmentQ } from '../shared/models/assessmentQ';
import { IUser } from '../shared/models/user';
import { assessmentParams } from '../shared/params/assessmentParam';

@Injectable({
  providedIn: 'root'
})
export class AssessmentService {
  apiUrl = environment.apiUrl;
  private currentUserSource = new ReplaySubject<IUser>(1);
  currentUser$ = this.currentUserSource.asObservable();
  qParams = new assessmentParams();
  routeId: string;
  user: IUser;
  
  constructor(private activatedRoute: ActivatedRoute, 
    private router: Router,
    private accountService:AccountService,
    private http: HttpClient) { 
      this.accountService.currentUser$.pipe(take(1))
        .subscribe(user => this.user = user);
    }

    getOrderItemAssessment(orderitemid: number) {
      return this.http.get<IAssessment>(this.apiUrl + 'orderassessment/itemassessment/' + orderitemid);
    }
    
    getOrderAssessment(orderid: number) {
      return this.http.get<IAssessment>(this.apiUrl + 'orderassessment/orderassessment/' + orderid);
    }
    updateAssessment(assessment: IAssessment) {
      return this.http.put<boolean>(this.apiUrl + 'orderassessment/editassessment', assessment);
    }
    updateAssessmentQs(assessmentQs: IAssessmentQ[]) {
      return this.http.put<boolean>(this.apiUrl + 'orderassessment/updateassessmentqs', assessmentQs);
    }

    updateAssessmentQ(assessmentQ: IAssessmentQ) {
          return this.http.put<boolean>(this.apiUrl + 'orderassessment/edititemassessment', assessmentQ);
    }

    deleteAssessmentQ(questionId: number) {
      return this.http.delete<boolean>(this.apiUrl + 'orderassessment/assessmentq/' + questionId);
    }

    deleteAssessment(orderitemid: number) {
      return this.http.delete<boolean>(this.apiUrl + 'orderassessment/assessment/' + orderitemid);
    }

    AddNewAssessment(assessment: IAssessment) {
      
    }

}
