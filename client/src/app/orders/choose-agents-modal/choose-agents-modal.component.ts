import { Component, EventEmitter, Input, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { ICustomerOfficialDto } from 'src/app/shared/models/customerOfficialDto';
import { IOrderBriefDto } from 'src/app/shared/models/orderBriefDto';

@Component({
  selector: 'app-choose-agents-modal',
  templateUrl: './choose-agents-modal.component.html',
  styleUrls: ['./choose-agents-modal.component.css']
})

//called by order-edit.component.ts
export class ChooseAgentsModalComponent implements OnInit {

  @Input() updateSelectedOfficialIds = new EventEmitter();
  //order: IOrderBriefDto;
  agents: ICustomerOfficialDto[]; // IChooseAgentDto[]; 

  constructor(public bsModalRef: BsModalRef) { }

  ngOnInit(): void {
  }

  updateAgentsSelected() {
    this.updateSelectedOfficialIds.emit(this.agents);
    this.bsModalRef.hide();
  }

}
