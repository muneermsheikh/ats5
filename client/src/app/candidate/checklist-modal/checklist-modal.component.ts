import { Component, EventEmitter, Input, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { IChecklistHRDto } from 'src/app/shared/models/checklistHRDto';
import { OrderItemBriefDto } from 'src/app/shared/models/orderItemBriefDto';

@Component({
  selector: 'app-checklist-modal',
  templateUrl: './checklist-modal.component.html',
  styleUrls: ['./checklist-modal.component.css']
})
export class ChecklistModalComponent implements OnInit {
  @Input() updateChecklist = new EventEmitter();
  
  chklst: IChecklistHRDto;
  
  constructor(public bsModalRef: BsModalRef) { }

  ngOnInit(): void {
  }

  
  updatechecklist() {
    this.updateChecklist.emit(this.chklst);
    this.bsModalRef.hide();
  }
}
