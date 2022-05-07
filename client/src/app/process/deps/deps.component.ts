import { Component, EventEmitter, Input, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ICVRefDeployDto } from 'src/app/shared/models/cvRefDeployDto';
import { ICVRefDto } from 'src/app/shared/models/cvRefDto';
import { IDeploymentStatus } from 'src/app/shared/models/deployStatus';

@Component({
  selector: 'app-deps',
  templateUrl: './deps.component.html',
  styleUrls: ['./deps.component.css']
})
export class DepsComponent implements OnInit {

  @Input() emitObj = new EventEmitter();
  
  //@Input() 
  depStatuses: IDeploymentStatus[];
  //@Input() 
  cvreferEdit: ICVRefDto ;

  form: FormGroup;

  constructor(private fb: FormBuilder, private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.activatedRoute.data.subscribe(data => { 
        this.cvreferEdit = data.cvreferEdit;
        this.depStatuses = data.statuses;
        console.log(this.cvreferEdit);
    })
    
    this.createForm();
    this.editForm(this.cvreferEdit);
    
  }
  updateDepTask() {

  }
  createForm() {
    this.form = this.fb.group({
      candidateName: '', applicationNo: 0, categoryRef: '', categoryName: '', 
      depItems: this.fb.array([])
    })
  }

  editForm(f: ICVRefDto) {
    console.log('in deps editform, cvrefEdit is:', f);
    this.form.patchValue({
      candidateName: f.candidateName, applicationNo: f.applicationNo,
      categoryRef: f.categoryRef, categoryName: f.categoryName
    })

    this.form.setControl('depItems', this.setDepItems(f.deployments));
  }

  setDepItems(items: ICVRefDeployDto[]): FormArray {
    const formArray = new FormArray([]);
    items.forEach(i => {
      formArray.push(this.fb.group({
        transactionDate: i.transactionDate, stageId: i.stageId, cvRefId: i.cvRefId, id: i.id
      }))
    });
    return formArray;
  }

  get depItems(): FormArray{
    return this.form.get('depItems') as FormArray;
  }

  get getNextStageId() {
    var arr = new Array(this.depItems.length).map(x => x.stageId );
    var mx = Math.max(...arr);
    return this.depStatuses.find(x => x.stageId === mx).nextStageId;
    
  }
  
  newItem(): FormGroup {
    return this.fb.group({
      transactionDate: Date(), stageId: this.getNextStageId
    });
  }

  addItem() {
    this.depItems.push(this.newItem());
  }

  removeItem(i: number) {
    this.depItems.removeAt(i);
    this.depItems.markAsDirty();
    this.depItems.markAsTouched();
  }

  onSubmit() {
      
      this.emitObj.emit(this.cvreferEdit);
      
  }
}
