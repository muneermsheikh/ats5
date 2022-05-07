import { Component, EventEmitter, Input, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { IChecklistHRDto } from 'src/app/shared/models/checklistHRDto';

@Component({
  selector: 'app-checklist-modal',
  templateUrl: './checklist-modal.component.html',
  styleUrls: ['./checklist-modal.component.css']
})
export class ChecklistModalComponent implements OnInit {
  @Input() updateChecklist = new EventEmitter();
  
  chklst: IChecklistHRDto;
  
  constructor(public bsModalRef: BsModalRef, private toastr:ToastrService) { }

  ngOnInit(): void {

  }

  
  updatechecklist() {
    if (this.checkednoerror) {
      this.updateChecklist.emit(this.chklst);
      this.bsModalRef.hide();
    }
  }

  checkednoerror() {
    if(this.chklst.exceptionApproved && (this.chklst.exceptionApprovedBy==='' || this.chklst.exceptionApprovedOn.getFullYear() < 2000)) {
      this.toastr.warning('exception approved must accompany exception made by and approved on');
      return false;
    }
    var nonMatching = this.chklst.checklistHRItems.filter(x => x.mandatoryTrue && !x.response).map(x => x.parameter).join(' ');
    if (nonMatching!=='') {
      this.toastr.warning('error - flg parameters have not been checklisted :', nonMatching );
      return false;
    } 
    return true;
  }
}
