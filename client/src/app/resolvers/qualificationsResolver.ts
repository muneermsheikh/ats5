import { Injectable } from "@angular/core";
import { Resolve } from "@angular/router";
import { Observable } from "rxjs";
import { MastersService } from "../masters/masters.service";
import { IPaginationQualification } from "../shared/models/paginationQualification";

@Injectable({
     providedIn: 'root'
 })
 export class QualificationsResolver implements Resolve<IPaginationQualification> {
 
     constructor(private mastersService: MastersService) {}
 
     resolve(): Observable<IPaginationQualification> {
        return this.mastersService.getQualifications(false);
     }
 
 }