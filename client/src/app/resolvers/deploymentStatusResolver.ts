import { Injectable } from "@angular/core";
import { Resolve } from "@angular/router";
import { Observable } from "rxjs";
import { ProcessService } from "../process/process.service";
import { IDeploymentStatus } from "../shared/models/deployStatus";

@Injectable({
     providedIn: 'root'
 })
 export class DeploymentStatusResolver implements Resolve<IDeploymentStatus[]> {
 
     constructor(private processService: ProcessService) {}
 
     resolve(): Observable<IDeploymentStatus[]> {
        return this.processService.getDeployStatus();
     }
 
 }