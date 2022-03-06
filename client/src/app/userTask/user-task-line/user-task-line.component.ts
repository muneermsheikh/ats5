import { Component, Input, OnInit } from '@angular/core';
import { IApplicationTask } from 'src/app/shared/models/applicationTask';
import { IApplicationTaskInBrief } from 'src/app/shared/models/applicationTaskInBrief';

@Component({
  selector: 'app-user-task-line',
  templateUrl: './user-task-line.component.html',
  styleUrls: ['./user-task-line.component.css']
})
export class UserTaskLineComponent implements OnInit {
  @Input() task: IApplicationTaskInBrief;
  
  
  constructor() { }

  ngOnInit(): void {
  }

}
