import { Component, EventEmitter, Input, OnInit } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { ApplicationTask, IApplicationTask } from 'src/app/shared/models/applicationTask';
import { IEmployeeIdAndKnownAs } from 'src/app/shared/models/employeeIdAndKnownAs';
import { IUser } from 'src/app/shared/models/user';
import { TaskReminderModalComponent } from '../task-reminder-modal/task-reminder-modal.component';
import { UserTaskService } from '../user-task.service';

@Component({
  selector: 'app-tasks-modal',
  templateUrl: './tasks-modal.component.html',
  styleUrls: ['./tasks-modal.component.css']
})
export class TasksModalComponent implements OnInit {

  @Input() updatedRemidner = new EventEmitter();
  objs: IApplicationTask[];
  emps: IEmployeeIdAndKnownAs[];
  title: string;
  user: IUser;
  
  constructor(public bsModalRef: BsModalRef, private toastr: ToastrService, 
    public modalService: BsModalService, private userTaskService: UserTaskService) { }

  ngOnInit(): void {
    
  }

  createNewTask() {
    
  }

  
  async displayModalReminder(event: any) {

    var historyItemId = event.controls['id'].value;

    var obj = await this.createNewTaskObject(historyItemId);
    
      const config = {
        title: 'create task for ' ,
        class: 'modal-dialog-centered modal-lg',
        initialState: {
          obj, 
          emps: this.emps
        }
      }
      console.log('config for displaymodalremidner', config);
      this.bsModalRef = this.modalService.show(TaskReminderModalComponent, config);
      var returnedTask: IApplicationTask;
      this.bsModalRef.content.updatedRemidner.subscribe(values => {
          returnedTask = values;
          if(returnedTask ===undefined || returnedTask === null ) {
            this.toastr.warning('task creation canceled');
            return;
          }
          this.userTaskService.createTaskFromAppTask(returnedTask).subscribe(response => {
            if (response) this.toastr.success('task created');
            }, error => {
              this.toastr.warning('failed to create the task');
              console.log(error);
            })
      }, error => {
        this.toastr.warning('failed to create the task', error);
      })

  }


    async createNewTaskObject(historyItemId: number): Promise<IApplicationTask> {
      var task: IApplicationTask = new ApplicationTask();
    
      const dt= new Date().toISOString();
      const dt1 = new Date(dt);
      var loggedinEmployeeId = this.user.loggedInEmployeeId;     //why is this.user.loggedInEmployeeId undefined?
      if(loggedinEmployeeId === 0 || loggedinEmployeeId === undefined) {
        return null;
      }

      task.id=0;
      task.taskTypeId = 20, //obj.taskTypeName ='none',
      task.taskDate = new Date(); //obj.assignedToId=,  //obj.assignedToName='', 
      task.completeBy=new Date(dt1.setDate(dt1.getDate()+ 7));
      task.taskStatus='not started';
      task.taskDescription='reminder for ';
      //obj.taskOwnerName=this.user.displayName;
      task.taskOwnerId=loggedinEmployeeId; 
      task.historyItemId=historyItemId;

      return task;

    };


}
