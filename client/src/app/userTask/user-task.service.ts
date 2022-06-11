import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { of, ReplaySubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { ApplicationTask, IApplicationTask } from '../shared/models/applicationTask';
import { IApplicationTaskInBrief } from '../shared/models/applicationTaskInBrief';
import { IOrderAssignmentDto } from '../shared/models/orderAssignmentDto';
import { ITaskItem } from '../shared/models/taskItem';
import { IUser } from '../shared/models/user';
import { IPaginationAppTask, PaginationAppTask } from '../shared/pagination/paginationAppTask';
import { userTaskParams } from '../shared/params/userTaskParams';


@Injectable({
  providedIn: 'root'
})
export class UserTaskService {

  apiUrl = environment.apiUrl;
  private currentUserSource = new ReplaySubject<IUser>(1);
  currentUser$ = this.currentUserSource.asObservable();
  
  oParams: userTaskParams;
  pagination = new PaginationAppTask();
  tasks: IApplicationTask[];
  cache = new Map();
  cacheTasks = new Map();

    constructor(private http: HttpClient, private toastr:ToastrService) { }

    createOrderAssignmentTasks(assignmentTasks: IOrderAssignmentDto[]) {
      return this.http.post(this.apiUrl + 'orderassignment/orderitems', assignmentTasks);
    }

    createTaskFromAppTask(task: IApplicationTask)
    {
      return this.http.post<ApplicationTask>(this.apiUrl + 'task/getorcreate', task);
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

    getTasks(useCache: boolean, woPagination: boolean=false): any {     //returns IPaginationAppTask

      if (useCache === false)  this.cache = new Map();

      if (this.cache.size > 0 && useCache === true) {
        if (this.cache.has(Object.values(this.oParams).join('-'))) {
          this.pagination.data = this.cache.get(Object.values(this.oParams).join('-'));
          return of(this.pagination);
        }
      }
      
      let params = new HttpParams();
      if (this.oParams.taskStatus !== "" && this.oParams.taskStatus !== undefined ) params = params.append('taskStatus', this.oParams.taskStatus); 
      if (this.oParams.orderId !== 0 && this.oParams.orderId !== undefined) params = params.append('orderId', this.oParams.orderId.toString()); 
      if (this.oParams.assignedToId !== 0 && this.oParams.assignedToId !== undefined) params = params.append('assignedToId', this.oParams.assignedToId?.toString()); 
      if (this.oParams.assignedToNameLike !== '' && this.oParams.assignedToNameLike !== undefined ) params = params.append('assignedToNameLike', this.oParams.assignedToNameLike); 
      if (new Date(this.oParams.taskDate).getFullYear() > 2000) params = params.append('taskDate', this.oParams.taskDate.toString()); 
      if(this.oParams.candidateId !==0 && this.oParams.candidateId !== undefined) {
        params = params.append('candidateId', this.oParams.candidateId.toString());
        params = params.append('personType', 'candidate');
      }
      
      if (this.oParams.search) {
        params = params.append('search', this.oParams.search);
      }
      
      params = params.append('sort', this.oParams.sort);
      params = params.append('pageIndex', this.oParams.pageNumber.toString());
      params = params.append('pageSize', this.oParams.pageSize.toString());

      if(woPagination) {
        //return this.http.get<IApplicationTask[]>(this.apiUrl + 'task/wopagination', {observe: 'response', params})
        return this.http.get<IApplicationTask[]>(this.apiUrl + 'task/wopagination', {params})
        .pipe(
          map(response => {
            this.cache.set(Object.values(this.oParams).join('-'), response);
            this.tasks = response;
            return response;
          })
        )  
      } else {
        return this.http.get<IPaginationAppTask>(this.apiUrl + 'task', {observe: 'response', params})
        .pipe(
          map(response => {
            this.cache.set(Object.values(this.oParams).join('-'), response.body.data);
            this.pagination = response.body;
            return response.body;
          })
        )
      }
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
