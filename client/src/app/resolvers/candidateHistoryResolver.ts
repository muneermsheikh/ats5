import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve } from "@angular/router";
import { Observable } from "rxjs";
import { CandidateHistoryService } from "../candidate/candidate-history.service";
import { IUserHistory } from "../shared/models/userHistory";
import { userHistoryParams } from "../shared/params/userHistoryParams";


@Injectable({
     providedIn: 'root'
 })
 export class CandidateHistoryResolver implements Resolve<IUserHistory> {
 
     constructor(private candidateHistoryService: CandidateHistoryService) {}
 
     resolve(route: ActivatedRouteSnapshot): Observable<IUserHistory> {

        var hParam = new userHistoryParams();
        var routeid = route.paramMap.get('id');
        if ( routeid!=='' && routeid !== null) {
            hParam.personType='candidate';
            hParam.personId = +routeid;
        } else {
            var prospectiveId = route.paramMap.get('prospectiveId');
            if (prospectiveId !== '') {
                hParam.personType='prospective';
                hParam.personId=+prospectiveId;
            } else {
                var officialId = route.paramMap.get('officialId');
                if(officialId !== null) {
                    hParam.personType='official';
                    hParam.personId = +officialId;
                } else {
                    return null;
                }
            }
        }

        console.log('in candidateHistoryResolver, params is:', hParam);

        return this.candidateHistoryService.getHistory(hParam);
            
        //return this.candidateHistoryService.getCandidateHistoryByHistoryId(+routeid);

        //return this.candidateHistoryService.getCandidateHistoryByCandidateId(+routeid);
     }
 
 }