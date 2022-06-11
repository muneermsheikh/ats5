import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve } from "@angular/router";
import { Observable } from "rxjs";
import { CandidateHistoryService } from "../candidate/candidate-history.service";
import { ProspectiveService } from "../prospectives/prospective.service";
import { IProspectiveCandidate } from "../shared/models/prospectiveCandidate";
import { IUserHistory } from "../shared/models/userHistory";
import { IPaginationProspectiveCandidates } from "../shared/pagination/paginationProspectiveCandidates";
import { prospectiveCandidateParams } from "../shared/params/prospectiveCandidateParams";


@Injectable({
     providedIn: 'root'
 })
 export class ProspectiveCandidatesOfACategoryPendingResolver implements Resolve<IPaginationProspectiveCandidates> {
 
     constructor(private prospectiveService: ProspectiveService) {}
 
     resolve(route: ActivatedRouteSnapshot): Observable<IPaginationProspectiveCandidates> {

        var routeid = route.paramMap.get('categoryRef');
        if (routeid === '') return null;
        var pParams = new prospectiveCandidateParams();
        pParams.categoryRef=routeid;
        pParams.categoryRef=routeid;
        pParams.status = 'pending';
        this.prospectiveService.setParams(pParams);
        return this.prospectiveService.getProspectiveCandidates(false);
     }
 
 }