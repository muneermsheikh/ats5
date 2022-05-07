import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve } from "@angular/router";
import { Observable } from "rxjs";
import { IApplicationTaskInBrief } from "../shared/models/applicationTaskInBrief";
import { UserTaskService } from "../userTask/user-task.service";

@Injectable({
     providedIn: 'root'
 })
 export class UserTaskResolver implements Resolve<IApplicationTaskInBrief[]> {
 
     constructor(private taskService: UserTaskService) {}
 
     resolve(route: ActivatedRouteSnapshot): Observable<IApplicationTaskInBrief[]> {
         console.log('entered usertaskresolver');
         return this.taskService.getPendingTasksOfUser
         (
              +route.paramMap.get('taskOwnerId'),
              +route.paramMap.get('pageNo'), 
              +route.paramMap.get('pageSize')
          );
     }
 
 }