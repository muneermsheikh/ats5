import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { IAssessment } from 'src/app/shared/models/assessment';
import { IAssessmentQ } from 'src/app/shared/models/assessmentQ';
import { AssessmentService } from '../assessment.service';

@Component({
  selector: 'app-item-q',
  templateUrl: './item-q.component.html',
  styleUrls: ['./item-q.component.css']
})
export class ItemQComponent implements OnInit {
  q: IAssessment;
  form: FormGroup;
  
  constructor(private activatedRoute: ActivatedRoute,
    private service: AssessmentService, private toastr: ToastrService,
    private fb: FormBuilder) { }

  ngOnInit(): void {
    
  }

  createForm() {
    this.form = this.fb.group({
      id: 0, orderAssessmentId: 0, orderItemId: 0,
      orderId: 0, orderNo: 0, categoryId: 0, categoryName: '',
      orderItemAssessmentQs: this.fb.array([])
     })
  }

  patchForm(p:IAssessment) {
    this.form.patchValue({
      id: p.id, orderAssessmentId: p.orderAssessmentId, 
      orderItemId: p.orderItemId, orderId: p.orderId, 
      orderNo: p.orderNo, categoryId: p.categoryId, 
      categoryName: p.categoryName
    })
    this.form.setControl('orderItemAssessmentQs', this.setExistingItems(p.orderItemAssessmentQs));
  }

  setExistingItems(items: IAssessmentQ[]): FormArray {
    const formArray = new FormArray([]);
    items.forEach(i => {
      formArray.push(this.fb.group({
        id: i.id, assessmentId: i.assessmentId, orderItemId: i.orderItemId,
        orderId: i.orderId, questionNo: i.questionNo, subject: i.subject,
        maxMarks: i.maxMarks, isMandatory: i.isMandatory
      }))
    });
    return formArray;
  }

  get orderItemAssessmentQs(): FormArray {
    return this.form.get('orderItemAssessmentQs') as FormArray;
  }

  newOrderItemAssessmentQ(): FormGroup{
    return this.fb.group({
      id: 0, assessmentId: 0, orderItemId: 0,
      orderId: 0, questionNo: 0, subject: '',
      maxMarks: 0, isMandatory: false
    })
  }

  addOrderItemAssessmentQ(){
    this.orderItemAssessmentQs.push(this.newOrderItemAssessmentQ());
  }

  removeOrderItemAssessmentQ(i: number) {
    this.orderItemAssessmentQs.removeAt(i);
    this.orderItemAssessmentQs.markAsDirty();
    this.orderItemAssessmentQs.markAsTouched();
  }


}
