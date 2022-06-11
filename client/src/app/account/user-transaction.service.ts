import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { userHistorySpecParams } from '../shared/params/userHistorySpecParams';


@Injectable({
  providedIn: 'root'
})
export class UserTransactionService {
  baseUrl = environment.apiUrl;
  historyParams= new userHistorySpecParams;

  constructor(private http: HttpClient) { }
  
  searchUserHistories() {
    
    
  }
}
