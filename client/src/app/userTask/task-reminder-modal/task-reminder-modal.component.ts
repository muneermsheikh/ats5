import { Component, EventEmitter, Input, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { IApplicationTask } from 'src/app/shared/models/applicationTask';
import { IEmployeeIdAndKnownAs } from 'src/app/shared/models/employeeIdAndKnownAs';



@Component({
  selector: 'app-task-reminder-modal',
  templateUrl: './task-reminder-modal.component.html',
  styleUrls: ['./task-reminder-modal.component.css']
})
export class TaskReminderModalComponent implements OnInit {
  @Input() updatedRemidner = new EventEmitter();
  obj: IApplicationTask;
  emps: IEmployeeIdAndKnownAs[];
  title: string;
  
  constructor(public bsModalRef: BsModalRef, private toastr: ToastrService) { }

  ngOnInit(): void {
  }

  emitObject() {
    if (this.obj.assignedToId==0) {
      this.toastr.warning('task not assigned');
      return;
    }
    if(new Date(this.obj.completeBy).getFullYear() < 2000) {
      this.toastr.warning('complete by date should be provided');
      return;
    }
    if(new Date(this.obj.taskDate).getFullYear() < 2000) {
      this.toastr.warning('task date not provided');
      return;
    }
    if(this.obj.taskDescription==='') {
      this.toastr.warning('task description cannot be blank');
      return;
    }
    this.updatedRemidner.emit(this.obj);
    this.bsModalRef.hide();
  }
}
