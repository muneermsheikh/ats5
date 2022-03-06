import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { CommonService } from 'src/app/services/common.service';
import { IEmployeeBrief } from 'src/app/shared/models/employeeBrief';
import { employeeParams } from 'src/app/shared/models/employeeParams';
import { IEmployeePosition } from 'src/app/shared/models/empPosition';
import { EmployeeService } from './employee.service';

@Component({
  selector: 'app-employeelist',
  templateUrl: './employeelist.component.html',
  styleUrls: ['./employeelist.component.css']
})
export class EmployeelistComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  employees: IEmployeeBrief[];
  empParams = new employeeParams();
  totalCount: number;
  positions: IEmployeePosition[];
  
  sortOptions = [
    {name:'By Employee No Asc', value:'empno'},
    {name:'By Employee No Desc', value:'empnodesc'},
    {name:'By Position Asc', value:'position'},
    {name:'By Position Desc', value:'positiondesc'},
  ]

  constructor(private service: EmployeeService, private commonService: CommonService) { }

  ngOnInit(): void {
    this.service.setEmpParams(this.empParams);
    this.getEmployees(true);
    this.getPositions();
  }

  
  getEmployees(useCache=false) {
    this.service.getEmployees(useCache).subscribe(response => {
      this.employees = response.data;
      this.totalCount = response.count;
    }, error => {
      console.log(error);
    })
  }

  getPositions() {
    this.service.getEmployeePositions().subscribe(response => {
      this.positions = [{'name': 'All'}, ...response];
    })
  }

  
  onSearch() {
    const params = this.service.getEmpParams();
    params.search = this.searchTerm.nativeElement.value;
    params.pageNumber = 1;
    this.service.setEmpParams(params);
    this.getEmployees();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.empParams = new employeeParams();
    this.service.setEmpParams(this.empParams);
    this.getEmployees();
  }

  
  onPositionSelected(positionSelected: string) {
    const prms = this.service.getEmpParams();
    prms.position = positionSelected;
    prms.pageNumber=1;
    this.service.setEmpParams(prms);
    this.getEmployees();
  }


  onPageChanged(event: any){
    const params = this.service.getEmpParams();
    if (params.pageNumber !== event) {
      params.pageNumber = event;
      this.service.setEmpParams(params);
      this.getEmployees(true);
    }
  }


}
