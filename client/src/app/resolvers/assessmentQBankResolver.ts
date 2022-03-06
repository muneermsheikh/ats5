import { Injectable } from "@angular/core";
import { Resolve } from "@angular/router";
import { Observable } from "rxjs";
import { HrService } from "../hr/hr.service";
import { IAssessmentQBank } from "../shared/models/assessmentQBank";

@Injectable({
     providedIn: 'root'
 })
 export class AssessmentQBankResolver implements Resolve<IAssessmentQBank[]> {
 
     constructor(private qBankService: HrService) {}
 
     resolve(): Observable<IAssessmentQBank[]> {
         console.log('in assessmentQBankResolver');
        return this.qBankService.getAssessmentQBank();
     }
 
 }