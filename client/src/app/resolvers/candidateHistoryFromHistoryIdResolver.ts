import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve } from "@angular/router";
import { Observable } from "rxjs";
import { CandidateHistoryService } from "../candidate/candidate-history.service";
import { IUserHistory } from "../shared/models/userHistory";


@Injectable({
     providedIn: 'root'
 })
 export class CandidateHistoryFromHistoryIdResolver implements Resolve<IUserHistory> {
 
     constructor(private candidateHistoryService: CandidateHistoryService) {}
 
     resolve(route: ActivatedRouteSnapshot): Observable<IUserHistory> {
         return this.candidateHistoryService.getCandidateHistoryByHistoryId(+route.paramMap.get('id'));
     }
 
 }