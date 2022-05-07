import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ReplaySubject } from 'rxjs';
import { take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { AccountService } from '../account/account.service';
import { IMessagesDto } from '../shared/dtos/messagesDto';
import { ICandidateAssessedDto } from '../shared/models/candidateAssessedDto';
import { IMessage } from '../shared/models/message';
import { IUser } from '../shared/models/user';
import { paginationAssessedCVs } from '../shared/pagination/paginationAssessedCVs';


@Injectable({
  providedIn: 'root'
})
export class CvRefService {
  apiUrl = environment.apiUrl;
  user: IUser;
  private currentUserSource = new ReplaySubject<IUser>(1);
  currentUser$ = this.currentUserSource.asObservable();
  pagination = new paginationAssessedCVs();

  constructor(private activatedRoute: ActivatedRoute, 
      private accountService: AccountService,
      private http: HttpClient) {
        this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
  }

  getCVsAssessedAndApproved() {
    return this.http.get<ICandidateAssessedDto[]>(this.apiUrl + 'candidateassessment/assessedandapproved');
  }

  referCVs(cvassessmentids: number[]) {
    return this.http.post<IMessagesDto>(this.apiUrl + 'CVRef', cvassessmentids);
  }
  
}
