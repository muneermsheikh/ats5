import { Injectable } from "@angular/core";
import { Resolve } from "@angular/router";
import { Observable } from "rxjs";
import { ProcessService } from "../process/process.service";
import { IPaginationDeploy } from "../shared/pagination/paginationDeploy";

@Injectable({
     providedIn: 'root'
 })
 export class ProcessResolver implements Resolve<IPaginationDeploy> {
 
     constructor(private processService: ProcessService) {}
 
     resolve(): Observable<IPaginationDeploy> {
        return this.processService.getProcesses(false);
     }
 
 }