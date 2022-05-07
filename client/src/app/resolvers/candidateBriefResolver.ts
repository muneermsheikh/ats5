import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve } from "@angular/router";
import { Observable } from "rxjs";
import { CandidateService } from "../candidate/candidate.service";
import { ICandidateBriefDto } from "../shared/models/candidateBriefDto";


@Injectable({
     providedIn: 'root'
 })
 export class CandidateBriefResolver implements Resolve<ICandidateBriefDto> {
 
     constructor(private candidateService: CandidateService) {}
 
     resolve(route: ActivatedRouteSnapshot): Observable<ICandidateBriefDto> {
        var routeid = route.paramMap.get('id');
        if (routeid === '0') return null;
        return this.candidateService.getCandidateBrief(+routeid);
     }
 
 }