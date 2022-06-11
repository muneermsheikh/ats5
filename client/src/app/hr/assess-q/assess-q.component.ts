import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { currentBsVersion } from 'ngx-bootstrap/utils';
import { ToastrService } from 'ngx-toastr';
import { IAssessment } from 'src/app/shared/models/assessment';
import { IAssessmentQ } from 'src/app/shared/models/assessmentQ';
import { IOrderItemBriefDto } from 'src/app/shared/models/orderItemBriefDto';
import { AssessmentService } from '../assessment.service';
import { StddqsService } from '../stddqs.service';

@Component({
  selector: 'app-assess-q',
  templateUrl: './assess-q.component.html',
  styleUrls: ['./assess-q.component.css']
})
export class AssessQComponent implements OnInit {
  //@Input() q: IAssessment;
  //@Output() assessQsUpdated = new EventEmitter<IAssessment>();
  
  orderitem: IOrderItemBriefDto;
  assess: IAssessment;

  form: FormGroup;
  
  constructor(private activatedRoute: ActivatedRoute, private stddqservice: StddqsService,
    private service: AssessmentService, private toastr: ToastrService,
    private fb: FormBuilder) { }

  ngOnInit(): void {
    this.activatedRoute.data.subscribe(data => { 
      this.orderitem = data.itembrief;
      this.assess= data.assessment;
      //console.log('ngONINit', this.assess);
      this.createForm();
      if (this.assess) {
        this.patchForm(this.assess);
      } 
    })
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
        question: i.question, maxMarks: i.maxMarks, isMandatory: i.isMandatory
      }))
    });
    return formArray;
  }

  addStddQ() {
    console.log('assess', this.assess);
    this.stddqservice.getStddQs(true).subscribe(response => {
      const stddqs = response;
      if (stddqs===null) {
        this.toastr.warning('failed to retrieve standard questions');
        return;
      }

      stddqs.forEach(q => {
        this.orderItemAssessmentQs.push(this.fb.group({
          id: q.id, assessmentId: this.assess.id, orderId: this.assess.orderId,
          questionNo: q.qNo, subject: q.assessmentParameter, question: q.question, maxMarks: q.maxPoints,
          isMandatory: false
        }))
      })
    }, error => {
      this.toastr.error('error - failed to retrieve standard questions');
    })
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

  update() {
    this.service.updateAssessment(this.form.value).subscribe(response => {
      if (response) {
        this.toastr.success('updated the Assessment Question');
        
      } else {
        this.toastr.warning('failed to update the Assessment Question');
      }
    }, error => {
      this.toastr.error('error updating the assessment question', error);
    })
  }

  calcualteTotals() {
    //const subtotal = basket.items.reduce((a, b) => (b.price * b.quantity) + a, 0);
    return this.assess.orderItemAssessmentQs.map(x => x.maxMarks).reduce((a, b) => a + b);
  }
 /*
  onPageChanged(event: any) {
    this.assessQsUpdated.emit(this.q);
  }
 */
 
    /*
    editAssessment(assessment: IAssessment) {
      return this.service.updateAssessment(assessment).subscribe(response => {
        if (response) {
          this.toastr.success('The Order Assessment was updated');
        } else {
          this.toastr.warning('failed to update the Order Assessment');
        }
      }, error => {
        this.toastr.error('failed to update the assessment', error);
      })
    }

    editAssessmentQ(assessmentQ: IAssessmentQ) {
      return this.service.updateAssessmentQ(assessmentQ).subscribe(response => {
        if (response) {
          this.toastr.success('TheAssessment Question was updated');
        } else {
          this.toastr.warning('failed to update the Assessment Question');
        }
      }, error => {
        this.toastr.error('failed to update the assessment Question', error);
      })
    }
    
    deleteAssessment(orderitemid: number) {
      return this.service.deleteAssessment(orderitemid).subscribe(response => {
        if (response) {
          this.toastr.success('The Order Item Assessment was deleted');
        } else {
          this.toastr.warning('failed to delete the Order Item Assessment');
        }
      }, error => {
        this.toastr.error('failed to delete the Order Item assessment', error);
      })
    }
    

    deleteAssessmentQ(assessmentQId: number) {
      return this.service.deleteAssessmentQ(assessmentQId).subscribe(response => {
        if (response) {
          this.toastr.success('The Order Item Assessment Question was deleted');
        } else {
          this.toastr.warning('failed to delete the Order Item Assessment Question');
        }
      }, error => {
        this.toastr.error('failed to delete the Order Item assessment Question', error);
      })
    }
    
  }
  function Ouptut() {
    throw new Error('Function not implemented.');
  }
  */

}
