import { Component, EventEmitter, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { OrderService } from '../order.service';

@Component({
  selector: 'app-remuneration-modal',
  templateUrl: './remuneration-modal.component.html',
  styleUrls: ['./remuneration-modal.component.css']
})
export class RemunerationModalComponent implements OnInit {

  @Input() updateSelectedRemuneration = new EventEmitter();
  remun: any;

  customerName: string; categoryName: string;
  id: number; orderItemId: number; orderId: number; orderNo: number; orderDate: Date; categoryId: number; workHours: number;
  salaryCurrency: string; salaryMin: number; salaryMax: number; contractPeriodInMonths: number;
  housingProvidedFree: boolean; housingAllowance: number; housingNotProvided: boolean;
  foodProvidedFree: boolean; foodAllowance: number; foodNotProvided: boolean;
  transportProvidedFree: boolean; transportAllowance: number; transportNotProvided: boolean;
  otherAllowance: number; leavePerYearInDays: number; leaveAirfareEntitlementAfterMonths: number;


  closeBtnName: string;

  form: FormGroup;
  
  constructor(private service: OrderService, public bsModalRef: BsModalRef, private fb: FormBuilder ) {
   }

  ngOnInit(): void {
    
  }

 
  confirm() {
    this.remun= ({
      'customerName': this.customerName, 
      'categoryName': this.categoryName, 'id': this.id, 
      'orderItemId': this.orderItemId,
      'orderId': this.orderId, 
      'orderNo': this.orderNo, 
      'orderDate': this.orderDate, 
      'categoryId': this.categoryId,
      'workHours': this.workHours, 
      'salaryCurrency': this.salaryCurrency, 
      'salaryMin': this.salaryMin, 
      'salaryMax': this.salaryMax, 
      'contractPeriodInMonths': this.contractPeriodInMonths,
      'housingProvidedFree': this.housingProvidedFree, 
      'housingAllowance': this.housingAllowance, 
      'housingNotProvided': this.housingNotProvided, 
      'foodProvidedFree': this.foodProvidedFree, 
      'foodAllowance': this.foodAllowance, 
      'foodNotProvided': this.foodNotProvided, 
      'transportProvidedFree': this.transportProvidedFree, 
      'transportAllowance': this.transportAllowance, 
      'transportNotProvided': this.transportNotProvided, 
      'otherAllowance': this.otherAllowance, 
      'leavePerYearInDays': this.leavePerYearInDays, leaveAirfareEntitlementAfterMonths: this.leaveAirfareEntitlementAfterMonths
    })
    console.log('before emit, ', this.remun);
    this.updateSelectedRemuneration.emit(this.remun);
    
    this.bsModalRef.hide();
  }

  decline() {
    this.bsModalRef.hide();
  }

}
