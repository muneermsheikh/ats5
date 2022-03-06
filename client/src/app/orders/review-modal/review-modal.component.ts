import { Component, EventEmitter, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { IContractReviewItem } from 'src/app/shared/models/contractReviewItem';

@Component({
  selector: 'app-review-modal',
  templateUrl: './review-modal.component.html',
  styleUrls: ['./review-modal.component.css']
})
export class ReviewModalComponent implements OnInit {
  @Input() updateModalReview = new EventEmitter();
  review: IContractReviewItem;
  reviewStatus: {}
  form: FormGroup;  
  
  /*
  categoryName: string;
  customerName: string;
  orderNo: number;
  orderDate: Date;
  srNo: number;
  reviewParameter: string;
  response: boolean;
  isMandatoryTrue: boolean;
*/
  constructor(public bsModalRef: BsModalRef, private fb: FormBuilder ) { }

  ngOnInit(): void {
  }

  confirm() {
    this.updateModalReview.emit(this.review);
    this.bsModalRef.hide();
  }

  decline() {
    this.bsModalRef.hide();
  }

}
