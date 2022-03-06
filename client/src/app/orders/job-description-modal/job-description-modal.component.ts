import { Component, EventEmitter, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { IJobDescription } from 'src/app/shared/models/jobDescription';

import { OrderService } from '../order.service';

@Component({
  selector: 'app-job-description-modal',
  templateUrl: './job-description-modal.component.html',
  styleUrls: ['./job-description-modal.component.css']
})
export class JobDescriptionModalComponent implements OnInit {

  @Input() updateSelectedJD = new EventEmitter();
  jds: any;

  title: string;
    customerName: string; 
    orderNo: number;
    orderDate: Date;
    id: number;
    orderItemId: number;
    categoryName: string;
    jobDescInBrief: string;
    qualificationDesired: string;
    expDesiredMin: number;
    expDesiredMax: number;
    minAge: number;
    maxAge: number;
  

  closeBtnName: string;

  form: FormGroup;
    
  jd: IJobDescription;

  constructor(private service: OrderService, public bsModalRef: BsModalRef, private fb: FormBuilder ) {
   }

  ngOnInit(): void {
    //this.createForm();
    //this.form.patchValue(this.bsModalRef.content);
  }
/*
  createForm() {
      this.form = this.fb.group({
        id: [null],  orderNo: 0, orderDate: '', 
        orderItemId: 0,
        customerName: '',  categoryName: '', jobDescInBrief: '',
        qualificationDesired: '',
        expDesiredMin: 0, expDesiredMax: 0,
        minAge: 0, maxAge: 0
      } 
      );
    }
  */

  confirm() {
    this.jds= ({
      'jobDescInBrief': this.jobDescInBrief, 
      'orderNo' : this.orderNo, 'id': this.id,  'orderDate': this.orderDate, 'orderItemId' : this.orderItemId, 'categoryName': this.categoryName,
      'qualificationDesired' : this.qualificationDesired, 'expDesiredMin' : this.expDesiredMin, 'expDesiredMax' : this.expDesiredMax,
      'minAge' : this.minAge, 'maxAge': this.maxAge
    })
    this.updateSelectedJD.emit(this.jds);

    this.bsModalRef.hide();
  }

  decline() {
    this.bsModalRef.hide();
  }

}
