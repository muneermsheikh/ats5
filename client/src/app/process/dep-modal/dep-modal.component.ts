import { Component, EventEmitter, Input, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { ICVRefDeployDto } from 'src/app/shared/models/cvRefDeployDto';
import { ICVRefDto } from 'src/app/shared/models/cvRefDto';
import { IDeploymentStatus } from 'src/app/shared/models/deployStatus';


@Component({
  selector: 'app-dep-modal',
  templateUrl: './dep-modal.component.html',
  styleUrls: ['./dep-modal.component.css']
})
export class DepModalComponent implements OnInit {

  @Input() emitObj = new EventEmitter();
  
  depStatuses: IDeploymentStatus[];
  cvreferEdit: ICVRefDto ;

  form: FormGroup;

  constructor(public bsModalRef: BsModalRef, private fb: FormBuilder) { }

  ngOnInit(): void {
    this.createForm();
    this.editForm();
  }

  updateDepTask() {

  }
  createForm() {
    this.form = this.fb.group({
      candidateName: '', applicationNo: 0, categoryRef: '', categoryName: '', 
      depItems: this.fb.array([])
    })
  }

  editForm() {
    console.log('in depmodal editform, cvrefEdit is:', this.cvreferEdit);
    this.form.patchValue({
      candidateName: this.cvreferEdit.candidateName, applicationNo: this.cvreferEdit.applicationNo,
      categoryRef: this.cvreferEdit.categoryRef, categoryName: this.cvreferEdit.categoryName
    })

    this.form.setControl('depItems', this.setDepItems(this.cvreferEdit.deployments));
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
      this.bsModalRef.hide();
    
  }
}
