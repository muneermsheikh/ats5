import { Component, Input, OnInit } from '@angular/core';
import { IEmployeeBrief } from 'src/app/shared/models/employeeBrief';

@Component({
  selector: 'app-employee-item',
  templateUrl: './employee-item.component.html',
  styleUrls: ['./employee-item.component.css']
})
export class EmployeeItemComponent implements OnInit {
  @Input() emp: IEmployeeBrief;

  constructor() { }

  ngOnInit(): void {
  }

}
