import { Component, EventEmitter, Input, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { IEmploymentDto } from 'src/app/shared/dtos/employmentDto';
import { IEmployment } from 'src/app/shared/models/selectionDecision';

@Component({
  selector: 'app-selection-modal',
  templateUrl: './selection-modal.component.html',
  styleUrls: ['./selection-modal.component.css']
})
export class SelectionModalComponent implements OnInit {

  @Input() updateEmployment = new EventEmitter();
  
  emp: IEmployment;
  title: string;
  
  errors: string[];

  constructor(public bsModalRef: BsModalRef, private toastr: ToastrService) { }

  ngOnInit(): void {
    console.log('entered ngoninit', this.emp);
  }

  emitEmployment() {
    if (this.checkedNoError()) {
      this.updateEmployment.emit(this.emp);
      this.bsModalRef.hide();
    }
  }

  checkedNoError() : boolean {
    if(this.emp.salaryCurrency === '') this.errors.push('salary currency not entered');
    if(this.emp.salary === 0 ) this.errors.push('salary not mentioned');
    if(this.emp.contractPeriodInMonths === 0) this.errors.push('Contract Period not mentioned');
    if(this.emp.leavePerYearInDays===0) this.errors.push('annual leave not mentioned');
    if(this.emp.leaveAirfareEntitlementAfterMonths===0) this.errors.push('entitlement after months not mentioned');

    return this.errors.length ===0;
  }

}
