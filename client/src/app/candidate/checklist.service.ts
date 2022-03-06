import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IChecklistHRDto } from '../shared/models/checklistHRDto';
import { IUser } from '../shared/models/user';

@Injectable({
  providedIn: 'root'
})
export class ChecklistService {
  apiUrl = environment.apiUrl;
  private currentUserSource = new ReplaySubject<IUser>(1);
  currentUser$ = this.currentUserSource.asObservable();
  cl: IChecklistHRDto;
  
  constructor(private http: HttpClient) { }

  getChecklist(candidateid: number, orderitemid: number) {
    return this.http.get<IChecklistHRDto>(this.apiUrl + 'checklist/checklisthr/' + candidateid  + '/' + orderitemid) ;
  }

  updateChecklist(checklist: IChecklistHRDto) {
    return this.http.put(this.apiUrl + 'checklist', checklist);
  }

}
