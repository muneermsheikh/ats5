import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve } from "@angular/router";
import { Observable } from "rxjs";
import { StddqsService } from "../hr/stddqs.service";
import { IAssessmentStandardQ } from "../shared/models/assessmentStandardQ";

@Injectable({
     providedIn: 'root'
 })
 export class AssessmentStddQResolver implements Resolve<IAssessmentStandardQ> {
 
     constructor(private service: StddqsService) {}
 
     resolve(route: ActivatedRouteSnapshot): Observable<IAssessmentStandardQ> {

        var routeid = route.paramMap.get('id');
        var q = this.service.getStddQ(+routeid);
        console.log('stddq resolver, q is', q);
        return q;
     }
 
 }