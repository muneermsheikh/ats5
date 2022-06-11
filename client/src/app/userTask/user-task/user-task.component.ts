import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { take } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';
import { IApplicationTask } from 'src/app/shared/models/applicationTask';
import { IApplicationTaskInBrief } from 'src/app/shared/models/applicationTaskInBrief';
import { IUser } from 'src/app/shared/models/user';
import { IPaginationAppTask } from 'src/app/shared/pagination/paginationAppTask';
import { PaginationTask } from 'src/app/shared/pagination/paginationTask';
import { userTaskParams } from 'src/app/shared/params/userTaskParams';
import { UserTaskService } from '../user-task.service';


@Component({
  selector: 'app-user-task',
  templateUrl: './user-task.component.html',
  styleUrls: ['./user-task.component.css']
})

export class UserTaskComponent implements OnInit {
  routeId: string;
  user: IUser;
  tasks: IApplicationTask[];
  cache = new Map();
  
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  userTasks: IPaginationAppTask;
  oParams = new userTaskParams();
  pagination = new PaginationTask();
  totalCount: number;
  taskStatuses: ['not started', 'started', 'completed', 'canceled'];
  taskTypes: ['general', 'hr', 'admin', 'processing']
  
  isAddMode: boolean;
  loading = false;

  sortOptions = [
    {name:'By Task Date Asc', value:'taskdate'},
    {name:'By Task Date Desc', value:'taskdatedesc'},
    {name:'By Task status', value:'taskStatus'},
    {name:'By assigned to', value:'assignedTo'},
    {name:'By assigned to Desc', value:'assignedToDesc'},
    {name:'By Order No', value:'orderno'},
  ]

  constructor(private activatedRoute: ActivatedRoute, 
      private router: Router,
      private accountService: AccountService,
      private taskService: UserTaskService) { 
    this.routeId = this.activatedRoute.snapshot.params['id'];
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
  }

  ngOnInit(): void {
   this.activatedRoute.data.subscribe(data => {
      this.tasks = data.task;
    }, error => {
      console.log(error);
    })
  }

  getTasks(userCache=false) {
    this.taskService.getTasks(userCache).subscribe(response => {
      this.tasks = response.data;
      this.totalCount = response.count;
    })
  }

  onSearch() {
    const params = this.taskService.getOParams();
    params.search = this.searchTerm.nativeElement.value;
    params.pageNumber = 1;
    this.taskService.setOParams(params);
    this.getTasks();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.oParams = new userTaskParams();
    this.taskService.setOParams(this.oParams);
    this.getTasks();
  }

  
  onTaskStatusSelected(statusSelected: string) {
    const prms = this.taskService.getOParams();
    prms.taskStatus = statusSelected;
    prms.pageNumber=1;
    this.taskService.setOParams(prms);
    this.getTasks();
  }


  onPageChanged(event: any){
    const params = this.taskService.getOParams();
    if (params.pageNumber !== event) {
      params.pageNumber = event;
      this.taskService.setOParams(params);
      this.getTasks(true);
    }
  }

  setOParams(params: userTaskParams) {
    this.oParams = params;
  }
  
  getOParams() {
    return this.oParams;
  }

  
}
