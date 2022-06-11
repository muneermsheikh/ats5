import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IUser } from '../shared/models/user';
import { PaginationTask } from '../shared/pagination/paginationTask';
import { userTaskParams } from '../shared/params/userTaskParams';

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
