import { Injectable } from "@angular/core";
import { Resolve } from "@angular/router";
import { Observable } from "rxjs";
import { IEmployeeIdAndKnownAs } from "../shared/models/employeeIdAndKnownAs";
import { SharedService } from "../shared/services/shared.service";

@Injectable({
     providedIn: 'root'
 })
 export class EmployeeIdsAndKnownAsResolver implements Resolve<IEmployeeIdAndKnownAs[]> {
 
     constructor(private sharedService: SharedService) {}
 
     resolve(): Observable<IEmployeeIdAndKnownAs[]> {
        return this.sharedService.getEmployeeIdAndKnownAs();
     }
 
 }