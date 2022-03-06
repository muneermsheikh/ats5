import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve } from "@angular/router";
import { Observable } from "rxjs";
import { CandidateHistoryService } from "../candidate/candidate-history.service";
import { IUserHistory } from "../shared/models/userHistory";


@Injectable({
     providedIn: 'root'
 })
 export class CandidateHistoryResolver implements Resolve<IUserHistory> {
 
     constructor(private candidateHistoryService: CandidateHistoryService) {}
 
     resolve(route: ActivatedRouteSnapshot): Observable<IUserHistory> {
        return this.candidateHistoryService.getCandidateHistoryByCandidateId(+route.paramMap.get('id'));
     }
 
 }