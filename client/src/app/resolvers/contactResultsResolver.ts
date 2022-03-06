import { Injectable } from "@angular/core";
import { Resolve } from "@angular/router";
import { Observable } from "rxjs";
import { CandidateHistoryService } from "../candidate/candidate-history.service";
import { IContactResult } from "../shared/models/contactResult";

@Injectable({
     providedIn: 'root'
 })
 export class ContactResultsResolver implements Resolve<IContactResult[]> {
 
     constructor(private candidateHistoryService: CandidateHistoryService) {}
 
     resolve(): Observable<IContactResult[]> {
        return this.candidateHistoryService.getContactResults();
     }
 
 }