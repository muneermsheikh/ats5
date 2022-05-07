import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve } from "@angular/router";
import { Observable } from "rxjs";
import { ProcessService } from "../process/process.service";
import { ICVRefDto } from "../shared/models/cvRefDto";

@Injectable({
     providedIn: 'root'
 })
 export class CVRefResolver implements Resolve<ICVRefDto> {
 
     constructor(private service: ProcessService) {}
 
     resolve(route: ActivatedRouteSnapshot): Observable<ICVRefDto> {
        //return this.service.getDeployments(+route.paramMap.get('cvrefid'));
        return this.service.getDeployments(16);
     }
 
 }