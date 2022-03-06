import { Component, EventEmitter, Input, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { IChecklistHRDto } from 'src/app/shared/models/checklistHRDto';
import { IChecklistHRItem } from 'src/app/shared/models/checklistHRItem';
import { ChecklistService } from '../checklist.service';

@Component({
  selector: 'app-checklist-modal-array',
  templateUrl: './checklist-modal-array.component.html',
  styleUrls: ['./checklist-modal-array.component.css']
})
export class ChecklistModalArrayComponent implements OnInit {
  @Input() updateChecklist = new EventEmitter();
  
  chklst: IChecklistHRDto;

  form: FormGroup;
  //items: IChecklistHRItem[]; 
  
  constructor(public bsModalRef: BsModalRef, private fb: FormBuilder) { }

  ngOnInit(): void {
    console.log('received in modal-array.component.ts, chklist', this.chklst.checklistHRItems);
    //console.log('in ngOnInit, items is: ', this.items);  // = this.chklst.checklistHRItems;
    this.createForm();

  }

  createForm() {
    this.form = this.fb.group({
      id: 0, candidateId: 0, applicationNo: 0, candidateName: '',
      categoryRef: '', orderRef: '', orderItemId: 0, userLoggedIn: '',
      checkedOn: '', hrExecComments: '', checklistHRItems: this.fb.array([])
    })

    this.loadMember();
    //this.patchForm(this.chklst);
  }

  patchForm(p: IChecklistHRDto) {
    this.form.patchValue({
      id: p.id, candidateId: p.candidateId, applicationNo: p.applicationNo, 
      candidateName: p.candidateName, categoryRef: p.categoryRef, 
      orderRef: p.orderRef, orderItemId: p.orderItemId, 
      userLoggedIn: p.userLoggedIn, checkedOn: p.checkedOn, 
      hrExecComments: p.hrExecComments
    });
    
    //console.log(p.checklistHRItems);
    //if (p.checklistHRItems !== undefined ) 
      this.form.setControl('checklistHRItems', this.setExistingItems(p.checklistHRItems));
  }

  loadMember() {
    this.form.patchValue(this.chklst);
    if(this.chklst.checklistHRItems !==null && this.chklst.checklistHRItems !== undefined  ) {
      for(const i of this.chklst.checklistHRItems) {
        this.checklistHRItems.push(new FormControl(i));
      }
    }
  }
  
  setExistingItems(items: IChecklistHRItem[]): FormArray {
      console.log('in setExistingItems, items is: ', items);
      const formArray = new FormArray([]);
      items.forEach(i => {
        formArray.push(this.fb.group({
          id: i.id, checklistHRId: i.checklistHRId, srNo: i.srNo, 
          parameter: i.parameter, response: i.response, 
          mandatoryTrue: i.mandatoryTrue, exceptions: i.exceptions
        }))
      });
      return formArray;
  } 

  get checklistHRItems(): FormArray {
    return this.form.get("checklistHRItems") as FormArray;
  }

  newChecklistHRItem(): FormGroup{
    return this.fb.group({
      id: 0, checklistHRId: 0, srNo: 0, parameter: '', response: false, 
      mandatoryTrue: false, exceptions: ''
    })
  }

  addChecklistItem(){
    this.checklistHRItems.push(this.newChecklistHRItem());
  }

  removeChecklistItem(i: number) {
    this.checklistHRItems.removeAt(i);
    this.checklistHRItems.markAsDirty();
    this.checklistHRItems.markAsTouched();
  }

  updatechecklist() {
    this.updateChecklist.emit(this.chklst);
    this.bsModalRef.hide();
  }

}
