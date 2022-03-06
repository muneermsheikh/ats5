import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { IApplicationTask } from '../models/applicationTask';

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  CreateTask(t: IApplicationTask) {
    return this.http.post<IApplicationTask>(this.apiUrl + 'tasks', {t});
  }
}
