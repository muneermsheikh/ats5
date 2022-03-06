import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { IChecklistHRDto } from 'src/app/shared/models/checklistHRDto';
import { IChecklistHRItem } from 'src/app/shared/models/checklistHRItem';

@Component({
  selector: 'app-checklist-plain',
  templateUrl: './checklist-plain.component.html',
  styleUrls: ['./checklist-plain.component.css']
})
export class ChecklistPlainComponent implements OnInit {
  @Input() checklist: IChecklistHRDto;
  @Output() msgEvent = new EventEmitter<IChecklistHRDto>();
  form: FormGroup;

  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
  }

  createForm() {
    this.form = this.fb.group({
      id: 0, candidateId: 0, applicationNo: 0, candidateName: '',
      categoryRef: '', orderRef: '', orderItemId: 0, userLoggedIn: '',
      checkedOn: '', hrExecComments: '', checklistHRItems: this.fb.array([])
    })

    this.loadMember();
    //this.patchForm(this.checklist);
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
    this.form.patchValue(this.checklist);
    if(this.checklist.checklistHRItems !==null && this.checklist.checklistHRItems !== undefined  ) {
      for(const i of this.checklist.checklistHRItems) {
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
  }

  
}
