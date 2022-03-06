import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of, ReplaySubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { IApplicationTask } from '../shared/models/applicationTask';
import { IPaginationTask, PaginationTask } from '../shared/models/paginationTask';
import { IUser } from '../shared/models/user';
import { userTaskParams } from '../shared/models/userTaskParams';

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  apiUrl = environment.apiUrl;
  private currentUserSource = new ReplaySubject<IUser>(1);
  currentUser$ = this.currentUserSource.asObservable();
  
  oParams: userTaskParams;
  pagination = new PaginationTask();
  cache = new Map();

  constructor(private http: HttpClient) { }

    
}
