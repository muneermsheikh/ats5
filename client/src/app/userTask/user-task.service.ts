import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { of, ReplaySubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { IApplicationTask } from '../shared/models/applicationTask';
import { IApplicationTaskInBrief } from '../shared/models/applicationTaskInBrief';
import { IOrderAssignmentDto } from '../shared/models/orderAssignmentDto';
import { ITaskItem } from '../shared/models/taskItem';
import { IUser } from '../shared/models/user';
import { IPaginationTask, PaginationTask } from '../shared/pagination/paginationTask';
import { userTaskParams } from '../shared/params/userTaskParams';


@Injectable({
  providedIn: 'root'
})
export class UserTaskService {

  apiUrl = environment.apiUrl;
  private currentUserSource = new ReplaySubject<IUser>(1);
  currentUser$ = this.currentUserSource.asObservable();
  
  oParams: userTaskParams;
  pagination = new PaginationTask();
  cache = new Map();

    constructor(private http: HttpClient, private toastr:ToastrService) { }

    createOrderAssignmentTasks(assignmentTasks: IOrderAssignmentDto[]) {
      return this.http.post(this.apiUrl + 'orderassignment/orderitems', assignmentTasks);
    }

    createTaskFromAppTask(task: IApplicationTask)
    {
      this.toastr.info('entered createTAskFromAppTask');
      return this.http.post(this.apiUrl + 'task', task);
    }

    getPendingTasksOfUser(userid: number, pageindex: number, pagesize: number) {
      return this.http.get<IApplicationTaskInBrief[]>(this.apiUrl + 'task/pendingtasksofauser/' + userid + '/' + pageindex + '/' + pagesize);
    }

    setOParams(params: userTaskParams) {
      this.oParams = params;
    }
    
    getOParams() {
      return this.oParams;
    }

    getTasks(useCache: boolean) { 

      if (useCache === false) {
        this.cache = new Map();
      }

      if (this.cache.size > 0 && useCache === true) {
        if (this.cache.has(Object.values(this.oParams).join('-'))) {
          this.pagination.data = this.cache.get(Object.values(this.oParams).join('-'));
          return of(this.pagination);
        }
      }

      let params = new HttpParams();
      if (this.oParams.taskStatus !== "") {params = params.append('taskStatus', this.oParams.taskStatus); }
      if (this.oParams.orderId !== 0) {params = params.append('orderId', this.oParams.orderId.toString()); }
      if (this.oParams.assignedToId !== 0) {params = params.append('assignedToId', this.oParams.assignedToId.toString()); }
      if (this.oParams.assignedToNameLike !== '') {params = params.append('assignedToNameLike', this.oParams.assignedToNameLike); }
      if (this.oParams.taskDate.getFullYear() > 2000) {params = params.append('taskDate', this.oParams.taskDate.toString()); }

      if (this.oParams.search) {
        params = params.append('search', this.oParams.search);
      }
      
      params = params.append('sort', this.oParams.sort);
      params = params.append('pageIndex', this.oParams.pageNumber.toString());
      params = params.append('pageSize', this.oParams.pageSize.toString());

      return this.http.get<IPaginationTask>(this.apiUrl + 'task/tasksbriefpaginated', {observe: 'response', params})
        .pipe(
          map(response => {
            this.cache.set(Object.values(this.oParams).join('-'), response.body.data);
            this.pagination = response.body;
            return response.body;
          })
        )
    }
    
    getTask(id: number) {
      return this.http.get<IApplicationTask>(this.apiUrl + 'task/byid/' + id);
    }

    getTaskItem(taskitemid: number) {
      return this.http.get<ITaskItem>(this.apiUrl + 'task/taskitem/' + taskitemid);
    }

    register(model: any) {
      return this.http.post(this.apiUrl + 'task/register', model);  // also composes email msg to customer
      }
    
    UpdateTask(model: any) {
      return this.http.put(this.apiUrl + 'task', model)
    }

    CreateNewTask(model: any) {
      return this.http.post(this.apiUrl + 'task', model)
    }

}
