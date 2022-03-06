import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { IPaginationCandidate } from '../shared/models/paginationCandidate';
import { IUserHistoryDto } from '../shared/models/userHistoryDto';
import { IUserHistorySearch } from '../shared/models/userHistorySearch';
import { userHistorySpecParams } from '../shared/models/userHistorySpecParams';

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
