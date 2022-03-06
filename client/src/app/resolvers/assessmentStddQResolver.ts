import { Injectable } from "@angular/core";
import { Resolve } from "@angular/router";
import { Observable } from "rxjs";
import { HrService } from "../hr/hr.service";
import { IAssessmentQBank } from "../shared/models/assessmentQBank";
import { IAssessmentStandardQ } from "../shared/models/assessmentStandardQ";

@Injectable({
     providedIn: 'root'
 })
 export class AssessmentStddQResolver implements Resolve<IAssessmentStandardQ[]> {
 
     constructor(private qBankService: HrService) {}
 
     resolve(): Observable<IAssessmentStandardQ[]> {
        return this.qBankService.getStddQs();
     }
 
 }