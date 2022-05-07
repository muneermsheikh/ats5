import { animate, state, style, transition, trigger } from "@angular/animations";
import { Component, OnInit } from "@angular/core";
const USER_DATA = 
[
  {"id": 1, "isExpanded":false, "name": "John Smith", "occupation": "Advisor", "dateOfBirth": "1984-05-05", "age": 36,
    "subjects":[
        {"id": 1, "name":"Bio", "type":"Medical", "grade":"A" },
        {"id": 2, "name":"Chemistry", "type":"Medical", "grade":"A"},
        {"id": 3, "name":"Physics", "type":"Medical", "grade":"A" }
    ]},
  {"id": 2, "isExpanded":false, "name": "Muhi Masri", "occupation": "Developer", "dateOfBirth": "1992-02-02", "age": 28,
    "subjects":[
    { "id": 4, "name":"Bio","type":"Medical", "grade":"A"},
    { "id": 5, "name":"Chemistry", "type":"Medical", "grade":"A" },
    { "id": 6, "name":"Physics", "type":"Medical", "grade":"A"}
 ]},
  {"id": 3, "isExpanded":false, "name": "Peter Adams", "occupation": "HR", "dateOfBirth": "2000-01-01", "age": 20, 
  "subjects":[
    { "id": 7, "name":"Maths","type":"Engg", "grade":"A"},
    { "id": 8, "name":"Algebra", "type":"Engg", "grade":"A" },
    { "id": 9, "name":"Geometry", "type":"Engg", "grade":"A"}
 ]},
  {"id": 4, "isExpanded":false, "name": "Lora Bay", "occupation": "Marketing", "dateOfBirth": "1977-03-03", "age": 43,
  "subjects":[
    { "id": 10, "name":"Hindi","type":"Language", "grade":"A"},
    { "id": 11, "name":"English", "type":"Language", "grade":"A"},
    { "id": 12, "name":"Marathi", "type":"Language", "grade":"A"}
 ]},
]

const COLUMNS_SCHEMA = [
  { key: 'isSelected', type: 'isSelected', label: ''},
  { key: "name", type: "text", label: "Full Name"},
  { key: "occupation",  type: "text", label: "Occupation" },
  { key: "dateOfBirth", type: "date", label: "Date of Birth"},
  { key: "age", type: "number", label: "Age" },
  { key: "isEdit", type: "isEdit", label: ""}
]

@Component({
  selector: 'app-projects',
  templateUrl: './projects.component.html',
  styleUrls: ['./projects.component.css'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({ height: '0px', minHeight: '0' })),
      state('expanded', style({ height: '*' })),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ],
})
export class ProjectsComponent implements OnInit {
  displayedColumns: string[] = COLUMNS_SCHEMA.map((col) => col.key);;
  dataSource: any = USER_DATA;
  columnsSchema: any = COLUMNS_SCHEMA;

  constructor(
    /* private service: EmploymentService, private ordersService: OrderService, private toastr: ToastrService, private activatedRoute: ActivatedRoute
    */
    ) { }

  ngOnInit(): void {
/*
    this.activatedRoute.data.subscribe(data => { 
      this.dataSource.data = data.orderbrief.data;
      console.log('projects.componentts. ngoninit', this.dataSource.data);
    })
    */
  }

  addRow() {
    const newRow = {
      id: Date.now(), "name": "", "occupation": "", "dateOfBirth": "", "age": 0, isEdit: true
    }
    this.dataSource = [newRow, ...this.dataSource];
  }

  removeRow(id: number) {
    this.dataSource = this.dataSource.filter((u) => u.id !== id);
  }
  
  removeSelectedRows() {
    this.dataSource = this.dataSource.filter((u: any) => !u.isSelected);
  }

  /*
  toggleTableRows() {
    this.isTableExpanded = !this.isTableExpanded;

    this.dataSource.data.forEach((row: any) => {
      row.isExpanded = this.isTableExpanded;
    })
  }
*/
  

}
