import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve } from "@angular/router";
import { Observable } from "rxjs";
import { AssessmentService } from "../hr/assessment.service";
import { IAssessment } from "../shared/models/assessment";

@Injectable({
     providedIn: 'root'
 })
 export class AssessmentQsResolver implements Resolve<IAssessment> {
 
     constructor(private service: AssessmentService) {}
 
     resolve(route: ActivatedRouteSnapshot): Observable<IAssessment> {
        return this.service.getOrderItemAssessment(+route.paramMap.get('id'));
     }
 
 }